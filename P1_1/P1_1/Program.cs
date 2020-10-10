using System;
using System.IO;
using System.Collections;

namespace P1_1
{
    class Program
    {
        public static byte[] Solve(byte[] inputBytes, byte[] bmpBytes)
        {
            // we need a function that takes the input, turns it into bits, then takes two bits at at time, hides the two in each byte (two bits per byte)
            // byte FF

            //get bits
            BitArray inputbits = new BitArray(inputBytes);
            // 48 color bytes, 74 bytes in total if you add in the header bytes
            int j = 74 - 48;
            byte[] XORbmpBytes = new byte[bmpBytes.Length];
            byte[] CombinedIntoByte = new byte[1];
            BitArray TwoBits = new BitArray(2);
            bmpBytes.CopyTo(XORbmpBytes, 0);

            for (int step = 0; step < inputbits.Length; step += 8)
            {
                for (int i = step + 7; i >= step; i -= 2)
                {
                    TwoBits[1] = inputbits[i];
                    TwoBits[0] = inputbits[i - 1];
                    // convert two bits to byte
                    TwoBits.CopyTo(CombinedIntoByte, 0);
                    //then XOR with Byte
                    var CurrentBMPByte = bmpBytes[j];
                    var NewByte = CombinedIntoByte[0];
                    byte XORByte = (byte)(NewByte ^ CurrentBMPByte);
                    // set the XORByte in the new array
                    XORbmpBytes[j] = XORByte;
                    j++;
                }
            }
            return XORbmpBytes;
        }

        static void Main(string[] args)
        {
            // (Blue Green Red)
            // Also note that colors are defined from left to right and bottom to top
            byte[] bmpBytes = new byte[]
            {
                0x42,0x4D,0x4C,0x00,0x00,0x00,0x00,0x00,
                0x00,0x00,0x1A,0x00,0x00,0x00,0x0C,0x00,
                0x00,0x00,0x04,0x00,0x04,0x00,0x01,0x00,0x18,0x00,
                0x00,0x00,0xFF,
                0xFF,0xFF,0xFF,
                0x00,0x00,0xFF,
                0xFF,0xFF,0xFF,
                0xFF,0xFF,0xFF,
                0x00,0x00,0x00,
                0xFF,0xFF,0xFF,
                0x00,0x00,0x00,
                0xFF,0x00,0x00,
                0xFF,0xFF,0xFF,
                0xFF,0x00,0x00,
                0xFF,0xFF,0xFF,
                0xFF,0xFF,0xFF,
                0x00,0x00,0x00,
                0xFF,0xFF,0xFF,
                0x00,0x00,0x00
            };


            //Console.WriteLine(Convert.ToByte("F8", 16));
            //Console.WriteLine(BitConverter.ToString(bmpBytes));
            //Console.WriteLine(BitConverter.ToString(bmpBytes).Replace("-", " "));
            //Console.WriteLine(Convert.ToByte("FF", 16));

            // preprocess the input into a byte array
            //var input = "B1 FF FF CC 98 80 09 EA 04 48 7E C9";
            //dotnet run "B1 FF FF CC 98 80 09 EA 04 48 7E C9"
            string input = args[0];
            var input_split = input.Split(' ');
            byte[] inputBytes = new byte[input_split.Length];
            int i = 0;
            foreach (string item in input_split)
            {
                inputBytes.SetValue(Convert.ToByte(item, 16), i);
                i++;
            }

            byte[] ans = Solve(inputBytes, bmpBytes);
            Console.WriteLine(BitConverter.ToString(ans).Replace("-", " "));

            //WriteToFile(ans, "ans.bmp");
            //WriteToFile(bmpBytes, "original.bmp");
        }

        public static void WriteToFile(byte[] bmpBytes, string savefilename)
        {
            string filename = @$"C:\Users\spencerw\projects\cse-539\Steganography and Cryptanalysis Project\Steganography and Cryptanalysis Project\{savefilename}";
            try
            {
                using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bmpBytes, 0, bmpBytes.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
            }
        }
    }
}
