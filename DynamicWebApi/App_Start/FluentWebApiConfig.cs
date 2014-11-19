using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DynamicWebApi.Models;
using FluentWebApi.Configuration;

namespace DynamicWebApi
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
                OnGet<Customer>().
                Use(() => data);

            // GET /api/Customer/1
            request.
                OnGet<Customer, int>().
                Use(id => data.FirstOrDefault(c => c.Id == id));

            // GET /api/Customer/1/Fullname
            request.
                OnGet<Customer, int>(new { RouteTemplate = "api/Customer/{id}/Fullname", RouteName = "GetFullNameFromCustomer" }).
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