namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

public class TestProcedureCategory
{
    public Guid TestProcedureCategoryId { get; set; }

    public string Value { get; set; } = null!;

    public ICollection<TestProcedure> TestProcedures { get; set; } = new List<TestProcedure>();
}
