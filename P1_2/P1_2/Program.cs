using System;
using System.IO;
using System.Security.Cryptography;

namespace P1_2
{
    class Program
    {
        private static double Solve(string plaintext, string ciphertext)
        {
            DateTime start_date = new DateTime(2020, 7, 3, 11, 0, 0);
            DateTime finish_date = new DateTime(2020, 7, 4, 11, 0, 0);
            TimeSpan ts_start = start_date.Subtract(new DateTime(1970, 1, 1));
            TimeSpan ts_finish = finish_date.Subtract(new DateTime(1970, 1, 1));
            int start = (int)ts_start.TotalMinutes;
            int end = (int)ts_finish.TotalMinutes;

            // brute force
            double seed;
            for (int i = start; i <= end; i++)
            {
                // dt = new DateTime(2020, 7, 4, 10, 15, 0);
                //TimeSpan ts = dt.Subtract(new DateTime(1970, 1, 1));
                //Random rng = new Random((int)ts.TotalMinutes);
                Random rng = new Random(i);
                byte[] key = BitConverter.GetBytes(rng.NextDouble());
                if ( ciphertext == Encrypt(key, plaintext) )
                {
                    seed = i;
                    return seed;
                }
            }
            return -1;
        }
        
        private static string Encrypt(byte[] key, string secretString)
        {
            DESCryptoServiceProvider csp = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, csp.CreateEncryptor(key, key), CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(cs);
            sw.Write(secretString);
            sw.Flush();
            cs.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }

        static void Main(string[] args)
        {
            //string plaintext = "Hello World";
            //string ciphertext = "RgdIKNgHn2Wg7jXwAykTlA==";
            // dotnet run "Hello World" "RgdIKNgHn2Wg7jXwAykTlA=="
            string plaintext = args[0];
            string ciphertext = args[1];

            var ans = Solve(plaintext, ciphertext);
            Console.WriteLine(ans);
        }
    }
}
