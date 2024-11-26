using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.Entities.Basket_Module
{
    public class Basket
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}
