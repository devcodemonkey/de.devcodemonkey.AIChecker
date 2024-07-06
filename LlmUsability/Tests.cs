using AutoGen.Core;
using AutoGen.LMStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LlmUsability
{
    public class Tests
    {
        private LMStudioAgentManager lmAgentManager;

        public async Task ShowSameTempuratureAsync()
        {
            lmAgentManager = new LMStudioAgentManager();
            var lmAgent = lmAgentManager.GetEvolutionLMStudioAgent();

            List<IMessage> messages = new List<IMessage>();
            for (int i = 0; i < 20; i++)
                messages.Add(await lmAgent.SendAsync("Frage:\n[Was ist 1+1]\n\nAntwort:\n[1+1=2]"));


            foreach (var message in messages)
            {
                Console.WriteLine(message);
            }
        }
    }
}
