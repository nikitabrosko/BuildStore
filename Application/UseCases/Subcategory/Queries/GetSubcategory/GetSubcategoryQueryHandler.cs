﻿using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.UseCases.Subcategory.Queries.GetSubcategory
{
    public class GetSubcategoryQueryHandler : IRequestHandler<GetSubcategoryQuery, Domain.Entities.Subcategory>
    {
        private readonly IApplicationDbContext _context;

        public GetSubcategoryQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.Subcategory> Handle(GetSubcategoryQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Subcategories
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Subcategory), request.Id);
            }

            return entity;
        }
    }
}