using _AbsoPickUp.Models;
using _AbsoPickUp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _AbsoPickUp.IServices
{
    public interface IRequestService
    {
        Task<DeliveryRequest> AddDeliveryRequest(DeliveryRequest model);
        Task<int> AddDeliveryDetails(DeliveryDetails model);
        Task<ParcelDetails> AddParcelDetails(ParcelDetails model);
        Task<DistanceMatrixResponse> DistanceMatrixRequest(string source, string Destination, int typeId);
        public Task<DistanceMatrixResponse> DistanceMatrixRequestLatLon(string oLat, string oLon, string dLat, string dLon, int deliveryTypeId);
        Task<List<DeliveryTypePriceViewModel>> GetPrice();
        DriverOrderViewModel GetRequestDetailsById(int requestId);
        string GetReceiverPhoneNo(int RequestId);
        Task<ParcelDelivery> AddParcelDelivery(ParcelDelivery model);
        string GetReceiverAddress(int RequestId);
        Task<int> SendSaveUserNotifications(int requestId, int statusId);
    }
}
