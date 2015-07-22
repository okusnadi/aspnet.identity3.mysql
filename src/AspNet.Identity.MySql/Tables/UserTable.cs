using AspNet.Identity.MySql;
using System;
using System.Collections.Generic;

namespace AspNet.Identity.MySql
{
    /// <summary>
    /// Class that represents the Users table in the MySQL Database
    /// </summary>
    public class UserTable<TUser,TKey>
        where TUser :IdentityUser<TKey> where TKey : IEquatable<TKey>
    {
        private MySqlDatabase _database;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserTable(MySqlDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Returns the user's name given a user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserName(string userId)
        {
            string commandText = "select Name from user where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };

            return _database.GetStrValue(commandText, parameters);
        }

        /// <summary>
        /// Returns a User ID given a user name
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public string GetUserId(string userName)
        {
            string commandText = "select id from Users where username = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", userName } };

            return _database.GetStrValue(commandText, parameters);
        }

        /// <summary>
        /// Returns an TUser given the user's id
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public TUser GetUserById(TKey userId)
        {
            TUser user = null;
            string commandText = "select * from user where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };

            var rows = _database.Query(commandText, parameters);
            if (rows != null && rows.Count == 1)
            {
                return MapRow(rows[0]);
            }

            return user;
        }

        /// <summary>
        /// Returns a list of TUser instances given a user name
        /// </summary>
        /// <param name="userName">User's name</param>
        /// <returns></returns>
        public List<TUser> GetUserByName(string userName)
        {
            List<TUser> users = new List<TUser>();
            string commandText = "select * from user where username = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", userName } };

            var rows = _database.Query(commandText, parameters);
            foreach(var row in rows)
            {                
                users.Add(MapRow(row));
            }

            return users;
        }

        public TUser GetUserByEmail(string email)
        {
            TUser user = null;
            string commandText = "select * from user where email = @email";
            var parameters = new Dictionary<string, object>() { { "@email", email } };
            var rows = _database.Query(commandText, parameters);
            if (rows != null && rows.Count == 1)
            {
                return MapRow(rows[0]);
            }
            return null;
        }

        /// <summary>
        /// Return the user's password hash
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public string GetPasswordHash(string userId)
        {
            string commandText = "select password_hash from user where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", userId);

            var passHash = _database.GetStrValue(commandText, parameters);
            if(string.IsNullOrEmpty(passHash))
            {
                return null;
            }

            return passHash;
        }

        /// <summary>
        /// Sets the user's password hash
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public int SetPasswordHash(string userId, string passwordHash)
        {
            string commandText = "update user set password_hash = @pwdHash where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@pwdHash", passwordHash);
            parameters.Add("@id", userId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Returns the user's security stamp
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetSecurityStamp(string userId)
        {
            string commandText = "select security_stamp from user where id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };
            var result = _database.GetStrValue(commandText, parameters);

            return result;
        }

        /// <summary>
        /// Inserts a new user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Insert(TUser user)
        {
            string commandText = @"insert into user (Id, username, password_hash, security_stamp,email,email_confirmed,phone_number,phone_number_confirmed, access_failed_count,lockout_enabled,lockout_end,two_factor_enabled)
                values (@id, @name, @pwdHash, @SecStamp,@email,@emailconfirmed,@phonenumber,@phonenumberconfirmed,@accesscount,@lockoutenabled,@lockoutenddate,@twofactorenabled)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", user.Id);
            parameters.Add("@name", user.UserName);            
            parameters.Add("@pwdHash", user.PasswordHash);
            parameters.Add("@SecStamp", user.SecurityStamp);
            parameters.Add("@email", user.Email);
            parameters.Add("@emailconfirmed", user.EmailConfirmed);
            parameters.Add("@phonenumber", user.PhoneNumber);
            parameters.Add("@phonenumberconfirmed", user.PhoneNumberConfirmed);
            parameters.Add("@accesscount", user.AccessFailedCount);
            parameters.Add("@lockoutenabled", user.LockoutEnabled);
            parameters.Add("@lockoutenddate", user.LockoutEnd);
            parameters.Add("@twofactorenabled", user.TwoFactorEnabled);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        private int Delete(TKey userId)
        {
            string commandText = "delete from user where Id = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userId", userId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Delete(TUser user)
        {
            return Delete(user.Id);
        }

        /// <summary>
        /// Updates a user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Update(TUser user)
        {
            string commandText = @"update user set username = @userName, password_hash = @pswHash, security_stamp = @secStamp, 
                email=@email, email_confirmed=@emailconfirmed, phone_number=@phonenumber, phone_number_confirmed=@phonenumberconfirmed,
                access_failed_count=@accesscount, lockout_enabled=@lockoutenabled, lockout_end=@lockoutenddate, two_factor_enabled=@twofactorenabled  
                where ie = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userName", user.UserName);
            parameters.Add("@pswHash", user.PasswordHash);
            parameters.Add("@secStamp", user.SecurityStamp);
            parameters.Add("@userId", user.Id);
            parameters.Add("@email", user.Email);
            parameters.Add("@emailconfirmed", user.EmailConfirmed);
            parameters.Add("@phonenumber", user.PhoneNumber);
            parameters.Add("@phonenumberconfirmed", user.PhoneNumberConfirmed);
            parameters.Add("@accesscount", user.AccessFailedCount);
            parameters.Add("@lockoutenabled", user.LockoutEnabled);
            parameters.Add("@lockoutenddate", user.LockoutEnd);
            parameters.Add("@twofactorenabled", user.TwoFactorEnabled);

            return _database.Execute(commandText, parameters);
        }

        private TUser MapRow(Dictionary<string,string> row)
        {
            TUser user = (TUser)Activator.CreateInstance(typeof(TUser));
            user.Id = (TKey)Convert.ChangeType(row["id"], typeof(TKey));
            user.UserName = row["username"];
            user.PasswordHash = string.IsNullOrEmpty(row["password_hash"]) ? null : row["password_hash"];
            user.SecurityStamp = string.IsNullOrEmpty(row["security_stamp"]) ? null : row["security_stamp"];
            user.Email = string.IsNullOrEmpty(row["email"]) ? null : row["email"];
            user.EmailConfirmed = row["email_confirmed"] == "1" ? true : false;
            user.PhoneNumber = string.IsNullOrEmpty(row["phone_number"]) ? null : row["phone_number"];
            user.PhoneNumberConfirmed = row["phone_number_confirmed"] == "1" ? true : false;
            user.LockoutEnabled = row["lockout_enabled"] == "1" ? true : false;
            user.LockoutEnd = string.IsNullOrEmpty(row["lockout_end"]) ? DateTime.Now : DateTime.Parse(row["lockout_end"]);
            user.TwoFactorEnabled = row["two_factor_enabled"] == "1" ? true : false;
            user.AccessFailedCount = string.IsNullOrEmpty(row["access_failed_count"]) ? 0 : int.Parse(row["access_failed_count"]);
            return user;
        }
    }
}
