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
        ///using Refit;
        ///{{for using in usings}}
        ///using {{ using }};
        ///{{end}}
        ///namespace {{ namespace }}
        ///{
        ///    public interface {{ api_client_name }}
        ///    {
        ///{{ for operation in operations }}
        ///        {{ operation }}
        ///{{ end }}
        ///    }
        ///}.
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
        ///public Task&lt;ApiResponse&lt;{{ response }}&gt;&gt; {{ name }}({{ method_signature }});.
        /// </summary>
        internal static string ApiClientOperation {
            get {
                return ResourceManager.GetString("ApiClientOperation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // &lt;auto-generated /&gt;
        ///using Refit;
        ///namespace {{ namespace }}
        ///{
        ///    public class {{ name }}
        ///    {
        ///{{ for property in properties }}
        ///        {{ property }}
        ///{{ end }}
        ///    }
        ///}.
        /// </summary>
        internal static string ApiClientType {
            get {
                return ResourceManager.GetString("ApiClientType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to namespace Refit
        ///{
        ///    public class NoContentResponse
        ///    {
        ///    }
        ///}.
        /// </summary>
        internal static string NoContentResponse {
            get {
                return ResourceManager.GetString("NoContentResponse", resourceCulture);
            }
        }
    }
}
