using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Demo.Core.Domain;
using Demo.Data.Mapping;
using Demo.Core.Domain.Account;

namespace Demo.Data.Mapping.Account
{
    public class RoleMap : DbEntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            this.ToTable("Roles");
            this.Property(bp => bp.Name).IsRequired().HasMaxLength(100);
            this.Property(bp => bp.Status).IsRequired().HasMaxLength(32);
            this.Property(bp => bp.Code).IsRequired().HasMaxLength(256);
            this.Property(bp => bp.Status).IsRequired().HasMaxLength(32);
            this.Property(bp => bp.Description).HasMaxLength(400);
        }
    }
}
