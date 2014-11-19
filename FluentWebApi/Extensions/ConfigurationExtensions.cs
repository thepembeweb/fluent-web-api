﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Adds a <see cref="HttpVerb.Get"/> route for the model type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Route<T> OnGet<T>(this FluentWebApiRequest request) where T : class, IApiModel
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
        public static Route<T, TKey> OnGet<T, TKey>(this FluentWebApiRequest request) where T : class, IApiModel
        {
            return OnVerb<T, TKey>(HttpVerb.Get, default(TKey));
        }

        /// <summary>
        /// Adds a <see cref="HttpVerb.Get"/> route for the model type <typeparamref name="T"/>.
        /// The route will also have an ID parameter to identify the resource being retrieved. 
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <typeparam name="TKey">The type of the key that identifies the model</typeparam>
        /// <param name="request"></param>
        /// <param name="routeDictionary"></param>
        /// <returns></returns>
        public static Route<T, TKey> OnGet<T, TKey>(this FluentWebApiRequest request, object routeDictionary) where T : class, IApiModel
        {
            return OnVerb<T, TKey>(HttpVerb.Get, default(TKey), ToDictionary(routeDictionary));
        }

        /// <summary>
        /// Adds a <see cref="HttpVerb.Get"/> route for the model type <typeparamref name="T"/>.
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
        /// Adds a <see cref="HttpVerb.Post"/> route for the model type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">A model class that implements <see cref="IApiModel"/></typeparam>
        /// <param name="request"></param>
        /// <param name="model">An instance of the model class, can be null</param>
        /// <returns></returns>
        public static Route<T> OnPost<T>(this FluentWebApiRequest request, T model) where T : class, IApiModel
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
        public static Route<T, TKey> OnPut<T, TKey>(this FluentWebApiRequest request, TKey id, T model) where T : class, IApiModel
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
        public static Route<T, TKey> OnDelete<T, TKey>(this FluentWebApiRequest request, TKey id) where T : class, IApiModel
        {
            return OnVerb<T, TKey>(HttpVerb.Delete, id);
        }

        public static Route<T> OnCustomHttpVerb<T>(this FluentWebApiRequest request, string verb) where T : class, IApiModel
        {
            return OnVerb<T>(HttpVerb.GetVerb(verb));
        }

        public static Route<T, TParams> OnCustomHttpVerb<T, TParams>(this FluentWebApiRequest request, string verb, TParams parameters = null) 
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

        private static Route<T, TParams> OnVerb<T, TParams>(HttpVerb verb, TParams parameters, IDictionary<string, string> routeDictionary = null) 
            where T : class, IApiModel
        {
            var apiModelBinder = ApiModelBinder<T>.Instance;
            return apiModelBinder.AddRoute<TParams>(verb, routeDictionary);
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

            route.CollectionRetriever = func;
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

            route.SetItemRetriever(func);
            return ApiModelBinder<T>.Instance;
        }

        public static IApiModelBinder<T> ReplyWith<T, TKey>(this Route<T, TKey> route, Func<Responder, TKey, IHttpActionResult> func)
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

            route.SetReplierWithId(func);
            return ApiModelBinder<T>.Instance;
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