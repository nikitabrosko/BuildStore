using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Order.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateOrderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = new Domain.Entities.Order
            {
                Date = DateTime.Now,
                ProductsDictionary = request.ProductsDictionary,
                Customer = request.Customer
            };

            await _context.Orders.AddAsync(orderEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var deliveryEntity = await CreateDeliveryAsync(orderEntity, 
                request.DeliveryType, cancellationToken);

            orderEntity.Delivery = deliveryEntity;
            orderEntity.DeliveryId = deliveryEntity.Id;

            var paymentEntity = await CreatePaymentAsync(orderEntity, request.PaymentType, 
                request.CreditCardNumber, request.CardExpMonth, 
                request.CardExpYear, cancellationToken);

            orderEntity.Payment = paymentEntity;
            orderEntity.PaymentId = paymentEntity.Id;

            await _context.SaveChangesAsync(cancellationToken);

            return orderEntity.Id;
        }

        private async Task<Domain.Entities.Payment> CreatePaymentAsync(Domain.Entities.Order order, 
            PaymentType type, string creditCardNumber, int cardExpMonth, 
            int cardExpYear, CancellationToken cancellationToken)
        {
            var paymentEntity = new Domain.Entities.Payment
            {
                Order = order,
                OrderId = order.Id,
                Type = type,
                CreditCardNumber = creditCardNumber,
                CardExpMonth = cardExpMonth,
                CardExpYear = cardExpYear,
                Allowed = false
            };

            await _context.Payments.AddAsync(paymentEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return paymentEntity;
        }

        private async Task<Domain.Entities.Delivery> CreateDeliveryAsync(Domain.Entities.Order order, 
            DeliveryType type, CancellationToken cancellationToken)
        {
            var deliveryEntity = new Domain.Entities.Delivery
            {
                Order = order,
                OrderId = order.Id,
                Type = type,
                Fulfilled = false
            };

            await _context.Deliveries.AddAsync(deliveryEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return deliveryEntity;
        }
    }
}