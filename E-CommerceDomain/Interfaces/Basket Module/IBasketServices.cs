using E_CommerceDomain.DTOs.Basket_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.Interfaces.Basket_Module
{
    public interface IBasketServices
    {
        public BasketDTO GetBasketById(string BasketId);
        public BasketDTO AddOrUpdateBasket(BasketDTO Item);
        public bool DeleteBasket(string BasketId);
    }
}
