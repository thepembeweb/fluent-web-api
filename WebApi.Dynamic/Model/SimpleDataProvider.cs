using System;
using System.Collections.Generic;

namespace FluentWebApi.Model
{
    class ListDataProvider<T, TKey> : IDataProvider<T, TKey> 
        where T : class, IApiModel<TKey>
    {
        private readonly Func<IEnumerable<T>> _func;
        public ListDataProvider(Func<IEnumerable<T>> func)
        {
            _func = func;
        }

        public IEnumerable<T> Get()
        {
            return _func();
        }

        public T GetByKey(TKey key)
        {
            throw new NotImplementedException();
        }
    }

    class SingleDataProvider<T, TKey> : IDataProvider<T, TKey>
        where T : class, IApiModel<TKey>
    {
        private readonly Func<TKey, T> _func;
        public SingleDataProvider(Func<TKey, T> func)
        {
            _func = func;
        }

        public IEnumerable<T> Get()
        {
            throw new NotImplementedException();
        }

        public T GetByKey(TKey key)
        {
            return _func(key);
        }
    }
}