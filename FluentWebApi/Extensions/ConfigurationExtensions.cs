using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using FluentWebApi.Controllers;
using FluentWebApi.Model;
using FluentWebApi.Routing;

// ReSharper disable once CheckNamespace
namespace FluentWebApi.Configuration
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Adds a route for the resource type <typeparamref name="T"/> and marks the <see cref="HttpVerb.Get"/> verb as allowed.
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        public static Route<T> OnGet<T>(this FluentWebApiRequest request, Expression<Func<T>> expression = null) where T : class, IApiModel
        {
            return OnVerb<T>(HttpVerb.Get);
        }

        /// <summary>
        /// Adds a <see cref="HttpVerb.Get"/> route for the resource type <typeparamref name="T"/>.
        /// The route will also have an ID parameter to identify the resource being retrieved. 
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <typeparam name="TKey">The type of the key that identifies the model</typeparam>
        public static Route<T, TKey> OnGet<T, TKey>(this FluentWebApiRequest request, Expression<Func<TKey, T>> expression = null) where T : class, IApiModel
        {
            return OnVerb<T, TKey>(HttpVerb.Get, default(TKey));
        }

        /// <summary>
        /// Adds a route for the resource type <typeparamref name="T"/> and marks the <see cref="HttpVerb.Get"/> verb as allowed.
        /// The route will also have an ID parameter to identify the resource being retrieved. 
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <typeparam name="TKey">The type of the key that identifies the model</typeparam>
        public static Route<T, TKey> OnGet<T, TKey>(this FluentWebApiRequest request, object routeDictionary) where T : class, IApiModel
        {
            return OnVerb<T, TKey>(HttpVerb.Get, default(TKey), ToDictionary(routeDictionary));
        }

        /// <summary>
        /// Adds a route for the resource type <typeparamref name="T"/> and marks the <see cref="HttpVerb.Get"/> verb as allowed.
        /// The route will also have an ID parameter to identify the resource being retrieved. 
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <typeparam name="TKey">The type of the key that identifies the model</typeparam>
        public static Route<T, TKey> OnGet<T, TKey>(this FluentWebApiRequest request, Expression<Func<TKey, T>> expression, object routeDictionary) where T : class, IApiModel
        {
            return OnVerb<T, TKey>(HttpVerb.Get, default(TKey), ToDictionary(routeDictionary));
        }

        /// <summary>
        /// Adds a route for the resource type <typeparamref name="T"/> and marks the <see cref="HttpVerb.Get"/> verb as allowed.
        /// The route will also have an ID parameter to identify the resource being retrieved. 
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <typeparam name="TKey">The type of the key that identifies the model</typeparam>
        /// <param name="request"></param>
        /// <param name="routeDictionary"></param>
        /// <returns></returns>
        public static Route<T, TKey> OnGet<T, TKey>(this FluentWebApiRequest request, IDictionary<string, string> routeDictionary) where T : class, IApiModel
        {
            return OnVerb<T, TKey>(HttpVerb.Get, default(TKey), routeDictionary);
        }

        /// <summary>
        /// Adds a route for the resource type <typeparamref name="T"/> and marks the <see cref="HttpVerb.Get"/> verb as allowed.
        /// The route will also have an ID parameter to identify the resource being retrieved. 
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <typeparam name="TKey">The type of the key that identifies the model</typeparam>
        public static Route<T, TKey> OnGet<T, TKey>(this FluentWebApiRequest request, Expression<Func<TKey, T>> expression, IDictionary<string, string> routeDictionary) where T : class, IApiModel
        {
            return OnVerb<T, TKey>(HttpVerb.Get, default(TKey), routeDictionary);
        }

        /// <summary>
        /// Adds a route for the resource type <typeparamref name="T"/> and marks the <see cref="HttpVerb.Post"/> verb as allowed.
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        public static Route<T> OnPost<T>(this FluentWebApiRequest request, T model = default(T)) where T : class, IApiModel
        {
            return OnVerb<T>(HttpVerb.Post);
        }

        /// <summary>
        /// Adds a route for the resource type <typeparamref name="T"/> and marks the <see cref="HttpVerb.Put"/> verb as allowed.
        /// The route will also have an ID parameter to identify the resource being updated (or created), 
        /// using the values stored in the model.
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <typeparam name="TKey">The type of the key that identifies the model</typeparam>
        /// <returns></returns>
        public static Route<T, TKey> OnPut<T, TKey>(this FluentWebApiRequest request, Expression<Func<TKey, T>> expression = null) where T : class, IApiModel
        {
            return OnVerb<T, TKey>(HttpVerb.Put, default(TKey));
        }

        /// <summary>
        /// Adds a route for the resource type <typeparamref name="T"/> and marks the <see cref="HttpVerb.Delete"/> verb as allowed.
        /// The route will also have an ID parameter to identify the resource being deleted.
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <typeparam name="TKey">The type of the key that identifies the model</typeparam>
        /// <returns></returns>
        public static Route<T, TKey> OnDelete<T, TKey>(this FluentWebApiRequest request, Expression<Func<TKey, T>> expression = null) where T : class, IApiModel
        {
            return OnVerb<T, TKey>(HttpVerb.Delete, default(TKey));
        }

        /// <summary>
        /// Adds a route for the resource type <typeparamref name="T"/> and marks the custom <paramref name="verb" /> as allowed.
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <param name="request"></param>
        /// <param name="verb">A custom HTTP verb.</param>
        /// <returns></returns>
        public static Route<T> OnCustomHttpVerb<T>(this FluentWebApiRequest request, string verb) where T : class, IApiModel
        {
            return OnVerb<T>(HttpVerb.GetVerb(verb));
        }

        /// <summary>
        /// Adds a route for the resource type <typeparamref name="T"/> and marks the custom <paramref name="verb" /> as allowed.
        /// The route will also have an <paramref name="id"/> parameter to identify the resource being deleted.
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <typeparam name="TKey">The type of the key that identifies the model</typeparam>
        /// <param name="request"></param>
        /// <param name="verb">A custom HTTP verb.</param>
        /// <returns></returns>
        public static Route<T, TKey> OnCustomHttpVerb<T, TKey>(this FluentWebApiRequest request, string verb, TKey id = null) 
            where T : class, IApiModel
            where TKey : class
        {
            return OnVerb<T, TKey>(HttpVerb.GetVerb(verb), id);
        }

        private static Route<T> OnVerb<T>(HttpVerb verb)
            where T : class, IApiModel
        {
            var apiModelBinder = ApiModelBinder<T>.Instance;
            apiModelBinder.AllowVerb(verb);

            return apiModelBinder.GetOrCreateRoute();
        }

        private static Route<T, TParams> OnVerb<T, TParams>(HttpVerb verb, TParams parameters, IDictionary<string, string> routeDictionary = null) 
            where T : class, IApiModel
        {
            var apiModelBinder = ApiModelBinder<T>.Instance;
            apiModelBinder.AllowVerb(verb);

            return apiModelBinder.GetOrCreateRoute<TParams>(routeDictionary);
        }

        /// <summary>
        /// Configures the <paramref name="route"/> to use the given <paramref name="func"/> to retrieve an <see cref="IEnumerable{T}"/> of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        public static Route<T> ReadUsing<T>(this Route<T> route, Func<IEnumerable<T>> func)
            where T : class, IApiModel
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            route.CollectionRetriever = func;
            return route;
        }

        /// <summary>
        /// Configures the <paramref name="route"/> to use the given <paramref name="func"/> to retrieve an instance of <typeparamref name="T"/>,
        /// identified by a key value of type <typeparamref name="TKey"/>.
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <typeparam name="TKey">The type of the key that identifies the model</typeparam>
        public static Route<T, TKey> ReadUsing<T, TKey>(this Route<T, TKey> route, Func<TKey, T> func)
            where T : class, IApiModel<TKey>
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            route.ItemRetriever = o => func((TKey)o);
            return route;
        }

        /// <summary>
        /// Configures the <paramref name="route"/> to use the given <paramref name="method"/> to add a new instance of <typeparamref name="T"/>
        /// to the data model.
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        public static Route<T> CreateUsing<T>(this Route<T> route, Action<T> method)
            where T : class, IApiModel
        {
            return CreateUsing(route, data =>
            {
                method(data);
                return data;
            });
        }

        /// <summary>
        /// Configures the <paramref name="route"/> to use the given <paramref name="method"/> to add a new instance of <typeparamref name="T"/>
        /// to the data model.
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <returns>The model after it has been passed through the data layer, which can be useful to retrieve default values that are being set by a database, in example.</returns>
        public static Route<T> CreateUsing<T>(this Route<T> route, Func<T, T> method)
            where T : class, IApiModel
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            route.Creator = method;
            return route;
        }

        /// <summary>
        /// Configures the <paramref name="route"/> to use the given <paramref name="method"/> to update an instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <typeparam name="TKey">The type of the key that identifies the model</typeparam>
        public static Route<T, TKey> UpdateUsing<T, TKey>(this Route<T, TKey> route, Action<TKey, T> method)
            where T : class, IApiModel<TKey>
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            route.Updater = (o, m) => method((TKey)o, m);
            return route;
        }

        /// <summary>
        /// Configures the <paramref name="route"/> to use the given <paramref name="method"/> to remove an instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <typeparam name="TKey">The type of the key that identifies the model</typeparam>
        public static Route<T, TKey> DeleteUsing<T, TKey>(this Route<T, TKey> route, Action<TKey> method)
            where T : class, IApiModel<TKey>
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            route.Deleter = o => method((TKey)o);
            return route;
        }

        /// <summary>
        /// Configures Fluent Web API to use a custom reply instead of its default behavior when responding to the <paramref name="route"/>.
        /// </summary>
        public static Route<T> ReplyWith<T>(this Route<T> route, Func<Responder, IHttpActionResult> func)
            where T : class, IApiModel
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            route.Replier = func;
            return route;
        }

        /// <summary>
        /// Configures Fluent Web API to use a custom reply instead of its default behavior when responding to the <paramref name="route"/>.
        /// </summary>
        public static Route<T, TKey> ReplyWith<T, TKey>(this Route<T, TKey> route, Func<Responder, TKey, IHttpActionResult> func)
            where T : class, IApiModel<TKey>
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            route.ReplierWithId = (r, o) => func(r, (TKey)o);
            return route;
        }

        /// <summary>
        /// Configures Fluent Web API to use a custom reply instead of its default behavior when responding to the <paramref name="route"/>.
        /// </summary>
        public static Route<T> ReplyWith<T>(this Route<T> route, Func<Responder, T, IHttpActionResult> func)
            where T : class, IApiModel
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            route.ReplierWithModel = func;
            return route;
        }

        /// <summary>
        /// Configures Fluent Web API to use a custom reply instead of its default behavior when responding to the <paramref name="route"/>.
        /// </summary>
        public static Route<T, TKey> ReplyWith<T, TKey>(this Route<T, TKey> route, Func<Responder, TKey, T, IHttpActionResult> func)
            where T : class, IApiModel<TKey>
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            route.ReplierWithIdAndModel = (r, o, m) => func(r, (TKey)o, m);
            return route;
        }

        private static IDictionary<string, string> ToDictionary(object values)
        {
            var valuesAsDictionary = values as IDictionary<string, string>;
            if (valuesAsDictionary != null)
            {
                return valuesAsDictionary;
            }

            if (values != null)
            {
                return values.GetType().GetProperties().ToDictionary(property => property.Name, property => property.GetValue(values) as string);
            }

            return null;
        }
    }
}