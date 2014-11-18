using System;
using System.Collections.Generic;
using System.Linq;
using FluentWebApi.Model;
using FluentWebApi.Routing;

namespace FluentWebApi.Configuration
{
    class ApiModelBinder<T> : IApiModelBinder<T> where T : class, IApiModel
    {
        private static readonly ApiModelBinder<T> _instance;
        private readonly IList<Route<T>> _routes = new List<Route<T>>();

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

        public Route<T> AddRoute(HttpVerb httpVerb)
        {
            return AddRoute(httpVerb, null);
        }

        public Route<T> AddRoute<TData>(HttpVerb httpVerb)
        {
            return AddRoute(httpVerb, typeof(TData));
        }

        private Route<T> AddRoute(HttpVerb httpVerb, Type parameter)
        {
            var route = new Route<T>(httpVerb, parameter);
            _routes.Add(route);

            return route;
        }

        public Route<T> GetRoute(HttpVerb httpVerb)
        {
            return _routes.FirstOrDefault(r => r.HttpVerb == httpVerb && r.KeyType == null);
        }

        public Route<T> GetRoute<TData>(HttpVerb httpVerb)
        {
            return _routes.FirstOrDefault(
                r => r.HttpVerb == httpVerb && r.KeyType == typeof(TData));
        }
    }
}