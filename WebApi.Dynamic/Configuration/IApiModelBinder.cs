using WebApi.Dynamic.Model;

namespace WebApi.Dynamic.Configuration
{
    public interface IApiModelBinder<in T> where T : class, IApiModel
    {
        
    }
}