using Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WebUI.Models.Role
{
    public class ModelForRoles
    {
        public IEnumerable<Domain.Entities.Category> CategoriesForHeader { get; set; }

        public PaginatedList<IdentityRole> Roles { get; set; }

        public string SearchPattern { get; set; }
    }
}
