using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace WisejLib
{
    /// <summary>
    /// These flags defines which fields CollectFields will return. The flags can be combined using the | statement
    /// </summary>
    [Flags]
    public enum PropertyCollectFlags
    {
        /// <summary>
        /// Collect fields but not primary keys
        /// </summary>
        Fields = 1,
        /// <summary>
        /// Collect primary keys
        /// </summary>
        PrimaryKey = 2
    }

    /// <summary>
    /// Defines the state of an entity. SaveChanges respects DbState
    /// The states cannot be combined
    /// </summary>
    public enum DbState
    {
        /// <summary>
        /// data is not modified
        /// </summary>
        None = 0,
        /// <summary>
        /// It's a new record
        /// </summary>
        New = 1,
        /// <summary>
        /// It's an existing record that has been changed
        /// </summary>
        Changed = 2,
        /// <summary>
        /// record is to be deleted
        /// </summary>
        Deleted = 4
    }

    /// <summary>
    /// This is the base of every model class. It implements the INotifyPropertyChanged interface
    /// </summary>
    public class DbEntity : INotifyPropertyChanged
    {
        /// <summary>
        /// RowId is the unique primary key of a each and every table in Sqlite. Since all Schema classes 
        /// derive from DbEntity they don't need to declare RowId as a property.
        /// </summary>
        [PK]
        public int RowId
        {
            get => GetProperty<int>();
            set => SetProperty(value);
        }

        /// <summary>
        /// State of this entity (see DbState)
        /// This is not changed internally, except for the Saving process.
        /// </summary>
        [Calculated]
        public DbState State { get; set; } = DbState.None;

        /// <summary>
        /// Fired whenever a property in this class or in a derived class changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Retrieves the table name from the Table attribute. If the attribute is missing, the class name is used.
        /// This property uses lazy loading so the attribute is only queried once
        /// </summary>
        public string Tablename
        {
            get
            {
                if (_Tablename == null)
                    _Tablename = GetTablename();
                return _Tablename;
            }
        }
        /// <summary>
        /// Returns the SQL statement for saving this model class in the database
        /// This property employs lazy loading and the string is only built once
        /// </summary>
        private string SqlInsertStatement
        {
            get
            {
                if (_SqlInsertStatement == null)
                    _SqlInsertStatement = BuildSqlInsertStatement();
                return _SqlInsertStatement;
            }
        }
        /// <summary>
        /// Returns the SQL statement for saving this model class in the database
        /// This property employs lazy loading and the string is only built once
        /// </summary>
        private string SqlUpdateStatement
        {
            get
            {
                if (_SqlUpdateStatement == null)
                    _SqlUpdateStatement = BuildSqlUpdateStatement();
                return _SqlUpdateStatement;
            }
        }
        /// <summary>
        /// Returns the SQL statement for deleteing this model class in the database
        /// This property employs lazy loading and the string is only built once
        /// </summary>
        private string SqlDeleteStatement
        {
            get
            {
                if (_SqlDeleteStatement == null)
                    _SqlDeleteStatement = BuildSqlDeleteStatement();
                return _SqlDeleteStatement;
            }
        }

        private string _Tablename;
        private string _SqlInsertStatement;
        private string _SqlUpdateStatement;
        private string _SqlDeleteStatement;

        /// <summary>
        /// Connects to the database and saves the class data in a local transaction.
        /// If RowId is 0, a new database record is inserted otherwise the record identified by RowId is updated
        /// </summary>
        public void SaveChanges()
        {
            if (State == DbState.None)
                return;

            using (var conn = DB.Connection)
            using (var tx = conn.BeginTransaction())
            {
                SaveChanges(tx);
                tx.Commit();
            }
        }

        /// <summary>
        /// Uses the specified transaction to save the class data to the database.
        /// If RowId is 0, a new database record is inserted otherwise the record identified by RowId is updated
        /// </summary>
        public void SaveChanges(IDbTransaction tx)
        {
            if (State == DbState.Deleted)
            {
                tx.Connection.Execute(SqlDeleteStatement, this, tx);
                return;
            }

            // RowId = 0 basically means DbState.New
            // by using the RowId rather than of DbState this method does not require the DbState to be set externally.
            if (RowId == 0)
                RowId = tx.Connection.QuerySingle<int>(SqlInsertStatement, this, tx);
            else
                tx.Connection.Execute(SqlUpdateStatement, this, tx);

            // Whatever the state was before, now that it's saved, the State will be DbState.None
            State = DbState.None;
        }

        /// <summary>
        /// This dictionary holds all property values
        /// </summary>
        private Dictionary<string, object> PropertyValues { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Retrieve a property.
        /// Cool trick (I didn't know, Gianluca from ITG introduced this to me): the CallerMemberName fills the name parameter
        /// automagically using the nbame of the property from where this method was called. In other words, you 
        /// can omit the name parameter and it will still contain the calling property's name
        /// </summary>
        protected T GetProperty<T>([CallerMemberName] string name = "")
        {
            if (PropertyValues.TryGetValue(name, out object value))
                return (T)value;
            return default;
        }

        /// <summary>
        /// Set a property and fire the PropertyChanged event if the new property value is different from the old value
        /// </summary>
        protected void SetProperty(object value, [CallerMemberName] string name = "")
        {
            PropertyValues.TryGetValue(name, out object oldValue);
            if (!Equals(oldValue, value))
            {
                PropertyValues[name] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Collects all fields and/or primary keys, depending on the PropertyCollectFlags
        /// </summary>
        /// <param name="flags">See PropertyCollectFlags</param>
        /// <returns>A list of field names</returns>
        private List<string> CollectFields(PropertyCollectFlags flags = PropertyCollectFlags.Fields)
        {
            return CollectFields(GetType(), flags);
        }

        /// <summary>
        /// Collects all fields and/or primary keys from the specified type, depending on the PropertyCollectFlags
        /// </summary>
        /// <param name="type">The type from which fields are to be collected</param>
        /// <param name="flags">See PropertyCollectFlags</param>
        /// <returns>A list of field names</returns>
        private static List<string> CollectFields(Type type, PropertyCollectFlags flags = PropertyCollectFlags.Fields)
        {
            if (!flags.HasFlag(PropertyCollectFlags.Fields) && !flags.HasFlag(PropertyCollectFlags.PrimaryKey))
                throw new ArgumentException("PropertyCollectFlags must contain at least 1 flag");

            var list = new List<string>();

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var prop in properties)
            {
                bool isPublic = prop.GetSetMethod() != null;
                bool isReadWrite = prop.CanRead && prop.CanWrite;
                bool isCalculated = prop.GetCustomAttribute<Calculated>(true) != null;

                if (!isPublic || !isReadWrite || isCalculated)
                    continue;

                bool isPrimaryKey = prop.GetCustomAttribute<PK>(true) != null;
                bool wantPrimaryKey = flags.HasFlag(PropertyCollectFlags.PrimaryKey);
                bool wantFields = flags.HasFlag(PropertyCollectFlags.Fields);
                if ((isPrimaryKey && wantPrimaryKey) || (!isPrimaryKey && wantFields))
                    list.Add(prop.Name);
            }
            return list;
        }

        /// <summary>
        /// Retrieve the Table attribute value. If the attribute is missing the class name is returned
        /// </summary>
        private string GetTablename()
        {
            Type type = GetType();
            var attr = type.GetCustomAttribute<Tablename>(true);
            return attr != null ? attr.Name : type.Name;
        }

        /// <summary>
        /// Collects the field names from the class and combines them into a SQL update statement
        /// </summary>
        private string BuildSqlUpdateStatement()
        {
            var fields = CollectFields(PropertyCollectFlags.Fields);
            var primaryKey = CollectFields(PropertyCollectFlags.PrimaryKey);

            var sb = new StringBuilder();

            sb.AppendLine($"update {Tablename} set");
            sb.AppendLine(fields.ListAsString(f => $"{f} = @{f}"));
            sb.AppendLine($"where {primaryKey.ListAsString(f => $"{f} = @{f}", " and ")}");

            return sb.ToString();
        }

        /// <summary>
        /// Collects the field names from the class and combines them into a SQL insert statement. Note that the 
        /// RowId field will never be set in this statement because it is automatically incremented by the sqlite
        /// database. However, the newly incremented RowId value will be written to the RowId property.
        /// </summary>
        private string BuildSqlInsertStatement()
        {
            List<string> fields = CollectFields(PropertyCollectFlags.Fields);
            var sb = new StringBuilder();
            sb.AppendLine($"insert into {Tablename}");
            sb.AppendLine($"({fields.ListAsString(f => f)})");
            sb.AppendLine("values");
            sb.AppendLine($"({fields.ListAsString(f => $"@{f}")})");
            sb.AppendLine($"returning RowId");

            return sb.ToString();
        }

        /// <summary>
        /// Collects the field names of the primary key from the class and combines them into a SQL delete statement
        /// </summary>
        private string BuildSqlDeleteStatement()
        {
            var primaryKey = CollectFields(PropertyCollectFlags.PrimaryKey);

            var sb = new StringBuilder();
            sb.AppendLine($"delete from {Tablename}");
            sb.AppendLine($"where {primaryKey.ListAsString(f => $"{f} = @{f}", " and ")}");

            return sb.ToString();
        }
    }
}