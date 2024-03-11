using _AbsoPickUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _AbsoPickUp.ViewModels
{
    public class AssignRequestsViewModel
    {

    }

    public class UnAssignedRequestsViewModel
    {
        public List<DeliveryDetails> NewUnAssignedRequests { get; set; }
        public List<DeliveryDetails> UnAssignedRequestsToday { get; set; }
    }

    public class DriverPositionViewModel
    {
        public string location { get; set; }
        public decimal lat { get; set; }
        public decimal lon { get; set; }
        public string gpstimestamp { get; set; }
        public int driver_id { get; set; }
        public string driverPosition { get; set; }
        public string markerPreLoc { get; set; }
        public string marker { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public bool Online { get; set; }
    }
}
