using System;
using System.Collections.Generic;
using FluentWebApi.Model;
using FluentWebApi.Routing;

namespace FluentWebApi.Configuration {
    public class Route<T>
        where T : class, IApiModel {
        internal Route(HttpVerb httpVerb)
        {
            HttpVerb = httpVerb;
        }

        internal HttpVerb HttpVerb { get; private set; }
        internal Func<IEnumerable<T>> DataRetriever { get; set; } 

        internal IEnumerable<T> GetData()
        {
            return DataRetriever();
        }
    }

    public class Route<T, TKey>
        where T : class, IApiModel
    {
        internal Route(HttpVerb httpVerb)
        {
            HttpVerb = httpVerb;
        }

        internal HttpVerb HttpVerb { get; private set; }

        internal Func<TKey, T> DataRetriever { get; set; }

        internal T GetData(TKey id)
        {
            return DataRetriever(id);
        }
    }
}