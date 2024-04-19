using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCMS.Data.Models;

namespace TCMS.Data.Generators
{
    public static class ManifestGenerator
    {
        public static List<Manifest> GenerateManifests(int count)
        {
            var faker = new Faker();
            var manifests = new List<Manifest>();

            for (int i = 0; i < count; i++)
            {
                var manifest = new Manifest
                {
                    
                };
                // Generate and assign ManifestItems to the manifest here
                manifests.Add(manifest);
            }

            return manifests;
        }
    }
}
