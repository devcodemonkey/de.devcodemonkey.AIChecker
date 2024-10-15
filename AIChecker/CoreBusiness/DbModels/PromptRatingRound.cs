namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels
{
    public class PromptRatingRound
    {
        public Guid PromptRatingRoundId { get; set; }        

        public int Rating { get; set; }

        public int Round { get; set; }

        public Guid ResultId { get; set; }

        public virtual Result Result { get; set; }
    }
}
