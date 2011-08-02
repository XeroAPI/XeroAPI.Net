using System;

namespace XeroApi.Model
{
    [Flags]
    public enum LineAmountType
    {
        NoTax = 0,
        Inclusive,
        Exclusive,
    }
}