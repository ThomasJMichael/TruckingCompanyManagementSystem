using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TCMS.Data.Models;

namespace TCMS.Data.Generators
{
    public static class InventoryGenerator
    {
        public static Inventory GenerateInventory(int productId)
        {
            var faker = new Faker();
            var inventory = new Inventory
            {
                ProductId = productId,
                QuantityOnHand = faker.Random.Int(0, 100) // Generate a random initial inventory quantity
            };

            return inventory;
        }
    }

}
