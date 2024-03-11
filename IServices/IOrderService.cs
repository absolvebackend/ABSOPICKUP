using _AbsoPickUp.Models;
using _AbsoPickUp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _AbsoPickUp.IServices
{
    public interface IOrderService
    {
        FilterationResponseModel<AssignedCompletedViewModel> GetUserOrdersFilteredList(FilterationOrderListViewModel model, string userId);
        FilterationResponseModel<UnassignedRequestViewModel> GetUnAssignedRequestsFilteredList(FilterationOrderListViewModel model, string userId);
        List<AssignedCompletedViewModel> GetAllUserOrders(string userId, int CurrentPage, int PageSize);
        Task<int> CancelUserOrder(int requestId, string senderId);
        Task<int> OrderCancelByDriver(DriverCancellationViewModel model);
        Orders GetOrderDetails(int orderId);
        int GetRequestStatus(int RequestId);
        List<Orders> GetOrderDetailsByStatusId(int statusId, int driverId);
        FilterationResponseModel<AssignedCompletedViewModel> GetCompletedUserOrdersFilteredList(FilterationOrderListViewModel model, string userId);
        Task<int> AddPaymentDetails(int orderId, string userId);
        Task<int> CommitTransaction(int orderId, string userId);
        DriverEarningsViewModel GetDriverEarnings(int driverId);
        Task<int> UserCancelOrderNotifications(int requestId);
        Task<int> DriverCancelOrderNotifications(int requestId);
    }
}
