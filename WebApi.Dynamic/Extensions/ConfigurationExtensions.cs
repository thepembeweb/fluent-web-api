using System;
using System.Collections.Generic;
using System.Web.Http;
using FluentWebApi.Controllers;
using FluentWebApi.Model;
using FluentWebApi.Routing;

// ReSharper disable once CheckNamespace
namespace FluentWebApi.Configuration
{
    public static class ConfigurationExtensions
    {

        //public static IApiModelBinder<T, TKey> For<T, TKey>(this HttpConfiguration config)
        //    where T : class, IApiModel<TKey>
        //{
        //    return ApiModelBinder<T, TKey>.Instance;
        //}

        //public static IApiModelBinder<T> Get<T, TKey>(this HttpConfiguration config)
        //    where T : class, IApiModel<TKey>
        //{
        //    return ApiModelBinder<T, TKey>.Instance;
        //}

        /// <summary>
        /// Adds a <see cref="HttpVerb.Get"/> route for the model type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Route<T> OnGet<T>(this DynamicWebApiRequest request) where T : class, IApiModel
        {
            return OnVerb<T>(HttpVerb.Get);
        }

        /// <summary>
        /// Adds a <see cref="HttpVerb.Get"/> route for the model type <typeparamref name="T"/>.
        /// The route will also have an ID parameter to identify the resource being retrieved. 
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <typeparam name="TKey">The type of the key that identifies the model</typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Route<T, TKey> OnGet<T, TKey>(this DynamicWebApiRequest request) where T : class, IApiModel
        {
            return OnVerb<T, TKey>(HttpVerb.Get, default(TKey));
        }

        /// <summary>
        /// Adds a <see cref="HttpVerb.Post"/> route for the model type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <param name="request"></param>
        /// <param name="model">An instance of the model class, can be null</param>
        /// <returns></returns>
        public static Route<T> OnPost<T>(this DynamicWebApiRequest request, T model) where T : class, IApiModel
        {
            return OnVerb<T>(HttpVerb.Post);
        }

        /// <summary>
        /// Adds a <see cref="HttpVerb.Put"/> route for the model type <typeparamref name="T"/>.
        /// The route will also have a <paramref name="id"/> parameter to identify the resource being updated (or created), 
        /// using the values stored in <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <typeparam name="TKey">The type of the key that identifies the model</typeparam>
        /// <param name="request"></param>
        /// <param name="id">Any value of <typeparamref name="TKey"/></param>
        /// <param name="model">An instance of the model class, can be null</param>
        /// <returns></returns>
        public static Route<T, TKey> OnPut<T, TKey>(this DynamicWebApiRequest request, TKey id, T model) where T : class, IApiModel
        {
            return OnVerb<T, TKey>(HttpVerb.Put, id);
        }

        /// <summary>
        /// Adds a <see cref="HttpVerb.Delete"/> route for the model type <typeparamref name="T"/>.
        /// The route will also have an <paramref name="id"/> parameter to identify the resource being deleted.
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <typeparam name="TKey">The type of the key that identifies the model</typeparam>
        /// <param name="request"></param>
        /// <param name="id">Any value of <typeparamref name="TKey"/></param>
        /// <returns></returns>
        public static Route<T, TKey> OnDelete<T, TKey>(this DynamicWebApiRequest request, TKey id) where T : class, IApiModel
        {
            return OnVerb<T, TKey>(HttpVerb.Delete, id);
        }

        public static Route<T> OnCustomHttpVerb<T>(this DynamicWebApiRequest request, string verb) where T : class, IApiModel
        {
            return OnVerb<T>(HttpVerb.GetVerb(verb));
        }

        public static Route<T, TParams> OnCustomHttpVerb<T, TParams>(this DynamicWebApiRequest request, string verb, TParams parameters = null) 
            where T : class, IApiModel
            where TParams : class
        {
            return OnVerb<T, TParams>(HttpVerb.GetVerb(verb), parameters);
        }

        private static Route<T> OnVerb<T>(HttpVerb verb)
            where T : class, IApiModel
        {
            var apiModelBinder = ApiModelBinder<T>.Instance;
            return apiModelBinder.AddRoute(verb);
        }

        private static Route<T, TParams> OnVerb<T, TParams>(HttpVerb verb, TParams parameters) 
            where T : class, IApiModel
        {
            var apiModelBinder = ApiModelBinder<T>.Instance;
            return apiModelBinder.AddRoute<TParams>(verb);
        }

        public static IApiModelBinder<T> Use<T>(this Route<T> route, Func<IEnumerable<T>> func)
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

            route.DataRetriever = func;
            return ApiModelBinder<T>.Instance;
        }

        public static IApiModelBinder<T> Use<T, TKey>(this Route<T, TKey> route, Func<TKey, T> func)
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

            route.DataRetriever = func;
            return ApiModelBinder<T>.Instance;
        }

        public static IApiModelBinder<T> ReplyWith<T, TKey>(this Route<T, TKey> route, Func<HelperApiController, TKey, IHttpActionResult> func)
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

            route.Replier = func;
            return ApiModelBinder<T>.Instance;
        } 


        //public static IApiModelBinder<T, TKey> Use<T, TKey>(this IApiModelBinder<T, TKey> apiModelBinder, Func<TKey, T> func)
        //    where T : class, IApiModel<TKey>
        //{
        //    if (apiModelBinder == null)
        //    {
        //        throw new ArgumentNullException("apiModelBinder");
        //    }

        //    if (func == null)
        //    {
        //        throw new ArgumentNullException("func");
        //    }

        //    var amb = apiModelBinder as ApiModelBinder<T, TKey>;
        //    if (amb.HasCustomDataProvider)
        //    {
        //        throw new InvalidOperationException("A custom data provider has been configured for this IApiModel.");
        //    }

        //    amb.AddDataProvider(func);
        //    return amb;
        //}

        //public static IApiModelBinder<T, TKey> Use<T, TKey>(this IApiModelBinder<T, TKey> apiModelBinder, Func<IEnumerable<T>> func)
        //    where T : class, IApiModel<TKey>
        //{
        //    if (apiModelBinder == null)
        //    {
        //        throw new ArgumentNullException("apiModelBinder");
        //    }

        //    if (func == null)
        //    {
        //        throw new ArgumentNullException("func");
        //    }

        //    var amb = apiModelBinder as ApiModelBinder<T, TKey>;
        //    if (amb.HasCustomDataProvider)
        //    {
        //        throw new InvalidOperationException("A custom data provider has been configured for this IApiModel.");
        //    }

        //    amb.AddDataProvider(func);
        //    return amb;
        //}

        //public static IApiModelBinder<T, TKey> Use<T, TKey>(this IApiModelBinder<T, TKey> apiModelBinder, IDataProvider<T, TKey> dataProvider)
        //    where T : class, IApiModel<TKey>
        //{
        //    if (apiModelBinder == null)
        //    {
        //        throw new ArgumentNullException("apiModelBinder");
        //    }

        //    if (dataProvider == null)
        //    {
        //        throw new ArgumentNullException("dataProvider");
        //    }

        //    var amb = apiModelBinder as ApiModelBinder<T, TKey>;
        //    amb.SetCustomDataProvider(dataProvider);
        //    return amb;
        //}
    }
}