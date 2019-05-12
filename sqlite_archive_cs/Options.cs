using System.IO;
using CommandLine;

namespace sqlite_archive_cs
{
    class Options
    {
        [Option('d', "db", Required = true, HelpText = "File name of the database to operate on.")]
        public string DB { get; set; }

        [Option('t', Required = true, HelpText ="Name of the table to import files to.")]
        public string Table { get; set; }

        [Option('x', HelpText = "Extracts files from the database instead of adding them.")]
        public bool Extract { get; set; }

        [Option("output-dir", HelpText = "Directory to extract files to. Default is current directory. Has no effect if extract mode is not enabled.")]
        public string OutputDir { get; set; } = Directory.GetCurrentDirectory();

        [Option(HelpText = "Replace existing entries in the database instead of skipping them.")]
        public bool Replace { get; set; }

        [Option(HelpText = "Print some additional information")]
        public bool Verbose { get; set; }

        [Option("json-file", HelpText = "Location of the file to output the JSON of the list of duplicates.")]
        public string JsonFile { get; set; } = $"{Directory.GetCurrentDirectory()}\\duplicates.json";

        [Option("quiet-json", HelpText = "Don't print the json of the duplicates to the terminal.")]
        public bool QuietJson { get; set; }

        [Option("full-dups-path", HelpText = "Put the full path of a file as the key for the duplicates json file instead of a relative path.")]
        public bool FullDups { get; set; }

        [Option("dups-current-db", HelpText ="Only show the duplicates from the current database instead of all of them.")]
        public bool DupsCurrentDB { get; set; }

        [Option(HelpText ="Run VACUUM and exit.")]
        public bool Compact { get; set; }
    }
}