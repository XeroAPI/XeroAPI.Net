using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XeroApi.Validation.Helpers
{
    static class DecimalHelper
    {
        const decimal tolerance = 0.005m;

        public static bool NearlyEqualTo(this decimal d, decimal other, decimal tol = tolerance)
        {
            var diff = Math.Abs(d - other);

            if (d < other)
                return (diff <= tol);
            else
                return (diff < tol);
        }
    }
}
