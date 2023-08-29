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
    public class BST
    {
        #region [ Variables ]

        protected string fileName;
        protected int id;

        public struct Offsets
        {
            public ushort code;
            public ushort addr512;
            public ushort size512;
        }

        public class Layer
        {
            public ushort code;
            public sbyte posx;
            public sbyte posy;
            public int width;
            public int height;
            public Color[] palette;
            public byte[] pixels;
        };

        public List<Layer> layers;

        #endregion

        #region [ Properties ]

        public string FileName { get { return fileName; } }
        public int ID { get { return id; } }

        #endregion

        public BST(string file, int id, string fileName = "")
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

            byte[] id = br.ReadBytes(4);
            if (id[0] != 'd' || id[1] != 'B' || id[2] != 'S' || id[3] != 'T')
                throw new BadImageFormatException("File is not dBST format");
            int numLayers = br.ReadUInt16();

            Offsets[] offsets = new Offsets[numLayers];
            for (int i = 0; i < numLayers; i++)
            {
                offsets[i].code = br.ReadUInt16();
                offsets[i].addr512 = br.ReadUInt16();
                offsets[i].size512 = br.ReadUInt16();
            }

            layers = new List<Layer>();
            for (int i = 0; i < numLayers; i++)
            {
                Layer layer = new Layer();
                long addr512 = offsets[i].addr512 << 9;
                br.BaseStream.Seek(addr512, SeekOrigin.Begin);

                layer.posx = br.ReadSByte();
                layer.posy = br.ReadSByte();
                int width = br.ReadByte();
                int height = br.ReadByte();

                layer.code = offsets[i].code;
                layer.width = width;
                layer.height = height;

                int numColors = br.ReadUInt16();
                br.BaseStream.Seek(-2, SeekOrigin.Current); // Hack to inject transparent color
                layer.palette = Actions.BGR555ToColor(br.ReadBytes(numColors * 2));
                layer.palette[0] = Color.Transparent;

                layer.pixels = new byte[width * height];
                PackCompress.Decode(br, ref layer.pixels, width, height);

                layers.Add(layer);
            }

            br.Close();

            SortLayers();
        }

        public void Write(string fileOut)
        {
            int numBsts = layers.Count;
            if (numBsts > 84)
			{
                MessageBox.Show("Saving more than 84 images isn't supported.\nWrite aborted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
			}

            BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileOut));

            Action PadTo512 = () => {
                int pad = (512 - (int)bw.BaseStream.Position) & 511;
                for (int i = 0; i < pad; i++) bw.Write((byte)0);
            };

            // Header
            bw.Write('d'); bw.Write('B'); bw.Write('S'); bw.Write('T');
            int numLayers = layers.Count;
            bw.Write((ushort)numLayers);

            Offsets[] offsets = new Offsets[numLayers];
            PadTo512();  // Reserve with dummy zeroes

            // Write content
            for (int i = 0; i < numLayers; i++)
            {
                Layer layer = GetLayer(i);
                offsets[i].code = layer.code;
                offsets[i].addr512 = (ushort)(bw.BaseStream.Position >> 9);

                bw.Write((sbyte)layer.posx);
                bw.Write((sbyte)layer.posy);
                bw.Write((byte)layer.width);
                bw.Write((byte)layer.height);
                bw.Write((ushort)layer.palette.Length);
                bw.Write(Actions.ColorToBGR555(layer.palette.Skip(1).ToArray()));
                byte[] data = PackCompress.Encode(layer.pixels, layer.width, layer.height);
                bw.Write(data);
                PadTo512();

                offsets[i].size512 = (ushort)(bw.BaseStream.Position >> 9);
                offsets[i].size512 -= offsets[i].addr512;
            }

            // Rewrite header
            bw.BaseStream.Seek(6, SeekOrigin.Begin);
            for (int i = 0; i < numLayers; i++)
            {
                bw.Write(offsets[i].code);
                bw.Write(offsets[i].addr512);
                bw.Write(offsets[i].size512);
            }
            bw.BaseStream.Seek(0, SeekOrigin.End);

            bw.Close();
        }

        public int Count
        {
            get { return layers.Count; }
        }

        public void SortLayers()
        {
            layers.Sort((x, y) => x.code.CompareTo(y.code));
        }

        public Color[] GetPalette(int index)
        {
            if (index < 0 || index >= layers.Count)
                return null;
            return layers[index].palette;
        }

        public Layer GetLayer(int index)
        {
            if (index < 0 || index >= layers.Count)
                return null;
            return layers[index];
        }

        public bool RenameLayer(int index, ushort code)
		{
            if (index < 0 || index >= layers.Count)
                return false;

            // Check if already exist
            for (int i = 0; i < layers.Count; i++)
            {
                if (i == index) continue;
                if (layers[i].code == code) return false;
            }

            layers[index].code = code;
            SortLayers();
            return true;
        }

        public int NewLayer(ushort code)
        {
            // Check if already exist
            foreach (var slayer in layers)
            {
                if (slayer.code == code) return -1;
            }

            Layer layer = new Layer();
            layer.code = code;
            layer.posx = 0;
            layer.posy = 0;
            layer.width = 4;
            layer.height = 4;
            layer.palette = new Color[4];
            layer.palette[0] = Color.Transparent;
            for (int i = 1; i < 4; i++) layer.palette[i] = Color.Black;
            layer.pixels = new byte[4 * 4];

            layers.Add(layer);
            SortLayers();

            return layers.Count - 1;
        }

        public bool DeleteLayer(int index)
        {
            if (index < 0 || index >= layers.Count)
                return false;
            layers.RemoveAt(index);
            return true;
        }
    }
}
