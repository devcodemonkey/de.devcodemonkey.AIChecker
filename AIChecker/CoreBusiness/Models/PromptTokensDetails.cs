﻿using System.Text.Json.Serialization;

namespace de.devcodemonkey.AIChecker.CoreBusiness.Models
{
    public class PromptTokensDetails
    {
        [JsonPropertyName("cached_tokens")]
        public int CachedTokens { get; set; }

        [JsonPropertyName("audio_tokens")]
        public int AudioTokens { get; set; }
    }
}
