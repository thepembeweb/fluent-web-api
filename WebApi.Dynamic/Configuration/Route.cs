using System;
using System.Collections.Generic;
using System.Web.Http;
using FluentWebApi.Controllers;
using FluentWebApi.Model;
using FluentWebApi.Routing;

namespace FluentWebApi.Configuration {
    public class Route<T>
        where T : class, IApiModel {

        internal Route(HttpVerb httpVerb) : this(httpVerb, null) { }

        internal Route(HttpVerb httpVerb, Type keyType)
        {
            HttpVerb = httpVerb;
            KeyType = keyType;
        }

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