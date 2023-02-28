﻿using Newtonsoft.Json;
using OpenApiSdkGenerator.Extensions;
using OpenApiSdkGenerator.JsonConverters;
using Scriban;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace OpenApiSdkGenerator.Models
{
    public record Operation
    {
        private const string VALID_NAME_CHARACTERS_PATTERN = "[^0-9a-zA-Z]+";

        [JsonIgnore]
        public string Path { get; set; } = null!;

        [JsonIgnore]
        public string HttpMethod { get; set; } = null!;

        [JsonProperty("_reference")]
        public string Reference { get; set; } = null!;

        [JsonProperty("summary")]
        public string Summary { get; set; } = null!;

        [JsonProperty("description")]
        public string Description { get; set; } = null!;

        [JsonProperty("operationId")]
        public string OperationId { get; set; } = null!;

        [JsonProperty("tags")]
        public string[] Tags { get; set; } = Array.Empty<string>();

        [JsonProperty("parameters")]
        public Parameter[] Parameters { get; set; } = Array.Empty<Parameter>();

        [JsonProperty("requestBody")]
        public RequestBody? RequestBody { get; set; }

        [JsonProperty("responses")]
        [JsonConverter(typeof(DictionaryConverter<Response>))]
        public IDictionary<string, Response> Responses { get; set; }

        [JsonIgnore]
        public string[] Attributes { get; set; } = Array.Empty<string>();

        [JsonIgnore]
        public bool ShouldBeGenerated { get; set; } = true;

        public override string ToString()
        {
            var template = Template.Parse(CodeBoilerplates.ApiClientOperation);
            return template.Render(new
            {
                HttpMethod,
                Path,
                Response = GetSuccessResponseType(),
                Name = GetName(),
                MethodSignature = GetMethodSignature(),
                Attributes
            });
        }

        public string GetName() => string.Join("",
            (
                OperationId ??
                Tags.FirstOrDefault(x => !x.Equals(nameof(OpenApiSdkGenerator), StringComparison.InvariantCultureIgnoreCase)) ??
                Regex.Replace($"{HttpMethod}{Path}", VALID_NAME_CHARACTERS_PATTERN, "")
            )
            .Split(' ')
            .Select(x => x.ToPascalCase()));

        private string GetSuccessResponseType()
        {
            var successResponse = Responses
                .FirstOrDefault(x => short.TryParse(x.Key, out var statusCode) && statusCode >= 200 && statusCode < 300);

            if (successResponse.Key == ((int)HttpStatusCode.NoContent).ToString())
            {
                return "NoContentResponse";
            }

            var mediaType = successResponse.Value.Content.First().Value;
            return mediaType.GetTypeName();
        }

        private string GetMethodSignature()
        {
            return string.Join(",", new[]
            {
                GetMethodParametersFromPath(),
                GetMethodParametersFromHeaders(),
                GetMethodParametersFromQuery(),
                RequestBody?.ToString() ?? string.Empty

            }.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        private string GetMethodParametersFromPath()
        {
            return string.Join(",", Parameters.Where(p => p.In == Enumerators.ParameterLocation.Path).Select(p => p.ToString()));
        }

        private string GetMethodParametersFromHeaders()
        {
            return string.Join(",", Parameters.Where(p => p.In == Enumerators.ParameterLocation.Header).Select(p => p.ToString()));
        }

        private string GetMethodParametersFromQuery()
        {
            if (!Parameters.Any(x => x.In == Enumerators.ParameterLocation.Query))
            {
                return string.Empty;
            }

            var queryParamsClassName = Parameter.GetAsQueryParamsClassName(GetName());
            if (string.IsNullOrWhiteSpace(queryParamsClassName))
            {
                return string.Empty;
            }

            return $"[Query] {queryParamsClassName} query";
        }

        public Operation ApplySdkOptions(SdkOptions? options)
        {
            if (options == null)
            {
                return this;
            }

            var sdkOperationOptions = options.Operations.FirstOrDefault(x => x.Name.Equals(GetName()));

            return this with
            {
                ShouldBeGenerated = !(sdkOperationOptions?.Ignore ?? false),
                Attributes = (options
                    .DefaultOperationAttributes ?? Array.Empty<string>())
                    .Concat(sdkOperationOptions?.Attributes ?? Array.Empty<string>())
                    .ToArray()
            };
        }
    }
}
