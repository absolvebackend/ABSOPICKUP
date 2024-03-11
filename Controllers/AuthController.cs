using _AbsoPickUp.Common;
using _AbsoPickUp.IServices;
using _AbsoPickUp.LoggerService;
using _AbsoPickUp.Models;
using _AbsoPickUp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static _AbsoPickUp.Common.GlobalVariables;

namespace _AbsoPickUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<UserDetails> _userManager;
        private readonly IAuthService _authService;
        private readonly ITwilioManager _twilioManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILoggerManager _logger;
        public AuthController(ILoggerManager logger, UserManager<UserDetails> userManager, IWebHostEnvironment hostingEnvironment, IAuthService authService, ITwilioManager twilioManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _authService = authService;
            _twilioManager = twilioManager;
            _emailSender = emailSender;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }


        #region Register Individual User
        [HttpPost]
        [Route("RegisterIndividualUser")]
        public async Task<IActionResult> RegisterIndividualUser([FromForm] RegisterIndividualViewModel model)
        {
            bool IdProofUploaded = false;
            bool IdCardBackUploaded = false;
            int _res = 0;
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInfo("API: Auth/RegisterIndividualUser : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
                }

                UserDetails _userDetails = new UserDetails
                {
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DialCode = model.DialCode,
                    Email = model.Email,
                    DeviceType = model.DeviceType,
                    DeviceToken = model.DeviceToken,
                    UserTypeId = model.UserTypeId,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = model.FirstName,
                    PhoneNumber = model.PhoneNumber,
                    DOB = model.DOB,
                    ApplicationStatus = (int)ApplicationStatus.Incomplete
                };

                var result = await _userManager.CreateAsync(_userDetails, model.Password);
                if (result.Succeeded)
                {
                    //Generate Access Token
                    string accessToken = CommonFunctions.GenerateAccessToken(_userDetails.Id, model.DeviceToken);
                    // document path 
                    if (model.IDProof != null)
                    {
                        var IDProofDocument = ContentDispositionHeaderValue.Parse(model.IDProof.ContentDisposition).FileName.Trim('"');
                        IDProofDocument = CommonFunctions.EnsureCorrectFilename(IDProofDocument);
                        IDProofDocument = CommonFunctions.RenameFileName(IDProofDocument);
                        using (FileStream fs = System.IO.File.Create(GetPathAndFilename(IDProofDocument, IDProofDocContainer)))
                        {
                            model.IDProof.CopyTo(fs);
                            fs.Flush();
                        }
                        string IDProofDocumentPath = IDProofDocContainer + IDProofDocument;
                        var IndividualUserIDProof = new IndividualUserDocuments()
                        {
                            IndividualUserId = _userDetails.Id,
                            DocTypeId = model.IDProofTypeId,
                            DocImgPath = IDProofDocumentPath
                        };
                        IdProofUploaded = _authService.UploadUserDocuments(IndividualUserIDProof);
                        //Set Application Status.
                        if (!(model.IDProofTypeId == (int)DocTypes.IDCard) && IdProofUploaded)
                        {
                            int _status = await _authService.SetUserApplicationStatus(_userDetails.Id, (int)ApplicationStatus.AwaitingVerification);
                        }
                        else if (IdProofUploaded)
                        {
                            int _status = await _authService.SetUserApplicationStatus(_userDetails.Id, (int)ApplicationStatus.Incomplete);
                        }
                    }
                    if (model.IDProofTypeId == (int)DocTypes.IDCard && model.IDCardBack != null && IdProofUploaded)
                    {
                        var IDCardBackPicDoc = ContentDispositionHeaderValue.Parse(model.IDCardBack.ContentDisposition).FileName.Trim('"');
                        IDCardBackPicDoc = CommonFunctions.EnsureCorrectFilename(IDCardBackPicDoc);
                        IDCardBackPicDoc = CommonFunctions.RenameFileName(IDCardBackPicDoc);
                        using (FileStream fs = System.IO.File.Create(GetPathAndFilename(IDCardBackPicDoc, IDProofDocContainer)))
                        {
                            model.IDCardBack.CopyTo(fs);
                            fs.Flush();
                        }
                        string IDCardBackPicDocumentPath = IDProofDocContainer + IDCardBackPicDoc;
                        var IndividualUserIDCardBack = new IndividualUserDocuments()
                        {
                            IndividualUserId = _userDetails.Id,
                            DocTypeId = (int)DocTypes.IDCardBack,
                            DocImgPath = IDCardBackPicDocumentPath
                        };
                        IdCardBackUploaded = _authService.UploadUserDocuments(IndividualUserIDCardBack);
                        if (IdCardBackUploaded)
                        {
                            int _status = await _authService.SetUserApplicationStatus(_userDetails.Id, (int)ApplicationStatus.AwaitingVerification);
                        }
                    }
                    var response = await _authService.GetUserRegisterResponse(_userDetails.Id, accessToken);
                    if (_userDetails.Id != null && _userDetails.Id != string.Empty)
                    {
                        _res = 1;
                        if (IdProofUploaded)
                        {
                            _res = 2;
                            if (IdCardBackUploaded)
                            {
                                _res = 3;
                            }
                        }
                    }
                    if ((model.IDProofTypeId == (int)DocTypes.IDCard && _res == 3) || _res == 2)
                    {
                        return Ok(new { status = true, data = response, message = ResponseMessages.msgRegister, code = StatusCodes.Status200OK });
                    }
                    return Ok(new { status = false, message = "Image Not Provided / Uploaded. Please try again.", code = StatusCodes.Status204NoContent });
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = result.Errors.First().Description, code = StatusCodes.Status204NoContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Auth/RegisterIndividualUser : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Register Business User
        [HttpPost("RegisterBusinessUser")]
        public async Task<IActionResult> RegisterBusinessUser([FromForm] RegisterBusinessViewModel model)
        {
            int _status;
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInfo("API: Auth/RegisterBusinessUser : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
                }

                UserDetails _userDetails = new UserDetails
                {
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DialCode = model.DialCode,
                    Email = model.Email,
                    DeviceType = model.DeviceType,
                    DeviceToken = model.DeviceToken,
                    UserTypeId = model.UserTypeId,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = model.FirstName,
                    PhoneNumber = model.PhoneNumber,
                    DOB = string.Empty,
                    ApplicationStatus = (int)ApplicationStatus.Incomplete
                };
                var result = await _userManager.CreateAsync(_userDetails, model.Password);
                if (result.Succeeded)
                {
                    string accessToken = CommonFunctions.GenerateAccessToken(_userDetails.Id, model.DeviceToken);
                    //document paths
                    var documentLicenceFile = ContentDispositionHeaderValue.Parse(model.LicenceFilePath.ContentDisposition).FileName.Trim('"');
                    documentLicenceFile = CommonFunctions.EnsureCorrectFilename(documentLicenceFile);
                    documentLicenceFile = CommonFunctions.RenameFileName(documentLicenceFile);
                    using (FileStream fs = System.IO.File.Create(GetPathAndFilename(documentLicenceFile, LicenceFileContainer)))
                    {
                        model.LicenceFilePath.CopyTo(fs);
                        fs.Flush();
                    }
                    string documentLicencePath = LicenceFileContainer + documentLicenceFile;

                    var documentVATFile = ContentDispositionHeaderValue.Parse(model.VATFilePath.ContentDisposition).FileName.Trim('"');
                    documentVATFile = CommonFunctions.EnsureCorrectFilename(documentVATFile);
                    documentVATFile = CommonFunctions.RenameFileName(documentVATFile);
                    using (FileStream fs = System.IO.File.Create(GetPathAndFilename(documentVATFile, VATFileContainer)))
                    {
                        model.VATFilePath.CopyTo(fs);
                        fs.Flush();
                    }
                    string documentVATPath = VATFileContainer + documentVATFile;

                    var documentChamberFile = ContentDispositionHeaderValue.Parse(model.ChamberCommerceFilePath.ContentDisposition).FileName.Trim('"');
                    documentChamberFile = CommonFunctions.EnsureCorrectFilename(documentChamberFile);
                    documentChamberFile = CommonFunctions.RenameFileName(documentChamberFile);
                    using (FileStream fs = System.IO.File.Create(GetPathAndFilename(documentChamberFile, ChamberCommerceFileContainer)))
                    {
                        model.ChamberCommerceFilePath.CopyTo(fs);
                        fs.Flush();
                    }
                    string documentChamberPath = ChamberCommerceFileContainer + documentChamberFile;

                    var documentAgreementFile = ContentDispositionHeaderValue.Parse(model.AgreementFilePath.ContentDisposition).FileName.Trim('"');
                    documentAgreementFile = CommonFunctions.EnsureCorrectFilename(documentAgreementFile);
                    documentVATFile = CommonFunctions.RenameFileName(documentAgreementFile);
                    using (FileStream fs = System.IO.File.Create(GetPathAndFilename(documentAgreementFile, AgreementFileContainer)))
                    {
                        model.AgreementFilePath.CopyTo(fs);
                        fs.Flush();
                    }
                    string documentAgreementPath = AgreementFileContainer + documentAgreementFile;

                    var _documents = new BusinessDocuments()
                    {
                        LicenceFilePath = documentLicencePath,
                        VATFilePath = documentVATPath,
                        ChamberCommerceFilePath = documentChamberPath,
                        AgreementFilePath = documentAgreementPath,
                    };

                    var _details = new BusinessDetails()
                    {
                        Website = model.Website,
                        ContactPerson = model.ContactPerson,
                        LicenceNumber = model.LicenceNumber,
                        VAT = model.VAT,
                        ExternalContractNumber = model.ExternalNumber
                    };

                    bool _businessDocument = _authService.BusinessUserDocuments(_userDetails.Id, _documents);
                    bool _businessDetails = _authService.BusinessUserDetails(_userDetails.Id, _details);

                    if (_businessDetails && _businessDocument)
                    {
                        if (documentLicencePath != string.Empty && documentVATPath != string.Empty && documentChamberPath != string.Empty && documentAgreementPath != string.Empty)
                        {
                            _status = await _authService.SetUserApplicationStatus(_userDetails.Id, (int)ApplicationStatus.AwaitingVerification);
                        }
                        else
                        {
                            _status = await _authService.SetUserApplicationStatus(_userDetails.Id, (int)ApplicationStatus.Incomplete);
                        }
                        var response = await _authService.GetBusinessUserRegisterResponse(_userDetails.Id, accessToken);
                        return Ok(new { status = true, data = response, message = ResponseMessages.msgAdditionSuccess, docuploadmessage = ResponseMessages.msgDocUploadSuccess, code = StatusCodes.Status200OK });
                    }
                    else
                    {
                        _logger.LogInfo("API: Auth/RegisterBusinessUser : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                    }
                }
                else
                {
                    _logger.LogInfo("API: Auth/RegisterBusinessUser : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Auth/RegisterBusinessUser : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region"User Login"
        [HttpPost]
        [Route("UserLogin")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            UserDetails user = new UserDetails();
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Auth/UserLogin : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                if (model.EmailPhone.All(char.IsDigit))
                {
                    user = _authService.FindByPhoneNumberAsync(model.EmailPhone);
                }
                else if (model.EmailPhone.Contains('@'))
                {
                    user = await _userManager.FindByEmailAsync(model.EmailPhone);
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = "Enter Valid Email/Password", code = StatusCodes.Status406NotAcceptable });
                }
                await _userManager.CheckPasswordAsync(user, model.Password);
                if (user != null && !user.IsDeleted && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    user.DeviceToken = model.DeviceToken;
                    user.DeviceType = model.DeviceType;
                    user.UpdatedDate = DateTime.UtcNow;
                    user.UpdatedBy = user.FirstName;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        // change security stamp only on current username / password
                        await _userManager.UpdateSecurityStampAsync(user);
                    }
                    string accessToken = CommonFunctions.GenerateAccessToken(user.Id, user.DeviceToken);
                    var userLoginResponse = await _authService.GetUserLoginResponse(user.Id, accessToken);
                    if (model.EmailPhone.All(char.IsDigit))
                    {
                        if (!user.PhoneNumberConfirmed) // check whether phone number is verified or not
                        {
                            return Ok(new
                            {
                                status = false,
                                message = ResponseMessages.msgPhoneNumberNotConfirmed,
                                data = userLoginResponse,
                                code = StatusCodes.Status412PreconditionFailed
                            });
                        }
                        else
                        {
                            return Ok(new { status = true, message = ResponseMessages.msgUserLoginSuccess, data = userLoginResponse, code = StatusCodes.Status200OK });
                        }
                    }
                    else if (model.EmailPhone.Contains('@')) // email id
                    {
                        if (!user.EmailConfirmed) // check whether Email is verified or not
                        {
                            return Ok(new
                            {
                                status = false,
                                message = ResponseMessages.msgEmailNotConfirmed,
                                data = userLoginResponse,
                                code = StatusCodes.Status412PreconditionFailed
                            });
                        }
                        else
                        {
                            return Ok(new { status = true, message = ResponseMessages.msgUserLoginSuccess, data = userLoginResponse, code = StatusCodes.Status200OK });
                        }
                    }
                }
                else
                {
                    return Ok(new { status = false, message = ResponseMessages.msgInvalidCredentials, data = new { }, code = StatusCodes.Status401Unauthorized });
                }
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgInvalidCredentials, code = StatusCodes.Status401Unauthorized });
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Auth/UserLogin : " + ex.Message.ToString());
                return Ok(new { status = false, message = ResponseMessages.msgSomethingWentWrong, error = ex.Message, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region"Admin Login"
        [HttpPost]
        [Route("AdminLogin")]
        public async Task<IActionResult> AdminLogin(LoginViewModel model)
        {
            UserDetails user = new UserDetails();
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Auth/AdminLogin : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                if (model.EmailPhone.All(char.IsDigit))
                {
                    user = _authService.FindByPhoneNumberAsync(model.EmailPhone);
                }
                else if (model.EmailPhone.Contains('@'))
                {
                    user = await _userManager.FindByEmailAsync(model.EmailPhone);
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = "Enter Valid Email/Password", code = StatusCodes.Status406NotAcceptable });
                }
                await _userManager.CheckPasswordAsync(user, model.Password);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password) && user.UserTypeId == (int)AppUserTypes.Admin)
                {
                    user.DeviceToken = model.DeviceToken;
                    user.DeviceType = model.DeviceType;
                    user.UpdatedDate = DateTime.UtcNow;
                    user.UpdatedBy = user.FirstName;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        // change security stamp only on current username / password
                        await _userManager.UpdateSecurityStampAsync(user);
                    }
                    string accessToken = CommonFunctions.GenerateAccessToken(user.Id, user.DeviceToken);
                    var userLoginResponse = await _authService.GetAdminLoginResponse(user.Id, accessToken);
                    if (model.EmailPhone.Contains('@')) // email id
                    {
                        if (!user.EmailConfirmed) // check whether Email is verified or not
                        {
                            return Ok(new
                            {
                                status = false,
                                message = ResponseMessages.msgEmailNotConfirmed,
                                data = userLoginResponse,
                                code = StatusCodes.Status412PreconditionFailed
                            });
                        }
                        else
                        {
                            return Ok(new { status = true, message = ResponseMessages.msgAdminLoginSuccess, data = userLoginResponse, code = StatusCodes.Status200OK });
                        }
                    }
                }
                else
                {
                    return Ok(new { status = false, message = ResponseMessages.msgInvalidCredentials, data = new { }, code = StatusCodes.Status401Unauthorized });
                }
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgInvalidCredentials, code = StatusCodes.Status401Unauthorized });
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Auth/AdminLogin : " + ex.Message.ToString());
                return Ok(new { status = false, message = ResponseMessages.msgSomethingWentWrong, error = ex.Message, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "User Social Login"
        [HttpPost]
        [Route("UserSocialLogin")]
        public async Task<IActionResult> UserSocialLogin(SocialLoginViewModel model)
        {
            try
            {
                UserDetails user = new UserDetails();
                if (!ModelState.IsValid)
                {
                    _logger.LogInfo("API: Auth/UserSocialLogin : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
                }
                if (model.EmailPhone.Contains('@')) // Email
                {
                    user = await _userManager.FindByEmailAsync(model.EmailPhone);
                }
                else if (model.EmailPhone.All(char.IsDigit)) // PhoneNumber
                {
                    user = _authService.FindByPhoneNumberAsync(model.EmailPhone);
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = "Enter Valid Email/Password", code = StatusCodes.Status406NotAcceptable });
                }
                if (user == null)
                {
                    UserDetails _appuser = new UserDetails
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        DeviceType = model.DeviceType.ToLower(),
                        DeviceToken = model.DeviceToken,
                        IsSocialUser = true,
                        CreatedDate = DateTime.UtcNow,
                        UserTypeId = model.UserTypeId
                    };
                    if (model.LoginType.ToLower() == LoginType.Facebook.ToString().ToLower())
                    {
                        _appuser.FacebookId = model.SocialId;
                    }
                    else if (model.LoginType.ToLower() == LoginType.Google.ToString().ToLower())
                    {
                        _appuser.GoogleId = model.SocialId;
                    }
                    else
                    {
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
                    }
                    if (model.EmailPhone.Contains('@')) // email
                    {
                        _appuser.UserName = model.EmailPhone;
                        _appuser.Email = model.EmailPhone;
                        _appuser.EmailConfirmed = true;
                    }
                    else if (model.EmailPhone.All(char.IsDigit))//phone
                    {
                        _appuser.PhoneNumber = model.EmailPhone;
                        _appuser.PhoneNumberConfirmed = true;
                    }
                    else
                    {
                        return Ok(new { status = false, data = new { }, message = "Enter Valid Email/Password", code = StatusCodes.Status406NotAcceptable });
                    }

                    var result = await _userManager.CreateAsync(_appuser);
                    if (result.Succeeded)
                    {
                        string accessToken = CommonFunctions.GenerateAccessToken(_appuser.Id, _appuser.DeviceToken);
                        var userLoginResponse = await _authService.GetUserLoginResponse(_appuser.Id, accessToken);
                        return Ok(new
                        {
                            status = true,
                            data = userLoginResponse,
                            message = "User" + " " + ResponseMessages.msgCreationSuccess,
                            code = StatusCodes.Status200OK
                        });
                    }
                    else
                    {
                        return Ok(new { status = false, data = new { }, message = result.Errors.First().Description, code = StatusCodes.Status200OK });
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(user.FirstName))
                    {
                        user.FirstName = model.FirstName;
                    }

                    if (string.IsNullOrEmpty(user.LastName))
                    {
                        user.LastName = model.LastName;
                    }
                    user.UserTypeId = model.UserTypeId;
                    user.DeviceToken = model.DeviceToken;
                    user.DeviceType = model.DeviceType.ToLower();
                    // change security stamp only on current username/password
                    await _userManager.UpdateSecurityStampAsync(user);
                    // Generating access Token
                    string accessToken = CommonFunctions.GenerateAccessToken(user.Id, user.DeviceToken);
                    var userLoginResponse = await _authService.GetUserLoginResponse(user.Id, accessToken);
                    return Ok(new { status = true, data = userLoginResponse, message = ResponseMessages.msgUserLoginSuccess, code = StatusCodes.Status200OK });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Auth/UserSocialLogin : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region VerifyPhone
        [Authorize]
        [HttpPost]
        [Route("VerifyPhone")]
        public async Task<IActionResult> VerifyPhone(VerifyPhoneModel model)
        {
            try
            {
                var CurrentUserId = CommonFunctions.getUserId(User);
                if (string.IsNullOrEmpty(CurrentUserId))
                {
                    _logger.LogInfo("API: Auth/VerifyPhone : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgTokenExpired, code = StatusCodes.Status401Unauthorized });
                }
                var user = await _userManager.FindByIdAsync(CurrentUserId);
                if (user != null)
                {
                    // check user verification using twilio
                    string userPhoneNo = "+" + user.DialCode + user.PhoneNumber;
                    // var verificationResult = await _twilioManager.CheckVerificationAsync(userPhoneNo, model.code);
                    // if (verificationResult.IsValid)
                    // {
                    user.PhoneNumberConfirmed = true;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Ok(new { status = true, data = new { }, message = ResponseMessages.msgPhoneNoVerifiedSuccess, code = StatusCodes.Status200OK });
                    }
                    else
                    {
                        _logger.LogInfo("API: Auth/VerifyPhone : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                    }
                    // }
                    // else
                    // {
                    //     return Ok(new { status = false, data = new { }, message = verificationResult.Errors.FirstOrDefault().ToString(), code = StatusCodes.Status204NoContent });
                    // }
                }
                else
                {
                    return Ok(new { status = false, message = ResponseMessages.msgCouldNotFoundAssociatedUser, data = new { }, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Auth/VerifyPhone : " + ex.Message.ToString());
                return Ok(new { status = false, message = ResponseMessages.msgSomethingWentWrong, error = ex.Message, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region ResendPhoneOtp
        [Authorize]
        [HttpPost]
        [Route("ResendPhoneOtp")]
        public async Task<IActionResult> ResendPhoneOtp(PhoneModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInfo("API: Auth/ResendPhoneOtp : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
                }

                var CurrentUserId = CommonFunctions.getUserId(User);
                if (string.IsNullOrEmpty(CurrentUserId))
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgTokenExpired, code = StatusCodes.Status401Unauthorized });
                }
                var user = await _userManager.FindByIdAsync(CurrentUserId);
                if (user != null)
                {
                    user.DialCode = model.DialCode;
                    user.PhoneNumber = model.PhoneNo;
                    await _userManager.UpdateAsync(user);

                    // string userPhoneNo = "+" + user.DialCode + user.PhoneNumber;
                    // var verificationResult = await _twilioManager.StartVerificationAsync(userPhoneNo, TwilioChannelTypes.Sms.ToString().ToLower());
                    // if (verificationResult.IsValid)
                    // {
                    return Ok(new { status = true, data = new { }, message = ResponseMessages.msgOTPSentOnMobileSuccess, code = StatusCodes.Status200OK });
                    // }
                    // else
                    // {
                    //     return Ok(new { status = false, data = new { }, message = verificationResult.Errors.FirstOrDefault().ToString(), code = StatusCodes.Status404NotFound });
                    // }
                }
                return Ok(new { status = false, message = ResponseMessages.msgCouldNotFoundAssociatedUser, code = StatusCodes.Status404NotFound });
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Auth/ResendPhoneOtp : " + ex.Message.ToString());
                return Ok(new { status = false, message = ResponseMessages.msgSomethingWentWrong, error = ex.Message, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "Verify Email"
        [Authorize]
        [HttpPost]
        [Route("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(VerifyUserModel model)
        {
            try
            {
                var CurrentUserId = CommonFunctions.getUserId(User);
                if (string.IsNullOrEmpty(CurrentUserId))
                {
                    _logger.LogInfo("API: Auth/VerifyEmail : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgTokenExpired, code = StatusCodes.Status401Unauthorized });
                }
                var user = await _userManager.FindByIdAsync(CurrentUserId);
                if (user != null)
                {
                    // Check whether otp matches
                    if (user.Otp == Convert.ToInt32(model.otp))
                    {
                        user.EmailConfirmed = true;
                        var result = await _userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            return Ok(new { status = true, data = new { }, message = ResponseMessages.msgVerifiedUser, code = StatusCodes.Status200OK });
                        }
                        else
                        {
                            _logger.LogInfo("API: Auth/VerifyEmail : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                            return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                        }
                    }
                    else
                    {
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgInvalidOTP, code = StatusCodes.Status204NoContent });
                    }
                }
                else
                {
                    return Ok(new { status = false, message = ResponseMessages.msgCouldNotFoundAssociatedUser, data = new { }, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Auth/VerifyEmail : " + ex.Message.ToString());
                return Ok(new { status = false, message = ResponseMessages.msgSomethingWentWrong, error = ex.Message, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "ResendEmailCode"
        [HttpPost]
        [Route("ResendEmailCode")]
        public async Task<IActionResult> ResendEmailCode(EmailModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInfo("API: Auth/ResendEmailCode : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
                }
                var user = await _userManager.FindByEmailAsync(model.Email);
                int otpcode = CommonFunctions.getFourDigitCode();
                if (user != null)
                {
                    user.Otp = otpcode;
                    await _userManager.UpdateAsync(user);
                    var msg = EmailMessages.GetUserRegistrationResendEmailConfirmationMsg(user.FirstName, Convert.ToInt32(user.Otp));
                    await _emailSender.SendEmailAsync(email: user.Email, subject: EmailMessages.confirmationEmailSubject, message: msg);
                    return Ok(new { status = true, data = new { otpcode }, message = ResponseMessages.msgOTPSentSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    return Ok(new { status = false, message = ResponseMessages.msgCouldNotFoundAssociatedUser, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Auth/ResendEmailCode : " + ex.Message.ToString());
                return Ok(new { status = false, message = ResponseMessages.msgSomethingWentWrong, error = ex.Message, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Logout
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                string CurrentUserId = CommonFunctions.getUserId(User);
                if (string.IsNullOrEmpty(CurrentUserId))
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgTokenExpired, code = StatusCodes.Status401Unauthorized });
                }
                var user = await _userManager.FindByIdAsync(CurrentUserId);
                if (user != null)
                {
                    user.DeviceType = string.Empty;
                    user.DeviceToken = string.Empty;
                    user.UpdatedDate = DateTime.UtcNow;
                    user.UpdatedBy = user.FirstName;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        await _userManager.UpdateSecurityStampAsync(user);
                        return Ok(new { status = true, data = new { }, message = ResponseMessages.msgLogoutSuccess, code = StatusCodes.Status200OK });
                    }
                    else
                    {
                        return Ok(new { status = false, data = new { }, message = result.Errors.FirstOrDefault().Description, code = StatusCodes.Status204NoContent });
                    }
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgCouldNotFoundAssociatedUser, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Auth/Logout : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region DeleteUser
        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                string CurrentUserId = CommonFunctions.getUserId(User);
                if (string.IsNullOrEmpty(CurrentUserId))
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgTokenExpired, code = StatusCodes.Status401Unauthorized });
                }
                var user = await _userManager.FindByIdAsync(CurrentUserId);
                if (user != null)
                {
                    user.DeviceType = string.Empty;
                    user.DeviceToken = string.Empty;
                    user.UpdatedDate = DateTime.UtcNow;
                    user.UpdatedBy = user.FirstName;
                    user.Email = DateTime.Now.Ticks + "deleted" + user.Email;
                    user.IsDeleted = true;
                    user.PhoneNumber = DateTime.Now.Ticks.ToString().Substring(0, 10);
                    user.NormalizedEmail = DateTime.Now.Ticks + "deleted" + user.Email;
                    user.UserName = DateTime.Now.Ticks + "deleted" + user.Email;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        await _userManager.UpdateSecurityStampAsync(user);
                        return Ok(new { status = true, data = new { }, message = ResponseMessages.msgDeletionSuccess, code = StatusCodes.Status200OK });
                    }
                    else
                    {
                        return Ok(new { status = false, data = new { }, message = result.Errors.FirstOrDefault().Description, code = StatusCodes.Status204NoContent });
                    }
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgCouldNotFoundAssociatedUser, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Auth/DeleteUser : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion


        #region Forgot Password
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPassword model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Auth/ForgotPassword : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            UserDetails user = new UserDetails();
            try
            {
                if (model.EmailPhone.All(char.IsDigit))          //phone number
                {
                    user = _authService.FindByPhoneNumberAsync(model.EmailPhone);
                }
                else if (model.EmailPhone.Contains('@'))         //email
                {
                    user = await _userManager.FindByEmailAsync(model.EmailPhone);
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status406NotAcceptable });
                }
                if (user != null)
                {
                    int otp = CommonFunctions.getFourDigitCode();
                    user.Otp = otp;
                    await _userManager.UpdateAsync(user);

                    if (model.EmailPhone.Contains('@'))
                    {
                        if (user.EmailConfirmed)
                        {
                            var msg = EmailMessages.GetUserForgotPasswordMsg(user.FirstName, otp);
                            await _emailSender.SendEmailAsync(email: user.Email, subject: EmailMessages.resetPasswordSubject, message: msg);
                        }
                        return Ok(new { status = true, data = new { otp }, message = ResponseMessages.msgOTPSentSuccess, code = StatusCodes.Status200OK });
                    }
                    else if (model.EmailPhone.All(char.IsDigit))
                    {
                        if (user.PhoneNumberConfirmed)
                        {
                            string userPhoneNo = "+" + user.DialCode + user.PhoneNumber;
                            var verificationResult = await _twilioManager.StartVerificationAsync(userPhoneNo, TwilioChannelTypes.Sms.ToString().ToLower());
                            if (verificationResult.IsValid)
                            {
                                return Ok(new { status = true, data = new { }, message = ResponseMessages.msgOTPSentOnMobileSuccess, code = StatusCodes.Status200OK });
                            }
                            else
                            {
                                return Ok(new { status = false, data = new { }, message = verificationResult.Errors.FirstOrDefault().ToString(), code = StatusCodes.Status204NoContent });
                            }
                        }
                        else
                        {
                            return Ok(new { status = true, data = new { otp }, message = ResponseMessages.msgOTPSentSuccess, code = StatusCodes.Status200OK });
                        }
                    }
                    else
                    {
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgCouldNotFoundAssociatedUser, code = StatusCodes.Status404NotFound });
                    }
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgCouldNotFoundAssociatedUser, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Auth/ForgotPassword : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Reset Password
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetUserModel model)
        {
            UserDetails user = new UserDetails();
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Auth/ResetPassword : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                if (model.EmailPhone.All(char.IsDigit))          //phone number
                {
                    user = _authService.FindByPhoneNumberAsync(model.EmailPhone);
                }
                else if (model.EmailPhone.Contains('@'))            //email
                {
                    user = await _userManager.FindByEmailAsync(model.EmailPhone);
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = "Enter Valid Email/PhoneNumber", code = StatusCodes.Status406NotAcceptable });
                }

                if (user != null)
                {
                    if (model.EmailPhone.Contains('@'))
                    {
                        if (user.Otp == Convert.ToInt32(model.Otp))
                        {
                            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                            IdentityResult result = await _userManager.ResetPasswordAsync(user, code, model.newPassword);
                            if (result.Succeeded)
                            {
                                return Ok(new { status = true, data = new { }, message = ResponseMessages.msgPasswordChangeSuccess, code = StatusCodes.Status200OK });
                            }
                            else
                            {
                                return Ok(new { status = false, data = new { }, message = result.Errors.First().Description, code = StatusCodes.Status200OK });
                            }
                        }
                        else
                        {
                            return Ok(new { status = false, data = new { }, message = ResponseMessages.msgInvalidOTP, code = StatusCodes.Status204NoContent });
                        }
                    }
                    else if (model.EmailPhone.All(char.IsDigit))
                    {
                        string userPhoneNo = "+" + user.DialCode + user.PhoneNumber;
                        var verificationResult = await _twilioManager.CheckVerificationAsync(userPhoneNo, model.Otp);
                        if (verificationResult.IsValid)
                        {
                            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                            IdentityResult result = await _userManager.ResetPasswordAsync(user, code, model.newPassword);
                            await _userManager.UpdateAsync(user);
                            return Ok(new { status = true, data = new { }, message = ResponseMessages.msgPasswordChangeSuccess, code = StatusCodes.Status200OK });
                        }
                        else
                        {
                            return Ok(new { status = false, data = new { }, message = verificationResult.Errors.First(), code = StatusCodes.Status204NoContent });
                        }
                    }
                    else
                    {
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgInvalidOTP, code = StatusCodes.Status204NoContent });
                    }
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgCouldNotFoundAssociatedUser, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Auth/ResetPassword : " + ex.Message.ToString());
                return Ok(new { status = false, message = ResponseMessages.msgSomethingWentWrong, error = ex.Message, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region ChangePassword
        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInfo("API: Auth/ChangePassword : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status200OK });
                }
                if (model.OldPassword == model.NewPassword)
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgSamePasswords, code = StatusCodes.Status200OK });
                }
                var CurrentUserId = CommonFunctions.getUserId(User);
                if (string.IsNullOrEmpty(CurrentUserId))
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgTokenExpired, code = StatusCodes.Status200OK });
                }
                var user = await _userManager.FindByIdAsync(CurrentUserId);
                if (user != null)
                {
                    IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return Ok(new { status = true, data = new { }, message = ResponseMessages.msgPasswordChangeSuccess, code = StatusCodes.Status200OK });
                    }
                    else
                    {
                        return Ok(new { status = false, data = new { }, message = result.Errors.First().Description, code = StatusCodes.Status200OK });
                    }
                }
                else
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgCouldNotFoundAssociatedUser, code = StatusCodes.Status200OK });

            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = new { }, message = ex.Message, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "UpdateUserProfilePic"
        [Authorize]
        [HttpPost]
        [Route("UpdateProfilePic")]
        public async Task<IActionResult> UpdateProfilePic([FromForm] ImageFileUploadModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInfo("API: Auth/UpdateProfilePic : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                    return Ok(new
                    {
                        status = false,
                        data = new { },
                        message = ResponseMessages.msgParametersNotCorrect,
                        code = StatusCodes.Status400BadRequest
                    });
                }
                string currentUserId = CommonFunctions.getUserId(User);
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgTokenExpired, code = StatusCodes.Status401Unauthorized });
                }
                var user = await _userManager.FindByIdAsync(currentUserId);
                if (user != null)
                {
                    var filename = ContentDispositionHeaderValue.Parse(model.ImgFile.ContentDisposition).FileName.Trim('"');
                    filename = CommonFunctions.EnsureCorrectFilename(filename);
                    filename = CommonFunctions.RenameFileName(filename);
                    using (FileStream fs = System.IO.File.Create(GetPathAndFilename(filename, profilePictureContainer)))
                    {
                        model.ImgFile.CopyTo(fs);
                        fs.Flush();
                    }
                    user.ProfilePic = profilePictureContainer + filename;
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Ok(new
                        {
                            status = true,
                            data = new { profilepicurl = user.ProfilePic },
                            message = ResponseMessages.msgProfilePicUpdated,
                            code = StatusCodes.Status200OK
                        });
                    }
                    else
                    {
                        return Ok(new { status = false, data = new { }, message = result.Errors.First().Description, code = StatusCodes.Status200OK });

                    }
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgCouldNotFoundAssociatedUser, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Auth/UpdateProfilePic : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "GetDocumentTypes"
        [HttpGet]
        [Route("GetDocumentTypes")]
        public IActionResult GetDocumentTypes()
        {
            try
            {
                var _docTypeDetails = _authService.GetDocumentType();
                if (_docTypeDetails != null)
                {
                    return Ok(new { status = true, data = _docTypeDetails, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Auth/GetDocumentTypes : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Auth/GetDocumentTypes : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Get All Business Users
        [HttpGet("GetAllBusinessUsers")]
        public async Task<IActionResult> GetAllBusinessUsers()
        {
            if (!ModelState.IsValid)
            {
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var result = await _authService.GetBusinessUsers();
                if (result != null)
                {
                    return Ok(new { status = true, data = result, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Auth/GetAllBusinessUsers : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Auth/GetAllBusinessUsers : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion



        #region "private Methods"
        private string GetPathAndFilename(string filename, string foldername)
        {
            string path = _hostingEnvironment.WebRootPath + "//" + foldername + "//";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path + filename;
        }
        #endregion

    }
}