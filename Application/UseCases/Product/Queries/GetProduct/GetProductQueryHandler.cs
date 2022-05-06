using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.UseCases.Product.Queries.GetProduct
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Domain.Entities.Product>
    {
        private readonly IApplicationDbContext _context;

        public GetProductQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products
                .FindAsync(new object[] {request.Id}, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            return entity;
        }
    }
}