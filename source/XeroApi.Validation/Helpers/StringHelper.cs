using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XeroApi.Validation.Helpers
{
    static class StringHelper
    {
        public static bool IsNullOrWhiteSpace(this string str)
        {
            if (str != null)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (!char.IsWhiteSpace(str[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
