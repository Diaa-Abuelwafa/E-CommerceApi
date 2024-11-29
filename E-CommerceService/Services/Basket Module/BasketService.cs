using E_CommerceDomain.DTOs.Basket_Module;
using E_CommerceDomain.Entities.Basket_Module;
using E_CommerceDomain.Interfaces.Basket_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceService.Services.Basket_Module
{
    public class BasketService : IBasketServices
    {
        private readonly IBasketRepository BasketRepository;

        public BasketService(IBasketRepository BasketRepository)
        {
            this.BasketRepository = BasketRepository;
        }
        public BasketDTO AddOrUpdateBasket(BasketDTO Item)
        {
            Basket B = new Basket()
            {
                Id = Item.Id,
                Items = Item.Items,
                DeliveryMethodId = Item.DeliveryMethodId,
                PaymentIntentId = Item.PaymentIntentId,
                ClientSecret = Item.ClientSecret
            };

            var BasketFromDb = BasketRepository.AddOrUpdateBasket(B);

            BasketDTO Basket = new BasketDTO()
            {
                Id = BasketFromDb.Id,
                Items = BasketFromDb.Items,
                DeliveryMethodId = BasketFromDb.DeliveryMethodId,
                PaymentIntentId = BasketFromDb.PaymentIntentId,
                ClientSecret = BasketFromDb.ClientSecret
            };

            return Basket; 
        }

        public bool DeleteBasket(string BasketId)
        {
            return BasketRepository.DeleteBasket(BasketId);
        }

        public BasketDTO GetBasketById(string BasketId)
        {
            var BasketFromDb = BasketRepository.GetBasket(BasketId);

            if(BasketFromDb is null)
            {
                return null;
            }

            BasketDTO Basket = new BasketDTO()
            {
                Id = BasketFromDb.Id,
                Items = BasketFromDb.Items,
                DeliveryMethodId = BasketFromDb.DeliveryMethodId,
                PaymentIntentId = BasketFromDb.PaymentIntentId,
                ClientSecret = BasketFromDb.ClientSecret
            };


            return Basket;
        }
    }
}
