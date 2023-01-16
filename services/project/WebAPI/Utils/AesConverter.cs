using System;
using System.IO;
using System.Security.Cryptography;

namespace WebAPI.Utils
{
    public class AesConverter
    {
        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        // Al3x3yGaY!69420HanD$0M3$qU1DWarD
        private static readonly byte[] KeyBytes =
        {
            65, 108, 51, 120, 51, 121, 71, 97, 89, 33, 54, 57, 52, 50, 48, 72,
            97, 110, 68, 36, 48, 77, 51, 36, 113, 85, 49, 68, 87, 97, 114, 68
        };

        public static (byte[] data, byte[] iv) Serialize(byte[] bytesToBeEncrypted)
        {
            byte[] encryptedBytes;
            byte[] iv = new byte[16];
            Random.NextBytes(iv);
            
            using (var ms = new MemoryStream())
            {
                using (var AES = new AesManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = KeyBytes;
                    AES.IV = iv;
                    AES.Mode = CipherMode.CBC;
                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return (encryptedBytes, iv);
        }

        public static byte[] Deserialize(byte[] bytesToBeDecrypted, byte[] iv)
        {
            byte[] decryptedBytes;
            using (var ms = new MemoryStream())
            {
                using (var AES = new AesManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = KeyBytes;
                    AES.IV = iv;
                    AES.Mode = CipherMode.CBC;
                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
    }
}