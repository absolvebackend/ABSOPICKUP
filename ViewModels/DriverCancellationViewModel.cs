using _AbsoPickUp.Models;
using System.Collections.Generic;

namespace _AbsoPickUp.ViewModels
{
    public class DriverCancellationViewModel
    {
        public CancelledOrders cancellation { get; set; }
        public int DriverId { get; set; }
    }

    public class DriverEarningsViewModel
    {
        public int TodayEarnings { get; set; }
        public List<LastWeekEarnings> LastWeekEarnings { get; set; }
        public int TotalEarnings { get; set; }
    }

    public class LastWeekEarnings
    {
        public int Amount { get; set; }
    }
}
