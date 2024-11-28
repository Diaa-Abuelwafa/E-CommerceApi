using E_CommerceDomain.Entities.Order_Module;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.Configurations.Order_Module
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Configurations To Store Status In DB As String And Retrive It As Enum
            builder.Property(x => x.Status)
                   .HasConversion(x => x.ToString(), x => (OrderStatus)Enum.Parse(typeof(OrderStatus), x));


            // To Consider The Shipping Address As Properties Not Mape It As Table
            builder.OwnsOne(x => x.ShippingAddress, x => x.WithOwner());

            // Relationship With The DeliveryMethod Entity
            builder.HasOne(x => x.DeliveryMethod)
                   .WithMany()
                   .HasForeignKey(x => x.DeliveryMethodId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.Items)
                   .WithMany();
        }
    }
}
