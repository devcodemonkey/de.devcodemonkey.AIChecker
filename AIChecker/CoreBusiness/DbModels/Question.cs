namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

public partial class Question
{
    public Guid QuestionId { get; set; }

    public Guid? AnswerId { get; set; }

    public Guid? QuestionCategoryId { get; set; }

    public string Value { get; set; } = null!;

    public bool? IsCorrect { get; set; } = null!;

    public virtual Answer Answer { get; set; } = null!;

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    public virtual QuestionCategory Category { get; set; } = null!;

    public virtual ICollection<TestProcedure> TestProcedures { get; set; } = new List<TestProcedure>();
}
