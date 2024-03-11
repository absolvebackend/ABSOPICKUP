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
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static _AbsoPickUp.Common.GlobalVariables;

namespace _AbsoPickUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IAuthService _authService;
        private readonly IDriverService _driverService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<UserDetails> _userManager;
        private readonly ILoggerManager _logger;
        private readonly ITwilioManager _twilioManager;

        public AdminController(ITwilioManager twilioManager, ILoggerManager logger, UserManager<UserDetails> userManager, IAuthService authService, IAdminService adminService, IDriverService driverService, IWebHostEnvironment hostingEnvironment)
        {
            _adminService = adminService;
            _driverService = driverService;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _authService = authService;
            _logger = logger;
            _twilioManager = twilioManager;
        }

        #region Get Parcel Category
        [HttpGet("GetParcelCategory")]
        public async Task<IActionResult> GetParcelCategory()
        {
            try
            {
                var result = await _adminService.GetAllParcelCategory();
                if (result != null)
                {
                    return Ok(new { status = true, data = result, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Admin/GetParcelCategory : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Admin/GetParcelCategory : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Get Parcel Sub Category
        [HttpGet("GetParcelSubCategory")]
        public async Task<IActionResult> GetParcelSubCategory(int id)
        {
            if (id <= 0)
            {
                _logger.LogInfo("API: Admin/GetParcelSubCategory : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var result = await _adminService.GetAllParcelSubCategory(id);
                if (result != null)
                {
                    return Ok(new { status = true, data = result, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Admin/GetParcelSubCategory : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Admin/GetParcelSubCategory : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Approve Driver
        [Authorize]
        [HttpPost("ApproveDriver")]
        public IActionResult ApproveDriver(ApproveDriverViewModel model)
        {
            var _driverMobileNo = _adminService.DoesDriverExists(model.DriverId);
            if (_driverMobileNo == string.Empty || !ModelState.IsValid)
            {
                _logger.LogInfo("API: Admin/ApproveDriver : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                if (model != null)
                {
                    bool result = _adminService.ApproveDriverDetails(model);
                    if (result)
                    {
                        var _sent = _adminService.AdminDriverSendVetNotifications(model.DriverId).Result;
                        var msg = model.AdminHasApproved ? NewDriverAcceptedNotificationText : NewDriverRejectedNotificationText + model.RejectReason;
                        // var _sms = _twilioManager.SendMessage(msg, _driverMobileNo);

                        return Ok(new { status = true, pushnotificationssent = _sent, message = "Driver Vetted Successfully", code = StatusCodes.Status200OK });
                    }
                    else
                    {
                        _logger.LogInfo("API: Admin/ApproveDriver : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                    }
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Admin/ApproveDriver : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Approve Application User
        [Authorize]
        [HttpPost("ApproveAppUser")]
        public IActionResult ApproveAppUser(ApproveAppUserViewModel model)
        {
            var _userPhone = _adminService.DoesUserExists(model.UserTypeId, model.UserId);
            if (_userPhone == string.Empty || !ModelState.IsValid)
            {
                _logger.LogInfo("API: Admin/ApproveAppUser : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                if (model != null)
                {
                    bool result = _adminService.ApproveAppUserDetails(model);
                    if (result)
                    {
                        var msg = model.AdminHasApproved ? NewUserAcceptedNotificationText : NewUserRejectedNotificationText + model.RejectReason;
                        // var _sms = _twilioManager.SendMessage(msg, _userPhone);

                        var _sent = _adminService.AdminUserSendVetNotifications(model.UserId).Result;
                        return Ok(new { status = true, pushnotificationssent = _sent, message = "User Vetted Successfully", code = StatusCodes.Status200OK });
                    }
                    else
                    {
                        _logger.LogInfo("API: Admin/ApproveAppUser : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                    }
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Admin/ApproveAppUser : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Get Drivers List
        [Authorize]
        [HttpPost("GetAllDriverList")]
        public FilterationResponseModel<DriverListViewModel> GetAllDriverDetails(FilterationListViewModel model)
        {
            try
            {
                int PageSize = model.pageSize;
                int CurrentPage = model.pageNumber;
                var _list = _adminService.GetAllDriverDetails(PageSize, CurrentPage);
                int count = _list.Count;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // if CurrentPage is greater than 1 means it has previousPage  
                var previousPage = CurrentPage > 1 ? "Yes" : "No";

                // if TotalPages is greater than CurrentPage means it has nextPage  
                var nextPage = CurrentPage < TotalPages ? "Yes" : "No";
                // Returing List of Customers Collections  
                FilterationResponseModel<DriverListViewModel> obj = new FilterationResponseModel<DriverListViewModel>
                {
                    totalCount = TotalCount,
                    pageSize = PageSize,
                    currentPage = CurrentPage,
                    totalPages = TotalPages,
                    previousPage = previousPage,
                    nextPage = nextPage,
                    dataList = _list
                };
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Admin/GetAllDriverList : " + ex.Message.ToString());
                FilterationResponseModel<DriverListViewModel> obj = new FilterationResponseModel<DriverListViewModel>
                {
                    totalCount = 0,
                    pageSize = 0,
                    currentPage = 0,
                    totalPages = 0,
                    previousPage = "",
                    nextPage = "",
                    dataList = null,
                    exception = ex.Message
                };
                return obj;
            }
        }
        #endregion

        #region Get Users List - new, approved, rejected
        [Authorize]
        [HttpPost("GetAllUserList")]
        public FilterationResponseModel<UserListViewModel> GetAllUsersList(FilterationListViewModel model)
        {
            try
            {
                int PageSize = model.pageSize;
                int CurrentPage = model.pageNumber;
                var _list = _adminService.GetAllUsersList(PageSize, CurrentPage);
                int count = _list.Count;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // if CurrentPage is greater than 1 means it has previousPage  
                var previousPage = CurrentPage > 1 ? "Yes" : "No";

                // if TotalPages is greater than CurrentPage means it has nextPage  
                var nextPage = CurrentPage < TotalPages ? "Yes" : "No";
                // Returing List of Customers Collections  
                FilterationResponseModel<UserListViewModel> obj = new FilterationResponseModel<UserListViewModel>
                {
                    totalCount = TotalCount,
                    pageSize = PageSize,
                    currentPage = CurrentPage,
                    totalPages = TotalPages,
                    previousPage = previousPage,
                    nextPage = nextPage,
                    dataList = _list
                };
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Admin/GetAllUserList : " + ex.Message.ToString());
                FilterationResponseModel<UserListViewModel> obj = new FilterationResponseModel<UserListViewModel>
                {
                    totalCount = 0,
                    pageSize = 0,
                    currentPage = 0,
                    totalPages = 0,
                    previousPage = "",
                    nextPage = "",
                    dataList = null,
                    exception = ex.Message
                };
                return obj;
            }
        }

        [Authorize]
        [HttpPost("GetAllApprovedUserList")]
        public FilterationResponseModel<UserListViewModel> GetAllApprovedUsersList(FilterationListViewModel model)
        {
            try
            {
                int PageSize = model.pageSize;
                int CurrentPage = model.pageNumber;
                var _list = _adminService.GetAllApprovedUsersList(PageSize, CurrentPage);
                int count = _list.Count;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // if CurrentPage is greater than 1 means it has previousPage  
                var previousPage = CurrentPage > 1 ? "Yes" : "No";

                // if TotalPages is greater than CurrentPage means it has nextPage  
                var nextPage = CurrentPage < TotalPages ? "Yes" : "No";
                // Returing List of Customers Collections  
                FilterationResponseModel<UserListViewModel> obj = new FilterationResponseModel<UserListViewModel>
                {
                    totalCount = TotalCount,
                    pageSize = PageSize,
                    currentPage = CurrentPage,
                    totalPages = TotalPages,
                    previousPage = previousPage,
                    nextPage = nextPage,
                    dataList = _list
                };
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Admin/GetAllApprovedUserList : " + ex.Message.ToString());
                FilterationResponseModel<UserListViewModel> obj = new FilterationResponseModel<UserListViewModel>
                {
                    totalCount = 0,
                    pageSize = 0,
                    currentPage = 0,
                    totalPages = 0,
                    previousPage = "",
                    nextPage = "",
                    dataList = null,
                    exception = ex.Message
                };
                return obj;
            }
        }

        [Authorize]
        [HttpPost("GetAllRejectedUserList")]
        public FilterationResponseModel<UserListViewModel> GetAllRejectedUsersList(FilterationListViewModel model)
        {
            try
            {
                int PageSize = model.pageSize;
                int CurrentPage = model.pageNumber;
                var _list = _adminService.GetAllRejectedUsersList(PageSize, CurrentPage);
                int count = _list.Count;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // if CurrentPage is greater than 1 means it has previousPage  
                var previousPage = CurrentPage > 1 ? "Yes" : "No";

                // if TotalPages is greater than CurrentPage means it has nextPage  
                var nextPage = CurrentPage < TotalPages ? "Yes" : "No";
                // Returing List of Customers Collections  
                FilterationResponseModel<UserListViewModel> obj = new FilterationResponseModel<UserListViewModel>
                {
                    totalCount = TotalCount,
                    pageSize = PageSize,
                    currentPage = CurrentPage,
                    totalPages = TotalPages,
                    previousPage = previousPage,
                    nextPage = nextPage,
                    dataList = _list
                };
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Admin/GetAllRejectedUserList : " + ex.Message.ToString());
                FilterationResponseModel<UserListViewModel> obj = new FilterationResponseModel<UserListViewModel>
                {
                    totalCount = 0,
                    pageSize = 0,
                    currentPage = 0,
                    totalPages = 0,
                    previousPage = "",
                    nextPage = "",
                    dataList = null,
                    exception = ex.Message
                };
                return obj;
            }
        }
        #endregion

        #region Upload Driver Documents
        [Authorize]
        [HttpPost("UploadDriverDoc")]
        public async Task<IActionResult> UploadDriverDocuments([FromForm] DriverVehicleDocumentViewModel model)
        {
            var _driverPhone = _adminService.DoesDriverExists(model.DriverId);
            if (_driverPhone == string.Empty || !ModelState.IsValid)
            {
                _logger.LogInfo("API: Admin/UploadDriverDoc : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var documentFile = ContentDispositionHeaderValue.Parse(model.DriverDocFile.ContentDisposition).FileName.Trim('"');
                documentFile = CommonFunctions.EnsureCorrectFilename(documentFile);
                documentFile = CommonFunctions.RenameFileName(documentFile);
                using (FileStream fs = System.IO.File.Create(GetPathAndFilename(documentFile, DriverDocumentContainer)))
                {
                    model.DriverDocFile.CopyTo(fs);
                    fs.Flush();
                }
                string documentPath = DriverDocumentContainer + documentFile;

                var _documentdetails = new DriverDocuments()
                {
                    DocTypeId = model.DocTypeId,
                    DriverId = model.DriverId,
                    DocImgPath = documentPath
                };

                var result = await _adminService.UploadDriverDocuments(_documentdetails);

                if (result > 0)
                {
                    return Ok(new { status = true, data = result, message = "Driver Document" + ResponseMessages.msgAdditionSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Admin/UploadDriverDoc : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Admin/UploadDriverDoc : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Upload User Documents
        [Authorize]
        [HttpPost("UploadUserDoc")]
        public async Task<IActionResult> UploadUserDocuments([FromForm] UserDocumentsViewModel model)
        {
            var _userPhone = _adminService.DoesUserExists(model.UserTypeId, model.UserId);
            if (_userPhone == string.Empty || !ModelState.IsValid)
            {
                _logger.LogInfo("API: Admin/UploadUserDoc : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var documentFile = ContentDispositionHeaderValue.Parse(model.UserDocFile.ContentDisposition).FileName.Trim('"');
                documentFile = CommonFunctions.EnsureCorrectFilename(documentFile);
                documentFile = CommonFunctions.RenameFileName(documentFile);
                using (FileStream fs = System.IO.File.Create(GetPathAndFilename(documentFile, AdminFilesContainer)))
                {
                    model.UserDocFile.CopyTo(fs);
                    fs.Flush();
                }
                string documentPath = AdminFilesContainer + documentFile;

                if (model.UserTypeId == (int)AppUserTypes.Individual)
                {
                    var _documentdetails = new IndividualUserDocuments()
                    {
                        DocTypeId = model.DocTypeId,
                        IndividualUserId = model.UserId,
                        DocImgPath = documentPath
                    };

                    var result = await _adminService.UploadIndividualUserDocuments(_documentdetails);
                    if (result > 0)
                    {
                        return Ok(new { status = true, data = result, message = "Individual User Document" + ResponseMessages.msgAdditionSuccess, code = StatusCodes.Status200OK });
                    }
                    else
                    {
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                    }
                }
                else if (model.UserTypeId == (int)AppUserTypes.Business)
                {
                    BusinessUserDocumentsViewModel _budvw = new BusinessUserDocumentsViewModel
                    {
                        UserId = model.UserId,
                        DocTypeId = model.DocTypeId,
                        UserDocFile = documentPath,
                        UserTypeId = model.UserTypeId
                    };

                    var result = await _adminService.UploadBusinessUserDocuments(_budvw);
                    if (result > 0)
                    {
                        return Ok(new { status = true, data = result, message = "Business User Document" + ResponseMessages.msgAdditionSuccess, code = StatusCodes.Status200OK });
                    }
                    else
                    {
                        _logger.LogInfo("API: Admin/UploadUserDoc : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                    }
                }
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Admin/UploadUserDoc : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Admin Reports
        [Authorize]
        [HttpPost("GetParcelStatusByDriver")]
        public FilterationResponseModel<DeliveryDetailReportViewModel> GetParcelStatusByDriver(FilterationDriverReportViewModel model)
        {
            FilterationResponseModel<DeliveryDetailReportViewModel> obj = new FilterationResponseModel<DeliveryDetailReportViewModel>();
            var _driverPhone = _adminService.DoesDriverExists(model.driverId);
            if (_driverPhone == string.Empty)
            {
                _logger.LogInfo("API: Admin/GetParcelStatusByDriver : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                obj.exception = ResponseMessages.msgParametersNotCorrect;
                return obj;
            }
            try
            {
                int PageSize = model.pageSize;
                int CurrentPage = model.pageNumber;
                var _list = _adminService.GetParcelDetailsByDriver(model.driverId, PageSize, CurrentPage);
                int count = _list.Count;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // if CurrentPage is greater than 1 means it has previousPage  
                var previousPage = CurrentPage > 1 ? "Yes" : "No";

                // if TotalPages is greater than CurrentPage means it has nextPage  
                var nextPage = CurrentPage < TotalPages ? "Yes" : "No";
                // Returing List of Customers Collections  
                obj.totalCount = TotalCount;
                obj.pageSize = PageSize;
                obj.currentPage = CurrentPage;
                obj.totalPages = TotalPages;
                obj.previousPage = previousPage;
                obj.nextPage = nextPage;
                obj.dataList = _list;
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                obj.totalCount = 0;
                obj.pageSize = 0;
                obj.currentPage = 0;
                obj.totalPages = 0;
                obj.previousPage = "";
                obj.nextPage = "";
                obj.dataList = null;
                obj.exception = ex.Message;
                return obj;
            }
        }

        [Authorize]
        [HttpPost("GetParcelStatusByUser")]
        public FilterationResponseModel<DeliveryDetailReportViewModel> GetParcelStatusByUser(FilterationUserReportViewModel model)
        {
            FilterationResponseModel<DeliveryDetailReportViewModel> obj = new FilterationResponseModel<DeliveryDetailReportViewModel>();
            var _userPhone = _adminService.DoesUserExists(model.userTypeId, model.userId);
            if (_userPhone == string.Empty)
            {
                _logger.LogInfo("API: Admin/GetParcelStatusByDriver : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                obj.exception = ResponseMessages.msgParametersNotCorrect;
                return obj;
            }
            try
            {
                int PageSize = model.pageSize;
                int CurrentPage = model.pageNumber;
                var _list = _adminService.GetParcelDetailsByUser(model.userId, PageSize, CurrentPage);
                int count = _list.Count;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // if CurrentPage is greater than 1 means it has previousPage  
                var previousPage = CurrentPage > 1 ? "Yes" : "No";

                // if TotalPages is greater than CurrentPage means it has nextPage  
                var nextPage = CurrentPage < TotalPages ? "Yes" : "No";
                // Returing List of Customers Collections  
                obj.totalCount = TotalCount;
                obj.pageSize = PageSize;
                obj.currentPage = CurrentPage;
                obj.totalPages = TotalPages;
                obj.previousPage = previousPage;
                obj.nextPage = nextPage;
                obj.dataList = _list;
                return obj;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                obj.totalCount = 0;
                obj.pageSize = 0;
                obj.currentPage = 0;
                obj.totalPages = 0;
                obj.previousPage = "";
                obj.nextPage = "";
                obj.dataList = null;
                obj.exception = ex.Message;
                return obj;
            }
        }

        #endregion

        #region Individual / Business / Driver Details 
        [Authorize]
        [HttpGet("GetDriverInfo")]
        public IActionResult GetDriverInfoById(int driverId)
        {
            if (driverId <= 0)
            {
                _logger.LogInfo("API: Admin/GetDriverInfo : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var result = _driverService.GetDriverInfo(driverId);
                if (result != null)
                {
                    return Ok(new { status = true, data = result, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Admin/GetDriverInfo : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Admin/GetDriverInfo : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize]
        [HttpGet("GetIndiviualUserInfo")]
        public IActionResult GetIndividualUserById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogInfo("API: Admin/GetIndiviualUserInfo : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var result = _adminService.GetIndiviualUserDetailsById(userId);
                if (result != null)
                {
                    return Ok(new { status = true, data = result, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Admin/GetIndiviualUserInfo : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Admin/GetIndiviualUserInfo : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize]
        [HttpGet("GetBusinessUserInfo")]
        public IActionResult GetBusinessUserInfo(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogInfo("API: Admin/GetBusinessUserInfo : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var result = _adminService.GetBusinessUserDetailsById(userId);
                if (result != null)
                {
                    return Ok(new { status = true, data = result, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Admin/GetBusinessUserInfo : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Admin/GetBusinessUserInfo : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize]
        [HttpPost("UpdateDriverInfo")]
        public async Task<IActionResult> UpdateDriverInfo([FromForm] UpdateDriverInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Admin/UpdateDriverInfo : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            int _status;
            try
            {
                //upload driver profile pic
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

                var _resPP = await _adminService.UploadDriverDocuments(_documentdetails);
                //upload driver personal Id
                var documentImgFile = ContentDispositionHeaderValue.Parse(model.PersonalIDImg.ContentDisposition).FileName.Trim('"');
                documentImgFile = CommonFunctions.EnsureCorrectFilename(documentImgFile);
                documentImgFile = CommonFunctions.RenameFileName(documentImgFile);
                using (FileStream fs = System.IO.File.Create(GetPathAndFilename(documentImgFile, DriverDocumentContainer)))
                {
                    model.PersonalIDImg.CopyTo(fs);
                    fs.Flush();
                }
                string documentImgPath = DriverDocumentContainer + documentImgFile;

                var _documentdetailsIDProof = new DriverDocuments()
                {
                    DocTypeId = model.PersonalIDDocTypeId,
                    DriverId = model.DriverId,
                    DocImgPath = documentImgPath
                };

                var _resID = await _adminService.UploadDriverDocuments(_documentdetailsIDProof);
                //upload driver selfie
                var documentFileSelfie = ContentDispositionHeaderValue.Parse(model.SelfieImg.ContentDisposition).FileName.Trim('"');
                documentFileSelfie = CommonFunctions.EnsureCorrectFilename(documentFileSelfie);
                documentFileSelfie = CommonFunctions.RenameFileName(documentFileSelfie);
                using (FileStream fs = System.IO.File.Create(GetPathAndFilename(documentFileSelfie, DriverDocumentContainer)))
                {
                    model.SelfieImg.CopyTo(fs);
                    fs.Flush();
                }
                string selfiePath = DriverDocumentContainer + documentFileSelfie;

                var _selfiedetails = new DriverDocuments()
                {
                    DocTypeId = Convert.ToInt32(DocTypes.Selfie),
                    DriverId = model.DriverId,
                    DocImgPath = selfiePath
                };

                var _resSelf = await _adminService.UploadDriverDocuments(_selfiedetails);
                //upload driver proof of residence
                var residentProofFile = ContentDispositionHeaderValue.Parse(model.ProofOfResidenceImg.ContentDisposition).FileName.Trim('"');
                residentProofFile = CommonFunctions.EnsureCorrectFilename(residentProofFile);
                residentProofFile = CommonFunctions.RenameFileName(residentProofFile);
                using (FileStream fs = System.IO.File.Create(GetPathAndFilename(residentProofFile, DriverDocumentContainer)))
                {
                    model.ProofOfResidenceImg.CopyTo(fs);
                    fs.Flush();
                }
                string residentProofPath = DriverDocumentContainer + residentProofFile;

                var residentProofdetails = new DriverDocuments()
                {
                    DocTypeId = Convert.ToInt32(DocTypes.ProofOfResidence),
                    DriverId = model.DriverId,
                    DocImgPath = residentProofPath
                };

                var _resProof = await _adminService.UploadDriverDocuments(residentProofdetails);
                //update driver info
                DriverInfoUpdateViewModel _info = new DriverInfoUpdateViewModel
                {
                    DriverId = model.DriverId,
                    Email = model.Email,
                    Address = model.Address,
                    City = model.City,
                    ProvinceId = model.ProvinceId,
                    ProfilePic = _profilePic
                };

                var _resUP = await _adminService.UpdateDriverInfo(_info);

                var result = _resID + _resUP + _resProof + _resSelf + _resPP;

                UpdateDriverResponseViewModel _res = new UpdateDriverResponseViewModel
                {
                    DriverId = _resUP > 0 ? model.DriverId : 0,
                    PersonalIDDocTypeId = _resUP > 0 ? model.PersonalIDDocTypeId : 0,
                    PersonalIDImg = _resID > 0 ? documentImgPath : string.Empty,
                    ProofOfResidenceImg = _resProof > 0 ? residentProofPath : string.Empty,
                    SelfieImg = _resSelf > 0 ? selfiePath : string.Empty,
                    ProfilePic = _resPP > 0 ? _profilePic : string.Empty
                };

                if (_resID > 0 && _resProof > 0 && _resSelf > 0 && _resPP > 0)
                {
                    _status = await _driverService.SetDriverApplicationStatus(model.DriverId, (int)ApplicationStatus.AwaitingVerification);
                    _res.ApplicationStatus = (int)ApplicationStatus.Verified;
                }
                else
                {
                    _status = await _driverService.SetDriverApplicationStatus(model.DriverId, (int)ApplicationStatus.Incomplete);
                    _res.ApplicationStatus = (int)ApplicationStatus.Verified;
                }

                if (result == 5)
                {
                    return Ok(new { status = true, data = _res, message = "Driver Info" + ResponseMessages.msgUpdationSuccess, code = StatusCodes.Status200OK });
                }
                else if (_resPP == 0)
                {
                    return Ok(new { status = false, data = _res, message = "Profile Pic" + ResponseMessages.msgUpdateFailed, code = StatusCodes.Status204NoContent });
                }
                else if (_resID == 0)
                {
                    return Ok(new { status = false, data = _res, message = "ID Proof" + ResponseMessages.msgUpdateFailed, code = StatusCodes.Status204NoContent });
                }
                else if (_resUP == 0)
                {
                    return Ok(new { status = false, data = _res, message = "Driver Personal Info" + ResponseMessages.msgUpdateFailed, code = StatusCodes.Status204NoContent });
                }
                else if (_resProof == 0)
                {
                    return Ok(new { status = false, data = _res, message = "Residence Proof" + ResponseMessages.msgUpdateFailed, code = StatusCodes.Status204NoContent });
                }
                else if (_resSelf == 0)
                {
                    return Ok(new { status = false, data = _res, message = "Selfie Image" + ResponseMessages.msgUpdateFailed, code = StatusCodes.Status204NoContent });
                }
                else
                {
                    _logger.LogInfo("API: Admin/UpdateDriverInfo : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Admin/UpdateDriverInfo : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize]
        [HttpPost("UpdateIndividualUserInfo")]
        public async Task<IActionResult> UpdateIndividualUserInfo([FromForm] UpdateIndividualViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Admin/UpdateIndividualUserInfo : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            bool IdProofUploaded = false;
            bool IdCardBackUploaded = false;
            string IDProofDocumentPath = "";
            string IDCardBackPicDocumentPath = "";
            UpdateIndividualResponseViewModel _res = new UpdateIndividualResponseViewModel();
            try
            {
                string currentUserId = CommonFunctions.getUserId(User);
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgTokenExpired, code = StatusCodes.Status401Unauthorized });
                }
                var user = await _userManager.FindByIdAsync(currentUserId);
                var _profilePic = user.ProfilePic;
                if (user != null)
                {
                    //save profile pic
                    if (model.ProfilePic != null)
                    {
                        var filename = ContentDispositionHeaderValue.Parse(model.ProfilePic.ContentDisposition).FileName.Trim('"');
                        filename = CommonFunctions.EnsureCorrectFilename(filename);
                        filename = CommonFunctions.RenameFileName(filename);
                        using (FileStream fs = System.IO.File.Create(GetPathAndFilename(filename, profilePictureContainer)))
                        {
                            model.ProfilePic.CopyTo(fs);
                            fs.Flush();
                        }
                        _profilePic = profilePictureContainer + filename;
                    }
                    //update user details
                    user.ProfilePic = _profilePic;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.EmailConfirmed = (user.Email == model.Email) && user.EmailConfirmed;
                    user.PhoneNumberConfirmed = (user.PhoneNumber == model.PhoneNumber) && user.PhoneNumberConfirmed;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;
                    user.DOB = model.DOB;
                    user.UpdatedDate = DateTime.UtcNow;
                    user.UpdatedBy = user.UserName;

                    IdentityResult result = await _userManager.UpdateAsync(user);
                    //save user ID Proof
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
                        IDProofDocumentPath = IDProofDocContainer + IDProofDocument;
                        var IndividualUserIDProof = new IndividualUserDocuments()
                        {
                            IndividualUserId = currentUserId,
                            DocTypeId = model.IDProofTypeId,
                            DocImgPath = IDProofDocumentPath
                        };
                        IdProofUploaded = _authService.UpdateUserDocuments(IndividualUserIDProof);
                        //Set Application Status.
                        if (!(model.IDProofTypeId == (int)DocTypes.IDCard) && IdProofUploaded)
                        {
                            int _status = await _authService.SetUserApplicationStatus(currentUserId, (int)ApplicationStatus.AwaitingVerification);
                            _res.ApplicationStatus = (int)ApplicationStatus.Verified;
                        }
                        else if (!IdProofUploaded)
                        {
                            int _status = await _authService.SetUserApplicationStatus(currentUserId, (int)ApplicationStatus.Incomplete);
                            _res.ApplicationStatus = (int)ApplicationStatus.Incomplete;
                        }
                    }
                    //save user ID Card Back image
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
                        IDCardBackPicDocumentPath = IDProofDocContainer + IDCardBackPicDoc;
                        var IndividualUserIDCardBack = new IndividualUserDocuments()
                        {
                            IndividualUserId = currentUserId,
                            DocTypeId = (int)DocTypes.IDCardBack,
                            DocImgPath = IDCardBackPicDocumentPath
                        };
                        IdCardBackUploaded = _authService.UpdateUserDocuments(IndividualUserIDCardBack);
                        if ((model.IDProofTypeId == (int)DocTypes.IDCard) && IdCardBackUploaded)
                        {
                            int _status = await _authService.SetUserApplicationStatus(currentUserId, (int)ApplicationStatus.AwaitingVerification);
                            _res.ApplicationStatus = (int)ApplicationStatus.Verified;
                        }
                    }

                    //generate response object
                    _res.FirstName = model.FirstName;
                    _res.LastName = model.LastName;
                    _res.UserTypeId = model.UserTypeId;
                    _res.IDProofTypeId = model.IDProofTypeId;
                    _res.ProfilePicImgPath = _profilePic;
                    _res.IDProofImgPath = IDProofDocumentPath;
                    _res.IDCardBackImgPath = IDCardBackPicDocumentPath;

                    if (result.Succeeded && (((model.IDProofTypeId != (int)DocTypes.IDCard) && IdProofUploaded) || IdCardBackUploaded))
                    {
                        return Ok(new { status = true, data = _res, message = "Individual user info" + ResponseMessages.msgUpdationSuccess, code = StatusCodes.Status200OK });
                    }
                    else
                    {
                        _logger.LogInfo("API: Admin/UpdateIndividualUserInfo : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status204NoContent });
                    }
                }
                else
                {
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgCouldNotFoundAssociatedUser, code = StatusCodes.Status204NoContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Admin/UpdateIndividualUserInfo : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        [HttpGet("SitePolicy")]
        public ActionResult<string> GetUserPolicy()
        {
            try
            {
                var webRoot = _hostingEnvironment.WebRootPath;
                var fileContent = System.IO.File.ReadAllText(webRoot + "/templates/UserAppPolicy.html");
                return fileContent;
            }
            catch (Exception ex)
            {
                _logger.LogInfo("API: Admin/SitePolicy: " + StatusCodes.Status500InternalServerError.ToString() + ": " + ex.Message);
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
        public ActionResult<string> GetUserTerms()
        {
            try
            {
                var webRoot = _hostingEnvironment.WebRootPath;
                var fileContent = System.IO.File.ReadAllText(webRoot + "/templates/UserAppTerms.html");
                return fileContent;
            }
            catch (Exception ex)
            {
                _logger.LogInfo("API: Admin/TermsConditions: " + StatusCodes.Status500InternalServerError.ToString() + ": " + ex.Message);
                return Ok(new
                {
                    status = false,
                    data = new { },
                    message = ex.Message,
                    code = StatusCodes.Status500InternalServerError
                });
            }
        }


        [HttpGet("GetPrivacyPolicy")]
        public ActionResult<string> GetPrivacyPolicy()
        {
            try
            {
                var webRoot = _hostingEnvironment.WebRootPath;
                var fileContent = System.IO.File.ReadAllText(webRoot + "/templates/UserAppPolicy.html");
                return Content(fileContent, "text/html");
            }
            catch (Exception ex)
            {
                _logger.LogInfo("API: Admin/SitePolicy: " + StatusCodes.Status500InternalServerError.ToString() + ": " + ex.Message);
                return Ok(new
                {
                    status = false,
                    data = new { },
                    message = ex.Message,
                    code = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpGet("GetTermsConditions")]
        public ActionResult<string> GetTermsConditions()
        {
            try
            {
                var webRoot = _hostingEnvironment.WebRootPath;
                var fileContent = System.IO.File.ReadAllText(webRoot + "/templates/UserAppTerms.html");
                return Content(fileContent, "text/html");
            }
            catch (Exception ex)
            {
                _logger.LogInfo("API: Admin/TermsConditions: " + StatusCodes.Status500InternalServerError.ToString() + ": " + ex.Message);
                return Ok(new
                {
                    status = false,
                    data = new { },
                    message = ex.Message,
                    code = StatusCodes.Status500InternalServerError
                });
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
