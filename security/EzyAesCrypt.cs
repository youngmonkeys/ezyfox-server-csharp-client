using System;
using System.IO;
using System.Security.Cryptography;

namespace com.tvd12.ezyfoxserver.client.security
{
	public class EzyAesCrypt
	{
        private static readonly EzyAesCrypt DEFAULT = new EzyAesCrypt();

        public static EzyAesCrypt getDefault()
        {
            return DEFAULT;
        }

        public byte[] encrypt(byte[] content, byte[] encryptionKey)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = encryptionKey;
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] encryptedBytes = performCryptography(content, encryptor);
                    byte[] iv = aes.IV;
                    byte[] result = new byte[iv.Length + encryptedBytes.Length];
                    Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                    Buffer.BlockCopy(encryptedBytes, 0, result, iv.Length, encryptedBytes.Length);
                    return result;
                }
            }
        }

        public byte[] decrypt(byte[] content, byte[] decryptionKey)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = decryptionKey;

                byte[] iv = new byte[aes.BlockSize / 8];
                Buffer.BlockCopy(content, 0, iv, 0, iv.Length);
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor())
                {
                    byte[] encryptedData = new byte[content.Length - iv.Length];
                    Buffer.BlockCopy(content, iv.Length, encryptedData, 0, encryptedData.Length);
                    return performCryptography(encryptedData, decryptor);
                }
            }
        }

        private byte[] performCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                }

                return ms.ToArray();
            }
        }
    }
}

