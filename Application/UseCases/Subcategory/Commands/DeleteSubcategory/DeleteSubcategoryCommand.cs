using MediatR;

namespace Application.UseCases.Subcategory.Commands.DeleteSubcategory
{
    public class DeleteSubcategoryCommand : IRequest<int>
    {
        public int Id { get; set; }

        public bool SubcategoriesDeletion { get; set; }

        public bool ProductsDeletion { get; set; }
    }
}