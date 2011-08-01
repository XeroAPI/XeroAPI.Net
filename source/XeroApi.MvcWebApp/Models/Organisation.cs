using System;

namespace Xero.ScreencastWeb.Models
{
    [Serializable]
    public class Organisation : ModelBase
    {
        public override string ApiEndpointName
        {
            get { return "Organisation"; }
        }

        public string Name { get; set; }
    }

    [Serializable]
    public class Organisations : ModelListBase<Organisation>
    {
        
    }
}
