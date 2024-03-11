namespace _AbsoPickUp.ViewModels
{
    public class NotificationViewModel
    {
        public string Id { get; set; }
        public int ToDriverId { get; set; }
        public string ToUserId { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }
        public int RequestId { get; set; }
        public string Text { get; set; }
        public string CreatedOn { get; set; }
    }
}
