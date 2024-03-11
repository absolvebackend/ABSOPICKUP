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
    public class RequestService : IRequestService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserDetails> _userManager;
        private readonly INotificationService _notificationService;

        public RequestService(UserManager<UserDetails> userManager, ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }
        //add new request
        public async Task<DeliveryRequest> AddDeliveryRequest(DeliveryRequest model)
        {
            try
            {
                await _context.DeliveryRequest.AddAsync(model);
                await _context.SaveChangesAsync();
                return model;
            }
            catch
            {
                throw;
            }
        }
        //add status and driver position details for every request per parcel status
        public async Task<int> AddDeliveryDetails(DeliveryDetails model)
        {
            try
            {
                await _context.DeliveryDetails.AddAsync(model);
                var order = _context.Orders.FirstOrDefault(x => x.RequestId == model.RequestId);

                if (order != null && order.OrderId > 0)
                {
                    if (model.StatusId == (int)(ParcelStatus.DeliveryOnRoute))
                    {
                        order.OrderStatus = (int)(OrderStatus.Processing);
                        order.UpdatedDateTime = DateTime.UtcNow;
                        _context.Orders.Update(order);
                    }
                    else if (model.StatusId == (int)(ParcelStatus.Delivered))
                    {
                        order.OrderStatus = (int)(OrderStatus.Complete);
                        order.UpdatedDateTime = DateTime.UtcNow;
                        _context.Orders.Update(order);
                    }
                }

                return await _context.SaveChangesAsync(); ;
            }
            catch
            {
                throw;
            }
        }
        //get receiver phone number
        public string GetReceiverPhoneNo(int RequestId)
        {
            var request = _context.DeliveryRequest.FirstOrDefault(x => x.RequestId == RequestId);
            return "+" + request.DialCode + request.ReceiverMobileNumber;
        }
        public string GetReceiverAddress(int RequestId)
        {
            var request = _context.DeliveryRequest.FirstOrDefault(x => x.RequestId == RequestId);
            return request.ReceiverAddress;
        }
        //add parcel details
        public async Task<ParcelDetails> AddParcelDetails(ParcelDetails model)
        {
            try
            {
                var _parcel = _context.ParcelDetails.FirstOrDefault(x => x.RequestId == model.RequestId);
                if (_parcel != null)
                {
                    _parcel.RequestId = model.RequestId;
                    _parcel.ParcelName = model.ParcelName;
                    _parcel.ParcelNotes = model.ParcelNotes;
                    _parcel.ImgBeforePacking = model.ImgBeforePacking;
                    _parcel.ImgAfterPacking = model.ImgAfterPacking;
                    _context.ParcelDetails.Update(_parcel);
                }
                else
                {
                    await _context.ParcelDetails.AddAsync(model);
                }
                await _context.SaveChangesAsync();
                return model;
            }
            catch
            {
                throw;
            }
        }
        //get distance and time for delivery using place_id
        public Task<DistanceMatrixResponse> DistanceMatrixRequest(string source, string destination, int typeId)
        {
            try
            {
                var elemRES = CommonFunctions.GoogleDistanceMatrixAPI(source, destination);

                var dataModel = (from x in _context.DeliveryPrice
                                 join m in _context.DeliveryTypes
                                 on x.TypeId equals m.DeliveryTypeId
                                 where m.DeliveryTypeId == typeId
                                 select new DeliveryTypePriceViewModel()
                                 {
                                     TypeId = x.TypeId,
                                     DeliveryType = m.Description,
                                     Description = x.DeliverBy,
                                     Price = x.Amount
                                 }).FirstOrDefault();

                DistanceMatrixResponse dmr = new DistanceMatrixResponse
                {
                    Distance = elemRES.element.Distance == null ? string.Empty : elemRES.element.Distance.Text,
                    Duration = elemRES.element.Duration == null ? string.Empty : elemRES.element.Duration.Text,
                    DeliveryType = dataModel.DeliveryType,
                    Description = dataModel.Description,
                    Price = dataModel.Price
                };

                return Task.FromResult(dmr);
            }
            catch
            {
                throw;
            }
        }
        //get distance and time for delivery using lat, long
        public Task<DistanceMatrixResponse> DistanceMatrixRequestLatLon(string oLat, string oLon, string dLat, string dLon, int deliveryTypeId)
        {
            try
            {
                var elemRES = CommonFunctions.GoogleDistanceMatrixAPILatLon(oLat, oLon, dLat, dLon);

                var dataModel = (from x in _context.DeliveryPrice
                                 join m in _context.DeliveryTypes
                                 on x.TypeId equals m.DeliveryTypeId
                                 where m.DeliveryTypeId == deliveryTypeId
                                 select new DeliveryTypePriceViewModel()
                                 {
                                     TypeId = x.TypeId,
                                     DeliveryType = m.Description,
                                     Description = x.DeliverBy,
                                     Price = x.Amount
                                 }).FirstOrDefault();

                var dmr = new DistanceMatrixResponse
                {
                    Distance = elemRES.element.Distance == null ? string.Empty : elemRES.element.Distance.Text,
                    Duration = elemRES.element.Duration == null ? string.Empty : elemRES.element.Duration.Text,
                    DeliveryType = dataModel.DeliveryType,
                    Description = dataModel.Description,
                    Price = dataModel.Price
                };

                return Task.FromResult(dmr);
            }
            catch
            {
                throw;
            }
        }
        //get delivery price
        public async Task<List<DeliveryTypePriceViewModel>> GetPrice()
        {
            try
            {
                var dataModel = await (from x in _context.DeliveryPrice
                                       join m in _context.DeliveryTypes
                                       on x.TypeId equals m.DeliveryTypeId
                                       select new DeliveryTypePriceViewModel()
                                       {
                                           TypeId = x.TypeId,
                                           DeliveryType = m.Description,
                                           Description = x.DeliverBy,
                                           Price = x.Amount
                                       }).ToListAsync();
                return dataModel;
            }
            catch
            {
                throw;
            }
        }
        //get all details for request
        public DriverOrderViewModel GetRequestDetailsById(int requestId)
        {
            try
            {
                var _data = _context.DeliveryRequest.Where(x => x.RequestId == requestId).ToList();
                if (_data.Count == 0)
                {
                    return null;
                }
                DriverOrderViewModel dovm = new DriverOrderViewModel
                {
                    UOVM = new UserOrdersViewModel(),
                    ListDeliveryDetails = new List<DeliveryDetailsViewModel>(),
                    DriverInfo = new RequestDriverInfoViewModel()
                };

                var _user = _userManager.FindByIdAsync(_data.FirstOrDefault().SenderId).Result;

                var request = (from dr in _context.DeliveryRequest.Where(x => x.RequestId == requestId)
                               from org in _context.Orders.Where(x => x.RequestId == dr.RequestId && x.OrderStatus != (int)OrderStatus.Cancelled).DefaultIfEmpty()
                               join dreq in _context.DeliveryTypes
                               on dr.DeliveryTypeId equals dreq.DeliveryTypeId
                               join dpr in _context.DeliveryPrice
                               on dr.DeliveryTypeId equals dpr.TypeId
                               join paro in _context.ParcelDetails
                               on dr.RequestId equals paro.RequestId into ppGroup
                               from parc in ppGroup.DefaultIfEmpty()
                               select new UserOrdersViewModel
                               {
                                   RequestId = dr.RequestId,
                                   SourceAddress = string.IsNullOrEmpty(dr.SenderAddress) ? string.Empty : dr.SenderAddress,
                                   SourceLat = string.IsNullOrEmpty(dr.SenderLat.ToString()) ? string.Empty : dr.SenderLat.ToString(),
                                   SourceLong = string.IsNullOrEmpty(dr.SenderLong.ToString()) ? string.Empty : dr.SenderLong.ToString(),
                                   DestinationAddress = string.IsNullOrEmpty(dr.ReceiverAddress) ? string.Empty : dr.ReceiverAddress,
                                   DestinationLat = string.IsNullOrEmpty(dr.ReceiverLat.ToString()) ? string.Empty : dr.ReceiverLat.ToString(),
                                   DestinationLong = string.IsNullOrEmpty(dr.ReceiverLong.ToString()) ? string.Empty : dr.ReceiverLong.ToString(),
                                   DeliveryType = dreq == null ? string.Empty : dreq.Description,
                                   Price = dpr == null ? 0 : dpr.Amount,
                                   Duration = string.IsNullOrEmpty(dr.TotalDeliveryTime) ? string.Empty : dr.TotalDeliveryTime,
                                   Distance = string.IsNullOrEmpty(dr.TotalDeliveryDistance) ? string.Empty : dr.TotalDeliveryDistance,
                                   SenderName = _user == null ? string.Empty : _user.FirstName + " " + _user.LastName,
                                   SenderPhoneNo = _user == null ? string.Empty : _user.PhoneNumber,
                                   ReceiverName = string.IsNullOrEmpty(dr.ReceiverName) ? string.Empty : dr.ReceiverName,
                                   ReceiverEmail = string.IsNullOrEmpty(dr.ReceiverEmail) ? string.Empty : dr.ReceiverEmail,
                                   DialCode = dr.DialCode,
                                   ReceiverMobileNumber = string.IsNullOrEmpty(dr.ReceiverMobileNumber) ? string.Empty : dr.ReceiverMobileNumber,
                                   ParcelName = parc == null ? string.Empty : parc.ParcelName,
                                   ParcelNotes = parc == null ? string.Empty : parc.ParcelNotes,
                                   ParcelImgBefore = parc == null ? string.Empty : parc.ImgBeforePacking,
                                   ParcelImgAfter = parc == null ? string.Empty : parc.ImgAfterPacking,
                                   LastRequestStatusId = 0,
                                   LastRequestStatus = "",
                                   OrderId = org == null ? 0 : org.OrderId,
                                   OrderStatus = org == null ? string.Empty : ((OrderStatus)(org.OrderStatus)).ToString(),
                                   OrderCreatedDate = org == null ? string.Empty : org.CreatedDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateFormat),
                                   OrderCreatedTime = org == null ? string.Empty : org.CreatedDateTime.AddHours(addHoursToUTCDatetimeForSA).ToShortTimeString()
                               }).SingleOrDefault();

                var details = (from dd in _context.DeliveryDetails
                               where dd.RequestId == requestId
                               select new DeliveryDetailsViewModel
                               {
                                   DriverId = dd.DriverId,
                                   RequestId = dd.RequestId,
                                   StatusId = dd.StatusId,
                                   DriverLat = dd.DriverLat,
                                   DriverLong = dd.DriverLong,
                                   DeliveryDateTimeUTC = dd.DeliveryDateTime,
                                   StatusUpdateDate = dd.DeliveryDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateFormat),
                                   StatusUpdateTime = dd.DeliveryDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultTimeFormat),
                                   RequestStatus = ((ParcelStatus)dd.StatusId).ToString()
                               });

                var delDetails = details.OrderByDescending(x => x.DeliveryDateTimeUTC).Take(1).FirstOrDefault();

                var _statusId = delDetails.StatusId;
                var _driverId = delDetails.DriverId;

                if (request != null)
                {
                    request.LastRequestStatusId = _statusId;
                    request.LastRequestStatus = ((ParcelStatus)_statusId).ToString();
                    dovm.UOVM = request;
                }

                dovm.ListDeliveryDetails.AddRange(details.OrderBy(x => x.DeliveryDateTimeUTC));

                if (_driverId > 0)
                {
                    dovm.DriverInfo = GetDriverInfo(_driverId);
                }

                return dovm;
            }
            catch { throw; }
        }
        //add details for delivering parcel like PIN etc
        public async Task<ParcelDelivery> AddParcelDelivery(ParcelDelivery model)
        {
            try
            {
                var order = _context.Orders.FirstOrDefault(x => x.RequestId == model.RequestId);
                model.OrderId = order.OrderId;
                var result = await _context.ParcelDelivery.AddAsync(model);
                await _context.SaveChangesAsync();
                return model;
            }
            catch { throw; }
        }
        //get driver details
        public RequestDriverInfoViewModel GetDriverInfo(int driverId)
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
                                   where dd.DriverId == driverId && dd.IsDeleted == false
                                   select new RequestDriverInfoViewModel
                                   {
                                       Name = !string.IsNullOrEmpty(dd.FirstName + " " + dd.LastName) ? dd.FirstName + " " + dd.LastName : string.Empty,
                                       PhoneNumber = !string.IsNullOrEmpty(dd.PhoneNumber) ? dd.PhoneNumber : string.Empty,
                                       VehicleBrand = !string.IsNullOrEmpty(vb.BrandName) ? vb.BrandName : string.Empty,
                                       VehicleColor = !string.IsNullOrEmpty(vc.VehicleColorName) ? vc.VehicleColorName : string.Empty,
                                       RegistrationNumber = !string.IsNullOrEmpty(vd.RegisterationNumber) ? vd.RegisterationNumber : string.Empty,
                                   }).FirstOrDefault();

                return _driverInfo;
            }
            catch
            {
                throw;
            }
        }
        //send push notification and save in db per parcel status
        public async Task<int> SendSaveUserNotifications(int requestId, int statusId)
        {
            try
            {
                var _userId = _context.DeliveryRequest.FirstOrDefault(x => x.RequestId == requestId).SenderId;
                var _uTokens = _context.UserDetails.Where(x => x.Id == _userId).Select(x => x.DeviceToken).ToList();
                int _type = 0;
                string _status = "";
                var _order = new Orders();

                if (statusId == 1 || statusId == 2 || statusId == 7) { return 0; }
                else
                {
                    _order = _context.Orders.FirstOrDefault(x => x.RequestId == requestId);
                }
                if (statusId == 3) { _type = (int)NotificationTypes.PARCEL_WITH_DRIVER; _status = ParcelWithDriverNotificationText; }
                else if (statusId == 4) { _type = (int)NotificationTypes.PARCEL_ENROUTE; _status = ParcelEnRouteNotificationText; }
                else if (statusId == 5) { _type = (int)NotificationTypes.PARCEL_ARRIVED; _status = ParcelArrivedNotificationText; }
                else if (statusId == 6) { _type = (int)NotificationTypes.PARCEL_DELIVERED; _status = ParcelDeliveredNotificationText; }

                if (_uTokens.Count() > 0)
                {
                    string title = NotificationTitle;
                    string body = _status;
                    var _sent = await _notificationService.SendUserPushNotifications(_uTokens.ToArray(), title, body, _order);

                    if (_sent)
                    {
                        Notifications _notif = new Notifications
                        {
                            RequestId = requestId,
                            Text = body,
                            CreatedOn = DateTime.UtcNow,
                            ToUserId = _userId,
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
