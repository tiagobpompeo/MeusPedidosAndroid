using System;
using Newtonsoft.Json;

namespace MeusPedidos.Models
{
    public class Categories
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
