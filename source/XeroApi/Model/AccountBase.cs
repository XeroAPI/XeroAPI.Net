using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XeroApi.Model
{
    public class AccountBase
    {
        public decimal? Outstanding { get; set;  }

        public decimal? Overdue { get; set; }

        public override string ToString()
        {
            return String.Format("Outstanding: {0:C}, Overdue: {1:C}.", Outstanding ?? 0, Overdue ?? 0);
        }
    }
}
