using de.devcodemonkey.AIChecker.CoreBusiness.DbModelInterfaces;

namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

public partial class Model : IValue
{
    public Guid ModelId { get; set; }

    public DateTime? LastModelUpdate { get; set; }

    public DateTime? Timestamp { get; set; }

    public string? Value { get; set; }

    public string? Description { get; set; }

    public string? BaseModels { get; set; }

    public string? Link { get; set; }

    public double? Size { get; set; }

    public string? Quantification { get; set; }

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
