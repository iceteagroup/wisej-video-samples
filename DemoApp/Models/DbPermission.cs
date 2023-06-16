using Dapper;
using DemoApp.Schema;
using System.Collections.Generic;
using System.Linq;
using DemoApp.Classes;
using WisejLib;

namespace DemoApp.Models
{
    public class DbPermission : Permissions
    {
        // PermissionId ids, used in Items
        public const int PERM_Users = 0;
        public const int PERM_Addresses = PERM_Users + 1;
        public const int PERM_Invoices = PERM_Addresses + 1;

        /// <summary>
        /// Lookup list of permissions
        /// </summary>
        public static readonly LookupPair[] Items = new LookupPair[]
        {
            new LookupPair{ Id = PERM_Users, Text = "Users" },
            new LookupPair{ Id = PERM_Addresses, Text = "Adresses" },
            new LookupPair{ Id = PERM_Invoices, Text = "Invoices" },
        };

        [Calculated]
        public string PermissionText => Items[PermissionId].Text;
        
        /// <summary>
        /// Load all permissions of a userId
        /// </summary>
        /// <param name="userId">RowId of the user in question</param>
        public static List<DbPermission> GetAll(int userId)
        {
            using (var conn = DB.Connection)
                return conn.Query<DbPermission>($"select rowid, * from Permissions where UserId = {userId}").ToList();
        }
    }
}
