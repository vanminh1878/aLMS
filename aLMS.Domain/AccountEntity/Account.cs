using aLMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aLMS.Domain.AccountEntity
{
    public class Account: Entity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool? Status { get; set; }

    }
}
