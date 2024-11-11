using de.devcodemonkey.AIChecker.CoreBusiness.Models;

namespace de.devcodemonkey.AIChecker.UseCases.Interfaces
{
    public interface ISendAndSaveApiRequestUseCase
    {   
        Task ExecuteAsync(string userMessage, string systemPrompt, string resultSetValue, int requestCount = 1, int maxTokens = -1, double temperature = 0.7, bool saveProcessUsage = true, int saveInterval = 5, bool writeOutput = true, string? environmentTokenName = null, string source = "http://localhost:1234/v1/chat/completions", string model = "nothing set");
        Task ExecuteAsync(SendToLmsParams sendToLmsParams);
    }
}