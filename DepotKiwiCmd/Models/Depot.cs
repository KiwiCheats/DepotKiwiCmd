using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DepotKiwi.Models {
    public class Depot {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("files")]
        public List<FileInfo> Files { get; set; } = new();
    }
}