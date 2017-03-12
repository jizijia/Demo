using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.Domain.Base;

namespace Demo.Data.Mapping.Base
{
    public class PlanLogMap : DbEntityTypeConfiguration<PlanLog>
    {
        public PlanLogMap()
        {
            this.ToTable("PlanLogs");
        }
    }
}
