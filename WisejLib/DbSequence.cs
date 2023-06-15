using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WisejLib
{
    /// <summary>
    /// Static class that handles Microsoft SqlServer Sequences
    /// </summary>
    public static class DbSequence
    {
        /// <summary>
        /// Returns a list of informations about defined Sequences
        /// </summary>
        /// <param name="tx">Transaction (required)</param>
        /// <returns>A list of DbSequenceInfo</returns>
        public static List<DbSequenceInfo> Collect(IDbTransaction tx)
        {
            var sql = $"select name, minimum_value, start_value, current_value from sys.sequences";
            return tx.Connection.Query<DbSequenceInfo>(sql, transaction: tx).ToList();
        }

        /// <summary>
        /// Creates a sequence. Method will fail if the sequences already exists
        /// </summary>
        /// <param name="tx">Transaction (required)</param>
        /// <param name="seqName">Name of the sequence (requiored)</param>
        /// <param name="minValue">The value to start with (optional, default=1)</param>
        public static void Create(IDbTransaction tx, string seqName, int minValue = 1)
        {
            CheckParameters(tx, seqName);

            tx.Connection.Execute($"create sequence {seqName.ToUpper()} minvalue {minValue}", transaction: tx);
        }

        /// <summary>
        /// Creates a sequence. Method will fail if the sequences already exists
        /// </summary>
        /// <param name="tx">Transaction (required)</param>
        /// <param name="seqName">Name of the sequence (requiored)</param>
        /// <param name="minValue">The value to start with (optional, default=1)</param>
        public static void CreateIfNotExists(IDbTransaction tx, string seqName, int minValue = 1)
        {
            CheckParameters(tx, seqName);
            seqName = seqName.ToUpper();
            if(tx.Connection.QuerySingleOrDefault<int?>($"select 1 from sys.sequences where name = @seqName", new { seqName }, tx) == null)
                tx.Connection.Execute($"create sequence {seqName} minvalue {minValue}", transaction: tx);
        }

        /// <summary>
        /// deletes a sequence
        /// </summary>
        /// <param name="tx">Transaction (required)</param>
        /// <param name="seqName">Name of the sequence (requiored)</param>
        public static void Drop(IDbTransaction tx, string seqName)
        {
            CheckParameters(tx, seqName);

            tx.Connection.Execute($"drop sequence {seqName.ToUpper()}", transaction: tx);
        }

        /// <summary>
        /// Returns true if the sequence exists
        /// </summary>
        /// <param name="seqName">Name of the sequence (requiored)</param>
        /// <returns>true if the sequence exists</returns>
        public static bool Exists(string seqName)
        {
            using (var conn = DB.Connection)
            {
                var sql = $"select 1 from sys.sequences where name = @Name";
                return conn.QuerySingleOrDefault<int?>(sql, new { Name = seqName }) != null;
            }
        }

        /// <summary>
        /// Returns true if the sequence exists
        /// </summary>
        /// <param name="tx">Transaction (required)</param>
        /// <param name="seqName">Name of the sequence (requiored)</param>
        /// <returns>true if the sequence exists</returns>
        public static bool Exists(IDbTransaction tx, string seqName)
        {
            CheckParameters(tx, seqName);

            var sql = $"select 1 from sys.sequences where name = @Name";
            return tx.Connection.QuerySingleOrDefault<int?>(sql, transaction: tx) != null;
        }

        /// <summary>
        /// Creates a sequence. If it exists it will be overwritten
        /// </summary>
        /// <param name="tx">Transaction (required)</param>
        /// <param name="seqName">Name of the sequence (requiored)</param>
        /// <param name="minValue">The value to start with (optional, default=1)</param>
        public static void Overwrite(IDbTransaction tx, string seqName, int minValue = 1)
        {
            CheckParameters(tx, seqName);

            tx.Connection.Execute($"create sequence {seqName.ToUpper()} minvalue {minValue}", transaction: tx);
        }

        /// <summary>
        /// Queries the database for sequence information
        /// </summary>
        /// <param name="tx">Transaction (required)</param>
        /// <param name="seqName">Name of the sequence (requiored)</param>
        /// <returns>A DbSequenceInfo object or null if the sequence doesn't exist</returns>
        public static DbSequenceInfo QueryInfo(IDbTransaction tx, string seqName)
        {
            CheckParameters(tx, seqName);

            var sql = $"select name, minimum_value, start_value, current_value from sys.sequences where name = @Name";
            var info = new DbSequenceInfo { name = seqName };
            return tx.Connection.QuerySingleOrDefault<DbSequenceInfo>(sql, info, tx);
        }

        /// <summary>
        /// Resets a sequence to a certain value
        /// </summary>
        /// <param name="tx">Transaction (required)</param>
        /// <param name="seqName">Name of the sequence (required)</param>
        /// <param name="currentValue">The value to start the resetted sequence with  (required)</param>
        public static void Restart(IDbTransaction tx, string seqName, int currentValue)
        {
            CheckParameters(tx, seqName);

            tx.Connection.Execute($"alter sequence {seqName} restart with {currentValue}", transaction: tx);
        }

        /// <summary>
        /// Resets a sequence to its start value
        /// </summary>
        /// <param name="tx">Transaction (required)</param>
        /// <param name="seqName">Name of the sequence (required)</param>
        public static void Restart(IDbTransaction tx, string seqName)
        {
            CheckParameters(tx, seqName);

            tx.Connection.Execute($"alter sequence {seqName} restart", transaction: tx);
        }

        /// <summary>
        /// If the Sequence exists, it is set to currentValue.
        /// If it doesn't exist it ist created with startValue
        /// </summary>
        /// <param name="tx">Transaction (required)</param>
        /// <param name="seqName">Name of the sequence (required)</param>
        /// <param name="startValue">The initial value of the sequence when it is created for the first time</param>
        /// <param name="currentValue">The minimum value the sequence ist set to. Pass the highest RowId value of the associated database table here</param>
        public static void Synchronize(IDbTransaction tx, string seqName, int startValue, int currentValue)
        {
            CheckParameters(tx, seqName);

            var info = QueryInfo(tx, seqName);
            if (info != null)
            {
                if (info.current_value != currentValue)
                    Restart(tx, seqName, currentValue);
            }
            else
                Create(tx, seqName, startValue);
        }

        /// <summary>
        /// Internal method. Checking transaction and sequence name parameters is used everywhere so I outsourced the checks.
        /// The attribute [CallerMemberName] automatically inserts the name of the calling method
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="seqName"></param>
        /// <param name="methodName"></param>
        /// <exception cref="ArgumentException"></exception>
        private static void CheckParameters(IDbTransaction tx, string seqName, [CallerMemberName] string methodName = "")
        {
            if (tx is null)
                throw new ArgumentException($"Parameter '{nameof(seqName)}' cannot be null when calling {methodName}.");

            if (string.IsNullOrEmpty(seqName))
                throw new ArgumentException($"Parameter '{nameof(seqName)}' cannot be null or empty when calling {methodName}.");
        }
    }

    /// <summary>
    /// A class that is returned by DbSequence.QueryInfo and holds some of the sequence informations
    /// </summary>
    public class DbSequenceInfo
    {
        /// <summary>
        /// The name of the sequence
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// The lowest value that the sequence can have
        /// </summary>
        public int minimum_value { get; set; }
        /// <summary>
        /// The value that the sequence is started with
        /// </summary>
        public int start_value { get; set; }
        /// <summary>
        /// The value that the sequence has now. Calling NEXT VALUE FOR will return current_value + 1
        /// </summary>
        public int current_value { get; set; }
    }
}
