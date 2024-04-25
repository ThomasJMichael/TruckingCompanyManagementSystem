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
    public static class PurchaseOrderGenerator
    {
        private static int _manifestIndex = 0;
        public static List<PurchaseOrder> GeneratePurchaseOrders(int count, List<Manifest> manifests)
        {
            var purchaseOrderFaker = new Faker<PurchaseOrder>()
                .RuleFor(o => o.Company, f => f.Company.CompanyName())
                .RuleFor(o => o.Address, f => f.Address.StreetAddress())
                .RuleFor(o => o.City, f => f.Address.City())
                .RuleFor(o => o.State, f => f.Address.State())
                .RuleFor(o => o.Zip, f => f.Address.ZipCode())
                .RuleFor(o => o.Direction, f => f.PickRandom<ShipmentDirection>())
                .RuleFor(o => o.DateCreated, f => f.Date.Past(1))
                .RuleFor(o => o.ShippingCost, f => f.Finance.Amount(10, 100))
                .RuleFor(o => o.ShippingPaid, f => f.Random.Bool())
                .RuleFor(o => o.Manifest, f => f.PickRandom(manifests))
                .FinishWith((f, po) =>
                {
                    if (_manifestIndex >= manifests.Count)
                    {
                        _manifestIndex = 0;
                    }

                    po.Manifest = manifests[_manifestIndex];
                    po.ManifestId = po.Manifest.ManifestId;

                    _manifestIndex++;
                });

            return purchaseOrderFaker.Generate(count);
        }
    }

}
