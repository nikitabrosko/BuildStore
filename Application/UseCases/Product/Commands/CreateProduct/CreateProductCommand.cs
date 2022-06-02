using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Product.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int QuantityPerUnit { get; set; }

        public int SupplierId { get; set; }

        public string CategoryName { get; set; }

        public float Discount { get; set; }

        public float Weight { get; set; }

        public IFormFile[] Pictures { get; set; }
    }
}