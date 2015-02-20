using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilidades {

    /// <summary>
    /// Class to manage general methods.
    /// </summary>
    public static class FuncionesGlobales {

        /// <summary>
        /// Enumerator for encryption.
        /// </summary>
        public enum encrypt {
            Ninguna = 0,
            Base64  = 1,
            AES     = 2,
            _3DES   = 3,
            MD5     = 4,
            SHA1    = 5,
            SHA256  = 6,
            SHA384  = 7,
            SHA512  = 8
        };

        /// <summary>
        /// Validates the specified input string for encodes (encrypts) it or not.
        /// </summary>
        /// <param name="cadena">
        /// The string to be validated.
        /// </param>
        /// <param name="enc">
        /// The encode (encrypt) type to be applied if it is necessary.
        /// </param>
        /// <returns>
        /// A validated string.
        /// </returns>
        public static string validar_tostr(string cadena, encrypt enc = encrypt.Ninguna) {
            switch (enc) {
                case encrypt.Base64:
                    return !string.IsNullOrEmpty(cadena) ? Base64.encriptar(cadena) : null;
                case encrypt.AES:
                    return !string.IsNullOrEmpty(cadena) ? AES.encriptar(cadena)    : null;
                case encrypt._3DES:
                    return !string.IsNullOrEmpty(cadena) ? _3DES.encriptar(cadena)  : null;
                case encrypt.MD5:
                    return !string.IsNullOrEmpty(cadena) ? Hashing.MD5(cadena)      : null;
                case encrypt.SHA1:
                    return !string.IsNullOrEmpty(cadena) ? Hashing.SHA_1(cadena)    : null;
                case encrypt.SHA256:
                    return !string.IsNullOrEmpty(cadena) ? Hashing.SHA_256(cadena)  : null;
                case encrypt.SHA384:
                    return !string.IsNullOrEmpty(cadena) ? Hashing.SHA_384(cadena)  : null;
                case encrypt.SHA512:
                    return !string.IsNullOrEmpty(cadena) ? Hashing.SHA_512(cadena)  : null;
                default:
                    return !string.IsNullOrEmpty(cadena) ? cadena                   : null;
            }
        }
    }
}