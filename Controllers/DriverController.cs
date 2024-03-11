using _AbsoPickUp.Common;
using _AbsoPickUp.IServices;
using _AbsoPickUp.LoggerService;
using _AbsoPickUp.Models;
using _AbsoPickUp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static _AbsoPickUp.Common.GlobalVariables;

namespace _AbsoPickUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IEmailSender _emailSender;
        private readonly ITwilioManager _twilioManager;
        private readonly INotificationService _notificationService;
        private readonly ILoggerManager _logger;

        public DriverController(ILoggerManager logger, INotificationService notificationService, IWebHostEnvironment hostingEnvironment, IDriverService driverService, IEmailSender emailSender, ITwilioManager twilioManager)
        {
            _hostingEnvironment = hostingEnvironment;
            _driverService = driverService;
            _emailSender = emailSender;
            _twilioManager = twilioManager;
            _notificationService = notificationService;
            _logger = logger;
        }

        #region Driver Login
        [HttpPost("DriverLogin")]
        public async Task<IActionResult> DriverLogin(LoginViewModel model)
        {
            DriverDetails user = new DriverDetails();
            try
            {
                if (model.EmailPhone.All(char.IsDigit))          //phone number
                {
                    user = _driverService.FindByPhoneNumberAsync(model.EmailPhone);
                }
                else if (model.EmailPhone.Contains('@'))         //email
                {
                    user = _driverService.FindByEmailAddressAsync(model.EmailPhone);
                }
                else
                {
                    _logger.LogInfo("API: Driver/DriverLogin : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                    return Ok(new { status = false, data = new { }, message = "Enter Valid Email/Password", code = StatusCodes.Status406NotAcceptable });
                }
                string decryptpassword = "";
                try
                {
                    decryptpassword = user.PasswordHash;
                }
                catch (System.Exception)
                {

                    return Ok(new { status = false, message = ResponseMessages.msgInvalidCredentials, data = new { }, code = StatusCodes.Status401Unauthorized });
                }

                var result = CommonFunctions.Decrypt(decryptpassword);
                if (user != null && !user.IsDeleted && result == model.Password)
                {

                    user.DeviceToken = model.DeviceToken;
                    user.DeviceType = model.DeviceType;
                    user.UpdatedDate = DateTime.UtcNow;
                    user.UpdatedBy = user.FirstName;

                    var res = _driverService.UpdateAsync(user);
                    if (res)
                    {
                        string accessToken = CommonFunctions.GenerateAccessToken(user.DriverId.ToString(), user.DeviceToken);
                        var driverLoginResponse = await _driverService.GetDriverLoginResponse(user.DriverId.ToString(), accessToken, model.EmailPhone);
                        if (model.EmailPhone.All(char.IsDigit))
                        {
                            if (!user.IsPhoneNumberConfirmed) // check whether phone number is verified or not
                            {
                                return Ok(new
                                {
                                    status = false,
                                    message = ResponseMessages.msgPhoneNumberNotConfirmed,
                                    data = driverLoginResponse,
                                    code = StatusCodes.Status412PreconditionFailed
                                });
                            }
                            else
                            {
                                return Ok(new { status = true, message = ResponseMessages.msgDriverLoginSuccess, data = driverLoginResponse, code = StatusCodes.Status200OK });
                            }
                        }
                        else if (model.EmailPhone.Contains('@')) // email id
                        {
                            return Ok(new
                            {
                                status = false,
                                message = ResponseMessages.msgEmailNotConfirmed,
                                data = driverLoginResponse,
                                code = StatusCodes.Status412PreconditionFailed
                            });
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
                _logger.LogError("API: Driver/DriverLogin: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = "User not registered.", code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Add Driver Personal Details
        [HttpPost("DriverPersonalDetails")]
        public IActionResult DriverPersonalDetails(DriverPersonalInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Driver/DriverPersonalDetails: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            if (_driverService.IsEmailRegistered(model.Email))
            {
                _logger.LogInfo("API: Driver/DriverPersonalDetails: " + StatusCodes.Status406NotAcceptable.ToString() + ": " + ResponseMessages.msgEmailNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgEmailNotCorrect, code = StatusCodes.Status406NotAcceptable });
            }
            //if (_driverService.IsPhoneRegistered(model.PhoneNumber))
            //{
            //    _logger.LogInfo("API: Driver/DriverPersonalDetails: " + StatusCodes.Status406NotAcceptable.ToString() + ": " + ResponseMessages.msgPhoneAlreadyExists);
            //    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgPhoneAlreadyExists, code = StatusCodes.Status406NotAcceptable });
            //}
            try
            {
                var _driverDetails = new DriverDetails()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DialCode = model.DialCode,
                    PhoneNumber = model.PhoneNumber,
                    IsPhoneNumberConfirmed = true,
                    ScreenId = (int)Screens.DriverPersonalInfo,
                    Email = model.Email,
                    Address = model.Address,
                    City = model.City,
                    ProvinceId = model.ProvinceId,
                    DeviceType = model.DeviceType,
                    DeviceToken = model.DeviceToken,
                    PasswordHash = CommonFunctions.EncryptPassword(model.Password),
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = model.FirstName,
                    ApplicationStatus = (int)ApplicationStatus.Verified
                };
                var result = _driverService.AddDriverPersonalInfo(_driverDetails);
                if (result)
                {
                    string accessToken = CommonFunctions.GenerateAccessToken(_driverDetails.DriverId.ToString(), model.DeviceToken);
                    return Ok(new { status = true, data = accessToken, message = ResponseMessages.msgAdditionSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Driver/DriverPersonalDetails: " + StatusCodes.Status406NotAcceptable.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/DriverPersonalDetails: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Add Driver Documents
        [HttpPost("AddDriverDocuments")]
        public IActionResult AddDriverDocuments([FromForm] DriverVehicleDocumentsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Driver/AddDriverDocuments: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var documentFile = ContentDispositionHeaderValue.Parse(model.ProfilePic.ContentDisposition).FileName.Trim('"');
                documentFile = CommonFunctions.EnsureCorrectFilename(documentFile);
                documentFile = CommonFunctions.RenameFileName(documentFile);
                using (FileStream fs = System.IO.File.Create(GetPathAndFilename(documentFile, ProfilePicContainer)))
                {
                    model.ProfilePic.CopyTo(fs);
                    fs.Flush();
                }
                string documentPath = ProfilePicContainer + documentFile;
                string _profilePic = documentPath;

                var _documentdetails = new DriverDocuments()
                {
                    DocTypeId = Convert.ToInt32(DocTypes.DriverProfilePic),
                    DriverId = model.DriverId,
                    DocImgPath = documentPath
                };

                var documentImgFile = ContentDispositionHeaderValue.Parse(model.PersonalId.ContentDisposition).FileName.Trim('"');
                documentImgFile = CommonFunctions.EnsureCorrectFilename(documentImgFile);
                documentImgFile = CommonFunctions.RenameFileName(documentImgFile);
                using (FileStream fs = System.IO.File.Create(GetPathAndFilename(documentImgFile, DriverDocumentContainer)))
                {
                    model.PersonalId.CopyTo(fs);
                    fs.Flush();
                }
                string documentImgPath = DriverDocumentContainer + documentImgFile;

                var _documentdetailsIDProof = new DriverDocuments()
                {
                    DocTypeId = model.DocTypeId,
                    DriverId = model.DriverId,
                    DocImgPath = documentImgPath
                };

                var documentFileSelfie = ContentDispositionHeaderValue.Parse(model.Selfie.ContentDisposition).FileName.Trim('"');
                documentFileSelfie = CommonFunctions.EnsureCorrectFilename(documentFileSelfie);
                documentFileSelfie = CommonFunctions.RenameFileName(documentFileSelfie);
                using (FileStream fs = System.IO.File.Create(GetPathAndFilename(documentFileSelfie, DriverDocumentContainer)))
                {
                    model.Selfie.CopyTo(fs);
                    fs.Flush();
                }
                string selfiePath = DriverDocumentContainer + documentFileSelfie;

                var _selfiedetails = new DriverDocuments()
                {
                    DocTypeId = Convert.ToInt32(DocTypes.Selfie),
                    DriverId = model.DriverId,
                    DocImgPath = selfiePath
                };

                var residentProofFile = ContentDispositionHeaderValue.Parse(model.ResidentProof.ContentDisposition).FileName.Trim('"');
                residentProofFile = CommonFunctions.EnsureCorrectFilename(residentProofFile);
                residentProofFile = CommonFunctions.RenameFileName(residentProofFile);
                using (FileStream fs = System.IO.File.Create(GetPathAndFilename(residentProofFile, DriverDocumentContainer)))
                {
                    model.ResidentProof.CopyTo(fs);
                    fs.Flush();
                }
                string residentProofPath = DriverDocumentContainer + residentProofFile;

                var residentProofdetails = new DriverDocuments()
                {
                    DocTypeId = Convert.ToInt32(DocTypes.ProofOfResidence),
                    DriverId = model.DriverId,
                    DocImgPath = residentProofPath
                };

                var result = _driverService.AddDriverDocuments(_profilePic, model.DOB, _documentdetailsIDProof);
                var response = _driverService.AddDocuments(_selfiedetails, residentProofdetails);

                if (result && response)
                {
                    return Ok(new { status = true, data = new { }, message = "Documents" + ResponseMessages.msgAdditionSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Driver/AddDriverDocuments: " + StatusCodes.Status406NotAcceptable.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/AddDriverDocuments: " + ex.Message.ToString());
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
                _logger.LogInfo("API: Driver/ForgotPassword: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            DriverDetails user = new DriverDetails();
            try
            {
                if (model.EmailPhone.All(char.IsDigit))          //phone number
                {
                    user = _driverService.FindByPhoneNumberAsync(model.EmailPhone);
                }
                else if (model.EmailPhone.Contains('@'))         //email
                {
                    user = _driverService.FindByEmailAddressAsync(model.EmailPhone);
                }
                else
                {
                    _logger.LogInfo("API: Driver/ForgotPassword: " + StatusCodes.Status406NotAcceptable.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status406NotAcceptable });
                }
                if (user != null)
                {
                    int otp = CommonFunctions.getFourDigitCode();
                    user.Otp = otp;
                    _driverService.UpdateAsync(user);

                    if (model.EmailPhone.Contains('@'))
                    {
                        var msg = EmailMessages.GetUserForgotPasswordMsg(user.FirstName, otp);
                        await _emailSender.SendEmailAsync(email: user.Email, subject: EmailMessages.resetPasswordSubject, message: msg);
                        return Ok(new { status = true, data = new { otp }, message = ResponseMessages.msgOTPSentSuccess, code = StatusCodes.Status200OK });
                    }
                    else if (model.EmailPhone.All(char.IsDigit))
                    {
                        if (user.IsPhoneNumberConfirmed)
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
                _logger.LogError("API: Driver/ForgotPassword: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Reset Password
        [HttpPost("ResetDriverPassword")]
        public async Task<IActionResult> ResetDriverPassword(ResetUserModel model)
        {
            DriverDetails user = new DriverDetails();
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Driver/ResetDriverPassword: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                if (model.EmailPhone.All(char.IsDigit))          //phone number
                {
                    user = _driverService.FindByPhoneNumberAsync(model.EmailPhone);
                }
                else if (model.EmailPhone.Contains('@'))         //email
                {
                    user = _driverService.FindByEmailAddressAsync(model.EmailPhone);
                }
                else
                {
                    _logger.LogInfo("API: Driver/ResetDriverPassword: " + StatusCodes.Status406NotAcceptable.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                    return Ok(new { status = false, data = new { }, message = "Enter Valid Email/Password", code = StatusCodes.Status406NotAcceptable });
                }
                if (user != null)
                {
                    if (model.EmailPhone.Contains('@'))
                    {
                        if (user.Otp == Convert.ToInt32(model.Otp))
                        {
                            var response = CommonFunctions.EncryptPassword(model.newPassword);
                            user.PasswordHash = response;
                            _driverService.UpdateAsync(user);
                            return Ok(new { status = true, data = new { }, message = ResponseMessages.msgPasswordChangeSuccess, code = StatusCodes.Status200OK });
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
                            var response = CommonFunctions.EncryptPassword(model.newPassword);
                            user.PasswordHash = response;
                            _driverService.UpdateAsync(user);
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
                _logger.LogError("API: Driver/ResetDriverPassword: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Resend Driver Phone OTP
        [HttpPost("ResendDriverPhoneOTP")]
        public async Task<IActionResult> ResendDriverPhoneOTP(PhoneModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Driver/ResendDriverPhoneOTP: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                // var user = _driverService.FindByPhoneNumberAsync(model.PhoneNo.Trim());
                // if (user == null)
                // {
                //     string userPhoneNo = "+" + model.DialCode + model.PhoneNo;
                //     var verificationResult = await _twilioManager.StartVerificationAsync(userPhoneNo, TwilioChannelTypes.Sms.ToString().ToLower());
                //     if (verificationResult.IsValid)
                //     {
                return Ok(new { status = true, data = new { }, message = ResponseMessages.msgOTPSentOnMobileSuccess, code = StatusCodes.Status200OK });
                //     }
                //     else
                //     {
                //         return Ok(new { status = false, data = new { }, message = verificationResult.Errors.FirstOrDefault().ToString(), code = StatusCodes.Status404NotFound });
                //     }
                // }
                // else
                // {
                //     return Ok(new { status = false, data = new { }, message = ResponseMessages.msgPhoneAlreadyExists, code = StatusCodes.Status409Conflict });
                // }

            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/ResendDriverPhoneOTP: " + ex.Message.ToString());
                return Ok(new { status = false, message = ResponseMessages.msgSomethingWentWrong, error = ex.Message, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Verify Driver Phone
        [HttpPost("VerifyDriverPhone")]
        public async Task<IActionResult> VerifyDriverPhone(VerifyDriverPhoneModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Driver/VerifyDriverPhone: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                // string userPhoneNo = "+" + model.DialCode + model.PhoneNumber;
                // var verificationResult = await _twilioManager.CheckVerificationAsync(userPhoneNo, model.code);
                // if (verificationResult.IsValid)
                // {
                return Ok(new { status = true, data = new { }, message = ResponseMessages.msgPhoneNoVerifiedSuccess, code = StatusCodes.Status200OK });
                // }
                // else
                // {
                //     return Ok(new { status = false, data = new { }, message = verificationResult.Errors.FirstOrDefault().ToString(), code = StatusCodes.Status204NoContent });
                // }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/VerifyDriverPhone: " + ex.Message.ToString());
                return Ok(new { status = false, message = ResponseMessages.msgSomethingWentWrong, error = ex.Message, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Logout
        [Authorize]
        [HttpPost("DriverLogout")]
        public async Task<IActionResult> DriverLogout()
        {
            try
            {
                var CurrentUserId = CommonFunctions.getUserId(User);
                if (string.IsNullOrEmpty(CurrentUserId))
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgTokenExpired, code = StatusCodes.Status401Unauthorized });
                }
                var user = await _driverService.FindDriverById(CurrentUserId);
                if (user != null)
                {
                    user.DeviceType = string.Empty;
                    user.DeviceToken = string.Empty;
                    user.UpdatedDate = DateTime.UtcNow;
                    user.UpdatedBy = user.FirstName;

                    var result = _driverService.UpdateAsync(user);
                    if (result)
                    {
                        return Ok(new { status = true, data = new { }, message = ResponseMessages.msgLogoutSuccess, code = StatusCodes.Status200OK });
                    }
                    else
                    {
                        _logger.LogInfo("API: Driver/DriverLogout: " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                    }
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgCouldNotFoundAssociatedUser, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/DriverLogout: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region DeleteDriver
        [Authorize]
        [HttpPost("DeleteDriver")]
        public async Task<IActionResult> DeleteDriver()
        {
            try
            {
                var CurrentUserId = CommonFunctions.getUserId(User);
                if (string.IsNullOrEmpty(CurrentUserId))
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgTokenExpired, code = StatusCodes.Status401Unauthorized });
                }
                var user = await _driverService.FindDriverById(CurrentUserId);
                if (user != null)
                {
                    user.DeviceType = string.Empty;
                    user.DeviceToken = string.Empty;
                    user.UpdatedDate = DateTime.UtcNow;
                    user.UpdatedBy = user.FirstName;
                    user.Email = DateTime.Now.Ticks + "deleted" + user.Email;
                    user.IsDeleted = true;
                    user.PhoneNumber = DateTime.Now.Ticks.ToString().Substring(0, 10);
                    var result = _driverService.UpdateAsync(user);
                    if (result)
                    {
                        return Ok(new { status = true, data = new { }, message = ResponseMessages.msgDeletionSuccess, code = StatusCodes.Status200OK });
                    }
                    else
                    {
                        _logger.LogInfo("API: Driver/DriverLogout: " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                    }
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgCouldNotFoundAssociatedUser, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/DriverLogout: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "GetDriverInfo"
        [Authorize]
        [HttpPost]
        [Route("GetDriverInfo")]
        public IActionResult GetDriverInfo(int driverId)
        {
            if (driverId <= 0)
            {
                _logger.LogInfo("API: Driver/GetDriverInfo: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _driverInfo = _driverService.GetDriverInfo(driverId);
                if (_driverInfo != null)
                {
                    return Ok(new { status = true, data = _driverInfo, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Driver/GetDriverInfo: " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/GetDriverInfo: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "DriverResponseToNotice"
        [Authorize]
        [HttpPost]
        [Route("DriverResponseToNotice")]
        public async Task<IActionResult> DriverResponseToNotice(AssignRequestViewModel reqDetails)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Driver/DriverResponseToNotice: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _res = await _driverService.DriverResponseToNewRequest(reqDetails);
                var _order = new Orders();

                if (reqDetails.hasAccepted && _res == 2)
                {
                    _order = await _driverService.CreateOrderForAcceptedRequest(reqDetails.RequestId, reqDetails.DriverId);
                    if (_order != null)
                    {
                        var _uTokens = _driverService.GetSenderDeviceTokens(_order.OrderId).ToArray();
                        if (_uTokens.Length > 0)
                        {
                            string title = NotificationTitle;
                            StringBuilder strMsg = new StringBuilder();
                            strMsg.Append("Order no. ");
                            strMsg.Append(_order.OrderId.ToString());
                            strMsg.Append(" generated against your parcel request");
                            string body = strMsg.ToString();
                            var _sent = await _notificationService.SendUserPushNotifications(_uTokens, title, body, _order);

                            if (_sent)
                            {
                                Notifications _notif = new Notifications
                                {
                                    RequestId = reqDetails.RequestId,
                                    Text = body,
                                    CreatedOn = DateTime.UtcNow,
                                    ToUserId = _order.SenderId,
                                    ToDriverId = 0,
                                    Type = (int)NotificationTypes.CONFIRMED_REQUEST,
                                    IsRead = false,
                                    IsDeleted = false
                                };

                                var _saved = await _notificationService.SavePushNotifications(_notif);
                            }
                        }

                        return Ok(new { status = true, data = _order, message = ResponseMessages.msgOrderCreated + _order.OrderId, code = StatusCodes.Status200OK });
                    }
                }
                else if (!reqDetails.hasAccepted && _res == 1)
                {
                    return Ok(new { status = true, data = reqDetails, message = ResponseMessages.msgRequestRejected, code = StatusCodes.Status200OK });
                }
                else if (reqDetails.hasAccepted && _res == 0 && _order == null)
                {
                    return Ok(new { status = true, data = reqDetails, message = ResponseMessages.msgRequestOrderCancelled, code = StatusCodes.Status207MultiStatus });
                }
                return Ok(new { status = false, data = reqDetails, message = ResponseMessages.msgRequestAlreadyAccepted, code = StatusCodes.Status201Created });
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/DriverResponseToNotice: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "GetRequestNoticeForTopDrivers"
        [Authorize]
        [HttpPost]
        [Route("GetRequestNoticeForTopDrivers")]
        public IActionResult GetRequestNoticeForTopDrivers(int driverId)
        {
            if (driverId == 0)
            {
                _logger.LogInfo("API: Driver/GetRequestNoticeForTopDrivers: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _res = _driverService.GetRequestNoticeForTopDrivers(driverId);
                if (_res != null && _res.Count > 0)
                {
                    var _tokenList = _driverService.GetDeviceTokenForDriver(driverId);
                    if (_tokenList.Count() > 0)
                    {
                        string title = NotificationTitle;
                        string body = NewParcelRequestNotificationText;
                        _notificationService.SendDriverPushNotifications(_tokenList.ToArray(), title, body, _res.FirstOrDefault());
                    }
                    return Ok(new { status = true, data = _res, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgRequestNotFound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/GetRequestNoticeForTopDrivers: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "GetOpenRequests"
        [Authorize]
        [HttpPost]
        [Route("GetOpenRequests")]
        public IActionResult GetOpenRequests(int driverId)
        {
            if (driverId <= 0)
            {
                _logger.LogInfo("API: Driver/GetRequestNoticeForTopDrivers: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _res = _driverService.GetAllOpenRequestNotice(driverId);
                if (_res != null)
                {
                    return Ok(new { status = true, data = _res, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Driver/GetRequestNoticeForTopDrivers: " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/GetRequestNoticeForTopDrivers: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "SaveNoticeForDriver"
        [Authorize]
        [HttpPost]
        [Route("SaveNoticeForDriver")]
        public IActionResult SaveNoticeForDriver(DriverPositionViewModel driverPosition)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Driver/SaveNoticeForDriver: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _res = _driverService.SaveNoticeForDriver(driverPosition);
                if (_res.Result > 0)
                {
                    return Ok(new { status = true, data = new { }, message = "Driver current position" + ResponseMessages.msgAdditionSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgRequestNotFound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/SaveNoticeForDriver: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "GetDriverRatings"
        [Authorize]
        [HttpPost]
        [Route("GetDriverRatings")]
        public IActionResult GetDriverRatings(int driverId)
        {
            if (driverId == 0)
            {
                _logger.LogInfo("API: Driver/GetDriverRatings: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _driverRating = _driverService.GetDriverRating(driverId);
                if (_driverRating != null)
                {
                    return Ok(new { status = true, data = _driverRating, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Driver/GetDriverRatings: " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/GetDriverRatings: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "AddDriverRatings"
        [Authorize]
        [HttpPost]
        [Route("AddDriverRatings")]
        public IActionResult AddDriverRatings(UserReviewViewModel userReview)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Driver/AddDriverRatings: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var rating = new DriverRatings
                {
                    DriverId = userReview.DriverId,
                    UserId = userReview.UserId,
                    Ratings = userReview.Rating,
                    UserComment = userReview.Review,
                    CommentedAt = DateTime.UtcNow
                };

                var _res = _driverService.SaveDriverRating(userReview.OrderId, rating);
                if (_res > 0)
                {
                    return Ok(new { status = true, data = new { }, message = "User Review" + ResponseMessages.msgAdditionSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Driver/AddDriverRatings: " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgAlreadyRatedForOrder, code = StatusCodes.Status406NotAcceptable });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/AddDriverRatings: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "AddDriverWorkStatus"
        [Authorize]
        [HttpPost]
        [Route("AddDriverWorkStatus")]
        public IActionResult AddDriverWorkStatus(int driverId, bool Online)
        {
            if (driverId <= 0)
            {
                _logger.LogInfo("API: Driver/AddDriverWorkStatus: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _res = _driverService.AddDriverWorkStatus(driverId, Online);
                if (_res != null)
                {
                    return Ok(new { status = true, data = new { }, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Driver/AddDriverWorkStatus: " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/AddDriverWorkStatus: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "GetDriverWorkStatus"
        [Authorize]
        [HttpPost("GetDriverWorkStatus")]
        public IActionResult GetDriverWorkStatus(int driverId, string FromDate, string ToDate)
        {
            if (driverId <= 0 || string.IsNullOrEmpty(FromDate) || string.IsNullOrEmpty(ToDate))
            {
                _logger.LogInfo("API: Driver/GetDriverWorkStatus: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var BeginTime = DateTime.Parse(FromDate);
                var EndTime = DateTime.Parse(ToDate);
                var _allStatus = _driverService.GetDriverWorkStatus(driverId, BeginTime, EndTime);
                if (_allStatus.Count > 0)
                {
                    return Ok(new { status = true, data = _allStatus, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Driver/GetDriverWorkStatus: " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/GetDriverWorkStatus: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize]
        [HttpPost("GetCurrentDriverWorkStatus")]
        public IActionResult GetCurrentDriverWorkStatus(int driverId)
        {
            if (driverId <= 0)
            {
                _logger.LogInfo("API: Driver/GetCurrentDriverWorkStatus: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _status = _driverService.GetDriverCurrentWorkStatus(driverId);
                return Ok(new { currentStatus = _status, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/GetCurrentDriverWorkStatus: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "GetDriverAcceptedRequests"
        [Authorize]
        [HttpPost]
        [Route("GetAcceptedRequests")]
        public IActionResult GetRequestsAcceptedByDriver(int driverId)
        {
            if (driverId <= 0)
            {
                _logger.LogInfo("API: Driver/GetAcceptedRequests: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _res = _driverService.GetDriverAcceptedRequest(driverId);
                if (_res != null)
                {
                    return Ok(new { status = true, data = _res, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Driver/GetAcceptedRequests: " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/GetAcceptedRequests: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "Verify PIN"
        [Authorize]
        [HttpPost]
        [Route("VerifyPIN")]
        public IActionResult VerifyDeliverPIN(VerifyPINViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Driver/VerifyPIN: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _res = _driverService.VerifyDeliveryPIN(model);
                if (_res != null)
                {
                    return Ok(new { status = true, data = _res.Result, message = ResponseMessages.msgPINVerificationSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgPINVerificationFailure, code = StatusCodes.Status406NotAcceptable });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/VerifyPIN: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Driver Working Details
        [Authorize]
        [HttpGet("GetWorkInProgress")]
        public IActionResult GetWorkInProgress(int driverId)
        {
            if (driverId <= 0)
            {
                _logger.LogInfo("API: Driver/GetWorkInProgress: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _res = _driverService.GetDriverWorkInProgress(driverId);
                if (_res != null)
                {
                    return Ok(new { status = true, data = _res, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Driver/GetWorkInProgress: " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/GetWorkInProgress: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize]
        [HttpGet("GetCompletedWork")]
        public IActionResult GetCompletedWork(int driverId)
        {
            if (driverId <= 0)
            {
                _logger.LogInfo("API: Driver/GetCompletedWork: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _res = _driverService.GetDriverCompletedRequests(driverId);
                if (_res != null)
                {
                    return Ok(new { status = true, data = _res, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Driver/GetCompletedWork: " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/GetCompletedWork: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "AddDriverBankDetails"
        [Authorize]
        [HttpPost]
        [Route("AddUpdateDriverBankDetails")]
        public async Task<IActionResult> AddUpdateDriverBankDetails(DriverBankDetails model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Driver/AddUpdateDriverBankDetails: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _res = await _driverService.AddUpdateDriverBankDetails(model);
                if (_res > 0)
                {
                    return Ok(new { status = true, data = model, message = "Driver bank details" + ResponseMessages.msgAdditionSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Driver/AddUpdateDriverBankDetails: " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/AddUpdateDriverBankDetails: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "GetDriverBankDetails"
        [Authorize]
        [HttpPost]
        [Route("GetDriverBankDetails")]
        public IActionResult GetDriverBankDetails(int driverId)
        {
            if (driverId <= 0)
            {
                _logger.LogInfo("API: Driver/GetDriverBankDetails: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _res = _driverService.GetDriverBankDetails(driverId);
                if (_res != null)
                {
                    return Ok(new { status = true, data = _res, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Driver/GetDriverBankDetails: " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/GetDriverBankDetails: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region "GetDocumentTypes"
        [HttpGet]
        [Route("GetDocTypesForPersonalId")]
        public IActionResult GetDocumentTypes()
        {
            try
            {
                var _docTypeDetails = _driverService.GetPersonalIdDocType();
                if (_docTypeDetails != null)
                {
                    return Ok(new { status = true, data = _docTypeDetails, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Driver/GetDocTypesForPersonalId: " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Driver/GetDocTypesForPersonalId: " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region ChangePassword
        [Authorize]
        [HttpPost]
        [Route("ChangePassword")]
        public IActionResult ChangePassword(ChangeDriverPasswordModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInfo("API: Driver/ChangePassword: " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status200OK });
                }
                if (model.OldPassword == model.NewPassword)
                {
                    _logger.LogInfo("API: Driver/ChangePassword: " + StatusCodes.Status406NotAcceptable.ToString() + ": " + ResponseMessages.msgSamePasswords);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgSamePasswords, code = StatusCodes.Status406NotAcceptable });
                }
                int result = _driverService.ChangePassword(model.DriverId, model.OldPassword, model.NewPassword);
                if (result > 0)
                {
                    return Ok(new { status = true, data = new { }, message = ResponseMessages.msgPasswordChangeSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgCouldNotFoundAssociatedUser, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogInfo("API: Driver/ChangePassword: " + StatusCodes.Status500InternalServerError.ToString() + ": " + ex.Message);
                return Ok(new { status = false, data = new { }, message = ex.Message, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        [HttpGet("SitePolicy")]
        public ActionResult<string> GetDriverPolicy()
        {
            try
            {
                var webRoot = _hostingEnvironment.WebRootPath;
                var fileContent = System.IO.File.ReadAllText(webRoot + "/templates/DriverAppPolicy.html");
                return fileContent;
            }
            catch (Exception ex)
            {
                _logger.LogInfo("API: Driver/SitePolicy: " + StatusCodes.Status500InternalServerError.ToString() + ": " + ex.Message);
                return Ok(new
                {
                    status = false,
                    data = new { },
                    message = ex.Message,
                    code = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpGet("TermsConditions")]
        public ActionResult<string> GetDriverTerms()
        {
            try
            {
                var webRoot = _hostingEnvironment.WebRootPath;
                var fileContent = System.IO.File.ReadAllText(webRoot + "/templates/DriverAppTerms.html");
                return fileContent;
            }
            catch (Exception ex)
            {
                _logger.LogInfo("API: Driver/TermsConditions: " + StatusCodes.Status500InternalServerError.ToString() + ": " + ex.Message);
                return Ok(new { status = false, data = new { }, message = ex.Message, code = StatusCodes.Status500InternalServerError });
            }
        }


        [HttpGet("GetDriverPrivacyPolicy")]
        public ActionResult<string> GetDriverPrivacyPolicy()
        {
            try
            {
                var webRoot = _hostingEnvironment.WebRootPath;
                var fileContent = System.IO.File.ReadAllText(webRoot + "/templates/DriverAppPolicy.html");
                return Content(fileContent, "text/html");
            }
            catch (Exception ex)
            {
                _logger.LogInfo("API: Driver/SitePolicy: " + StatusCodes.Status500InternalServerError.ToString() + ": " + ex.Message);
                return Ok(new
                {
                    status = false,
                    data = new { },
                    message = ex.Message,
                    code = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpGet("GetDriverTermsConditions")]
        public ActionResult<string> GetDriverTermsConditions()
        {
            try
            {
                var webRoot = _hostingEnvironment.WebRootPath;
                var fileContent = System.IO.File.ReadAllText(webRoot + "/templates/DriverAppTerms.html");
                return Content(fileContent, "text/html");
            }
            catch (Exception ex)
            {
                _logger.LogInfo("API: Driver/TermsConditions: " + StatusCodes.Status500InternalServerError.ToString() + ": " + ex.Message);
                return Ok(new { status = false, data = new { }, message = ex.Message, code = StatusCodes.Status500InternalServerError });
            }
        }


        #region Private Member
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
