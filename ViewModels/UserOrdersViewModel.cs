using _AbsoPickUp.Models;
using System;
using System.Collections.Generic;

namespace _AbsoPickUp.ViewModels
{
    public class UserOrdersViewModel
    {
        public int? OrderId { get; set; }
        public int? RequestId { get; set; }
        public string SourceAddress { get; set; }
        public string SourceLat { get; set; }
        public string SourceLong { get; set; }
        public string DestinationAddress { get; set; }
        public string DestinationLat { get; set; }
        public string DestinationLong { get; set; }
        public string DeliveryType { get; set; }
        public double? Price { get; set; }
        public string Duration { get; set; }
        public string Distance { get; set; }
        public string SenderName { get; set; }
        public string SenderPhoneNo { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverEmail { get; set; }
        public int? DialCode { get; set; }
        public string ReceiverMobileNumber { get; set; }
        public string ParcelName { get; set; }
        public string ParcelNotes { get; set; }
        public string ParcelImgBefore { get; set; }
        public string ParcelImgAfter { get; set; }
        public string OrderCreatedDate { get; set; }
        public string OrderCreatedTime { get; set; }
        public string OrderStatus { get; set; }
        public int? LastRequestStatusId { get; set; }
        public string LastRequestStatus { get; set; }
        public UserOrdersViewModel()
        {
            OrderCreatedDate = String.Empty;
            OrderCreatedTime = String.Empty;
            OrderStatus = String.Empty;
            OrderId = 0;
        }
    }

    public class AssignedCompletedViewModel
    {
        public int? OrderId { get; set; }
        public int? DriverId { get; set; }
        public DeliveryRequest Request { get; set; }

        public string SenderName { get; set; }
        public string SenderPhoneNo { get; set; }
        public string DeliveryType { get; set; }
        public double? Price { get; set; }

        public string ParcelName { get; set; }
        public string ParcelNotes { get; set; }
        public string ParcelImgBefore { get; set; }
        public string ParcelImgAfter { get; set; }
        public string OrderCreatedDate { get; set; }
        public string OrderCreatedTime { get; set; }
        public string OrderStatus { get; set; }
        public int? LastRequestStatusId { get; set; }
        public string LastRequestStatus { get; set; }
        public bool IsDriverRated { get; set; }
        public AssignedCompletedViewModel()
        {
            OrderCreatedDate = String.Empty;
            OrderCreatedTime = String.Empty;
            OrderStatus = String.Empty;
            OrderId = 0;
        }
    }

    public class UnassignedRequestViewModel
    {
        public DeliveryRequest Request { get; set; }
        public string DeliveryType { get; set; }
        public double? Price { get; set; }
        public string ParcelName { get; set; }
        public string ParcelNotes { get; set; }
        public string ParcelImgBefore { get; set; }
        public string ParcelImgAfter { get; set; }
    }
    public class DriverOrderViewModel
    {
        public UserOrdersViewModel UOVM { get; set; }
        public List<DeliveryDetailsViewModel> ListDeliveryDetails { get; set; }
        public RequestDriverInfoViewModel DriverInfo { get; set; }
}

    public class OrderRequestFinalStatusViewModel
    {
        public int? OrderId { get; set; }
        public int? RequestFinalStatusId { get; set; }
    }

    public class UserOrderViewModel
    {
        public int OrderId { get; set; }
        public int RequestId { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public string RecieverAddress { get; set; }
        public string DriverName { get; set; }
        public string OrderStatus { get; set; }
        public int DeliveryTypeId { get; set; }
        public string DeliveryType { get; set; }
        public double DeliveryPrice { get; set; }
        public int ParcelId { get; set; }
        public string ParcelName { get; set; }
        public string Distance { get; set; }
        public string Duration { get; set; }
        public DateTime AcceptedDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }

}
