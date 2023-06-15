using System;
using System.Data;
using System.Data.SQLite;

namespace WisejLib
{
    /// <summary>
    /// Database helper class that currently does nothing else than holding the connection 
    /// string and creating a connection
    /// </summary>
    public static class DB
    {
        /// <summary>
        /// The connection string to be used for establishing connections to a Sqlite database.
        /// THis ConnectionStirng must be set prior to accessing the Connection property
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// Use this to get a connection to a Sqlite 3 database.
        /// Important: the ConnectionString property must have been set prior 
        /// to accessing the Connection property
        /// </summary>
        public static IDbConnection Connection => GetConnection();

        /// <summary>
        /// Private method to open a connection
        /// </summary>
        /// <returns></returns>
        private static IDbConnection GetConnection()
        {
            IDbConnection conn = new SQLiteConnection(ConnectionString);
            conn.Open();
            return conn;
        }
    }
}
