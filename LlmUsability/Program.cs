using AutoGen.Core;
using AutoGen.LMStudio;
using Humanizer;

namespace LlmUsability
{
    internal class Program
    {        
        static async Task Main(string[] args)
        {
            Tests tests = new Tests();            
            await tests.ShowSameTempuratureAsync();
        }
    }
}
