using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace XeroApi.Validation.Model
{
    enum Country
    {
        [Description("New Zealand")]
        NZ,
        [Description("United Kingdom")]
        UK,
        [Description("Australia")]
        AU,
        [Description("Worldwide")]
        WW,
        [Description("United States")]
        US
    }
}
