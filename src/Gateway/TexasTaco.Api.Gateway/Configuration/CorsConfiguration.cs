using System.ComponentModel.DataAnnotations;

namespace TexasTaco.Api.Gateway.Configuration
{
    internal record CorsConfiguration
    {
        [Required]
        public List<string> AllowedOrigins { get; set; } = [];
        [Required]
        public List<string> AllowedHeaders { get; set; } = [];
        [Required]
        public List<string> AllowedMethods { get; set; } = [];
    }
}
