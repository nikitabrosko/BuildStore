using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int QuantityPerUnit { get; set; }

        public Supplier Supplier { get; set; }

        public CategoryBase Category { get; set; }

        public float Discount { get; set; }

        public float Weight { get; set; }

        public IFormFile PictureRaw { get; set; }

        public byte[] Picture { get; set; }
    }
}