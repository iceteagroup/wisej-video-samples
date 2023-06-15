using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace WisejLib
{
    /// <summary>Static class to read from the SqlServer database schema</summary>
    public static class DbSchema
    {
        /// <summary>
        /// Returns a list of tables of views.
        /// Uses an internal SqlConnection
        /// </summary>
        /// <param name="tableType">Pass "BASE TABLE" for tables or "VIEW for views</param>
        /// <returns>A list of table names</returns>
        public static List<string> GetTables(string tableType = "BASE TABLE")
        {
            using (var conn = DB.Connection)
                return conn.GetTables(tableType);
        }

        /// <summary>
        /// (Extension) Returns a list of tables or views.
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="tableType"></param>
        /// <returns></returns>
        public static List<string> GetTables(this IDbTransaction tx, string tableType = "BASE TABLE")
        {
            return tx.Connection.GetTables(tableType, tx);
        }

        /// <summary>
        /// (Extension) Returns a list of tables or views.
        /// </summary>
        /// <param name="conn">A SqlServer connection</param>
        /// <param name="tableType">Pass "BASE TABLE" for tables or "VIEW for views</param>
        /// <returns>A list of table names</returns>
        public static List<string> GetTables(this IDbConnection conn, string tableType = "BASE TABLE", IDbTransaction tx = null)
        {
            if (conn is null)
                throw new ArgumentNullException(nameof(conn));
            if (string.IsNullOrEmpty(tableType))
                throw new ArgumentException($"'{nameof(tableType)}' cannot be null or empty.", nameof(tableType));

            string query = $"select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_CATALOG = '{conn.Database}' and TABLE_TYPE = '{tableType}' order by TABLE_Name";
            return conn.Query<string>(query, transaction: tx).ToList();
        }

        /// <summary>
        /// Returns a list of field names of a given table. Uses internal SqlConnection
        /// </summary>
        /// <param name="tableName">Name of the table to return fields from</param>
        /// <returns>List of field names</returns>
        public static List<DbSchemaField> GetFields(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentException($"'{nameof(tableName)}' cannot be null or empty.", nameof(tableName));

            using (var conn = DB.Connection)
                return conn.GetFields(tableName);
        }

        /// <summary>
        /// (Extension) Returns a list of field names of a given table
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static List<DbSchemaField> GetFields(this IDbTransaction tx, string tableName)
        {
            return tx.Connection.GetFields(tableName, tx);
        }

        /// <summary>
        /// (Extension) Returns a list of field names of a given table
        /// </summary>
        /// <param name="conn">IDbConnection to SqlServer</param>
        /// <param name="tableName">Name of the table to return fields from</param>
        /// <returns>List of field names</returns>
        public static List<DbSchemaField> GetFields(this IDbConnection conn, string tableName, IDbTransaction tx = null)
        {
            if (conn is null)
                throw new ArgumentNullException(nameof(conn));
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentException($"'{nameof(tableName)}' cannot be null or empty.", nameof(tableName));

            string query =
                "select COL.TABLE_NAME as Tablename, COL.COLUMN_NAME as Columnname, case when COL.IS_NULLABLE = 'NO' then 1 else 0 end as NotNull,\r\n" +
                "COL.DATA_TYPE as Datatype, COL.CHARACTER_MAXIMUM_LENGTH as Length, COL.NUMERIC_PRECISION as Precision, COL.NUMERIC_SCALE as Scale,\r\n" +
                "COL.ORDINAL_POSITION as Position, case when KY.COLUMN_NAME is null then 0 else 1 end as PK\r\n" +
                "from INFORMATION_SCHEMA.COLUMNS COL\r\n" +
                "left join INFORMATION_SCHEMA.KEY_COLUMN_USAGE KY on KY.TABLE_CATALOG = COL.TABLE_CATALOG and KY.TABLE_NAME = COL.TABLE_NAME and KY.COLUMN_NAME = COL.COLUMN_NAME\r\n" +
                "and KY.CONSTRAINT_NAME in (select TC.CONSTRAINT_NAME from INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC where TC.CONSTRAINT_TYPE = 'PRIMARY KEY' and TC.CONSTRAINT_NAME = KY.CONSTRAINT_NAME)\r\n" +
                ((!string.IsNullOrEmpty(tableName) && tableName != "*") ? $"where COL.TABLE_NAME = '{tableName}'\r\n" : "") +
                "order by COL.TABLE_NAME, COL.ORDINAL_POSITION";
            return conn.Query<DbSchemaField>(query, transaction: tx).ToList();
        }
    }

    /// <summary>Class that describes a table field</summary>
    public class DbSchemaField
    {
        public string Tablename { get; set; }
        public string Columnname { get; set; }
        public int Position { get; set; }
        public bool NotNull { get; set; }
        public string Datatype { get; set; }
        public int Length { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public bool PK { get; set; }

        /// <summary>
        /// Get the C# equivalent of the field's sql type 
        /// </summary>
        public string CSharpType => GetCSharpType();

        private string GetCSharpType()
        {
            string result;
            switch (Datatype)
            {
                case "char":
                case "varchar":
                case "text":
                case "nchar":
                case "nvarchar":
                case "ntext":
                    result = "string";
                    break;
                case "bit":
                    result = "bool";
                    break;
                case "tinyint":
                case "xmallint":
                case "int":
                    result = "int";
                    break;
                case "bigint":
                    result = "long";
                    break;
                case "decimal":
                case "numeric":
                case "money":
                case "smallmoney":
                    result = "decimal";
                    break;
                case "float":
                case "real":
                    result = "double";
                    break;
                case "smalldatetime":
                case "datetime":
                case "datetime2":
                case "date":
                case "Time":
                    result = "DateTime";
                    break;
                case "uniqueidentifier":
                    result = "Guid";
                    break;
                default:
                    result = Datatype;
                    break;
            }
            if (result != "string" && NotNull)
                result += "?";
            return result;
        }
    }

}
