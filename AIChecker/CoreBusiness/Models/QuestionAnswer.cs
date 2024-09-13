namespace de.devcodemonkey.AIChecker.CoreBusiness.Models
{
    public class QuestionAnswer
    {
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public ICollection<string>? Images { get; set; }
    }
}
