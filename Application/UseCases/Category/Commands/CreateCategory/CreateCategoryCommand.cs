using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Category.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile Picture { get; set; }
    }
}