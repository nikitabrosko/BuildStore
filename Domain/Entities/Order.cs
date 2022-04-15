using System;

namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public bool IsDeliveryCompleted { get; set; }

        public DateTime Date { get; set; }

        public bool IsPaid { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}