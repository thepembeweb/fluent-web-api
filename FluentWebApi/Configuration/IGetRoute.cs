using FluentWebApi.Model;

namespace FluentWebApi.Configuration
{
    public interface IGetRoute<T>
        where T : class, IApiModel
    {
    }
}