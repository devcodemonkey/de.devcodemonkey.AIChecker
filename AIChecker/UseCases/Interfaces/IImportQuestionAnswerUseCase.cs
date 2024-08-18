namespace de.devcodemonkey.AIChecker.UseCases.Interfaces;

public interface IImportQuestionAnswerUseCase
{
    Task ExecuteAsnc(string filePath);
}