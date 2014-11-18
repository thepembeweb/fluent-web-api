using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;

namespace FluentWebApi.Services
{
    class DynamicControllerSelector : DefaultHttpControllerSelector
    {
        private readonly HttpConfiguration _configuration;
        private readonly Lazy<ConcurrentDictionary<string, HttpControllerDescriptor>> _controllerInfoCache;

        public DynamicControllerSelector(HttpConfiguration configuration) : base(configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            _controllerInfoCache = new Lazy<ConcurrentDictionary<string, HttpControllerDescriptor>>();
            _configuration = configuration;
        }

        /// <summary>
        /// Tries to select an appropriate controller class for the given <paramref name="request"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            IHttpRouteData routeData = request.GetRouteData();
            if (routeData != null && routeData.Route.DataTokens != null)
            {
                object controllerTypeObj = null;
                if (routeData.Route.DataTokens.TryGetValue("ControllerType", out controllerTypeObj))
                {
                    var controllerType = controllerTypeObj as Type;
                    if (controllerType != null)
                    {
                        //Found a dynamic api controller type!
                        string controllerName = routeData.Route.DataTokens["ControllerName"].ToString();
                        var controllerDescriptor = new HttpControllerDescriptor(_configuration, controllerName, controllerType);
                        _controllerInfoCache.Value.TryAdd(controllerName, controllerDescriptor);

                        return controllerDescriptor;
                    }
                }
            }

            return base.SelectController(request); // Fallback to the normal procedure
        }

        public override IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            // Merge our cache and the base cache together
            var baseDictionary = base.GetControllerMapping();
            var ourDictionary = _controllerInfoCache.Value.ToDictionary(c => c.Key, c => c.Value, StringComparer.OrdinalIgnoreCase);

            foreach (var kvp in baseDictionary)
            {
                if (ourDictionary.ContainsKey(kvp.Key))
                {
                    continue;
                }

                ourDictionary.Add(kvp.Key, kvp.Value);
            }

            return ourDictionary;
        }
    }
}