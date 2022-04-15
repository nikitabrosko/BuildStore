using System.Collections.Generic;

namespace Domain.Entities
{
    public class Supplier
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string PhoneNumber { get; set; }

        public string EMail { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}