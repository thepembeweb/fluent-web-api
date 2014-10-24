using System.Collections.Generic;

namespace WebApi.Dynamic.Model
{
    //public interface IDataProvider
    //{
    //    IEnumerable<object> Get();

    //    object GetByKey(object key);
    //}

    public interface IDataProvider<out T, in TKey> //: IDataProvider 
        where T : class, IApiModel<TKey>
    {
        IEnumerable<T> Get();
        
        T GetByKey(TKey key);
    }
}