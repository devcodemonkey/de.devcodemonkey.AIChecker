using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.MarkdownExporter
{
    public static class Configuration
    {
        public static string TempPath { get; } = Path.Combine(Path.GetTempPath(), "MarkdownExporter");

        public static readonly Color[] ChartColors = new Color[]
         {
            Colors.Red,
            Colors.Blue,
            Colors.Green,
            Colors.Orange,
            Colors.Purple,
            Colors.Cyan,
            Colors.Magenta,
            Colors.Yellow,
            Colors.Black,
            Colors.Gray,
            Colors.DarkBlue,
            Colors.DarkGreen,
            Colors.DarkRed,
            Colors.DarkCyan,
            Colors.DarkMagenta,
            Colors.DarkOrange,
            Colors.Brown,
            Colors.Pink,
            Colors.Violet,
            Colors.Indigo,
            Colors.Turquoise,
            Colors.LimeGreen,
            Colors.Chartreuse,
            Colors.SkyBlue,
            Colors.Crimson,
            Colors.Gold,
            Colors.Navy,
            Colors.Teal,
            Colors.Olive,
            Colors.Salmon
         };
    }
}
