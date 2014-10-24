using System;
using System.Collections.Generic;
using WebApi.Dynamic.Model;

namespace WebApi.Dynamic.Configuration
{
    class ApiModelBinder<T, TKey> : IApiModelBinder<T, TKey> where T : class, IApiModel<TKey>
    {
        private IDataProvider<T, TKey> _customDataProvider;
        private IDataProvider<T, TKey> _returnsT;
        private IDataProvider<T, TKey> _returnsIEnumerableOfT;

        private static readonly ApiModelBinder<T, TKey> _instance;

        static ApiModelBinder()
        {
            _instance = new ApiModelBinder<T, TKey>(); //Create a default instance per bound IApiModel
        }

        private ApiModelBinder()
        {
            
        }

        public static ApiModelBinder<T, TKey> Instance
        {
            get
            {
                return _instance;
            }
        }

        public bool HasCustomDataProvider
        {
            get
            {
                return _customDataProvider != null;
            }
        }

        public bool HasListDataProvider
        {
            get
            {
                return _customDataProvider != null || _returnsIEnumerableOfT != null;
            }
        }

        public bool HasSingleDataProvider
        {
            get
            {
                return _customDataProvider != null || _returnsT != null;
            }
        }

        public void AddDataProvider(Func<TKey, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            _returnsT = new SingleDataProvider<T, TKey>(func);
        }

        public void AddDataProvider(Func<IEnumerable<T>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            _returnsIEnumerableOfT = new ListDataProvider<T, TKey>(func);
        }

        public void SetCustomDataProvider(IDataProvider<T, TKey> dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }

            _customDataProvider = dataProvider;
        }

        public IEnumerable<T> GetList()
        {
            if (HasCustomDataProvider)
            {
                return _customDataProvider.Get();
            }

            if (HasListDataProvider)
            {
                return _returnsIEnumerableOfT.Get();
            }

            throw new InvalidOperationException();
        }

        public T Get(TKey id)
        {
            if (HasCustomDataProvider)
            {
                return _customDataProvider.GetByKey(id);
            }

            if (HasSingleDataProvider)
            {
                return _returnsT.GetByKey(id);
            }

            throw new InvalidOperationException();
        }
    }
}