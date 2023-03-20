using BirokratNext;
using BironextWordpressIntegrationHub.structs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace primer_obravanja_partnerjev {
    public interface IVatIdParser {
        Task<string> Get(WoocommerceOrder order);
    }

    public class NopVatParser : IVatIdParser {
        public async Task<string> Get(WoocommerceOrder order) {
            return "";
        }
    }

    public class VatNumberParser : IVatIdParser {
        public async Task<string> Get(WoocommerceOrder order) {
            return VatIdHelper.GetVatIdFromMetadata(order, "_vat_number");
        }
    }

    public class YwEuVatIdParser : IVatIdParser {
        public async Task<string> Get(WoocommerceOrder order) {
            return VatIdHelper.GetVatIdFromMetadata(order, "yweu_billing_vat");
        }
    }

    public class EstradaVatIdParser : IVatIdParser {

        ApiClientV2 client;

        public EstradaVatIdParser(ApiClientV2 client) {
            this.client = client;
        }

        public async Task<string> Get(WoocommerceOrder order) {

            string davcna = await new YwEuVatIdParser().Get(order);

            davcna = davcna.Replace(" ", "");

            if (davcna.Contains("HR")) {
                return davcna;
            } else if (davcna.Contains("SI")) {
                return davcna;
            } else if (davcna.Contains("OIB")) {
                return davcna;
            }

            if (string.IsNullOrEmpty(davcna))
                return davcna;

            if (order.Data.Billing.Country == "HR") {
                if (IsVatExempt(order)) {
                    davcna = $"HR{davcna}";
                } else {
                    davcna = $"OIB{davcna}";
                }
            } else if (order.Data.Billing.Country == "SI") {

                string some = await client.utilities.DavcnaStevilka("utilities/partner/checkvatid", davcna);
                if (some.Length > 5) {
                    davcna = JsonConvert.DeserializeAnonymousType(some, new { DavcnaStevilka = "" }).DavcnaStevilka;
                }
                Console.WriteLine();


            }
            return davcna;
        }

        private static bool IsVatExempt(WoocommerceOrder order) {
            bool isvatexempt = false;
            var some = order.Data.MetaData.Where(x => x.Key == "is_vat_exempt").ToList();
            if (some.Count > 0) {
                string isVatExempt = Convert.ToString(some.First().Value);

                if (isVatExempt == "yes")
                    isvatexempt = true;
            }
            double sum = 0;
            foreach (var item in order.Items) {
                sum += double.Parse(item.SubtotalTax) + double.Parse(item.TotalTax);
            }
            if (sum == 0) {
                if (sum != 0 && isvatexempt == true) {
                    throw new Exception("The order said that vat exempt is true, but the sum of VATs was not 0");
                }
                isvatexempt = true;
            }

            return isvatexempt;
        }
    }

    public class VatIdHelper {
        public static string GetVatIdFromMetadata(WoocommerceOrder order, string attr_name) {
            var some = order.Data.MetaData.Where(x => x.Key == attr_name).ToList();
            string davcna = "";
            if (some.Count > 0) {
                var tmp = some.First().Value;
                davcna = Convert.ToString(tmp);
            }

            return davcna;
        }
    }
}
