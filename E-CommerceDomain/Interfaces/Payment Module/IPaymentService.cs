using E_CommerceDomain.DTOs.Basket_Module;
using E_CommerceDomain.Entities.Basket_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.Interfaces.Payment_Module
{
    public interface IPaymentService
    {
        public Task<BasketDTO> CreateOrUpdatePaymentIntentId(string BasketId);
        public bool UpdateOrderStatusBasedOnResultOfPaymentOperation(string PaymentIntentId, bool Success);
    }
}
