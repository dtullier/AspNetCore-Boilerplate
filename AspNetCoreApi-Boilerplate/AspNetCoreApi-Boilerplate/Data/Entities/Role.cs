using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace AspNetCoreApi_Boilerplate.Data.Entities
{
    public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<UserRole> Users { get; set; } = new HashSet<UserRole>();
    }

    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
        }
    }
}
