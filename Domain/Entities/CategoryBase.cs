using System.Collections.Generic;

namespace Domain.Entities
{
    public class CategoryBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }

        public CategoryBase()
        {
            Products = new HashSet<Product>();
        }
    }
}