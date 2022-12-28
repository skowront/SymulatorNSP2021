﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SymulatorNSP.Core.Resources {
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
    public class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SymulatorNSP.Core.Resources.Resource", typeof(Resource).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Complex.
        /// </summary>
        public static string ComplexGenerationConfiguration {
            get {
                return ResourceManager.GetString("ComplexGenerationConfiguration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This configuration is supposed to simulate both &quot;real-life&quot; properyt values and add some misspellings..
        /// </summary>
        public static string ComplexGenerationConfigurationDescription {
            get {
                return ResourceManager.GetString("ComplexGenerationConfigurationDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Key can not be empty!.
        /// </summary>
        public static string eChangeResultKeyEmptyExplanation {
            get {
                return ResourceManager.GetString("eChangeResultKeyEmptyExplanation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Success!.
        /// </summary>
        public static string eChangeResultSuccessExplanation {
            get {
                return ResourceManager.GetString("eChangeResultSuccessExplanation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to We store only results that were done using at least 1000000 records..
        /// </summary>
        public static string eChangeResultTooFewRecordsExplanation {
            get {
                return ResourceManager.GetString("eChangeResultTooFewRecordsExplanation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown error!.
        /// </summary>
        public static string eChangeResultUnknownExplanation {
            get {
                return ResourceManager.GetString("eChangeResultUnknownExplanation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your current performance factor is worse than the previous. It will not be submitted..
        /// </summary>
        public static string eChangeResultWorseFactorExplanation {
            get {
                return ResourceManager.GetString("eChangeResultWorseFactorExplanation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A record with this Nickname exists. You must enter the same key as one used during first submission with this Nickname..
        /// </summary>
        public static string eChangeResultWrongKeyExplanation {
            get {
                return ResourceManager.GetString("eChangeResultWrongKeyExplanation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Minimal.
        /// </summary>
        public static string OptimizedGenerationConfiguration {
            get {
                return ResourceManager.GetString("OptimizedGenerationConfiguration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This configuration is optimized in a way that your generated recors will have no property values, except for Nationality, to minimize the impact on memory consumption. No misspellings in nationality will be present..
        /// </summary>
        public static string OptimizedGenerationConfigurationDescription {
            get {
                return ResourceManager.GetString("OptimizedGenerationConfigurationDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Simple.
        /// </summary>
        public static string SimpleGenerationConfiguration {
            get {
                return ResourceManager.GetString("SimpleGenerationConfiguration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This configuration is optimized in a way that your generated recors will have very short property values to minimize the impact on your browser&apos;s memory consumption. Misspellings in nationality will be present..
        /// </summary>
        public static string SimpleGenerationConfigurationDescription {
            get {
                return ResourceManager.GetString("SimpleGenerationConfigurationDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to unrecognized.
        /// </summary>
        public static string Unrecognized {
            get {
                return ResourceManager.GetString("Unrecognized", resourceCulture);
            }
        }
    }
}
