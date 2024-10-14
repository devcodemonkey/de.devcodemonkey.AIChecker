namespace de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
{
    public interface ILoadUnloadLms
    {
        bool LoadLms(string modelName);
        bool UnloadLms();
    }
}