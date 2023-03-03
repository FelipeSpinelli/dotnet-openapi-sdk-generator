namespace OpenApiSdkGenerator.Models
{
    public record SdkTypeOptions
    {
        public string Name { get; set; } = null!;
        public string ReplacementName { get; set; } = null!;
        public bool Ignore { get; set; }

        public string GetName()
        {
            return string.IsNullOrWhiteSpace(ReplacementName) ? Name : ReplacementName;
        }
    }
}
