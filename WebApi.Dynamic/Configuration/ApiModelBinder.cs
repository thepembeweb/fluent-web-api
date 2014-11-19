using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;
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

        public Route<T> AddRoute(HttpVerb httpVerb, IDictionary<string, string> routeDictionary = null)
        {
            return AddRoute(httpVerb, null, routeDictionary);
        }

        public Route<T, TData> AddRoute<TData>(HttpVerb httpVerb, IDictionary<string, string> routeDictionary = null)
        {
            var route = new Route<T, TData>(httpVerb, routeDictionary);
            _routes.Add(route);

            return route;
        }

        private Route<T> AddRoute(HttpVerb httpVerb, Type parameter, IDictionary<string, string> routeDictionary)
        {
            var route = new Route<T>(httpVerb, parameter, routeDictionary);
            _routes.Add(route);

            return route;
        }

        public Route<T> GetRoute(HttpVerb httpVerb, HttpRequestMessage request)
        {
            var route = GetRouteByName(request);

            if (route != null)
            {
                return route;
            }
            
            return _routes.FirstOrDefault(r => r.HttpVerb == httpVerb && r.KeyType == null);
        }

        public Route<T> GetRoute<TData>(HttpVerb httpVerb, HttpRequestMessage request)
        {
            var route = GetRouteByName(request);

            if (route != null)
            {
                return route;
            }

            return _routes.FirstOrDefault(
                r => r.HttpVerb == httpVerb && r.KeyType == typeof(TData));
        }

        private Route<T> GetRouteByName(HttpRequestMessage request)
        {
            var routeData = request.GetRouteData();
            if (routeData != null)
            {
                var routeName = routeData.Route.GetRouteName();
                return _routes.FirstOrDefault(r => string.Equals(r.Name, routeName, StringComparison.OrdinalIgnoreCase));
            }

            return null;
        }
    }
}