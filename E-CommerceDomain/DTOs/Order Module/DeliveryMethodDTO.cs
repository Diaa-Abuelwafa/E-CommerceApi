using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.DTOs.Order_Module
{
    public class DeliveryMethodDTO
    {
        public string ShortName { get; set; }
        public string? DeliveryTime { get; set; }
        public decimal Cost { get; set; }
    }
}
