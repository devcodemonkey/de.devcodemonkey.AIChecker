namespace de.devcodemonkey.AIChecker.CoreBusiness.Models
{
    public class SendToLmsParams
    {
        public virtual string UserMessage { get; set; }
        public virtual string SystemPrompt { get; set; }
        public virtual string ResultSet { get; set; }
        public virtual int RequestCount { get; set; } = 1;
        public virtual int MaxTokens { get; set; } = -1;
        public virtual double Temperature { get; set; } = 0.7;
        public virtual bool SaveProcessUsage { get; set; } = true;
        public virtual int SaveInterval { get; set; } = 5;
        public virtual bool WriteOutput { get; set; } = true;
        public virtual string? EnvironmentTokenName { get; set; }
        public virtual string Source { get; set; } = "http://localhost:1234/v1/chat/completions";
        public virtual string Model { get; set; } = "nothing set";
        public virtual string? ResponseFormat { get; set; }

        public virtual string? QuestionCategory { get; set; }
        public virtual bool QuestionsCorrect { get; set; }

        public Guid? AnswerId { get; set; }
        public Guid? QuestionId { get; set; }
    }
}
