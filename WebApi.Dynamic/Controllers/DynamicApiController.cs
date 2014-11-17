using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;
using FluentWebApi.Configuration;
using FluentWebApi.Model;
using FluentWebApi.Routing;

namespace FluentWebApi.Controllers
{
    public class Responder : ApiController
    {
        private readonly ApiController _parent;

        private Responder() { }

        internal Responder(ApiController parent)
        {
            _parent = parent;
            InitializeContext();
        }

        private void InitializeContext()
        {
            ActionContext = _parent.ActionContext;
            Configuration = _parent.Configuration;
            ControllerContext = _parent.ControllerContext;
            Request = _parent.Request;
            RequestContext = _parent.RequestContext;
            User = _parent.User;
        }

        public new OkResult Ok()
        {
            return base.Ok();
        }

        public new OkNegotiatedContentResult<T> Ok<T>(T content)
        {
            return base.Ok(content);
        }

        public new CreatedNegotiatedContentResult<T> Created<T>(Uri location, T content)
        {
            return base.Created(location, content);
        }

        public new CreatedNegotiatedContentResult<T> Created<T>(string location, T content)
        {
            return base.Created(location, content);
        }

        public new CreatedAtRouteNegotiatedContentResult<T> CreatedAtRoute<T>(string routeName, object routeValues, T content)
        {
            return base.CreatedAtRoute(routeName, routeValues, content);
        }

        public new CreatedAtRouteNegotiatedContentResult<T> CreatedAtRoute<T>(string routeName, IDictionary<string, object> routeValues, T content)
        {
            return base.CreatedAtRoute(routeName, routeValues, content);
        }

        public new StatusCodeResult StatusCode(HttpStatusCode status)
        {
            return base.StatusCode(status);
        }

        public new BadRequestResult BadRequest()
        {
            return base.BadRequest();
        }

        public new InvalidModelStateResult BadRequest(ModelStateDictionary modelState)
        {
            return base.BadRequest(modelState);
        }

        public new BadRequestErrorMessageResult BadRequest(string message)
        {
            return base.BadRequest(message);
        }
        
        public new NotFoundResult NotFound()
        {
            return base.NotFound();
        }

        
    }

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

            if (route.Replier != null)
            {
                return route.Reply(this);
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

            if (route.Replier != null)
            {
                return route.Reply(this, id);
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
