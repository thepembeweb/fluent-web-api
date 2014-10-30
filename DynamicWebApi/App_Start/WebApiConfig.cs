using System.Collections.Generic;
using System.Web.Http;
using DynamicWebApi.Models;
using WebApi.Dynamic.Configuration;

namespace DynamicWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var data = new List<Customer>
            {
                new Customer { Id = 1, FirstName = "Chuck", LastName = "Norris" },
                new Customer { Id = 2, FirstName = "Steven", LastName = "Seagal" }
            };

            // Web API configuration and services
            
            var request = config.UseDynamicWebApi();

            // Set up the data providers for the Customer API model
            request.OnGet<Customer>();
            //TODO Return a route something something instead of IApiModelBinder? Useful when you need to give only a set 
            //     of extension methods

            request.OnPost<Customer>();

            //config.For<Customer, int>()
            //    .Use(id => data.FirstOrDefault(c => c.Id == id))
            //    .Use(data.AsEnumerable);


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
