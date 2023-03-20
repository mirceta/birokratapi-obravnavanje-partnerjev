﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System.Collections.Generic;
using Newtonsoft.Json;

namespace BironextWordpressIntegrationHub.structs
{
    public class WoocommerceOrderItem
    {
        public string BirokratSifra { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("order_id")]
        public int OrderId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("variation_id")]
        public int VariationId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("tax_class")]
        public string TaxClass { get; set; }

        [JsonProperty("subtotal")]
        public string Subtotal { get; set; }

        [JsonProperty("subtotal_tax")]
        public string SubtotalTax { get; set; }

        [JsonProperty("total")]
        public string Total { get; set; }

        [JsonProperty("total_tax")]
        public string TotalTax { get; set; }

        [JsonProperty("taxes")]
        public Taxes Taxes { get; set; }

        [JsonProperty("meta_data")]
        public IList<object> MetaData { get; set; }

        [JsonProperty("origin_product")]
        public dynamic OriginProduct { get; set; }
        public Dictionary<string, object> AdditionalData { get; set; }
        [JsonProperty("sku")]
        public string Sku { get; set; }
    }
}