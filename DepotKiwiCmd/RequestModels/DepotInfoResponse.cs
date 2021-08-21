﻿using System.Text.Json.Serialization;

using DepotKiwi.Models;

namespace DepotKiwi.RequestModels {
    public class DepotInfoResponse {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        
        [JsonPropertyName("depot")]
        public Depot Depot { get; set; }
    }
}