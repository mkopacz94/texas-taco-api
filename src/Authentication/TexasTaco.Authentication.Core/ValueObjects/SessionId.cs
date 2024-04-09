using System.Text.Json.Serialization;

namespace TexasTaco.Authentication.Core.ValueObjects
{
    public record SessionId(Guid Value)
    {
        [JsonConstructor]
        private SessionId() : this(Guid.NewGuid())
        {
        }
    }
}
