namespace WebUI.Models.Supplier
{
    public class ModelForCreateSupplier
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}
