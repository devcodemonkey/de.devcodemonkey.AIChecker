
namespace de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

public interface IDeserializer<T>
{
    List<T> DeserializedClass { get; }

    Task<List<T>> DeserialzeFileAsync(string filePath);
}
