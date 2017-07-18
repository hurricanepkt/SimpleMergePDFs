using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace SimpleMergePDFs
{
    class Options
    {
        private string assemblyVersion;

        public Options(Version assemblyVersion)
        {
            this.assemblyVersion = string.Format("{0}.{1}.{2}.{3}", assemblyVersion.Major, assemblyVersion.Minor ,  assemblyVersion.Build, assemblyVersion.MinorRevision);
        }

        [Option('f', "folder", Required = true, HelpText = "Input folder to process.")]
        public string InputFolder { get; set; }

        // Omitting long name, default --verbose
        [Option('v', "verbose", HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option('r', "recursive", HelpText = "Searches All Subfolders")]
        public bool Recursive { get; set; }
        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("Simple Merge PDFs", assemblyVersion),
                Copyright = new CopyrightInfo("Mark Greenway", 2017),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddOptions(this);
            return help;
        }
    }
}