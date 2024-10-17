using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;

namespace de.devcodemonkey.AIChecker.MarkdownExporter
{
    public class MdFontStyles : IMdFontStyles
    {
        private IMdFile File { get; }

        public MdFontStyles(IMdFile file)
        {
            File = file ?? throw new ArgumentNullException(nameof(file));
        }

        public static string Bold(string text) => $"**{text}**";

        public static string Italic(string text) => $"_{text}_";

        public static string BoldItalic(string text) => $"***{text}***";

        public void AddBoldText(string text) => File.Text.AppendLine(Bold(text));

        public void AddItalicText(string text) => File.Text.AppendLine(Italic(text));

        public void AddBoldItalicText(string text) => File.Text.AppendLine(BoldItalic(text));

        public static string H1(string text) => $"# {text}\n";

        public static string H2(string text) => $"## {text}\n";

        public static string H3(string text) => $"### {text} \n";

        public static string H4(string text) => $"#### {text} \n";

        public static string H5(string text) => $"##### {text} \n";

        public static string H6(string text) => $"###### {text} \n";

        public void AddH1Text(string text) => File.Text.AppendLine(H1(text));

        public void AddH2Text(string text) => File.Text.AppendLine(H2(text));

        public void AddH3Text(string text) => File.Text.AppendLine(H3(text));

        public void AddH4Text(string text) => File.Text.AppendLine(H4(text));

        public void AddH5Text(string text) => File.Text.AppendLine(H5(text));

        public void AddH6Text(string text) => File.Text.AppendLine(H6(text));

        public void AddText(string text) => File.Text.AppendLine(text);
    }
}
