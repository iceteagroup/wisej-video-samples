using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using Dapper;

namespace Schematix.Classes
{
    internal static class DB
    {
        public static IDbConnection Connection { get; private set; }
        public static List<string> Tables { get; private set; }

        public static void Open(string filename)
        {
            Connection = new SQLiteConnection($"Data Source={filename}");
            Tables = GetTables();
        }

        private static List<string> GetTables()
        {
            return Connection.Query<string>("select name from pragma_table_list where name not like 'sqlite_%' order by name").ToList();
        }

        public static List<TableColumn> GetColumns(string tablename)
        {
            var list = Connection.Query<TableColumn>("select * from pragma_table_info(@tablename) order by cid", new { tablename }).ToList();
            list.Insert(0, new TableColumn
            {
                cid = -1,
                name = "RowId",
                type = "int",
                notnull = 1,
                dflt_value = null,
                pk = 1,
            });
            return list;
        }
    }

    internal class TableColumn
    {
        public int cid { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int notnull { get; set; }
        public string dflt_value { get; set; }
        public int pk { get; set; }
        public bool IsRowId => name.Equals("RowId", StringComparison.OrdinalIgnoreCase);

        public string CSharpType => GetCSharpType();

        private string GetCSharpType()
        {
            string nullSign = notnull == 1 ? string.Empty : "?";
            switch (type.ToUpper())
            {
                case "INT":
                case "SMALLINT":
                case "TINYINT":
                    return "int" + nullSign;
                case "DATE":
                case "TIME":
                case "DATETIME":
                    return "DateTime" + nullSign;
                case "BOOL":
                    return "bool" + nullSign;
                case "MONEY":
                    return "decimal" + nullSign;
                case "NTEXT":
                case "TEXT":
                    return "string";
                default:
                    if (type.StartsWith("NVARCHAR") || type.StartsWith("NCHAR") || type.StartsWith("VARCHAR") || type.StartsWith("CHAR"))
                        return "string";
                     return "string";
                    //throw new Exception($"Unhandled Sqlite type [{type}]");
            }
        }
    }
}

