using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet.Identity.MySql
{
    /// <summary>
    /// EntityType that represents one specific role claim
    /// 
    /// </summary>
    /// <typeparam name="TKey"/>
    public class IdentityRoleClaim<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Primary key
        /// 
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// User Id for the role this claim belongs to
        /// 
        /// </summary>
        public virtual TKey RoleId { get; set; }

        /// <summary>
        /// Claim type
        /// 
        /// </summary>
        public virtual string ClaimType { get; set; }

        /// <summary>
        /// Claim value
        /// 
        /// </summary>
        public virtual string ClaimValue { get; set; }
    }
}
