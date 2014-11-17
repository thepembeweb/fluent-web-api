using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using DynamicWebApi.Models;
using FluentWebApi.Configuration;

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
            
            var request = config.UseFluentWebApi();

            // Add routes

            // GET /api/Customer
            request.
                OnGet<Customer>().
                Use(() => data);

            // GET /api/Customer/1
            //request.
            //    OnGet<Customer, int>().
            //    Use(id => data.FirstOrDefault(c => c.Id == id));


            // GET /api/Customer/1/Custom
            request.
                OnGet<Customer, int>(/* how to do this... */).
                ReplyWith((controller, id) =>
                {
                    var model = data.FirstOrDefault(c => c.Id == id);
                    if (model == null)
                    {
                        return controller.NotFound();
                    }

                    return controller.Ok(new { FullName = string.Format("{0} {1}", model.FirstName, model.LastName) });
                });

            // Set up the data providers for the Customer API model
            //request.OnGet<Customer>().Use(_ => new List<Customer>()).Reply();
            //request.OnGet<Customer, int>().Use(id => new Customer()).Reply();
            //request.OnPost<Customer>().Use(c => new Customer()).Reply();
            
            //TODO Return a route something something instead of IApiModelBinder? Useful when you need to give only a set 
            //     of extension methods

            //request.OnPost<Customer>();

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
