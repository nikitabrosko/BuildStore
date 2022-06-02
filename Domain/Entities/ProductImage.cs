using Microsoft.AspNetCore.Http;

namespace Domain.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }

        public Product Product { get; set; }

        public IFormFile PictureRaw { get; set; }

        public byte[] Picture { get; set; }
    }
}