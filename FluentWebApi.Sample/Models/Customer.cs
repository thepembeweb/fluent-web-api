using FluentWebApi.Model;

namespace FluentWebApi.Sample.Models
{
    public class Customer : IApiModel<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}