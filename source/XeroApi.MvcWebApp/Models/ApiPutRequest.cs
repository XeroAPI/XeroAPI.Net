using System;

namespace Xero.ScreencastWeb.Models
{
    public class ApiPutRequest<TModel> : ApiGetRequest<TModel>
        where TModel : ModelBase, new()
    {

        public ModelListBase<TModel> Payload
        {
            get; 
            set;
        }

    }
}
