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
    public class CHR
    {
		#region [ Variables ]

		protected string fileName;
        protected int id;

        public byte chrFlags;
        public Color[][] palette;
        public byte[][] tiles;
        public byte[] chrData;
        public long chrDataPos;
        public List<Sprite> sprites;

		#endregion

		#region [ Properties ]

		public string FileName { get { return fileName; } }
        public int ID { get { return id; } }

		#endregion

		#region [ Character Data ]

		public struct Sprite
        {
            public List<Layer> layers;
            public bool extended;

            public byte flags1, flags2;
            public byte unknown1, unknown2;
            public byte unknown3, unknown4;

            public long debugOffset; // DEBUG
            public long debugRemain; // DEBUG
        };
        public struct Layer
        {
            public int x;
            public int y;
            public ushort tile;

            public byte objsize;
            public byte objshape;

            public bool vflip;
            public bool hflip;

            public bool translucent;

            public string datastr;

            public int Width
            {
                get
                {
                    switch (objsize * 16 + objshape)
                    {
                        case 0x10: return 2;
                        case 0x20: return 4;
                        case 0x30: return 8;
                        case 0x01: return 2;
                        case 0x11: return 4;
                        case 0x21: return 4;
                        case 0x31: return 8;
                        case 0x22: return 2;
                        case 0x32: return 4;
                        default: return 1;
                    }
                }
            }

            public int Height
            {
                get
                {
                    switch (objsize * 16 + objshape)
                    {
                        case 0x10: return 2;
                        case 0x20: return 4;
                        case 0x30: return 8;
                        case 0x21: return 2;
                        case 0x31: return 4;
                        case 0x02: return 2;
                        case 0x12: return 4;
                        case 0x22: return 4;
                        case 0x32: return 8;
                        default: return 1;
                    }
                }
            }

            public override string ToString()
            {
                return string.Format("{2}: {0},{1} {5}{6} {3}x{4}", x, y, tile, Width * 8, Height * 8, vflip ? "V" : "-", hflip ? "H" : "-");
            }
        };

        #endregion

        public CHR(string file, int id, string fileName = "")  {
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
            if (id[0] != 'd' || id[1] != 'C' || id[2] != 'H' || id[3] != 'R')
                throw new BadImageFormatException("File is not dCHR format");
            chrFlags = br.ReadByte();
            if (chrFlags != 0 && chrFlags != 2)
                throw new NotSupportedException("dCHR version must be 0 or 2");
            byte numPal = br.ReadByte();
            ushort numTiles = br.ReadUInt16();

            // Palettes
            palette = new Color[numPal][];
            for (int i = 0; i < numPal; i++)
                palette[i] = Actions.BGR555ToColor(br.ReadBytes(32));

            // Tiles
            tiles = new byte[numTiles][];
            for (int i = 0; i < numTiles; i++)
                tiles[i] = br.ReadBytes(32);

            // Character data
            chrDataPos = br.BaseStream.Position;
            chrData = br.ReadBytes((int)(br.BaseStream.Length - chrDataPos));

            br.Close();

            // Decode character data
            DecodeCharacterData();
        }

        public void Write(string fileOut)
        {
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileOut));

            // Header
            bw.Write('d'); bw.Write('C'); bw.Write('H'); bw.Write('R');
            bw.Write(chrFlags);
            bw.Write((byte)palette.Length);
            bw.Write((ushort)tiles.Length);

            // Palettes
            for (int i = 0; i < palette.Length; i++)
                bw.Write(Actions.ColorToBGR555(palette[i]));

            // Tiles
            for (int i = 0; i < tiles.Length; i++)
                bw.Write(tiles[i]);

            // Character data
            bw.Write(chrData);

            bw.Close();
        }
        
        public void DecodeCharacterData()
		{
            Stream stream = new MemoryStream(chrData);
            BinaryReader br = new BinaryReader(stream);

            // This stuff is still not working properly
            sprites = new List<Sprite>();
            if ((chrFlags & 2) == 2)
            {
                // Read data header
                ushort numSprites = br.ReadUInt16();
                ushort zeroValue = br.ReadUInt16();
                if (zeroValue != 0)
                    throw new FormatException("Zero expected in data");

                long animOffsetPos = br.BaseStream.Position;

                // Read sprites offset
                ushort[] offsets = new ushort[numSprites];
                for (int i = 0; i < numSprites; i++)
                    offsets[i] = br.ReadUInt16();

                // Read sprites data
                for (int i = 0; i < numSprites; i++)
                {
                    br.BaseStream.Seek(animOffsetPos + offsets[i], SeekOrigin.Begin);
                    Sprite sprite = new Sprite();
                    sprite.debugOffset = br.BaseStream.Position;
                    int numLayers = br.ReadByte();

                    if (numLayers == 16)
					{
                        sprite.flags1 = br.ReadByte();
                        sprite.flags2 = br.ReadByte();
                        numLayers = br.ReadByte();

                        sprite.extended = true;
                        sprite.layers = new List<Layer>();
                        for (int j = 0; j < numLayers; j++)
                        {
                            Layer layer = new Layer();
                            ushort halfwd0 = br.ReadUInt16();
                            ushort halfwd1 = br.ReadUInt16();
                            ushort halfwd2 = br.ReadUInt16();

                            layer.x = ((halfwd1 & 0x800) != 0) ? (int)((halfwd1 & 0xFFF) | 0xFFFFF000) : (halfwd1 & 0x7FF);
                            layer.y = ((halfwd0 & 0x200) != 0) ? (int)((halfwd0 & 0x3FF) | 0xFFFFFC00) : (halfwd0 & 0x1FF);
                            layer.tile = halfwd2;
                            layer.objsize = (byte)((halfwd1 & 0xC000) >> 14);
                            layer.objshape = (byte)((halfwd0 & 0xC000) >> 14);
                            layer.vflip = (halfwd1 & 0x2000) != 0;
                            layer.hflip = (halfwd1 & 0x1000) != 0;
                            layer.translucent = (halfwd0 & 0x0400) != 0;

                            layer.datastr = string.Format("{0:X04}:{1:X04}:{2:X04}", halfwd0, halfwd1, halfwd2);
                            sprite.layers.Add(layer);
                        }
                    }
                    else
					{
                        sprite.flags1 = br.ReadByte();
                        sprite.flags2 = 0;

                        sprite.extended = false;
                        sprite.layers = new List<Layer>();
                        for (int j = 0; j < numLayers; j++)
                        {
                            Layer layer = new Layer();
                            sbyte byte0 = br.ReadSByte();
                            sbyte byte1 = br.ReadSByte();
                            byte byte2 = br.ReadByte();
                            byte byte3 = br.ReadByte();

                            layer.x = byte0;
                            layer.y = byte1;
                            layer.tile = byte2;
                            layer.objsize = (byte)((byte3 & 0xC0) >> 6);
                            layer.objshape = (byte)((byte3 & 0x0C) >> 2);
                            layer.vflip = (byte3 & 0x20) != 0;
                            layer.hflip = (byte3 & 0x10) != 0;
                            layer.translucent = (byte3 & 0x02) != 0;
                            if ((byte3 & 0x01) != 0) layer.tile += 256;

                            layer.datastr = string.Format("{0:X02}:{1:X02}:{2:X02}:{3:X02}", byte0, byte1, byte2, byte3);
                            sprite.layers.Add(layer);
                        }
                    }

                    sprite.unknown1 = br.ReadByte();
                    sprite.unknown2 = br.ReadByte();
                    sprite.unknown3 = br.ReadByte();
                    sprite.unknown4 = br.ReadByte();
                    if (i + 1 < numSprites)
                    {
                        sprite.debugRemain = animOffsetPos + offsets[i + 1] - br.BaseStream.Position;
                    }
                    else
                    {
                        sprite.debugRemain = br.BaseStream.Length - br.BaseStream.Position;
                    }
                    sprites.Add(sprite);
                }
            }
        }

        public void DrawLayer(Bitmap bmp, int offset_x, int offset_y, ref Layer part, int paletteSlot = 0, bool highlighted = true)
        {
            int startID = part.tile;
            int twidth = part.Width;
            int theight = part.Height;
            bool vflip = part.vflip;
            bool hflip = part.hflip;
            Color[] pal = palette[paletteSlot];

            if (!highlighted)
            {
                pal = new Color[16];
                for (int i = 0; i < 16; i++)
                    pal[i] = Color.FromArgb(palette[paletteSlot][i].ToArgb() & 0x7FFFFFFF);
            }

            offset_x += part.x;
            offset_y += part.y;
            for (int ty = 0; ty < theight; ty++)
            {
                for (int tx = 0; tx < twidth; tx++)
                {
                    int id = startID + (vflip ? (theight - 1 - ty) : ty) * twidth + (hflip ? (twidth - 1 - tx) : tx);
                    if (id >= tiles.Length) continue;
                    byte[] tile = tiles[id];

                    int offsetY = ty * 8 + offset_y + (vflip ? 7 : 0);
                    for (int y = 0; y < 8; y++)
                    {
                        int offsetX = tx * 8 + offset_x + (hflip ? 6 : 0);
                        {
                            for (int x = 0; x < 4; x++)
                            {
                                if (offsetX >= 0 && offsetY >= 0 && offsetX < (bmp.Width-1) && offsetY < bmp.Height)
                                {
                                    byte value = tile[y * 4 + x];
                                    int valueL = value & 15;
                                    int valueH = value >> 4;
                                    if (valueL > 0)
                                        bmp.SetPixel(offsetX + (hflip ? 1 : 0), offsetY, pal[valueL]);
                                    if (valueH > 0)
                                        bmp.SetPixel(offsetX + (hflip ? 0 : 1), offsetY, pal[valueH]);
                                }
                                offsetX += hflip ? -2 : 2;
                            }
                        }
                        offsetY += vflip ? -1 : 1;
                    }
                }
            }
        }

    }
}
