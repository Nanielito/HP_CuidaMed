using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilidades {

    /// <summary>
    /// Class to manage encryption methods.
    /// </summary>
    public sealed class Seguridad {

        /// <summary>
        /// The field _instance represents a singleton for the object instance.
        /// </summary>
        private static readonly Seguridad _instance = new Seguridad();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static Seguridad Instance {
            get {
                return _instance;
            }
        }

        /*******************************/
        /* Development                 */
        /* AES  1234                   */
        /* 3DES qwerty                 */
        /* 123PollitoIngles            */
        /* Production (u should erase) */
        /* AES  wasd256                */
        /* 3DES zxcpoi098              */
        /* 123PollitoIngles            */
        /*******************************/
        /// <summary>
        /// The field key represents the salt to be used for encryption methods.
        /// </summary>
        private static string key = (
            new Func<string>(() => {
                //For test pc: Add pc name to file pm.txt
                string[] pc = System.IO.File.ReadAllLines(System.IO.Directory.GetParent(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6)).Parent.FullName + @"\pm.txt");

                if (pc.Contains(System.Net.Dns.GetHostName().ToUpper()))
                    return "e37oGFwYNWy+EjiNnJc6GA8pvSFmH1Tq0hufm/U+EsINreiZ0rqQr81jtkZKCv2x";
                else
                    return "wGuifXrKc8WTPditxvZSNwQM17bdeCtXic12gDGNKWc7F1POZqMKjyPjmIiSY8gP";
            })
        )();

        /// <summary>
        /// Gets the encryption key.
        /// </summary>
        /// <returns>
        /// The encryption key value. 
        /// </returns>
        public string getKey() {
            return key;
        }
    }

    /// <summary>
    /// Class to manage Base64 encoding.
    /// </summary>
    public static class Base64 {

        /// <summary>
        /// Encodes to Base64 the specified input string.
        /// </summary>
        /// <param name="cadena">
        /// The string to be encoded.
        /// </param>
        /// <returns>
        /// An encoded string in Base64.
        /// </returns>
        public static string encriptar(string cadena) {
            return Crypter.Base64.Encrypt(cadena);
        }

        /// <summary>
        /// Decodes from Base64 the specified input string.
        /// </summary>
        /// <param name="cadena">
        /// The string to be decoded.
        /// </param>
        /// <returns>
        /// A decoded string from Base64.
        /// </returns>
        public static string desencriptar(string cadena) {
            return Crypter.Base64.Decrypt(cadena);
        }
    }

    /// <summary>
    /// Class to manage 3DES encoding.
    /// </summary>
    public static class _3DES {

        /// <summary>
        /// Encodes to 3DES the specified input string.
        /// </summary>
        /// <param name="cadena">
        /// The string to be encoded.
        /// </param>
        /// <returns>
        /// An encoded string in 3DES.
        /// </returns>
        public static string encriptar(string cadena) {
            return Crypter._3DES.Encrypt(cadena, Seguridad.Instance.getKey());
        }

        /// <summary>
        /// Encodes to 3DES the specified input string using a salt given.
        /// </summary>
        /// <param name="cadena">
        /// The string to be encoded.
        /// </param>
        /// <param name="contrasena">
        /// The salt to be used for encode.
        /// </param>
        /// <returns>
        /// An encoded string in 3DES.
        /// </returns>
        public static string encriptar(string cadena, string contrasena) {
            return Crypter._3DES.Encrypt(cadena, contrasena);
        }

        /// <summary>
        /// Decodes from 3DES the specified input string.
        /// </summary>
        /// <param name="cadena">
        /// The string to be decoded.
        /// </param>
        /// <returns>
        /// A decoded string from 3DES.
        /// </returns>
        public static string desencriptar(string cadena) {
            return Crypter._3DES.Decrypt(cadena, Seguridad.Instance.getKey());
        }

        /// <summary>
        /// Decodes from 3DES the specified input string.
        /// </summary>
        /// <param name="cadena">
        /// The string to be decoded.
        /// </param>
        /// <param name="contrasena">
        /// The salt to be used for decode.
        /// </param>
        /// <returns>
        /// A decoded string from 3DES.
        /// </returns>
        public static string desencriptar(string cadena, string contrasena) {
            return Crypter._3DES.Decrypt(cadena, contrasena);
        }
    }

    /// <summary>
    /// Class to manage AES encoding.
    /// </summary>
    public static class AES {

        /// <summary>
        /// Encodes to AES the specified input string.
        /// </summary>
        /// <param name="cadena">
        /// The string to be encoded.
        /// </param>
        /// <returns>
        /// An encoded string in AES.
        /// </returns>
        public static string encriptar(string cadena) {
            return Crypter.AES.Encrypt(cadena, Seguridad.Instance.getKey());
        }

        /// <summary>
        /// Encodes to AES the specified input string using a salt given.
        /// </summary>
        /// <param name="cadena">
        /// The string to be encoded.
        /// </param>
        /// <param name="contrasena">
        /// The salt to be used for encode.
        /// </param>
        /// <returns>
        /// An encoded string in AES.
        /// </returns>
        public static string encriptar(string cadena, string contrasena) {
            return Crypter.AES.Encrypt(cadena, contrasena);
        }

        /// <summary>
        /// Decodes from AES the specified input string.
        /// </summary>
        /// <param name="cadena">
        /// The string to be decoded.
        /// </param>
        /// <returns>
        /// A decoded string from AES.
        /// </returns>
        public static string desencriptar(string cadena) {
            return Crypter.AES.Decrypt(cadena, Seguridad.Instance.getKey());
        }

        /// <summary>
        /// Decodes from AES the specified input string.
        /// </summary>
        /// <param name="cadena">
        /// The string to be decoded.
        /// </param>
        /// <param name="contrasena">
        /// The salt to be used for decode.
        /// </param>
        /// <returns>
        /// A decoded string from AES.
        /// </returns>
        public static string desencriptar(string cadena, string contrasena) {
            return Crypter.AES.Decrypt(cadena, contrasena);
        }
    }

    /// <summary>
    /// Class to manage hashing encryption.
    /// </summary>
    public static class Hashing {

        /// <summary>
        /// Encrypts to MD5 the specified input string.
        /// </summary>
        /// <param name="cadena">
        /// The string to be encrypted.
        /// </param>
        /// <returns>
        /// An encrypted string in MD5.
        /// </returns>
        public static string MD5(string cadena) {
            return Crypter.Hashing.MD5(cadena, Seguridad.Instance.getKey(), false);
        }

        /// <summary>
        /// Encrypts to MD5 the specified input string.
        /// </summary>
        /// <param name="cadena">
        /// The string to be encrypted.
        /// </param>
        /// <param name="contrasena">
        /// The salt to be used for encryption.
        /// </param>
        /// <returns>
        /// An encrypted string in MD5.
        /// </returns>
        public static string MD5(string cadena, string contrasena) {
            return Crypter.Hashing.MD5(cadena, contrasena, false);
        }

        /// <summary>
        /// Encrypts to SHA-1 the specified input string.
        /// </summary>
        /// <param name="cadena">
        /// The string to be encrypted.
        /// </param>
        /// <returns>
        /// An encrypted string in SHA-1.
        /// </returns>
        public static string SHA_1(string cadena) {
            return Crypter.Hashing.SHA_1(cadena, Seguridad.Instance.getKey(), false);
        }

        /// <summary>
        /// Encrypts to SHA-1 the specified input string.
        /// </summary>
        /// <param name="cadena">
        /// The string to be encrypted.
        /// </param>
        /// <param name="contrasena">
        /// The salt to be used for encryption.
        /// </param>
        /// <returns>
        /// An encrypted string in SHA-1.
        /// </returns>
        public static string SHA_1(string cadena, string contrasena) {
            return Crypter.Hashing.SHA_1(cadena, contrasena, false);
        }

        /// <summary>
        /// Encrypts to SHA-256 the specified input string.
        /// </summary>
        /// <param name="cadena">
        /// The string to be encrypted.
        /// </param>
        /// <returns>
        /// An encrypted string in SHA-256.
        /// </returns>
        public static string SHA_256(string cadena) {
            return Crypter.Hashing.SHA_256(cadena, Seguridad.Instance.getKey(), false);
        }

        /// <summary>
        /// Encrypts to SHA-256 the specified input string.
        /// </summary>
        /// <param name="cadena">
        /// The string to be encrypted.
        /// </param>
        /// <param name="contrasena">
        /// The salt to be used for encryption.
        /// </param>
        /// <returns>
        /// An encrypted string in SHA-256.
        /// </returns>
        public static string SHA_256(string cadena, string contrasena) {
            return Crypter.Hashing.SHA_256(cadena, contrasena, false);
        }

        /// <summary>
        /// Encrypts to SHA-384 the specified input string.
        /// </summary>
        /// <param name="cadena">
        /// The string to be encrypted.
        /// </param>
        /// <returns>
        /// An encrypted string in SHA-384.
        /// </returns>
        public static string SHA_384(string cadena) {
            return Crypter.Hashing.SHA_384(cadena, Seguridad.Instance.getKey(), false);
        }

        /// <summary>
        /// Encrypts to SHA-384 the specified input string.
        /// </summary>
        /// <param name="cadena">
        /// The string to be encrypted.
        /// </param>
        /// <param name="contrasena">
        /// The salt to be used for encryption.
        /// </param>
        /// <returns>
        /// An encrypted string in SHA-384.
        /// </returns>
        public static string SHA_384(string cadena, string contrasena) {
            return Crypter.Hashing.SHA_384(cadena, contrasena, false);
        }

        /// <summary>
        /// Encrypts to SHA-512 the specified input string.
        /// </summary>
        /// <param name="cadena">
        /// The string to be encrypted.
        /// </param>
        /// <returns>
        /// An encrypted string in SHA-512.
        /// </returns>
        public static string SHA_512(string cadena) {
            return Crypter.Hashing.SHA_512(cadena, Seguridad.Instance.getKey(), false);
        }

        /// <summary>
        /// Encrypts to SHA-512 the specified input string.
        /// </summary>
        /// <param name="cadena">
        /// The string to be encrypted.
        /// </param>
        /// <param name="contrasena">
        /// The salt to be used for encryption.
        /// </param>
        /// <returns>
        /// An encrypted string in SHA-512.
        /// </returns>
        public static string SHA_512(string cadena, string contrasena) {
            return Crypter.Hashing.SHA_512(cadena, contrasena, false);
        }
    }
}