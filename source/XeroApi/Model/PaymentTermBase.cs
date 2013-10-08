using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XeroApi.Model
{
    public class PaymentTermBase
    {
        public int Day { get; set; }
        public string Type { get; set; }
        public override string ToString()
        {
            string suffix;

            switch (Day)
            {
                case 1:
                case 21:
                case 31:
                    suffix = "st";
                    break;
                case 2:
                case 22:
                    suffix = "nd";
                    break;
                case 3:
                case 23:
                    suffix = "rd";
                    break;
                default:
                    suffix = "th";
                    break;
            }

            return String.Format("{0}{1} {2}", Day, suffix, Type);
        }
    }
}
