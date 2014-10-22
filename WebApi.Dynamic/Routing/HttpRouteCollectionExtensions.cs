using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace WebApi.Dynamic.Routing
{
    public static class HttpRouteCollectionExtensions
    {
        /// <summary>
        /// Maps the specified route template.
        /// </summary>
        /// <param name="routes">A collection of routes for the application.</param>
        /// <param name="name">The name of the route to map.</param>
        /// <param name="routeTemplate">The route template for the route.</param>
        /// <returns>A reference to the mapped route.</returns>
        public static IHttpRoute MapDynamicRoute(this HttpRouteCollection routes, string name, string routeTemplate)
        {
            return MapDynamicRoute(routes, name, routeTemplate, defaults: null, constraints: null, handler: null);
        }

        /// <summary>
        /// Maps the specified route template and sets default constraints.
        /// </summary>
        /// <param name="routes">A collection of routes for the application.</param>
        /// <param name="name">The name of the route to map.</param>
        /// <param name="routeTemplate">The route template for the route.</param>
        /// <param name="defaults">An object that contains default route values.</param>
        /// <returns>A reference to the mapped route.</returns>
        public static IHttpRoute MapDynamicRoute(this HttpRouteCollection routes, string name, string routeTemplate, object defaults)
        {
            return MapDynamicRoute(routes, name, routeTemplate, defaults, constraints: null, handler: null);
        }

         /// <summary>
         /// Maps the specified route template and sets default route values and constraints.
         /// </summary>
         /// <param name="routes">A collection of routes for the application.</param>
         /// <param name="name">The name of the route to map.</param>
         /// <param name="routeTemplate">The route template for the route.</param>
         /// <param name="defaults">An object that contains default route values.</param>
         /// <param name="constraints">A set of expressions that specify values for <paramref name="routeTemplate"/>.</param>
         /// <returns>A reference to the mapped route.</returns>
        public static IHttpRoute MapDynamicRoute(this HttpRouteCollection routes, string name, string routeTemplate, object defaults, object constraints)
         {
             return MapDynamicRoute(routes, name, routeTemplate, defaults, constraints, handler: null);
         }

         /// <summary>
         /// Maps the specified route template and sets default route values, constraints, and end-point message handler.
         /// </summary>
         /// <param name="routes">A collection of routes for the application.</param>
         /// <param name="name">The name of the route to map.</param>
         /// <param name="routeTemplate">The route template for the route.</param>
         /// <param name="defaults">An object that contains default route values.</param>
         /// <param name="constraints">A set of expressions that specify values for <paramref name="routeTemplate"/>.</param>
         /// <param name="handler">The handler to which the request will be dispatched.</param>
         /// <returns>A reference to the mapped route.</returns>
        public static IHttpRoute MapDynamicRoute(this HttpRouteCollection routes, string name, string routeTemplate, object defaults, object constraints, HttpMessageHandler handler)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }

            HttpRouteValueDictionary defaultsDictionary = new HttpRouteValueDictionary(defaults);
            HttpRouteValueDictionary constraintsDictionary = new HttpRouteValueDictionary(constraints);
            IHttpRoute route = routes.CreateRoute(routeTemplate, defaultsDictionary, constraintsDictionary, dataTokens: null, handler: handler);
            routes.Add(name, route);
            return route;
        }
    }
}