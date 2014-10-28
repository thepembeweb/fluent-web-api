using System.Web.Http;
using WebApi.Dynamic.Configuration;
using WebApi.Dynamic.Model;

namespace WebApi.Dynamic.Controllers
{
    class DynamicApiController<T, TKey> : ApiController where T : class, IApiModel<TKey>
    {
        private readonly ApiModelBinder<T, TKey> _modelBinder;

        public DynamicApiController()
        {
            _modelBinder = ApiModelBinder<T, TKey>.Instance;
        }

        public IHttpActionResult Get()
        {
            if (!_modelBinder.HasListDataProvider)
            {
                //TODO Provide a descriptive exception
                return InternalServerError();
            }
            
            return Ok(_modelBinder.GetList());
        }

        public IHttpActionResult GetById(TKey id)
        {
            if (!_modelBinder.HasSingleDataProvider)
            {
                //TODO Provide a descriptive exception
                return InternalServerError();
            }

            var model = _modelBinder.Get(id);
            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

    }
}
