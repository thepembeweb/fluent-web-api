using System;
using System.Collections.Generic;
using WebApi.Dynamic.Model;

// ReSharper disable once CheckNamespace
namespace WebApi.Dynamic.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IApiModelBinder<T, TKey> Use<T, TKey>(this IApiModelBinder<T, TKey> apiModelBinder, Func<TKey, T> func)
            where T : class, IApiModel<TKey>
        {
            if (apiModelBinder == null)
            {
                throw new ArgumentNullException("apiModelBinder");
            }

            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            var amb = apiModelBinder as ApiModelBinder<T, TKey>;
            if (amb.HasCustomDataProvider)
            {
                throw new InvalidOperationException("A custom data provider has been configured for this IApiModel.");
            }

            amb.AddDataProvider(func);
            return amb;
        }

        public static IApiModelBinder<T, TKey> Use<T, TKey>(this IApiModelBinder<T, TKey> apiModelBinder, Func<IEnumerable<T>> func)
            where T : class, IApiModel<TKey>
        {
            if (apiModelBinder == null)
            {
                throw new ArgumentNullException("apiModelBinder");
            }

            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            var amb = apiModelBinder as ApiModelBinder<T, TKey>;
            if (amb.HasCustomDataProvider)
            {
                throw new InvalidOperationException("A custom data provider has been configured for this IApiModel.");
            }

            amb.AddDataProvider(func);
            return amb;
        }

        public static IApiModelBinder<T, TKey> Use<T, TKey>(this IApiModelBinder<T, TKey> apiModelBinder, IDataProvider<T, TKey> dataProvider)
            where T : class, IApiModel<TKey>
        {
            if (apiModelBinder == null)
            {
                throw new ArgumentNullException("apiModelBinder");
            }

            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }

            var amb = apiModelBinder as ApiModelBinder<T, TKey>;
            amb.SetCustomDataProvider(dataProvider);
            return amb;
        }
    }
}