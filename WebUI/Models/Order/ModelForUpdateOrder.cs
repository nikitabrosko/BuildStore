namespace WebUI.Models.Order
{
    public class ModelForUpdateOrder
    {
        public int Id { get; set; }

        public string ElementId { get; set; }

        public bool IsDeliveryComplete { get; set; }

        public bool IsPaymentAllowed { get; set; }
    }
}
