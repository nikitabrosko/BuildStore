using System.Collections.Generic;

namespace Domain.Entities
{
    public abstract class CategoryBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<Subcategory> Subcategories { get; set; }

        protected CategoryBase()
        {
            Products = new HashSet<Product>();
            Subcategories = new HashSet<Subcategory>();
        }
    }
}