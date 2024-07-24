using AutoGen.Core;
using AutoGen.LMStudio;
using Humanizer;
using AutoGen.Ollama.Extension;
using AutoGen.Ollama;

namespace LlmUsability
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //await new RunLMStudio().RunAsync();
            await new RunOllama().RunAsync();
        }

    }
}
