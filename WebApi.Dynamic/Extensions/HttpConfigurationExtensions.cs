using System.Web.Http.Dispatcher;
using WebApi.Dynamic.Configuration;
using WebApi.Dynamic.Model;
using WebApi.Dynamic.Services;

// ReSharper disable once CheckNamespace
namespace System.Web.Http
{
    public static class HttpConfigurationExtensions
    {
        public static void UseDynamicWebApi(this HttpConfiguration config)
        {
            config.Services.Replace(typeof(IHttpControllerSelector), new DynamicControllerSelector(config));
        }

        public static IApiModelBinder<T, TKey> For<T, TKey>(this HttpConfiguration config)
            where T : class, IApiModel<TKey>
        {
            return ApiModelBinder<T, TKey>.Instance;
        }
    }
}