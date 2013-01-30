using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace XeroApi.Validation.Model
{
    enum TaxType
    {
        [Description("Tax on Capital Purchases")]
        CAPEXSRINPUT,
        [Description("Tax on Capital Sales")]
        CAPEXSROUTPUT,
        [Description("Tax on Capital Purchases")]
        CAPEXINPUT,
        [Description("Tax on Capital Sales")]
        CAPEXOUTPUT,
        [Description("Tax on Capital Purchases")]
        CAPEXINPUT2,
        [Description("Tax on Capital Sales")]
        CAPEXOUTPUT2,
        [Description("Tax on Expenses")]
        SRINPUT,
        [Description("Tax on Income")]
        SROUTPUT,
        [Description("Input Taxed")]
        INPUTTAXED,
        [Description("Tax on Expenses")]
        INPUT,
        [Description("Tax on Income")]
        OUTPUT,
        [Description("Tax on Expenses")]
        INPUT2,
        [Description("Tax on Income")]
        OUTPUT2,
        [Description("Tax on Expenses")]
        RRINPUT,
        [Description("Tax on Income")]
        RROUTPUT,
        [Description("Exempt Expenses")]
        EXEMPTINPUT,
        [Description("Exempt Income")]
        EXEMPTOUTPUT,
        [Description("Tax Free Exports")]
        EXEMPTEXPORT,
        [Description("No VAT")]
        NONE,
        [Description("Tax on Imports")]
        GSTONIMPORTS,
        [Description("Tax on Capital Imports")]
        GSTONCAPIMPORTS,
        [Description("Zero Rated EC Expenses")]
        ECZRINPUT,
        [Description("Zero Rated EC Income")]
        ECZROUTPUT,
        [Description("Zero Rated Expenses")]
        ZERORATEDINPUT,
        [Description("Zero Rated Income")]
        ZERORATEDOUTPUT
    }
}
