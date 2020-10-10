using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace P2
{
    class Program
    {
        private static Random random = new Random();

        static void Main(string[] args)
        {
            //string da_byte = "C5";
            string da_byte = args[0];
            byte da_actual_byte = Convert.ToByte(da_byte, 16);
            int birthday_string_length = 10;
            MD5 hash_function = MD5.Create();
            Dictionary<string, string> hash_dictionary = new Dictionary<string, string>();
            byte[] salted;
            byte[] first_five;
            string next_random_string;
            string key;

            //string first_random_string = RandomString(birthday_string_length);
            //byte[] first_bytes = AddSomeSalt(first_random_string, da_actual_byte, hash_function);

            while (true)
            {
                next_random_string = RandomString(birthday_string_length);

                salted = AddSomeSalt(ref next_random_string, ref da_actual_byte, ref hash_function);
                first_five = GetTheFirstFiveBytes(ref salted);
                key = BitConverter.ToString(first_five).Replace("-", " ");

                if (hash_dictionary.ContainsKey(key))
                {
                    Console.WriteLine(hash_dictionary[key] + "," + next_random_string);
                    break;
                }
                else
                {
                    hash_dictionary.Add(key, next_random_string);
                }
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static byte[] AddSomeSalt(ref string input, ref byte salt, ref MD5 hash_function)
        {
            // append byte input to salt the password
            byte[] input_bytes = Encoding.UTF8.GetBytes(input);
            byte[] salted_bytes = new byte[input_bytes.Length + 1];
            input_bytes.CopyTo(salted_bytes, 0);
            salted_bytes[input_bytes.Length] = salt;
            return hash_function.ComputeHash(salted_bytes);
        }

        public static byte[] GetTheFirstFiveBytes(ref byte[] salted)
        {
            byte[] only_five = new byte[5];
            for (int i = 0; i < 5; i++)
            {
                only_five[i] = salted[i];
            }
            return only_five;
        }
    }
}
