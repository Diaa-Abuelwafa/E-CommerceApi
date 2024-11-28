using E_CommerceDomain.Entities.Basket_Module;
using E_CommerceDomain.Interfaces.Basket_Module;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_CommerceRepository.Repositories.Basket_Module
{
    public class BasketRepository : IBasketRepository
    {
        private IDatabase Redis;
        public BasketRepository(IConnectionMultiplexer Redis)
        {
            this.Redis = Redis.GetDatabase();
        }
        public Basket AddOrUpdateBasket(Basket Item)
        {
            var Items = JsonSerializer.Serialize(Item);
            var Test = Redis.StringSet(Item.Id, Items, TimeSpan.FromDays(1));

            return Item;
        }

        public bool DeleteBasket(string BasketId)
        {
            return Redis.KeyDelete(BasketId);
        }

        public Basket GetBasket(string BasketId)
        {
            var ItemJson = Redis.StringGet(BasketId);

            if(string.IsNullOrEmpty(ItemJson))
            {
                return null;
            }

            return JsonSerializer.Deserialize<Basket>(ItemJson);
        }
    }
}
