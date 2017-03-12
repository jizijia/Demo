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
    public class UserMap : DbEntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.ToTable("Users"); 
            this.Property(bp => bp.UserName).IsRequired().HasMaxLength(20);
            this.Property(bp => bp.Password).IsRequired().HasMaxLength(128);
            this.Property(bp => bp.ValidCode).IsRequired().HasMaxLength(128);
            this.Property(bp => bp.IsDeleted).IsRequired().HasMaxLength(2);
            this.Property(bp => bp.IsOnline).HasMaxLength(2);
            this.Property(bp => bp.MobileNumber).HasMaxLength(32);
            this.Property(bp => bp.NickName).HasMaxLength(20);
            this.Property(bp => bp.RegisterIP).IsRequired().HasMaxLength(32);
            this.Property(bp => bp.Status).IsRequired().HasMaxLength(32);
            this.Property(bp => bp.Comments).HasMaxLength(400);
            this.HasMany(u => u.Roles).WithMany(r => r.Users).Map(m =>
            {
                m.ToTable("UserInRole");
                m.MapLeftKey("UserID");
                m.MapRightKey("RoleID");
            });

            this.Ignore(u => u.Permissions);
        }
    }
}
