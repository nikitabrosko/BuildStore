using Application.UseCases.Identity.User.Queries.GetUsersWithRolesWithPagination;

namespace WebUI.Models.User
{
    public class ModelForUserPartial
    {
        public UserWithRolesDto User { get; set; }

        public string ElementId { get; set; }
    }
}
