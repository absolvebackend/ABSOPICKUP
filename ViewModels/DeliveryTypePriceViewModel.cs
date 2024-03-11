using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _AbsoPickUp.ViewModels
{
    public class DeliveryTypePriceViewModel
    {
        public int TypeId { get; set; }
        public string DeliveryType { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}
