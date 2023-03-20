using BirokratNext;
using BironextWordpressIntegrationHub.structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace primer_obravanja_partnerjev {
    public interface IWooToBiroPartnerSearcher {
        Task<Dictionary<string, object>> MatchWooToBiroUser(WoocommerceOrder order, Dictionary<string, string> additionalInfo);
    }

    public class DavcnaWithVariablePrefixSearcher : IWooToBiroPartnerSearcher {

        /*
         Match by davcna stevilka with the following rules:
        - if davcna has no prefix, it will match it to EXACTLY this one or nothing!
        - if davcna has a prefix, it will remove all whitespace and match it EXACTLY like that.

        This means:
        - Suppose we have OIB 0100101 on db
            - OIB 0100101 will find no matches
            - OIB0100101 will find no matches
        - Suppose we have OIB0100101 on db
            - OIB 0100101 will be matched with it
            - OIB0100101 will be matched with it


        When do you use this mapper?
        - If you have a situation where you have the same partner added twice, once with whitespace
          and once without whitespace. You need to be able to return the same partner EVERY TIME
          this happens!
         */

        ApiClientV2 client;
        IVatIdParser vatIdParser;

        public DavcnaWithVariablePrefixSearcher(ApiClientV2 client,
            IVatIdParser vatIdParser) {
            this.client = client;
            this.vatIdParser = vatIdParser;
        }

        public async Task<Dictionary<string, object>> MatchWooToBiroUser(WoocommerceOrder order, Dictionary<string, string> additionalInfo) {

            string davcna = await vatIdParser.Get(order);
            if (!string.IsNullOrEmpty(davcna)) {
                return await searchByDavcna(davcna);
            }
            return null;
        }

        async Task<Dictionary<string, object>> searchByDavcna(string davcna_original) {
            List<Dictionary<string, object>> results = null;

            string davcna_no_prefix = OnlyDigits(davcna_original);
            results = await GetDavcnaMatch_DisregardWhitespace(davcna_original, results, davcna_no_prefix);

            if (results == null || results.Count == 0)
                return null;

            // try find exact match
            var match = results.Where(x => (string)x["Davčna številka"] == davcna_original).ToList();

            if (match.Count == 1) {
                return match[0];
            } else {
                // else take first one
                return null;
            }
        }

        private async Task<List<Dictionary<string, object>>> GetDavcnaMatch_DisregardWhitespace(string davcna_original, List<Dictionary<string, object>> results, string davcna_no_prefix) {

            Func<string, string> rmws = (x) => x.Replace(" ", "");
            results = await PartnerSearchHelper.findmatches(client,
                $"*{davcna_no_prefix}*",
                (x) => (rmws(((string)x["Davčna številka"])) == $"{rmws(davcna_original)}"));

            return results;
        }

        private static string OnlyDigits(string davcna_original) {
            return new string(davcna_original.Where(c => char.IsDigit(c)).ToArray());
        }



    }

    public class NazivUlicaSearcher : IWooToBiroPartnerSearcher {
        IPartnerWooToBiroMapper attributeMapper;
        ApiClientV2 client;
        public NazivUlicaSearcher(IPartnerWooToBiroMapper attributeMapper,
            ApiClientV2 client) {
            this.attributeMapper = attributeMapper;
            this.client = client;
        }

        public async Task<Dictionary<string, object>> MatchWooToBiroUser(WoocommerceOrder order, Dictionary<string, string> additionalInfo) {
            List<Dictionary<string, object>> results = null;
            string name = await attributeMapper.GetNaziv(order);
            results = await PartnerSearchHelper.findmatches(client, name, (x) => (string)x["Partner"] == name);

            if (results != null && results.Count > 0) {
                return TryMatchByUlica(order, results);
            } else { // no results
                return null;
            }
        }

        private Dictionary<string, object> TryMatchByUlica(WoocommerceOrder order, List<Dictionary<string, object>> results) {
            var some = results.Where(x => (string)x["Ulica"] == attributeMapper.GetUlica(order).GetAwaiter().GetResult()).ToList();
            if (some.Count > 0) {
                return some[0]; // correct match
            } else {
                return null;
            }
        }
    }

    public class PartnerSearchHelper {
        public async static Task<List<Dictionary<string, object>>> findmatches(ApiClientV2 client,
                string searchterm,
                Func<Dictionary<string, object>, bool> comparison) {
            string sifrantPartnerjevPath = @"sifranti\poslovnipartnerjiinosebe\poslovnipartnerji";
            var matches = await client.sifrant.Podatki(sifrantPartnerjevPath, searchterm);
            var truematches = new List<Dictionary<string, object>>();
            foreach (var x in matches) {

                if (comparison(x)) {
                    truematches.Add(x);
                }

            }
            return truematches;
        }
    }
}
