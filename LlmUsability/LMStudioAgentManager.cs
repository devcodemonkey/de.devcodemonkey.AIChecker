using AutoGen.Core;
using AutoGen.LMStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LlmUsability
{
    public class LMStudioAgentManager
    {
        private LMStudioConfig config = new LMStudioConfig("localhost", 1234);

        public LMStudioAgentManager()
        {
            config = new LMStudioConfig("localhost", 1234);
        }

        public LMStudioAgent GetDefaultLMStudioAgent()
        {
            return new LMStudioAgent(
                "asssistant",
                config: config,
                temperature: 0);
        }

        public LMStudioAgent GetEvolutionLMStudioAgent()
        {
            return new LMStudioAgent(
                "asssistant",
                config: config,
                temperature: 0,
                systemMessage: "Gib nur eine JSON-Datei im Format: {\"Bewertung\": [Wert]} zurück. Keine Erklärungen oder zusätzlichen Informationen. Beurteile, wie gut die folgende Frage zur Antwort passt. Bewerte auf einer Skala von 1 bis 10000, wobei 1 schlecht und 10000 perfekt ist.");
        }
    }
}
