using E_CommerceApi.HandlingErrors;
using E_CommerceDomain.DTOs.Product_Module;
using E_CommerceDomain.Interfaces.Product_Module;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_CommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService ProductService;

        public ProductController(IProductService ProductService)
        {
            this.ProductService = ProductService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetAllProductsResponse), 200)]
        public IActionResult GetAllProducts([FromQuery] ProductParams Params)
        {
            var Products = ProductService.GetAllProducts(Params);

            return Ok(Products);
        }

        [HttpGet("{Id:int}")]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        public IActionResult GetProductBtId(int Id)
        {
            var Product = ProductService.GetProductById(Id);

            if (Product is not null)
            {
                return Ok(Product);
            }

            return NotFound(new ApiErrorResponse((int)HttpStatusCode.NotFound, "No Product With This Id"));
        }

        [HttpGet("Brands")]
        [ProducesResponseType(typeof(BrandTypeDTO), 200)]
        public IActionResult GetAllBrands()
        {
            var Brands = ProductService.GetAllBrands();

            return Ok(Brands);
        }

        [HttpGet("Types")]
        [ProducesResponseType(typeof(BrandTypeDTO), 200)]
        public IActionResult GetAllTypes()
        {
            var Types = ProductService.GetAllTypes();

            return Ok(Types);
        }
    }
}
