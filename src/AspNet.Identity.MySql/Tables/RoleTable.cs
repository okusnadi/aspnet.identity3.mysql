using AspNet.Identity.MySql;
using System;
using System.Collections.Generic;

namespace AspNet.Identity.MySql
{
    /// <summary>
    /// Class that represents the Role table in the MySQL Database
    /// </summary>
    public class RoleTable <TKey> where TKey:IEquatable<TKey>
    {
        private MySqlDatabase _database;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public RoleTable(MySqlDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Deltes a role from the Roles table
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns></returns>
        public int Delete(string roleId)
        {
            string commandText = "Delete from Roles where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", roleId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Inserts a new Role in the Roles table
        /// </summary>
        /// <param name="roleName">The role's name</param>
        /// <returns></returns>
        public int Insert(IdentityRole<TKey> role)
        {
            string commandText = "Insert into Roles (Id, Name) values (@id, @name)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@name", role.Name);
            parameters.Add("@id", role.Id);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Returns a role name given the roleId
        /// </summary>
        /// <param name="roleId">The role Id</param>
        /// <returns>Role name</returns>
        public string GetRoleName(TKey roleId)
        {
            string commandText = "Select Name from Roles where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", roleId);

            return _database.GetStrValue(commandText, parameters);
        }

        /// <summary>
        /// Returns the role Id given a role name
        /// </summary>
        /// <param name="roleName">Role's name</param>
        /// <returns>Role's Id</returns>
        public TKey GetRoleId(string roleName)
        {
            TKey roleId= default(TKey);
            string commandText = "Select Id from Roles where Name = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", roleName } };

            var result = _database.QueryValue(commandText, parameters);
            if (result != null)
            {
                return (TKey)Convert.ChangeType(result, typeof(TKey));
            }

            return roleId;
        }

        /// <summary>
        /// Gets the IdentityRole given the role Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public IdentityRole<TKey> GetRoleById(TKey roleId)
        {
            var roleName = GetRoleName(roleId);
            IdentityRole<TKey> role = null;

            if(roleName != null)
            {
                role = new IdentityRole<TKey>(roleName, roleId);
            }

            return role;

        }

        /// <summary>
        /// Gets the IdentityRole given the role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public IdentityRole<TKey> GetRoleByName(string roleName)
        {
            var roleId = GetRoleId(roleName);
            IdentityRole<TKey> role = null;

            if (roleId != null)
            {
                role = new IdentityRole<TKey>(roleName, roleId);
            }

            return role;
        }

        public int Update(IdentityRole<TKey> role)
        {
            string commandText = "Update Roles set Name = @name where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", role.Id);

            return _database.Execute(commandText, parameters);
        }
    }
}
