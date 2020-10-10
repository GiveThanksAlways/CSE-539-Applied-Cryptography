using System;
using System.Security.Cryptography;
using System.Numerics;
using System.Text;
using System.IO;

namespace P3
{
    class Program
    {
        static BigInteger GetBigInteger(int e, int c)
        {
            BigInteger exponent = BigInteger.Pow(2, e);
            return exponent - c;
        }

        static byte[] Aes_encrypt(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            using (Aes AES = Aes.Create())
            {
                AES.Key = Key;
                AES.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = AES.CreateEncryptor(AES.Key, AES.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        static string Aes_decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;

            using (Aes AES = Aes.Create())
            {
                AES.Key = Key;
                AES.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = AES.CreateDecryptor(AES.Key, AES.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        static byte[] get_bytes_from_string(string input)
        {
            //string input = args[0];
            var input_split = input.Split(' ');
            byte[] inputBytes = new byte[input_split.Length];
            int i = 0;
            foreach (string item in input_split)
            {
                inputBytes.SetValue(Convert.ToByte(item, 16), i);
                i++;
            }
            return inputBytes;
        }

        static void Main(string[] args)
        {

            var IV = get_bytes_from_string(args[0]); // hex string
            var g_e = Int32.Parse(args[1]); // base 10
            var g_c = Int32.Parse(args[2]);
            var N_e = Int32.Parse(args[3]);
            var N_c = Int32.Parse(args[4]);
            var x = Int32.Parse(args[5]);
            var g_y = BigInteger.Parse(args[6]);
            var cipherText = get_bytes_from_string(args[7]); // hex string
            var plainText = args[8]; // string

            

            BigInteger g = GetBigInteger(g_e, g_c);
            BigInteger N = GetBigInteger(N_e, N_c);

            // create key for AES
            BigInteger key_big_int = BigInteger.ModPow(g_y, x, N); // BigInteger.Pow(g_y, x);
            byte[] key = key_big_int.ToByteArray();
            

            // AES decrypt ciphertext
            string decryption = Aes_decrypt(cipherText, key, IV);
            Console.Write(decryption);
            Console.Write(",");

            // AES encrypt plaintext
            byte[] encryption = Aes_encrypt(plainText, key, IV);
            Console.Write(BitConverter.ToString(encryption).Replace("-", " "));

            /*dotnet run 
            "A2 2D 93 61 7F DC 0D 8E C6 3E A7 74 51 1B 24 B2" 
            251 
            465 
            255 
            1311 
            2101864342
            8995936589171851885163650660432521853327227178155593274584417851704581358902 
            "F2 2C 95 FC 6B 98 BE 40 AE AD 9C 07 20 3B B3 9F F8 2F 6D 2D 69 D6 5D 40 0A 75 45 80 45 F2 DE C8 6E C0 FF 33 A4 97 8A AF 4A CD 6E 50 86 AA 3E DF"
            AfYw7Z6RzU9ZaGUloPhH3QpfA1AXWxnCGAXAwk3f6MoTx*/
        }
    }
}
