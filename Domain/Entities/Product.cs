using System.Collections.Generic;

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

        public ICollection<ProductImage> Images { get; set; }

        public ICollection<ProductsDictionary> ProductsDictionaries { get; set; }

        public Product()
        {
            Images = new HashSet<ProductImage>();
            ProductsDictionaries = new HashSet<ProductsDictionary>();
        }
    }
}