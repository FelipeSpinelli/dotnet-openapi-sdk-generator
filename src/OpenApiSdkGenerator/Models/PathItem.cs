namespace OpenApiSdkGenerator.Models
{
    public record PathItem
    {
        public Operation? Get { get; set; }
        public Operation? Post { get; set; }
        public Operation? Put { get; set; }
        public Operation? Patch { get; set; }
        public Operation? Delete { get; set; }
        public Operation? Head { get; set; }
        public Operation? Options { get; set; }
        public Operation? Trace { get; set; }
    }
}
