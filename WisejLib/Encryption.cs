using System;
using System.IO;
using System.Security.Cryptography;

namespace WisejLib
{
    /// <summary>String encryption using System.Security.Cryptography</summary>
    public static class Encryption
    {
        /// <summary>Internal private key used to encrypt/decrypt strings</summary>
        private const string STANDARD_ENCRYPTION_KEY = "M1$ft22=9gwTT";

        /// <summary>Encrypts a string</summary>
        /// <param name="clearText">The text to be encrypted</param>
        /// <param name="key">The key to use for encrypting (default=STANDARD_ENCRYPTION_KEY)</param>
        /// <returns>The encrypted string (which may be longer than the original)</returns>
        public static string Encrypt(this string clearText, string key = STANDARD_ENCRYPTION_KEY)
        {
            if (string.IsNullOrEmpty(clearText))
                return clearText;

            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            byte[] encryptedData = EncryptByteArray(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>Decrypts a string</summary>
        /// <param name="cipherText">The text to be decrypted</param>
        /// <param name="key">The key to use for decrypting (default=STANDARD_ENCRYPTION_KEY). This must be the same key that was used for encrypting</param>
        /// <returns>The decrypted string</returns>
        public static string Decrypt(this string cipherText, string key = STANDARD_ENCRYPTION_KEY)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                byte[] decryptedData = DecryptByteArray(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
                return System.Text.Encoding.Unicode.GetString(decryptedData);
            }
            catch
            {
                return cipherText;
            }
        }

        /// <summary>Generates a random password</summary>
        /// <param name="length">The length of the generated password</param>
        /// <param name="allUpperCase">If true, only uppercase letters are included</param>
        /// <returns>The random password</returns>
        public static string GeneratePassword(int length, bool allUpperCase)
        {
            char[] result = new char[length];
            string characters = "a2$b3%c4d5=e6*f7#g8+h9jk%23=m4n5#6p+7q8r%9s=tu*2v3w+4x$5y6z=7A8B#9C+DE$2F%3G4H*56#J7+K8$L9MN=23*P4#Q5R6$S7%T8=U9*VW#X2+Y3$Z";
            characters += characters;
            if (allUpperCase)
                characters = characters.ToUpper();
            // subsequent calls that come very fast after each other lead to identical values so we wait a
            // tiny little bit between calls. Why that happens? I have no idea :-(
            System.Threading.Thread.Sleep(1);
            Random random = new Random();
            for (var i = 0; i < length; i++)
                result[i] = characters[random.Next(characters.Length - 1)];
            return new string(result);
        }

        #region private -------------------------------------------------------

        private static byte[] EncryptByteArray(byte[] clearText, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Aes alg = Aes.Create();
            // obsolete:
            // Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(clearText, 0, clearText.Length);
            cs.Close();
            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        }

        private static byte[] DecryptByteArray(byte[] cipherData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Aes alg = Aes.Create();
            // obsolete:
            // Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherData, 0, cipherData.Length);
            cs.Close();
            byte[] decryptedData = ms.ToArray();
            return decryptedData;
        }

        #endregion
    }
}