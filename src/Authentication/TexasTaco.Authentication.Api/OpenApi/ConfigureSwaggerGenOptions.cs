using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TexasTaco.Authentication.Api.OpenApi
{
    internal class ConfigureSwaggerGenOptions(
        IApiVersionDescriptionProvider _apiVersionDescriptionProvider) 
        : IConfigureNamedOptions<SwaggerGenOptions>
    {
        public void Configure(string? name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                var openApiInfo = new OpenApiInfo
                {
                    Title = $"TexasTaco.Authentication.Api v{description.ApiVersion}",
                    Version = description.ApiVersion.ToString()
                };

                options.SwaggerDoc(description.GroupName, openApiInfo);
            }    
        }
    }
}
