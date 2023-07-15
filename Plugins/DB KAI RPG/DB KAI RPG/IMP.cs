/*
 * Copyright (C) 2023  Justburner
 *
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with this program.  If not, see <http://www.gnu.org/licenses/>. 
 *
 * Developer: Justburner
 * 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ekona;
using Ekona.Images;
using Ekona.Images.Formats;
using System.Runtime.InteropServices;

namespace DB_KAI_RPG
{
    public class IMP
    {
        #region [ Variables ]

        protected string fileName;
        protected int id;

        public int texWidth;
        public int texHeight;
        public byte[] texRaw;
        public Color[] palette;

        public byte[] data;
        public int[] decTable;

        #endregion

        #region [ Properties ]

        public string FileName { get { return fileName; } }
        public int ID { get { return id; } }

        #endregion

        #region [ Constants ]

        public static readonly int[] kDecTab_ZR = {
             -1,  -2,  -3,  -4,  -5,  -6,  -7,  -8,  -9, -10, -11, -12, -13, -14, -15, -16,
            -18, -20, -22, -24, -26, -28, -30, -32, -34, -36, -38, -40, -42, -44, -46, -48
        };
        public static readonly int[] kDecTab_R7 = {
             22, 20, 18, 16, 14, 12, 10,  8,   7,   6,   5,   4,   3,   2,   1,   0,
             -1, -2, -3, -4, -5, -6, -7, -8, -10, -12, -14, -16, -18, -20, -22, -24
        };
        public static readonly int[] kDecTab_R5 = {
              0, -1, -2, -3, -4, -6, -8, -10
        };
        public static readonly int[] kDecTab_SWP = {
            0xFF, 0xFF, 0x2F, 0x4F, 0x2E, 0x30, 0x4D, 0x51,
            0x2C, 0x32, 0x6E, 0x70, 0x53, 0x4B, 0x8F, 0x8D
        };

        #endregion

        public IMP(string file, int id, string fileName = "")
        {
            this.fileName = fileName;
            this.id = id;
            try
            {
                Read(file);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void GenDecTable(int width)
		{
            decTable = new int[256];
            for (int i = 0; i < 32; i++)
            {
                decTable[i] = kDecTab_ZR[i];
            }
            for (int i = 32; i < 256; i++)
            {
                decTable[i] = kDecTab_R5[i >> 5] * width + kDecTab_R7[i & 31];
            }
            for (int i = 2; i < 16; i++)
            {
                int off = kDecTab_SWP[i];
                int tmp = decTable[i];
                decTable[i] = decTable[off];
                decTable[off] = tmp;
            }
        }

        public void Decode()
		{
            MemoryStream st = new MemoryStream(data);
            BinaryReader br = new BinaryReader(st);

            // Create decoding table
            GenDecTable(texWidth);

            // Packed Texture
            int ptr = 0;
            texRaw[ptr++] = br.ReadByte();
            while (ptr < (texWidth * texHeight))
            {
                int index = br.ReadByte();
                if (index < 0x10)
                {
                    // Raw copy
                    while (true)
                    {
                        texRaw[ptr++] = br.ReadByte();
                        if (index == 0) break;
                        index--;
                    }
                }
                else if (index == 0x10)
                {
                    // Repeat last
                    byte rVal = texRaw[ptr - 1];
                    int rCnt = br.ReadByte() + 3;
                    do
                    {
                        rCnt--;
                        texRaw[ptr++] = rVal;
                    } while (rCnt > 0);
                }
                else if (index >= 0x20)
                {
                    // Recall pixels
                    int tab8, tab6;
                    if (index < 0x80)
                    {
                        tab8 = decTable[index & 15];
                        tab6 = (index >> 4) + 1;
                        index = 0;
                    }
                    else
                    {
                        tab8 = decTable[br.ReadByte()];
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
                            texRaw[ptr++] = texRaw[refPtr++];
                        } while (tab6 > 0);
                    }
                    if (index != 0)
                    {
                        // Copy remaining as raw copy
                        do
                        {
                            index--;
                            texRaw[ptr++] = br.ReadByte();
                        } while (index > 0);
                    }
                }
                else
                {
                    // Invalid...
                }
            }
        }

        protected int Encode_CheckRepeat(int ptr, int len)
		{
            byte value = texRaw[ptr - 1];
            int size = 0;
            while (ptr < len)
            {
                if (value != texRaw[ptr]) break;
                size++;
                ptr++;
                if (size >= 258) break;
            }
            return size;
		}

        protected int Encode_CheckIndex(int ptr, int len, int decIndex)
        {
            int tab8 = decTable[decIndex];
            int size = 0;
            if (tab8 >= 0) return 0;
            while (ptr < len && (ptr + tab8) >= 0 && (ptr + tab8) < len)
            {
                if (texRaw[ptr] != texRaw[ptr + tab8]) break;
                size++;
                ptr++;
                if (size >= 18) break;
            }
            return size;
        }

        public void Encode()
        {
            MemoryStream st = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(st);
            int len = texRaw.Length;

            // Create decoding table
            GenDecTable(texWidth);

            // Packed Texture
            int ptrB = 1, ptrE = 1;
            bw.Write(texRaw[0]);
            int raw = 0;

            Action WriteRawData = () =>
            {
                while (raw > 0)
                {
                    int wraw = Math.Min(raw, 16);
                    bw.Write((byte)(wraw - 1));
                    for (int i = 0; i < wraw; i++) bw.Write(texRaw[ptrB++]);
                    raw -= wraw;
                }
                if (ptrB != ptrE) throw new BadImageFormatException("Internal discrepancy");
            };

            // This compression algorithm is not as good as the original but does the job well.
            // The only thing lacking is raw copy after a recall but that would require a complete overhaul.
            while (ptrE < len)
            {
                // Check repeating byte
                int rep = Encode_CheckRepeat(ptrE, len);

                // Find best index by brute force
                int bestIdx = 0;
                int bestSize = 0;
                for (int i = 0; i < 256; i++)
                {
                    int size = Encode_CheckIndex(ptrE, len, i);
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

            data = st.ToArray();
        }

        public void Read(string fileIn)
        {
            BinaryReader br = new BinaryReader(File.OpenRead(fileIn));

            // Header
            byte[] id = br.ReadBytes(4);
            if (id[0] != 'd' || id[1] != 'I' || id[2] != 'M' || id[3] != 'P')
                throw new BadImageFormatException("File is not dIMP format");
            byte tileSized = br.ReadByte();
            if (tileSized != 0 && tileSized != 1)
                throw new BadImageFormatException("dIMP header malformed");
            byte format = br.ReadByte();
            if (format != 1 && format != 4)
                throw new BadImageFormatException("dIMP format malformed");
            byte widthCode = br.ReadByte();
            byte heightCode = br.ReadByte();

            // Palettes
            if (format == 4)
			{
                // 256 Colors
                palette = Actions.BGR555ToColor(br.ReadBytes(512));
            }
            else
			{
                // 32 Colors !?
                Color[] imgPal = Actions.BGR555ToColor(br.ReadBytes(64));
                palette = new Color[256];
                for (int i = 0; i < 256; i++) palette[i] = imgPal[i & 31];
            }

            // Texture
            texWidth = widthCode * ((tileSized == 1) ? 8 : 2);
            texHeight = heightCode * ((tileSized == 1) ? 8 : 2);
            texRaw = new byte[texWidth * texHeight];

            // Read compressed data
            long remain = br.BaseStream.Length - br.BaseStream.Position;
            data = br.ReadBytes((int)remain);

            // Decode data into texture
            Decode();

            br.Close();
        }

        public void Write(string fileOut)
        {
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileOut));

            // Header
            bw.Write('d'); bw.Write('I'); bw.Write('M'); bw.Write('P');
            if (texWidth >= 510 || texHeight >= 510)
			{
                // Just an hack for now...
                bw.Write((byte)1);
                bw.Write((byte)4);
                bw.Write((byte)(texWidth / 8));
                bw.Write((byte)(texHeight / 8));
            }
            else
			{
                bw.Write((byte)0);
                bw.Write((byte)4);
                bw.Write((byte)(texWidth / 2));
                bw.Write((byte)(texHeight / 2));
            }

            // Palettes
            bw.Write(Actions.ColorToBGR555(palette));

            // Write compressed data
            bw.Write(data);

            bw.Close();
        }
    }
}
