using FluentWebApi.Model;

namespace FluentWebApi.Configuration
{
    public interface IPostRoute<T>
        where T : class, IApiModel
    {
    }
}