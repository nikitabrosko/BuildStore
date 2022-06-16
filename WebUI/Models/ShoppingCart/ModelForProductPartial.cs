using Domain.Entities;

namespace WebUI.Models.ShoppingCart
{
    public class ModelForProductPartial
    {
        public ProductsDictionary ProductsDictionary { get; set; }

        public string ElementId { get; set; }
    }
}
