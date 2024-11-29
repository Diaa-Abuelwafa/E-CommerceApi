using E_CommerceDomain.DTOs.Basket_Module;
using E_CommerceDomain.Entities.Basket_Module;
using E_CommerceDomain.Entities.Order_Module;
using E_CommerceDomain.Interfaces;
using E_CommerceDomain.Interfaces.Basket_Module;
using E_CommerceDomain.Interfaces.Payment_Module;
using E_CommerceDomain.Specifications;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceService.Services.Payment_Module
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketServices BasketService;
        private readonly IUnitOfWork UnitOfWork;
        private readonly IConfiguration Config;

        public PaymentService(IBasketServices BasketService, IUnitOfWork UnitOfWork, IConfiguration Config)
        {
            this.BasketService = BasketService;
            this.UnitOfWork = UnitOfWork;
            this.Config = Config;
        }
        public async Task<BasketDTO> CreateOrUpdatePaymentIntentId(string BasketId)
        {
            // To Communicate With Stripe Gateway
            StripeConfiguration.ApiKey = Config["Stripe:SecretKey"];


            var Basket = BasketService.GetBasketById(BasketId);

            if(Basket is null)
            {
                return null;
            }

            PaymentIntentService StripeService = new PaymentIntentService();

            if (Basket.PaymentIntentId is null)
            {
                // Create PaymentIntentId

                decimal Sum = 0;

                foreach(var Item in Basket.Items)
                {
                    Sum += Item.Price * Item.Quantity;
                }

                DeliveryMethod Delivery = UnitOfWork.Repo<DeliveryMethod, int>().GetByIdWithoutSpecifications((int)Basket.DeliveryMethodId);

                Sum += Delivery.Cost;

                PaymentIntentCreateOptions Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(Sum * 100),
                    PaymentMethodTypes = new List<string>() { "card" },
                    Currency = "usd"
                };


                var PaymentIntent = await StripeService.CreateAsync(Options);

                Basket.PaymentIntentId = PaymentIntent.Id;
                Basket.ClientSecret = PaymentIntent.ClientSecret;

                BasketService.AddOrUpdateBasket(Basket);
            }
            else
            {
                // Update PaymentIntentId  

                decimal Sum = 0;

                foreach (var Item in Basket.Items)
                {
                    Sum += Item.Price * Item.Quantity;
                }

                var Delivery = UnitOfWork.Repo<DeliveryMethod, int>().GetByIdWithoutSpecifications((int)Basket.DeliveryMethodId);

                Sum += Delivery.Cost;

                PaymentIntentUpdateOptions Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(Sum * 100),
                };


                var PaymentIntent = await StripeService.UpdateAsync(Basket.PaymentIntentId, Options);

                Basket.PaymentIntentId = PaymentIntent.Id;
                Basket.ClientSecret = PaymentIntent.ClientSecret;

                BasketService.AddOrUpdateBasket(Basket);
            }

            return Basket;
        }

        public bool UpdateOrderStatusBasedOnResultOfPaymentOperation(string PaymentIntentId, bool Success)
        {
            var Spec = new Specifications<Order, int>();
            Spec.Criteria = P => P.PaymentIntentId == PaymentIntentId;

            var Order = UnitOfWork.Repo<Order, int>().GetById(Spec).FirstOrDefault();


            if(Success)
            {
                Order.Status = OrderStatus.PaymentRecived;
            }
            else
            {
                Order.Status = OrderStatus.PaymentFaild;
            }

            bool Flag = UnitOfWork.Repo<Order, int>().Update(Order);

            if(Flag)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
