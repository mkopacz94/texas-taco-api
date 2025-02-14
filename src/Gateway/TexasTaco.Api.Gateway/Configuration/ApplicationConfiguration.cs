using System.ComponentModel.DataAnnotations;

namespace TexasTaco.Api.Gateway.Configuration
{
    internal record ApplicationConfiguration
    {
        [Required]
        public CorsConfiguration Cors { get; set; } = new();
    }
}
