using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Product.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int QuantityPerUnit { get; set; }

        public float Discount { get; set; }

        public float Weight { get; set; }

        public int? CategoryId { get; set; }

        public int? SupplierId { get; set; }

        public IFormFile[] Pictures { get; set; }
    }
}