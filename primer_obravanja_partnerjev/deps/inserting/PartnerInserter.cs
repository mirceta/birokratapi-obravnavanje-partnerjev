using BirokratNext;
using BironextWordpressIntegrationHub.structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace primer_obravanja_partnerjev {

    public interface ISifrantPartnerjevInserter {
        Task<string> EnforceWoocommerceBillingPartnerCreated(WoocommerceOrder order, Dictionary<string, string> additionalInfo);
    }
    public class SwitchOnDavcnaPartnerInserter : ISifrantPartnerjevInserter {

        OneSearcherPartnerInserter withDavcna;
        OneSearcherPartnerInserter withoutDavcna;
        IVatIdParser vatIdParser;

        public SwitchOnDavcnaPartnerInserter(ApiClientV2 client, IVatIdParser parser, IPartnerWooToBiroMapper mapper, bool povoziVseAtribute) {
            this.vatIdParser = parser;
            withDavcna = new OneSearcherPartnerInserter(client, parser, mapper,
                    new DavcnaWithVariablePrefixSearcher(client, parser), povoziVseAtribute);
            withoutDavcna = new OneSearcherPartnerInserter(client, parser, mapper,
                    new NazivUlicaSearcher(mapper, client), povoziVseAtribute);
        }

        public async Task<string> EnforceWoocommerceBillingPartnerCreated(WoocommerceOrder order, Dictionary<string, string> additionalInfo) {

            string davcna = await vatIdParser.Get(order);
            Console.WriteLine(davcna);
            if (!string.IsNullOrEmpty(davcna)) {
                additionalInfo = fillAdditionalInfo(additionalInfo, davcna);
                return await withDavcna.EnforceWoocommerceBillingPartnerCreated(order, additionalInfo);
            } else {
                return await withoutDavcna.EnforceWoocommerceBillingPartnerCreated(order, additionalInfo);
            }

        }

        private Dictionary<string, string> fillAdditionalInfo(Dictionary<string, string> additionalInfo, string davcna) {
            if (additionalInfo == null) {
                additionalInfo = new Dictionary<string, string>();
            }

            additionalInfo["VATID"] = davcna;
            return additionalInfo;
        }
    }

    public class OneSearcherPartnerInserter : ISifrantPartnerjevInserter {
        IWooToBiroPartnerSearcher searcher;
        IVatIdParser vatIdParser;
        IPartnerWooToBiroMapper attributeMapper;
        ApiClientV2 client;
        bool povoziVsePartnerjeveAtribute;

        public OneSearcherPartnerInserter(ApiClientV2 client,
            IVatIdParser parser,
            IPartnerWooToBiroMapper attributeMapper,
            IWooToBiroPartnerSearcher searcher,
            bool povoziVsePartnerjeveAtribute = false) {
            if (client == null)
                throw new ArgumentNullException("client");
            if (attributeMapper == null)
                throw new ArgumentNullException("attributeMapper");
            if (parser == null)
                throw new ArgumentNullException("parser");
            if (searcher == null)
                throw new ArgumentNullException("searcher");
            this.searcher = searcher;
            this.attributeMapper = attributeMapper;
            this.vatIdParser = parser;
            this.client = client;
            this.povoziVsePartnerjeveAtribute = povoziVsePartnerjeveAtribute;
        }

        public async Task<string> EnforceWoocommerceBillingPartnerCreated(WoocommerceOrder order, Dictionary<string, string> additionalInfo) {

            string woodavcna = "";
            if (additionalInfo != null && additionalInfo.ContainsKey("VATID")) {
                woodavcna = additionalInfo["VATID"];
            } else {
                woodavcna = await vatIdParser.Get(order);
            }

            var match = await searcher.MatchWooToBiroUser(order, additionalInfo);

            if (match == null) {
                return await new PartnerInserterHelper(client, attributeMapper).UstvariNovegaPartnerja(order, woodavcna);
            } else if (povoziVsePartnerjeveAtribute) {
                await new PartnerInserterHelper(client, attributeMapper).PovoziObstojecegaPartnerja(order, (string)match["Oznaka"]);
            }

            return (string)match["Oznaka"];
        }
    }

    public class PartnerInserterHelper {

        const string SIFRANTPARTNERJEVPATH = @"sifranti\poslovnipartnerjiinosebe\poslovnipartnerji";

        ApiClientV2 client;
        IPartnerWooToBiroMapper mapper;

        public PartnerInserterHelper(ApiClientV2 client,
            IPartnerWooToBiroMapper mapper) {
            this.client = client;
            this.mapper = mapper;
        }

        public async Task<string> UstvariNovegaPartnerja(WoocommerceOrder order, string davcna) {
            var pack = await mapper.GetPackedParameters(order);
            if (!string.IsNullOrEmpty(davcna)) {
                pack["txtDavcnaStevilka"] = davcna;
                pack["IDStevilka"] = davcna;
            }
            string sifra = (string)await client.sifrant.Create(SIFRANTPARTNERJEVPATH, pack);
            return sifra;
        }

        public async Task PovoziObstojecegaPartnerja(WoocommerceOrder order, string oznaka) {

            // you must call UpdateParameters so that the PL gets filled with the correct partner's attributes!
            var pars = await client.sifrant.UpdateParameters(SIFRANTPARTNERJEVPATH, oznaka);

            var pack = await mapper.GetPackedParameters(order);
            pack["txtSifraPartnerja"] = oznaka;
            string sifra = (string)await client.sifrant.Update(SIFRANTPARTNERJEVPATH, pack);
        }
    }
}
