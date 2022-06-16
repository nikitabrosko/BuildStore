namespace WebUI.Models.User
{
    public class ModelForCreateUser
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
