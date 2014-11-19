using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using FluentWebApi.Controllers;
using FluentWebApi.Model;
using FluentWebApi.Routing;
using FluentWebApi.Services;

namespace FluentWebApi.Configuration {
    public class Route
    {
        private Route() {}

        internal static readonly Type ApiModelType = typeof(IApiModel<>);
        
        // DataToken constants
        internal const string FluentWebApiEnabled = "FluentWebApiEnabled";
        internal const string ControllerType = "ControllerType";
        internal const string ControllerName = "ControllerName";
        
        // RouteDictionary constants
        public const string RouteTemplate = "RouteTemplate";
        public const string RouteName = "RouteName";
    }

    public class Route<T>
        where T : class, IApiModel {

        // Allowed, we're caching the type definition for each IApiModel
        // ReSharper disable StaticFieldInGenericType
        private static Type _dynamicApiControllerTypeDefinition;
        private static readonly Type ModelType = typeof(T);
        // ReSharper enable StaticFieldInGenericType

        internal Route(HttpVerb httpVerb) : this(httpVerb, null, null) { }

        internal Route(HttpVerb httpVerb, Type keyType) : this(httpVerb, keyType, null) { }

        internal Route(HttpVerb httpVerb, Type keyType, IDictionary<string, string> routeDictionary)
        {
            HttpVerb = httpVerb;
            KeyType = keyType;
            RouteDictionary = routeDictionary;

            InitializeHttpRoute();
        } 

        private void InitializeHttpRoute()
        {
            // Initialize the vars based on the data in this Fluent API route.
            var routeTemplate = GetRouteTemplate();
            var dataTokens = GetDataTokens();
            
            // Create a HttpRoute object 
            var httpRoute = GlobalConfiguration.Configuration.Routes.CreateRoute(routeTemplate, defaults: null, constraints: null, dataTokens: dataTokens, handler: new FluentApiHttpHandler(GlobalConfiguration.Configuration));

            // and add it to the routing table
            EnsureRouteName();
            GlobalConfiguration.Configuration.Routes.Add(Name, httpRoute);
        }

        private string GetRouteTemplate()
        {
            if (RouteDictionary != null)
            {
                string routeTemplate;
                if (RouteDictionary.TryGetValue(Route.RouteTemplate, out routeTemplate))
                {
                    return routeTemplate;
                }
            }

            // Build a template based on the current IApiModel type
            var routeTemplateBuilder = new StringBuilder(64);
            routeTemplateBuilder.AppendFormat("api/{0}", typeof(T).Name);

            if (KeyType != null)
            {
                routeTemplateBuilder.Append("/{id}");
            }

            return routeTemplateBuilder.ToString();
        }

        private void EnsureRouteName()
        {
            if (Name == null)
            {
                Name = GetRouteName();
            }
        }

        private string GetRouteName()
        {
            if (RouteDictionary != null)
            {
                string routeName;
                if (RouteDictionary.TryGetValue(Route.RouteName, out routeName))
                {
                    return routeName;
                }
            }

            // Return a route name based on the IApiModel, the verb, and key type
            var routeNameBuilder = new StringBuilder(64);
            routeNameBuilder.AppendFormat("{0}.{1}", typeof(T).Name, HttpVerb.Verb);

            if (KeyType != null)
            {
                routeNameBuilder.AppendFormat(".{0}", KeyType.Name);
            }

            return routeNameBuilder.ToString();
        }

        private Dictionary<string, object> GetDataTokens()
        {
            // Find the IApiModel<TKey> matching the model type
            Type controllerType = _dynamicApiControllerTypeDefinition;

            if (controllerType == null)
            {
                var apiModels = ModelType.FindInterfaces((t, c) => t.IsConstructedGenericType && t.GetGenericTypeDefinition() == Route.ApiModelType, null);
                if (apiModels.Length > 0)
                {
                    var apiModel = apiModels.First();
                    // Create a new type for FluentWebApiController<IApiModel<TKey>, TKey>
                    controllerType = typeof(FluentWebApiController<,>).MakeGenericType(ModelType, apiModel.GenericTypeArguments[0]);
                }
            }

            if (controllerType == null)
            {
                throw new InvalidOperationException(string.Format("\"{0}\" does not implement the FluentWebApi.Model.IApiModel<TKey> interface.", ModelType.Name));
            }

            if (_dynamicApiControllerTypeDefinition == null)
            {
                _dynamicApiControllerTypeDefinition = controllerType;
            }

            EnsureRouteName();
            return new Dictionary<string, object>
            {
                {Route.FluentWebApiEnabled, true},
                {Route.ControllerType, controllerType},
                {Route.ControllerName, ModelType.Name},
                {Route.RouteName, Name}
            };
        }

        internal string Name { get; private set; }

        internal IDictionary<string, string> RouteDictionary { get; private set; }

        internal HttpVerb HttpVerb { get; private set; }
        internal Type KeyType { get; private set; }

        internal Func<IEnumerable<T>> CollectionRetriever { get; set; }

        internal Func<object, T> ItemRetriever { get; private set; }

        /// <summary>
        /// Assigns the given delegate to <see cref="ItemRetriever"/> but boxes the typed parameter.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="func"></param>
        internal void SetItemRetriever<TData>(Func<TData, T> func)
        {
            ItemRetriever = o => func((TData)o);
        }

        internal Func<Responder, IHttpActionResult> Replier { get; set; }

        internal Func<Responder, object, IHttpActionResult> ReplierWithId { get; private set; }

        /// <summary>
        /// Assigns the given delegate to <see cref="ReplierWithId"/> but boxes the typed parameter.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="func"></param>
        internal void SetReplierWithId<TData>(Func<Responder, TData, IHttpActionResult> func)
        {
            ReplierWithId = (r, o) => func(r, (TData)o);
        }

        internal IEnumerable<T> GetData()
        {
            return CollectionRetriever();
        }

        internal T GetDataById(object id)
        {
            return ItemRetriever(id);
        }

        internal IHttpActionResult Reply(ApiController controller)
        {
            return Replier(new Responder(controller));
        }

        internal IHttpActionResult Reply(ApiController controller, object id)
        {
            return ReplierWithId(new Responder(controller), id);
        }
    }
}