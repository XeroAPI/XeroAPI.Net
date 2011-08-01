using System.Collections.Generic;

namespace Xero.ScreencastWeb.Models
{
    public class ModelListBase<TModel> : List<TModel>
        where TModel : ModelBase
    {
        public string ToXml()
        {
            return ModelSerializer.Serialize(this);
        }
    }
}