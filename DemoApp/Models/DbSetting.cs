using Dapper;
using DemoApp.Schema;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DemoApp.Models
{
    /// <summary>
    /// Database wrapper for the Settings Schema class
    /// </summary>
    public class DbSetting : Settings
    {
        /// <summary>
        /// Returns a list of settings
        /// </summary>
        /// <param name="conn">A Sql connection</param>
        /// <param name="userRowId">The RowId of the user for whom settings are to be read or 0 to read all settings from all users</param>
        /// <returns>List of DbSetting</returns>
        public static List<DbSetting> GetAll(IDbConnection conn, int userRowId = 0)
        {
            var sql = "select rowid, * from settings where " + (userRowId > 0 ? $"UserId = {userRowId}" : "UserId is not null");
            return conn.Query<DbSetting>(sql).ToList();
        }

        /// <summary>
        /// Return Settings identified by name and (optionally) user RowId
        /// </summary>
        /// <param name="conn">The Sql Connection</param>
        /// <param name="name">The name of the setting (case-insensitive)</param>
        /// <param name="userRowId">RowId of the user</param>
        /// <returns>The DbSettings or null if not found</returns>
        public static DbSetting GetByName(IDbConnection conn, string name, int userRowId)
        {
            var sql = "select rowid, * from settings where SettingsName = @name";
            if (userRowId > 0)
                sql += $" and UserId = {userRowId}";
            return conn.QuerySingleOrDefault<DbSetting>(sql, new { name });
        }

        /// <summary>
        /// Return the application's general settings. This setting has UserId == null and the name is irrelevant
        /// </summary>
        /// <param name="conn">The Sql Connection</param>
        /// <returns>Return the application's general settings or null if not found</returns>
        public static DbSetting GetAppSettings(IDbConnection conn)
        {
            var sql = "select rowid, * from settings where UserId is null";
            return conn.QuerySingleOrDefault<DbSetting>(sql);
        }
    }
}
