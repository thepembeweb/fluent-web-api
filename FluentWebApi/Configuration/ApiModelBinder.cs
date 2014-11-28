using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;
using FluentWebApi.Model;
using FluentWebApi.Routing;

namespace FluentWebApi.Configuration
{
    /// <summary>
    /// This class holds the Fluent Web API configuration for any given type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">A class that implements the <see cref="IApiModel"/> interface.</typeparam>
    internal class ApiModelBinder<T> where T : class, IApiModel
    {
        // ApiModelBinder<T> is a singleton
        private static readonly ApiModelBinder<T> _instance;

        // Store all routes that have been defined for this model class.
        private readonly IList<Route<T>> _routes = new List<Route<T>>();

        // And remember which HTTP verbs are allowed.
        private readonly HashSet<HttpVerb> _allowedVerbs = new HashSet<HttpVerb>();

        static ApiModelBinder()
        {
            _instance = new ApiModelBinder<T>(); //Create a default instance per bound IApiModel
        }

        private ApiModelBinder()
        {
            
        }

        /// <summary>
        /// Returns the singleton instance of <see cref="ApiModelBinder{T}"/>.
        /// </summary>
        public static ApiModelBinder<T> Instance
        {
            get
            {
                return _instance;
            }
        }
        
        /// <summary>
        /// Returns an enumeration of all allowed HTTP verbs for the model bound to this <see cref="ApiModelBinder{T}"/>.
        /// </summary>
        public IEnumerable<HttpVerb> AllowedVerbs
        {
            get
            {
                return _allowedVerbs.AsEnumerable();
            }
        }

        /// <summary>
        /// Retrieves or creates a <see cref="Route{T}"/> that matches the settings in the <paramref name="routeDictionary"/>.
        /// </summary>
        /// <param name="routeDictionary">Optional. When defined, the <see cref="IDictionary{TKey, TValue}"/> should at least contain an entry for RouteName and RouteTemplate.</param>
        public Route<T> GetOrCreateRoute(IDictionary<string, string> routeDictionary = null)
        {
            return GetOrCreateRoute(null, routeDictionary);
        }

        /// <summary>
        /// Retrieves or creates a <see cref="Route{T, TData}"/> that matches the settings in the <paramref name="routeDictionary"/>.
        /// </summary>
        /// <param name="routeDictionary">Optional. When defined, the <see cref="IDictionary{TKey, TValue}"/> should at least contain an entry for RouteName and RouteTemplate.</param>
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

        /// <summary>
        /// Retrieves the <see cref="Route{T}"/> that matches the given <see cref="HttpRequestMessage"/>.
        /// </summary>
        public Route<T> GetRoute(HttpRequestMessage request)
        {
            var routeName = GetRouteNameFromRequest(request);
            return GetRoute(routeName);
        }

        /// <summary>
        /// Retrieves the <see cref="Route{T}"/> that matches the given <see cref="HttpRequestMessage"/>.
        /// </summary>
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

        /// <summary>
        /// Marks the given <paramref name="verb"/> as allowed.
        /// </summary>
        /// <param name="verb"></param>
        public void AllowVerb(HttpVerb verb)
        {
            _allowedVerbs.Add(verb);
        }
    }
}