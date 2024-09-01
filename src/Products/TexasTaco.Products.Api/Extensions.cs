using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using TexasTaco.Products.Api.Endpoints;

namespace TexasTaco.Products.Api
{
    public static class Extensions
    {
        internal static IServiceCollection AddTexasTacoProductsApiVersioning(
            this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        internal static IServiceCollection AddEndpoints(
            this IServiceCollection services,
            Assembly assembly)
        {
            var serviceDescriptors = assembly
                .DefinedTypes
                .Where(t => t is { IsAbstract: false, IsInterface: false }
                    && t.IsAssignableTo(typeof(IEndpoint)))
                .Select(t => ServiceDescriptor.Transient(typeof(IEndpoint), t))
                .ToArray();

            services.TryAddEnumerable(serviceDescriptors);

            return services;
        }

        internal static IApplicationBuilder MapEndpoints(
            this WebApplication app,
            RouteGroupBuilder? routeGroupBuilder = null)
        {
            var endpoints = app.Services
                .GetRequiredService<IEnumerable<IEndpoint>>();

            IEndpointRouteBuilder endpointRouteBuilder =
                routeGroupBuilder is null ? app : routeGroupBuilder;

            foreach (var endpoint in endpoints)
            {
                endpoint.MapEndpoint(endpointRouteBuilder);
            }

            return app;
        }
    }
}
