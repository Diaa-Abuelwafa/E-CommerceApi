﻿using E_CommerceDomain.Entities.Product_Module;
using E_CommerceRepository.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_CommerceRepository.Data.Helper
{
    public static class SeedData
    {
        public static void Seed(AppDbContext Context)
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
        }
    }
}
