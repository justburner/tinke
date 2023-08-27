/*
 * Copyright (C) 2022-2023  Justburner
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

        public class Layer
        {
            public int x;
            public int y;
            public ushort tile;

            // size[0:1] shape[2:3]
            //  8x8, 16x16, 32x32, 64x64
            // 16x8,  32x8, 32x16, 64x32
            // 8x16,  8x32, 16x32, 32x64
            public int objsize;

            public bool vflip;
            public bool hflip;

            public bool translucent;

            public static Layer Blank()
            {
                Layer layer = new Layer();
                return layer;
            }

            public Layer Clone()
            {
                Layer layer = new Layer();
                layer.x = x;
                layer.y = y;
                layer.tile = tile;
                layer.objsize = objsize;
                layer.vflip = vflip;
                layer.hflip = hflip;
                layer.translucent = translucent;
                return layer;
            }

            public int Width
            {
                get
                {
                    switch (objsize)
                    {
                        case 1: return 2; // 16x16
                        case 2: return 4; // 32x32
                        case 3: return 8; // 64x64
                        case 4: return 2; // 16x8
                        case 5: return 4; // 32x8
                        case 6: return 4; // 32x16
                        case 7: return 8; // 64x32
                        case 10: return 2; // 16x32
                        case 11: return 4; // 32x64
                        default: return 1;
                    }
                }
            }

            public int Height
            {
                get
                {
                    switch (objsize)
                    {
                        case 1: return 2; // 16x16
                        case 2: return 4; // 32x32
                        case 3: return 8; // 64x64
                        case 6: return 2; // 32x16
                        case 7: return 4; // 64x32
                        case 8: return 2; // 8x16
                        case 9: return 4; // 8x32
                        case 10: return 4; // 16x32
                        case 11: return 8; // 32x64
                        default: return 1;
                    }
                }
            }

            public override string ToString()
            {
                return string.Format("{2}: {0},{1} {5}{6}{7} {3}x{4}", x, y, tile, Width * 8, Height * 8, vflip ? "V" : "-", hflip ? "H" : "-", translucent ? "t" : "-");
            }
        }

        public class Frame
        {
            public List<Layer> layers;
            public ushort ticks;

            public byte extrasize;
            public byte[] extradata;

            public static Frame Blank()
            {
                Frame frame = new Frame();
                frame.layers = new List<Layer>();
                frame.ticks = 0;
                frame.extrasize = 0;
                frame.extradata = new byte[2];

                Layer layer = new Layer();
                frame.layers.Add(layer);

                return frame;
            }

            public Frame Clone()
            {
                Frame frame = new Frame();
                frame.layers = new List<Layer>();
                foreach (var layer in layers)
                    frame.layers.Add(layer.Clone());
                frame.ticks = ticks;
                frame.extrasize = extrasize;
                frame.extradata = extradata.ToArray();

                return frame;
            }
        }

        public class Sprite
        {
            public List<Frame> frames;

            public static Sprite Blank()
            {
                Sprite sprite = new Sprite();
                sprite.frames = new List<Frame>();
                sprite.frames.Add(Frame.Blank());

                return sprite;
            }

            public Sprite Clone()
            {
                Sprite sprite = new Sprite();
                sprite.frames = new List<Frame>();
                foreach (var frame in frames)
                    sprite.frames.Add(frame.Clone());

                return sprite;
            }
        }

        #endregion

        public CHR(int numPals, int numTiles)
        {
            this.fileName = "";
            this.id = -1;

            // Create dummy CHR
            this.chrFlags = 0;
            this.palette = new Color[numPals][];
            for (int i = 0; i < numPals; i++)
            {
                this.palette[i] = new Color[16];
                for (int j = 0; j < 16; j++) this.palette[i][j] = Color.Black;
            }
            this.tiles = new byte[numTiles][];
            for (int i = 0; i < numTiles; i++) this.tiles[i] = new byte[32];

            chrData = new byte[0];
            chrDataPos = 0;

            // Decode character data
            DecodeCharacterData();
        }

        public CHR(BinaryReader br)
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
            if (readAll) {
                // TODO figure out length
                chrDataPos = br.BaseStream.Position;
                chrData = br.ReadBytes((int)(br.BaseStream.Length - chrDataPos));
            }
            else
			{
                chrDataPos = 0;
                chrData = new byte[0];
            }

            // Decode character data
            DecodeCharacterData();
        }

        public void Write(BinaryWriter bw)
        {
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
        }
        
        public void DecodeCharacterData()
        {
            if ((chrFlags & 2) != 2) return;
            if (chrData.Length == 0) return;

            Stream stream = new MemoryStream(chrData);
            BinaryReader br = new BinaryReader(stream);

            sprites = new List<Sprite>();

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
                sprite.frames = new List<Frame>();

                while (true)
                {
                    Frame frame = new Frame();
                    int numLayers = br.ReadByte();
                    if (numLayers == 0) break;

                    if (numLayers == 16)
                    {
                        frame.ticks = br.ReadUInt16();
                        numLayers = br.ReadByte();

                        frame.layers = new List<Layer>();
                        for (int j = 0; j < numLayers; j++)
                        {
                            Layer layer = new Layer();
                            ushort halfwd0 = br.ReadUInt16();
                            ushort halfwd1 = br.ReadUInt16();
                            ushort halfwd2 = br.ReadUInt16();

                            layer.x = (int)(halfwd1 & 0xFFF) << 20 >> 20;
                            layer.y = (int)(halfwd0 & 0x3FF) << 22 >> 22;
                            layer.tile = halfwd2;
                            layer.objsize = ((halfwd0 & 0xC000) >> 12) | ((halfwd1 & 0xC000) >> 14);
                            layer.vflip = (halfwd1 & 0x2000) != 0;
                            layer.hflip = (halfwd1 & 0x1000) != 0;
                            layer.translucent = (halfwd0 & 0x0400) != 0;

                            //if ((halfwd0 & 0x3800) != 0) throw new FormatException("Unexpected flags");

                            frame.layers.Add(layer);
                        }
                    }
                    else
                    {
                        frame.ticks = br.ReadByte();

                        frame.layers = new List<Layer>();
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
                            layer.objsize = (byte3 & 0x0C) | ((byte3 & 0xC0) >> 6);
                            layer.vflip = (byte3 & 0x20) != 0;
                            layer.hflip = (byte3 & 0x10) != 0;
                            layer.translucent = (byte3 & 0x02) != 0;
                            if ((byte3 & 0x01) != 0) layer.tile += 256;

                            frame.layers.Add(layer);
                        }
                    }
                    long extrabegin = br.BaseStream.Position;
                    frame.extrasize = br.ReadByte();
                    long extraend = (extrabegin + frame.extrasize * 3 + 2) & ~1; // weird...
                    int exbytes = (int)(extraend - extrabegin - 1);

                    frame.extradata = br.ReadBytes(exbytes);

                    if (frame.extradata.Length != exbytes)
                        throw new FormatException("Extradata does not match");

                    sprite.frames.Add(frame);
                }
                byte zeroValue2 = br.ReadByte();
                //if (zeroValue2 != 0)
                //    MessageBox.Show(string.Format("{0}:Zero expected at end: {1:X02} @ {2:X08}", i+1, zeroValue2, chrDataPos + br.BaseStream.Position));

                long remain;
                if (i + 1 < numSprites)
                {
                    remain = animOffsetPos + offsets[i + 1] - br.BaseStream.Position;
                }
                else
                {
                    remain = br.BaseStream.Length - br.BaseStream.Position;
                }
                //if (remain != 0)
                //    MessageBox.Show(string.Format("{0}:Remain: {1} bytes @ {2:X08}", i+1, remain, chrDataPos + br.BaseStream.Position));

                sprites.Add(sprite);
            }
        }

        public void EncodeCharacterData()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            if ((chrFlags & 2) == 2)
            {
                ushort numSprites = (ushort)sprites.Count;

                // Write data header
                bw.Write(numSprites);
                bw.Write((ushort)0);

                // Write dummy sprites offset
                ushort[] offsets = new ushort[numSprites];
                for (int i = 0; i < numSprites; i++)
                    bw.Write((ushort)0);

                // Write sprites data
                for (int i = 0; i < numSprites; i++)
                {
                    Sprite sprite = sprites[i];
                    offsets[i] = (ushort)(bw.BaseStream.Position - 4);

                    List<Frame> frames = sprite.frames;
                    for (int f = 0; f < frames.Count; f++)
                    {
                        Frame frame = frames[f];
                        int numLayers = frame.layers.Count;

                        // Figure out if going to use extended format
                        bool extended = false;
                        if (numLayers == 16)
                            extended = true;
                        if (frame.ticks >= 256)
                            extended = true;
                        for (int j = 0; j < numLayers; j++)
                        {
                            Layer layer = frame.layers[j];
                            if (layer.x < -128 || layer.x > 127) extended = true;
                            if (layer.y < -128 || layer.y > 127) extended = true;
                            if (layer.tile >= 512) extended = true;
                        }

                        if (extended)
                        {
                            bw.Write((byte)16);
                            bw.Write((ushort)frame.ticks);
                            bw.Write((byte)numLayers);

                            for (int li = 0; li < numLayers; li++)
                            {
                                Layer layer = frame.layers[li];

                                ushort halfwd0 = (ushort)((layer.y & 0x3FF) | ((layer.objsize & 12) << 12) | (layer.translucent ? 0x0400 : 0));
                                ushort halfwd1 = (ushort)((layer.x & 0xFFF) | ((layer.objsize & 3) << 14) | (layer.hflip ? 0x1000 : 0) | (layer.vflip ? 0x2000 : 0));
                                ushort halfwd2 = layer.tile;

                                bw.Write(halfwd0);
                                bw.Write(halfwd1);
                                bw.Write(halfwd2);
                            }
                        }
                        else
                        {
                            bw.Write((byte)numLayers);
                            bw.Write((byte)frame.ticks);

                            for (int li = 0; li < numLayers; li++)
                            {
                                Layer layer = frame.layers[li];

                                sbyte byte0 = (sbyte)layer.x;
                                sbyte byte1 = (sbyte)layer.y;
                                byte byte2 = (byte)layer.tile;
                                byte byte3 = (byte)((layer.objsize & 12) | ((layer.objsize & 3) << 6) | (layer.hflip ? 0x10 : 0) | (layer.vflip ? 0x20 : 0) | (layer.translucent ? 0x02 : 0));
                                if (layer.tile >= 256) byte3 |= 0x01;

                                bw.Write(byte0);
                                bw.Write(byte1);
                                bw.Write(byte2);
                                bw.Write(byte3);
                            }
                        }

                        long extrabegin = bw.BaseStream.Position;
                        bw.Write(frame.extrasize);
                        long extraend = (extrabegin + frame.extrasize * 3 + 2) & ~1; // weird...
                        int exbytes = (int)(extraend - extrabegin - 1);

                        byte[] extradata = frame.extradata;
                        Array.Resize(ref extradata, exbytes);
                        bw.Write(extradata);
                    }

                    bw.Write((byte)0);
                    bw.Write((byte)0);
                }

                // Rewrite address table
                bw.BaseStream.Seek(4, SeekOrigin.Begin);
                for (int i = 0; i < numSprites; i++)
                    bw.Write(offsets[i]);
                bw.BaseStream.Seek(0, SeekOrigin.End);
            }

            chrData = ms.ToArray();
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
