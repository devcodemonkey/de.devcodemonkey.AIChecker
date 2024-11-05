namespace de.devcodemonkey.AIChecker.CoreBusiness.Models
{    
    public class MoreQuestionsUseCaseParams
    {
        public string ResultSet { get; set; }
        public string Model { get; set; }
        public int? MaxTokens { get; set; }
        public int Temperature { get; set; }        
        public string Category { get; set; }
        public string? ResponseFormat { get; set; }
        public string SystemPrompt { get; set; }               
    }
}
