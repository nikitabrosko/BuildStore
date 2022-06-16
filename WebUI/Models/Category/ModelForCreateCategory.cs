using Microsoft.AspNetCore.Http;

namespace WebUI.Models.Category
{
    public class ModelForCreateCategory
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile PictureRaw { get; set; }
    }
}
