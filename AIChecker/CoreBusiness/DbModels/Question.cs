namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

public partial class Question
{
    public Guid QuestionId { get; set; }

    public Guid? AnswerId { get; set; }

    public Guid? CategoryId { get; set; }

    public string Value { get; set; } = null!;

    public virtual Answer Answer { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
