using MediatR;

namespace Application.UseCases.Subcategory.Commands.UpdateSubcategory
{
    public class UpdateSubcategoryCommand : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? NewCategoryId { get; set; }
    }
}