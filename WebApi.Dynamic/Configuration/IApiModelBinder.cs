using WebApi.Dynamic.Model;

namespace WebApi.Dynamic.Configuration
{
    public interface IApiModelBinder<in T, in TKey> where T : class, IApiModel<TKey>
    {
        
    }
}