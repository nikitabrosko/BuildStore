using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities
{
    public class Category : CategoryBase
    {
        [NotMapped]
        public IFormFile PictureRaw { get; set; }

        public byte[] Picture { get; set; }

        public ICollection<Subcategory> Subcategories { get; set; }

        public Category()
        {
            Subcategories = new HashSet<Subcategory>();
        }
    }
}