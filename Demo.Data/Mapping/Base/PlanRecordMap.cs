using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.Domain.Base;

namespace Demo.Data.Mapping.Base
{
    public class PlanRecordMap : DbEntityTypeConfiguration<PlanRecord>
    {
        public PlanRecordMap()
        {
            this.ToTable("PlanRecords");
        }
    }
}
