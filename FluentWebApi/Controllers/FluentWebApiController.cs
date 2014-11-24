using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using FluentWebApi.Configuration;
using FluentWebApi.Model;
using FluentWebApi.Resources;
using FluentWebApi.Routing;

namespace FluentWebApi.Controllers
{
    /// <summary>
    /// Web API controller class for an <see cref="IApiModel"/>. This class will be instantiated when a Fluent Web API route is being hit.
    /// </summary>
    /// <typeparam name="T">A class that implements <see cref="IApiModel{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies an instance of <typeparamref name="T"/>.</typeparam>
    internal class FluentWebApiController<T, TKey> : ApiController where T : class, IApiModel<TKey>
    {
        private readonly ApiModelBinder<T> _modelBinder;

        public FluentWebApiController()
        {
            _modelBinder = ApiModelBinder<T>.Instance;
        }

        private void AddAllowedVerbsToHeader()
        {
            var allowedVerbs = _modelBinder.AllowedVerbs;
            HttpContext.Current.Response.Headers.Add("Access-Control-Allow-Methods", string.Join(",", allowedVerbs.Select(v => v.Verb)));
        }

        private bool IsVerbAllowed(HttpVerb verb)
        {
            return _modelBinder.AllowedVerbs.Any(v => v.Verb == verb.Verb);
        }
        
        [HttpOptions]
        public IHttpActionResult AllowedMethods()
        {
            AddAllowedVerbsToHeader();

            return Ok();
        }

        public IHttpActionResult Get()
        {
            if (!IsVerbAllowed(HttpVerb.Get))
            {
                // 405 - Method not allowed + header mentioning the allowed verbs
                AddAllowedVerbsToHeader();
                return StatusCode(HttpStatusCode.MethodNotAllowed);  
            }

            var route = _modelBinder.GetRoute(Request);
            if (route == null)
            {
                return InternalServerError(new Exception(SR.ErrNoRouteConfigured));
            }

            if (route.Replier != null)
            {
                return route.Reply(this);
            }
            
            return Ok(route.GetData());
        }

        public IHttpActionResult GetById(TKey id)
        {
            if (!IsVerbAllowed(HttpVerb.Get))
            {
                AddAllowedVerbsToHeader();
                return StatusCode(HttpStatusCode.MethodNotAllowed);
            }

            var route = _modelBinder.GetRoute<TKey>(Request);
            if (route == null)
            {
                return InternalServerError(new Exception(SR.ErrNoRouteConfigured));
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
            if (!IsVerbAllowed(HttpVerb.Post))
            {
                AddAllowedVerbsToHeader();
                return StatusCode(HttpStatusCode.MethodNotAllowed);
            }

            var route = _modelBinder.GetRoute(Request);
            if (route == null)
            {
                return InternalServerError(new Exception(SR.ErrNoRouteConfigured));
            }

            if (route.ReplierWithModel != null)
            {
                return route.Reply(this, model);
            }

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
