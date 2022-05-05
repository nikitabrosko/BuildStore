﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.UseCases.Category.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = _context.Categories
                .OfType<Domain.Entities.Category>()
                .SingleOrDefault(c => c.Id.Equals(request.Id));

            if (entity is null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }

            if (!request.Name.Equals(entity.Name))
            {
                var checkForExistsEntity = _context.Categories
                    .Any(category => category.Name.Equals(request.Name));

                if (checkForExistsEntity)
                {
                    throw new ItemExistsException($"{nameof(Category)} with that name is already exists!");
                }
            }

            entity.Name = request.Name;
            entity.Description = request.Description;

            if (request.Picture != null)
            {
                entity.Picture = new byte[request.Picture.Length];

                await using var stream = request.Picture.OpenReadStream();
                var count = stream.Read(entity.Picture, 0, (int)request.Picture.Length);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}