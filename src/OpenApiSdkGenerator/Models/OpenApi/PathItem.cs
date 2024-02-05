using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenApiSdkGenerator.Models.OpenApi
{
    public record PathItem
    {
        [JsonProperty("_reference")]
        public string Reference { get; set; } = null!;

        [JsonProperty("summary")]
        public string Summary { get; set; } = null!;

        [JsonProperty("description")]
        public string Description { get; set; } = null!;

        [JsonProperty("parameters")]
        public Parameter[] Parameters { get; set; } = null!;

        [JsonProperty("get")]
        public Operation? Get { get; set; }

        [JsonProperty("post")]
        public Operation? Post { get; set; }

        [JsonProperty("put")]
        public Operation? Put { get; set; }

        [JsonProperty("patch")]
        public Operation? Patch { get; set; }

        [JsonProperty("delete")]
        public Operation? Delete { get; set; }

        [JsonProperty("head")]
        public Operation? Head { get; set; }

        [JsonProperty("options")]
        public Operation? Options { get; set; }

        [JsonProperty("trace")]
        public Operation? Trace { get; set; }

        public IEnumerable<Operation> GetOperations(string path) => InternalGetOperations()
            .Where(x => x != null)
            .Select(x => x with { Path = path }) ?? Array.Empty<Operation>();

        private IEnumerable<Operation?> InternalGetOperations()
        {
            yield return Get == null ? null : Get with { HttpMethod = nameof(Get) };
            yield return Post == null ? null : Post with { HttpMethod = nameof(Post) };
            yield return Put == null ? null : Put with { HttpMethod = nameof(Put) };
            yield return Patch == null ? null : Patch with { HttpMethod = nameof(Patch) };
            yield return Delete == null ? null : Delete with { HttpMethod = nameof(Delete) };
            yield return Head == null ? null : Head with { HttpMethod = nameof(Head) };
            yield return Options == null ? null : Options with { HttpMethod = nameof(Options) };
            yield return Trace == null ? null : Trace with { HttpMethod = nameof(Trace) };
        }
    }
}
