using Newtonsoft.Json.Linq;

namespace OpenApiSdkGenerator.Models;

public record Info
{
    private const string V1 = "v1";
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Version { get; set; } = null!;

    public string GetVersion()
    {
        if (string.IsNullOrEmpty(Version))
        {
            return V1;
        }

        return Version.Replace(".","_");
    }

    public static Info LoadFrom(string json)
    {
        var empty = new Info
        {
            Description = null!,
            Title = null!,
            Version = V1
        };

        if (string.IsNullOrEmpty(json))
        {
            return empty;
        }
        
        var jObj = JObject.Parse(json);

        if (jObj is null || !jObj.ContainsKey("info"))
        {
            return empty;
        }

        return jObj["info"]!.ToObject<Info>() ?? empty;
    }
}
