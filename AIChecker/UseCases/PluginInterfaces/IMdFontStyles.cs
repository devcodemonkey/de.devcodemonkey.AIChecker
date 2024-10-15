namespace de.devcodemonkey.AIChecker.UseCases.PluginInterfaces
{
    public interface IMdFontStyles
    {
        void AddBoldItalicText(string text);
        void AddBoldText(string text);
        void AddH1Text(string text);
        void AddH2Text(string text);
        void AddH3Text(string text);
        void AddH4Text(string text);
        void AddH5Text(string text);
        void AddH6Text(string text);
        void AddItalicText(string text);
        void AddText(string text);
    }
}