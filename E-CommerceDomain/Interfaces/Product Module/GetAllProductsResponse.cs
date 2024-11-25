using E_CommerceDomain.DTOs.Product_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.Interfaces.Product_Module
{
    public class GetAllProductsResponse
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public int Count { get; set; } = 0;
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}
