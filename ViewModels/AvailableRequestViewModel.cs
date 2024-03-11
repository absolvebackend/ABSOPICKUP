namespace _AbsoPickUp.ViewModels
{
    public class AvailableRequestViewModel
    {
        public string SenderAddress { get; set; }
        public string ReceiverAddress { get; set; }
        public string Distance { get; set; }
        public string Duration { get; set; }
        public string DeliveryType { get; set; }
        public double DeliveryPrice { get; set; }
        public int RequestId { get; set; }
        public int DeliveryTypeId { get; set; }
        public string RequestDate { get; set; }
        public string RequestTime { get; set; }
        public string SenderLatitude { get; set; }
        public string SenderLongitude { get; set; }
        public string ReceiverLatitude { get; set; }
        public string ReceiverLongitude { get; set; }
    }
    public class SenderRequestViewModel
    {
        public string SenderPlaceId { get; set; }
        public string ReceiverPlaceId { get; set; }
        public string SenderLatitude { get; set; }
        public string SenderLongitude { get; set; }
        public string ReceiverLatitude { get; set; }
        public string ReceiverLongitude { get; set; }
        public string DeliveryType { get; set; }

    }
}
