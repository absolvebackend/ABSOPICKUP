using System.Collections.Generic;

namespace _AbsoPickUp.ViewModels
{
    public class FilterationResponseModel<T>
    {
        public int totalCount { get; set; } = 0;
        public int pageSize { get; set; } = 0;
        public int currentPage { get; set; } = 0;
        public int totalPages { get; set; } = 0;
        public string previousPage { get; set; } = string.Empty;
        public string nextPage { get; set; } = string.Empty;
        public string searchQuery { get; set; } = string.Empty;
        public string exception { get; set; } = string.Empty;
        public List<T> dataList { get; set; }
    }
}
