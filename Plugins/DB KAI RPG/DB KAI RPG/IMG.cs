/*
 * Copyright (C) 2022  Justburner
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
    public class IMG
    {
        #region [ Variables ]

        protected string fileName;
        protected int id;

        public int texWidth;
        public int texHeight;
        public byte[] texRaw;
        public Color[][] palette;

        #endregion

        #region [ Properties ]

        public string FileName { get { return fileName; } }
        public int ID { get { return id; } }

        #endregion

        public IMG(string file, int id, string fileName = "")
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
            if (id[0] != 'd' || id[1] != 'I' || id[2] != 'M' || id[3] != 'G')
                throw new BadImageFormatException("File is not dIMG format");

            // DEBUG
            byte zero = br.ReadByte();
            if (zero != 0)
                throw new BadImageFormatException("dIMG header malformed");
            byte format = br.ReadByte();
            if (format != 0 && format != 3)
                throw new BadImageFormatException("dIMG format malformed");
            byte widthCode = br.ReadByte();
            byte heightCode = br.ReadByte();
            int numPal = (format == 0) ? 16 : 1;
            int textureBytes = (widthCode * 2) * (heightCode * 2);

            // Palettes
            palette = new Color[numPal][];
            for (int i = 0; i < numPal; i++)
                palette[i] = Actions.BGR555ToColor(br.ReadBytes(32));

            // Texture
            texRaw = br.ReadBytes(textureBytes);
            texWidth = widthCode * 4;
            texHeight = heightCode * 2;

            br.Close();
        }

        public void Write(string fileOut)
        {
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileOut));
            int format = (palette.Length >= 16) ? 0 : 3;
            int numPals = (format == 0) ? 16 : 1;

            // Header
            bw.Write('d'); bw.Write('I'); bw.Write('M'); bw.Write('G');
            bw.Write((byte)0);
            bw.Write((byte)format);
            bw.Write((byte)(texWidth / 4));
            bw.Write((byte)(texHeight / 2));

            // Palettes
            for (int i = 0; i < numPals; i++)
                bw.Write(Actions.ColorToBGR555(palette[i]));

            // Textures
            bw.Write(texRaw);

            bw.Close();
        }
    }
}
