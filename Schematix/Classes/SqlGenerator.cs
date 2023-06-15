using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schematix.Classes
{
    internal class SqlGenerator
    {
        public SqlGenerator(string tablename, List<TableColumn> columns, Options options)
        {
            Tablename = tablename;
            Columns = columns;
            Options = options;
        }

        public static string Generate(string tablename, List<TableColumn> columns, Options options)
        {
            var generator = new SqlGenerator(tablename, columns, options);
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(generator.SelectStatement))
                sb.AppendLine(generator.SelectStatement);

            if (!string.IsNullOrEmpty(generator.InsertStatement))
                sb.AppendLine(generator.InsertStatement);

            if (!string.IsNullOrEmpty(generator.UpdateStatement))
                sb.AppendLine(generator.UpdateStatement);

            if (!string.IsNullOrEmpty(generator.DeleteStatement))
                sb.AppendLine(generator.DeleteStatement);
            return sb.ToString();
        }

        private string Tablename { get; }
        private List<TableColumn> Columns { get; }
        public Options Options { get; }

        public string SelectStatement => GetSelect();
        public string InsertStatement => GetInsert();
        public string UpdateStatement => GetUpdate();
        public string DeleteStatement => GetDelete();


        private string GetSelect()
        {
            var sb = new StringBuilder();

            string columnString;
            if (Columns.Any())
            {
                columnString = ColumnsToString(col => GetFullColumnName(Options, Tablename, col.name), true);
                sb.AppendLine($"select");
                sb.AppendLine($"{columnString}");
                sb.AppendLine($"from {GetFullTablename(Options, Tablename)}");
            }
            else
            {
                var prefix = string.IsNullOrEmpty(Options.TableAlias) ? Tablename : Options.TableAlias;
                var s = Options.FullyQualifiedColumns ? $"{prefix}.*" : "*";
                columnString = $"select {GetFullColumnName(Options, Tablename, "RowId")}, {s} from {GetFullTablename(Options, Tablename)}";
                sb.AppendLine($"{columnString}");
            }

            return sb.ToString();
        }

        private string GetInsert()
        {
            if (!Columns.Any() || (Columns.Count == 1 && Columns[0].IsRowId))
                return string.Empty;

            var sb = new StringBuilder();

            sb.AppendLine($"insert into {Tablename} (");
            sb.AppendLine(ColumnsToString(col => col.name));
            sb.AppendLine(") values (");
            sb.AppendLine(ColumnsToString(col => $"@{col.name}"));
            sb.AppendLine(")");

            return sb.ToString();
        }

        private string GetUpdate()
        {
            if (!Columns.Any() || (Columns.Count == 1 && Columns[0].IsRowId))
                return string.Empty;

            var sb = new StringBuilder();

            sb.AppendLine($"update {Tablename} set");
            sb.AppendLine(ColumnsToString(col => $"{col.name} = @{col.name}"));
            sb.AppendLine("where rowid = @RowId");

            return sb.ToString();
        }

        private string GetDelete()
        {
            return $"delete from {Tablename} where rowid = @RowId";
        }

        private string ColumnsToString(Func<TableColumn, string> convertFunction, bool includeRowId = false)
        {
            var sb = new StringBuilder();

            int columnsInLine = 0;
            foreach (var column in Columns)
            {
                if (column.name.Equals("RowId", StringComparison.OrdinalIgnoreCase) && !includeRowId)
                    continue;

                var value = convertFunction(column);
                if (string.IsNullOrEmpty(value))
                    continue;

                if (columnsInLine == 0)
                {
                    sb.Append($"\t{value}");
                    columnsInLine = 1;
                }
                else if (columnsInLine < Options.ColumnsPerLine)
                {
                    sb.Append($", {value}");
                    columnsInLine++;
                }
                else
                {
                    sb.AppendLine(",");
                    sb.Append($"\t{value}");
                    columnsInLine = 1;
                }
            }
            return sb.ToString();
        }

        public string GetFullTablename(Options options, string tableName)
        {
            string s = tableName;
            if (!string.IsNullOrEmpty(options.TableAlias) && !tableName.Equals(options.TableAlias, StringComparison.OrdinalIgnoreCase))
                s = $"{tableName} as {options.TableAlias}";
            return s;
        }

        public string GetFullColumnName(Options options, string tableName, string columnName)
        {
            var prefix = string.IsNullOrEmpty(options.TableAlias) ? tableName : options.TableAlias;
            var s = options.FullyQualifiedColumns ? $"{prefix}.{columnName}" : columnName;

            if (options.ColumnAlias)
                s = $"{s} as {prefix}_{columnName}";

            return s;
        }

    }
}
