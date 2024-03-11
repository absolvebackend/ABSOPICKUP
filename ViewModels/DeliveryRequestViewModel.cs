using _AbsoPickUp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace _AbsoPickUp.ViewModels
{
    public class DeliveryRequestViewModel
    {
        public decimal SenderLat { get; set; }
        public decimal SenderLong { get; set; }
        [MaxLength(512)]
        public string SenderPlaceId { get; set; }
        public string SenderAddress { get; set; }
        [MaxLength(50)]
        public string ReceiverName { get; set; }
        [MaxLength(256)]
        public string ReceiverEmail { get; set; }
        [MaxLength(512)]
        public string ReceiverPlaceId { get; set; }
        public string ReceiverAddress { get; set; }
        public int DialCode { get; set; }
        [MaxLength(15)]
        public string ReceiverMobileNumber { get; set; }
        public decimal ReceiverLat { get; set; }
        public decimal ReceiverLong { get; set; }
        [Required]
        public int DeliveryTypeId { get; set; }
        public string TotalDeliveryTime { get; set; }
        public string TotalDeliveryDistance { get; set; }
    }

    public class RequestDetailsViewModel
    {
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public decimal SenderLat { get; set; }
        public decimal SenderLong { get; set; }
        public string SenderPhone { get; set; }
        public string ReceiverName { get; set; }
        public decimal ReceiverLat { get; set; }
        public decimal ReceiverLong { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverEmail { get; set; }
        public int DialCode { get; set; }
        public string ReceiverMobileNumber { get; set; }
        public int DeliveryTypeId { get; set; }
        public string DeliveryType { get; set; }
        public int DeliveryPrice { get; set; }
        public string TotalDeliveryTime { get; set; }
        public string TotalDeliveryDistance { get; set; }
    }

    public class DeliveryRequestLaterViewModel
    {
        public DeliveryRequestViewModel request { get; set; }
        public DateTime RequestDateTime { get; set; }
    }

    public class Position
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class RequestDistanceViewModel
    {
        public Position Source { get; set; }
        public Position Destination { get; set; }
        public int DeliveryTypeId { get; set; }
    }

    public class DriverAcceptedRequests
    {
        public int RequestId { get; set; }
        public int OrderId { get; set; }
        public string DialCode { get; set; }
        public string SenderAddress { get; set; }
        public string SenderPhoneNumber { get; set; }
        public decimal SenderLat { get; set; }
        public decimal SenderLong { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public decimal ReceiverLat { get; set; }
        public decimal ReceiverLong { get; set; }
        public string Distance { get; set; }
        public double DistanceValue { get; set; }
        public string Duration { get; set; }
        public string DeliveryType { get; set; }
        public int DeliveryTypeId { get; set; }
        public int DeliveryPrice { get; set; }
        public string Status { get; set; }
        public string OrderDate { get; set; }
        public string OrderTime { get; set; }
    }

    public class DeliverRequestWithParcelViewModel
    {
        public ParcelDetailsViewModel Parcel { get; set; }
        public DeliveryRequestViewModel DeliveryRequest { get; set; }
    }

    public class DeliveryResponse
    {
        public bool status { get; set; }
        public DeliveryRequest data { get; set; }
        public string message { get; set; }
        public int code { get; set; }
    }

    public class UserDeliveryResponse
    {
        public bool status { get; set; }
        public DeliveryRequest Request_data { get; set; }
        public ParcelDetails Parcel_data { get; set; }
        public string message { get; set; }
        public int code { get; set; }
    }

    public class ParcelDetailsResponse
    {
        public bool status { get; set; }
        public ParcelDetails data { get; set; }
        public string message { get; set; }
        public int code { get; set; }
    }

    public class OrderWithStatusViewModel
    {
        public int OrderId { get; set; }
        public int OrderStatusId { get; set; }
        public string OrderStatus { get; set; }
        public int RequestId { get; set; }
        public string DialCode { get; set; }
        public string SenderAddress { get; set; }
        public string SenderPhoneNumber { get; set; }
        public decimal SenderLat { get; set; }
        public decimal SenderLong { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public decimal ReceiverLat { get; set; }
        public decimal ReceiverLong { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public int DeliveryTypeId { get; set; }
        public string DeliveryType { get; set; }
        public double DeliveryPrice { get; set; }
        public string Distance { get; set; }
        public double sortByDistanceVal { get; set; }
        public string Duration { get; set; }
        public string AcceptedDateTime { get; set; }
        public string CreatedDateTime { get; set; }
        public string UpdatedDateTime { get; set; }

    }

    public class DeliveryRequestUserViewModel
    {
        public decimal SenderLat { get; set; }
        public decimal SenderLong { get; set; }
        [MaxLength(512)]
        public string SenderPlaceId { get; set; }
        public string SenderAddress { get; set; }
        [MaxLength(50)]
        public string ReceiverName { get; set; }
        [MaxLength(256)]
        public string ReceiverEmail { get; set; }
        [MaxLength(512)]
        public string ReceiverPlaceId { get; set; }
        public string ReceiverAddress { get; set; }
        public int DialCode { get; set; }
        [MaxLength(15)]
        public string ReceiverMobileNumber { get; set; }
        public decimal ReceiverLat { get; set; }
        public decimal ReceiverLong { get; set; }
        [Required]
        public int DeliveryTypeId { get; set; }
        public string TotalDeliveryTime { get; set; }
        public string TotalDeliveryDistance { get; set; }
        public int RequestId { get; set; }
        public string ParcelName { get; set; }
        public string ParcelNotes { get; set; }
        [Required]
        public IFormFile ImgBeforePacking { get; set; }
        [Required]
        public IFormFile ImgAfterPacking { get; set; }
    }
}
