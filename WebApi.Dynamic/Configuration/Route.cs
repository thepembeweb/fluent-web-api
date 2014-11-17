using System;
using System.Collections.Generic;
using System.Web.Http;
using FluentWebApi.Controllers;
using FluentWebApi.Model;
using FluentWebApi.Routing;

namespace FluentWebApi.Configuration {
    public class Route<T>
        where T : class, IApiModel {
        internal Route(HttpVerb httpVerb)
        {
            HttpVerb = httpVerb;
        }

        internal HttpVerb HttpVerb { get; private set; }
        internal Func<IEnumerable<T>> DataRetriever { get; set; }
        internal Func<Responder, IHttpActionResult> Replier { get; set; }

        internal IEnumerable<T> GetData()
        {
            return DataRetriever();
        }

        internal IHttpActionResult Reply(ApiController controller)
        {
            return Replier(new Responder(controller));
        }
    }

    public class Route<T, TKey>
        where T : class, IApiModel
    {
        internal Route(HttpVerb httpVerb)
        {
            HttpVerb = httpVerb;
        }

        internal HttpVerb HttpVerb { get; private set; }

        internal Func<TKey, T> DataRetriever { get; set; }
        internal Func<Responder, TKey, IHttpActionResult> Replier { get; set; }

        internal T GetData(TKey id)
        {
            return DataRetriever(id);
        }

        internal IHttpActionResult Reply(ApiController controller, TKey id)
        {
            return Replier(new Responder(controller), id);
        }
    }
}