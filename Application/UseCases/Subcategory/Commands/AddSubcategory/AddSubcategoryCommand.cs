using MediatR;

namespace Application.UseCases.Subcategory.Commands.AddSubcategory
{
    public class AddSubcategoryCommand : IRequest<int>
    {
        public int SubcategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}