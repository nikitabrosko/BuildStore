using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models.Home
{
    public class ModelForError
    {
        public IEnumerable<Domain.Entities.Category> Categories { get; set; }

        public Domain.Entities.ShoppingCart ShoppingCart { get; set; }
    }
}
