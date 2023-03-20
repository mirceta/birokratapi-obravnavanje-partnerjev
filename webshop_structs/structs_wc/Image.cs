﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using Newtonsoft.Json;

namespace BironextWordpressIntegrationHub.structs
{
    public class Image
    {

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("alt")]
        public string Alt { get; set; }

        [JsonProperty("src")]
        public string Src { get; set; }

        [JsonProperty("srcset")]
        public dynamic Srcset { get; set; }

        [JsonProperty("sizes")]
        public dynamic Sizes { get; set; }
    }

}