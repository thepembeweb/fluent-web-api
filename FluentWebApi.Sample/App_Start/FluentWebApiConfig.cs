using System;
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
                ReadUsing(() => data);
                // And reply using the default mechanism


            // GET /api/Customer/1
            request.
                // When you perform a GET with an int ID on the Customer resource
                OnGet((int id) => Resource.Of<Customer>()).
                // Use this method to retrieve the customer
                ReadUsing(id => data.FirstOrDefault(c => c.Id == id));
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

            // POST /api/Customer
            request.
                OnPost(Resource.Of<Customer>()).
                CreateUsing(customer => data.Add(customer));

            // PUT /api/Customer/1
            request.
                OnPut((int id) => Resource.Of<Customer>()).
                UpdateUsing((id, customer) =>
                {
                    var idx = data.FindIndex(c => c.Id == id);
                    if (idx != -1)
                    {
                        data[idx].FirstName = customer.FirstName;
                        data[idx].LastName = customer.LastName;
                    }
                });


            // DELETE /api/Customer/1
            request.
                OnDelete((int id) => Resource.Of<Customer>()).
                DeleteUsing(id => data.RemoveAll(c => c.Id == id));
        }
    }
}