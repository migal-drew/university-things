using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkFeistel
{
    class Program
    {
        static UInt32 f(UInt32 b, UInt32 key)
        {
            UInt32 s1, s2;

            UInt16 right = (UInt16)b;
            UInt16 left = (UInt16)(((UInt32.MaxValue << 16) & b) >> 16);

            s1 = shl_16((UInt16)(~left), 12); //right half
            s1 = s1 | ((UInt32)shl_16(left, 4) << 16); //add left half
            
            s2 = right; //right half
            s2 = s2 | ((UInt32)shr_16(right, 6) << 16);

            UInt32 xl = s1 ^ s2;
            return xl ^ key;
        }
        
        static UInt32 shr_32(UInt32 block, int n)
        {
            return (block >> n) | (block << 32 - n);
        }

        static UInt32 shl_32(UInt32 block, int n)
        {
            return (block << n) | (block >> 32 - n);
        }

        static UInt16 shr_16(UInt16 block, int n)
        {
            return (UInt16)((block >> n) | (block << 16 - n));
        }

        static UInt16 shl_16(UInt16 block, int n)
        {
            return (UInt16)((block << n) | (block >> 16 - n));
        }

        static UInt32[] encrypt(UInt32[] blocks, UInt32 key, int rounds, bool mode)
        {
            int len = blocks.Length;
            if (len % 2 != 0) 
                throw new Exception("Number of blocks shoud be even!");
            UInt32[] res = new UInt32[len];

            for (int i = 0; i < len; i+=2)
            {
                UInt32 x_left = blocks[i];
                UInt32 x_right = blocks[i + 1];
                for (int j = 0; j < rounds; j++)
                {
                    UInt32 k;
                    if (mode) {
                        k = shl_32(key, j * 5);
                        Console.WriteLine("Encrypting: round #" +
                            j.ToString() + " key=" + k.ToString());
                    }
                    else {
                        k = shl_32(key, (rounds - j - 1) * 5);
                        Console.WriteLine("Decoding:   round #" +
                            j.ToString() + " key=" + k.ToString());
                    }

                    if (j == rounds - 1) //last round
                    {
                        x_right = f(x_left, k) ^ x_right;
                    }
                    else //swap blocks
                    {
                        UInt32 tmp = x_left;
                        x_left = f(x_left, k) ^ x_right;
                        x_right = tmp;
                    }
                }
                res[i]     = x_left;
                res[i + 1] = x_right;
            }

            return res;
        }

        static String blocksToText(UInt32[] blocks)
        {
            string decMes = string.Empty;
            foreach (UInt32 b in blocks)
            {
                UInt32 mask = UInt16.MaxValue;
                mask = mask << 24;
                for (int i = 0; i < 4; i++)
                {
                    UInt32 tmp = b & (mask >> 8 * i);
                    tmp = tmp >> (3 - i) * 8;
                    byte c = (byte)tmp;
                    decMes += (char)c;
                }
            }
            return decMes;
        }

        static void printBinaries(UInt32[] blocks)
        {
            foreach (UInt32 b in blocks)
                Console.WriteLine(Convert.ToString(b, 2).PadLeft(32, '0'));
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            const int symPerBlock = 8;

            String mes = "Lights go out and I can't be saved \n";
            
            if (mes.Length % symPerBlock != 0)
                mes = mes.PadRight(mes.Length + (symPerBlock - mes.Length % symPerBlock));
            Console.WriteLine("Message : " + mes + "%");

            int mesLeng = mes.Length;
            UInt32[] blocks = new UInt32[mesLeng / 4];
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = 0;
                for (int j = 0; j < 4; j++)
                {
                    UInt32 symbol = mes[i * 4 + j];
                    symbol = symbol << 32 - 8 * (j + 1);
                    blocks[i] = blocks[i] | symbol;
                }
            }

            Console.WriteLine("Initial ");
            printBinaries(blocks);
            Console.WriteLine("_______________");
            Console.WriteLine();

            UInt32 key = 5687382;
            int rounds = 8;
            UInt32[] encrypted = encrypt(blocks, key, rounds, true);
            printBinaries(encrypted);
            Console.WriteLine(blocksToText(encrypted));
            Console.WriteLine("_______________");
            Console.WriteLine();

            UInt32[] decrypted = encrypt(encrypted, key, rounds, false);
            printBinaries(decrypted);

            Console.WriteLine("Decoded text ");
            Console.WriteLine("---------------------------");
            Console.WriteLine(blocksToText(decrypted));
            Console.ReadLine();
        }
    }
}
