namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

public partial class Answer
{
    public Guid AnswerId { get; set; }

    public Guid? QuestionId { get; set; }

    public string Value { get; set; } = null!;

    public virtual ICollection<Img> Imgs { get; set; } = new List<Img>();

    public virtual Question? Question { get; set; }
}
