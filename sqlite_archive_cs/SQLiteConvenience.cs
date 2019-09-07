using System;
using System.Collections.Generic;
//using System.Text;
using System.Data.SQLite;

namespace sqlite_archive_cs
{
    public abstract class SQLiteConvenienceAbstract
    {
        public abstract void Compact();
        public abstract void InsertFilesNoAtomic(string table, FileInfo fileinfo, bool replace, bool verbose, bool replacenovacuum);
        public abstract void InsertFilesNoAtomic(string table, List<string> files, bool replace, bool verbose, bool replacenovacuum);
        public abstract void InsertFilesAtomic(string table, FileInfo fileinfo, bool replace, bool verbose, bool replacenovacuum);
        public abstract void InsertFilesAtomic(string table, List<string> files, bool replace, bool verbose, bool replacenovacuum);
    }
    public class SQLiteConvenience : SQLiteConvenienceAbstract
    {
        private readonly SQLiteConnection _connection;
        public SQLiteConvenience(string filename, bool wal, int autovacuum, bool verbose)
        {
            SQLiteConnectionStringBuilder constring = new SQLiteConnectionStringBuilder
            {
                DataSource = filename,
                FailIfMissing = false,
                ReadOnly = false
            };
            /*
            if (wal)
            {
                constring.JournalMode = SQLiteJournalModeEnum.Wal;
            }
            else
            {
                constring.JournalMode = SQLiteJournalModeEnum.Delete;
            }
            */

            switch (wal)
            {
                case true:
                    constring.JournalMode = SQLiteJournalModeEnum.Wal;
                    break;
                case false:
                default:
                    constring.JournalMode = SQLiteJournalModeEnum.Delete;
                    break;
            }

            SQLiteConnection _connection = new SQLiteConnection(constring.ToString());
            _connection.Open();

            List<string> AutoVacuumStatus = new List<string>
            {
                "AutoVacuum is disabled.",
                "AutoVacuum set to full.",
                "AutoVacuum set to incremental."
            };

            using (SQLiteCommand cmd = new SQLiteCommand(_connection))
            {
                /*
                if (autovacuum == 0)
                {
                    cmd.CommandText = "PRAGMA auto_vacuum = 0";
                }
                else if (autovacuum == 1 || autovacuum > 2)
                {
                    cmd.CommandText = "PRAGMA auto_vacuum = 1";
                }
                else if (autovacuum == 2)
                {
                    cmd.CommandText = "PRAGMA auto_vacuum = 2";
                }
                else
                {
                    cmd.CommandText = "PRAGMA auto_vacuum = 1";
                }
                */

                switch (autovacuum)
                {
                    case 0:
                        cmd.CommandText = "PRAGMA auto_vacuum = 0;";
                        if (verbose == true)
                        {
                            Console.WriteLine(AutoVacuumStatus[0]);
                        }
                        break;
                    case 1:
                    default:
                        cmd.CommandText = "PRAGMA auto_vacuum = 1;";
                        if (verbose == true)
                        {
                            Console.WriteLine(AutoVacuumStatus[1]);
                        }
                        break;
                    case 2:
                        cmd.CommandText = "PRAGMA auto_vacuum = 2;";
                        if (verbose == true)
                        {
                            Console.WriteLine(AutoVacuumStatus[2]);
                        }
                        break;
                }

                cmd.ExecuteNonQuery();
            }
        }

        public SQLiteConvenience(string filename, int autovacuum, bool verbose)
        {
            SQLiteConnectionStringBuilder constring = new SQLiteConnectionStringBuilder
            {
                DataSource = filename,
                FailIfMissing = false,
                ReadOnly = false
            };
            SQLiteConnection _connection = new SQLiteConnection(constring.ToString());
            _connection.Open();

            List<string> AutoVacuumStatus = new List<string>
            {
                "AutoVacuum is disabled.",
                "AutoVacuum set to full.",
                "AutoVacuum set to incremental."
            };

            using (SQLiteCommand cmd = new SQLiteCommand(_connection))
            {
                /*
                if (autovacuum == 0)
                {
                    cmd.CommandText = "PRAGMA auto_vacuum = 0";
                }
                else if (autovacuum == 1 || autovacuum > 2)
                {
                    cmd.CommandText = "PRAGMA auto_vacuum = 1";
                }
                else if (autovacuum == 2)
                {
                    cmd.CommandText = "PRAGMA auto_vacuum = 2";
                }
                else
                {
                    cmd.CommandText = "PRAGMA auto_vacuum = 1";
                }
                */

                switch (autovacuum)
                {
                    case 0:
                        cmd.CommandText = "PRAGMA auto_vacuum = 0;";

                        if (verbose)
                        {
                            Console.WriteLine(AutoVacuumStatus[0]);
                        }
                        break;
                    case 1:
                    default:
                        cmd.CommandText = "PRAGMA auto_vacuum = 1;";

                        if (verbose)
                        {
                            Console.WriteLine(AutoVacuumStatus[1]);
                        }
                        break;
                    case 2:
                        cmd.CommandText = "PRAGMA auto_vacuum = 2;";

                        if (verbose)
                        {
                            Console.WriteLine(AutoVacuumStatus[2]);
                        }
                        break;
                }

                cmd.ExecuteNonQuery();
            }
        }

        public SQLiteConvenience(string filename)
        {
            SQLiteConnectionStringBuilder constring = new SQLiteConnectionStringBuilder
            {
                DataSource = filename,
                FailIfMissing = false,
                ReadOnly = false
            };
            SQLiteConnection _connection = new SQLiteConnection(constring.ToString());
            _connection.Open();

            using (SQLiteCommand cmd = new SQLiteCommand(_connection))
            {
                cmd.CommandText = "PRAGMA auto_vacuum = 1";
                cmd.ExecuteNonQuery();
            }
        }

        public override void Compact()
        {
            Console.Out.NewLine = " ";
            Console.WriteLine("Compacting the database, this might take a while...");
            using (SQLiteCommand cmd = new SQLiteCommand(_connection))
            {
                cmd.CommandText = "VACUUM;";
                cmd.ExecuteNonQuery();
            }
            Console.Out.NewLine = Environment.NewLine;
            Console.WriteLine("done.");
        }

        public override void InsertFilesNoAtomic(string table, FileInfo fileinfo, bool replace, bool verbose, bool replacenovacuum)
        {
            string query;
            if (replace)
            {
                query = $"INSERT OR REPLACE INTO {table} ([filename], [data], [hash]) VALUES (@filename, @data, @hash)";
            }
            else
            {
                query = $"INSERT INTO {table} ([filename], [data], [hash]) VALUES (@filename, @data, @hash)";
            }
            
            using (SQLiteCommand cmd = new SQLiteCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@filename", fileinfo.Name);
                cmd.Parameters.AddWithValue("@data", fileinfo.GetData());
                cmd.Parameters.AddWithValue("@hash", fileinfo.Digest);
                cmd.ExecuteNonQuery();
            }

            if (!replacenovacuum && replace)
            {
                Compact();
            }
        }
        public override void InsertFilesNoAtomic(string table, List<string> files, bool replace, bool verbose, bool replacenovacuum)
        {
            foreach (string value in files)
            {
                string query;
                FileInfo fileinfo = new FileInfo(value);
                if (replace)
                {
                    query = $"INSERT OR REPLACE INTO {table} ([filename], [data], [hash] VALUES (@filename, @data, @hash)";
                }
                else
                {
                    query = $"INSERT INTO {table} ([filename], [data], [hash] VALUES (@filename, @data, @hash)";
                }

                using (SQLiteCommand cmd = new SQLiteCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@filename", fileinfo.Name);
                    cmd.Parameters.AddWithValue("@data", fileinfo.GetData());
                    cmd.Parameters.AddWithValue("@hash", fileinfo.Digest);
                    cmd.ExecuteNonQuery();
                }

                if (!replacenovacuum && replace)
                {
                    Compact();
                }
            }
        }

        public override void InsertFilesAtomic(string table, FileInfo fileinfo, bool replace, bool verbose, bool replacenovacuum)
        {
            InsertFilesNoAtomic(table, fileinfo, replace, verbose, replacenovacuum);
        }
        public override void InsertFilesAtomic(string table, List<string> files, bool replace, bool verbose, bool replacevacuum)
        {
            throw new NotImplementedException();
        }
    }
}
