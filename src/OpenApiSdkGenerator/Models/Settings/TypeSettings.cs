using OpenApiSdkGenerator.Models.Settings.Enumerators;

namespace OpenApiSdkGenerator.Models.Sdk;

public record TypeSettings
{
    private static readonly TypeSettings _default = new()
    {
        Ignore = false,
        Usage = TypeUsage.WithVersionSuffix
    };

    public string Name { get; set; } = null!;
    public string ReplacementName { get; set; } = null!;
    public bool Ignore { get; set; }
    public TypeUsage Usage { get; set; }

    public string GetName()
    {
        return string.IsNullOrWhiteSpace(ReplacementName) ? Name : ReplacementName;
    }

    public static TypeSettings Default(string name) => _default with { Name = name };
}