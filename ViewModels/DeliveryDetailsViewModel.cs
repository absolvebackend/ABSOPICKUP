using _AbsoPickUp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _AbsoPickUp.ViewModels
{
    public class DeliveryDetailsViewModel
    {
        public int DriverId { get; set; }
        [Required]
        public int RequestId { get; set; }
        [Required]
        public int StatusId { get; set; }
        public string RequestStatus { get; set; }
        public decimal DriverLat { get; set; }
        public decimal DriverLong { get; set; }
        public string StatusUpdateDate { get; set; }
        public string StatusUpdateTime { get; set; }
        public DateTime DeliveryDateTimeUTC { get; set; }
    }

    public class DeliveryDetailReportViewModel
    {
        public int DriverId { get; set; }
        public string SenderId { get; set; }
        public int RequestId { get; set; }
        public string RequestCreateDateTime { get; set; }
        public string SenderAddress { get; set; }
        public string ReceiverAddress { get; set; }
        public List<DeliveryDetailsResponse> StatusList { get; set; }
    }

    public class UpdateParcelStatusViewModel
    {
        public int DriverId { get; set; }
        [Required]
        public int RequestId { get; set; }
        [Required]
        public int StatusId { get; set; }
        public decimal DriverLat { get; set; }
        public decimal DriverLong { get; set; }
    }

    public class AssignRequestViewModel
    {
        public int DriverId { get; set; }
        public int RequestId { get; set; }
        public decimal DriverLat { get; set; }
        public decimal DriverLong { get; set; }
        public bool hasAccepted { get; set; }
    }

    public class DeliveryDetailsResponseViewModel
    {
        public string ParcelStatusSetTo { get; set; }
        public string ParcelETA { get; set; }
        public string PINSent { get; set; }
        public bool SMSSentWithPIN { get; set; }
        public bool SMSSentWithETA { get; set; }

        public DeliveryDetailsResponseViewModel()
        {
            SMSSentWithPIN = false;
            SMSSentWithETA = false;
            ParcelStatusSetTo = String.Empty;
            ParcelETA = String.Empty;
            PINSent = String.Empty;
        }
        
    }

    public class DeliveryDetailsResponse
    {
        public int RequestId { get; set; }
        public int StatusId { get; set; }
        public string RequestStatus { get; set; }
        public decimal DriverLat { get; set; }
        public decimal DriverLong { get; set; }
        public string StatusDate { get; set; }
        public string StatusTime { get; set; }
    }
}
