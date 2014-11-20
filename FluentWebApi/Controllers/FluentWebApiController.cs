using System.Linq;
using System.Web;
using System.Web.Http;
using FluentWebApi.Configuration;
using FluentWebApi.Model;

namespace FluentWebApi.Controllers
{
    internal class FluentWebApiController<T, TKey> : ApiController where T : class, IApiModel<TKey>
    {
        private readonly ApiModelBinder<T> _modelBinder;

        public FluentWebApiController()
        {
            _modelBinder = ApiModelBinder<T>.Instance;
        }

        [HttpOptions]
        public IHttpActionResult AllowedMethods()
        {
            var allowedVerbs = _modelBinder.EnabledVerbs;
            HttpContext.Current.Response.Headers.Add("Access-Control-Allow-Methods", string.Join(",", allowedVerbs.Select(v => v.Verb)));

            return Ok();
        }

        public IHttpActionResult Get()
        {
            var route = _modelBinder.GetRoute(Request);
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
            var route = _modelBinder.GetRoute<TKey>(Request);
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

        public IHttpActionResult Post(T model)
        {
            var route = _modelBinder.GetRoute(Request);
            if (route == null)
            {
                //TODO Provide a descriptive exception
                return InternalServerError();
            }

            // TODO reply mechanism
            //if (route.Replier != null)
            //{
            //    return route.Reply(this, model);
            //}

            if (ModelState.IsValid)
            {
                model = route.Creator(model);

                var getbyIdRoute = _modelBinder.GetRoute(null, typeof(TKey));
                if (getbyIdRoute != null)
                {
                    return CreatedAtRoute(getbyIdRoute.Name, new { model.Id }, model);
                }
                
                // No ID route found. Return 200 OK with the newly created item, although this is not really RESTful...
                return Ok(model);
            }

            return BadRequest(ModelState);
        }


    }
}
