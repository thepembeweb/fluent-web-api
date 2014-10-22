using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;
using WebApi.Dynamic.Controllers;
using WebApi.Dynamic.Model;

namespace WebApi.Dynamic.Services
{
    public class DynamicControllerSelector : DefaultHttpControllerSelector
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
            HttpControllerDescriptor controllerDescriptor;
            if (routeData != null)
            {
                // TODO Deal with attribute routing
                //controllerDescriptor = base.GetDirectRouteController(routeData);
                //if (controllerDescriptor != null)
                //{
                //    return controllerDescriptor;
                //}
            }

            string controllerName = GetControllerName(request);
            if (!String.IsNullOrEmpty(controllerName))
            {
                if (_controllerInfoCache.Value.TryGetValue(controllerName, out controllerDescriptor))
                {
                    return controllerDescriptor;
                }

                // Try to locate a dynamic API model that matches the controller name
                ICollection<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly assembly in assemblies)
                {
                    if (assembly == null || assembly.IsDynamic)
                    {
                        // can't call GetTypes on a null (or dynamic?) assembly
                        continue;
                    }

                    var modelTypes = assembly.GetTypes().Where(t => t.IsClass && typeof(IApiModel).IsAssignableFrom(t) && string.Equals(t.Name, controllerName, StringComparison.OrdinalIgnoreCase)).ToListOrNull();
                    if (modelTypes != null && modelTypes.Count == 1)
                    {
                        var controllerType = typeof(DynamicApiController<>).MakeGenericType(modelTypes.First());
                        controllerDescriptor = new HttpControllerDescriptor(_configuration, controllerName, controllerType);
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