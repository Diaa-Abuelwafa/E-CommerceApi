using E_CommerceDomain.DTOs.Order_Module;
using E_CommerceDomain.Entities.Account_Module;
using E_CommerceDomain.Entities.Order_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.Interfaces.Order_Module
{
    public interface IOrderService
    {
        public OrderResponseDTO CreateOrder(string BuyerEmail, AddressDTO ShippingAddress, int DeliveryMethodId, string BasketId);
        public List<OrderResponseDTO> GetAllOrdersForCurrentUser(string BuyerEmail);
        public OrderResponseDTO GetOrderByIdForCurrentUser(int OrderId, string BuyerEmail);
        public List<DeliveryMethodDTO> GetAllDeliveryMethods();
    }
}
