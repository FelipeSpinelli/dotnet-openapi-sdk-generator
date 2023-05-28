using OpenApiSdkGenerator.Sample;
using RestEase;

namespace OpenApiSdkGenerator.UnitTests;

public class OpenApiSdkGeneratorRequestTests
{
    [Fact]
    public async Task X()
    {
        var client = new RestClient("http://localhost", RequestModifier).For<IPetApi>();
        var pets = client.FindPets(new FindPetsQueryParams { CreatedTo = DateTime.UtcNow }, CancellationToken.None);

    }

    private async Task RequestModifier(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var requestString = await request?.Content?.ReadAsStringAsync(cancellationToken) ?? string.Empty;
    }
}
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

        Assert.Equal(rawQuery, "tags=abc&tags=def&limit=10&created.from=01/01/0001 00:00:00&created.to=01/01/0001 00:00:00");
    }
}