using System.Net.Http;
using System.Web.Http.Routing;

namespace FluentWebApi.Routing
{
    /// <summary>
    /// Route class for routes defined using FluentWebApi
    /// </summary>
    internal class HttpRoute : System.Web.Http.Routing.HttpRoute
    {
        public HttpRoute()
            : this(routeTemplate: null, defaults: null, constraints: null, dataTokens: null, handler: null)
        {
        }

        public HttpRoute(string routeTemplate)
            : this(routeTemplate, defaults: null, constraints: null, dataTokens: null, handler: null)
        {
        }

        public HttpRoute(string routeTemplate, HttpRouteValueDictionary defaults)
            : this(routeTemplate, defaults, constraints: null, dataTokens: null, handler: null)
        {
        }

        public HttpRoute(string routeTemplate, HttpRouteValueDictionary defaults, HttpRouteValueDictionary constraints)
            : this(routeTemplate, defaults, constraints, dataTokens: null, handler: null)
        {
        }

        public HttpRoute(string routeTemplate, HttpRouteValueDictionary defaults, HttpRouteValueDictionary constraints, HttpRouteValueDictionary dataTokens)
            : this(routeTemplate, defaults, constraints, dataTokens, handler: null)
        {
        }

        public HttpRoute(string routeTemplate, HttpRouteValueDictionary defaults, HttpRouteValueDictionary constraints, HttpRouteValueDictionary dataTokens, HttpMessageHandler handler)
            : base(routeTemplate, defaults, constraints, dataTokens, handler)
        {
        }
    }
}