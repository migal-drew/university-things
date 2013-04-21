using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Steganographia
{
    class Program
    {
        static void getLuminosity(Bitmap b, ref int[] lumin)
        {
            const double forRed = 0.3;
            const double forGreen = 0.6;
            const double forBlue = 0.1;

            for (int i = 0; i < b.Height; i++)
                for (int j = 0; j < b.Width; j++)
                {
                    Color c = b.GetPixel(j, i);
                    int lum = (int)(forRed * c.R + forGreen * c.G + forBlue * c.B);
                    lumin[b.Width * i + j] = lum;
                }
        }

        static void codeMessage(ref Bitmap b, int[] lumin, byte[] message, int sigma, double lambda)
        {
            if (message.Length > (int)(b.Height / sigma - 1) * (b.Width / sigma - 1))
            {
                Console.WriteLine("Message is too long!");
                return;
            }

            int leng = message.Length;
            int k = 0;
            for (int i = sigma; i + sigma < b.Height; i+=sigma)
                for (int j = sigma; j + sigma < b.Width; j+=sigma)
                {
                    if (k == leng) return;

                    int lum = lumin[b.Width * i + j];
                    Color c = b.GetPixel(j, i);
                    int delta = (int)(lambda * lum);
                    if (message[k++] == 0)
                        b.SetPixel(j, i, Color.FromArgb(c.R, c.G, c.B - delta));
                    else
                        b.SetPixel(j, i, Color.FromArgb(c.R, c.G, c.B + delta));
                    Color tmp = b.GetPixel(j, i);
                }
        }

        //Formats a byte[] into a binary string (010010010010100101010)
        public static string Format(byte[] data)
        {
            //storage for the resulting string
            string result = string.Empty;
            //iterate through the byte[]
            foreach (byte value in data)
            {
                //storage for the individual byte
                string binarybyte = Convert.ToString(value, 2).PadLeft(8, '0');
                
                //append the binarybyte to the result
                result += binarybyte;
            }
            //return the result
            return result;
        }

        static int getSum(Bitmap b, int col, int row, int sigma)
        {
            int sum = 0;
            for (int i = 1; i <= sigma; i++)
            {
                Color cUp    = b.GetPixel(col, row - i);
                Color cDown  = b.GetPixel(col, row + i);
                Color cLeft  = b.GetPixel(col - i, row);
                Color cRight = b.GetPixel(col + i, row);
                sum += cUp.B + cDown.B + cLeft.B + cRight.B;
            }
            return sum / (4 * sigma);
        }

        static string decode(Bitmap b, int sigma, int byteCount)
        {
            string result = String.Empty;
            int w = b.Width;
            int h = b.Height;
            int k = 1;
            for (int i = sigma; i + sigma < b.Height; i+=sigma)
                for (int j = sigma; j + sigma < b.Width; j += sigma)
                {
                    if (byteCount < k) return result; else k++;
                    Color c = b.GetPixel(j, i);
                    int realBlue = c.B;
                    int predictedBlue = getSum(b, j, i, sigma);
                    if (predictedBlue >= realBlue)
                        result += '0';
                    else
                        result += '1';
                 }
            return result;
        }

        static void Main(string[] args)
        {
            Bitmap btmp;
            btmp = new Bitmap(args[0]);
            //btmp = new Bitmap("lena.bmp");
            Console.WriteLine(btmp.PixelFormat.ToString());
            int h = btmp.Height;
            int w = btmp.Width;
            int[] luminosity = new int[w * h];
            getLuminosity(btmp, ref luminosity);

            string message = 
                "Well, you're just across the street" +
                "Looks a mile to my feet" + 
                "I want to go to you" +
                "Funny how I'm nervous still" +
                "I've always been the easy kill" +
                "I guess I always will";

            byte[] asciiValues = Encoding.ASCII.GetBytes(message);
            string binStr = Format(asciiValues);
            // Binary representation of ascii symbols
            int len = binStr.Length;
            byte[] binMessage = new byte[len];
            for (int i = 1; i < len; i++)
                binMessage[i] = Convert.ToByte(binStr[i].ToString());

            Console.WriteLine("Message: " + message);
            Console.WriteLine("Binary representation");

            for (int i = 0; i < binMessage.Length; i++)
            {
                if (i % 8 == 0)
                    Console.Write(" ");
                Console.Write(binMessage[i]);
            }
            Console.WriteLine();

            int sigma = 2;
            double lambda = 0.1;
            codeMessage(ref btmp, luminosity, binMessage, sigma, lambda);
            btmp.Save("result.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

            //btmp = new Bitmap("result.bmp");
            Console.WriteLine(btmp.PixelFormat.ToString());
            string decodedStr = decode(btmp, sigma, message.Length * 8);

            Console.WriteLine("\nDecoded binary sequence:");
            for (int i = 0; i < decodedStr.Length; i++)
            {
                if (i % 8 == 0)
                    Console.Write(" ");
                Console.Write(decodedStr[i]);
            }
            Console.WriteLine("\nCharacter representation");

            string decodedMessage = String.Empty;
            for (int i = 0; i < decodedStr.Length; i+=8)
            {
                int code = Convert.ToInt32(decodedStr.Substring(i, 8), 2);
                decodedMessage += (char)code;
            }
            Console.WriteLine(decodedMessage);
            //codeMessage(ref btmp
            //foreach (byte b in asciiValues)
              //  Console.WriteLine(b);

            /*
            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                {
                    int c = luminosity[i * h + j];
                    btmp.SetPixel(j, i, Color.FromArgb(c, c, c));
                }

            btmp.Save("luminosityLena.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            */

        }
    }
}
