using System;
using System.Collections.Generic;
using FluentWebApi.Model;
using FluentWebApi.Routing;

namespace FluentWebApi.Configuration
{
    class ApiModelBinder<T> : IApiModelBinder<T> where T : class, IApiModel
    {
        //private IDataProvider<T, TKey> _customDataProvider;
        //private IDataProvider<T, TKey> _returnsT;
        //private IDataProvider<T, TKey> _returnsIEnumerableOfT;

        private static readonly ApiModelBinder<T> _instance;

        static ApiModelBinder()
        {
            _instance = new ApiModelBinder<T>(); //Create a default instance per bound IApiModel
        }

        private ApiModelBinder()
        {
            
        }

        public static ApiModelBinder<T> Instance
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
                return false; //return _customDataProvider != null;
            }
        }

        public bool HasListDataProvider
        {
            get
            {
                return false; //return _customDataProvider != null || _returnsIEnumerableOfT != null;
            }
        }

        public bool HasSingleDataProvider
        {
            get
            {
                return false; //return _customDataProvider != null || _returnsT != null;
            }
        }

        public void AddDataProvider<TKey>(Func<TKey, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            //_returnsT = new SingleDataProvider<T, TKey>(func);
        }

        public void AddDataProvider(Func<IEnumerable<T>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            //_returnsIEnumerableOfT = new ListDataProvider<T, TKey>(func);
        }

        //public void SetCustomDataProvider<TKey>(IDataProvider<T, TKey> dataProvider)
        //{
        //    if (dataProvider == null)
        //    {
        //        throw new ArgumentNullException("dataProvider");
        //    }

        //    //_customDataProvider = dataProvider;
        //}

        public IEnumerable<T> GetList()
        {
            //if (HasCustomDataProvider)
            //{
            //    return _customDataProvider.Get();
            //}

            //if (HasListDataProvider)
            //{
            //    return _returnsIEnumerableOfT.Get();
            //}

            throw new InvalidOperationException();
        }

        public T Get<TKey>(TKey id)
        {
            //if (HasCustomDataProvider)
            //{
            //    return _customDataProvider.GetByKey(id);
            //}

            //if (HasSingleDataProvider)
            //{
            //    return _returnsT.GetByKey(id);
            //}

            throw new InvalidOperationException();
        }

        public void AddRoute(HttpVerb httpVerb)
        {
            
        }
    }
}