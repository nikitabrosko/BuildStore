using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        
        public DateTime Date { get; set; }

        public Delivery Delivery { get; set; }

        public int? DeliveryId { get; set; }

        public Payment Payment { get; set; }
        
        public int? PaymentId { get; set; }

        public Customer Customer { get; set; }

        public int CustomerId { get; set; }

        public ICollection<ProductsDictionary> ProductsDictionary { get; set; }

        public Order()
        {
            ProductsDictionary = new HashSet<ProductsDictionary>();
        }
    }
}