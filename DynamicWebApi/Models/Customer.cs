using WebApi.Dynamic.Model;

namespace DynamicWebApi.Models
{
    public class Customer : IApiModel
    {
        public Customer()
        {
            FirstName = "Wesley";
            LastName = "Cabus";
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}