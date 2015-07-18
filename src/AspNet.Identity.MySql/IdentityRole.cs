using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet.Identity.MySql
{
    public class IdentityRole<TKey> where TKey : IEquatable<TKey>
    {
        private string roleName;
        private TKey roleId;

        /// <summary>
        /// Navigation property for users in the role
        /// 
        /// </summary>
        public virtual ICollection<IdentityUserRole<TKey>> Users { get; } = (ICollection<IdentityUserRole<TKey>>)new List<IdentityUserRole<TKey>>();

        /// <summary>
        /// Navigation property for claims in the role
        /// 
        /// </summary>
        public virtual ICollection<IdentityRoleClaim<TKey>> Claims { get; } = (ICollection<IdentityRoleClaim<TKey>>)new List<IdentityRoleClaim<TKey>>();

        /// <summary>
        /// A random value that should change whenever a role is persisted to the store
        /// 
        /// </summary>
        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Role id
        /// 
        /// </summary>
        public virtual TKey Id { get; set; }

        /// <summary>
        /// Role name
        /// 
        /// </summary>
        public virtual string Name { get; set; }

        public virtual string NormalizedName { get; set; }

        public IdentityRole()
        {
        }

        /// <summary>
        /// Constructor
        /// 
        /// </summary>
        /// <param name="roleName"/>
        public IdentityRole(string roleName)
          : this()
        {
            this.Name = roleName;
        }

        public IdentityRole(string roleName, TKey roleId)
        {
            this.roleName = roleName;
            this.roleId = roleId;
        }
    }
}
