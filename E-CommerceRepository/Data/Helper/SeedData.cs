using E_CommerceDomain.Entities.Account_Module;
using E_CommerceDomain.Entities.Order_Module;
using E_CommerceDomain.Entities.Product_Module;
using E_CommerceRepository.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Address = E_CommerceDomain.Entities.Account_Module.Address;

namespace E_CommerceRepository.Data.Helper
{
    public static class SeedData
    {
        public static void Seed(AppDbContext Context, AccountDbContext AccountContext)
        {
            // Add Brands SeedData To DB
            if(Context.Brands.Count() == 0)
            {
                var BrandsFromFile = File.ReadAllText("../E-CommerceRepository/Data/SeedDataFiles/Product Module/brands.json");

                if(BrandsFromFile is not null)
                {
                    var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsFromFile);

                    Context.Brands.AddRange(Brands);

                    // After Add These Seed Data Make Save Changes
                    Context.SaveChanges();
                }
            }


            // Add Types SeedData To DB
            if (Context.Types.Count() == 0)
            {
                var TypesFromFile = File.ReadAllText("../E-CommerceRepository/Data/SeedDataFiles/Product Module/types.json");

                if (TypesFromFile is not null)
                {
                    var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesFromFile);

                    Context.Types.AddRange(Types);

                    // After Add These Seed Data Make Save Changes
                    Context.SaveChanges();
                }
            }


            // Add Products SeedData To DB
            if (Context.Products.Count() == 0)
            {
                var ProductsFromFile = File.ReadAllText("../E-CommerceRepository/Data/SeedDataFiles/Product Module/products.json");

                if (ProductsFromFile is not null)
                {
                    var Products = JsonSerializer.Deserialize<List<Product>>(ProductsFromFile);

                    Context.Products.AddRange(Products);

                    // After Add These Seed Data Make Save Changes
                    Context.SaveChanges();
                }
            }

            // Add Addresses Seed Data To Identity DB
            if(AccountContext.Addresses.Count() == 0)
            {
                var AddresseJson = File.ReadAllText("../E-CommerceRepository/Data/SeedDataFiles/Account Module/Addresses.json");

                if(AddresseJson is not null && AddresseJson.Count() > 0)
                {
                    var Addresses = JsonSerializer.Deserialize<List<Address>>(AddresseJson);

                    AccountContext.Addresses.AddRange(Addresses);

                    AccountContext.SaveChanges();
                }
            }

            if(Context.DeliveryMethods.Count() == 0)
            {
                var DataFromFile = File.ReadAllText("../E-CommerceRepository/Data/SeedDataFiles/Order Module/delivery.json");

                if(DataFromFile is not null)
                {
                    var Data = JsonSerializer.Deserialize<List<DeliveryMethod>>(DataFromFile);

                    Context.DeliveryMethods.AddRange(Data);

                    Context.SaveChanges();
                }
            }
        }
    }
}
