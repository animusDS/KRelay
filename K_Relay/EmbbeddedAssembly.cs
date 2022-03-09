using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace K_Relay
{
    public class EmbeddedAssembly
    {
        // Version 1.3

        private static Dictionary<string, Assembly> _dic;

        public static void Load(string embeddedResource, string fileName)
        {
            if (_dic == null)
                _dic = new Dictionary<string, Assembly>();

            byte[] ba = null;
            Assembly asm = null;
            var curAsm = Assembly.GetExecutingAssembly();

            using (var stm = curAsm.GetManifestResourceStream(embeddedResource))
            {
                // Either the file is not existed or it is not mark as embedded resource
                if (stm == null)
                    throw new Exception(embeddedResource + " is not found in Embedded Resources.");

                // Get byte[] from the file from embedded resource
                ba = new byte[(int)stm.Length];
                stm.Read(ba, 0, (int)stm.Length);
                try
                {
                    asm = Assembly.Load(ba);

                    // Add the assembly/dll into dictionary
                    _dic.Add(asm.FullName, asm);
                    return;
                }
                catch
                {
                    // Purposely do nothing
                    // Unmanaged dll or assembly cannot be loaded directly from byte[]
                    // Let the process fall through for next part
                }
            }

            var fileOk = false;
            var tempFile = "";

            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                var fileHash = BitConverter.ToString(sha1.ComputeHash(ba)).Replace("-", string.Empty);
                ;

                tempFile = Path.GetTempPath() + fileName;

                if (File.Exists(tempFile))
                {
                    var bb = File.ReadAllBytes(tempFile);
                    var fileHash2 = BitConverter.ToString(sha1.ComputeHash(bb)).Replace("-", string.Empty);

                    if (fileHash == fileHash2)
                        fileOk = true;
                    else
                        fileOk = false;
                }
                else
                {
                    fileOk = false;
                }
            }

            if (!fileOk) File.WriteAllBytes(tempFile, ba);

            asm = Assembly.LoadFile(tempFile);

            _dic.Add(asm.FullName, asm);
        }

        public static Assembly Get(string assemblyFullName)
        {
            if (_dic == null || _dic.Count == 0)
                return null;

            if (_dic.ContainsKey(assemblyFullName))
                return _dic[assemblyFullName];

            return null;
        }
    }
}