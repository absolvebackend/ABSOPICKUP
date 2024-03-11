using System.ComponentModel.DataAnnotations;

namespace _AbsoPickUp.ViewModels
{
    public class FilterationListViewModel
    {
        public string sortOrder { get; set; }
        public string sortField { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string searchQuery { get; set; }
        public string filterBy { get; set; } = "";
    }
    public class FilterationOrderListViewModel
    {
        public string sortOrder { get; set; }
        public string sortField { get; set; }
        [Required]
        public int pageNumber { get; set; }
        [Required]
        public int pageSize { get; set; }
        [Required]
        public int requestStatusId { get; set; } = 0;
    }
    public class FilterationDriverReportViewModel
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string searchQuery { get; set; }
        public int driverId { get; set; }
    }

    public class FilterationUserReportViewModel
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string searchQuery { get; set; }
        public string userId { get; set; }
        public int userTypeId { get; set; }
    }
}
