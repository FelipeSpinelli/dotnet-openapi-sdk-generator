namespace OpenApiSdkGenerator.Extensions
{
    public static class UtilsExtensions
    {
        public static string ToPascalCase(this string value)
        {
            return $"{value.Substring(0, 1).ToUpper()}{value.Substring(1)}";
        }
    }
}
