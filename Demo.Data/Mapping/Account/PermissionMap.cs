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
    public class PermissionMap : DbEntityTypeConfiguration<Permission>
    {
        public PermissionMap()
        {
            this.ToTable("Permissions");
            this.Property(bp => bp.Name).IsRequired().HasMaxLength(64);
            this.Property(bp => bp.Code).IsRequired().HasMaxLength(400);
            this.Property(bp => bp.IsDeleted).IsRequired().HasMaxLength(2);
            this.Property(bp => bp.IsPublic).IsRequired().HasMaxLength(2);
            this.Property(bp => bp.ModuleName).HasMaxLength(100);
            this.Property(bp => bp.PageName).HasMaxLength(100);
            this.Property(bp => bp.ElementName).HasMaxLength(100);
            this.Property(bp => bp.Url).HasMaxLength(800);
            this.Property(bp => bp.Comments).HasMaxLength(400);
            this.Property(bp => bp.Status).IsRequired().HasMaxLength(32);
            this.Property(bp => bp.Type).IsRequired().HasMaxLength(32);
            this.HasMany(c => c.Roles).WithMany(cr => cr.FuntionResources).Map(m =>
            {
                m.ToTable("RoleInPermission");
                m.MapLeftKey("RoleID");
                m.MapRightKey("PermissionID");
            });
        }
    }
}
