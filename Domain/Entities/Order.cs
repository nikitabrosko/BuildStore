using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public bool IsDeliveryCompleted { get; set; }

        public DateTime Date { get; set; }

        public bool IsPaid { get; set; }

        public DateTime PaymentDate { get; set; }

        public Delivery Delivery { get; set; }

        public Payment Payment { get; set; }

        public Customer Customer { get; set; }
    }
}