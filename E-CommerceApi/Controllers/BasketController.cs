using E_CommerceApi.HandlingErrors;
using E_CommerceDomain.DTOs.Basket_Module;
using E_CommerceDomain.Interfaces.Basket_Module;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_CommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketServices BasketServices;

        public BasketController(IBasketServices BasketServices)
        {
            this.BasketServices = BasketServices;
        }

        [HttpGet]
        public IActionResult GetBasketById(string BasketId)
        {
            var Basket = BasketServices.GetBasketById(BasketId);

            if(Basket is not null)
            {
                return Ok(Basket);
            }

            return NotFound(new ApiErrorResponse((int)HttpStatusCode.NotFound, "Basket Not Found"));
        }

        [HttpPost]
        public IActionResult AddOrUpdateBasket(BasketDTO BasketFromUser)
        {
            var Basket = BasketServices.AddOrUpdateBasket(BasketFromUser);

            return Created();
        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteBasket(string BasketId)
        {
            bool Flag = BasketServices.DeleteBasket(BasketId);

            if(Flag)
            {
                return NoContent();
            }

            return BadRequest(new ApiErrorResponse((int)HttpStatusCode.NoContent, "Not Found Basket"));
        }

    }
}
