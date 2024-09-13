﻿using CommandLine;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("importQuestions", HelpText = "Imports Questions and Answers to the db.")]
    public class ImportQuestionsVerb
    {
        [Option('p', "path", Required = true, HelpText = "Path to the file with Questions and Answers.")]
        public string? Path { get; set; }
    }
}