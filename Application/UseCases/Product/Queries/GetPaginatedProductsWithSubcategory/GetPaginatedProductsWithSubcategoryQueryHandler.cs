﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Product.Queries.GetPaginatedProductsWithSubcategory
{
    public class GetPaginatedProductsWithSubcategoryQueryHandler : IRequestHandler<GetPaginatedProductsWithSubcategoryQuery, PaginatedList<Domain.Entities.Product>>
    {
        private readonly IApplicationDbContext _context;

        public GetPaginatedProductsWithSubcategoryQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Domain.Entities.Product>> Handle(GetPaginatedProductsWithSubcategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .OfType<Domain.Entities.Category>()
                .Include(c => c.Subcategories)
                    .ThenInclude(s => s.Subcategories)
                .SingleOrDefaultAsync(c => c.Id.Equals(request.SubcategoryId), cancellationToken);

            if (category is null)
            {
                var subcategory = await _context.Categories
                .OfType<Domain.Entities.Subcategory>()
                .Include(s => s.Subcategories)
                .Include(s => s.Products)
                .SingleOrDefaultAsync(s => s.Id.Equals(request.SubcategoryId), cancellationToken);

                var products = _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .Where(p => p.Category is Domain.Entities.Subcategory)
                    .Where(p => p.Category.Id.Equals(subcategory.Id));

                products = subcategory.Subcategories
                    .Select(subcategorySubcategory => _context.Products
                        .Include(p => p.Category)
                        .Include(p => p.Images)
                        .Where(p => p.Category is Domain.Entities.Subcategory)
                        .Where(p => p.Category.Id.Equals(subcategorySubcategory.Id)))
                    .Aggregate(products, (current, subcategoryProducts) => current
                        .Union(subcategoryProducts));

                return await PaginatedList<Domain.Entities.Product>.CreateAsync(products, request.PageNumber, request.PageSize);
            }
            else
            {
                var products = category.Subcategories
                    .Select(subcategory => _context.Products
                        .Include(p => p.Category)
                        .Include(p => p.Images)
                        .Where(p => p.Category is Domain.Entities.Subcategory)
                        .Where(p => p.Category.Id.Equals(subcategory.Id)))
                    .Aggregate((current, subcategoryProducts) => current
                        .Union(subcategoryProducts));

                products = category.Subcategories
                    .Select(subcategory => _context.Categories
                        .OfType<Domain.Entities.Subcategory>()
                        .Include(s => s.Products)
                        .ThenInclude(p => p.Images)
                        .Where(s => s.Category.Id.Equals(subcategory.Id)))
                    .Aggregate((current, subSubcategory) => current
                        .Union(subSubcategory)).ToList()
                        .Select(subSubcategory => _context.Products
                        .Include(p => p.Category)
                        .Include(p => p.Images)
                        .Where(p => p.Category is Domain.Entities.Subcategory)
                        .Where(p => p.Category.Id.Equals(subSubcategory.Id)))
                    .Aggregate(products, (current, subSubcategoryProducts) => current
                        .Union(subSubcategoryProducts));

                return await PaginatedList<Domain.Entities.Product>.CreateAsync(products, request.PageNumber, request.PageSize);
            }
        }
    }
}