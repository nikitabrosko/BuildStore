using MediatR;

namespace Application.UseCases.Category.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }
    }
}