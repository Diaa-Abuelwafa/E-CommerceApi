using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.DTOs.Order_Module
{
    public class OrderDTO
    {
        public int DeliveryMethodId { get; set; }
        public string BasketId { get; set; }
        public AddressDTO Address { get; set; }
    }
}
