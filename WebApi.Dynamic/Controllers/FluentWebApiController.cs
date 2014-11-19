using System.Web.Http;
using FluentWebApi.Configuration;
using FluentWebApi.Model;
using FluentWebApi.Routing;

namespace FluentWebApi.Controllers
{
    internal class FluentWebApiController<T, TKey> : ApiController where T : class, IApiModel<TKey>
    {
        private readonly ApiModelBinder<T> _modelBinder;

        public FluentWebApiController()
        {
            _modelBinder = ApiModelBinder<T>.Instance;
        }

        public IHttpActionResult Get()
        {
            var route = _modelBinder.GetRoute(HttpVerb.Get, Request);
            if (route == null)
            {
                //TODO Provide a descriptive exception
                return InternalServerError();
            }

            if (route.Replier != null)
            {
                return route.Reply(this);
            }
            
            return Ok(route.GetData());
        }

        public IHttpActionResult GetById(TKey id)
        {
            var route = _modelBinder.GetRoute<TKey>(HttpVerb.Get, Request);
            if (route == null)
            {
                //TODO Provide a descriptive exception
                return InternalServerError();
            }

            if (route.ReplierWithId != null)
            {
                return route.Reply(this, id);
            }

            var model = route.GetDataById(id);
            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

    }
}
