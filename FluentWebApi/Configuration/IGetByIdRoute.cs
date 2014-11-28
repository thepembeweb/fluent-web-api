using FluentWebApi.Model;

namespace FluentWebApi.Configuration
{
    public interface IGetByIdRoute<T, TKey>
        where T : class, IApiModel
    {
    }
}