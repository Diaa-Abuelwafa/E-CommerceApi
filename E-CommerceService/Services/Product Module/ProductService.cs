using E_CommerceDomain.DTOs.Product_Module;
using E_CommerceDomain.Entities.Product_Module;
using E_CommerceDomain.Interfaces;
using E_CommerceDomain.Interfaces.Product_Module;
using E_CommerceDomain.Specifications;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace E_CommerceService.Services.Product_Module
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork Unit;
        private readonly IConfiguration Config;

        public ProductService(IUnitOfWork Unit, IConfiguration Config)
        {
            this.Unit = Unit;
            this.Config = Config;
        }

        public GetAllProductsResponse GetAllProducts(ProductParams Params)
        {
            // Call Repository To Get All Products

            var Spec = new Specifications<Product, int>();

            // Add Includes To The Specifications
            Spec.Includes.Add(P => P.Type);
            Spec.Includes.Add(P => P.Brand);

            // If Ask Sorting Add Sorting To The Specifications
            if (Params.OrderByAsec is not null)
            {
                switch(Params.OrderByAsec)
                {
                    default:
                    case "name":
                        Spec.OrderBy = P => P.Name;
                        break;

                    case "price":
                        Spec.OrderBy = P => P.Price;
                        break;
                }
            }
            else if(Params.OrderByDesc is not null)
            {
                switch (Params.OrderByDesc)
                {
                    default:
                    case "name":
                        Spec.OrderByDesc = P => P.Name;
                        break;

                    case "price":
                        Spec.OrderByDesc = P => P.Price;
                        break;
                }
            }

            // If Ask Filteration Add Filteration To The Specifications
            Spec.Criteria = P =>
                (Params.TypeId == null || P.TypeId == Params.TypeId) &&
                (Params.BrandId == null || P.BrandId == Params.BrandId) &&
                (string.IsNullOrEmpty(Params.SearchByName) || P.Name.ToLower().Contains(Params.SearchByName.ToLower()));


            // Make The Response To Calculate The Count Before The Pagination
            GetAllProductsResponse Response = new GetAllProductsResponse();

            Response.Count = Unit.Repo<Product, int>().CountItems(Spec);


            // If Ask Pagination

            if (Params.PageIndex is not null && Params.PageIndex != 0)
            {
                Spec.Skip = (int)((Params.PageIndex - 1) * Params.PageSize);

                Spec.Take = (int)Params.PageSize;
            }

            var ProductsFromDb = Unit.Repo<Product, int>().GetAll(Spec).ToList();

            // Mapping To Product DTO
            List<ProductDTO> Products = new List<ProductDTO>();

            foreach (var Product in ProductsFromDb)
            {
                var P = new ProductDTO()
                {
                    Id = Product.Id,
                    Name = Product.Name,
                    Description = Product.Description,
                    PictureUrl = $"{Config["BaseUrl"]}{Product.PictureUrl}",
                    TypeId = Product.Type.Id,
                    TypeName = Product.Type.Name,
                    BrandId = Product.Brand.Id,
                    BrandName = Product.Brand.Name
                };

                Products.Add(P);
            }


            Response.Products = Products;


            if (Params.PageIndex is not null)
            {
                Response.PageIndex = (int)Params.PageIndex;
                Response.PageSize = (int)Params.PageSize;
            }


            return Response;
        }

        public ProductDTO GetProductById(int Id)
        {
            var Spec = new Specifications<Product, int>();
            Spec.Includes.Add(P => P.Brand);
            Spec.Includes.Add(P => P.Type);
            Spec.Criteria = p => p.Id == Id;

            var ProductFromDb = Unit.Repo<Product, int>().GetById(Spec).FirstOrDefault(x => x.Id == Id);

            if (ProductFromDb is null)
            {
                return null;
            }

            var P = new ProductDTO()
            {
                Id = ProductFromDb.Id,
                Name = ProductFromDb.Name,
                Description = ProductFromDb.Description,
                PictureUrl = $"{Config["BaseUrl"]}{ProductFromDb.PictureUrl}",
                TypeId = ProductFromDb.Type.Id,
                TypeName = ProductFromDb.Type.Name,
                BrandId = ProductFromDb.Brand.Id,
                BrandName = ProductFromDb.Brand.Name
            };

            return P;
        }

        public List<BrandTypeDTO> GetAllTypes()
        {
            var Spec = new Specifications<ProductType, int>();

            var TypesFromDb = Unit.Repo<ProductType, int>().GetAll(Spec).ToList();

            List<BrandTypeDTO> Types = new List<BrandTypeDTO>();

            foreach (var T in TypesFromDb)
            {
                var Brand = new BrandTypeDTO()
                {
                    Id = T.Id,
                    Name = T.Name
                };

                Types.Add(Brand);
            }

            return Types;
        }

        public List<BrandTypeDTO> GetAllBrands()
        {
            var Spec = new Specifications<ProductBrand, int>();

            var BrandsFromDb = Unit.Repo<ProductBrand, int>().GetAll(Spec).ToList();

            List<BrandTypeDTO> Brands = new List<BrandTypeDTO>();

            foreach (var B in BrandsFromDb)
            {
                var Brand = new BrandTypeDTO()
                {
                    Id = B.Id,
                    Name = B.Name
                };

                Brands.Add(Brand);
            }

            return Brands;
        }

    }
}
