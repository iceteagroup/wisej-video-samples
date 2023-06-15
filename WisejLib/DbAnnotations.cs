using System;

namespace WisejLib
{
    /// <summary>
    /// This attribute marks a primary key field. Typically data classes derive from DbEntity 
    /// which already includes RowId as the primary key so this attribute shouldn't be
    /// neccessarily used elsewhere
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PK : Attribute { }

    /// <summary>
    /// This attribute marks a field that is not stored in the database so it will be ignored when
    /// DbEntity builds sql statements.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Calculated : Attribute { }

    /// <summary>
    /// This attribute defines the name of the table in the database which can different 
    /// from its model class name
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class Tablename : Attribute
    {
        /// <summary>
        /// Creating the Tablename attribute
        /// </summary>
        /// <param name="name">Name of the table in the database</param>
        public Tablename(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of the table in the database
        /// </summary>
        public string Name { get; set; }
    }
}
