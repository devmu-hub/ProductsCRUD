﻿using System.Security.Cryptography;
using System.Text;

namespace ProductsCRUD.Application.S_CipherService
{
    public class CipherService : ICipherService
    {
        private const string Key = "H52j89Mezb7kJkWNx9L2b9AnBHhuDEpR";


        public string Encrypt(string plainText)
        {
            byte[] iv = new byte[16];

            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new();

                using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);

                using (StreamWriter streamWriter = new(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }

                array = memoryStream.ToArray();
            }

            return Convert.ToBase64String(array);
        }

        public string Decrypt(string cipherText)
        {
            byte[] iv = new byte[16];

            byte[] buffer = Convert.FromBase64String(cipherText);

            using Aes aes = Aes.Create();

            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = iv;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream memoryStream = new(buffer);

            using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);

            using StreamReader streamReader = new(cryptoStream);

            return streamReader.ReadToEnd();
        }
    }
}
