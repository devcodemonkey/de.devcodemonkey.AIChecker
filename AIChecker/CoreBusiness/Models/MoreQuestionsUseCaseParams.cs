namespace de.devcodemonkey.AIChecker.CoreBusiness.Models
{    
    public class MoreQuestionsUseCaseParams
    {
        public virtual string ResultSet { get; set; }
        public virtual string Model { get; set; }
        public virtual int? MaxTokens { get; set; }
        public virtual int Temperature { get; set; }        
        public virtual string Category { get; set; }
        public virtual string? ResponseFormat { get; set; }
        public virtual string SystemPrompt { get; set; }               
    }
}
