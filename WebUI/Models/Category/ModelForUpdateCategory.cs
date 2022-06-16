using Microsoft.AspNetCore.Http;

namespace WebUI.Models.Category
{
    public class ModelForUpdateCategory
    {
        public int Id { get; set; }

        public string ElementId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile PictureRaw { get; set; }
    }
}
