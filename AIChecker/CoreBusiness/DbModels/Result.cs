namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

public partial class Result
{
    public Guid ResultId { get; set; }

    public Guid? AnswerId { get; set; }

    public Guid? QuestionsId { get; set; }

    public Guid ResultSetId { get; set; }

    public Guid? RequestObjectId { get; set; }

    public Guid? RequestReasonId { get; set; }

    public string? RequestId { get; set; } = null!;

    public string? Asked { get; set; } = null!;

    public string? Message { get; set; } = null!;

    public string? ResponseFormat { get; set; }

    public Guid ModelId { get; set; }

    public Guid? SystemPromptId { get; set; }

    public double? Temperature { get; set; }

    public int? MaxTokens { get; set; }

    public int? PromptTokens { get; set; }

    public int? CompletionTokens { get; set; }

    public int? TotalTokens { get; set; }

    public DateTime? RequestCreated { get; set; }

    public DateTime? RequestStart { get; set; }

    public DateTime? RequestEnd { get; set; }        

    public virtual Model Model { get; set; } = null!;

    public virtual Answer? Answer { get; set; }

    public virtual Question? Questions { get; set; }

    public virtual RequestObject RequestObject { get; set; } = null!;

    public virtual RequestReason RequestReason { get; set; } = null!;

    public virtual ResultSet ResultSet { get; set; } = null!;

    public virtual SystemPrompt SystemPrompt { get; set; } = null!;

    public virtual PromptRatingRound? PromptRatingRound { get; set; } = null!;
}
