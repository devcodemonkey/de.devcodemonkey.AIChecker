namespace de.devcodemonkey.AIChecker.CoreBusiness.Models
{
    public class MoreQuestionsUseCaseParams
    {
        public string Model { get; set; }
        public int MaxTokens { get; set; }
        public string ResultSet { get; set; }
        public string Category { get; set; }

        public string PromptRequirements { get; set; }
        public string? ResponseFormat { get; set; }
        public string SystemPrompt { get; set; }
        public string Message { get; set; }        
    }
}
