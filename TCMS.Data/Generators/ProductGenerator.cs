﻿using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Data.Models;

namespace TCMS.Data.Generators
{
    public static class ProductGenerator
    {
        // Generates and returns a list of fake products
        public static List<Product> GenerateProducts(int count)
        {
            var productFaker = new Faker<Product>()
                // Rule for generating a product ID - assuming it's an auto-incremented field, you might not need to set this
                //.RuleFor(p => p.ProductId, f => f.IndexFaker + 1)

                // Rule for generating a product name
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                // Rule for generating a product description
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                // Rule for generating a product price
                .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000));

            return productFaker.Generate(count);
        }
    }
}
