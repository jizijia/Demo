using System.Data.Entity.ModelConfiguration;
using Demo.Core.Domain.Account;
using Demo.Data.Mapping;

namespace Demo.Data.Mapping.Account
{
    public partial class UserActivityLogMap : DbEntityTypeConfiguration<UserActivityLog>
    {
        public UserActivityLogMap()
        {
            this.ToTable("UserActivityLog");
            this.Property(al => al.Comment);
            this.Property(al => al.ActivityType).HasMaxLength(50);
            this.Property(al => al.IpAddress).HasMaxLength(32);
            this.Property(al => al.PageUrl).HasMaxLength(400);
            this.Property(al => al.ReferrerUrl).HasMaxLength(400);
        }
    }
}
