using CinemaApp.Domain.DomainModels;
using Microsoft.AspNetCore.Identity;

namespace CinemaApp.Domain.Identity
{
    public class CinemaAppApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }

        public virtual ShoppingCart UserCart { get; set; }
    }
}
