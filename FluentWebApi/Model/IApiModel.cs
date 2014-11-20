namespace FluentWebApi.Model
{
    public interface IApiModel { }
    
    public interface IApiModel<out TKey> : IApiModel
    {
        TKey Id { get; }
    }
}