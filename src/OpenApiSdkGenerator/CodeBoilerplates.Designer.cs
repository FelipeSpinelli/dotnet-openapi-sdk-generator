﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OpenApiSdkGenerator {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class CodeBoilerplates {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CodeBoilerplates() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("OpenApiSdkGenerator.CodeBoilerplates", typeof(CodeBoilerplates).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // &lt;auto-generated /&gt;
        ///#nullable enable
        ///using RestEase;
        ///using System.Threading;
        ///{{for using in usings}}
        ///using {{ using }};
        ///{{end}}
        ///namespace {{ namespace }}
        ///{
        ///    public partial interface {{ api_client_name }}
        ///    {
        ///{{ for operation in operations }}
        ///        {{ operation }}
        ///{{ end }}
        ///    }
        ///}
        ///#nullable restore.
        /// </summary>
        internal static string ApiClientInterface {
            get {
                return ResourceManager.GetString("ApiClientInterface", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {{for attribute in attributes}}
        ///{{ attribute }}
        ///{{end}}
        ///[{{ http_method }}(&quot;{{ path }}&quot;)]
        ///[AllowAnyStatusCode]
        ///public Task&lt;Response&lt;{{ response }}&gt;&gt; {{ name }}({{ method_signature }});.
        /// </summary>
        internal static string ApiClientOperation {
            get {
                return ResourceManager.GetString("ApiClientOperation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // &lt;auto-generated /&gt;
        ///#nullable enable
        ///using RestEase;
        ///{{ newtonsoft_using }}
        ///namespace {{ namespace }}
        ///{
        ///    public {{type}} {{ name }}
        ///    {
        ///{{ for property in properties }}
        ///        {{ property }}
        ///{{ end }}
        ///        {{ to_string }}
        ///    }
        ///}
        ///#nullable restore.
        /// </summary>
        internal static string ApiClientType {
            get {
                return ResourceManager.GetString("ApiClientType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to public override string ToString()
        ///{
        ///    return this.GetRawQueryString();
        ///}.
        /// </summary>
        internal static string ApiQueryClassToStringMethod {
            get {
                return ResourceManager.GetString("ApiQueryClassToStringMethod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #nullable enable
        ///namespace {{ namespace }}
        ///{
        ///    public class NoContentResponse
        ///    {
        ///    }
        ///}
        ///#nullable restore.
        /// </summary>
        internal static string NoContentResponse {
            get {
                return ResourceManager.GetString("NoContentResponse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to //&lt;auto-generated /&gt;
        ///#nullable enable
        ///using RestEase;
        ///using System.Collections;
        ///using System.Text;
        ///
        ///namespace {{ namespace }}
        ///{
        ///    internal static class OpenApiSdkGeneratorUtils
        ///    {
        ///        public static string GetRawQueryString(this object obj)
        ///        {
        ///            var type = obj.GetType();
        ///            var sb = new StringBuilder();
        ///            var isFirst = true;
        ///            foreach (var property in type.GetProperties())
        ///            {
        ///                var queryAttribute = property.GetCu [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string OpenApiSdkGeneratorUtils {
            get {
                return ResourceManager.GetString("OpenApiSdkGeneratorUtils", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // &lt;auto-generated /&gt;
        ///using Microsoft.Extensions.Configuration;
        ///using RestEase.HttpClientFactory;
        ///using System.Diagnostics.CodeAnalysis;
        ///using Polly;
        ///using {{ namespace }};
        ///namespace Microsoft.Extensions.DependencyInjection;
        ///
        ///[ExcludeFromCodeCoverage]
        ///public static partial class {{ api_name }}ServiceCollectionExtensions
        ///{
        ///    public partial class {{ api_name }}ConnectionSettings
        ///    {
        ///        public Uri BaseAddress { get; set; }
        ///        public TimeSpan Timeout { get; set; }
        ///        public boo [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SdkServicesCollectionExtensions {
            get {
                return ResourceManager.GetString("SdkServicesCollectionExtensions", resourceCulture);
            }
        }
    }
}
