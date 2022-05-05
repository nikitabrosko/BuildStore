namespace Domain.Entities
{
    public class Subcategory : CategoryBase
    {
        public CategoryBase Category { get; set; }
    }
}