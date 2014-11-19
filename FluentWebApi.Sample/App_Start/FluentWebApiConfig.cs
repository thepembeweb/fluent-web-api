using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FluentWebApi.Configuration;
using FluentWebApi.Sample.Models;

namespace FluentWebApi.Sample
{
    public static class FluentWebApiConfig
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

            // Add Fluent Web API routes

            // GET /api/Customer
            request.
                // When you perform a GET on the Customer resource
                OnGet(() => Resource.Of<Customer>()).
                // Use this method to retrieve the customers
                Use(() => data);
                // And reply using the default mechanism


            // GET /api/Customer/1
            request.
                // When you perform a GET with an int ID on the Customer resource
                OnGet((int id) => Resource.Of<Customer>()).
                // Use this method to retrieve the customer
                Use(id => data.FirstOrDefault(c => c.Id == id));
                // And reply using the default mechanism


            // GET /api/Customer/1/Fullname
            request.
                OnGet((int id) => Resource.Of<Customer>(), new { RouteTemplate = "api/Customer/{id}/Fullname", RouteName = "GetFullNameFromCustomer" }).
                ReplyWith((controller, id) =>
                {
                    var model = data.FirstOrDefault(c => c.Id == id);
                    if (model == null)
                    {
                        return controller.NotFound();
                    }

                    return controller.Ok(new { FullName = string.Format("{0} {1}", model.FirstName, model.LastName) });
                });
        }
    }
}