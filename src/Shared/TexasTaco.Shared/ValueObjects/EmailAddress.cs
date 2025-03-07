using System.Text.RegularExpressions;
using TexasTaco.Shared.Exceptions;

namespace TexasTaco.Shared.ValueObjects
{
    public record EmailAddress
    {
        private readonly static Regex EmailRegex = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

        public string Value { get; }

        public EmailAddress(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidEmailFormatException(
                    "Email address cannot contain whitespace characters and cannot be null.");
            }

            if (!EmailRegex.IsMatch(value))
            {
                throw new InvalidEmailFormatException($"Given email " +
                    $"value ({value}) is in wrong format.");
            }

            Value = value.ToLower();
        }

        public static explicit operator string(EmailAddress emailAddress)
            => emailAddress.Value;
    }
}
