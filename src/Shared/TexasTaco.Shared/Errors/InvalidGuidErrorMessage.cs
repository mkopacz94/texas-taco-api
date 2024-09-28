namespace TexasTaco.Shared.Errors
{
    public static class InvalidGuidErrorMessage
    {
        public static ErrorMessage Create(string guidString, string objectName)
        {
            return new (ErrorsCodes.InvalidGuidFormat,
                $"The given {objectName} Id GUID \"{guidString}\" is invalid " +
                "and cannot be parsed. Provide GUID in a correct format.");
        }
    }
}
