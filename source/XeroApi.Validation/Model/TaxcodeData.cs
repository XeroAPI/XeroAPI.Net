using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XeroApi.Validation.Model
{
    class TaxCodeData
    {
        public TaxType TaxType { get; set; }
        public Country Country { get; set; }
        public double Rate { get; set; }

        public static readonly TaxCodeData[] TaxCodes = new TaxCodeData[] 
        {
            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.CAPEXINPUT, Rate = 17.5d},
            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.CAPEXINPUT2, Rate = 20},
            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.CAPEXOUTPUT, Rate = 17.5d},
            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.CAPEXOUTPUT2, Rate = 20},
            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.CAPEXSRINPUT, Rate = 15},
            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.CAPEXSROUTPUT, Rate = 15},

            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.INPUT, Rate = 17.5d},
            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.OUTPUT, Rate = 17.5d},
            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.INPUT2, Rate = 20},
            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.OUTPUT2, Rate = 20},
            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.SRINPUT, Rate = 15},
            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.SROUTPUT, Rate = 15},

            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.RRINPUT, Rate = 5},
            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.RROUTPUT, Rate = 5},

            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.ZERORATEDINPUT, Rate = 0},
            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.ZERORATEDOUTPUT, Rate = 0},
            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.ECZRINPUT, Rate = 0},
            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.ECZROUTPUT, Rate = 0},
            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.EXEMPTINPUT, Rate = 0},
            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.EXEMPTOUTPUT, Rate = 0},
            new TaxCodeData(){Country = Country.UK, TaxType = TaxType.NONE, Rate = 0},
        };
    }
}
