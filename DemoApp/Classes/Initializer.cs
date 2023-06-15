using Dapper;
using DemoApp.Models;
using System;
using System.Data;
using WisejLib;

namespace DemoApp.Classes
{
    /// <summary>
    /// This class is used to create default data in the database. The ADMIN super user is created 
    /// in any case. Further creation of users can be added so that there is some beef in the database 
    /// and you can test things at runtime. 
    /// </summary>
    public static class Initializer
    {
        /// <summary>
        /// The entry point of the Initializer class. Call Initializer.Execute() to run the 
        /// initializations
        /// </summary>
        public static void Execute()
        {
            using (var conn = DB.Connection)
            using (var tx = conn.BeginTransaction())
            {
                CreateUsers(tx);
                tx.Commit();
            }
        }

        /// <summary>
        /// Create all default users in the database. Called by Execute.
        /// </summary>
        private static void CreateUsers(IDbTransaction tx)
        {
            DbUser user;

            if (!ExistsUser(tx, "ADMIN"))
            {
                user = new DbUser
                {
                    Loginname = "ADMIN",
                    Salutation = Salutations.SALUTATION_Male,
                    Lastname = "Administrator",
                    IsActive = true,
                    IsAdmin = true,
                    Hired = new DateTime(2000, 1, 1),
                };
                user.SaveChanges(tx);
            }

            if (!ExistsUser(tx, "JOE"))
            {
                user = new DbUser
                {
                    Loginname = "JOE",
                    Salutation = Salutations.SALUTATION_Male,
                    Firstname = "Joachim",
                    Lastname = "Meyer",
                    JobTitle = "CVC (Chief Video Creator)",
                    IsActive = true,
                    IsAdmin = true,
                    Hired = new DateTime(2000, 1, 1),
                };
                user.SaveChanges(tx);
            }
        }

        /// <summary>
        /// Check is a user exists already
        /// </summary>
        /// <param name="tx">A Sqlite Transaction</param>
        /// <param name="loginname">Look for a user identified by this login name</param>
        /// <returns>Returns true if the loginname exists already</returns>
        private static bool ExistsUser(IDbTransaction tx, string loginname)
        {
            return tx.Connection.QuerySingleOrDefault<int?>("select 1 from users where loginname = @loginname", new { loginname }) != null;
        }
    }
}
