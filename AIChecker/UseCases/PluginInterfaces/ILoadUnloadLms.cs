namespace de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

public interface ILoadUnloadLms
{
    bool Load(string modelName);
    bool Unload();
}
