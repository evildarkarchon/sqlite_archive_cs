using System;
using System.Collections.Generic;
//using System.Text;
using System.Data.SQLite;

namespace sqlite_archive_cs
{
    public abstract class SQLiteActionAbstract
    {
        public abstract void Compact();
        public abstract void InsertFilesNoAtomic(string table, FileInfo fileinfo, bool replace, bool verbose, bool replacenovacuum);
        public abstract void InsertFilesNoAtomic(string table, List<string> files, bool replace, bool verbose, bool replacenovacuum);
        public abstract void InsertFilesAtomic(string table, FileInfo fileinfo, bool replace, bool verbose, bool replacenovacuum);
        public abstract void InsertFilesAtomic(string table, List<string> files, bool replace, bool verbose, bool replacenovacuum);
    }
    public class SQLiteConvenience : SQLiteActionAbstract
    {
        private readonly SQLiteConnection connection;

        public SQLiteConnection GetConnection() => connection;

        private int GetAutoVacuum()
        {
            using (SQLiteCommand cmd = new SQLiteCommand(GetConnection()))
            {
                cmd.CommandText = "PRAGMA auto_vacuum;";
                return int.Parse(cmd.ExecuteScalar().ToString());
            }
        }
        public SQLiteConvenience(string filename, bool wal, int autovacuum, bool verbose)
        {
            SQLiteConnectionStringBuilder constring = new SQLiteConnectionStringBuilder
            {
                DataSource = filename,
                FailIfMissing = false,
                ReadOnly = false
            };
            var is_network_path = Utility.IsNetworkPath(filename);
            switch (wal)
            {

                case true when is_network_path == false:
                    constring.JournalMode = SQLiteJournalModeEnum.Wal;
                    break;
                case true when is_network_path == true:
                case false:
                default:
                    constring.JournalMode = SQLiteJournalModeEnum.Delete;
                    break;
            }

            connection = new SQLiteConnection(constring.ToString());
            connection.Open();

            List<string> AutoVacuumStatus = new List<string>
            {
                "AutoVacuum is disabled.",
                "AutoVacuum set to full.",
                "AutoVacuum set to incremental."
            };

            using (SQLiteCommand cmd = new SQLiteCommand(GetConnection()))
            {
                var AutoVacuum = GetAutoVacuum();
                int NewAutoVacuum;

                switch (autovacuum)
                {
                    case 0 when AutoVacuum != 0:
                        cmd.CommandText = "PRAGMA auto_vacuum = 0;";
                        cmd.ExecuteNonQuery();
                        NewAutoVacuum = GetAutoVacuum();
                        if (verbose && NewAutoVacuum == 0)
                        {
                            Console.WriteLine(AutoVacuumStatus[0]);
                        }
                        break;
                    case 1 when AutoVacuum != 1:
                        cmd.CommandText = "PRAGMA auto_vacuum = 1;";
                        cmd.ExecuteNonQuery();
                        NewAutoVacuum = GetAutoVacuum();
                        if (verbose && NewAutoVacuum == 1)
                        {
                            Console.WriteLine(AutoVacuumStatus[1]);
                        }
                        break;
                    case 2 when AutoVacuum != 2:
                        cmd.CommandText = "PRAGMA auto_vacuum = 2;";
                        cmd.ExecuteNonQuery();
                        NewAutoVacuum = GetAutoVacuum();
                        if (verbose && NewAutoVacuum == 2)
                        {
                            Console.WriteLine(AutoVacuumStatus[2]);
                        }
                        break;
                    default:
                        if (AutoVacuum != 1)
                        {
                            cmd.CommandText = "PRAGMA auto_vacuum = 1;";
                            cmd.ExecuteNonQuery();

                            NewAutoVacuum = GetAutoVacuum();
                            if (verbose && NewAutoVacuum == 1)
                            {
                                Console.WriteLine(AutoVacuumStatus[1]);
                            }
                        }
                        break;
                }
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
            connection = new SQLiteConnection(constring.ToString());
            connection.Open();

            List<string> AutoVacuumStatus = new List<string>
            {
                "AutoVacuum is disabled.",
                "AutoVacuum set to full.",
                "AutoVacuum set to incremental."
            };

            using (SQLiteCommand cmd = new SQLiteCommand(GetConnection()))
            {
                var AutoVacuum = GetAutoVacuum();
                int NewAutoVacuum;

                switch (autovacuum)
                {
                    case 0:
                        if (AutoVacuum != 0)
                        {
                            cmd.CommandText = "PRAGMA auto_vacuum = 0;";
                            cmd.ExecuteNonQuery();
                        }
                        NewAutoVacuum = GetAutoVacuum();
                        if (verbose && NewAutoVacuum == 0)
                        {
                            Console.WriteLine(AutoVacuumStatus[0]);
                        }
                        break;
                    case 1:
                    default:
                        if (AutoVacuum != 1)
                        {
                            cmd.CommandText = "PRAGMA auto_vacuum = 1;";
                            cmd.ExecuteNonQuery();
                        }
                        NewAutoVacuum = GetAutoVacuum();
                        if (verbose && NewAutoVacuum == 1)
                        {
                            Console.WriteLine(AutoVacuumStatus[1]);
                        }
                        break;
                    case 2:
                        if (AutoVacuum != 2)
                        {
                            cmd.CommandText = "PRAGMA auto_vacuum = 2;";
                            cmd.ExecuteNonQuery();
                        }
                        NewAutoVacuum = GetAutoVacuum();
                        if (verbose && NewAutoVacuum == 2)
                        {
                            Console.WriteLine(AutoVacuumStatus[2]);
                        }
                        break;
                }
            }
        }

        public SQLiteConvenience(string filename, bool verbose)
        {
            SQLiteConnectionStringBuilder constring = new SQLiteConnectionStringBuilder
            {
                DataSource = filename,
                FailIfMissing = false,
                ReadOnly = false
            };
            connection = new SQLiteConnection(constring.ToString());
            connection.Open();

            using (SQLiteCommand cmd = new SQLiteCommand(GetConnection()))
            {
                if (GetAutoVacuum() != 1)
                {
                    cmd.CommandText = "PRAGMA auto_vacuum = 1";
                    cmd.ExecuteNonQuery();
                    if (verbose && GetAutoVacuum() == 1)
                    {
                        Console.WriteLine("AutoVacuum set to full.");
                    }
                }
            }
        }

        public override void Compact()
        {
            Console.Out.NewLine = " ";
            Console.WriteLine("Compacting the database, this might take a while...");
            using (SQLiteCommand cmd = new SQLiteCommand(GetConnection()))
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

            using (SQLiteCommand cmd = new SQLiteCommand(query, GetConnection()))
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

                using (SQLiteCommand cmd = new SQLiteCommand(query, GetConnection()))
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

        public override void InsertFilesAtomic(string table, FileInfo fileinfo, bool replace, bool verbose, bool replacenovacuum) => InsertFilesNoAtomic(table, fileinfo, replace, verbose, replacenovacuum);
        public override void InsertFilesAtomic(string table, List<string> files, bool replace, bool verbose, bool replacenovacuum)
        {
            throw new NotImplementedException();
        }
    }
}
