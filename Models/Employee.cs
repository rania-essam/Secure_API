using Microsoft.AspNetCore.Identity;

namespace WebAPI_DAY3.Models
{
    public class Employee : IdentityUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public decimal Age { get; set; }
    }
}
