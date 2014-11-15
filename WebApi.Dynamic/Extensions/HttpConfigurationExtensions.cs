using System.Web.Http.Dispatcher;
using FluentWebApi.Configuration;
using FluentWebApi.Services;

// ReSharper disable once CheckNamespace
namespace System.Web.Http
{
    public static class HttpConfigurationExtensions
    {
        public static DynamicWebApiRequest UseFluentWebApi(this HttpConfiguration config)
        {
            config.Services.Replace(typeof(IHttpControllerSelector), new DynamicControllerSelector(config));
            return new DynamicWebApiRequest();
        }
    }
}