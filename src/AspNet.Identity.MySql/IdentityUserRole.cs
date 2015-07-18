using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet.Identity.MySql
{
    public class IdentityUserRole<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// UserId for the user that is in the role
        /// 
        /// </summary>
        public virtual TKey UserId { get; set; }

        /// <summary>
        /// RoleId for the role
        /// 
        /// </summary>
        public virtual TKey RoleId { get; set; }
    }
}
