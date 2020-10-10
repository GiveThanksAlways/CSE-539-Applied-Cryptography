using System;
using System.Security.Cryptography;
using System.Numerics;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace P4
{
    class Program
    {
        static BigInteger RSA_Encrypt(BigInteger plainText, BigInteger n, BigInteger e)
        {
            BigInteger cipherText = BigInteger.ModPow(plainText, e, n);
            return cipherText;
        }

        static BigInteger RSA_Decrypt(BigInteger cipherText, BigInteger n, BigInteger d)
        {
            BigInteger plainText = BigInteger.ModPow(cipherText, d, n);
            return plainText;
        }

        static BigInteger GetBigInteger(int e, int c)
        {
            BigInteger exponent = BigInteger.Pow(2, e);
            return exponent - c;
        }

        static BigInteger Extended_Euclidean_Algorithm_GCD(BigInteger a, BigInteger b )
        {
            List<BigInteger> s = new List<BigInteger>();
            List<BigInteger> t = new List<BigInteger>();
            List<BigInteger> r = new List<BigInteger>();
            BigInteger q;

            s.Add(1);
            s.Add(0);
            t.Add(0);
            t.Add(1);
            r.Add(a);
            r.Add(b);

            int i = 2;
            while (b >0)
            {
                q = BigInteger.Divide(a, b);
                r.Add(BigInteger.ModPow(a, 1, b));
                // pi = p_i-2 - p_i-1 q_i-2
                t.Add(BigInteger.Subtract(t[i - 2], BigInteger.Multiply(t[i - 1], q)));
                s.Add(BigInteger.Subtract(s[i - 2], BigInteger.Multiply(s[i - 1], q)));
                a = b;
                b = r[^1];
                i++;
            }
            return t[^2];
        }

        static void Main(string[] args)
        {
            var p_e = Int32.Parse(args[0]); // base 10
            var p_c = Int32.Parse(args[1]);
            var q_e = Int32.Parse(args[2]);
            var q_c = Int32.Parse(args[3]);
            var cipherText = BigInteger.Parse(args[4]);
            var plainText = BigInteger.Parse(args[5]);
            BigInteger e = 65537; // 2^16 + 1
            BigInteger p = GetBigInteger(p_e, p_c);
            BigInteger q = GetBigInteger(q_e, q_c);
            BigInteger n =  BigInteger.Multiply(p, q);
            BigInteger phi_n = BigInteger.Multiply(BigInteger.Subtract(p,1), BigInteger.Subtract(q, 1));

            // d from extended euclidean algorithm

            // public key is (n,e) while private key is (n,d)
            // bezout lemma ax + by = gcd(a,b)
            BigInteger d = Extended_Euclidean_Algorithm_GCD(phi_n, e);

            // if negative, perform modular arithmetic to keep it within unsigned integers
            if (d < 0)
            {
                d = BigInteger.Add(phi_n, d);
            }

            // verify using e*d mod(phi_n) = 1
            BigInteger check = BigInteger.ModPow(BigInteger.Multiply(e, d), 1, phi_n);

            BigInteger encrypted_message = RSA_Encrypt(plainText, n, e);
            BigInteger decrypted_message = RSA_Decrypt(cipherText, n, d);


            Console.Write(decrypted_message.ToString());
            Console.Write(",");
            Console.Write(encrypted_message.ToString());



            /*
             * useful help for encrypt/decrypt: https://www.di-mgt.com.au/rsa_alg.html
             * help with extended euclidean algorithm: https://en.wikipedia.org/wiki/Extended_Euclidean_algorithm
             * 
             */
        }
    }
}
