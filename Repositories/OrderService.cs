using _AbsoPickUp.Data;
using _AbsoPickUp.IServices;
using _AbsoPickUp.Models;
using _AbsoPickUp.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _AbsoPickUp.Common.GlobalVariables;

namespace _AbsoPickUp.Repositories
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;
        public OrderService(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }
        //get all user orders - assigned
        public FilterationResponseModel<AssignedCompletedViewModel> GetUserOrdersFilteredList(FilterationOrderListViewModel model, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return null;
            }
            int PageSize = model.pageSize;
            int CurrentPage = model.pageNumber;

            // Returing List of Assigned or Completed Orders  
            var _list = GetAllUserOrders(userId, CurrentPage, PageSize);

            // Get's No of Rows Count   
            int count = _list.Count();

            // Display TotalCount to Records to User  
            int TotalCount = count;

            // Calculating Totalpage by Dividing (No of Records / Pagesize)  
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            // if CurrentPage is greater than 1 means it has previousPage  
            var previousPage = CurrentPage > 1 ? "Yes" : "No";

            // if TotalPages is greater than CurrentPage means it has nextPage  
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            FilterationResponseModel<AssignedCompletedViewModel> obj = new FilterationResponseModel<AssignedCompletedViewModel>
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
        //get all user orders completed
        public FilterationResponseModel<AssignedCompletedViewModel> GetCompletedUserOrdersFilteredList(FilterationOrderListViewModel model, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return null;
            }
            int PageSize = model.pageSize;
            int CurrentPage = model.pageNumber;

            // Returing List of Assigned or Completed Orders  
            var _list = GetCompletedUserOrders(userId, CurrentPage, PageSize);

            // Get's No of Rows Count   
            int count = _list.Count();

            // Display TotalCount to Records to User  
            int TotalCount = count;

            // Calculating Totalpage by Dividing (No of Records / Pagesize)  
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            // if CurrentPage is greater than 1 means it has previousPage  
            var previousPage = CurrentPage > 1 ? "Yes" : "No";

            // if TotalPages is greater than CurrentPage means it has nextPage  
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            FilterationResponseModel<AssignedCompletedViewModel> obj = new FilterationResponseModel<AssignedCompletedViewModel>
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
        //get all user orders unassigned
        public FilterationResponseModel<UnassignedRequestViewModel> GetUnAssignedRequestsFilteredList(FilterationOrderListViewModel model, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return null;
            }
            int PageSize = model.pageSize;
            int CurrentPage = model.pageNumber;

            var _list = GetAllUserUnAssignedRequests(userId, CurrentPage, PageSize);
            // Get's No of Rows Count   
            int count = _list.Count();

            // Display TotalCount to Records to User  
            int TotalCount = count;

            // Calculating Totalpage by Dividing (No of Records / Pagesize)  
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            // if CurrentPage is greater than 1 means it has previousPage  
            var previousPage = CurrentPage > 1 ? "Yes" : "No";

            // if TotalPages is greater than CurrentPage means it has nextPage  
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";
            // Returing List of Customers Collections  
            FilterationResponseModel<UnassignedRequestViewModel> obj = new FilterationResponseModel<UnassignedRequestViewModel>
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
        //return paginated results for assigned, with driver, enroute
        public List<AssignedCompletedViewModel> GetAllUserOrders(string userId, int CurrentPage, int PageSize)
        {
            int idx = 0;
            int startIdx = (CurrentPage - 1) * PageSize;
            List<Orders> UsersOrdersList = new List<Orders>();
            List<OrderRequestFinalStatusViewModel> listorfsvm = new List<OrderRequestFinalStatusViewModel>();
            try
            {
                var ordersListUser = _context.Orders.Where(x => x.SenderId == userId).ToList();
                foreach (var order in ordersListUser)
                {
                    var requestStatus = GetRequestStatus(order.RequestId);
                    if (requestStatus > ((int)(ParcelStatus.Unassigned)) && requestStatus < ((int)(ParcelStatus.Delivered)))
                    {
                        if (idx < startIdx)
                        {
                            idx++;
                            continue;
                        }
                        if (order.OrderStatus != (int)OrderStatus.Cancelled)
                        {
                            UsersOrdersList.Add(order);
                            OrderRequestFinalStatusViewModel orfsvm = new OrderRequestFinalStatusViewModel
                            {
                                OrderId = order.OrderId,
                                RequestFinalStatusId = requestStatus
                            };
                            listorfsvm.Add(orfsvm);
                            if (UsersOrdersList.Count == PageSize)
                            {
                                break;
                            }
                        }
                    }
                }
                if (UsersOrdersList.Count == 0) { return new List<AssignedCompletedViewModel>(); }

                var UserOrders = (from or in UsersOrdersList
                                  join dr in _context.DeliveryRequest
                                  on or.RequestId equals dr.RequestId
                                  join dpr in _context.DeliveryPrice
                                  on dr.DeliveryTypeId equals dpr.TypeId
                                  join paro in _context.ParcelDetails
                                  on dr.RequestId equals paro.RequestId into ppGroup
                                  from parc in ppGroup.DefaultIfEmpty()
                                  join los in listorfsvm
                                  on or.OrderId equals los.OrderId
                                  select new AssignedCompletedViewModel
                                  {
                                      OrderId = or.OrderId,
                                      Request = dr,
                                      DeliveryType = ((ParcelType)dr.DeliveryTypeId).ToString(),
                                      Price = dpr == null ? 0 : dpr.Amount,
                                      ParcelName = parc == null ? string.Empty : parc.ParcelName,
                                      ParcelNotes = parc == null ? string.Empty : parc.ParcelNotes,
                                      ParcelImgBefore = parc == null ? string.Empty : parc.ImgBeforePacking,
                                      ParcelImgAfter = parc == null ? string.Empty : parc.ImgAfterPacking,
                                      OrderCreatedDate = or.CreatedDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateFormat),
                                      OrderCreatedTime = or.CreatedDateTime.AddHours(addHoursToUTCDatetimeForSA).ToShortTimeString(),
                                      OrderStatus = ((OrderStatus)or.OrderStatus).ToString(),
                                      LastRequestStatusId = los == null ? 0 : los.RequestFinalStatusId,
                                      LastRequestStatus = los == null ? "" : ((ParcelStatus)los.RequestFinalStatusId).ToString()
                                  }
                              ).ToList();

                return UserOrders;
            }
            catch (Exception ex) { throw ex; }
        }
        //get paginated results for completed user orders
        public List<AssignedCompletedViewModel> GetCompletedUserOrders(string userId, int CurrentPage, int PageSize)
        {
            int idx = 0;
            int startIdx = (CurrentPage - 1) * PageSize;
            List<Orders> UsersOrdersList = new List<Orders>();
            List<OrderRequestFinalStatusViewModel> listorfsvm = new List<OrderRequestFinalStatusViewModel>();
            try
            {
                var ordersListUser = _context.Orders.Where(x => x.SenderId == userId).ToList();
                foreach (var order in ordersListUser)
                {
                    var requestStatus = GetRequestStatus(order.RequestId);
                    if (requestStatus == ((int)(ParcelStatus.Delivered)))
                    {
                        if (idx < startIdx)
                        {
                            idx++;
                            continue;
                        }
                        UsersOrdersList.Add(order);
                        OrderRequestFinalStatusViewModel orfsvm = new OrderRequestFinalStatusViewModel
                        {
                            OrderId = order.OrderId,
                            RequestFinalStatusId = requestStatus
                        };
                        listorfsvm.Add(orfsvm);
                        if (UsersOrdersList.Count == PageSize)
                        {
                            break;
                        }
                    }
                }
                if (UsersOrdersList.Count == 0) { return new List<AssignedCompletedViewModel>(); }

                var UserOrders = (from or in UsersOrdersList
                                  join dr in _context.DeliveryRequest
                                  on or.RequestId equals dr.RequestId
                                  join dpr in _context.DeliveryPrice
                                  on dr.DeliveryTypeId equals dpr.TypeId
                                  join paro in _context.ParcelDetails
                                  on dr.RequestId equals paro.RequestId into ppGroup
                                  from parc in ppGroup.DefaultIfEmpty()
                                  join los in listorfsvm
                                  on or.OrderId equals los.OrderId
                                  select new AssignedCompletedViewModel
                                  {
                                      OrderId = or.OrderId,
                                      DriverId = or.DriverId,
                                      Request = dr,
                                      DeliveryType = ((ParcelType)dr.DeliveryTypeId).ToString(),
                                      Price = dpr == null ? 0 : dpr.Amount,
                                      ParcelName = parc == null ? string.Empty : parc.ParcelName,
                                      ParcelNotes = parc == null ? string.Empty : parc.ParcelNotes,
                                      ParcelImgBefore = parc == null ? string.Empty : parc.ImgBeforePacking,
                                      ParcelImgAfter = parc == null ? string.Empty : parc.ImgAfterPacking,
                                      OrderCreatedDate = or.CreatedDateTime.AddHours(addHoursToUTCDatetimeForSA).ToString(DefaultDateFormat),
                                      OrderCreatedTime = or.CreatedDateTime.AddHours(addHoursToUTCDatetimeForSA).ToShortTimeString(),
                                      OrderStatus = ((OrderStatus)or.OrderStatus).ToString(),
                                      LastRequestStatusId = los == null ? 0 : los.RequestFinalStatusId,
                                      LastRequestStatus = los == null ? "" : ((ParcelStatus)los.RequestFinalStatusId).ToString(),
                                      IsDriverRated = or.IsDriverRated
                                  }
                              ).ToList();

                return UserOrders;
            }
            catch (Exception ex) { throw ex; }
        }
        //get paginated results for unassigned requests
        public List<UnassignedRequestViewModel> GetAllUserUnAssignedRequests(string userId, int CurrentPage, int PageSize)
        {
            List<DeliveryRequest> Unassignedlist = new List<DeliveryRequest>();
            int idx = 0;
            int startIdx = (CurrentPage - 1) * PageSize;
            try
            {
                var UserRequestList = _context.DeliveryRequest.Where(x => x.SenderId == userId).ToList();
                foreach (var req in UserRequestList)
                {
                    req.ReceiverEmail ??= string.Empty;
                    req.SenderPlaceId ??= string.Empty;
                    var requestStatus = GetRequestStatus(req.RequestId);
                    if (requestStatus == ((int)(ParcelStatus.Unassigned)))
                    {
                        if (idx < startIdx)
                        {
                            idx++;
                            continue;
                        }
                        Unassignedlist.Add(req);
                        if (Unassignedlist.Count == PageSize)
                        {
                            break;
                        }
                    }
                }
                if (Unassignedlist.Count == 0) { return new List<UnassignedRequestViewModel>(); }

                var UnAssignedRequests = (from dr in Unassignedlist
                                          join dpr in _context.DeliveryPrice
                                          on dr.DeliveryTypeId equals dpr.TypeId
                                          join paro in _context.ParcelDetails
                                          on dr.RequestId equals paro.RequestId into ppGroup
                                          from parc in ppGroup.DefaultIfEmpty()
                                          select new UnassignedRequestViewModel()
                                          {
                                              Request = dr,
                                              DeliveryType = ((ParcelType)dr.DeliveryTypeId).ToString(),
                                              Price = dpr == null ? 0 : dpr.Amount,
                                              ParcelName = parc == null ? string.Empty : parc.ParcelName,
                                              ParcelNotes = parc == null ? string.Empty : parc.ParcelNotes,
                                              ParcelImgBefore = parc == null ? string.Empty : parc.ImgBeforePacking,
                                              ParcelImgAfter = parc == null ? string.Empty : parc.ImgAfterPacking
                                          }
                               ).ToList();

                return UnAssignedRequests;
            }
            catch (Exception ex) { throw ex; }
        }
        //order cancelled by driver
        public async Task<int> OrderCancelByDriver(DriverCancellationViewModel model)
        {
            try
            {
                var order = _context.Orders.FirstOrDefault(x => x.OrderId == model.cancellation.OrderId && x.DriverId == model.DriverId);

                // check if order pending
                if (order != null && order.OrderStatus == (int)(OrderStatus.Pending))
                {
                    var requestId = order.RequestId;
                    var requestStatusId = GetRequestStatus(requestId);

                    //check if request assigned
                    if (requestStatusId == (int)(ParcelStatus.Assigned))
                    {
                        //unassign request
                        var unAssignRequest = new DeliveryDetails
                        {
                            DriverId = 0,
                            RequestId = requestId,
                            StatusId = (int)(ParcelStatus.Unassigned),
                            DeliveryDateTime = DateTime.UtcNow
                        };
                        _context.DeliveryDetails.Add(unAssignRequest);

                        //order cancelled
                        _context.CancelledOrders.Add(model.cancellation);

                        //order status set to cancel
                        order.OrderStatus = (int)(OrderStatus.Cancelled);
                        order.UpdatedDateTime = DateTime.UtcNow;
                        _context.Orders.Update(order);

                        //notice status set to not accepted
                        var notice = _context.ParcelNotifications.FirstOrDefault(x => x.RequestId == requestId && x.DriverId == model.DriverId && x.Accepted);
                        if (notice != null)
                        {
                            notice.Accepted = false;
                            _context.ParcelNotifications.Update(notice);
                        }
                    }
                }
                return await _context.SaveChangesAsync();
            }
            catch { throw; }
        }
        //order cancelled by user
        public async Task<int> CancelUserOrder(int requestId, string senderId)
        {
            try
            {
                var requestStatusId = GetRequestStatus(requestId);
                if (requestStatusId < (int)(ParcelStatus.WithDriver))
                {
                    var cancelRequest = new DeliveryDetails
                    {
                        RequestId = requestId,
                        StatusId = (int)(ParcelStatus.Cancelled),
                        DeliveryDateTime = DateTime.UtcNow
                    };
                    _context.DeliveryDetails.Add(cancelRequest);
                }
                if (requestStatusId == (int)(ParcelStatus.Assigned))
                {
                    var order = _context.Orders.FirstOrDefault(x => x.RequestId == requestId);

                    if (order != null && order.OrderStatus < (int)(OrderStatus.Processing) && requestStatusId < (int)(ParcelStatus.WithDriver))
                    {
                        var cancel = new CancelledOrders
                        {
                            SenderId = senderId,
                            OrderId = order.OrderId,
                            CancelledDateTime = DateTime.UtcNow
                        };
                        _context.CancelledOrders.Add(cancel);

                        order.OrderStatus = (int)(OrderStatus.Cancelled);
                        order.UpdatedDateTime = DateTime.UtcNow;
                        _context.Orders.Update(order);

                        var notice = _context.ParcelNotifications.FirstOrDefault(x => x.RequestId == requestId && x.DriverId == order.DriverId && x.Accepted);
                        if (notice != null)
                        {
                            notice.Accepted = false;
                            _context.ParcelNotifications.Update(notice);
                        }
                    }
                }
                return await _context.SaveChangesAsync();
            }
            catch { throw; }
        }
        //get order details
        public Orders GetOrderDetails(int orderId)
        {
            return _context.Orders.FirstOrDefault(x => x.OrderId == orderId);
        }
        //get order details per status
        public List<Orders> GetOrderDetailsByStatusId(int driverId, int statusId)
        {
            return _context.Orders.Where(x => x.OrderStatus == statusId && x.DriverId == driverId).ToList();
        }
        //get request status by Id
        public int GetRequestStatus(int RequestId)
        {
            int _status = 0;
            var _request = _context.DeliveryDetails.Where(x => x.RequestId == RequestId).ToList();
            if (_request.Count > 0)
            {
                _status = _request.OrderByDescending(x => x.DeliveryDateTime).Take(1).FirstOrDefault().StatusId;
            }
            return _status;
        }

        //public async Task<Payment> CreatePayment()
        //{
        //    Dictionary<string, string> _config = new Dictionary<string, string>();
        //    _config.Add("mode","sandbox");
        //    _config.Add("clientId", paypal_client_id);
        //    _config.Add("clientSecret", paypal_client_secret);
        //    var _accessToken = new OAuthTokenCredential(_config).GetAccessToken();
        //    var _apiContext = new APIContext(_accessToken)
        //    {
        //        Config = _config
        //    };
        //    try
        //    {
        //        Payment _payment = new Payment
        //        {
        //            intent = "sale",
        //            payer = new Payer { payment_method = "paypal" },
        //            transactions = new List<Transaction>
        //            {
        //                new Transaction
        //                {
        //                    amount = new Amount
        //                    {
        //                        currency = "USD",
        //                        total = "1"
        //                    },
        //                    description = "Normal Delivery"
        //                }
        //            },
        //            redirect_urls = new RedirectUrls
        //            {
        //                cancel_url = "http://www.pleasurebuilder.com/",
        //                return_url = "http://www.revoluza.com/"
        //            }
        //        };

        //        var _createdPayment = await Task.Run(() => _payment.Create(_apiContext));
        //        return _createdPayment;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //public async Task<Payment> ExecutePayment(string payerId, string token, string paymentId)
        //{
        //    Dictionary<string, string> _config = new Dictionary<string, string>();
        //    _config.Add("mode", "sandbox");
        //    _config.Add("clientId", paypal_client_id);
        //    _config.Add("clientSecret", paypal_client_secret);
        //    var _apiContext = new APIContext(token)
        //    {
        //        Config = _config
        //    };
        //    PaymentExecution _paymentExecution = new PaymentExecution() { payer_id = payerId };
        //    Payment _payment = new Payment() { id = paymentId };
        //    Payment _executedPayment = await Task.Run(() => _payment.Execute(_apiContext, _paymentExecution));
        //    return _executedPayment;
        //}
        //add order payment details
        public async Task<int> AddPaymentDetails(int orderId, string userId)
        {
            try
            {
                var _reqId = _context.Orders.FirstOrDefault(x => x.OrderId == orderId).RequestId;
                var _delTypeId = _context.DeliveryRequest.FirstOrDefault(x => x.RequestId == _reqId).DeliveryTypeId;
                var _price = _context.DeliveryPrice.FirstOrDefault(x => x.TypeId == _delTypeId).Amount;
                PaymentDetails _details = new PaymentDetails
                {
                    OrderId = orderId,
                    UserId = userId,
                    PaymentStatusId = (int)PaymentStatus.Pending,
                    Currency = "USD",
                    Amount = _price,
                    CreatedAt = DateTime.UtcNow
                };

                _context.PaymentDetails.Add(_details);
                return await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
        //add order successful transaction details
        public async Task<int> CommitTransaction(int orderId, string userId)
        {
            try
            {
                var _reqId = _context.Orders.FirstOrDefault(x => x.OrderId == orderId).RequestId;
                var _delTypeId = _context.DeliveryRequest.FirstOrDefault(x => x.RequestId == _reqId).DeliveryTypeId;
                var _price = _context.DeliveryPrice.FirstOrDefault(x => x.TypeId == _delTypeId).Amount;
                TransactionMaster _master = new TransactionMaster
                {
                    OrderId = orderId.ToString(),
                    UserId = userId,
                    Amount = _price,
                    PaymentMode = (int)PaymentMode.Paypal,
                    TransactionType = (int)TransactionType.Credit,
                    TransactionDate = DateTime.UtcNow
                };

                _context.TransactionMaster.Add(_master);

                var _payment = _context.PaymentDetails.FirstOrDefault(x => x.OrderId == orderId);
                _payment.PaymentStatusId = (int)PaymentStatus.Success;
                _payment.TransactionId = _master.Id;
                _payment.UpdatedAt = DateTime.UtcNow;

                _context.PaymentDetails.Update(_payment);
                return await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
        //get driver earnings today, last week, total
        public DriverEarningsViewModel GetDriverEarnings(int driverId)
        {
            int todayEarnings = 0;
            int weekDayOneEarnings = 0;
            int weekDayTwoEarnings = 0;
            int weekDayThreeEarnings = 0;
            int weekDayFourEarnings = 0;
            int weekDayFiveEarnings = 0;
            int weekDaySixEarnings = 0;
            int weekDaySevenEarnings = 0;
            int totalEarnings = 0;
            DriverEarningsViewModel model = new DriverEarningsViewModel
            {
                LastWeekEarnings = new List<LastWeekEarnings>()
            };
            LastWeekEarnings DayOneAmount = new LastWeekEarnings();
            LastWeekEarnings DayTwoAmount = new LastWeekEarnings();
            LastWeekEarnings DayThreeAmount = new LastWeekEarnings();
            LastWeekEarnings DayFourAmount = new LastWeekEarnings();
            LastWeekEarnings DayFiveAmount = new LastWeekEarnings();
            LastWeekEarnings DaySixAmount = new LastWeekEarnings();
            LastWeekEarnings DaySevenAmount = new LastWeekEarnings();

            try
            {
                var today = DateTime.UtcNow;
                var startOfDay = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
                var AllOrders = _context.Orders.Where(x => x.DriverId == driverId && x.OrderStatus == ((int)OrderStatus.Complete)).ToList();
                //Today Earnings
                var TodayOrders = AllOrders.Where(x => x.CreatedDateTime >= startOfDay).ToList();
                todayEarnings = TodayOrders.Join(_context.DeliveryRequest, u => u.RequestId, uir => uir.RequestId,
                (u, uir) => new { u, uir }).Join(_context.DeliveryPrice, r => r.uir.DeliveryTypeId, ro => ro.TypeId, (r, ro) => new { r, ro })
                .Sum(m => m.ro.Amount);

                //Weekly Earning
                var firstDayOfWeek = startOfDay.AddDays(-7);
                var secondDayOfWeek = startOfDay.AddDays(-6);
                var thirdDayOfWeek = startOfDay.AddDays(-5);
                var fourthDayOfWeek = startOfDay.AddDays(-4);
                var fifthDayOfWeek = startOfDay.AddDays(-3);
                var sixthDayOfWeek = startOfDay.AddDays(-2);
                var lastDayOfWeek = startOfDay.AddDays(-1);

                var FirstDayOrders = AllOrders.Where(x => x.CreatedDateTime >= firstDayOfWeek && x.CreatedDateTime < secondDayOfWeek).ToList();
                var SecondDayOrders = AllOrders.Where(x => x.CreatedDateTime >= secondDayOfWeek && x.CreatedDateTime < thirdDayOfWeek).ToList();
                var ThirdDayOrders = AllOrders.Where(x => x.CreatedDateTime >= thirdDayOfWeek && x.CreatedDateTime < fourthDayOfWeek).ToList();
                var FourthDayOrders = AllOrders.Where(x => x.CreatedDateTime >= fourthDayOfWeek && x.CreatedDateTime < fifthDayOfWeek).ToList();
                var FifthDayOrders = AllOrders.Where(x => x.CreatedDateTime >= fifthDayOfWeek && x.CreatedDateTime < sixthDayOfWeek).ToList();
                var SixthDayOrders = AllOrders.Where(x => x.CreatedDateTime >= sixthDayOfWeek && x.CreatedDateTime < lastDayOfWeek).ToList();
                var SeventhDayOrders = AllOrders.Where(x => x.CreatedDateTime >= lastDayOfWeek && x.CreatedDateTime < startOfDay).ToList();

                //Last Week Day One
                weekDayOneEarnings = FirstDayOrders.Join(_context.DeliveryRequest, u => u.RequestId, uir => uir.RequestId,
                (u, uir) => new { u, uir }).Join(_context.DeliveryPrice, r => r.uir.DeliveryTypeId, ro => ro.TypeId, (r, ro) => new { r, ro })
                .Sum(m => m.ro.Amount);
                //Last Week Day Two
                weekDayTwoEarnings = SecondDayOrders.Join(_context.DeliveryRequest, u => u.RequestId, uir => uir.RequestId,
                (u, uir) => new { u, uir }).Join(_context.DeliveryPrice, r => r.uir.DeliveryTypeId, ro => ro.TypeId, (r, ro) => new { r, ro })
                .Sum(m => m.ro.Amount);
                //Last Week Day Three
                weekDayThreeEarnings = ThirdDayOrders.Join(_context.DeliveryRequest, u => u.RequestId, uir => uir.RequestId,
                (u, uir) => new { u, uir }).Join(_context.DeliveryPrice, r => r.uir.DeliveryTypeId, ro => ro.TypeId, (r, ro) => new { r, ro })
                .Sum(m => m.ro.Amount);
                //Last Week Day Four
                weekDayFourEarnings = FourthDayOrders.Join(_context.DeliveryRequest, u => u.RequestId, uir => uir.RequestId,
                (u, uir) => new { u, uir }).Join(_context.DeliveryPrice, r => r.uir.DeliveryTypeId, ro => ro.TypeId, (r, ro) => new { r, ro })
                .Sum(m => m.ro.Amount);
                //Last Week Day Five
                weekDayFiveEarnings = FifthDayOrders.Join(_context.DeliveryRequest, u => u.RequestId, uir => uir.RequestId,
                (u, uir) => new { u, uir }).Join(_context.DeliveryPrice, r => r.uir.DeliveryTypeId, ro => ro.TypeId, (r, ro) => new { r, ro })
                .Sum(m => m.ro.Amount);
                //Last Week Day Six
                weekDaySixEarnings = SixthDayOrders.Join(_context.DeliveryRequest, u => u.RequestId, uir => uir.RequestId,
                (u, uir) => new { u, uir }).Join(_context.DeliveryPrice, r => r.uir.DeliveryTypeId, ro => ro.TypeId, (r, ro) => new { r, ro })
                .Sum(m => m.ro.Amount);
                //Last Week Day Seven
                weekDaySevenEarnings = SeventhDayOrders.Join(_context.DeliveryRequest, u => u.RequestId, uir => uir.RequestId,
                (u, uir) => new { u, uir }).Join(_context.DeliveryPrice, r => r.uir.DeliveryTypeId, ro => ro.TypeId, (r, ro) => new { r, ro })
                .Sum(m => m.ro.Amount);

                //Total Earning
                totalEarnings = AllOrders.Join(_context.DeliveryRequest, u => u.RequestId, uir => uir.RequestId,
                (u, uir) => new { u, uir }).Join(_context.DeliveryPrice, r => r.uir.DeliveryTypeId, ro => ro.TypeId, (r, ro) => new { r, ro })
                .Sum(m => m.ro.Amount);

                DayOneAmount.Amount = weekDayOneEarnings;
                DayTwoAmount.Amount = weekDayTwoEarnings;
                DayThreeAmount.Amount = weekDayThreeEarnings;
                DayFourAmount.Amount = weekDayFourEarnings;
                DayFiveAmount.Amount = weekDayFiveEarnings;
                DaySixAmount.Amount = weekDaySixEarnings;
                DaySevenAmount.Amount = weekDaySevenEarnings;

                model.TodayEarnings = todayEarnings;
                model.LastWeekEarnings.Add(DayOneAmount);
                model.LastWeekEarnings.Add(DayTwoAmount);
                model.LastWeekEarnings.Add(DayThreeAmount);
                model.LastWeekEarnings.Add(DayFourAmount);
                model.LastWeekEarnings.Add(DayFiveAmount);
                model.LastWeekEarnings.Add(DaySixAmount);
                model.LastWeekEarnings.Add(DaySevenAmount);

                model.TotalEarnings = totalEarnings;
                return model;
            }
            catch
            {
                throw;
            }
        }
        //send save push notification when user cancel order
        public async Task<int> UserCancelOrderNotifications(int requestId)
        {
            try
            {
                int _type = (int)NotificationTypes.ORDER_CANCELLED;
                var _driverId = _context.DeliveryDetails.Where(x => x.RequestId == requestId && x.StatusId != ((int)ParcelStatus.Cancelled)).OrderByDescending(x => x.DeliveryDateTime).FirstOrDefault().DriverId;
                var _order = _context.Orders.FirstOrDefault(x => x.RequestId == requestId && x.OrderStatus == ((int)OrderStatus.Cancelled));
                var _uTokens = _context.DriverDetails.Where(x => x.DriverId == _driverId).Select(x => x.DeviceToken).ToList();

                if (_driverId == 0 || _order == null || _uTokens.Count == 0)
                {
                    return 0;
                }

                var _orderId = _order == null ? string.Empty : _order.OrderId.ToString();

                if (_uTokens.Count() > 0)
                {
                    string title = NotificationTitle;
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.Append("Order No. ");
                    strMsg.Append(_orderId);
                    strMsg.Append(" has been cancelled by user.");
                    string body = strMsg.ToString();

                    var _sent = await _notificationService.SendDriverPushNotifications(_uTokens.ToArray(), title, body, _order);

                    if (_sent)
                    {
                        Notifications _notif = new Notifications
                        {
                            RequestId = requestId,
                            Text = body,
                            CreatedOn = DateTime.UtcNow,
                            ToUserId = string.Empty,
                            ToDriverId = _driverId,
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
        //send save push notification when driver cancel order
        public async Task<int> DriverCancelOrderNotifications(int requestId)
        {
            try
            {
                var _userId = _context.DeliveryRequest.FirstOrDefault(x => x.RequestId == requestId).SenderId;
                var _uTokens = _context.UserDetails.Where(x => x.Id == _userId).Select(x => x.DeviceToken).ToList();
                int _type = (int)NotificationTypes.ORDER_CANCELLED;
                var _order = _context.Orders.FirstOrDefault(x => x.RequestId == requestId && x.OrderStatus == ((int)OrderStatus.Cancelled));

                if (_uTokens.Count() > 0)
                {
                    string title = NotificationTitle;
                    string _oId = _order == null ? string.Empty : _order.OrderId.ToString();
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.Append("Order No. ");
                    strMsg.Append(_oId);
                    strMsg.Append(" has been cancelled by our executive.");
                    string body = strMsg.ToString();

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
