using Application.Common.Models;
using Application.UseCases.Identity.User.Queries.GetUsersWithRolesWithPagination;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WebUI.Models.User
{
    public class ModelForUsers
    {
        public IEnumerable<Domain.Entities.Category> CategoriesForHeader { get; set; }

        public PaginatedList<UserWithRolesDto> Users { get; set; }

        public IEnumerable<IdentityRole> Roles { get; set; }

        public string SearchPattern { get; set; }
    }
}
