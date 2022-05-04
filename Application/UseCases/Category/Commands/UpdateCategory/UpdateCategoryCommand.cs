using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Category.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile Picture { get; set; }

        public Domain.Entities.Subcategory Subcategory { get; set; }
    }
}