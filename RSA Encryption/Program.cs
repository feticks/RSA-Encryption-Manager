using System;
using System.IO;
using System.Security.Cryptography;

namespace RSA_Encryption {
    class Program {
        static void Main(string[] args) {
            if (!Directory.Exists("Keys"))
                Directory.CreateDirectory("Keys");

            // Exporting
            var rsa = new RSACryptoServiceProvider(2048); // Creates new instance with key size.
            Manager.Save(rsa, "Keys/Keys Base64.txt"); // Converts the public & private key to XML format and saves in specified path.

            File.WriteAllText("Keys/Public XML.txt", // Writing public key as XML format.
                Manager.KeyToString(
                    rsa.ExportParameters(false)));

            File.WriteAllText("Keys/Private XML.txt", // Writing private key as XML format.
                Manager.KeyToString(
                    rsa.ExportParameters(true)));

            // Importing
            var importedRsa = Manager.Import("Keys/Keys Base64.txt"); // Reads the public & private key from the XML format.
            var publicKey = Manager.KeyToString(importedRsa.ExportParameters(false)); // Grabbing the public key as an XML string. 
            var privateKey = Manager.KeyToString(importedRsa.ExportParameters(true)); // Grabbing the private key as an XML string.

            string textToEncrypt = "Hello World!"; //Unencrypted string.
            Write("Text to encrypt:", textToEncrypt);

            var encryptedText = RSA.Encrypt(textToEncrypt, publicKey); // Encrypting string.
            Write("Encrypted text:", encryptedText);

            var decryptedText = RSA.Decrypt(encryptedText, privateKey); // Decryting string.
            Write("Decrypted text:", decryptedText);

            Console.ReadKey();
        }

        static void Write(string first, string second) {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(first);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(second + Environment.NewLine);
        }
    }
}