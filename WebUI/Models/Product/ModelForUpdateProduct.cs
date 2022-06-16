using Microsoft.AspNetCore.Http;

namespace WebUI.Models.Product
{
    public class ModelForUpdateProduct
    {
        public int Id { get; set; }

        public string ElementId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int QuantityPerUnit { get; set; }

        public float Discount { get; set; }

        public float Weight { get; set; }

        public string CategoryId { get; set; }

        public string SupplierId { get; set; }

        public IFormFile[] Pictures { get; set; }
    }
}
