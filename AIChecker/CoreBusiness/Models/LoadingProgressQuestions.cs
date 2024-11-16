namespace de.devcodemonkey.AIChecker.CoreBusiness.Models
{
    public class LoadingProgressQuestions
    {
        public int QuestionsCounter { get; set; }
        public int QuestionsCount { get; set; }
        public int AnswersCounter { get; set; }
        public int AnswersCount { get; set; }
        public int TotalCounter { get; set; }
        
        public TimeSpan RunningTime { get; set; }
        public TimeSpan CalulationTime { get; set; }
    }
}
