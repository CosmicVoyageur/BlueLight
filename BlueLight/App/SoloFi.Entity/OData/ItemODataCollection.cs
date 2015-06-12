using System.Collections.Generic;
using Newtonsoft.Json;

namespace SoloFi.Entity.OData
{
    public class ItemODataCollection
    {
        [JsonProperty(PropertyName = "@odata.context")]
        public string Context { get; set; }
        [JsonProperty(PropertyName = "value")]
        public List<Item> Items { get; set; }
    }

    public class ODataCollection<TEntity>
    {
        [JsonProperty(PropertyName = "@odata.context")]
        public string Context { get; set; }

        [JsonProperty(PropertyName = "value")]
        public List<TEntity> Value { get; set; }
    }
}
