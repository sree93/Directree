using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace Directree.Logic.Data
{
    public class DbHelper
    {
        private string CONNECTION_STRING = "Data Source=data.db";
        private string DATABASE_FILENAME = "data.db";

        private string[] CREATE_STATEMENTS = {
                                                 "CREATE TABLE cleaner_delete(_id INTEGER PRIMARY KEY AUTOINCREMENT, sourceDir varchar(1000), extList varchar(500), includeSubDirs INTEGER, lastAction REAL, interval REAL)",
                                                 "CREATE TABLE cleaner_move(_id INTEGER PRIMARY KEY AUTOINCREMENT, sourceDir varchar(1000), destDir varchar(1000), extList varchar(500), includeSubDirs INTEGER, lastAction REAL, interval REAL)"
                                             };

        public DbHelper()
        {
            if (!File.Exists(DATABASE_FILENAME))
            {
                SQLiteConnection.CreateFile(DATABASE_FILENAME);
                using (SQLiteConnection con = new SQLiteConnection(CONNECTION_STRING))
                {
                    SQLiteCommand command = con.CreateCommand();

                    con.Open();
                    foreach (string cmd in CREATE_STATEMENTS)
                    {
                        command.CommandText = cmd;
                        command.ExecuteNonQuery();
                    }
                }

            }
        }

        public int executeNonQuery(string cmd)
        {
            int result = -1;

            using (SQLiteConnection con = new SQLiteConnection(CONNECTION_STRING))
            {
                SQLiteCommand command = new SQLiteCommand(cmd, con);
                result = command.ExecuteNonQuery();
            }

            return result;
        }

        public SQLiteDataReader executeQuery(string cmd)
        {
            SQLiteDataReader sdr;
            using (SQLiteConnection con = new SQLiteConnection(CONNECTION_STRING))
            {
                SQLiteCommand command = new SQLiteCommand(cmd, con);
                sdr = command.ExecuteReader();
            }

            return sdr;
        }
    }
}
