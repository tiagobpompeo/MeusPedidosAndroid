using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MeusPedidos.Models
{
    public class PriceOff
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "category_id")]
        public int Category_id { get; set; }
        [JsonProperty(PropertyName = "policies")]
        public List<Policy> Policies { get; set; }

        public class Policy
        {
            [JsonProperty(PropertyName = "min")]
            public int Min { get; set; }
            [JsonProperty(PropertyName = "discount")]
            public double Discount { get; set; }
        }
    }
}
