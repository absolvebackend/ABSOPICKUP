using _AbsoPickUp.Common;
using _AbsoPickUp.Data;
using _AbsoPickUp.IServices;
using _AbsoPickUp.Models;
using _AbsoPickUp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static _AbsoPickUp.Common.GlobalVariables;

namespace _AbsoPickUp.Repositories
{
    public class DriverService : IDriverService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserDetails> _userManager;
        private readonly IRequestService _requestService;
        public DriverService(UserManager<UserDetails> userManager, ApplicationDbContext context, IRequestService requestService)
        {
            _context = context;
            _userManager = userManager;
            _requestService = requestService;
        }
        //add driver personal info
        public bool AddDriverPersonalInfo(DriverDetails model)
        {
            try
            {
                _context.DriverDetails.Add(model);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw;
            }
        }
        //add driver documents
        public bool AddDriverDocuments(string profilepic, string dob, DriverDocuments model)
        {
            try
            {
                var response = _context.DriverDetails.FirstOrDefault(x => x.DriverId == model.DriverId);
                if (response != null)
                {
                    response.ProfilePic = profilepic;
                    response.DOB = dob;
                    response.ScreenId = (int)Screens.DriverDocuments;
                    _context.DriverDetails.Update(response);
                }
                _context.DriverDocuments.Add(model);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool AddDocuments(DriverDocuments model, DriverDocuments datamodel)
        {
            try
            {
                _context.DriverDocuments.Add(model);
                _context.DriverDocuments.Add(datamodel);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw;
            }
        }
        //get driver details by Id
        public async Task<DriverDetails> FindDriverById(string id)
        {
            try
            {
                var result = await (from dd in _context.DriverDetails
                                    where dd.DriverId.ToString() == id && dd.IsDeleted == false
                                    select dd).FirstOrDefaultAsync();
                return result;
            }
            catch
            {
                throw;
            }
        }
        //update driver details
        public bool UpdateAsync(DriverDetails model)
        {
            try
            {
                _context.DriverDetails.Update(model);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                throw;
            }
        }
        //find driver by phone number
        public DriverDetails FindByPhoneNumberAsync(string phoneNumber)
        {
            try
            {
                return _context.DriverDetails.FirstOrDefault(x => x.PhoneNumber.Trim() == phoneNumber);
            }
            catch
            {
                throw;
            }
        }
        //find driver by email
        public DriverDetails FindByEmailAddressAsync(string email)
        {
            try
            {
                return _context.DriverDetails.FirstOrDefault(x => x.Email == email);
            }
            catch
            {
                throw;
            }
        }
        //update driver device details at login, logout, reset password
        public bool UpdateDriver(string id, string devicetype, string devicetoken)
        {
            try
            {
                var result = _context.DriverDetails.Where(x => x.DriverId.ToString() == id && x.IsDeleted == false);
                if (result != null)
                {
                    var response = new DriverDetails
                    {
                        DeviceType = devicetype,
                        DeviceToken = devicetoken
                    };
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        //get driver profile info
        public DriverInfoViewModel GetDriverInfo(int driverId)
        {
            try
            {
                var _driverInfo = (from dd in _context.DriverDetails
                                   join vd in _context.VehicleDetails
                                   on dd.DriverId equals vd.DriverId into vdGroup
                                   from vd in vdGroup.DefaultIfEmpty()
                                   join vb in _context.VehicleBrand
                                   on vd.BrandId equals vb.BrandId into vbGroup
                                   from vb in vbGroup.DefaultIfEmpty()
                                   join vc in _context.VehicleColour
                                   on vd.VehicleColorId equals vc.VehicleColorId into vcGroup
                                   from vc in vcGroup.DefaultIfEmpty()
                                   join pro in _context.SouthAfricaProvinces
                                   on dd.ProvinceId equals pro.ProvinceId
                                   where dd.DriverId == driverId && dd.IsDeleted == false
                                   select new DriverInfoViewModel
                                   {
                                       Name = !string.IsNullOrEmpty(dd.FirstName + " " + dd.LastName) ? dd.FirstName + " " + dd.LastName : string.Empty,
                                       PhoneNumber = !string.IsNullOrEmpty(dd.PhoneNumber) ? dd.PhoneNumber : string.Empty,
                                       VehicleBrand = !string.IsNullOrEmpty(vb.BrandName) ? vb.BrandName : string.Empty,
                                       VehicleColor = !string.IsNullOrEmpty(vc.VehicleColorName) ? vc.VehicleColorName : string.Empty,
                                       RegistrationNumber = !string.IsNullOrEmpty(vd.RegisterationNumber) ? vd.RegisterationNumber : string.Empty,
                                       ProfilePic = !string.IsNullOrEmpty(dd.ProfilePic) ? dd.ProfilePic : string.Empty,
                                       DOB = !string.IsNullOrEmpty(dd.DOB) ? dd.DOB : string.Empty,
                                       Email = !string.IsNullOrEmpty(dd.Email) ? dd.Email : string.Empty,
                                       Address = !string.IsNullOrEmpty(dd.Address) ? dd.Address : string.Empty,
                                       City = !string.IsNullOrEmpty(dd.City) ? dd.City : string.Empty,
                                       Province = !string.IsNullOrEmpty(pro.Name) ? pro.Name : string.Empty,
                                       IsPhoneNumberConfirmed = dd.IsPhoneNumberConfirmed,
                                       CreatedDate = dd.CreatedDate == null ? string.Empty : dd.CreatedDate.GetValueOrDefault().AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateTimeFormat),
                                       Rating = GetAverageDriverRating(driverId),
                                       TodayEarnings = GetDriverTodayEarnings(driverId),
                                       VehicleTypeId = vd.VehicleTypeId,
                                       VehicleBrandId = vb.BrandId,
                                       VehicleColorId = vc.VehicleColorId,
                                       ProvinceId = pro.ProvinceId
                                   }).FirstOrDefault();

                var _driverAllDocs = _context.DriverDocuments.Where(x => x.DriverId == driverId).ToList();

                _driverInfo.DriverSelfieImgPath = _driverAllDocs.FirstOrDefault(x => x.DocTypeId == (int)DocTypes.Selfie) == null ? string.Empty : _driverAllDocs.FirstOrDefault(x => x.DocTypeId == (int)DocTypes.Selfie).DocImgPath;
                _driverInfo.DriverLicenseImgPath = _driverAllDocs.FirstOrDefault(x => x.DocTypeId == (int)DocTypes.License) == null ? string.Empty : _driverAllDocs.FirstOrDefault(x => x.DocTypeId == (int)DocTypes.License).DocImgPath;
                _driverInfo.DriverProofOfResidenceImgPath = _driverAllDocs.FirstOrDefault(x => x.DocTypeId == (int)DocTypes.ProofOfResidence) == null ? string.Empty : _driverAllDocs.FirstOrDefault(x => x.DocTypeId == (int)DocTypes.ProofOfResidence).DocImgPath;
                _driverInfo.VehicleRegisterationImgPath = _driverAllDocs.FirstOrDefault(x => x.DocTypeId == (int)DocTypes.VehicleRegisteration) == null ? string.Empty : _driverAllDocs.FirstOrDefault(x => x.DocTypeId == (int)DocTypes.VehicleRegisteration).DocImgPath;
                _driverInfo.VehicleFrontSideImgPath = _driverAllDocs.FirstOrDefault(x => x.DocTypeId == (int)DocTypes.VehicleFrontsideImage) == null ? string.Empty : _driverAllDocs.FirstOrDefault(x => x.DocTypeId == (int)DocTypes.VehicleFrontsideImage).DocImgPath;
                _driverInfo.VehicleBackSideImgPath = _driverAllDocs.FirstOrDefault(x => x.DocTypeId == (int)DocTypes.VehicleBacksideImage) == null ? string.Empty : _driverAllDocs.FirstOrDefault(x => x.DocTypeId == (int)DocTypes.VehicleBacksideImage).DocImgPath;

                return _driverInfo;
            }
            catch
            {
                throw;
            }
        }
        //get driver earnings for today
        public double GetDriverTodayEarnings(int driverId)
        {
            double todayEarnings = 0;

            try
            {
                var startOfDay = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0);
                var TodayOrders = _context.Orders.Where(x => x.DriverId == driverId && x.OrderStatus == ((int)OrderStatus.Complete) && x.CreatedDateTime >= startOfDay).ToList();

                //Today Earnings
                todayEarnings = TodayOrders.Join(_context.DeliveryRequest, u => u.RequestId, uir => uir.RequestId,
                (u, uir) => new { u, uir }).Join(_context.DeliveryPrice, r => r.uir.DeliveryTypeId, ro => ro.TypeId, (r, ro) => new { r, ro })
                .Sum(m => m.ro.Amount);

                return todayEarnings;
            }
            catch { throw; }
        }
        //get response for driver login
        public async Task<UserDetailsViewModel> GetDriverLoginResponse(string userId, string accessToken, string loginType)
        {
            try
            {
                var _vehicle = _context.VehicleDetails.FirstOrDefault(x => x.DriverId.ToString() == userId);
                var _vehTypeId = _vehicle == null ? 0 : _vehicle.VehicleTypeId;
                var _loginResponse = await _context.DriverDetails.Where(x => x.DriverId.ToString() == userId && x.IsDeleted == false)
                                      .Select(x => new UserDetailsViewModel
                                      {
                                          FirstName = x.FirstName,
                                          LastName = x.LastName,
                                          Email = x.Email,
                                          DialCode = x.DialCode,
                                          PhoneNumber = x.PhoneNumber,
                                          IsPhoneNoVerified = x.IsPhoneNumberConfirmed,
                                          ScreenId = x.ScreenId,
                                          accessToken = accessToken,
                                          LoginType = loginType,
                                          ProfilePic = x.ProfilePic,
                                          ApplicationStatusId = x.ApplicationStatus,
                                          ApplicationStatus = ((ApplicationStatus)x.ApplicationStatus).ToString(),
                                          VehicleTypeId = _vehTypeId,
                                          AppStatusMessage = CommonFunctions.GetAppStatusMessage(x.ApplicationStatus, x.RejectReason)
                                      }).FirstOrDefaultAsync();
                return _loginResponse;
            }
            catch
            {
                throw;
            }
        }

        //get request details for driver
        public AvailableRequestViewModel GetAvailableRequestDetails(int noticeId)
        {
            try
            {
                //get notification details
                var notification = _context.ParcelNotifications.FirstOrDefault(x => x.NotificationId == noticeId);
                //get request for which notification created
                var request = _context.DeliveryRequest.FirstOrDefault(x => x.RequestId == notification.RequestId);
                //get delivery type details
                var _deliveryType = _context.DeliveryTypes.FirstOrDefault(x => x.DeliveryTypeId == request.DeliveryTypeId);
                var _price = _context.DeliveryPrice.FirstOrDefault(x => x.TypeId == request.DeliveryTypeId).Amount;
                //create response object
                AvailableRequestViewModel dmr = new AvailableRequestViewModel();
                var result = CommonFunctions.GoogleDistanceMatrixAPILatLon(request.SenderLat.ToString(), request.SenderLong.ToString(), request.ReceiverLat.ToString(), request.ReceiverLong.ToString());
                dmr.SenderAddress = request.SenderAddress;
                dmr.ReceiverAddress = request.ReceiverAddress;
                dmr.Distance = result.element.Status != "ZERO_RESULTS" ? result.element.Distance.Text : string.Empty;
                dmr.Duration = result.element.Status != "ZERO_RESULTS" ? result.element.Duration.Text : string.Empty;
                dmr.DeliveryType = _deliveryType.Description;
                dmr.DeliveryPrice = _price;
                dmr.RequestId = request.RequestId;
                //Add Request Later DateTime details
                if (request.RequestDateTime != DateTime.Parse("0001-01-01 00:00:00") && request.RequestDateTime > DateTime.UtcNow)
                {
                    dmr.RequestDate = request.RequestDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateFormat);
                    dmr.RequestTime = request.RequestDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultTimeFormat);
                }
                else
                {
                    dmr.RequestDate = request.CreatedDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateFormat);
                    dmr.RequestTime = request.CreatedDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultTimeFormat);
                }
                //set notification sent with datetime fields
                notification.IsNotificationSent = true;
                notification.NotifySentDateTime = DateTime.UtcNow;
                _context.ParcelNotifications.Update(notification);
                _context.SaveChanges();

                return dmr;
            }
            catch
            {
                throw;
            }
        }
        //get all unassigned parcels or new requests
        public UnAssignedRequestsViewModel GetAllUnassignedParcels()
        {
            try
            {
                UnAssignedRequestsViewModel uarvw = new UnAssignedRequestsViewModel
                {
                    NewUnAssignedRequests = new List<DeliveryDetails>(),
                    UnAssignedRequestsToday = new List<DeliveryDetails>()
                };
                //get datetime over which to search for requests
                var now = DateTime.UtcNow;
                var startOfDay = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                var checkForDateTime = now.AddMinutes(-1 * notifyDriverRequestSentTimeInMins);
                var unassignedRequestIds = _context.DeliveryDetails.Where(x => x.DeliveryDateTime > checkForDateTime).Select(x => x.RequestId).Distinct().ToList();
                //from new requests today
                foreach (int requestId in unassignedRequestIds)
                {
                    //check status
                    var StatusDetail = _context.DeliveryDetails.Where(x => x.RequestId == requestId).OrderByDescending(x => x.DeliveryDateTime).FirstOrDefault();
                    if (StatusDetail.StatusId == (int)ParcelStatus.Unassigned)
                    {
                        uarvw.NewUnAssignedRequests.Add(StatusDetail);
                    }
                }
                return uarvw;
            }
            catch
            {
                throw;
            }
        }

        public UnAssignedRequestsViewModel GetAllOpenUnassignedParcels()
        {
            try
            {
                UnAssignedRequestsViewModel uarvw = new UnAssignedRequestsViewModel
                {
                    NewUnAssignedRequests = new List<DeliveryDetails>(),
                    UnAssignedRequestsToday = new List<DeliveryDetails>()
                };
                //get datetime over which to search for requests
                var now = DateTime.UtcNow;
                var startOfDay = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                var checkForDateTime = now.AddMinutes(-1 * notifyDriverOpenLastMins);
                var unassignedRequestIds = _context.DeliveryDetails.Where(x => x.DeliveryDateTime > startOfDay && x.DeliveryDateTime < checkForDateTime).Select(x => x.RequestId).Distinct().ToList();
                //from new requests today
                foreach (int requestId in unassignedRequestIds)
                {
                    //check status
                    var StatusDetail = _context.DeliveryDetails.Where(x => x.RequestId == requestId).OrderByDescending(x => x.DeliveryDateTime).FirstOrDefault();
                    if (StatusDetail.StatusId == (int)ParcelStatus.Unassigned)
                    {
                        uarvw.UnAssignedRequestsToday.Add(StatusDetail);
                    }
                }
                return uarvw;
            }
            catch
            {
                throw;
            }
        }
        //save driver eligible for request
        public async Task<int> SaveNoticeForDriver(DriverPositionViewModel driverPosition)
        {
            try
            {
                //get all unassigned parcels 
                var now = DateTime.UtcNow;
                var startOfDay = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                var allUnAssignedParcels = GetAllUnassignedParcels();
                foreach (var notAssigned in allUnAssignedParcels.NewUnAssignedRequests)
                {
                    //check if driver already added
                    var _IsExists = _context.ParcelNotifications.Where(x => x.DriverId == driverPosition.driver_id && x.RequestId == notAssigned.RequestId).ToList().Count;
                    //check parcel status
                    var _parcelStatus = _context.DeliveryDetails.Where(x => x.RequestId == notAssigned.RequestId).OrderByDescending(x => x.DeliveryDateTime).FirstOrDefault().StatusId;
                    //check driver status
                    var _driverStatus = GetDriverWorkStatus(driverPosition.driver_id, startOfDay, now).FirstOrDefault() != null && GetDriverWorkStatus(driverPosition.driver_id, startOfDay, now).FirstOrDefault().OnlineStatus;
                    //check isServiceProvider
                    var newRequest = _context.DeliveryRequest.FirstOrDefault(x => x.RequestId == notAssigned.RequestId);
                    var _vehDetails = _context.VehicleDetails.FirstOrDefault(x => x.DriverId == driverPosition.driver_id);
                    var driverVehicleType = _vehDetails == null ? 0 : _vehDetails.VehicleTypeId;
                    bool IsServiceProvider = false;
                    //match parcel and vehicle type 
                    if (driverVehicleType > 0 && driverVehicleType == (int)ParcelType.Bakkie && newRequest.DeliveryTypeId == (int)ParcelType.Bakkie)
                    {
                        IsServiceProvider = true;
                    }
                    else if (driverVehicleType > 0 && driverVehicleType != (int)ParcelType.Bakkie && newRequest.DeliveryTypeId != (int)ParcelType.Bakkie)
                    {
                        IsServiceProvider = true;
                    }

                    //if driver not already registered and parcel unassigned and driver online and IsServiceprovider
                    if (_IsExists == 0 && _parcelStatus == ((int)(ParcelStatus.Unassigned)) && _driverStatus && IsServiceProvider)
                    {
                        //get driver distance from request
                        var result = CommonFunctions.GetDistanceFromLatLong(Convert.ToDouble(driverPosition.lat.ToString()), Convert.ToDouble(driverPosition.lon.ToString()), Convert.ToDouble(newRequest.SenderLat.ToString()), Convert.ToDouble(newRequest.SenderLong.ToString()), 'K');
                        var DistanceToPickUpLoc = Math.Ceiling(result);
                        //register driver if applicable
                        if (DistanceToPickUpLoc < notifyDriverWithinKms && IsServiceProvider)
                        {
                            ParcelNotifications parnot = new ParcelNotifications
                            {
                                DriverId = driverPosition.driver_id,
                                DriverLat = driverPosition.lat,
                                DriverLon = driverPosition.lon,
                                RequestId = newRequest.RequestId,
                                CreateDateTime = DateTime.UtcNow
                            };
                            _context.ParcelNotifications.Add(parnot);
                        }
                    }
                }
                var res = await _context.SaveChangesAsync();
                return res;
            }
            catch
            {
                throw;
            }
        }

        public List<AvailableRequestViewModel> GetRequestNoticeForTopDrivers(int driverId)
        {
            try
            {
                AvailableRequestViewModel dmr = new AvailableRequestViewModel();
                List<AvailableRequestViewModel> listAvailReq = new List<AvailableRequestViewModel>();
                List<ParcelNotifications> OpenParcelRequests = new List<ParcelNotifications>();
                List<int> UnassignedRequestIds = new List<int>();
                var now = DateTime.UtcNow;
                var startOfDay = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                var checkForDateTime = now.AddMinutes(-1 * notifyDriverRequestSentTimeInMins);
                var _driverWorkStatus = GetDriverWorkStatus(driverId, startOfDay, now).FirstOrDefault();
                var _driverStatus = _driverWorkStatus != null && _driverWorkStatus.OnlineStatus;
                //send driver request details if online
                if (!_driverStatus)
                {
                    return listAvailReq;
                }
                //Get All entries to Parcel Notifications in last 10 mins
                OpenParcelRequests = _context.ParcelNotifications.Where(x => x.CreateDateTime >= checkForDateTime && !x.Accepted).ToList();

                //get unassigned requestIds
                UnassignedRequestIds.AddRange(GetAllUnassignedParcels().NewUnAssignedRequests.Select(x => x.RequestId).ToList());

                //get all drivers with ratings for the request
                foreach (var reqId in UnassignedRequestIds)
                {
                    int NoOfDriversNotified = OpenParcelRequests.Where(x => x.RequestId == reqId && x.IsNotificationSent == true).ToList().Count;
                    int NoOfDriversToNotify = notifyNoOfTopDrivers - NoOfDriversNotified;
                    //check if current driver notified
                    bool IsCurrentDriverNotified = (OpenParcelRequests.Where(x => x.RequestId == reqId && x.IsNotificationSent == true && x.DriverId == driverId).ToList().Count > 0);

                    if (!((NoOfDriversToNotify == 0) || IsCurrentDriverNotified))
                    {
                        List<ParcelNotificationsViewModel> listPNot = new List<ParcelNotificationsViewModel>();
                        //get all drivers added for request
                        List<int> driverIds = OpenParcelRequests.Where(x => x.RequestId == reqId && x.IsNotificationSent != true).Select(x => x.DriverId).ToList();
                        //List drivers with ratings and notification data
                        var top3PNot = OpenParcelRequests.Where(x => driverIds.Contains(x.DriverId) && x.RequestId == reqId).Select(x => new ParcelNotificationsViewModel()
                        {
                            NotificationId = x.NotificationId,
                            DriverId = x.DriverId,
                            DriverRatings = GetAverageDriverRating(x.DriverId)
                        }).ToList().OrderByDescending(x => x.DriverRatings).ToList().Take(NoOfDriversToNotify);

                        foreach (var pNot in top3PNot)
                        {
                            //If current driver in top drivers, send notification.
                            if (pNot.DriverId == driverId)
                            {
                                dmr = GetAvailableRequestDetails(pNot.NotificationId);
                                listAvailReq.Add(dmr);
                            }
                        }
                    }
                }
                return listAvailReq;
            }
            catch { throw; }
        }

        public List<AvailableRequestViewModel> GetAllOpenRequestNotice(int driverId)
        {
            try
            {
                List<AvailableRequestViewModel> listARVM = new List<AvailableRequestViewModel>();
                var _vehDetails = _context.VehicleDetails.FirstOrDefault(x => x.DriverId == driverId);
                var _vehTypeId = _vehDetails == null ? 0 : _vehDetails.VehicleTypeId;

                //get unassigned requests 
                var allUnAssignedRequestIds = GetAllOpenUnassignedParcels().UnAssignedRequestsToday.Select(x => x.RequestId);
                //get requests already assigned to driver once
                var allRequestIdsAssignedToDriver = GetAcceptedRequestsTodayForDriver(driverId);
                //get open requests
                var openRequestIds = allUnAssignedRequestIds.Except(allRequestIdsAssignedToDriver).ToList();

                var _allOpenRequests = _context.DeliveryRequest.Where(x => openRequestIds.Contains(x.RequestId))
                    .Join(_context.DeliveryTypes, dr => dr.DeliveryTypeId, dt => dt.DeliveryTypeId, (dr, dt) => new { dr, dt })
                    .Join(_context.DeliveryPrice, dts => dts.dt.DeliveryTypeId, dp => dp.TypeId, (dts, dp) => new AvailableRequestViewModel()
                    {
                        SenderAddress = dts.dr.SenderAddress,
                        ReceiverAddress = dts.dr.ReceiverAddress,
                        RequestId = dts.dr.RequestId,
                        DeliveryType = dts.dt.Description,
                        DeliveryPrice = dp.Amount,
                        RequestDate = dts.dr.CreatedDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateFormat),
                        RequestTime = dts.dr.CreatedDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultTimeFormat),
                        DeliveryTypeId = dts.dr.DeliveryTypeId,
                        SenderLatitude = dts.dr.SenderLat.ToString(),
                        SenderLongitude = dts.dr.SenderLat.ToString(),
                        ReceiverLatitude = dts.dr.ReceiverLat.ToString(),
                        ReceiverLongitude = dts.dr.ReceiverLong.ToString(),
                        Distance = dts.dr.TotalDeliveryDistance,
                        Duration = dts.dr.TotalDeliveryTime
                    }).ToList();

                foreach (var item in _allOpenRequests)
                {
                    if (_vehTypeId == (int)ParcelType.Bakkie && item.DeliveryTypeId == (int)ParcelType.Bakkie)
                    {
                        listARVM.Add(item);
                    }
                    else if (_vehTypeId != (int)ParcelType.Bakkie && item.DeliveryTypeId != (int)ParcelType.Bakkie)
                    {
                        listARVM.Add(item);
                    }
                }

                return listARVM.Count > 0 ? listARVM.OrderByDescending(x => x.RequestDate).ToList() : null;

            }
            catch { throw; }
        }

        public async Task<int> DriverResponseToNewRequest(AssignRequestViewModel reqDetails)
        {
            try
            {
                var pNot = new ParcelNotifications();
                pNot = _context.ParcelNotifications.FirstOrDefault(x => x.DriverId == reqDetails.DriverId && x.RequestId == reqDetails.RequestId);
                //if driver accepts when notified as top rated
                if (pNot != null && reqDetails.hasAccepted == false)
                {
                    //request rejected by driver

                    pNot.Accepted = reqDetails.hasAccepted;
                    _context.ParcelNotifications.Update(pNot);
                    return await _context.SaveChangesAsync();
                }
                else
                {
                    //driver notification created for open requests

                    pNot = new ParcelNotifications
                    {
                        Accepted = false,
                        CreateDateTime = DateTime.UtcNow,
                        DriverId = reqDetails.DriverId,
                        RequestId = reqDetails.RequestId,
                        DriverLat = reqDetails.DriverLat,
                        DriverLon = reqDetails.DriverLong,
                        IsNotificationSent = true,
                        NotifySentDateTime = DateTime.UtcNow
                    };
                    _context.ParcelNotifications.Add(pNot);
                    _context.SaveChanges();
                    if (!reqDetails.hasAccepted)
                    {
                        //driver has rejected open request
                        return 1;
                    }
                }

                //request accepted by driver
                var _status = (int)ParcelStatus.Assigned;
                var _cancel = (int)ParcelStatus.Cancelled;

                //Request has been cancelled by user
                var _chkStatus = _context.DeliveryDetails.Where(x => x.RequestId == reqDetails.RequestId).OrderByDescending(x => x.DeliveryDateTime).ToList();
                var _requestCancelRows = _chkStatus.Where(x => x.StatusId == _cancel).Count();
                var _isUnAssigned = (_chkStatus.Take(1).Select(x => x.StatusId).FirstOrDefault() == (int)ParcelStatus.Unassigned);
                if (_requestCancelRows > 0 || !_isUnAssigned)
                {
                    return 0;
                }
                var _ReqAssignedRowsCount = _context.DeliveryDetails.Where(x => x.RequestId == reqDetails.RequestId && x.StatusId == _status && x.DriverId == reqDetails.DriverId).ToList().Count;

                //update delivery details and notification
                if (_ReqAssignedRowsCount == 0 && pNot != null && !pNot.Accepted)
                {
                    DeliveryDetails ddt = new DeliveryDetails
                    {
                        StatusId = _status,
                        DeliveryDateTime = DateTime.UtcNow,
                        DriverId = reqDetails.DriverId,
                        DriverLat = reqDetails.DriverLat,
                        DriverLong = reqDetails.DriverLong,
                        RequestId = reqDetails.RequestId
                    };
                    var newReq = _context.DeliveryDetails.Add(ddt);

                    pNot.Accepted = reqDetails.hasAccepted;
                    _context.ParcelNotifications.Update(pNot);
                    return await _context.SaveChangesAsync();
                }
                return 0;
            }
            catch
            {
                throw;
            }
        }
        //get average driver rating
        public double GetAverageDriverRating(int driverId)
        {
            try
            {
                var ratings = _context.DriverRatings.Where(x => x.DriverId == driverId).ToList();
                var rating = ratings.Count > 0 ? ratings.Average(x => x.Ratings) : 0;
                return Math.Round(rating, 1);
            }
            catch { throw; }
        }
        //get all driver ratings details
        public DriverRatingViewModel GetDriverRating(int driverId)
        {
            try
            {
                DriverRatingViewModel drvw = new DriverRatingViewModel
                {
                    ReviewList = new List<DriverReviewViewModel>()
                };

                var reviews = (from dri in _context.DriverRatings
                               join user in _context.UserDetails
                               on dri.UserId equals user.Id into userGroup
                               from usrG in userGroup
                               where dri.DriverId == driverId
                               select new DriverReviewViewModel()
                               {
                                   UserName = usrG.FirstName + " " + usrG.LastName,
                                   CommentedAt = dri.CommentedAt.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateFormat),
                                   Review = dri.UserComment,
                                   Rating = dri.Ratings
                               }).ToList();

                drvw.DriverId = driverId;
                drvw.UserRating = GetAverageDriverRating(driverId);
                drvw.ReviewList.AddRange(reviews);
                return drvw;
            }
            catch { throw; }
        }
        //get driver online or offline
        public List<DriverWorkStatus> GetDriverWorkStatus(int driverId, DateTime start, DateTime finish)
        {
            try
            {
                var status = _context.DriverWorkStatus.Where(x => x.DriverId == driverId && x.StatusDateTime >= start && x.StatusDateTime <= finish).OrderByDescending(x => x.StatusDateTime).ToList();
                return status;
            }
            catch { throw; }
        }

        public bool GetDriverCurrentWorkStatus(int driverId)
        {
            try
            {
                return _context.DriverWorkStatus.Where(x => x.DriverId == driverId).OrderByDescending(x => x.StatusDateTime).Take(1).Select(x => x.OnlineStatus).FirstOrDefault();
            }
            catch { throw; }
        }
        //update driver work status
        public async Task<int> AddDriverWorkStatus(int driverId, bool Online)
        {
            try
            {
                DriverWorkStatus dws = new DriverWorkStatus
                {
                    DriverId = driverId,
                    OnlineStatus = Online,
                    StatusDateTime = DateTime.UtcNow
                };

                _context.DriverWorkStatus.Add(dws);
                var status = await _context.SaveChangesAsync();
                return status;
            }
            catch { throw; }
        }
        //add ratings for driver
        public int SaveDriverRating(int OrderId, DriverRatings rating)
        {
            try
            {
                var _order = _context.Orders.FirstOrDefault(x => x.OrderId == OrderId);
                if (_order.IsDriverRated) { return 0; }
                _order.IsDriverRated = true;
                _context.Orders.Update(_order);
                _context.DriverRatings.Add(rating);
                var res = _context.SaveChanges();
                return res;
            }
            catch { throw; }
        }
        //get all requests accepted by driver
        public List<DriverAcceptedRequests> GetDriverAcceptedRequest(int driverId)
        {
            try
            {
                var _all = GetRequestsByDriverId(driverId);

                var _accepted = _all.Where(x => x.Status == (ParcelStatus.Assigned).ToString()).ToList();

                return _accepted.Count > 0 ? _accepted.OrderByDescending(x => x.DeliveryTypeId).OrderBy(x => x.DistanceValue).ToList() : null;
            }
            catch { throw; }
        }
        //get requests details for driver
        public List<DriverAcceptedRequests> GetRequestsByDriverId(int driverId)
        {
            try
            {
                var allNotices = _context.ParcelNotifications.Where(x => x.DriverId == driverId && x.Accepted).ToList();

                var allRequests = (from not in allNotices
                                   join cdr in _context.DeliveryRequest
                                   on not.RequestId equals cdr.RequestId
                                   join ord in _context.Orders
                                   on not.RequestId equals ord.RequestId into ordGroup
                                   from ordg in ordGroup
                                   where ordg.DriverId == driverId
                                   join usr in _context.UserDetails
                                   on cdr.SenderId equals usr.Id into usrGroup
                                   from ussr in usrGroup
                                   join cdt in _context.DeliveryTypes
                                   on cdr.DeliveryTypeId equals cdt.DeliveryTypeId into cdtGroup
                                   from cdtg in cdtGroup
                                   join cdp in _context.DeliveryPrice
                                   on cdr.DeliveryTypeId equals cdp.Id into cdpGroup
                                   from cdpg in cdpGroup
                                   select new DriverAcceptedRequests()
                                   {
                                       RequestId = not.RequestId,
                                       OrderId = ordg == null ? 0 : ordg.OrderId,
                                       DialCode = cdr == null ? "" : cdr.DialCode.ToString(),
                                       SenderPhoneNumber = ussr == null ? "" : ussr.PhoneNumber,
                                       ReceiverPhoneNumber = cdr == null ? "" : cdr.ReceiverMobileNumber,
                                       SenderAddress = cdr == null ? "" : cdr.SenderAddress,
                                       SenderLat = cdr == null ? 0 : cdr.SenderLat,
                                       SenderLong = cdr == null ? 0 : cdr.SenderLong,
                                       ReceiverAddress = cdr == null ? "" : cdr.ReceiverAddress,
                                       ReceiverLat = cdr == null ? 0 : cdr.ReceiverLat,
                                       ReceiverLong = cdr == null ? 0 : cdr.ReceiverLong,
                                       Distance = cdr == null ? string.Empty : cdr.TotalDeliveryDistance,
                                       DistanceValue = Convert.ToDouble(cdr.TotalDeliveryDistance == null ? string.Empty : cdr.TotalDeliveryDistance[0..^3]),
                                       Duration = string.IsNullOrEmpty(cdr.TotalDeliveryTime) ? string.Empty : cdr.TotalDeliveryTime,
                                       DeliveryType = cdtg == null ? string.Empty : cdtg.Description,
                                       DeliveryTypeId = cdr == null ? 0 : cdr.DeliveryTypeId,
                                       DeliveryPrice = cdr == null ? 0 : cdpg.Amount,
                                       OrderDate = ordg == null ? string.Empty : ordg.CreatedDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateFormat),
                                       OrderTime = ordg == null ? string.Empty : ordg.CreatedDateTime.AddHours(addHoursToUTCDatetimeForSA).ToShortTimeString(),
                                       Status = ((ParcelStatus)(_context.DeliveryDetails.Where(x => x.RequestId == not.RequestId).ToList().OrderByDescending(x => x.DeliveryDateTime).Take(1).Select(x => x.StatusId).FirstOrDefault())).ToString()
                                   }).ToList();

                return allRequests;
            }
            catch { throw; }
        }
        //get request details
        public RequestDetailsViewModel GetRequestDetails(int requestId)
        {
            try
            {
                RequestDetailsViewModel ddvm = new RequestDetailsViewModel();
                var request = _context.DeliveryRequest.FirstOrDefault(x => x.RequestId == requestId);
                if (request != null)
                {
                    var price = _context.DeliveryPrice.FirstOrDefault(x => x.TypeId == request.DeliveryTypeId);
                    var sender = _userManager.FindByIdAsync(request.SenderId).Result;

                    ddvm.SenderName = sender == null ? string.Empty : sender.FirstName + " " + sender.LastName;
                    ddvm.SenderPhone = sender == null ? string.Empty : sender.PhoneNumber;
                    ddvm.SenderAddress = request.SenderAddress;
                    ddvm.SenderLat = request.SenderLat;
                    ddvm.SenderLong = request.SenderLong;
                    ddvm.ReceiverAddress = request.ReceiverAddress;
                    ddvm.ReceiverLat = request.ReceiverLat;
                    ddvm.ReceiverLong = request.ReceiverLong;
                    ddvm.ReceiverName = request.ReceiverName;
                    ddvm.ReceiverMobileNumber = request.ReceiverMobileNumber;
                    ddvm.ReceiverEmail = request.ReceiverEmail;
                    ddvm.TotalDeliveryDistance = request.TotalDeliveryDistance;
                    ddvm.TotalDeliveryTime = request.TotalDeliveryTime;
                    ddvm.DialCode = request.DialCode;
                    ddvm.DeliveryPrice = price == null ? 0 : price.Amount;
                    ddvm.DeliveryTypeId = request.DeliveryTypeId;
                    ddvm.DeliveryType = ((ParcelType)request.DeliveryTypeId).ToString();

                }

                return ddvm;
            }
            catch { throw; }
        }
        //create order when driver accepts request
        public async Task<Orders> CreateOrderForAcceptedRequest(int requestId, int driverId)
        {
            try
            {
                var _anyExistingOrder = false;
                var _request = _context.DeliveryRequest.FirstOrDefault(x => x.RequestId == requestId);
                //parcel is assigned to this driver
                var IsAssignedToDriver = _context.DeliveryDetails.FirstOrDefault(x => x.RequestId == requestId && x.DriverId == driverId && x.StatusId == (int)ParcelStatus.Assigned);

                //all previous orders are cancelled
                var _prevOrders = _context.Orders.Where(x => x.RequestId == requestId).ToList();
                foreach (var item in _prevOrders)
                {
                    if (item.OrderStatus != (int)OrderStatus.Cancelled)
                    {
                        _anyExistingOrder = true;
                    }
                }

                if (IsAssignedToDriver != null && !_anyExistingOrder)
                {
                    Orders _order = new Orders
                    {
                        RequestId = requestId,
                        SenderId = _request.SenderId,
                        OrderStatus = (int)OrderStatus.Pending,
                        DriverId = driverId,
                        CreatedDateTime = DateTime.UtcNow,
                        AcceptedDateTime = DateTime.UtcNow
                    };
                    _context.Orders.Add(_order);
                    await _context.SaveChangesAsync();

                    return _order;
                }
                return null;
            }
            catch { throw; }
        }
        //get device token for sender for push notification
        public List<string> GetSenderDeviceTokens(int orderId)
        {
            try
            {
                List<string> tokens = new List<string>();
                var order = _context.Orders.FirstOrDefault(x => x.OrderId == orderId);
                var sender = _userManager.FindByIdAsync(order.SenderId).Result;
                tokens.Add(sender.DeviceToken);
                return tokens;
            }
            catch { throw; }
        }
        //get device token for driver for push notification
        public List<string> GetDriverDeviceTokens(int orderId)
        {
            try
            {
                List<string> tokens = new List<string>();
                var order = _context.Orders.FirstOrDefault(x => x.OrderId == orderId);
                var driverToken = _context.DriverDetails.FirstOrDefault(x => x.DriverId == order.DriverId).DeviceToken;
                tokens.Add(driverToken);
                return tokens;
            }
            catch { throw; }
        }
        //update order status
        public async Task<int> UpdateOrderStatus(int orderId, int statusId)
        {
            try
            {
                var _order = _context.Orders.FirstOrDefault(x => x.OrderId == orderId);
                if (_order != null && statusId > 0)
                {
                    _order.OrderStatus = statusId;
                    _context.Orders.Update(_order);
                    return await _context.SaveChangesAsync();
                }
                return 0;
            }
            catch { throw; }
        }
        //verify pin for delivery
        public async Task<ParcelDelivery> VerifyDeliveryPIN(VerifyPINViewModel model)
        {
            var updates = new DeliveryDetails();
            int _res = 0;
            int _ba = 0;

            try
            {
                var _verify = _context.ParcelDelivery.FirstOrDefault(x => x.OrderId == model.OrderId && x.DriverId == model.DriverId);
                var request = _context.DeliveryRequest.FirstOrDefault(x => x.RequestId == _verify.RequestId);
                var IsCorrectPIN = (model.ReceiverPhoneNo == _verify.ReceiverPhoneNo && model.DeliveryPIN == _verify.DeliveryPIN);

                if (_verify != null && _verify.PINId > 0)
                {
                    _verify.IsVerified = IsCorrectPIN;
                    _verify.VerifiedAt = DateTime.UtcNow;
                    _context.ParcelDelivery.Update(_verify);
                    _ba = await _context.SaveChangesAsync();

                    if (IsCorrectPIN)
                    {
                        updates.DriverId = _verify.DriverId;
                        updates.RequestId = _verify.RequestId;
                        updates.StatusId = (int)(ParcelStatus.Delivered);
                        updates.DriverLat = request.ReceiverLat;
                        updates.DriverLong = request.ReceiverLong;
                        updates.DeliveryDateTime = DateTime.UtcNow;

                        _res = await _requestService.AddDeliveryDetails(updates);
                    }
                }

                if (_res == 2 && _ba > 0)
                {
                    return _verify;
                }
                return null;
            }
            catch
            {
                throw;
            }
        }
        //check if driver email registered
        public bool IsEmailRegistered(string email)
        {
            bool _IsEmailRegistered = false;
            var NoOfUsers = _context.DriverDetails.Where(x => x.Email == email).ToList().Count;
            if (NoOfUsers > 0)
            {
                _IsEmailRegistered = true;
            }
            return _IsEmailRegistered;
        }
        //check if driver phone registered
        public bool IsPhoneRegistered(string phoneNumber)
        {
            bool _IsPhoneRegistered = false;
            var NoOfUsers = _context.DriverDetails.Where(x => x.PhoneNumber == phoneNumber).ToList().Count;
            if (NoOfUsers > 0)
            {
                _IsPhoneRegistered = true;
            }
            return _IsPhoneRegistered;
        }
        //get parcel details for work in progress for driver 
        public List<DriverAcceptedRequests> GetDriverWorkInProgress(int driverId)
        {
            try
            {
                var _all = GetRequestsByDriverId(driverId);

                var _inProgressWork = _all.Where(x => x.Status == (ParcelStatus.WithDriver).ToString() || x.Status == (ParcelStatus.DeliveryOnRoute).ToString() || x.Status == (ParcelStatus.Arrived).ToString()).ToList();

                return _inProgressWork.Count > 0 ? _inProgressWork.OrderByDescending(x => x.DeliveryTypeId).OrderBy(x => x.DistanceValue).ToList() : null;
            }
            catch { throw; }
        }
        //get all delivered request details for driver
        public List<DriverAcceptedRequests> GetDriverCompletedRequests(int driverId)
        {
            try
            {
                var _all = GetRequestsByDriverId(driverId);

                var _delivered = _all.Where(x => x.Status == (ParcelStatus.Delivered).ToString()).ToList();

                return _delivered.Count > 0 ? _delivered.OrderByDescending(x => x.DeliveryTypeId).OrderBy(x => x.DistanceValue).ToList() : null;
            }
            catch { throw; }
        }
        //update application status for driver
        public async Task<int> SetDriverApplicationStatus(int driverId, int appStatus)
        {
            var _driver = _context.DriverDetails.FirstOrDefault(x => x.DriverId == driverId);
            _driver.ApplicationStatus = appStatus;
            _context.DriverDetails.Update(_driver);
            return await _context.SaveChangesAsync();
        }
        // add or update driver bank details
        public async Task<int> AddUpdateDriverBankDetails(DriverBankDetails model)
        {
            try
            {
                var _dApp = _context.DriverBankDetails.Where(x => x.AccountNumber == model.AccountNumber || x.AccId == model.AccId).FirstOrDefault();
                if (_dApp != null)
                {
                    _dApp.AccountName = model.AccountName;
                    _dApp.AccountNumber = model.AccountNumber;
                    _dApp.Bank = model.Bank;
                    _dApp.Branch = model.Branch;
                    _dApp.BranchCode = model.BranchCode;
                    _dApp.DriverId = model.DriverId;
                    _dApp.SwiftCode = model.SwiftCode;
                    _context.DriverBankDetails.Update(_dApp);
                }
                else
                {
                    _context.DriverBankDetails.Add(model);
                }
                var status = await _context.SaveChangesAsync();
                return status;
            }
            catch { throw; }
        }
        // get driver bank details
        public DriverBankDetails GetDriverBankDetails(int driverId)
        {
            try
            {
                var bankDetails = _context.DriverBankDetails.FirstOrDefault(x => x.DriverId == driverId);
                return bankDetails;
            }
            catch { throw; }
        }
        // get device token for driver for push notification
        public List<string> GetDeviceTokenForDriver(int driverId)
        {
            try
            {
                List<string> tokens = new List<string>();
                var driverToken = _context.DriverDetails.FirstOrDefault(x => x.DriverId == driverId).DeviceToken;
                tokens.Add(driverToken);
                return tokens;
            }
            catch { throw; }
        }
        //get doctypes for personal Id.
        public List<GetDocTypeViewModel> GetPersonalIdDocType()
        {
            try
            {

                var _docType = (from d in _context.DocumentTypes
                                where new[] {
                                    (int)DocTypes.IDBook,
                                    (int)DocTypes.Passport,
                                    (int)DocTypes.License,
                                    (int)DocTypes.ElectionCard
                                }.Contains(d.DocTypeId)
                                select new GetDocTypeViewModel
                                {
                                    DocTypeId = d.DocTypeId,
                                    DocTypeName = d.DocTypeDescription
                                }).ToList();
                return _docType;
            }
            catch
            {
                throw;
            }
        }
        public int ChangePassword(int driverId, string OldPassword, string NewPassword)
        {
            try
            {
                var opHash = CommonFunctions.EncryptPassword(OldPassword);
                var _driver = _context.DriverDetails.FirstOrDefault(x => x.DriverId == driverId && x.PasswordHash == opHash);
                if (_driver != null)
                {
                    _driver.PasswordHash = CommonFunctions.EncryptPassword(NewPassword);
                    _context.DriverDetails.Update(_driver);
                    return _context.SaveChanges();
                }
                return 0;
            }
            catch
            {
                throw;
            }
        }

        public List<int> GetAcceptedRequestsTodayForDriver(int driverId)
        {
            try
            {
                var now = DateTime.UtcNow;
                var startOfDay = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                var checkForDateTime = now.AddMinutes(-1 * notifyDriverOpenLastMins);
                var _status = (int)ParcelStatus.Assigned;
                var AssignedRequestIds = _context.DeliveryDetails.Where(x => x.DeliveryDateTime > startOfDay && x.DeliveryDateTime < checkForDateTime && x.StatusId == _status && x.DriverId == driverId).Select(x => x.RequestId).Distinct().ToList();
                return AssignedRequestIds;
            }
            catch { throw; }
        }
    }
}
