namespace TexasTaco.Authentication.Core.Entities
{
    public sealed class SmtpOptions
    {
        public string? SourceAddress { get; set; }
        public string? Host { get; set; }
        public string? Password { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
    }
}
