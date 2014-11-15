using System.Collections.Generic;
using FluentWebApi.Model;

namespace FluentWebApi.Configuration
{
    internal class Storage<T> where T : class, IApiModel
    {
         public static readonly IList<Route<T>> Routes = new List<Route<T>>(); 
    }

    internal class Storage<T, TData> where T : class, IApiModel
    {
        public static readonly IList<Route<T, TData>> Routes = new List<Route<T, TData>>();
    }
}