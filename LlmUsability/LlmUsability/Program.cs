using AutoGen.Core;
using AutoGen.LMStudio;
using Humanizer;
using AutoGen.Ollama.Extension;
using AutoGen.Ollama;
using System.Diagnostics;

namespace LlmUsability
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //await new RunLMStudio().RunAsync();
            await new RunOllama().RunAsync();
            stopwatch.Stop();            
            Console.WriteLine("Verstrichene Zeit in Sekunden: " + stopwatch.Elapsed.TotalSeconds);            
        }

    }
}
