using System.IO;
using System.Collections.Generic;
using CommandLine;

namespace sqlite_archive_cs
{
    public class Options
    {
        [Value(0, Required = true, HelpText = "File name of the database to operate on.")]
        public string Db { get; set; }

        [Option('v', "verbose", HelpText = "Show additional information while performing tasks.")]
        public bool Verbose { get; set; }

        [Option('a', "autovacuum", Default = 1, HelpText = "Sets the automatic vacuum mode. (0 = disabled, 1 = full autovacuum mode, 2 = incremental autovacuum mode)")]
        public int AutoVacuum { get; set; }

    }

    [Verb("add", HelpText = "Options related to adding files to the database.")]
    public class AddFiles
    {
        [Value(0, Max = 1, HelpText = "Name of the table to import files to.")]
        public string Table { get; set; }

        [Value(1, HelpText = "List of files to add to the database.")]
        public IEnumerable<string> Files { get; set; }

        [Option("no-dups", HelpText = "Disables all duplicate entry tracking and reporting.")]
        public bool NoDups { get; set; }

        [Option("no-print-dups", HelpText = "Don't print the list of duplicate files to the terminal.")]
        public bool NoPrintDups { get; set; }

        [Option("dups-current-db", HelpText = "Only print the list of duplicates of the current database if there are more than one databases in the duplicates list.")]
        public bool DupsCurrentDb { get; set; }

        [Option("dups-file", HelpText = "Location of the file to store the list of duplicate files.")]
        public string DupsFile { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "duplicates.json");

        [Option('r', "replace", HelpText = "Enable replacement of an existing entry in the database.")]
        public bool Replace { get; set; }

        [Option("no-replace-vacuum", HelpText = "Don't run VACUUM when replacing an existing entry (does nothing if replace mode is not active)")]
        public bool NoReplaceVacuum { get; set; }

        [Option("lowercase-table-name", HelpText = "Make the case of the inferred table name lowercase (does nothing if table is specified)", Default = false)]
        public bool LowercaseTableName { get; set; }

        [Option("no-atomic", HelpText = "Run commit on every insert instead of at the end of the loop")]
        public bool NoAtomic { get; set; }

        [Option("vacuum", HelpText = "Run VACUUM at the end.")]
        public bool Vacuum { get; set; }

        [Option("exclude", HelpText = ": separated list of files to exclude from insertion.", Separator = ':')]
        public IEnumerable<string> Exclude { get; set; }
    }

    [Verb("create", HelpText = "Create a table in the database.")]
    public class Create
    {
        [Value(0, Max = 1, HelpText = "Name of the table to create")]
        public string Table { get; set; }
    }

    [Verb("drop", HelpText = "Drop specified table.")]
    public class Drop
    {
        [Option("no-vacuum", HelpText = "Don't run VACUUM after dropping the table.")]
        public bool NoVacuum { get; set; }
        [Value(0, Max = 1, HelpText = "Name of the table to drop.")]
        public string Table { get; set; }
    }

    [Verb("extract", HelpText = "Extract files from the database.")]
    public class Extract
    {
        [Value(0, Max = 1, HelpText = "Name of the table to extract data from.")]
        public string Table { get; set; }

        [Value(1, Max = 1, HelpText = "Directory to output files to. Defaults to a directory called output in the current directory.")]
        public string OutputDir { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "output");

        [Value(2, HelpText = "Files to extract from the database.")]
        public IEnumerable<string> Files { get; set; }

        [Option("lowercase-table-name", HelpText = "Make the case of the inferred table name lowercase (does nothing if table is specified)", Default = false)]
        public bool LowercaseTableName { get; set; }

        [Option('f', "force", HelpText = "Forces extraction of a file from the database, even if the digest of the data does not match the one recorded in the database.")]
        public bool Force { get; set; }

        [Option("infer-pop-file", HelpText = "Remove the name of the file used to infer the table name from the file list.")]
        public bool InferPopFile { get; set; }
    }
}