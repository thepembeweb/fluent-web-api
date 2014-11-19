using FluentWebApi.Configuration;

// ReSharper disable once CheckNamespace
namespace System.Web.Http
{
    public static class HttpConfigurationExtensions
    {
        public static FluentWebApiRequest UseFluentWebApi(this HttpConfiguration config)
        {
            return new FluentWebApiRequest();
        }
    }
}