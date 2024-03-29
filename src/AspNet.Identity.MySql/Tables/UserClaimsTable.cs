﻿using System.Collections.Generic;
using System.Security.Claims;
using System;

namespace AspNet.Identity.MySql
{
    /// <summary>
    /// Class that represents the UserClaims table in the MySQL Database
    /// </summary>
    public class UserClaimsTable<TKey> where TKey : IEquatable<TKey>
    {
        private MySqlDatabase _database;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserClaimsTable(MySqlDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Returns a ClaimsIdentity instance given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public ClaimsIdentity FindByUserId(TKey userId)
        {
            ClaimsIdentity claims = new ClaimsIdentity();
            string commandText = "select * from userclaim where user_id = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@UserId", userId } };

            var rows = _database.Query(commandText, parameters);
            foreach (var row in rows)
            {
                Claim claim = new Claim(row["claim_type"], row["claim_value"]);
                claims.AddClaim(claim);
            }

            return claims;
        }

        /// <summary>
        /// Deletes all claims from a user given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(string userId)
        {
            string commandText = "delete from userclaim where user_id = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("userId", userId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Inserts a new claim in UserClaims table
        /// </summary>
        /// <param name="userClaim">User's claim to be added</param>
        /// <param name="userId">User's id</param>
        /// <returns></returns>
        public int Insert(Claim userClaim, string userId)
        {
            string commandText = "insert into userclaim (claim_value, claim_type, user_id) values (@value, @type, @userId)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("value", userClaim.Value);
            parameters.Add("type", userClaim.Type);
            parameters.Add("userId", userId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Deletes a claim from a user 
        /// </summary>
        /// <param name="user">The user to have a claim deleted</param>
        /// <param name="claim">A claim to be deleted from user</param>
        /// <returns></returns>
        public int Delete(IdentityUser<TKey> user, Claim claim)
        {
            string commandText = "delete from userclaim where user_id = @userId and claim_value = @value and claim_type = @type";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("userId", user.Id);
            parameters.Add("value", claim.Value);
            parameters.Add("type", claim.Type);

            return _database.Execute(commandText, parameters);
        }
    }
}
