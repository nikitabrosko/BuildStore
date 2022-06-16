namespace WebUI.Models.Subcategory
{
    public class ModelForCreateSubcategory
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CategoryId { get; set; }
    }
}
