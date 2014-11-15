using FluentWebApi.Model;

namespace FluentWebApi.Configuration
{
    public interface IApiModelBinder<in T> where T : class, IApiModel
    {
        
    }
}