using WebApi.Dynamic.Model;
using WebApi.Dynamic.Routing;

// ReSharper disable once CheckNamespace
namespace WebApi.Dynamic.Configuration
{
    public static class ConfigurationExtensions
    {

        //public static IApiModelBinder<T, TKey> For<T, TKey>(this HttpConfiguration config)
        //    where T : class, IApiModel<TKey>
        //{
        //    return ApiModelBinder<T, TKey>.Instance;
        //}

        //public static IApiModelBinder<T> Get<T, TKey>(this HttpConfiguration config)
        //    where T : class, IApiModel<TKey>
        //{
        //    return ApiModelBinder<T, TKey>.Instance;
        //}

        public static IApiModelBinder<T> OnGet<T>(this DynamicWebApiRequest request) where T : class, IApiModel
        {
            return OnVerb<T>(HttpVerb.Get);
        }

        public static IApiModelBinder<T> OnPost<T>(this DynamicWebApiRequest request) where T : class, IApiModel
        {
            return OnVerb<T>(HttpVerb.Post);
        }

        public static IApiModelBinder<T> OnPut<T>(this DynamicWebApiRequest request) where T : class, IApiModel
        {
            return OnVerb<T>(HttpVerb.Put);
        }

        public static IApiModelBinder<T> OnDelete<T>(this DynamicWebApiRequest request) where T : class, IApiModel
        {
            return OnVerb<T>(HttpVerb.Delete);
        }

        public static IApiModelBinder<T> OnCustomHttpVerb<T>(this DynamicWebApiRequest request, string verb) where T : class, IApiModel
        {
            return OnVerb<T>(HttpVerb.GetVerb(verb));
        }

        private static IApiModelBinder<T> OnVerb<T>(HttpVerb verb) where T : class, IApiModel
        {
            var apiModelBinder = ApiModelBinder<T>.Instance;
            apiModelBinder.AddRoute(verb);

            return apiModelBinder;
        }

        //public static IApiModelBinder<T, TKey> Use<T, TKey>(this IApiModelBinder<T, TKey> apiModelBinder, Func<TKey, T> func)
        //    where T : class, IApiModel<TKey>
        //{
        //    if (apiModelBinder == null)
        //    {
        //        throw new ArgumentNullException("apiModelBinder");
        //    }

        //    if (func == null)
        //    {
        //        throw new ArgumentNullException("func");
        //    }

        //    var amb = apiModelBinder as ApiModelBinder<T, TKey>;
        //    if (amb.HasCustomDataProvider)
        //    {
        //        throw new InvalidOperationException("A custom data provider has been configured for this IApiModel.");
        //    }

        //    amb.AddDataProvider(func);
        //    return amb;
        //}

        //public static IApiModelBinder<T, TKey> Use<T, TKey>(this IApiModelBinder<T, TKey> apiModelBinder, Func<IEnumerable<T>> func)
        //    where T : class, IApiModel<TKey>
        //{
        //    if (apiModelBinder == null)
        //    {
        //        throw new ArgumentNullException("apiModelBinder");
        //    }

        //    if (func == null)
        //    {
        //        throw new ArgumentNullException("func");
        //    }

        //    var amb = apiModelBinder as ApiModelBinder<T, TKey>;
        //    if (amb.HasCustomDataProvider)
        //    {
        //        throw new InvalidOperationException("A custom data provider has been configured for this IApiModel.");
        //    }

        //    amb.AddDataProvider(func);
        //    return amb;
        //}

        //public static IApiModelBinder<T, TKey> Use<T, TKey>(this IApiModelBinder<T, TKey> apiModelBinder, IDataProvider<T, TKey> dataProvider)
        //    where T : class, IApiModel<TKey>
        //{
        //    if (apiModelBinder == null)
        //    {
        //        throw new ArgumentNullException("apiModelBinder");
        //    }

        //    if (dataProvider == null)
        //    {
        //        throw new ArgumentNullException("dataProvider");
        //    }

        //    var amb = apiModelBinder as ApiModelBinder<T, TKey>;
        //    amb.SetCustomDataProvider(dataProvider);
        //    return amb;
        //}
    }
}