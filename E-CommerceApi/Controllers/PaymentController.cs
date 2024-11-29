using E_CommerceApi.HandlingErrors;
using E_CommerceDomain.Interfaces.Payment_Module;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Net;

namespace E_CommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService PaymentService;

        public PaymentController(IPaymentService PaymentService)
        {
            this.PaymentService = PaymentService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreatePaymentIntent(string BasketId)
        {
            var Result = PaymentService.CreateOrUpdatePaymentIntentId(BasketId);

            if(Result is null)
            {
                return BadRequest(new ApiErrorResponse((int)HttpStatusCode.BadRequest));
            }

            return Ok(Result);
        }


        [HttpPost]
        [Route("Webhook")]
        public async Task<IActionResult> Index() //BaseUrl/api/payment/Webhook
        {

            /*
                Note : When This App Become On Server I Will Add This Endpoint To 
                Stripe Settings To Call It When Payment Operation Happens
             */

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            const string endpointSecret = "whsec_...";
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                var signatureHeader = Request.Headers["Stripe-Signature"];

                stripeEvent = EventUtility.ConstructEvent(json,
                        signatureHeader, endpointSecret);

                var PaymentIntent = stripeEvent.Data.Object as PaymentIntent;

                if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                {
                    PaymentService.UpdateOrderStatusBasedOnResultOfPaymentOperation(PaymentIntent.Id, true);
                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
                {
                    PaymentService.UpdateOrderStatusBasedOnResultOfPaymentOperation(PaymentIntent.Id, false);
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}
