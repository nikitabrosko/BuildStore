using System.Collections.Generic;

namespace Application.UseCases.Identity.User.Queries.GetUsersWithRolesWithPagination
{
    public class UserWithRolesDto
    {
        public Domain.IdentityEntities.User User { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}