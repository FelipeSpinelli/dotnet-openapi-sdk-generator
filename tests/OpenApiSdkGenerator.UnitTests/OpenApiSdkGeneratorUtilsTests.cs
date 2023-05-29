using OpenApiSdkGenerator.Sample;

namespace OpenApiSdkGenerator.UnitTests;

public class OpenApiSdkGeneratorUtilsTests
{
    [Fact]
    public void GetRawString_GivenAObject_ShouldGenerateAsExpected()
    {
        var queryParams = new FindPetsQueryParams
        {
            Tags = new string[] { "abc", "def" },
            Limit = 10                
        };

        var rawQuery = queryParams.GetRawQueryString();

        Assert.Equal(rawQuery, "tags=abc&tags=def&limit=10&created.from=0001-01-01 00:00:00.000&created.to=0001-01-01 00:00:00.000");
    }
}