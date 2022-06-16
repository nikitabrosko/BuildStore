namespace WebUI.Models.Order
{
    public class ModelForOrderPartial
    {
        public Domain.Entities.Order Order { get; set; }

        public string ElementId { get; set; }
    }
}
