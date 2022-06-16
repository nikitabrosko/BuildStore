namespace WebUI.Models.User
{
    public class ModelForUpdateUserRoles
    {
        public string ElementId { get; set; }

        public string UserId { get; set; }

        public string[] Roles { get; set; }
    }
}
