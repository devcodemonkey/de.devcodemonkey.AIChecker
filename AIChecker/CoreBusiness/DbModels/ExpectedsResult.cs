namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

public partial class ExpectedsResult
{
    public Guid ExpectedId { get; set; }

    public Guid ResultsId { get; set; }

    public virtual Expected Expected { get; set; } = null!;

    public virtual Result ExpectedNavigation { get; set; } = null!;
}
