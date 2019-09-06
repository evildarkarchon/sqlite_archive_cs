﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace sqlite_archive_cs
{
    public class SQLiteConvenience
    {
        SQLiteConnection connection;
        public SQLiteConvenience(string filename, bool wal, int autovacuum)
        {
            SQLiteConnectionStringBuilder constring = new SQLiteConnectionStringBuilder
            {
                DataSource = filename,
                FailIfMissing = false,
                ReadOnly = false
            };
            if (wal)
            {
                constring.JournalMode = SQLiteJournalModeEnum.Wal;
            }
            else if (!wal)
            {
                constring.JournalMode = SQLiteJournalModeEnum.Delete;
            }
            SQLiteConnection connection = new SQLiteConnection(constring.ToString());
            connection.Open();

            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
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
                cmd.ExecuteNonQuery();
            }
        }

        public void Compact()
        {
            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = "VACUUM;";
                cmd.ExecuteNonQuery();
            }
        }

        public void ExecQueryCommit(string table, FileInfo fileinfo, bool replace)
        {
            string query;
            if (replace)
            {
                query = string.Format("INSERT OR REPLACE INTO {0} ([filename], [data], [hash]) VALUES (@filename, @data, @hash", table);
            }
            else
            {
                query = string.Format("INSERT INTO {0} ([filename], [data], [hash] VALUES (@filename, @data, @hash", table);
            }
            
            using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@filename", fileinfo.Name);
                cmd.Parameters.AddWithValue("@data", fileinfo.Data);
                cmd.Parameters.AddWithValue("@hash", fileinfo.Digest);
                cmd.ExecuteNonQuery();
            }
        }
        
    }
}