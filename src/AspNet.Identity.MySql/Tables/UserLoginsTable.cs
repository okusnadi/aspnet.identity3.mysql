using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;

namespace AspNet.Identity.MySql
{
    /// <summary>
    /// Class that represents the UserLogins table in the MySQL Database
    /// </summary>
    public class UserLoginsTable<TKey> where TKey: IEquatable<TKey>
    {
        private MySqlDatabase _database;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserLoginsTable(MySqlDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Deletes a login from a user in the UserLogins table
        /// </summary>
        /// <param name="user">User to have login deleted</param>
        /// <param name="login">Login to be deleted from user</param>
        /// <returns></returns>
        public int Delete(IdentityUser<TKey> user, UserLoginInfo login)
        {
            string commandText = "Delete from UserLogins where UserId = @userId and LoginProvider = @loginProvider and ProviderKey = @providerKey";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("UserId", user.Id);
            parameters.Add("loginProvider", login.LoginProvider);
            parameters.Add("providerKey", login.ProviderKey);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Deletes all Logins from a user in the UserLogins table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(string userId)
        {
            string commandText = "Delete from UserLogins where UserId = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("UserId", userId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Inserts a new login in the UserLogins table
        /// </summary>
        /// <param name="user">User to have new login added</param>
        /// <param name="login">Login to be added</param>
        /// <returns></returns>
        public int Insert(IdentityUser<TKey> user, UserLoginInfo login)
        {
            string commandText = "Insert into UserLogins (LoginProvider, ProviderKey, UserId) values (@loginProvider, @providerKey, @userId)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("loginProvider", login.LoginProvider);
            parameters.Add("providerKey", login.ProviderKey);
            parameters.Add("userId", user.Id);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Return a userId given a user's login
        /// </summary>
        /// <param name="userLogin">The user's login info</param>
        /// <returns></returns>
        public string FindUserIdByLogin(UserLoginInfo userLogin)
        {
            string commandText = "Select UserId from UserLogins where LoginProvider = @loginProvider and ProviderKey = @providerKey";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("loginProvider", userLogin.LoginProvider);
            parameters.Add("providerKey", userLogin.ProviderKey);

            return _database.GetStrValue(commandText, parameters);
        }

        /// <summary>
        /// Returns a list of user's logins
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<UserLoginInfo> FindByUserId(TKey userId)
        {
            List<UserLoginInfo> logins = new List<UserLoginInfo>();
            string commandText = "select * from userlogin where userId = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@userId", userId } };

            var rows = _database.Query(commandText, parameters);
            foreach (var row in rows)
            {
                var login = new UserLoginInfo(row["login_provider"], row["provider_key"],row["display_name"]);
                logins.Add(login);
            }

            return logins;
        }
    }
}
