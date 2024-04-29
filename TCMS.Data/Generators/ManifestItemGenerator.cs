using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Common.enums;
using TCMS.Data.Models;

namespace TCMS.Data.Generators
{
    public static class ManifestItemGenerator
    {
        public static List<ManifestItem> GenerateManifestItemsForManifest(int manifestId, int itemCount,
            List<Product> products)
        {
            var faker = new Faker();
            var manifestItems = new List<ManifestItem>();

            for (int i = 0; i < itemCount; i++)
            {
                var number = faker.Random.Int(1, products.Count - 1);
                var productMaxQuantity = products.Find(id => id.ProductId == number).Inventory.QuantityOnHand;
                var manifestItem = new ManifestItem
                {
                    ManifestId = manifestId,
                    ProductId = products[number].ProductId,
                    Status = faker.PickRandom<ItemStatus>(),
                    Quantity = faker.Random.Int(1, productMaxQuantity)

                };
                manifestItems.Add(manifestItem);
            }

            return manifestItems;
        }
    }

}
