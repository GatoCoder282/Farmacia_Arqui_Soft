using Farmacia_Arqui_Soft.Domain.Ports;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using System;
using System.Linq; // Necesario para .Take()

namespace Farmacia_Arqui_Soft.Aplication.Services
{
    /// <summary>
    /// Implementación del servicio de encriptación usando AES-256.
    /// </summary>
    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        // Constructor estándar para servicios inyectables.
        public EncryptionService()
        {
            // La clave (Key) debe ser de 32 bytes (256 bits).
            // El IV debe ser de 16 bytes (128 bits).

            // NOTA IMPORTANTE: Estas cadenas deben ser lo suficientemente largas 
            // (32+ caracteres para key, 16+ para iv) para que .Take() funcione.
            // En producción, carga esto de un archivo de secretos.
            string keySource = "ESTA ES MI CLAVE SECRETA DE 32 BYTES PARA AES-256!";
            string ivSource = "MI IV DE 16 BYTES";

            try
            {
                // Asegurar 32 bytes para la clave
                _key = Encoding.UTF8.GetBytes(keySource).Take(32).ToArray();

                // Asegurar 16 bytes para el IV
                _iv = Encoding.UTF8.GetBytes(ivSource).Take(16).ToArray();

                if (_key.Length != 32 || _iv.Length != 16)
                {
                    // Esto solo ocurriría si las cadenas keySource/ivSource son demasiado cortas
                    throw new InvalidOperationException("Error de configuración de AES: La clave o el IV no tienen la longitud correcta. Verifica las cadenas keySource e ivSource.");
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores durante la inicialización
                throw new InvalidOperationException("Error al inicializar el servicio de encriptación.", ex);
            }
        }

        public string EncryptId(int id)
        {
            byte[] idBytes = BitConverter.GetBytes(id);
            byte[] encryptedBytes;

            using (Aes aes = Aes.Create())
            {
                // 1. Configuración de AES
                aes.Key = _key;
                aes.IV = _iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    // 2. Encriptar los datos
                    encryptedBytes = encryptor.TransformFinalBlock(idBytes, 0, idBytes.Length);
                }
            }

            // 3. Convertir a Base64 URL-Safe
            string base64 = Convert.ToBase64String(encryptedBytes);

            // Reemplazar caracteres no seguros para URL y quitar padding
            return base64.Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }

        public int DecryptId(string encryptedId)
        {
            // 1. Revertir URL-Safe y re-agregar padding si es necesario
            string base64 = encryptedId.Replace('-', '+').Replace('_', '/');

            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            byte[] cipherBytes;
            try
            {
                cipherBytes = Convert.FromBase64String(base64);
            }
            catch (FormatException ex)
            {
                throw new FormatException("Formato de ID encriptado inválido.", ex);
            }

            using (Aes aes = Aes.Create())
            {
                // 4. Configuración de AES
                aes.Key = _key;
                aes.IV = _iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    byte[] decryptedBytes;
                    try
                    {
                        // 5. Desencriptar los datos
                        decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                    }
                    catch (CryptographicException ex)
                    {
                        // Error de desencriptación (clave/IV incorrectos, padding corrupto, etc.)
                        throw new FormatException("Error de desencriptación. Posible ID corrupto o clave incorrecta.", ex);
                    }

                    if (decryptedBytes.Length != 4)
                    {
                        // 6. Verificar que el resultado desencriptado sea exactamente un entero (4 bytes)
                        throw new FormatException($"El ID desencriptado no tiene el tamaño esperado (4 bytes). Tamaño real: {decryptedBytes.Length} bytes.");
                    }

                    // 7. Convierte los 4 bytes desencriptados de vuelta a entero
                    return BitConverter.ToInt32(decryptedBytes, 0);
                }
            }
        }
    }
}