using OpenApiSdkGenerator.Models;

namespace OpenApiSdkGenerator
{
    public static class TemplateFilter
    {
        public static string GetOperationName(this Operation operation)
        {
            return operation.Name;
        }

        public static string GetOperationMethodSignature(this Operation operation)
        {
            return operation.MethodSignature;
        }

        public static string GetOperationSuccessResponseType(this Operation operation)
        {
            return operation.SuccessResponseType;
        }
    }
}
