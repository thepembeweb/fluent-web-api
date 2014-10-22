using System.Web.Http;
using WebApi.Dynamic.Model;

namespace WebApi.Dynamic.Controllers
{
    public class DynamicApiController<T> : ApiController where T : IApiModel, new()
    {
        public IHttpActionResult Get()
        {
            return Ok(new T());
        }
    }
}
