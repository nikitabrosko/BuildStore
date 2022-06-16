using Microsoft.AspNetCore.Identity;

namespace WebUI.Models.Role
{
    public class ModelForRolePartial
    {
        public IdentityRole Role { get; set; }

        public string ElementId { get; set; }
    }
}
