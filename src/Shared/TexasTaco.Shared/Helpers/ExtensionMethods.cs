namespace TexasTaco.Shared.Helpers
{
    public static class ExtensionMethods
    {
        public static string CapitalizeFirstCharacter(this string value)
            => char.ToUpper(value[0]) + value[1..];
    }
}
