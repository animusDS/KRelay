using System;
using System.IO;
using System.Reflection;
using Fiddler;

namespace Lib_K_Relay.Utilities
{
    public static class HookHttps
    {
        private const ushort Port = 8877;

        private static readonly string AssemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        
        public static void Run()
        {
            AttachEventListeners();
            EnsureRootCertificate();
            StartupFiddlerCore();
        }
        
        private static void StartupFiddlerCore()
        {
            var startupSettings =
                new FiddlerCoreStartupSettingsBuilder()
                    .ListenOnPort(Port)
                    .RegisterAsSystemProxy()
                    .ChainToUpstreamGateway()
                    .DecryptSSL()
                    .OptimizeThreadPool()
                    .Build();

            FiddlerApplication.Startup(startupSettings);
        }
        
        private static void EnsureRootCertificate()
        {
            var certProvider = new BCCertMaker.BCCertMaker();
            CertMaker.oCertProvider = certProvider;
            var rootCertificatePath = Path.Combine(AssemblyDirectory, "..", "..", "RootCertificate.p12");
            const string rootCertificatePassword = "S0m3T0pS3cr3tP4ssw0rd";
            if (!File.Exists(rootCertificatePath))
            {
                certProvider.CreateRootCertificate();
                certProvider.WriteRootCertificateAndPrivateKeyToPkcs12File(rootCertificatePath, rootCertificatePassword);
            }
            else
            {
                certProvider.ReadRootCertificateAndPrivateKeyFromPkcs12File(rootCertificatePath, rootCertificatePassword);
            }
            if (!CertMaker.rootCertIsTrusted())
            {
                CertMaker.trustRootCert();
            }
        }

        private static void AttachEventListeners()
        {
            // Set bBufferResponse to true on BeforeRequest to allow content manipulation. If it is set anywhere else system proxy will not work and it will not replace.
            FiddlerApplication.BeforeRequest += oSession => oSession.bBufferResponse = true;
            FiddlerApplication.BeforeResponse += oSession =>
            {
                // Inject Proxy Server
                if (!oSession.uriContains("account/servers")) return;
                oSession.utilDecodeResponse();
                oSession.utilReplaceInResponse("<Servers>",
                    "<Servers>" +
                                    "<Server>" +
                                        $"<Name>{Config.Default.ProxyServerName}</Name>" +
                                        "<DNS>127.0.0.1</DNS>" +
                                        "<Lat>32.80</Lat>" +
                                        "<Long>-96.77</Long>" +
                                        "<Usage>0.00</Usage>" +
                                    "</Server>");
                PluginUtils.Log("K Relay", "Injected Proxy Server");
            };
        }
        
        public static void Quit()
        {
            FiddlerApplication.Shutdown();
        }
    }
}