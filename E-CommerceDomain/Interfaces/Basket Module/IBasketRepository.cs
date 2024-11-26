using E_CommerceDomain.Entities.Basket_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.Interfaces.Basket_Module
{
    public interface IBasketRepository
    {
        public Basket GetBasket(string BasketId);
        public Basket AddOrUpdateBasket(Basket Item);
        public bool DeleteBasket(string BasketId);
    }
}
