using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Base
{
    public class PlanRecord : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Code { get; set; }
        [MaxLength(100)]
        public string Data { get; set; }
        public DateTime? BeginAt { get; set; }
        public DateTime? EndAt { get; set; }
        public DateTime? LastEndAt { get; set; }
        public DateTime? NextBeginAt { get; set; }
        [MaxLength(50)]
        public string Status { get; set; }
        [MaxLength(500)]
        public string Log { get; set; }
    }
}
