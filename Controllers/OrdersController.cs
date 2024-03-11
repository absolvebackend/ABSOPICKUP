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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static _AbsoPickUp.Common.GlobalVariables;

namespace _AbsoPickUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IDriverService _driverService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILoggerManager _logger;
        public OrdersController(ILoggerManager logger, IDriverService driverService, IOrderService orderService, IWebHostEnvironment hostingEnvironment)
        {
            _driverService = driverService;
            _orderService = orderService;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        #region Get User Orders
        [Authorize]
        [HttpGet("GetUserOrders")]
        public IActionResult GetUserOrdersByStatus([FromQuery] FilterationOrderListViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInfo("API: Orders/GetUserOrders : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var userId = CommonFunctions.getUserId(User);     //"92c9fadf-a3c3-4bec-b1cd-2f7b65f578c1"; 
                if (model.requestStatusId == (int)(ParcelStatus.Unassigned))
                {
                    var _ordersList = _orderService.GetUnAssignedRequestsFilteredList(model, userId);
                    if (_ordersList != null)
                    {
                        return Ok(new { status = true, data = _ordersList, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                    }
                    else
                    {
                        _logger.LogInfo("API: Orders/GetUserOrders : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                    }
                }
                else if (model.requestStatusId == (int)(ParcelStatus.Assigned))
                {
                    var _ordersList = _orderService.GetUserOrdersFilteredList(model, userId);
                    if (_ordersList != null)
                    {
                        return Ok(new { status = true, data = _ordersList, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                    }
                    else
                    {
                        _logger.LogInfo("API: Orders/GetUserOrders : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                    }
                }
                else if (model.requestStatusId == (int)(ParcelStatus.Delivered))
                {
                    var _ordersList = _orderService.GetCompletedUserOrdersFilteredList(model, userId);
                    if (_ordersList != null)
                    {
                        return Ok(new { status = true, data = _ordersList, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                    }
                    else
                    {
                        _logger.LogInfo("API: Orders/GetUserOrders : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                        return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                    }
                }
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgRequestStatusNotValid, code = StatusCodes.Status404NotFound });
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Orders/GetUserOrders : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Cancel Orders
        [Authorize]
        [HttpPost("OrderCancelByUser")]
        public async Task<IActionResult> CancelUserOrder(int requestId)
        {
            var userId = CommonFunctions.getUserId(User);
            var requsetStatusId = _orderService.GetRequestStatus(requestId);
            if (!(requsetStatusId < (int)(ParcelStatus.WithDriver) && requestId > 0) || (string.IsNullOrEmpty(userId)))
            {
                _logger.LogInfo("API: Orders/OrderCancelByUser : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgRequestCannotBeCancelled);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgRequestCannotBeCancelled, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var _res = await _orderService.CancelUserOrder(requestId, userId);
                int _intNotice = 0;
                if (_res > 0)
                {
                    _intNotice = await _orderService.UserCancelOrderNotifications(requestId);
                }
                if (_res == 4 && _intNotice > 0)
                {
                    return Ok(new { status = true, message = "Request and Order" + ResponseMessages.msgCancelled + ". Parcel status set to not accepted. User notification sent.", code = StatusCodes.Status200OK });
                }
                else if (_res == 1 && _intNotice > 0)
                {
                    return Ok(new { status = true, message = "Request" + ResponseMessages.msgCancelled + " User notification sent.", code = StatusCodes.Status200OK });
                }
                else if (_res == 1 && _intNotice == 0)
                {
                    return Ok(new { status = true, message = "Request" + ResponseMessages.msgCancelled, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Orders/OrderCancelByUser : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Orders/OrderCancelByUser : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize]
        [HttpGet("OrderCancelByDriver")]
        public async Task<IActionResult> CancelOrderByDriver(int orderId, int driverId, string reason)
        {
            var order = _orderService.GetOrderDetails(orderId);
            var _requestId = order.RequestId;
            var requestStatusId = _orderService.GetRequestStatus(order.RequestId);

            if (!(orderId > 0 && driverId > 0 && requestStatusId < (int)(ParcelStatus.WithDriver) && order.OrderStatus == (int)(OrderStatus.Pending)))
            {
                _logger.LogInfo("API: Orders/OrderCancelByDriver : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgRequestCannotBeCancelled);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgRequestCannotBeCancelled, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                CancelledOrders cancelOrder = new CancelledOrders()
                {
                    OrderId = orderId,
                    SenderId = order.SenderId,
                    CancelledDateTime = DateTime.UtcNow,
                    CancelReason = reason
                };
                DriverCancellationViewModel dcvm = new DriverCancellationViewModel
                {
                    cancellation = cancelOrder,
                    DriverId = driverId
                };

                var _res = await _orderService.OrderCancelByDriver(dcvm);

                if (_res > 0)
                {
                    var _intNotice = await _orderService.DriverCancelOrderNotifications(_requestId);
                    return Ok(new { status = true, message = "Order" + ResponseMessages.msgCancelled + ". Parcel status updated to not accepted. Driver notification sent. Request Unassigned For DriverId = " + driverId, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Orders/OrderCancelByDriver : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Orders/OrderCancelByDriver : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Get User Orders
        [Authorize]
        [HttpGet("GetOrdersByStatusId")]
        public IActionResult GetOrdersByStatusId(int driverId, int orderStatusId)
        {
            if (orderStatusId == 0 || driverId == 0)
            {
                _logger.LogInfo("API: Orders/GetOrdersByStatusId : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                List<OrderWithStatusViewModel> _owsvmList = new List<OrderWithStatusViewModel>();
                double _numDistance = 0;
                var _ordersList = _orderService.GetOrderDetailsByStatusId(driverId, orderStatusId);
                if (_ordersList.Count > 0)
                {
                    foreach (var order in _ordersList)
                    {
                        var driver = _driverService.FindDriverById(order.DriverId.ToString()).Result;
                        OrderWithStatusViewModel owsvm = new OrderWithStatusViewModel
                        {
                            OrderId = order.OrderId,
                            OrderStatusId = order.OrderStatus,
                            OrderStatus = ((OrderStatus)(order.OrderStatus)).ToString(),
                            RequestId = order.RequestId,
                            AcceptedDateTime = order.AcceptedDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateTimeFormat),
                            CreatedDateTime = order.CreatedDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateTimeFormat),
                            UpdatedDateTime = order.UpdatedDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateTimeFormat),
                            DriverId = order.DriverId,
                            DriverName = driver.FirstName + " " + driver.LastName
                        };

                        if (order.RequestId > 0)
                        {
                            var request = _driverService.GetRequestDetails(order.RequestId);
                            owsvm.DialCode = request.DialCode.ToString();
                            owsvm.SenderAddress = request.SenderAddress;
                            owsvm.SenderPhoneNumber = request.SenderPhone;
                            owsvm.SenderLat = request.SenderLat;
                            owsvm.SenderLong = request.SenderLong;
                            owsvm.ReceiverAddress = request.ReceiverAddress;
                            owsvm.ReceiverPhoneNumber = request.ReceiverMobileNumber;
                            owsvm.ReceiverLat = request.ReceiverLat;
                            owsvm.ReceiverLong = request.ReceiverLong;

                            owsvm.DeliveryTypeId = request.DeliveryTypeId;
                            owsvm.DeliveryType = request.DeliveryType;
                            owsvm.DeliveryPrice = request.DeliveryPrice;
                            owsvm.Distance = request.TotalDeliveryDistance;
                            owsvm.Duration = request.TotalDeliveryTime;

                            var strDistance = request.TotalDeliveryDistance[0..^3];
                            bool isDouble = double.TryParse(strDistance, out _numDistance);

                            owsvm.sortByDistanceVal = _numDistance;
                        }
                        _owsvmList.Add(owsvm);
                    }
                }
                if (_owsvmList.Count > 0)
                {
                    return Ok(new { status = true, data = _owsvmList.OrderBy(x => x.sortByDistanceVal).ToList(), message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Orders/GetOrdersByStatusId : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Orders/GetOrdersByStatusId : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }
        #endregion

        #region Order Payments & Driver Earnings
        [Authorize]
        [HttpGet("CreatePayment")]
        public async Task<IActionResult> CreatePayment(int orderId, string userId)
        {
            if (orderId == 0 || string.IsNullOrEmpty(userId))
            {
                _logger.LogInfo("API: Orders/CreatePayment : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var result = await _orderService.AddPaymentDetails(orderId, userId);
                if (result > 0)
                {
                    return Ok(new { status = true, data = result, message = ResponseMessages.msgPaymentSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Orders/CreatePayment : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Orders/CreatePayment : " + ex.Message.ToString());
                return Ok(new { status = false, error = ex.Message, message = ResponseMessages.msgSomethingWentWrong, code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize]
        [HttpGet("PaymentSuccess")]
        public async Task<IActionResult> ExecutePayment(int orderId, string userId)
        {
            if (orderId == 0 || string.IsNullOrEmpty(userId))
            {
                _logger.LogInfo("API: Orders/PaymentSuccess : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var result = await _orderService.CommitTransaction(orderId, userId);
                if (result > 0)
                {
                    return Ok(new { status = true, data = result, message = ResponseMessages.msgPaymentSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Orders/PaymentSuccess : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Orders/PaymentSuccess : " + ex.Message.ToString());
                return Ok(new
                {
                    status = false,
                    error = ex.Message,
                    message = ResponseMessages.msgSomethingWentWrong,
                    code = StatusCodes.Status500InternalServerError
                });
            }
        }

        [Authorize]
        [HttpGet("GetDriverEarnings")]
        public IActionResult GetDriverEarningsReport(int driverId)
        {
            if (driverId <= 0)
            {
                _logger.LogInfo("API: Orders/GetDriverEarnings : " + StatusCodes.Status400BadRequest.ToString() + ": " + ResponseMessages.msgParametersNotCorrect);
                return Ok(new { status = false, data = new { }, message = ResponseMessages.msgParametersNotCorrect, code = StatusCodes.Status400BadRequest });
            }
            try
            {
                var result = _orderService.GetDriverEarnings(driverId);
                if (result != null)
                {
                    return Ok(new { status = true, data = result, message = ResponseMessages.msgGotSuccess, code = StatusCodes.Status200OK });
                }
                else
                {
                    _logger.LogInfo("API: Orders/GetDriverEarnings : " + StatusCodes.Status204NoContent.ToString() + ": " + ResponseMessages.msgDatanotfound);
                    return Ok(new { status = false, data = new { }, message = ResponseMessages.msgDatanotfound, code = StatusCodes.Status404NotFound });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("API: Orders/GetDriverEarnings : " + ex.Message.ToString());
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
