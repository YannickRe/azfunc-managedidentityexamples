using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DotNet.Dataverse.Handlers
{
    class AssemblyBindingRedirectHandler
    {
        private static readonly string bindingRedirectListJson = @"
        [
            {
                ""ShortName"": ""System.Buffers"",
                ""RedirectToVersion"": ""4.0.3.0"",
                ""PublicKeyToken"": ""cc7b13ffcd2ddd51""
            },
            {
                ""ShortName"": ""System.Threading.Tasks.Extensions"",
                ""RedirectToVersion"": ""4.2.0.1"",
                ""PublicKeyToken"": ""cc7b13ffcd2ddd51""
            },
            {
                ""ShortName"": ""Microsoft.Identity.Client"",
                ""RedirectToVersion"": ""4.30.1.0"",
                ""PublicKeyToken"": ""0a613f4dd989e8ae""
            },
            {
                ""ShortName"": ""System.Runtime.CompilerServices.Unsafe"",
                ""RedirectToVersion"": ""4.0.6.0"",
                ""PublicKeyToken"": ""b03f5f7f11d50a3a""
            }
        ]
        ";
        ///<summary>
        /// Reads the "BindingRedirecs" field from the app settings and applies the redirection on the
        /// specified assemblies
        /// </summary>

        public static void ConfigureBindingRedirects()
        {
            var redirects = GetBindingRedirects();
            redirects.ForEach(RedirectAssembly);
        }

        private static List<BindingRedirect> GetBindingRedirects()
        {
            var result = new List<BindingRedirect>();
            //var bindingRedirectListJson = Environment.GetEnvironmentVariable("BindingRedirects");
            using (var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(bindingRedirectListJson)))
            {
                var serializer = new DataContractJsonSerializer(typeof(List<BindingRedirect>));
                result = (List<BindingRedirect>)serializer.ReadObject(memoryStream);
            }
            return result;
        }

        private static void RedirectAssembly(BindingRedirect bindingRedirect)
        {
            ResolveEventHandler handler = null;
            handler = (sender, args) =>
            {
                var requestedAssembly = new AssemblyName(args.Name);
                if (requestedAssembly.Name != bindingRedirect.ShortName)
                {
                    return null;
                }
                var targetPublicKeyToken = new AssemblyName("x, PublicKeyToken=" + bindingRedirect.PublicKeyToken).GetPublicKeyToken();
                requestedAssembly.SetPublicKeyToken(targetPublicKeyToken);
                requestedAssembly.Version = new Version(bindingRedirect.RedirectToVersion);
                requestedAssembly.CultureInfo = CultureInfo.InvariantCulture;
                AppDomain.CurrentDomain.AssemblyResolve -= handler;
                return Assembly.Load(requestedAssembly);
            };
            AppDomain.CurrentDomain.AssemblyResolve += handler;
        }

        [DataContract]
        public class BindingRedirect
        {
            [DataMember]
            public string ShortName { get; set; }
            [DataMember]
            public string PublicKeyToken { get; set; }
            [DataMember]
            public string RedirectToVersion { get; set; }
        }
    }
}
