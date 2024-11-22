﻿using System.Text.Json.Serialization;

namespace de.devcodemonkey.AIChecker.CoreBusiness.Models
{
    public class Usage
    {
        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens { get; set; }
        [JsonPropertyName("completion_tokens")]
        public int CompletionTokens { get; set; }
        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
        [JsonPropertyName("prompt_tokens_details")]
        public PromptTokensDetails? PromptTokensDetails { get; set; }
        [JsonPropertyName("completion_tokens_details")]
        public CompletionTokensDetails? CompletionTokensDetails { get; set; }
    }
}
