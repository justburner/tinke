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

        #endregion

        #region [ Properties ]

        public string FileName { get { return fileName; } }
        public int ID { get { return id; } }

        #endregion

        public IMP(int width, int height)
        {
            this.fileName = "";
            this.id = -1;

            // Create dummy IMP
            this.texWidth = width;
            this.texHeight = height;
            this.texRaw = new byte[width * height];
            this.palette = new Color[256];
            for (int i = 0; i < 256; i++) this.palette[i] = Color.Black;
            this.Recompress();
        }

        public IMP(BinaryReader br)
        {
            this.fileName = "";
            this.id = -1;
            try
            {
                Read(br, false);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

        public void Read(string fileIn)
        {
            BinaryReader br = new BinaryReader(File.OpenRead(fileIn));
            Read(br);
            br.Close();
        }

        public void Write(string fileOut)
        {
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileOut));
            Write(bw);
            bw.Close();
        }

        public void Read(BinaryReader br, bool readAll = true)
        {
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

            if (readAll)
			{
                // Read all compressed data
                long remain = br.BaseStream.Length - br.BaseStream.Position;
                data = br.ReadBytes((int)remain);

                // Decompress data into texture
                PackCompress.Decode(data, ref texRaw, texWidth, texHeight);
            }
            else
			{
                // Decompress data but keep track of stream position
                int start = (int)br.BaseStream.Position;
                PackCompress.Decode(br, ref texRaw, texWidth, texHeight);
                int end = (int)br.BaseStream.Position;

                // Go back to read as raw data
                br.BaseStream.Seek(start, SeekOrigin.Begin);
                data = br.ReadBytes(end - start);
            }
        }

        public void Write(BinaryWriter bw)
        {
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
        }

        public void Recompress()
        {
            data = PackCompress.Encode(texRaw, texWidth, texHeight);
        }
    }
}
