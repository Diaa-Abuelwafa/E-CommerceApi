using E_CommerceDomain.Entities.Basket_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.DTOs.Basket_Module
{
    public class BasketDTO
    {
        public string Id { get; set; } = null;
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}
