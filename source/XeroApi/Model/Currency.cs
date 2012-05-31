using System;

namespace XeroApi.Model
{
    public class Currency : EndpointModelBase
    {
        public string Code { get; set; }

        public string Description { get; set; }
    }

    public class Currencies : ModelList<Currency>
    {
    }
}