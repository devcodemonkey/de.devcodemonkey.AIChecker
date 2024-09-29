namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface ICreateMoreQuestionsUseCase
    {
        Task ExecuteAsync(string resultSet, string systemPrompt, int maxTokens = -1, double temperature = 0.7, string model = "nothing set", string source = "http://localhost:1234/v1/chat/completions", string? environmentTokenName = null);
    }
}