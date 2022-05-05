using Microsoft.AspNetCore.Http;

namespace Domain.Entities
{
    public class Category : CategoryBase
    {
        public IFormFile PictureRaw { get; set; }

        public byte[] Picture { get; set; }
    }
}