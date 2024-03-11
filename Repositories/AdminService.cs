using _AbsoPickUp.Data;
using _AbsoPickUp.IServices;
using _AbsoPickUp.Models;
using _AbsoPickUp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _AbsoPickUp.Common.GlobalVariables;

namespace _AbsoPickUp.Repositories
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;
        public AdminService(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }
        //not needed
        public async Task<List<ParcelCategory>> GetAllParcelCategory()
        {
            try
            {
                var parcelDetails = await (from p in _context.ParcelCategory
                                           select p).ToListAsync();
                return parcelDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //not needed
        public async Task<List<ParcelSubCategoryViewModel>> GetAllParcelSubCategory(int id)
        {
            try
            {
                var parcelSubDetails = await (from p in _context.ParcelSubCategory
                                              join s in _context.ParcelCategory
                                              on p.CategoryId equals s.CategoryId
                                              where p.CategoryId == id
                                              select new ParcelSubCategoryViewModel()
                                              {
                                                  CategoryId = p.CategoryId,
                                                  SubCategoryId = p.SubCategoryId,
                                                  Description = p.Description,
                                                  ParcelCategory = s.Description
                                              }).ToListAsync();
                return parcelSubDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //approve driver details
        public bool ApproveDriverDetails(ApproveDriverViewModel model)
        {
            try
            {
                var _driverInfo = (from x in _context.DriverDetails
                                   where x.DriverId == model.DriverId
                                   && x.IsDeleted == false
                                   select x).FirstOrDefault();
                if (_driverInfo != null && model.AdminHasApproved)
                {
                    _driverInfo.ApplicationStatus = (int)ApplicationStatus.Verified;
                    _driverInfo.RejectReason = string.Empty;
                    _driverInfo.UpdatedDate = DateTime.UtcNow;
                    _driverInfo.UpdatedBy = "Admin";
                    _context.DriverDetails.Update(_driverInfo);
                    _context.SaveChanges();
                    return true;
                }
                else if (_driverInfo != null && !model.AdminHasApproved)
                {
                    _driverInfo.ApplicationStatus = (int)ApplicationStatus.Rejected;
                    _driverInfo.RejectReason = model.RejectReason;
                    _driverInfo.UpdatedDate = DateTime.UtcNow;
                    _driverInfo.UpdatedBy = "Admin";
                    _context.DriverDetails.Update(_driverInfo);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                throw;
            }
        }
        //approve user details
        public bool ApproveAppUserDetails(ApproveAppUserViewModel model)
        {
            try
            {
                var _userInfo = (from j in _context.UserDetails
                                 where j.Id == model.UserId
                                 && j.UserTypeId == model.UserTypeId
                                 && j.IsDeleted == false
                                 select j).FirstOrDefault();

                if (_userInfo != null && model.AdminHasApproved)
                {
                    _userInfo.ApplicationStatus = (int)ApplicationStatus.Verified;
                    _userInfo.RejectReason = string.Empty;
                    _userInfo.UpdatedBy = "Admin";
                    _userInfo.UpdatedDate = DateTime.UtcNow;
                    _context.UserDetails.Update(_userInfo);
                    _context.SaveChanges();
                    return true;
                }
                else if (_userInfo != null && !model.AdminHasApproved)
                {
                    _userInfo.ApplicationStatus = (int)ApplicationStatus.Rejected;
                    _userInfo.RejectReason = model.RejectReason;
                    _userInfo.UpdatedBy = "Admin";
                    _userInfo.UpdatedDate = DateTime.UtcNow;
                    _context.UserDetails.Update(_userInfo);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                throw;
            }
        }
        //get all drivers list
        public List<DriverListViewModel> GetAllDriverDetails(int pgSize, int pgNumber)
        {
            var _driverList = (from ald in _context.DriverDetails
                               where ald.IsDeleted == false
                               select new DriverListViewModel()
                               {
                                   DriverId = ald.DriverId,
                                   FirstName = ald.FirstName,
                                   LastName = ald.LastName,
                                   PhoneNumber = ald.PhoneNumber,
                                   DOB = ald.DOB,
                                   ApplicationStatus = ((ApplicationStatus)ald.ApplicationStatus).ToString(),
                                   JoiningDate = ald.CreatedDate.GetValueOrDefault().AddHours(addHoursToUTCDatetimeForSA)
                               }).ToList().OrderByDescending(x => x.JoiningDate).Skip((pgNumber - 1) * pgSize).Take(pgSize).ToList();
            return _driverList;
        }
        //get all users list
        public List<UserListViewModel> GetAllUsersList(int pgSize, int pgNumber)
        {
            var _userList = (from udt in _context.UserDetails
                             join typ in _context.UserTypes
                             on udt.UserTypeId equals typ.ID
                             where udt.IsDeleted == false
                             && udt.ApplicationStatus == (int)ApplicationStatus.AwaitingVerification
                             select new UserListViewModel()
                             {
                                 UserId = udt.Id,
                                 FirstName = udt.FirstName,
                                 LastName = udt.LastName,
                                 PhoneNumber = udt.PhoneNumber,
                                 DOB = string.IsNullOrEmpty(udt.DOB) ? string.Empty : udt.DOB,
                                 UserTypeId = udt.UserTypeId,
                                 UserType = typ.Type,
                                 ApplicationStatus = ((ApplicationStatus)udt.ApplicationStatus).ToString(),
                                 JoiningDate = udt.CreatedDate.GetValueOrDefault().AddHours(addHoursToUTCDatetimeForSA)
                             }).ToList().OrderByDescending(x => x.JoiningDate).Skip((pgNumber - 1) * pgSize).Take(pgSize).ToList();
            return _userList;
        }

        public List<UserListViewModel> GetAllApprovedUsersList(int pgSize, int pgNumber)
        {
            var _userList = (from udt in _context.UserDetails
                             join typ in _context.UserTypes
                             on udt.UserTypeId equals typ.ID
                             where udt.IsDeleted == false
                             && udt.ApplicationStatus == (int)ApplicationStatus.Verified
                             select new UserListViewModel()
                             {
                                 UserId = udt.Id,
                                 FirstName = udt.FirstName,
                                 LastName = udt.LastName,
                                 PhoneNumber = udt.PhoneNumber,
                                 DOB = string.IsNullOrEmpty(udt.DOB) ? string.Empty : udt.DOB,
                                 UserTypeId = udt.UserTypeId,
                                 UserType = typ.Type,
                                 ApplicationStatus = ((ApplicationStatus)udt.ApplicationStatus).ToString(),
                                 JoiningDate = udt.CreatedDate.GetValueOrDefault().AddHours(addHoursToUTCDatetimeForSA)
                             }).ToList().OrderByDescending(x => x.JoiningDate).Skip((pgNumber - 1) * pgSize).Take(pgSize).ToList();
            return _userList;
        }

        public List<UserListViewModel> GetAllRejectedUsersList(int pgSize, int pgNumber)
        {
            var _userList = (from udt in _context.UserDetails
                             join typ in _context.UserTypes
                             on udt.UserTypeId equals typ.ID
                             where udt.IsDeleted == false
                             && udt.ApplicationStatus == (int)ApplicationStatus.Rejected
                             select new UserListViewModel()
                             {
                                 UserId = udt.Id,
                                 FirstName = udt.FirstName,
                                 LastName = udt.LastName,
                                 PhoneNumber = udt.PhoneNumber,
                                 DOB = string.IsNullOrEmpty(udt.DOB) ? string.Empty : udt.DOB,
                                 UserTypeId = udt.UserTypeId,
                                 UserType = typ.Type,
                                 ApplicationStatus = ((ApplicationStatus)udt.ApplicationStatus).ToString(),
                                 JoiningDate = udt.CreatedDate.GetValueOrDefault().AddHours(addHoursToUTCDatetimeForSA)
                             }).ToList().OrderByDescending(x => x.JoiningDate).Skip((pgNumber - 1) * pgSize).Take(pgSize).ToList();
            return _userList;
        }

        //public List<BusinessUserDetailViewModel> GetAllBusinessUserDetails(int pgSize, int pgNumber)
        //{
        //    List<BusinessUserDetailViewModel> _businessUserList = new List<BusinessUserDetailViewModel>();

        //    var _listUsers = _context.UserDetails.Where(x => x.UserTypeId == ((int)UserTypes.Business)).ToList().OrderByDescending(x => x.CreatedDate).ToList();
        //    var _userShortList = _listUsers.Skip((pgNumber - 1) * pgSize).Take(pgSize).ToList();

        //    foreach (var _user in _userShortList)
        //    {
        //        BusinessUserDetailViewModel divm = new BusinessUserDetailViewModel();
        //        var _busDocs = _context.BusinessDocuments.Where(x => x.UserId == _user.Id).FirstOrDefault();
        //        var _busDetails = _context.BusinessDetails.Where(x => x.UserId == _user.Id).FirstOrDefault();

        //        divm.UserId = _user.Id;
        //        divm.FirstName = _user.FirstName;
        //        divm.LastName = _user.LastName;
        //        divm.DialCode = _user.DialCode;
        //        divm.PhoneNumber = _user.PhoneNumber;
        //        divm.Email = _user.Email;
        //        divm.ApplicationStatus = _user.ApplicationStatus;
        //        divm.IsPhoneNoVerified = _user.PhoneNumberConfirmed;
        //        divm.IsEmailConfirmed = _user.EmailConfirmed;
        //        divm.CreatedDate = _user.CreatedDate.GetValueOrDefault().ToString(DefaultDateTimeFormat);

        //        divm.Website = _busDetails == null ? string.Empty : _busDetails.Website;
        //        divm.VAT = _busDetails == null ? string.Empty : _busDetails.VAT;
        //        divm.ContactPerson = _busDetails == null ? string.Empty : _busDetails.ContactPerson;
        //        divm.LicenceNumber = _busDetails == null ? string.Empty : _busDetails.LicenceNumber;
        //        divm.ExternalNumber = _busDetails == null ? string.Empty : _busDetails.ExternalContractNumber;

        //        divm.VATFilePath = _busDocs == null ? string.Empty : _busDocs.VATFilePath;
        //        divm.LicenceFilePath = _busDocs == null ? string.Empty : _busDocs.LicenceFilePath;
        //        divm.ChamberCommerceFilePath = _busDocs == null ? string.Empty : _busDocs.ChamberCommerceFilePath;
        //        divm.AgreementFilePath = _busDocs == null ? string.Empty : _busDocs.AgreementFilePath;

        //        _businessUserList.Add(divm);
        //    }
        //    return _businessUserList;
        //}

        //get individual user details by Id
        public IndividualUserDetailsViewModel GetIndiviualUserDetailsById(string userId)
        {
            try
            {
                IndividualUserDetailsViewModel divm = new IndividualUserDetailsViewModel();
                var _user = _context.UserDetails.FirstOrDefault(x => x.Id == userId);

                var _userDocs = _context.IndividualUserDocuments.Where(x => x.IndividualUserId == _user.Id).ToList();
                var _idProof = _userDocs.FirstOrDefault(x => x.DocTypeId == (int)DocTypes.Passport || x.DocTypeId == (int)DocTypes.IDBook || x.DocTypeId == (int)DocTypes.IDCard || x.DocTypeId == (int)DocTypes.License);
                var _idCardBack = _userDocs.FirstOrDefault(x => x.DocTypeId == ((int)DocTypes.IDCardBack));

                divm.UserId = _user.Id;
                divm.FirstName = _user.FirstName;
                divm.LastName = _user.LastName;
                divm.DialCode = _user.DialCode;
                divm.PhoneNumber = _user.PhoneNumber;
                divm.Email = _user.Email;
                divm.ApplicationStatus = _user.ApplicationStatus;
                divm.IsPhoneNoVerified = _user.PhoneNumberConfirmed;
                divm.IsEmailConfirmed = _user.EmailConfirmed;
                divm.CreatedDate = _user.CreatedDate.GetValueOrDefault().AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateTimeFormat);
                divm.ProfilePic = string.IsNullOrEmpty(_user.ProfilePic) ? string.Empty : _user.ProfilePic;
                divm.DOB = string.IsNullOrEmpty(_user.DOB) ? string.Empty : _user.DOB;
                divm.IDProof = _idProof == null ? string.Empty : _idProof.DocImgPath;
                divm.IDCardBack = _idCardBack == null ? string.Empty : _idCardBack.DocImgPath;
                divm.IDTypeId = _idProof == null ? 0 : _idProof.DocTypeId;
                divm.IDTypeValue = _idProof == null ? string.Empty : ((DocTypes)_idProof.DocTypeId).ToString();

                return divm;
            }
            catch
            {
                throw;
            }
        }
        //get business users details by Id
        public BusinessUserDetailViewModel GetBusinessUserDetailsById(string userId)
        {
            try
            {
                BusinessUserDetailViewModel divm = new BusinessUserDetailViewModel();
                var _user = _context.UserDetails.FirstOrDefault(x => x.Id == userId);
                var _busDocs = _context.BusinessDocuments.FirstOrDefault(x => x.UserId == _user.Id);
                var _busDetails = _context.BusinessDetails.FirstOrDefault(x => x.UserId == _user.Id);

                divm.UserId = _user.Id;
                divm.FirstName = _user.FirstName;
                divm.LastName = _user.LastName;
                divm.DialCode = _user.DialCode;
                divm.PhoneNumber = _user.PhoneNumber;
                divm.Email = _user.Email;
                divm.ApplicationStatus = _user.ApplicationStatus;
                divm.IsPhoneNoVerified = _user.PhoneNumberConfirmed;
                divm.IsEmailConfirmed = _user.EmailConfirmed;
                divm.CreatedDate = _user.CreatedDate.GetValueOrDefault().AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateTimeFormat);

                divm.Website = _busDetails == null ? string.Empty : _busDetails.Website;
                divm.VAT = _busDetails == null ? string.Empty : _busDetails.VAT;
                divm.ContactPerson = _busDetails == null ? string.Empty : _busDetails.ContactPerson;
                divm.LicenceNumber = _busDetails == null ? string.Empty : _busDetails.LicenceNumber;
                divm.ExternalNumber = _busDetails == null ? string.Empty : _busDetails.ExternalContractNumber;

                divm.VATFilePath = _busDocs == null ? string.Empty : _busDocs.VATFilePath;
                divm.LicenceFilePath = _busDocs == null ? string.Empty : _busDocs.LicenceFilePath;
                divm.ChamberCommerceFilePath = _busDocs == null ? string.Empty : _busDocs.ChamberCommerceFilePath;
                divm.AgreementFilePath = _busDocs == null ? string.Empty : _busDocs.AgreementFilePath;

                return divm;
            }
            catch
            {
                throw;
            }
        }
        //upload driver documents
        public async Task<int> UploadDriverDocuments(DriverDocuments model)
        {
            try
            {
                var _ifAnyDocExists = _context.DriverDocuments.FirstOrDefault(x => x.DriverId == model.DriverId && x.DocTypeId == model.DocTypeId);
                if (_ifAnyDocExists != null)
                {
                    _ifAnyDocExists.DocImgPath = model.DocImgPath;
                    _context.DriverDocuments.Update(_ifAnyDocExists);
                }
                else
                {
                    _context.DriverDocuments.Add(model);
                }
                return await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        //upload driver documents
        public async Task<int> UpdateDriverInfo(DriverInfoUpdateViewModel model)
        {
            try
            {
                var _ifDriverExists = _context.DriverDetails.FirstOrDefault(x => x.DriverId == model.DriverId);
                if (_ifDriverExists != null)
                {
                    _ifDriverExists.Email = model.Email;
                    _ifDriverExists.Address = model.Address;
                    _ifDriverExists.City = model.City;
                    _ifDriverExists.ProvinceId = model.ProvinceId;
                    _ifDriverExists.ProfilePic = model.ProfilePic;
                    _ifDriverExists.UpdatedDate = DateTime.UtcNow;
                    _ifDriverExists.UpdatedBy = _ifDriverExists.FirstName;

                    _context.DriverDetails.Update(_ifDriverExists);
                    return await _context.SaveChangesAsync();
                }

                return 0;
            }
            catch
            {
                throw;
            }
        }

        //upload individual user documents
        public async Task<int> UploadIndividualUserDocuments(IndividualUserDocuments model)
        {
            try
            {
                var _ifAnyDocExists = _context.IndividualUserDocuments.FirstOrDefault(x => x.IndividualUserId == model.IndividualUserId && x.DocTypeId == model.DocTypeId);
                if (_ifAnyDocExists != null)
                {
                    _ifAnyDocExists.DocImgPath = model.DocImgPath;
                    _context.IndividualUserDocuments.Update(_ifAnyDocExists);
                }
                else
                {
                    _context.IndividualUserDocuments.Add(model);
                }

                return await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
        //upload business user documents
        public async Task<int> UploadBusinessUserDocuments(BusinessUserDocumentsViewModel model)
        {
            try
            {
                var _BDocs = new BusinessDocuments();

                _BDocs = _context.BusinessDocuments.FirstOrDefault(x => x.UserId == model.UserId);
                if (_BDocs != null)
                {
                    if (model.DocTypeId == (int)DocTypes.AgreementFile) { _BDocs.AgreementFilePath = model.UserDocFile; }
                    else if (model.DocTypeId == (int)DocTypes.ChamberOfCommerceFile) { _BDocs.ChamberCommerceFilePath = model.UserDocFile; }
                    else if (model.DocTypeId == (int)DocTypes.VATFile) { _BDocs.VATFilePath = model.UserDocFile; }
                    else if (model.DocTypeId == (int)DocTypes.BusinessLicense) { _BDocs.LicenceFilePath = model.UserDocFile; }

                    _context.BusinessDocuments.Update(_BDocs);
                }
                else
                {
                    _BDocs = new BusinessDocuments
                    {
                        AgreementFilePath = string.Empty,
                        ChamberCommerceFilePath = string.Empty,
                        LicenceFilePath = string.Empty,
                        VATFilePath = string.Empty,
                        UserId = model.UserId
                    };

                    if (model.DocTypeId == (int)DocTypes.AgreementFile) { _BDocs.AgreementFilePath = model.UserDocFile; }
                    else if (model.DocTypeId == (int)DocTypes.ChamberOfCommerceFile) { _BDocs.ChamberCommerceFilePath = model.UserDocFile; }
                    else if (model.DocTypeId == (int)DocTypes.VATFile) { _BDocs.VATFilePath = model.UserDocFile; }
                    else if (model.DocTypeId == (int)DocTypes.BusinessLicense) { _BDocs.LicenceFilePath = model.UserDocFile; }

                    _context.BusinessDocuments.Add(_BDocs);
                }
                return await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
        //check if driver exists
        public string DoesDriverExists(int driverId)
        {
            var driver = _context.DriverDetails.FirstOrDefault(x => x.DriverId == driverId && !x.IsDeleted);
            var IsDriver = driver != null ? driver.PhoneNumber : string.Empty;
            return IsDriver;
        }
        //check if user exists
        public string DoesUserExists(int userTypeId, string userId)
        {
            var _user = _context.UserDetails.FirstOrDefault(x => x.Id == userId && x.UserTypeId == userTypeId && !x.IsDeleted);
            var IsUser = _user != null ? _user.PhoneNumber : string.Empty;
            return IsUser;
        }
        //get parcel details for driverId
        public List<DeliveryDetailReportViewModel> GetParcelDetailsByDriver(int driverId, int pgSize, int pgNumber)
        {
            try
            {
                var _response = new List<DeliveryDetailReportViewModel>();
                var _allStatus = new List<DeliveryDetailsResponse>();

                var _listReqIds = _context.DeliveryDetails.Where(x => x.DriverId == driverId && x.StatusId == (int)ParcelStatus.Assigned).Select(x => x.RequestId).ToList();
                var _pagedListReqIds = _listReqIds.Skip((pgNumber - 1) * pgSize).Take(pgSize).ToList();

                foreach (var reqId in _pagedListReqIds)
                {
                    DeliveryDetailReportViewModel _ddrvw = new DeliveryDetailReportViewModel();
                    var request = _context.DeliveryRequest.FirstOrDefault(x => x.RequestId == reqId);
                    var _statusList = _context.DeliveryDetails.Where(x => x.RequestId == reqId).ToList();
                    foreach (var _status in _statusList)
                    {
                        DeliveryDetailsResponse _ddsl = new DeliveryDetailsResponse
                        {
                            RequestId = _status.RequestId,
                            StatusId = _status.StatusId,
                            DriverLat = _status.DriverLat,
                            DriverLong = _status.DriverLong,
                            RequestStatus = ((ParcelStatus)_status.StatusId).ToString(),
                            StatusDate = _status.DeliveryDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateFormat),
                            StatusTime = _status.DeliveryDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultTimeFormat)
                        };
                        _allStatus.Add(_ddsl);
                    }
                    _ddrvw.StatusList = _allStatus;
                    _ddrvw.RequestId = reqId;
                    _ddrvw.SenderId = request == null ? string.Empty : request.SenderId;
                    _ddrvw.DriverId = driverId;
                    _ddrvw.ReceiverAddress = request == null ? string.Empty : request.ReceiverAddress;
                    _ddrvw.SenderAddress = request == null ? string.Empty : request.SenderAddress;
                    _ddrvw.RequestCreateDateTime = request == null ? string.Empty : request.CreatedDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateTimeFormat);
                    _response.Add(_ddrvw);
                }
                return _response;
            }
            catch
            {
                throw;
            }
        }

        //get parcel details for driverId
        public List<DeliveryDetailReportViewModel> GetParcelDetailsByUser(string userId, int pgSize, int pgNumber)
        {
            try
            {
                var _response = new List<DeliveryDetailReportViewModel>();
                var _allStatus = new List<DeliveryDetailsResponse>();

                var _listReqIds = _context.DeliveryRequest.Where(x => x.SenderId == userId).Select(x => x.RequestId).ToList();
                var _pagedListReqIds = _listReqIds.Skip((pgNumber - 1) * pgSize).Take(pgSize).ToList();

                foreach (var reqId in _pagedListReqIds)
                {
                    DeliveryDetailReportViewModel _ddrvw = new DeliveryDetailReportViewModel();
                    var request = _context.DeliveryRequest.FirstOrDefault(x => x.RequestId == reqId);
                    var _statusList = _context.DeliveryDetails.Where(x => x.RequestId == reqId).ToList();
                    foreach (var _status in _statusList)
                    {
                        DeliveryDetailsResponse _ddsl = new DeliveryDetailsResponse
                        {
                            RequestId = _status.RequestId,
                            StatusId = _status.StatusId,
                            DriverLat = _status.DriverLat,
                            DriverLong = _status.DriverLong,
                            RequestStatus = ((ParcelStatus)_status.StatusId).ToString(),
                            StatusDate = _status.DeliveryDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateFormat),
                            StatusTime = _status.DeliveryDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultTimeFormat)
                        };
                        if (_status.StatusId == (int)ParcelStatus.Assigned)
                        {
                            _ddrvw.DriverId = _status.DriverId;
                        }
                        _allStatus.Add(_ddsl);
                    }
                    _ddrvw.StatusList = _allStatus;
                    _ddrvw.RequestId = reqId;
                    _ddrvw.SenderId = userId;
                    _ddrvw.ReceiverAddress = request == null ? string.Empty : request.ReceiverAddress;
                    _ddrvw.SenderAddress = request == null ? string.Empty : request.SenderAddress;
                    _ddrvw.RequestCreateDateTime = request == null ? string.Empty : request.CreatedDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateTimeFormat);
                    _response.Add(_ddrvw);
                }
                return _response;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> AdminDriverSendVetNotifications(int driverId)
        {
            try
            {
                var _driver = _context.DriverDetails.Where(x => x.DriverId == driverId).ToList();
                var _uTokens = _driver.Select(x => x.DeviceToken).ToList();
                var _status = _driver.FirstOrDefault().ApplicationStatus;
                var _reason = _driver.FirstOrDefault().RejectReason == null ? string.Empty : _driver.FirstOrDefault().RejectReason;

                if (driverId == 0 || _uTokens.Count == 0)
                {
                    return 0;
                }

                if (_uTokens.Count() > 0 && _status > 0)
                {
                    string title = NotificationTitle;
                    string body = _status == (int)ApplicationStatus.Verified ? NewDriverAcceptedNotificationText : NewDriverRejectedNotificationText + _reason;

                    var _sent = await _notificationService.SendDriverPushNotifications(_uTokens.ToArray(), title, body, string.Empty);

                    if (_sent)
                    {
                        Notifications _notif = new Notifications
                        {
                            Text = body,
                            CreatedOn = DateTime.UtcNow,
                            ToUserId = string.Empty,
                            ToDriverId = driverId,
                            Type = _status == (int)ApplicationStatus.Verified ? (int)NotificationTypes.ADMIN_APPROVED : (int)NotificationTypes.ADMIN_REJECTED,
                            IsRead = false,
                            IsDeleted = false
                        };

                        return await _notificationService.SavePushNotifications(_notif);
                    }

                    return 0;
                }

                return 0;
            }
            catch
            {
                throw;
            }
        }
        //send save push notification when driver cancel order
        public async Task<int> AdminUserSendVetNotifications(string userId)
        {
            try
            {
                var _user = _context.UserDetails.Where(x => x.Id == userId).ToList();
                var _uTokens = _user.Select(x => x.DeviceToken).ToList();
                var _status = _user.FirstOrDefault().ApplicationStatus;
                int _type = _status == (int)ApplicationStatus.Verified ? (int)NotificationTypes.ADMIN_APPROVED : (int)NotificationTypes.ADMIN_REJECTED;
                var _reason = _user.FirstOrDefault().RejectReason == null ? string.Empty : _user.FirstOrDefault().RejectReason;

                if (_uTokens.Count() > 0)
                {
                    string title = NotificationTitle;

                    string body = _status == (int)ApplicationStatus.Verified ? NewUserAcceptedNotificationText : NewUserRejectedNotificationText + _reason;

                    var _sent = await _notificationService.SendUserPushNotifications(_uTokens.ToArray(), title, body, string.Empty);

                    if (_sent)
                    {
                        Notifications _notif = new Notifications
                        {
                            Text = body,
                            CreatedOn = DateTime.UtcNow,
                            ToUserId = userId,
                            ToDriverId = 0,
                            Type = _type,
                            IsRead = false,
                            IsDeleted = false
                        };

                        return await _notificationService.SavePushNotifications(_notif);
                    }

                    return 0;
                }

                return 0;
            }
            catch
            {
                throw;
            }
        }
    }
}
