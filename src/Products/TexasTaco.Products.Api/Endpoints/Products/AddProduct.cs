﻿using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Products.Core.DTO;
using TexasTaco.Products.Core.Entities;
using TexasTaco.Products.Core.Repositories;
using TexasTaco.Products.Core.ValueObjects;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.Authentication.Attributes;

namespace TexasTaco.Products.Api.Endpoints.Products
{
    internal class AddProduct : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("", [AuthorizeRole(Role.Admin)] async (
                [FromBody] ProductInputDto productInputDto,
                [FromServices] IProductsRepository productsRepository) =>
            {
                var product = new Product(
                    productInputDto.Name,
                    productInputDto.ShortDescription,
                    productInputDto.Recommended,
                    productInputDto.Price,
                    new PictureId(Guid.Parse(productInputDto.PictureId)),
                    new CategoryId(Guid.Parse(productInputDto.CategoryId)));

                await productsRepository.AddAsync(product);

                return Results.CreatedAtRoute(Routes.GetProduct, new { Id = product.Id.Value });
            })
            .RequireAuthorization()
            .WithTags(Tags.Products)
            .HasApiVersion(new ApiVersion(1))
            .Produces(StatusCodes.Status201Created, typeof(ProductDto))
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        }
    }
}
