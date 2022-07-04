using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace RSA_Encryption {
    class Manager {
        public static void Export(RSACryptoServiceProvider rsa, string path) {
            RSAParameters publicKey = rsa.ExportParameters(false); // Exporting the public key.
            RSAParameters privateKey = rsa.ExportParameters(true); // Exporting the private key.

            File.WriteAllText(path, // Writing to path.
                Convert.ToBase64String( // Encoding bytes to Base64.
                    Encoding.UTF8.GetBytes( // XML string to bytes.
                        $"{KeyToString(publicKey)}|{KeyToString(privateKey)}"))); // Formatting the public & private key into an XML string.
        }

        public static RSACryptoServiceProvider Import(string path) {
            var rsa = new RSACryptoServiceProvider();

            var keys = Encoding.UTF8.GetString( // Bytes to string.
                Convert.FromBase64String( // Decoding from Base64 to bytes.
                    File.ReadAllText(path) // Reading from path.
                    )).Split('|'); // Splitting public & private key.

            rsa.FromXmlString(keys[0]); // Setting the public key into the RSACryptoServiceProvider instance.
            rsa.FromXmlString(keys[1]); // Setting the private key into the RSACryptoServiceProvider instance.
            return rsa;
        }

        public static string KeyToString(RSAParameters key) {
            var stringWriter = new StringWriter(); // New instance of string writer.
            var xmlSerializer = new XmlSerializer(typeof(RSAParameters)); // New instance of XML serializer as RSA algorithm type.

            xmlSerializer.Serialize(stringWriter, key); // Formatting key as XML.
            return stringWriter.ToString(); // Converting back to a string.
        }
    }
}
