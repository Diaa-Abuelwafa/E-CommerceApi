using E_CommerceDomain.Entities.Product_Module;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.Configurations.Product_Module
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Relationship With ProductBrand
            builder.HasOne(x => x.Brand)
                   .WithMany(x => x.Products)
                   .HasForeignKey(x => x.BrandId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relationship With ProductType
            builder.HasOne(x => x.Type)
                   .WithMany(x => x.Products)
                   .HasForeignKey(x => x.TypeId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Configurations
            builder.Property(x => x.Price)
                   .HasColumnType("decimal(18,2)");
        }
    }
}
