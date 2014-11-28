using FluentWebApi.Model;

namespace FluentWebApi.Configuration
{
    public interface IDeleteRoute<T, TKey>
        where T : class, IApiModel
    {
    }
}