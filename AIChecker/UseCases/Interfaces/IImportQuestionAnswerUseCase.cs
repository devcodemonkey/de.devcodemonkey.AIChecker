namespace de.devcodemonkey.AIChecker.UseCases.Interfaces;

public interface IImportQuestionAnswerUseCase
{    
    Task ExecuteAsync(string filePath, string Category);
}