using System.Linq;
using FluentWebApi.Model;
using FluentWebApi.Routing;

namespace FluentWebApi.Configuration
{
    class ApiModelBinder<T> : IApiModelBinder<T> where T : class, IApiModel
    {
        private static readonly ApiModelBinder<T> _instance;

        static ApiModelBinder()
        {
            _instance = new ApiModelBinder<T>(); //Create a default instance per bound IApiModel
        }

        private ApiModelBinder()
        {
            
        }

        public static ApiModelBinder<T> Instance
        {
            get
            {
                return _instance;
            }
        }

        public Route<T> AddRoute(HttpVerb httpVerb)
        {
            var route = new Route<T>(httpVerb);
            Storage<T>.Routes.Add(route);

            return route;
        }

        public Route<T> GetRoute(HttpVerb httpVerb)
        {
            return Storage<T>.Routes.FirstOrDefault(r => r.HttpVerb == httpVerb && r.KeyType == null);
        } 
        
        public Route<T> AddRoute<TData>(HttpVerb httpVerb)
        {
            var route = new Route<T>(httpVerb, typeof(TData));
            Storage<T>.Routes.Add(route);

            return route;
        }

        public Route<T> GetRoute<TData>(HttpVerb httpVerb)
        {
            return Storage<T>.Routes.FirstOrDefault(
                r => r.HttpVerb == httpVerb && r.KeyType == typeof(TData));
        }
    }
}