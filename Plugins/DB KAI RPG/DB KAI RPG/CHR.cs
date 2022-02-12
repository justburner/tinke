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

        // Debugging
        public int adjust_offset = 0; // DEBUG

		#endregion

		#region [ Properties ]

		public string FileName { get { return fileName; } }
        public int ID { get { return id; } }

		#endregion

		#region [ Character Data ]

		public struct Sprite
        {
            public byte flags;
            public List<Layer> layers;
            public byte unknown1, unknown2;
            public byte unknown3, unknown4;

            public long debugOffset; // DEBUG
            public long debugRemain; // DEBUG
        };
        public struct Layer
        {
            public sbyte x;
            public sbyte y;
            public ushort tile;
            public byte ctrl;

            public bool VerticalFlip { get { return (ctrl & 32) == 32; } }
            public bool HorizontalFlip { get { return (ctrl & 16) == 16; } }

            public int Width
            {
                get
                {
                    switch (ctrl & 0xCC)
                    {
                        case 0x40: return 2;
                        case 0x80: return 4;
                        case 0xC0: return 8;
                        case 0x04: return 2;
                        case 0x44: return 4;
                        case 0x84: return 4;
                        case 0xC4: return 8;
                        case 0x88: return 2;
                        case 0xC8: return 4;
                        default: return 1;
                    }
                }
            }

            public int Height
            {
                get
                {
                    switch (ctrl & 0xCC)
                    {
                        case 0x40: return 2;
                        case 0x80: return 4;
                        case 0xC0: return 8;
                        case 0x84: return 2;
                        case 0xC4: return 4;
                        case 0x08: return 2;
                        case 0x48: return 4;
                        case 0x88: return 4;
                        case 0xC8: return 8;
                        default: return 1;
                    }
                }
            }

            public override string ToString()
            {
                return string.Format("{2}: {0},{1} {6}{7} {4}x{5} ${3:X02}", x, y, tile, ctrl, Width * 8, Height * 8, VerticalFlip ? "V" : "-", HorizontalFlip ? "H" : "-");
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
                    int numLayers = br.ReadByte();
                    sprite.flags = br.ReadByte();
                    sprite.debugOffset = br.BaseStream.Position;
                    sprite.layers = new List<Layer>();
                    for (int j = 0; j < numLayers; j++)
                    {
                        Layer layer = new Layer();
                        layer.x = br.ReadSByte();
                        layer.y = br.ReadSByte();
                        layer.tile = br.ReadByte();
                        layer.ctrl = br.ReadByte();
                        switch (layer.ctrl & 3)
                        {
                            case 0: /* normal */ break;
                            case 1: layer.tile += 256; break;
                            case 2: /* semi-transparent */ break;
                            case 3: br.BaseStream.Seek(adjust_offset, SeekOrigin.Current); layer.tile += 256; break;
                        }
                        sprite.layers.Add(layer);
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
            bool vflip = part.VerticalFlip;
            bool hflip = part.HorizontalFlip;
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
