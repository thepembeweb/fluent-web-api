using System.Web.Http.Dispatcher;
using WebApi.Dynamic.Configuration;
using WebApi.Dynamic.Services;

// ReSharper disable once CheckNamespace
namespace System.Web.Http
{
    public static class HttpConfigurationExtensions
    {
        public static DynamicWebApiRequest UseDynamicWebApi(this HttpConfiguration config)
        {
            config.Services.Replace(typeof(IHttpControllerSelector), new DynamicControllerSelector(config));
            return new DynamicWebApiRequest();
        }
    }
}