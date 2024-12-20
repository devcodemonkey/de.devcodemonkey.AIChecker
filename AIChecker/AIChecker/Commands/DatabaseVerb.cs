﻿using CommandLine;

namespace de.devcodemonkey.AIChecker.AIChecker.Commands
{
    [Verb("database", HelpText = "Start or stop the database. If no Option is set the database will start")]
    public class DatabaseVerb
    {
        [Option('s', "stop", HelpText = "Stop the database.")]
        public bool Stop { get; set; }

        [Option('r', "start", HelpText = "Start the database.")]
        public bool Start { get; set; }

        [Option('b', "backup", HelpText = "Backup the database.")]
        public bool Backup { get; set; }

        [Option('o', "restore", HelpText = "Restore the database.")]
        public bool Restore { get; set; }

        [Option("branch", HelpText = "The branch to restore the database from.")]
        public string Branch { get; set; }

        [Option("recreateDatabase", HelpText = "Recreates the database.")]
        public bool RecreateDatabase { get; set; }

        [Option('f', "force", Required = false, HelpText = "Force the recreation of the database.")]
        public bool Force { get; set; }
    }
}
