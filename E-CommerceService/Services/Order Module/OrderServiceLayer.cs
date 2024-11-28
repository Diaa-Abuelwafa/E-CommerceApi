using E_CommerceDomain.DTOs.Order_Module;
using E_CommerceDomain.Entities.Basket_Module;
using E_CommerceDomain.Entities.Order_Module;
using E_CommerceDomain.Entities.Product_Module;
using E_CommerceDomain.Interfaces;
using E_CommerceDomain.Interfaces.Basket_Module;
using E_CommerceDomain.Interfaces.Order_Module;
using E_CommerceDomain.Specifications;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceService.Services.Order_Module
{
    public class OrderServiceLayer : IOrderService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IBasketServices BasketService;
        private readonly IConfiguration Config;

        public OrderServiceLayer(IUnitOfWork UnitOfWork, IBasketServices BasketService, IConfiguration Config)
        {
            this.UnitOfWork = UnitOfWork;
            this.BasketService = BasketService;
            this.Config = Config;
        }
        public OrderResponseDTO CreateOrder(string BuyerEmail, AddressDTO ShippingAddress, int DeliveryMethodId, string BasketId)
        {
            // Make Object From Class Address To Make A Order
            Address ShipTo = new Address()
            {
                Street = ShippingAddress.Street,
                City = ShippingAddress.City,
                Country = ShippingAddress.Country
            };


            // Make Object From Class DeliveryMethod To Make A Order
            var Spec = new Specifications<DeliveryMethod, int>();
            Spec.Criteria = P => P.Id == DeliveryMethodId;
            DeliveryMethod Delivery = UnitOfWork.Repo<DeliveryMethod, int>().GetById(Spec).FirstOrDefault(x => x.Id == DeliveryMethodId);


            if(Delivery is null)
            {
                return null; 
            }

            // Make List<OrderItem> To Make A Order
            var Basket = BasketService.GetBasketById(BasketId);

            if(Basket is null)
            {
                return null;
            }

            List<OrderItem> Items = new List<OrderItem>();
            
            // Calculate The Price Without The DeliveryMethodCost
            decimal SubTotal = 0;

            foreach (var Item in Basket.Items)
            {
                OrderItem I = new OrderItem()
                {
                    Product = new ProductItemOrder()
                    {
                        ProductId = Item.Id,
                        ProductName = Item.Name,
                        PictureUrl = $"{Config["BaseUrl"]}{Item.PictureUrl}"
                    },

                    Price = UnitOfWork.Repo<Product, int>().GetByIdWithoutSpecifications(Item.Id).Price,

                    Quantity = Item.Quantity
                };

                SubTotal += I.Price * I.Quantity;

                Items.Add(I);
            }

            // Create The Order
            var NewOrder = new Order(BuyerEmail, ShipTo, Delivery, Items, SubTotal, "");

            // Add This Order In DB
            UnitOfWork.Repo<Order, int>().Add(NewOrder);

            OrderResponseDTO OrderResponse = new OrderResponseDTO()
            {
                BuyerEmail = NewOrder.BuyerEmail,
                OrderDate = NewOrder.OrderDate,
                Status = NewOrder.Status.ToString(),
                ShippingAddress = new AddressDTO()
                {
                    Street = NewOrder.ShippingAddress.Street,
                    City = NewOrder.ShippingAddress.City,
                    Country = NewOrder.ShippingAddress.Country
                },
                DeliveryMethodShortName = NewOrder.DeliveryMethod.ShortName,
                Total = NewOrder.GetTotal()

            };

            List<OrderItemDTO> ItemsDTO = new List<OrderItemDTO>();
            foreach(var Item in NewOrder.Items)
            {
                OrderItemDTO O = new OrderItemDTO()
                {
                    ProductId = Item.Product.ProductId,
                    ProductName = Item.Product.ProductName,
                    PictureUrl = $"{Config["BaseUrl"]}{Item.Product.PictureUrl}",
                    Price = Item.Price,
                    Quantity = Item.Quantity
                };

                ItemsDTO.Add(O);
                UnitOfWork.SaveChanges();
            }

            OrderResponse.Items = ItemsDTO;

            return OrderResponse;
        }

        public List<DeliveryMethodDTO> GetAllDeliveryMethods()
        {
            var Spec = new Specifications<DeliveryMethod, int>();
            var DeliveryMethods = UnitOfWork.Repo<DeliveryMethod, int>().GetAll(Spec);

            if(DeliveryMethods is null)
            {
                return null;
            }

            List<DeliveryMethodDTO> Methods = new List<DeliveryMethodDTO>();

            foreach(var D in DeliveryMethods)
            {
                var Method = new DeliveryMethodDTO()
                {
                    ShortName = D.ShortName,
                    DeliveryTime = D.DeliveryTime,
                    Cost = D.Cost
                };

                Methods.Add(Method);
            }

            return Methods;
        }

        public List<OrderResponseDTO> GetAllOrdersForCurrentUser(string BuyerEmail)
        {
            var Spec = new Specifications<Order, int>();
            Spec.Criteria = P => P.BuyerEmail == BuyerEmail;
            Spec.Includes.Add(P => P.DeliveryMethod);
            Spec.Includes.Add(P => P.Items);

            var Orders = UnitOfWork.Repo<Order, int>().GetById(Spec);

            if(Orders is null)
            {
                return null;
            }

            List<OrderResponseDTO> ResponseOrders = new List<OrderResponseDTO>();
            
            foreach(var Order in Orders)
            {
                OrderResponseDTO OrderResponse = new OrderResponseDTO()
                {
                    BuyerEmail = Order.BuyerEmail,
                    OrderDate = Order.OrderDate,
                    Status = Order.Status.ToString(),
                    ShippingAddress = new AddressDTO()
                    {
                        Street = Order.ShippingAddress.Street,
                        City = Order.ShippingAddress.City,
                        Country = Order.ShippingAddress.Country
                    },
                    DeliveryMethodShortName = Order.DeliveryMethod.ShortName,
                    Total = Order.GetTotal()

                };

                List<OrderItemDTO> ItemsDTO = new List<OrderItemDTO>();
                foreach (var Item in Order.Items)
                {
                    OrderItemDTO O = new OrderItemDTO()
                    {
                        ProductId = Item.Product.ProductId,
                        ProductName = Item.Product.ProductName,
                        PictureUrl = $"{Config["BaseUrl"]}{Item.Product.PictureUrl}",
                        Price = Item.Price,
                        Quantity = Item.Quantity
                    };

                    ItemsDTO.Add(O);
                }

                OrderResponse.Items = ItemsDTO;

                ResponseOrders.Add(OrderResponse);
            }

            return ResponseOrders;
        }

        public OrderResponseDTO GetOrderByIdForCurrentUser(int OrderId, string BuyerEmail)
        {
            var Spec = new Specifications<Order, int>();
            Spec.Criteria = P => P.BuyerEmail == BuyerEmail;
            Spec.Includes.Add(P => P.DeliveryMethod);
            Spec.Includes.Add(P => P.Items);

            var Order = UnitOfWork.Repo<Order, int>().GetById(Spec).FirstOrDefault(x => x.Id == OrderId);

            if (Order is null)
            {
                return null;
            }

            OrderResponseDTO OrderResponse = new OrderResponseDTO()
            {
                BuyerEmail = Order.BuyerEmail,
                OrderDate = Order.OrderDate,
                Status = Order.Status.ToString(),
                ShippingAddress = new AddressDTO()
                {
                    Street = Order.ShippingAddress.Street,
                    City = Order.ShippingAddress.City,
                    Country = Order.ShippingAddress.Country
                },
                DeliveryMethodShortName = Order.DeliveryMethod.ShortName,
                Total = Order.GetTotal()

            };

            List<OrderItemDTO> ItemsDTO = new List<OrderItemDTO>();
            foreach (var Item in Order.Items)
            {
                OrderItemDTO O = new OrderItemDTO()
                {
                    ProductId = Item.Product.ProductId,
                    ProductName = Item.Product.ProductName,
                    PictureUrl = $"{Config["BaseUrl"]}{Item.Product.PictureUrl}",
                    Price = Item.Price,
                    Quantity = Item.Quantity
                };

                ItemsDTO.Add(O);
            }

            OrderResponse.Items = ItemsDTO;

            return OrderResponse;
        }
    }
}
