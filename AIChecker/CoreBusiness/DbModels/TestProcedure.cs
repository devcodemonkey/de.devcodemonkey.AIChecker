using de.devcodemonkey.AIChecker.CoreBusiness.DbModelInterfaces;

namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

public class TestProcedure
{
    public Guid QuestionId { get; set; }

    public Guid AnswerId { get; set; }

    public Guid TestProcedureCategoryId { get; set; }

    public Question Question { get; set; } = null!;

    public Answer Answer { get; set; } = null!;

    public TestProcedureCategory TestProcedureCategory { get; set; } = null!;
}

