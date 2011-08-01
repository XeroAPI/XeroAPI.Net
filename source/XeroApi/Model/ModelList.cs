using System;
using System.Collections;
using System.Collections.Generic;

namespace XeroApi.Model
{
    public interface IModelList : IList
    {
    }

    public interface IModelList<TModel> : IModelList, IList<TModel>
        where TModel : ModelBase
    {
    }

    public abstract class ModelList<TModel> : List<TModel>, IModelList<TModel>
        where TModel : ModelBase
    {
    }
}
