using Dapper;
using DemoApp.Classes;
using DemoApp.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WisejLib;

namespace DemoApp.Models
{
    /// <summary>
    /// These are used when checking if a user has specific rights
    /// </summary>
    [Flags]
    public enum Rights { Void = 0, Edit = 1, Delete = 2 }

    /// <summary>
    /// Class that extends the Users class from Phonic.Schema
    /// </summary>
    public class DbUser : Users
    {
        public DbUser()
        {
            Salutation = Salutations.SALUTATION_Male;
        }

        /// <summary>
        /// Copy an instance of a DbUser to a new instance and not a 2nd reference to the origina instance
        /// </summary>
        public DbUser Copy()
        {
            var newObject = Utils.Clone<DbUser>(this);
            newObject.State = DbState.New;
            newObject.RowId = 0;
            return newObject;
        }

        /// <summary>
        /// Users have firstname and lastname. This property returns the 2 in the form lastname-comma-firstname
        /// </summary>
        public string Name => Lastname.Append(Firstname);

        /// <summary>
        /// The user with the loginname "ADMIN" is the system user that every Phonic database must have.
        /// The ADMIN user has no restrictions in the program and cann access everything
        /// </summary>
        public bool IsSysAdmin => Loginname == "ADMIN";

        /// <summary>
        /// The unencrypted version of the PasswordEnc property
        /// </summary>
        [Calculated]
        public string Password
        {
            get => PasswordEnc.Decrypt();
            set => PasswordEnc = value.Encrypt();
        }

        /// <summary>
        /// Internal list of permissions of the user. This list is not loaded from the database until it is accessed for the first time (lazy loading)
        /// </summary>
        [Calculated]
        private List<DbPermission> Permissions
        {
            get
            {
                if (_Permissions == null) 
                    _Permissions = DbPermission.GetAll(RowId);
                return _Permissions;
            }
        }
        private List<DbPermission> _Permissions;


        /// <summary>
        /// Load a user identified by loginname from the database. The method uses a temporary local database connection
        /// </summary>
        /// <param name="loginname">(required) requested login name</param>
        /// <returns>The user or null if not found</returns>
        public static DbUser GetByLoginname(string loginname)
        {
            using (var conn = DB.Connection)
            {
                var sql = "select rowid, * from Users where Loginname = @loginname";
                return conn.QuerySingleOrDefault<DbUser>(sql, new { loginname });
            }
        }

        /// <summary>
        /// Check if the user has one of the permission regardless of the permission's rights
        /// </summary>
        /// <param name="permissionIds">Variable number of permission ids</param>
        /// <returns>True if user is administrator or has one of the permissions</returns>
        public bool HasRight(params int[] permissionIds)
        {
            if (IsAdmin)
                return true;

            foreach (var permissionId in permissionIds)
            {
                if (Permissions.Any(x => x.PermissionId == permissionId))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Check if the user has the permission and at least one of the passed rights 
        /// </summary>
        /// <param name="permissionId">The requested permission</param>
        /// <param name="rights">Flags. User mus have on of these rights (see Rights enum in DbPermission.cs)</param>
        /// <returns>True if user is administrator or has the permissions with at least one of the specified rights</returns>
        public bool HasRight(int permissionId, Rights rights)
        {
            if (IsAdmin)
                return true;

            var permission = Permissions.SingleOrDefault(x => x.PermissionId == permissionId);
            if (permission == null)
                return false;

            if (rights.HasFlag(Rights.Edit) && permission.REdit)
                return true;
            if (rights.HasFlag(Rights.Delete) && permission.RDelete)
                return true;

            return false;
        }
    }
}