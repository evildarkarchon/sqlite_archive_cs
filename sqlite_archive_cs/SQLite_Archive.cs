using System;
using CommandLine;
//using System.IO;
//using System.Security.Cryptography;
//using System.Linq;
//using System.Text;
//using System.Collections.Generic;

using System.Data.SQLite;

namespace sqlite_archive_cs
{
    class SQLite_Archive
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);
            string connectionstring = result.MapResult(options => $"Data Source={options.DB};Version=3;New=True;", _ => default);
            SQLiteConnection connection;
            SQLiteCommand cmd;
            if (connectionstring != default)
            {
                connection = new SQLiteConnection(connectionstring);
                connection.Open();
                cmd = connection.CreateCommand();
            }
            else
            {
                throw new FormatException("Connection string is invalid");
            }
        }
    }
}
