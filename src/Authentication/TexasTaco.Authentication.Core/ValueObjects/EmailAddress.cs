using System.Net.Mail;
using System.Text.RegularExpressions;
using TexasTaco.Authentication.Core.Exceptions;

namespace TexasTaco.Authentication.Core.ValueObjects
{
    public record EmailAddress
    {
        private const string EmailRegexPattern = "\"^\\\\S+@\\\\S+\\\\.\\\\S+$\"";

        public string Value { get; }
        public EmailAddress(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidEmailAddressFormatException(
                    "Email address cannot contain whitespace characters and cannot be null.");
            }

            if(!MailAddress.TryCreate(value, out _))
            {
                throw new InvalidEmailAddressFormatException($"Given email " +
                    $"value ({value}) is in wrong format.");
            }

            if (value.Length > 100)
            {
                throw new InvalidEmailAddressFormatException($"Email address value " +
                    $"must be shorter than 100 characters.");
            }

            Value = value;
        }
    }
}
