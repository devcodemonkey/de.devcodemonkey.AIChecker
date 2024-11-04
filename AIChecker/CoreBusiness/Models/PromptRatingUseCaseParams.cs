namespace de.devcodemonkey.AIChecker.CoreBusiness.Models
{
    public class PromptRatingUseCaseParams
    {
        public string[] ModelNames { get; set; }
        public int MaxTokens { get; set; }
        public string ResultSet { get; set; }
        public string? Description { get; set; }
        public string PromptRequirements { get; set; }
        public string? ResponseFormat { get; set; }
        public Func<string> SystemPrompt { get; set; }
        public Func<string> Message { get; set; }
        public Func<string> RatingReason { get; set; }
        public Func<int> Rating { get; set; }
        public Func<bool> NewImprovement { get; set; }

        
    }
}
