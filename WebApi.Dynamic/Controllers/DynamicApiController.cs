using System.Web.Http;
using FluentWebApi.Configuration;
using FluentWebApi.Model;
using FluentWebApi.Routing;

namespace FluentWebApi.Controllers
{
    class DynamicApiController<T, TKey> : ApiController where T : class, IApiModel<TKey>
    {
        private readonly ApiModelBinder<T> _modelBinder;

        public DynamicApiController()
        {
            _modelBinder = ApiModelBinder<T>.Instance;
        }

        public IHttpActionResult Get()
        {
            var route = _modelBinder.GetRoute(HttpVerb.Get);
            if (route == null)
            {
                //TODO Provide a descriptive exception
                return InternalServerError();
            }
            
            return Ok(route.GetData());
        }

        public IHttpActionResult GetById(TKey id)
        {
            var route = _modelBinder.GetRoute<TKey>(HttpVerb.Get);
            if (route == null)
            {
                //TODO Provide a descriptive exception
                return InternalServerError();
            }

            var model = route.GetData(id);
            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

    }
}
