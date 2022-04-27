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

        public Category Category { get; set; }

        public float Discount { get; set; }

        public float Weight { get; set; }

        public byte[] Picture { get; set; }
    }
}