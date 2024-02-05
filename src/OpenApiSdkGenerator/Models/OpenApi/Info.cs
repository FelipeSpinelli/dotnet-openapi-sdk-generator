using Newtonsoft.Json.Linq;

namespace OpenApiSdkGenerator.Models.OpenApi;

public record Info
{
    private static readonly Info _empty = new Info
    {
        Description = null!,
        Title = null!,
        Version = V1
    };

    private const string V1 = "V1";
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Version { get; set; } = null!;

    public string GetVersion()
    {
        if (string.IsNullOrEmpty(Version))
        {
            return V1;
        }

        return $"V{Version.Split('.')[0]}";
    }

    public static Info LoadFrom(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return _empty;
        }

        var jObj = JObject.Parse(json);

        if (jObj is null || !jObj.ContainsKey("info"))
        {
            return _empty;
        }

        return jObj["info"]!.ToObject<Info>() ?? _empty;
    }
}
