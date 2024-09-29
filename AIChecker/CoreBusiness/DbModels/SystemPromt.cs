using de.devcodemonkey.AIChecker.CoreBusiness.DbModelInterfaces;

namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

public partial class SystemPrompt : IValue
{
    public Guid SystemPromptId { get; set; }

    public string Value { get; set; } = null!;

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
