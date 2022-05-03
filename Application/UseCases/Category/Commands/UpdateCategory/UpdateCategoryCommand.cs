using System.ComponentModel.DataAnnotations.Schema;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Category.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [NotMapped]
        public IFormFile Picture { get; set; }

        public Domain.Entities.Subcategory SubCategory { get; set; }
    }
}