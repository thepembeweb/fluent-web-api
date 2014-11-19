namespace FluentWebApi.Configuration
{
    public static class Resource
    {
        public static T Of<T>()
        {
            return default(T);
        }
    }
}