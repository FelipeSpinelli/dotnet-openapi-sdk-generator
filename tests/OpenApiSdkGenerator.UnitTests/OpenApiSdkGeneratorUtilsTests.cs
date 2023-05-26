using OpenApiSdkGenerator.Sample;
using System.Globalization;

namespace OpenApiSdkGenerator.UnitTests
{
    public class OpenApiSdkGeneratorUtilsTests
    {
        [Fact]
        public void GetRawString_GivenAObject_ShouldGenerateAsExpected()
        {
            var queryParams = new OpenApiSdkGenerator.Sample.FindPetsQueryParams
            {
                Tags = new string[] { "abc", "def" },
                Limit = 10                
            };

            var rawQuery = queryParams.GetRawQueryString();

            Assert.Equal(rawQuery, "tags=abc&tags=def&limit=10&created.from=01/01/0001 00:00:00&created.to=01/01/0001 00:00:00");
        }
    }
}