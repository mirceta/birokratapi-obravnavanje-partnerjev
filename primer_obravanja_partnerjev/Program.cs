using BirokratNext;
using BironextWordpressIntegrationHub.structs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace primer_obravanja_partnerjev {
    class Program {
        static async Task Main(string[] args) {
            var inserter = GetInserter();
            foreach (var order in GetLocalFolder_OrderJsons()) {
                string oznaka = await inserter.EnforceWoocommerceBillingPartnerCreated(order, null);
                Console.WriteLine($"BirokratId: {oznaka}");
            }
        }

        static ISifrantPartnerjevInserter GetInserter() {
            var client = new ApiClientV2("http://localhost:19000/api/", "testapikey"); // http://localhost:19000/api/ https://next.birokrat.si/api/
            var countryMapper = new HardcodedCountryMapper(); 
            var vatIdParser = new EstradaVatIdParser(client);
            var statusZavMapper = new EstradaStatusPartnerjaMapper(vatIdParser);
            var partnerInserter = new SwitchOnDavcnaPartnerInserter(client, vatIdParser,
                new PartnerWooToBiroMapper1(countryMapper, statusZavMapper, statusZavMapper),
                povoziVseAtribute: true);
            return partnerInserter;
        }

        public static IEnumerable<WoocommerceOrder> GetLocalFolder_OrderJsons() {
            foreach (var file in Directory.GetFiles(Path.Combine(Build.ProjectPath, "data"))) {
                var strOrder = File.ReadAllText(file);
                var order = JsonConvert.DeserializeObject<WoocommerceOrder>(strOrder);
                yield return order;
            }
        }
    }
}
