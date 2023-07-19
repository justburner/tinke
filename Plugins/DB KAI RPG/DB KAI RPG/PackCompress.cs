using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_KAI_RPG
{
	static class PackCompress
	{
        #region [ Constants ]

        public static readonly int[] kOffTab_ZR = {
             -1,  -2,  -3,  -4,  -5,  -6,  -7,  -8,  -9, -10, -11, -12, -13, -14, -15, -16,
            -18, -20, -22, -24, -26, -28, -30, -32, -34, -36, -38, -40, -42, -44, -46, -48
        };
        public static readonly int[] kOffTab_R7 = {
             22, 20, 18, 16, 14, 12, 10,  8,   7,   6,   5,   4,   3,   2,   1,   0,
             -1, -2, -3, -4, -5, -6, -7, -8, -10, -12, -14, -16, -18, -20, -22, -24
        };
        public static readonly int[] kOffTab_R5 = {
              0, -1, -2, -3, -4, -6, -8, -10
        };
        public static readonly int[] kOffTab_SWP = {
            0xFF, 0xFF, 0x2F, 0x4F, 0x2E, 0x30, 0x4D, 0x51,
            0x2C, 0x32, 0x6E, 0x70, 0x53, 0x4B, 0x8F, 0x8D
        };

        #endregion

        #region [ Offset Table ]

        /// <summary>
        /// Generate offsets table
        /// </summary>
        /// <param name="width">texture width</param>
        /// <returns>256 entries table</returns>
        static public int[] GenOffsetTable(int width)
        {
            int[] decTable = new int[256];
            for (int i = 0; i < 32; i++)
            {
                decTable[i] = kOffTab_ZR[i];
            }
            for (int i = 32; i < 256; i++)
            {
                decTable[i] = kOffTab_R5[i >> 5] * width + kOffTab_R7[i & 31];
            }
            for (int i = 2; i < 16; i++)
            {
                int off = kOffTab_SWP[i];
                int tmp = decTable[i];
                decTable[i] = decTable[off];
                decTable[off] = tmp;
            }
            return decTable;
        }

        #endregion

        #region [ Decoder ]

        static public int Decode(byte[] data, ref byte[] pixels, int width, int height)
        {
            MemoryStream st = new MemoryStream(data);
            BinaryReader br = new BinaryReader(st);
            Decode(br, ref pixels, width, height);
            return (int)br.BaseStream.Position;
        }

        static public int Decode(byte[] data, int dataIndex, ref byte[] pixels, int width, int height)
        {
            int len = data.Length - dataIndex;
            MemoryStream st = new MemoryStream(data, dataIndex, len);
            BinaryReader br = new BinaryReader(st);
            Decode(br, ref pixels, width, height);
            return (int)br.BaseStream.Position;
        }

        static public void Decode(BinaryReader br, ref byte[] pixels, int width, int height)
        {
            // Create decoding table
            int[] offTable = GenOffsetTable(width);

            // Packed Texture
            int ptr = 0;
            pixels[ptr++] = br.ReadByte();
            while (ptr < (width * height))
            {
                int index = br.ReadByte();
                if (index < 0x10)
                {
                    // Raw copy
                    while (true)
                    {
                        pixels[ptr++] = br.ReadByte();
                        if (index == 0) break;
                        index--;
                    }
                }
                else if (index == 0x10)
                {
                    // Repeat last
                    byte rVal = pixels[ptr - 1];
                    int rCnt = br.ReadByte() + 3;
                    do
                    {
                        rCnt--;
                        pixels[ptr++] = rVal;
                    } while (rCnt > 0);
                }
                else if (index >= 0x20)
                {
                    // Recall pixels
                    int tab8, tab6;
                    if (index < 0x80)
                    {
                        tab8 = offTable[index & 15];
                        tab6 = (index >> 4) + 1;
                        index = 0;
                    }
                    else
                    {
                        tab8 = offTable[br.ReadByte()];
                        tab6 = (index & 15) + 3;
                        index = (index >> 4) & 7;
                    }
                    if (tab6 != 0)
                    {
                        // Copy recall
                        int refPtr = ptr + tab8;
                        do
                        {
                            tab6--;
                            pixels[ptr++] = pixels[refPtr++];
                        } while (tab6 > 0);
                    }
                    if (index != 0)
                    {
                        // Copy remaining as raw copy
                        do
                        {
                            index--;
                            pixels[ptr++] = br.ReadByte();
                        } while (index > 0);
                    }
                }
                else
                {
                    // Invalid...
                }
            }
        }

        #endregion

        #region [ Encoder ]

        static private int Encode_CheckRepeat(byte[] pixels, int ptr, int len)
        {
            byte value = pixels[ptr - 1];
            int size = 0;
            while (ptr < len)
            {
                if (value != pixels[ptr]) break;
                size++;
                ptr++;
                if (size >= 258) break;
            }
            return size;
        }

        static private int Encode_CheckIndex(byte[] pixels, int ptr, int len, int tab8)
        {
            int size = 0;
            if (tab8 >= 0) return 0;
            while (ptr < len && (ptr + tab8) >= 0 && (ptr + tab8) < len)
            {
                if (pixels[ptr] != pixels[ptr + tab8]) break;
                size++;
                ptr++;
                if (size >= 18) break;
            }
            return size;
        }

        static public byte[] Encode(byte[] pixels, int width, int height)
        {
            MemoryStream st = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(st);
            int len = pixels.Length;

            // Create decoding table
            int[] offTable = GenOffsetTable(width);

            // Packed Texture
            int ptrB = 1, ptrE = 1;
            bw.Write(pixels[0]);
            int raw = 0;

            Action WriteRawData = () =>
            {
                while (raw > 0)
                {
                    int wraw = Math.Min(raw, 16);
                    bw.Write((byte)(wraw - 1));
                    for (int i = 0; i < wraw; i++) bw.Write(pixels[ptrB++]);
                    raw -= wraw;
                }
            };

            // This compression algorithm is not as good as the original but does the job well.
            // The only thing lacking is raw copy after a recall but that would require a complete overhaul.
            while (ptrE < len)
            {
                // Check repeating byte
                int rep = Encode_CheckRepeat(pixels, ptrE, len);

                // Find best index by brute force
                int bestIdx = 0;
                int bestSize = 0;
                for (int i = 0; i < 256; i++)
                {
                    int size = Encode_CheckIndex(pixels, ptrE, len, offTable[i]);
                    if (size > bestSize)
                    {
                        bestIdx = i;
                        bestSize = size;
                    }
                }

                // Any compression resulted >= 3 bytes gets added
                if (rep >= 3 && rep > bestSize)
                {
                    WriteRawData();
                    int wrep = Math.Min(rep, 258);
                    bw.Write((byte)0x10);
                    bw.Write((byte)(wrep - 3));
                    ptrB += wrep;
                    ptrE += wrep;
                }
                else if (bestSize >= 3)
                {
                    WriteRawData();
                    if (bestIdx < 16 && bestSize <= 8)
                    {
                        bw.Write((byte)(((bestSize - 1) << 4) | bestIdx));
                    }
                    else
                    {
                        bw.Write((byte)(0x80 + bestSize - 3));
                        bw.Write((byte)bestIdx);
                    }
                    ptrB += bestSize;
                    ptrE += bestSize;
                }
                else
                {
                    raw++;
                    ptrE++;
                }
            }

            // Tailing raw data
            WriteRawData();

            return st.ToArray();
        }

        #endregion
    }
}
