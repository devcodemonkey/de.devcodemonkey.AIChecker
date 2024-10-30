using de.devcodemonkey.AIChecker.CoreBusiness.DbModelInterfaces;

namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

public partial class ResultSet : IValue
{
    public Guid ResultSetId { get; set; }

    public string Value { get; set; } = null!;

    public string? Description { get; set; } 

    public string? PromptRequierements { get; set; }

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    public virtual ICollection<SystemResourceUsage> SystemResourceUsages { get; set; } = new List<SystemResourceUsage>();
}
