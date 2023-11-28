using System.Net.Mail;
using System.Text.RegularExpressions;
using TexasTaco.Authentication.Core.Exceptions;

namespace TexasTaco.Authentication.Core.ValueObjects
{
    public record EmailAddress
    {
        private readonly static Regex EmailRegex = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

        public string Value { get; }
        public EmailAddress(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidEmailFormatException(
                    "Email address cannot contain whitespace characters and cannot be null.");
            }

            if(!EmailRegex.IsMatch(value))
            {
                throw new InvalidEmailFormatException($"Given email " +
                    $"value ({value}) is in wrong format.");
            }

            Value = value;
        }
    }
}
