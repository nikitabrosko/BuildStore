using MediatR;

namespace Application.UseCases.Category.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest
    {
        public int Id { get; set; }

        public bool SubcategoriesDeletion { get; set; }

        public bool ProductsDeletion { get; set; }
    }
}