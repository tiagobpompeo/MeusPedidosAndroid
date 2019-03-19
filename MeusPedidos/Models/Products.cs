using System;
using Newtonsoft.Json;

namespace MeusPedidos.Models
{
    public class Products
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "photo")]
        public string Photo { get; set; }
        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }
        [JsonProperty(PropertyName = "category_id")]
        public int? Category_id { get; set; }
    }
}
