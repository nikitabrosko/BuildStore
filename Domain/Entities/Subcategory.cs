namespace Domain.Entities
{
    public class Subcategory : CategoryBase
    {
        public Category Category { get; set; }
    }
}