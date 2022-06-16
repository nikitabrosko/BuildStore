﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            IList<Domain.Entities.ProductsDictionary> productsDictionaries = new List<Domain.Entities.ProductsDictionary>();

            foreach (var productsDictionary in request.ProductsDictionary)
            {
                productsDictionaries.Add(await _context.ProductsDictionaries
                    .Include(p => p.Order)
                    .Include(p => p.Product)
                    .SingleAsync(p => p.Id.Equals(productsDictionary.Id), cancellationToken));
            }

            var orderEntity = new Domain.Entities.Order
            {
                Date = DateTime.Now,
                ProductsDictionary = productsDictionaries,
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
                Allowed = false
            };

            if (type.Equals(PaymentType.CreditCard))
            {
                paymentEntity.CreditCardNumber = creditCardNumber;
                paymentEntity.CardExpMonth = cardExpMonth;
                paymentEntity.CardExpYear = cardExpYear;
            }

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