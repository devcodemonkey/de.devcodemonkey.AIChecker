using de.devcodemonkey.AIChecker.CoreBusiness.DbModelInterfaces;

namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

public partial class Model : IValue
{
    public Guid ModelId { get; set; }

    public string? Value { get; set; }

    public string? BasicModells { get; set; }

    public string? Link { get; set; }

    public double? Size { get; set; }

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
