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
    internal class Route
    {
        internal static readonly Type ApiModelType = typeof(IApiModel<>);
        
        // DataToken constants
        internal const string FluentApiEnabled = "FluentApiEnabled";
        internal const string ControllerType = "ControllerType";
        internal const string ControllerName = "ControllerName";
    }

    public class Route<T>
        where T : class, IApiModel {

        // Allowed, we're caching the type definition for each IApiModel
        // ReSharper disable once StaticFieldInGenericType
        private static Type _dynamicApiControllerTypeDefinition;

        internal Route(HttpVerb httpVerb) : this(httpVerb, null, null) { }

        internal Route(HttpVerb httpVerb, Type keyType) : this(httpVerb, keyType, null) { }

        internal Route(HttpVerb httpVerb, Type keyType, string routeTemplate)
        {
            HttpVerb = httpVerb;
            KeyType = keyType;
            RouteTemplate = routeTemplate;

            InitializeHttpRoute();
        } 

        private void InitializeHttpRoute()
        {
            // Initialize the vars based on the data in this Fluent API route.
            var routeTemplate = GetRouteTemplate();

            // Find the IApiModel<TKey> matching the model type
            Type controllerType = _dynamicApiControllerTypeDefinition;
            var modelType = typeof(T);

            if (controllerType == null)
            {
                var apiModels = modelType.FindInterfaces((t, c) => t.IsConstructedGenericType && t.GetGenericTypeDefinition() == Route.ApiModelType, null);
                if (apiModels.Length > 0)
                {
                    var apiModel = apiModels.First();
                    // Create a new type for DynamicApiController<IApiModel<TKey>, TKey>
                    controllerType = typeof(DynamicApiController<,>).MakeGenericType(modelType, apiModel.GenericTypeArguments[0]);
                }
            }

            if (controllerType == null)
            {
                throw new InvalidOperationException(string.Format("\"{0}\" does not implement the FluentWebApi.Model.IApiModel<TKey> interface.", modelType.Name));
            }
            
            if (_dynamicApiControllerTypeDefinition == null)
            {
                _dynamicApiControllerTypeDefinition = controllerType;
            }

            var dataTokens = new Dictionary<string, object>
            {
                {Route.FluentApiEnabled, true},
                {Route.ControllerType, controllerType},
                {Route.ControllerName, modelType.Name}
            };

            // Create a HttpRoute object 
            var httpRoute = GlobalConfiguration.Configuration.Routes.CreateRoute(routeTemplate, defaults: null, constraints: null, dataTokens: dataTokens, handler: new FluentApiHttpHandler(GlobalConfiguration.Configuration));

            // and add it to the routing table
            var routeName = GetRouteName();
            GlobalConfiguration.Configuration.Routes.Add(routeName, httpRoute);
        }

        private string GetRouteTemplate()
        {
            if (!string.IsNullOrWhiteSpace(RouteTemplate))
            {
                // If a template has been given, use that.
                return RouteTemplate; 
            }

            // Build a template based on the current IApiModel type
            var routeTemplate = new StringBuilder(64);
            routeTemplate.AppendFormat("api/{0}", typeof(T).Name);

            if (KeyType != null)
            {
                routeTemplate.Append("/{id}");
            }

            return routeTemplate.ToString();
        }

        private string GetRouteName()
        {
            // Return a route name based on the IApiModel, the verb, and key type
            var routeName = new StringBuilder(64);
            routeName.AppendFormat("{0}.{1}", typeof(T).Name, HttpVerb.Verb);

            if (KeyType != null)
            {
                routeName.AppendFormat(".{0}", KeyType.Name);
            }

            return routeName.ToString();
        }

        internal string RouteTemplate { get; private set; }

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