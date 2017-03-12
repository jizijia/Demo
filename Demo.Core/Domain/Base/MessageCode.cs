using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Base
{
    public class MessageCode
    {

        [MaxLength(32)]
        public string Code { get; set; }


        [MaxLength(400)]
        public string Message { get; set; }
    }
}
