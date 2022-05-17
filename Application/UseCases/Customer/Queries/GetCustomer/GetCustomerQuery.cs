﻿using MediatR;

namespace Application.UseCases.Customer.Queries.GetCustomer
{
    public class GetCustomerQuery : IRequest<Domain.Entities.Customer>
    {
        public string UserName { get; set; }
    }
}