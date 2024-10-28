namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

public partial class Answer
{
    public Guid AnswerId { get; set; }    

    public string Value { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<Img> Imgs { get; set; } = new List<Img>();

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    public virtual ICollection<TestProcedure> TestProcedures { get; set; } = new List<TestProcedure>();
}
