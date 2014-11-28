using FluentWebApi.Model;

namespace FluentWebApi.Configuration
{
    public interface IPutRoute<T, TKey>
        where T : class, IApiModel
    {
    }
}