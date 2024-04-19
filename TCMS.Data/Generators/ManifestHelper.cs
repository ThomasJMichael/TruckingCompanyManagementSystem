using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Data.Models;

namespace TCMS.Data.Generators
{
    public static class ManifestHelper
    {
        public static List<List<ManifestItem>> DistributeManifestItems(List<ManifestItem> items, int numberOfShipments)
        {
            var shuffledItems = items.OrderBy(a => Guid.NewGuid()).ToList(); // Shuffle items to randomize distribution
            var result = new List<List<ManifestItem>>();

            int remainingItems = shuffledItems.Count;
            int startIndex = 0;
            for (int i = 0; i < numberOfShipments; i++)
            {
                int takeCount = remainingItems / (numberOfShipments - i); // Evenly distribute remaining items
                var shipmentItems = shuffledItems.GetRange(startIndex, takeCount);
                result.Add(shipmentItems);
                startIndex += takeCount;
                remainingItems -= takeCount;
            }

            return result;
        }
    }

}
