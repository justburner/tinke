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
    public class MAP
    {
        #region [ Variables ]

        protected string fileName;
        protected int id;

        public int texWidth;
        public int texHeight;
        public byte[] texImg;
        public byte[] texMsk;
        public Color[] palette;

        public byte[] dataImg;
        public byte[] dataMsk;
        public byte[] dataEx;

        #endregion

        #region [ Properties ]

        public string FileName { get { return fileName; } }
        public int ID { get { return id; } }

        #endregion

        public MAP(string file, int id, string fileName = "")
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

        public void Read(string fileIn)
        {
            BinaryReader br = new BinaryReader(File.OpenRead(fileIn));

            // Header
            byte[] id = br.ReadBytes(4);
            if (id[0] != 'd' || id[1] != 'M' || id[2] != 'A' || id[3] != 'P')
                throw new BadImageFormatException("File is not dMAP format");
            byte zero1 = br.ReadByte();
            byte zero2 = br.ReadByte();
            if (zero1 != 0 || zero2 != 0)
                throw new BadImageFormatException("dMAP header malformed");
            byte widthCode = br.ReadByte();
            byte heightCode = br.ReadByte();
            int mskAddr = br.ReadInt32(); //  8
            int dataExS = br.ReadInt32(); // 12

            // Read all data
            long remain = br.BaseStream.Length - br.BaseStream.Position;
            byte[] data = br.ReadBytes((int)remain);

            // Extra data
            if (dataExS != 0)
                dataEx = data.Take(dataExS).ToArray();
            else
                dataEx = new byte[0];

            // Palettes
            palette = Actions.BGR555ToColor(data.Skip(dataExS).Take(512).ToArray());

            // Textures
            texWidth = widthCode * 8;
            texHeight = heightCode * 8;
            texImg = new byte[texWidth * texHeight];
            texMsk = new byte[texWidth * texHeight];

            // Decompress images
            int sizeImg = PackCompress.Decode(data, dataExS + 512, ref texImg, texWidth, texHeight);
            dataImg = data.Skip(dataExS + 512).Take(sizeImg).ToArray();
            int sizeMsk = PackCompress.Decode(data, mskAddr - 16, ref texMsk, texWidth, texHeight);
            dataMsk = data.Skip(mskAddr - 16).Take(sizeMsk).ToArray();

            br.Close();
        }

        public void Write(string fileOut)
        {
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileOut));

            uint dataExS = (uint)dataEx.Length;
            uint imgAddr = 16U + dataExS + 512U + (uint)dataImg.Length;

            // Header
            bw.Write('d'); bw.Write('M'); bw.Write('A'); bw.Write('P');
            bw.Write((byte)0);
            bw.Write((byte)0);
            bw.Write((byte)(texWidth / 8));
            bw.Write((byte)(texHeight / 8));
            bw.Write(imgAddr); //  8
            bw.Write(dataExS); // 12

            // Extra data
            bw.Write(dataEx);

            // Palettes
            bw.Write(Actions.ColorToBGR555(palette));

            // Write compressed data
            bw.Write(dataImg);
            bw.Write(dataMsk);

            bw.Close();
        }

        public void RecompressImg()
        {
            dataImg = PackCompress.Encode(texImg, texWidth, texHeight);
        }
        public void RecompressMsk()
        {
            dataMsk = PackCompress.Encode(texMsk, texWidth, texHeight);
        }
    }
}
