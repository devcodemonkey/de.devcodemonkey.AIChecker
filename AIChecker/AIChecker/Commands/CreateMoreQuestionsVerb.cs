﻿using CommandLine;
using de.devcodemonkey.AIChecker.CoreBusiness.Models;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("createMoreQuestions", HelpText = "Creates more questions under the 'system prompt' and saves them under the result 'set name'.")]
    public class CreateMoreQuestionsVerb : MoreQuestionsUseCaseParams
    {
        [Option('r', "resultSet", Required = true, HelpText = "The result set name.")]
        public string? ResultSet { get; set; }

        [Option('s', "systemPrompt", Required = true, HelpText = "The system prompt or path (path:<file-path>) for creating questions.")]
        public string? SystemPrompt { get; set; }

        [Option("model", Required = true, HelpText = "The model name.")]
        public string Model { get; set; }

        [Option('c', "category", Required = true, HelpText = "The category.")]
        public string Category { get; set; }

        [Option('t', "maxTokens", Default = null, HelpText = "The maximum number of tokens.")]
        public int? MaxTokens { get; set; }

        [Option('p', "temperature", Default = 0, HelpText = "The temperature.")]
        public double Temperature { get; set; }


    }
}
