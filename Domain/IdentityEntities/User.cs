using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain.IdentityEntities
{
    public class User : IdentityUser
    {
        public Customer Customer { get; set; }
    }
}