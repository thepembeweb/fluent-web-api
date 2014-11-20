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
        private readonly HashSet<HttpVerb> _enabledVerbs = new HashSet<HttpVerb>();

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
        
        public IEnumerable<HttpVerb> EnabledVerbs
        {
            get
            {
                return _enabledVerbs.AsEnumerable();
            }
        }

        public Route<T> GetOrCreateRoute(IDictionary<string, string> routeDictionary = null)
        {
            return GetOrCreateRoute(null, routeDictionary);
        }

        public Route<T, TData> GetOrCreateRoute<TData>(IDictionary<string, string> routeDictionary = null)
        {
            var route = GetRoute<TData>(routeDictionary) as Route<T, TData>;
            if (route != null && routeDictionary != null)
            {
                // Check if the name of the route we found matches the one in the dictionary. If they do not match, we need to create
                // a new route!
                if (!string.Equals(route.Name, routeDictionary.GetRouteName(), StringComparison.OrdinalIgnoreCase))
                {
                    route = null;
                }
            }

            if (route == null)
            {
                route = new Route<T, TData>(routeDictionary);
                _routes.Add(route);
            }

            return route;
        }

        private Route<T> GetOrCreateRoute(Type parameter, IDictionary<string, string> routeDictionary)
        {
            Route<T> route = GetRoute(routeDictionary);
            if (route == null)
            {
                route = new Route<T>(parameter, routeDictionary);
                _routes.Add(route);
            }

            return route;
        }

        public Route<T> GetRoute(HttpRequestMessage request)
        {
            var routeName = GetRouteNameFromRequest(request);
            return GetRoute(routeName);
        }

        public Route<T> GetRoute<TData>(HttpRequestMessage request)
        {
            var routeName = GetRouteNameFromRequest(request);
            return GetRoute(routeName, typeof(TData));
        }

        private Route<T> GetRoute(IDictionary<string, string> routeDictionary)
        {
            string routeName = null;
            if (routeDictionary != null)
            {
                routeName = routeDictionary.GetRouteName();
            }

            return GetRoute(routeName);
        }

        private Route<T> GetRoute<TData>(IDictionary<string, string> routeDictionary)
        {
            string routeName = null;
            if (routeDictionary != null)
            {
                routeName = routeDictionary.GetRouteName();
            }

            return GetRoute(routeName, typeof(TData));
        }

        internal Route<T> GetRoute(string routeName, Type keyType = null)
        {
            Route<T> route = null;
            if (routeName != null)
            {
                route = _routes.FirstOrDefault(r => string.Equals(r.Name, routeName, StringComparison.OrdinalIgnoreCase));
            }

            if (route != null)
            {
                return route;
            }

            return _routes.FirstOrDefault(r => r.KeyType == keyType);
        }

        private string GetRouteNameFromRequest(HttpRequestMessage request)
        {
            var routeData = request.GetRouteData();
            if (routeData != null)
            {
                return routeData.Route.GetRouteName();
            }

            return null;
        }

        public void EnableVerb(HttpVerb verb)
        {
            _enabledVerbs.Add(verb);
        }
    }
}