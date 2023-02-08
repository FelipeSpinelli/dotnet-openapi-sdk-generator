namespace OpenApiSdkGenerator.Models
{
    public record Info
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Version { get; set; } = null!;
    }
}
