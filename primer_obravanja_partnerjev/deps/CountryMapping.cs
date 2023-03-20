﻿using BirokratNext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace primer_obravanja_partnerjev {
    public interface ICountryMapper {
        Task<string> Map(string value);
    }

    public class WooToBiroCountryMapper : ICountryMapper {

        ApiClientV2 client;
        List<Dictionary<string, object>> data;
        public WooToBiroCountryMapper(ApiClientV2 client) {
            this.client = client;
        }
        public async Task Setup() {
            await client.sifrant.Pagelen(1000);
            var x = await client.sifrant.Podatki(@"sifranti\drzave");
            data = x;
        }
        public async Task<string> Map(string value) {
            if (string.IsNullOrEmpty(value)) return value;
            string iso = GCountryMapper.WooCountryCodeToISOCountryCode(value);

            int cnt = data.Where(x => (string)x["Šifra ISO"] == iso).Count();
            var some = data.Where(x => (string)x["Šifra ISO"] == iso).ToList();
            if (cnt == 0) {
                throw new IntegrationProcessingException("This country is not handled!");
            } else if (cnt > 1) {
                throw new IntegrationProcessingException("Multiple countries have this code!");
            }

            var entry = data.Where(x => (string)x["Šifra ISO"] == iso).Single();
            return (string)entry["Oznaka države"]; // + entry["Ime države"]?

        }

    }

    public class WooStaticCountryMapper : ICountryMapper {

        Dictionary<string, string> input;
        string defaultCountry = null;
        public WooStaticCountryMapper(Dictionary<string, string> input, string defaultCountry = null) {
            this.input = input;
            this.defaultCountry = defaultCountry;
        }

        public async Task<string> Map(string value) {
            if (input.ContainsKey(value))
                return input[value];
            if (defaultCountry != null)
                return defaultCountry;
            throw new IntegrationProcessingException("This country is not handled!");
        }
    }

    public class HardcodedCountryMapper : ICountryMapper {

        string defaultCountry;
        public HardcodedCountryMapper(string defaultCountry = null) {
            this.defaultCountry = defaultCountry;
        }

        public async Task<string> Map(string value) {
            if (value == "HR")
                return "HRV";
            else if (value == "SI")
                return "SLO";

            if (defaultCountry != null)
                return defaultCountry;
            throw new IntegrationProcessingException("This country is not handled");
        }
    }

    public class GCountryMapper {
        public static string Map(string country_code) {
            var k = CountryMap();
            string result = "";
            if (k.ContainsKey(country_code))
                result = CountryMap()[country_code];
            return result;
        }

        public static string WooCountryCodeToBirokratCountryCode(string country_code) {
            var k = WooCountryCodeToBiroDropdownOption();
            string result = "";
            if (k.ContainsKey(country_code))
                result = k[country_code];
            return result;
        }

        public static string WooCountryCodeToISOCountryCode(string country_code) {
            var k = WooCountryCodeToISOCountry();
            string result = "";
            if (k.ContainsKey(country_code))
                result = k[country_code];
            return result;
        }

        private static Dictionary<string, string> CountryMap() {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["AF"] = "Afghanistan";
            dict["AX"] = "&#197;land Islands";
            dict["AL"] = "Albania";
            dict["DZ"] = "Algeria";
            dict["AS"] = "American Samoa";
            dict["AD"] = "Andorra";
            dict["AO"] = "Angola";
            dict["AI"] = "Anguilla";
            dict["AQ"] = "Antarctica";
            dict["AG"] = "Antigua and Barbuda";
            dict["AR"] = "Argentina";
            dict["AM"] = "Armenia";
            dict["AW"] = "Aruba";
            dict["AU"] = "Australia";
            dict["AT"] = "Austria";
            dict["AZ"] = "Azerbaijan";
            dict["BS"] = "Bahamas";
            dict["BH"] = "Bahrain";
            dict["BD"] = "Bangladesh";
            dict["BB"] = "Barbados";
            dict["BY"] = "Belarus";
            dict["BE"] = "Belgium";
            dict["BZ"] = "Belize";
            dict["BJ"] = "Benin";
            dict["BM"] = "Bermuda";
            dict["BT"] = "Bhutan";
            dict["BO"] = "Bolivia";
            dict["BQ"] = "Bonaire, Saint Estatius and Saba";
            dict["BA"] = "Bosnia and Herzegovina";
            dict["BW"] = "Botswana";
            dict["BV"] = "Bouvet Islands";
            dict["BR"] = "Brazil";
            dict["IO"] = "British Indian Ocean Territory";
            dict["BN"] = "Brunei";
            dict["BG"] = "Bulgaria";
            dict["BF"] = "Burkina Faso";
            dict["BI"] = "Burundi";
            dict["KH"] = "Cambodia";
            dict["CM"] = "Cameroon";
            dict["CA"] = "Canada";
            dict["CV"] = "Cape Verde";
            dict["KY"] = "Cayman Islands";
            dict["CF"] = "Central African Republic";
            dict["TD"] = "Chad";
            dict["CL"] = "Chile";
            dict["CN"] = "China";
            dict["CX"] = "Christmas Island";
            dict["CC"] = "Cocos (Keeling) Islands";
            dict["CO"] = "Colombia";
            dict["KM"] = "Comoros";
            dict["CG"] = "Congo";
            dict["CD"] = "Congo, Democratic Republic of the";
            dict["CK"] = "Cook Islands";
            dict["CR"] = "Costa Rica";
            dict["CI"] = "Côte d'Ivoire";
            dict["HR"] = "Croatia";
            dict["CU"] = "Cuba";
            dict["CW"] = "Curaçao";
            dict["CY"] = "Cyprus";
            dict["CZ"] = "Czech Republic";
            dict["DK"] = "Denmark";
            dict["DJ"] = "Djibouti";
            dict["DM"] = "Dominica";
            dict["DO"] = "Dominican Republic";
            dict["EC"] = "Ecuador";
            dict["EG"] = "Egypt";
            dict["SV"] = "El Salvador";
            dict["GQ"] = "Equatorial Guinea";
            dict["ER"] = "Eritrea";
            dict["EE"] = "Estonia";
            dict["ET"] = "Ethiopia";
            dict["FK"] = "Falkland Islands";
            dict["FO"] = "Faroe Islands";
            dict["FJ"] = "Fiji";
            dict["FI"] = "Finland";
            dict["FR"] = "France";
            dict["GF"] = "French Guiana";
            dict["PF"] = "French Polynesia";
            dict["TF"] = "French Southern Territories";
            dict["GA"] = "Gabon";
            dict["GM"] = "Gambia";
            dict["GE"] = "Georgia";
            dict["DE"] = "Germany";
            dict["GH"] = "Ghana";
            dict["GI"] = "Gibraltar";
            dict["GR"] = "Greece";
            dict["GL"] = "Greenland";
            dict["GD"] = "Grenada";
            dict["GP"] = "Guadeloupe";
            dict["GU"] = "Guam";
            dict["GT"] = "Guatemala";
            dict["GG"] = "Guernsey";
            dict["GN"] = "Guinea";
            dict["GW"] = "Guinea-Bissau";
            dict["GY"] = "Guyana";
            dict["HT"] = "Haiti";
            dict["HM"] = "Heard Island and McDonald Islands";
            dict["VA"] = "Holy See (Vatican City State)";
            dict["HN"] = "Honduras";
            dict["HK"] = "Hong Kong";
            dict["HU"] = "Hungary";
            dict["IS"] = "Iceland";
            dict["IN"] = "India";
            dict["ID"] = "Indonesia";
            dict["IR"] = "Iran";
            dict["IQ"] = "Iraq";
            dict["IE"] = "Republic of Ireland";
            dict["IM"] = "Isle of Man";
            dict["IL"] = "Israel";
            dict["IT"] = "Italy";
            dict["JM"] = "Jamaica";
            dict["JP"] = "Japan";
            dict["JE"] = "Jersey";
            dict["JO"] = "Jordan";
            dict["KZ"] = "Kazakhstan";
            dict["KE"] = "Kenya";
            dict["KI"] = "Kiribati";
            dict["KP"] = "Korea, Democratic People's Republic of";
            dict["KR"] = "Korea, Republic of (South)";
            dict["KW"] = "Kuwait";
            dict["KG"] = "Kyrgyzstan";
            dict["LA"] = "Laos";
            dict["LV"] = "Latvia";
            dict["LB"] = "Lebanon";
            dict["LS"] = "Lesotho";
            dict["LR"] = "Liberia";
            dict["LY"] = "Libya";
            dict["LI"] = "Liechtenstein";
            dict["LT"] = "Lithuania";
            dict["LU"] = "Luxembourg";
            dict["MO"] = "Macao S.A.R., China";
            dict["MK"] = "Macedonia";
            dict["MG"] = "Madagascar";
            dict["MW"] = "Malawi";
            dict["MY"] = "Malaysia";
            dict["MV"] = "Maldives";
            dict["ML"] = "Mali";
            dict["MT"] = "Malta";
            dict["MH"] = "Marshall Islands";
            dict["MQ"] = "Martinique";
            dict["MR"] = "Mauritania";
            dict["MU"] = "Mauritius";
            dict["YT"] = "Mayotte";
            dict["MX"] = "Mexico";
            dict["FM"] = "Micronesia";
            dict["MD"] = "Moldova";
            dict["MC"] = "Monaco";
            dict["MN"] = "Mongolia";
            dict["ME"] = "Montenegro";
            dict["MS"] = "Montserrat";
            dict["MA"] = "Morocco";
            dict["MZ"] = "Mozambique";
            dict["MM"] = "Myanmar";
            dict["NA"] = "Namibia";
            dict["NR"] = "Nauru";
            dict["NP"] = "Nepal";
            dict["NL"] = "Netherlands";
            dict["AN"] = "Netherlands Antilles";
            dict["NC"] = "New Caledonia";
            dict["NZ"] = "New Zealand";
            dict["NI"] = "Nicaragua";
            dict["NE"] = "Niger";
            dict["NG"] = "Nigeria";
            dict["NU"] = "Niue";
            dict["NF"] = "Norfolk Island";
            dict["MP"] = "Northern Mariana Islands";
            dict["NO"] = "Norway";
            dict["OM"] = "Oman";
            dict["PK"] = "Pakistan";
            dict["PW"] = "Palau";
            dict["PS"] = "Palestinian Territory";
            dict["PA"] = "Panama";
            dict["PG"] = "Papua New Guinea";
            dict["PY"] = "Paraguay";
            dict["PE"] = "Peru";
            dict["PH"] = "Philippines";
            dict["PN"] = "Pitcairn";
            dict["PL"] = "Poland";
            dict["PT"] = "Portugal";
            dict["PR"] = "Puerto Rico";
            dict["QA"] = "Qatar";
            dict["RE"] = "Reunion";
            dict["RO"] = "Romania";
            dict["RU"] = "Russia";
            dict["RW"] = "Rwanda";
            dict["BL"] = "Saint Barth&eacute;lemy";
            dict["SH"] = "Saint Helena";
            dict["KN"] = "Saint Kitts and Nevis";
            dict["LC"] = "Saint Lucia";
            dict["MF"] = "Saint Martin (French part)";
            dict["SX"] = "Sint Maarten / Saint Matin (Dutch part)";
            dict["PM"] = "Saint Pierre and Miquelon";
            dict["VC"] = "Saint Vincent and the Grenadines";
            dict["WS"] = "Samoa";
            dict["SM"] = "San Marino";
            dict["ST"] = "S&atilde;o Tom&eacute; and Pr&iacute;ncipe";
            dict["SA"] = "Saudi Arabia";
            dict["SN"] = "Senegal";
            dict["RS"] = "Serbia";
            dict["SC"] = "Seychelles";
            dict["SL"] = "Sierra Leone";
            dict["SG"] = "Singapore";
            dict["SK"] = "Slovakia";
            dict["SI"] = "Slovenia";
            dict["SB"] = "Solomon Islands";
            dict["SO"] = "Somalia";
            dict["ZA"] = "South Africa";
            dict["GS"] = "South Georgia/Sandwich Islands";
            dict["SS"] = "South Sudan";
            dict["ES"] = "Spain";
            dict["LK"] = "Sri Lanka";
            dict["SD"] = "Sudan";
            dict["SR"] = "Suriname";
            dict["SJ"] = "Svalbard and Jan Mayen";
            dict["SZ"] = "Swaziland";
            dict["SE"] = "Sweden";
            dict["CH"] = "Switzerland";
            dict["SY"] = "Syria";
            dict["TW"] = "Taiwan";
            dict["TJ"] = "Tajikistan";
            dict["TZ"] = "Tanzania";
            dict["TH"] = "Thailand    ";
            dict["TL"] = "Timor-Leste";
            dict["TG"] = "Togo";
            dict["TK"] = "Tokelau";
            dict["TO"] = "Tonga";
            dict["TT"] = "Trinidad and Tobago";
            dict["TN"] = "Tunisia";
            dict["TR"] = "Turkey";
            dict["TM"] = "Turkmenistan";
            dict["TC"] = "Turks and Caicos Islands";
            dict["TV"] = "Tuvalu     ";
            dict["UG"] = "Uganda";
            dict["UA"] = "Ukraine";
            dict["AE"] = "United Arab Emirates";
            dict["GB"] = "United Kingdom";
            dict["US"] = "United States";
            dict["UM"] = "United States Minor Outlying Islands";
            dict["UY"] = "Uruguay";
            dict["UZ"] = "Uzbekistan";
            dict["VU"] = "Vanuatu";
            dict["VE"] = "Venezuela";
            dict["VN"] = "Vietnam";
            dict["VG"] = "Virgin Islands, British";
            dict["VI"] = "Virgin Island, U.S.";
            dict["WF"] = "Wallis and Futuna";
            dict["EH"] = "Western Sahara";
            dict["YE"] = "Yemen";
            dict["ZM"] = "Zambia";
            dict["ZW"] = "Zimbabwe";
            return dict;
        }

        private static Dictionary<string, string> WooCountryCodeToISOCountry() {
            var dict = new Dictionary<string, string>();
            dict["AD"] = "AND";
            dict["AE"] = "ARE";
            dict["AF"] = "AFG";
            dict["AG"] = "ATG";
            dict["AI"] = "AIA";
            dict["AL"] = "ALB";
            dict["AM"] = "ARM";
            dict["AN"] = "ANT";
            dict["AO"] = "AGO";
            dict["AQ"] = "ATA";
            dict["AR"] = "ARG";
            dict["AS"] = "ASM";
            dict["AW"] = "ABW";
            dict["AX"] = "ALA";
            dict["AZ"] = "AZE";
            dict["BB"] = "BRB";
            dict["BD"] = "BGD";
            dict["BF"] = "BFA";
            dict["BG"] = "BGR";
            dict["BI"] = "BDI";
            dict["BM"] = "BMU";
            dict["BN"] = "BRN";
            dict["BO"] = "BOL";
            dict["BR"] = "BRA";
            dict["BS"] = "BHS";
            dict["BT"] = "BTN";
            dict["BV"] = "BVT";
            dict["BY"] = "BRA";
            dict["BZ"] = "BLZ";
            dict["CG"] = "COG";
            dict["CH"] = "CHE";
            dict["CI"] = "CIV";
            dict["CK"] = "COK";
            dict["CL"] = "CHL";
            dict["CM"] = "CMR";
            dict["CN"] = "CHN";
            dict["CO"] = "COL";
            dict["CR"] = "CRI";
            dict["CU"] = "CUB";
            dict["CX"] = "CXR";
            dict["CY"] = "CYP";
            dict["CZ"] = "CZE";
            dict["DJ"] = "DJI";
            dict["DK"] = "DNK";
            dict["DO"] = "DOM";
            dict["DZ"] = "DZA";
            dict["EC"] = "ECU";
            dict["EE"] = "EST";
            dict["EG"] = "EGY";
            dict["EH"] = "ESH";
            dict["ER"] = "ERI";
            dict["ET"] = "ETH";
            dict["FI"] = "FIN";
            dict["FJ"] = "FJI";
            dict["FK"] = "FLK";
            dict["FM"] = "FSM";
            dict["FR"] = "FRA";
            dict["GA"] = "GAB";
            dict["GB"] = "GBR";
            dict["GD"] = "GRD";
            dict["GE"] = "GEO";
            dict["GF"] = "GUF";
            dict["GH"] = "GHA";
            dict["GI"] = "GIB";
            dict["GL"] = "GRL";
            dict["GM"] = "GMB";
            dict["GN"] = "GIN";
            dict["GQ"] = "GNQ";
            dict["GR"] = "GRC";
            dict["GT"] = "GTM";
            dict["GU"] = "GUM";
            dict["GW"] = "GNB";
            dict["GY"] = "GUY";
            dict["HK"] = "HKG";
            dict["HN"] = "HND";
            dict["HT"] = "HTI";
            dict["ID"] = "IDN";
            dict["IN"] = "IND";
            dict["IQ"] = "IRQ";
            dict["JM"] = "JAM";
            dict["JO"] = "JOR";
            dict["KE"] = "KEN";
            dict["KI"] = "KIR";
            dict["KM"] = "COM";
            dict["KW"] = "KWT";
            dict["KY"] = "CYM";
            dict["KZ"] = "KAZ";
            dict["LA"] = "LAO";
            dict["LB"] = "LBN";
            dict["LR"] = "LBR";
            dict["LS"] = "LSO";
            dict["LU"] = "LUX";
            dict["LV"] = "LVA";
            dict["MA"] = "MAR";
            dict["MC"] = "MCO";
            dict["ME"] = "MNE";
            dict["MG"] = "MDG";
            dict["MH"] = "MHL";
            dict["ML"] = "MLI";
            dict["MN"] = "MNG";
            dict["MO"] = "MAC";
            dict["MP"] = "MNP";
            dict["MQ"] = "MTQ";
            dict["MR"] = "MRT";
            dict["MS"] = "MSR";
            dict["MT"] = "MLT";
            dict["MV"] = "MDV";
            dict["MW"] = "MWI";
            dict["MX"] = "MEX";
            dict["MY"] = "MYS";
            dict["MZ"] = "MOZ";
            dict["NA"] = "NAM";
            dict["NC"] = "NCL";
            dict["NE"] = "NER";
            dict["NG"] = "NGA";
            dict["NI"] = "NIC";
            dict["NL"] = "NLD";
            dict["NP"] = "NPL";
            dict["NR"] = "NRU";
            dict["NZ"] = "NZL";
            dict["OM"] = "OMN";
            dict["PA"] = "PAN";
            dict["PE"] = "PER";
            dict["PF"] = "PYF";
            dict["PG"] = "PNG";
            dict["PH"] = "PHL";
            dict["PK"] = "PAK";
            dict["PL"] = "POL";
            dict["PM"] = "SPM";
            dict["PN"] = "PCN";
            dict["PR"] = "PRI";
            dict["PW"] = "PLW";
            dict["QA"] = "QAT";
            dict["RE"] = "REU";
            dict["RW"] = "RWA";
            dict["SB"] = "SLB";
            dict["SD"] = "SDN";
            dict["SG"] = "SGP";
            dict["SJ"] = "SJM";
            dict["SK"] = "SVK";
            dict["SM"] = "SMR";
            dict["SN"] = "SEN";
            dict["SO"] = "SOM";
            dict["SR"] = "SUR";
            dict["ST"] = "STP";
            dict["SV"] = "SLV";
            dict["TD"] = "TCD";
            dict["TG"] = "TGO";
            dict["TJ"] = "TJK";
            dict["TK"] = "TKL";
            dict["TL"] = "TLS";
            dict["TN"] = "TUN";
            dict["TO"] = "TON";
            dict["TT"] = "TTO";
            dict["TV"] = "TUV";
            dict["UG"] = "UGA";
            dict["UM"] = "UMI";
            dict["UZ"] = "UZB";
            dict["VC"] = "VCT";
            dict["VE"] = "VEN";
            dict["VN"] = "VNM";
            dict["WS"] = "WSM";
            dict["YT"] = "MYT";
            dict["ZA"] = "ZAF";
            dict["ZM"] = "ZMB";
            dict["ZW"] = "ZWE";
            dict["AT"] = "AUT";
            dict["AU"] = "AUS";
            dict["BE"] = "BEL";
            dict["BA"] = "BIH";
            dict["CA"] = "CAN";
            dict["HR"] = "HRV";
            dict["DE"] = "DEU";
            dict["ES"] = "ESP";
            dict["HU"] = "HUN";
            dict["IT"] = "ITA";
            dict["IE"] = "IRL";
            dict["IL"] = "ISR";
            dict["IS"] = "ISL";
            dict["JP"] = "JPN";
            dict["KV"] = "";
            dict["LT"] = "LTU";
            dict["NO"] = "NOR";
            dict["PT"] = "PRT";
            dict["RO"] = "ROU";
            dict["RS"] = "SRB";
            dict["RU"] = "RUS";
            dict["SE"] = "SWE";
            dict["SI"] = "SVN";
            dict["TR"] = "TUR";
            dict["TUR"] = "TUR";
            dict["UA"] = "UKR";
            dict["UKR"] = "UKR";
            dict["BH"] = "BHR";
            dict["BJ"] = "BEN";
            dict["BW"] = "BWA";
            dict["CC"] = "CCK";
            dict["CD"] = "COD";
            dict["CF"] = "CAF";
            dict["CV"] = "CV   ZELENORTSKI OTOKI";
            dict["FO"] = "FRO";
            dict["GP"] = "GLP";
            dict["GS"] = "SGS";
            dict["HM"] = "HMD";
            dict["IO"] = "IO   BRIT.OZEMLJE IND.OCEANA";
            dict["IR"] = "IRN";
            dict["KG"] = "KGZ";
            dict["KH"] = "KHM";
            dict["KN"] = "KN   SAINT KITTS IN NEVIS";
            dict["KP"] = "KP   KOREJA DEM. LJUDSKA REP.";
            dict["KR"] = "KOR";
            dict["LC"] = "LC   SAINT LUCIA";
            dict["LI"] = "LIE";
            dict["LK"] = "LKA";
            dict["LY"] = "LBY";
            dict["MD"] = "MDA";
            dict["MK"] = "MKD";
            dict["MM"] = "MMR";
            dict["MU"] = "MUS";
            dict["NF"] = "NF   NORFOLŠKI OTOK";
            dict["NU"] = "NIU";
            dict["PS"] = "PSE";
            dict["PY"] = "PRY";
            dict["SA"] = "SAU";
            dict["SC"] = "SYC";
            dict["SH"] = "SH   SAINT HELENA";
            dict["SL"] = "SLE";
            dict["SY"] = "SYR";
            dict["SZ"] = "SZ   SVAZI";
            dict["TC"] = "TC   OTOKI TURKS IN CAICOS";
            dict["TF"] = "TF   FRANCOSKO JUŽNO OZEMLJE";
            dict["TH"] = "THA";
            dict["TM"] = "TKM";
            dict["TW"] = "TWN";
            dict["TZ"] = "TZA";
            dict["UY"] = "URY";
            dict["VA"] = "VA   SVETI SEDEŽ (VATIKAN. M. DRŽ.)";
            dict["VG"] = "VGB";
            dict["VI"] = "VIR";
            dict["VU"] = "VU   VANUATU";
            dict["WF"] = "WF   OTOKI WALLIS IN FUTUNA";
            dict["YE"] = "YEM";
            dict["US"] = "USA";
            return dict;
        }

        private static Dictionary<string, string> WooCountryCodeToBiroDropdownOption() {

            /*
            DefaultDictionary<string, Desc> some = new DefaultDictionary<string, Desc>(() => new Desc() { Birokrat = "", Woocommerce = "" });
            foreach (var key in dict.Keys) {
                some[key].Woocommerce = dict[key];
            }
            foreach (string key in dict2.Keys) {
                var tmp = key.Split(" ");
                string k = tmp[0];
                some[k].Birokrat = key;
            }

            string[] somekeys = some.Keys.ToArray();
            Array.Sort(somekeys);

            foreach (string key in somekeys) {
                if (some[key].Birokrat != "" && some[key].Woocommerce != "") { // same code
                    Console.WriteLine($"dict[\"{key}\"] = \"{some[key].Birokrat}\";");
                }
            }
            foreach (string key in somekeys) {
                if (some[key].Birokrat == "" || some[key].Woocommerce == "") { // diff code
                    Console.WriteLine($"dict[\"{key}\"] = \"{some[key].Birokrat}\" + \"-\"+ \"{some[key].Woocommerce}\"");
                }
            }*/


            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["AD"] = "AD   ANDORA";
            dict["AE"] = "AE   ZDRUŽENI ARABSKI EMIRATI";
            dict["AF"] = "AF   AFGANISTAN";
            dict["AG"] = "AG   ANTIGVA IN BARBUDA";
            dict["AI"] = "AI   ANGVILA";
            dict["AL"] = "AL   ALBANIJA";
            dict["AM"] = "AM   ARMENIJA";
            dict["AN"] = "AN   NIZOZEMSKI ANTILI";
            dict["AO"] = "AO   ANGOLA";
            dict["AQ"] = "AQ   ANTARKTIKA";
            dict["AR"] = "AR   ARGENTINA";
            dict["AS"] = "AS   AMERIŠKA SAMOA";
            dict["AW"] = "AW   ARUBA";
            dict["AX"] = "AX   ALAND OTOKI";
            dict["AZ"] = "AZ   AZERBAJDŽAN";
            dict["BB"] = "BB   BARBADOS";
            dict["BD"] = "BD   BANGLADEŠ";
            dict["BF"] = "BF   BURKINA FASO";
            dict["BG"] = "BG   BOLGARIJA";
            dict["BH"] = "BH   BAHRAJN";
            dict["BI"] = "BI   BURUNDI";
            dict["BJ"] = "BJ   BENIN";
            dict["BM"] = "BM   BERMUDI";
            dict["BN"] = "BN   BRUNEJ";
            dict["BO"] = "BO   BOLIVIJA";
            dict["BR"] = "BR   BRAZILIJA";
            dict["BS"] = "BS   BAHAMI";
            dict["BT"] = "BT   BUTAN";
            dict["BV"] = "BV   BOUVETOV OTOK";
            dict["BW"] = "BW   BOCVANA";
            dict["BY"] = "BY   BELORUSIJA";
            dict["BZ"] = "BZ   BELIZE";
            dict["CC"] = "CC   KOKOSOVI (KEELING) OTOKI";
            dict["CD"] = "CD   KONGO DEMOKRATIČNA REPUBLIKA";
            dict["CF"] = "CF   SREDNJEAFRIŠKA REPUBLIKA";
            dict["CG"] = "CG   KONGO";
            dict["CH"] = "CH   ŠVICA";
            dict["CI"] = "CI   SLONOKOŠČENA OBALA";
            dict["CK"] = "CK   COOKOVI OTOKI";
            dict["CL"] = "CL   ČILE";
            dict["CM"] = "CM   KAMERUN";
            dict["CN"] = "CN   KITAJSKA";
            dict["CO"] = "CO   KOLUMBIJA";
            dict["CR"] = "CR   KOSTARIKA";
            dict["CU"] = "CU   KUBA";
            dict["CV"] = "CV   ZELENORTSKI OTOKI";
            dict["CX"] = "CX   BOŽIČNI OTOK";
            dict["CY"] = "CY   CIPER";
            dict["CZ"] = "CZ   ČEŠKA REPUBLIKA";
            dict["DJ"] = "DJ   DŽIBUTI";
            dict["DK"] = "DK   DANSKA";
            dict["DO"] = "DO   DOMINIKANSKA REPUBLIKA";
            dict["DZ"] = "DZ   ALŽIRIJA";
            dict["EC"] = "EC   EKVADOR";
            dict["EE"] = "EE   ESTONIJA";
            dict["EG"] = "EG   EGIPT";
            dict["EH"] = "EH   ZAHODNA SAHARA";
            dict["ER"] = "ER   ERITREJA";
            dict["ET"] = "ET   ETIOPIJA";
            dict["FI"] = "FI   FINSKA";
            dict["FJ"] = "FJ   FIDŽI";
            dict["FK"] = "FK   FALKLANDSKI OTOKI (MALVINI)";
            dict["FM"] = "FM   MIKRONEZIJA (FEDERAT. DRŽAVE)";
            dict["FO"] = "FO   FERSKI OTOKI";
            dict["FR"] = "FR   FRANCIJA";
            dict["GA"] = "GA   GABON";
            dict["GB"] = "GB   VELIKA BRITANIJA";
            dict["GD"] = "GD   GRENADA";
            dict["GE"] = "GE   GRUZIJA";
            dict["GF"] = "GF   FRANCOSKA GVAJANA";
            dict["GH"] = "GH   GANA";
            dict["GI"] = "GI   GIBRALTAR";
            dict["GL"] = "GL   GRENLANDIJA";
            dict["GM"] = "GM   GAMBIJA";
            dict["GN"] = "GN   GVINEJA";
            dict["GP"] = "GP   GUADELOUPE";
            dict["GQ"] = "GQ   EKVATORIALNA GVINEJA";
            dict["GR"] = "GR   GRČIJA";
            dict["GS"] = "GS   J.GEORGIJA IN OTOKI J.SANDWICH";
            dict["GT"] = "GT   GVATEMALA";
            dict["GU"] = "GU   GUAM";
            dict["GW"] = "GW   GVINEJA BISSAU";
            dict["GY"] = "GY   GVAJANA";
            dict["HK"] = "HK   HONGKONG";
            dict["HM"] = "HM   HEARDOV OTOK IN MCDONALD.OTOKI";
            dict["HN"] = "HN   HONDURAS";
            dict["HT"] = "HT   HAITI";
            dict["ID"] = "ID   INDONEZIJA";
            dict["IN"] = "IN   INDIJA";
            dict["IO"] = "IO   BRIT.OZEMLJE IND.OCEANA";
            dict["IQ"] = "IQ   IRAK";
            dict["IR"] = "IR   IRAN (ISLAMSKA REPUBLIKA)";
            dict["JM"] = "JM   JAMAJKA";
            dict["JO"] = "JO   JORDANIJA";
            dict["KE"] = "KE   KENIJA";
            dict["KG"] = "KG   KIRGIZISTAN";
            dict["KH"] = "KH   KAMBODŽA";
            dict["KI"] = "KI   KIRIBATI";
            dict["KM"] = "KM   KOMORI";
            dict["KN"] = "KN   SAINT KITTS IN NEVIS";
            dict["KP"] = "KP   KOREJA DEM. LJUDSKA REP.";
            dict["KR"] = "KR   KOREJA REPUBLIKA";
            dict["KW"] = "KW   KUVAJT";
            dict["KY"] = "KY   KAJMANSKI OTOKI";
            dict["KZ"] = "KZ   KAZAHSTAN";
            dict["LA"] = "LA   LAOS";
            dict["LB"] = "LB   LIBANON";
            dict["LC"] = "LC   SAINT LUCIA";
            dict["LI"] = "LI   LIHTENŠTAJN";
            dict["LK"] = "LK   ŠRILANKA";
            dict["LR"] = "LR   LIBERIJA";
            dict["LS"] = "LS   LESOTO";
            dict["LU"] = "LU   LUKSEMBURG";
            dict["LV"] = "LV   LATVIJA";
            dict["LY"] = "LY   LIBIJSKA ARAB. DŽAMAHIRIJA";
            dict["MA"] = "MA   MAROKO";
            dict["MC"] = "MC   MONAKO";
            dict["MD"] = "MD   MOLDAVIJA REPUBLIKA";
            dict["ME"] = "ME   ČRNA GORA";
            dict["MG"] = "MG   MADAGASKAR";
            dict["MH"] = "MH   MARSHALLOVI OTOKI";
            dict["MK"] = "MK   MAKEDONIJA NEKD.JUG.REPUBL.";
            dict["ML"] = "ML   MALI";
            dict["MM"] = "MM   MJANMAR";
            dict["MN"] = "MN   MONGOLIJA";
            dict["MO"] = "MO   MACAO";
            dict["MP"] = "MP   SEVERNI MARIANSKI OTOKI";
            dict["MQ"] = "MQ   MARTINIK";
            dict["MR"] = "MR   MAVRETANIJA";
            dict["MS"] = "MS   MONTSERRAT";
            dict["MT"] = "MT   MALTA";
            dict["MU"] = "MU   MAURITIUS";
            dict["MV"] = "MV   MALDIVI";
            dict["MW"] = "MW   MALAVI";
            dict["MX"] = "MX   MEHIKA";
            dict["MY"] = "MY   MALEZIJA";
            dict["MZ"] = "MZ   MOZAMBIK";
            dict["NA"] = "NA   NAMIBIJA";
            dict["NC"] = "NC   NOVA KALEDONIJA";
            dict["NE"] = "NE   NIGER";
            dict["NF"] = "NF   NORFOLŠKI OTOK";
            dict["NG"] = "NG   NIGERIJA";
            dict["NI"] = "NI   NIKARAGVA";
            dict["NL"] = "NL   NIZOZEMSKA";
            dict["NP"] = "NP   NEPAL";
            dict["NR"] = "NR   NAURU";
            dict["NU"] = "NU   NIUE";
            dict["NZ"] = "NZ   NOVA ZELANDIJA";
            dict["OM"] = "OM   OMAN";
            dict["PA"] = "PA   PANAMA";
            dict["PE"] = "PE   PERU";
            dict["PF"] = "PF   FRANCOSKA POLINEZIJA";
            dict["PG"] = "PG   PAPUA NOVA GVINEJA";
            dict["PH"] = "PH   FILIPINI";
            dict["PK"] = "PK   PAKISTAN";
            dict["PL"] = "PL   POLJSKA";
            dict["PM"] = "PM   SAINT PIERRE IN MIQUELON";
            dict["PN"] = "PN   PITCAIRN";
            dict["PR"] = "PR   PORTORIKO";
            dict["PS"] = "PS   PALESTINSKO OZEMLJE ZASEDENO";
            dict["PW"] = "PW   PALAU";
            dict["PY"] = "PY   PARAGVAJ";
            dict["QA"] = "QA   KATAR";
            dict["RE"] = "RE   REUNION";
            dict["RW"] = "RW   RUANDA";
            dict["SA"] = "SA   SAUDOVA ARABIJA";
            dict["SB"] = "SB   SALOMONOVI OTOKI";
            dict["SC"] = "SC   SEJŠELI";
            dict["SD"] = "SD   SUDAN";
            dict["SG"] = "SG   SINGAPUR";
            dict["SH"] = "SH   SAINT HELENA";
            dict["SJ"] = "SJ   SVALBARD IN JAN MAYEN";
            dict["SK"] = "SK   SLOVAŠKA";
            dict["SL"] = "SL   SIERRA LEONE";
            dict["SM"] = "SM   SAN MARINO";
            dict["SN"] = "SN   SENEGAL";
            dict["SO"] = "SO   SOMALIJA";
            dict["SR"] = "SR   SURINAM";
            dict["ST"] = "ST   SAO TOME IN PRINCIPE";
            dict["SV"] = "SV   SALVADOR";
            dict["SY"] = "SY   SIRSKA ARABSKA REPUBLIKA";
            dict["SZ"] = "SZ   SVAZI";
            dict["TC"] = "TC   OTOKI TURKS IN CAICOS";
            dict["TD"] = "TD   ČAD";
            dict["TF"] = "TF   FRANCOSKO JUŽNO OZEMLJE";
            dict["TG"] = "TG   TOGO";
            dict["TH"] = "TH   TAJSKA";
            dict["TJ"] = "TJ   TADŽIKISTAN";
            dict["TK"] = "TK   TOKELAU";
            dict["TL"] = "TL   TIMOR-LESTE";
            dict["TM"] = "TM   TURKMENISTAN";
            dict["TN"] = "TN   TUNIZIJA";
            dict["TO"] = "TO   TONGA";
            dict["TT"] = "TT   TRINIDAD IN TOBAGO";
            dict["TV"] = "TV   TUVALU";
            dict["TW"] = "TW   TAJVAN PROVINCA KITAJSKE";
            dict["TZ"] = "TZ   TANZANIJA ZDRUŽENA REP.";
            dict["UG"] = "UG   UGANDA";
            dict["UM"] = "UM   STRANSKI ZUNANJI OTOKI ZDR.DRŽ.";
            dict["UY"] = "UY   URUGVAJ";
            dict["UZ"] = "UZ   UZBEKISTAN";
            dict["VA"] = "VA   SVETI SEDEŽ (VATIKAN. M. DRŽ.)";
            dict["VC"] = "VC   SAINT VINCENT IN GRENADINE";
            dict["VE"] = "VE   VENEZUELA";
            dict["VG"] = "VG   DEVIŠKI OTOKI (BRITANSKI)";
            dict["VI"] = "VI   DEVIŠKI OTOKI (ZDA)";
            dict["VN"] = "VN   VIETNAM";
            dict["VU"] = "VU   VANUATU";
            dict["WF"] = "WF   OTOKI WALLIS IN FUTUNA";
            dict["WS"] = "WS   SAMOA";
            dict["YE"] = "YE   JEMEN";
            dict["YT"] = "YT   MAYOTTE";
            dict["ZA"] = "ZA   JUŽNA AFRIKA";
            dict["ZM"] = "ZM   ZAMBIJA";
            dict["ZW"] = "ZW   ZIMBABVE";
            dict["AT"] = "AUT  AVSTRIJA";
            dict["AU"] = "AUS  AVSTRALIJA";
            dict["BE"] = "B    BELGIJA";
            dict["BA"] = "BIH  BOSNA IN HERCEGOVINA";
            dict["CA"] = "CAN  KANADA";
            dict["HR"] = "CRO  HRVAŠKA";
            dict["DE"] = "D    NEMČIJA";
            dict["ES"] = "ESP  ŠPANIJA";
            dict["HU"] = "HUN  MADŽARSKA";
            dict["IT"] = "I    ITALIJA";
            dict["IE"] = "IRL  IRSKA";
            dict["IL"] = "IZ   IZRAEL";
            dict["IS"] = "ISL  ISLANDIJA";
            dict["JP"] = "JAP  JAPONSKA";
            dict["KV"] = "KV   KOSOVO";
            dict["LT"] = "LTU  LITVA";
            dict["NO"] = "N    NORVEŠKA";
            dict["PT"] = "POR  PORTUGALSKA";
            dict["RO"] = "ROM  ROMUNIJA";
            dict["RS"] = "SRB  SRBIJA";
            dict["RU"] = "RUS  RUSKA FEDERACIJA";
            dict["SE"] = "SVE  ŠVEDSKA";
            dict["SI"] = "SLO  Slovenija";
            dict["TR"] = "TUR  TURČIJA";
            dict["TUR"] = "TUR  TURČIJA";
            dict["UA"] = "UKR  UKRAJINA";
            dict["UKR"] = "UKR  UKRAJINA";
            dict["US"] = "USA  ZDRUŽENE DRŽAVE";
            return dict;
        }
    }
}
