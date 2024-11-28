using E_CommerceApi.HandlingErrors;
using E_CommerceDomain.DTOs.Order_Module;
using E_CommerceDomain.Interfaces.Order_Module;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace E_CommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService OrderService;

        public OrderController(IOrderService OrderService)
        {
            this.OrderService = OrderService;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(OrderResponseDTO), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        public IActionResult CreateOrder(OrderDTO Order)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);


            var OrderResponse = OrderService.CreateOrder(BuyerEmail, Order.Address, Order.DeliveryMethodId, Order.BasketId);

            if(OrderResponse is null)
            {
                return BadRequest(new ApiErrorResponse((int)HttpStatusCode.BadRequest));
            }

            return Ok(OrderResponse);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllOrdersForCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);

            var Orders = OrderService.GetAllOrdersForCurrentUser(Email);

            if(Orders is null)
            {
                return BadRequest(new ApiErrorResponse((int)HttpStatusCode.BadRequest));
            }

            return Ok(Orders);
        }

        [HttpGet]
        [Authorize]
        [Route("{Id}")]
        public IActionResult GetOrderByIdForCurrentUser(int Id)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);

            var Orders = OrderService.GetOrderByIdForCurrentUser(Id, Email);

            if (Orders is null)
            {
                return BadRequest(new ApiErrorResponse((int)HttpStatusCode.BadRequest));
            }

            return Ok(Orders);
        }

        [HttpGet]
        [Route("Delivery")]
        public IActionResult GetAllDeliveryMethods()
        {
            var Methods = OrderService.GetAllDeliveryMethods();

            return Ok(Methods);
        }
    }
}
