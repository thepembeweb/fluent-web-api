using FluentWebApi.Configuration;

// ReSharper disable once CheckNamespace
namespace System.Web.Http.Routing
{
    // ReSharper disable once InconsistentNaming
    public static class IHttpRouteExtensions
    {
        /// <summary>
        /// Tries to retrieve the route name from the <see cref="IHttpRoute.DataTokens"/>.
        /// </summary>
        /// <param name="route"></param>
        /// <returns>The name of the route, or null if no route name was found in the <see cref="IHttpRoute.DataTokens"/> dictionary.</returns>
        public static string GetRouteName(this IHttpRoute route)
        {
            return GetValueFromDataTokens<string>(route, Route.RouteName);
        }

        /// <summary>
        /// Tries to retrieve the controller name from the <see cref="IHttpRoute.DataTokens"/>.
        /// </summary>
        /// <param name="route"></param>
        /// <returns>The name of the controller, or null if no name was found in the <see cref="IHttpRoute.DataTokens"/> dictionary.</returns>
        public static string GetControllerName(this IHttpRoute route)
        {
            return GetValueFromDataTokens<string>(route, Route.ControllerName);
        }

        /// <summary>
        /// Tries to retrieve the controller type from the <see cref="IHttpRoute.DataTokens"/>.
        /// </summary>
        /// <param name="route"></param>
        /// <returns>The type definition of the controller, or null if no controller type was found in the <see cref="IHttpRoute.DataTokens"/> dictionary.</returns>
        public static Type GetControllerType(this IHttpRoute route)
        {
            return GetValueFromDataTokens<Type>(route, Route.ControllerType);
        }

        /// <summary>
        /// Returns true if Fluent Web API is enabled for this <paramref name="route"/>.
        /// </summary>
        public static bool IsFluentWebApiEnabled(this IHttpRoute route)
        {
            return GetStructValueFromDataTokens<bool>(route, Route.FluentWebApiEnabled);
        }

        private static T GetValueFromDataTokens<T>(IHttpRoute route, string key) where T : class
        {
            if (route.DataTokens != null)
            {
                object value;
                if (route.DataTokens.TryGetValue(key, out value))
                {
                    return value as T;
                }
            }

            return default(T);
        }

        private static T GetStructValueFromDataTokens<T>(IHttpRoute route, string key) where T : struct
        {
            if (route.DataTokens != null)
            {
                object value;
                if (route.DataTokens.TryGetValue(key, out value))
                {
                    return (value as T?).GetValueOrDefault();
                }
            }

            return default(T);
        }
    }
}