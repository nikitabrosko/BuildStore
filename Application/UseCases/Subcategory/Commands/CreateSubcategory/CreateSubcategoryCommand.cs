using MediatR;

namespace Application.UseCases.Subcategory.Commands.CreateSubcategory
{
    public class CreateSubcategoryCommand : IRequest
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}