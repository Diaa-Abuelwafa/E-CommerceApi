using E_CommerceDomain.DTOs.Product_Module;
using E_CommerceDomain.Entities.Product_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.Interfaces.Product_Module
{
    public interface IProductService
    {
        public GetAllProductsResponse GetAllProducts(ProductParams Params);
        public ProductDTO GetProductById(int Id);
        public List<BrandTypeDTO> GetAllBrands();
        public List<BrandTypeDTO> GetAllTypes();
    }
}
