using E_CommerceDomain.Entities.Order_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.DTOs.Order_Module
{
    public class OrderResponseDTO
    {
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; }
        public AddressDTO ShippingAddress { get; set; }
        public string DeliveryMethodShortName { get; set; }
        public List<OrderItemDTO> Items { get; set; }
        public decimal Total { get; set; }
    }
}
