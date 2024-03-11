namespace _AbsoPickUp.ViewModels
{
    public class ApproveDriverViewModel
    {
        public int DriverId { get; set; }
        public bool AdminHasApproved { get; set; }
        public string RejectReason { get; set; }

    }

    public class ApproveAppUserViewModel
    {
        public string UserId { get; set; }
        public int UserTypeId { get; set; }
        public bool AdminHasApproved { get; set; }
        public string RejectReason { get; set; }
    }
}
