﻿/*
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
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ekona.Images;
using Ekona.Images.Formats;

namespace DB_KAI_RPG
{
    static class Transform
    {
        public enum PalFormat : int
        {
            Cancel = 0,
            WinPal = 1,
            Gimp = 2,
            PNG = 3,
            ACO = 4
        };

        #region [ Palette Transforms ]

        /// <summary>
        /// Find best color from palette
        /// </summary>
        /// <param name="color">Color reference</param>
        /// <param name="palette">Palette to seach</param>
        /// <returns>Index in palette</returns>
        static public int FindBestColor(Color color, Color[] palette)
        {
            int bestIndex = 0;
            int bestDistance = int.MaxValue;
            for (int i = 0; i < palette.Length; i++)
            {
                int deltaR = color.R - palette[i].R;
                int deltaG = color.G - palette[i].G;
                int deltaB = color.B - palette[i].B;
                int distance = deltaR * deltaR + deltaG * deltaG + deltaB * deltaB;
                if (distance < bestDistance)
                {
                    bestIndex = i;
                    bestDistance = distance;
                }
            }
            return bestIndex;
        }

        /// <summary>
        /// Generate a 1D palette from 2D palette
        /// </summary>
        /// <param name="palette">2D palette</param>
        /// <param name="entriesPerSlot">Number of entries per slot</param>
        /// <returns>1D palette</returns>
        static public Color[] Get1DPalette(Color[][] palette, int entriesPerSlot)
        {
            // Create full palette
            Color[] outPal = new Color[entriesPerSlot * palette.Length];
            for (int y = 0; y < palette.Length; y++)
            {
                for (int x = 0; x < entriesPerSlot; x++)
                {
                    int index = y * entriesPerSlot + x;
                    if (x < palette[y].Length)
                        outPal[index] = palette[y][x];
                    else
                        outPal[index] = Color.Black;
                }
            }
            return outPal;
        }

        /// <summary>
        /// Generate a 2D palette from 1D palette
        /// </summary>
        /// <param name="palette">1D palette</param>
        /// <param name="entriesPerSlot">Number of entries per slot</param>
        /// <returns>2D palette</returns>
        static public Color[][] Get2DPalette(Color[] palette, int entriesPerSlot)
        {
            int slots = (palette.Length + (entriesPerSlot - 1)) / entriesPerSlot;

            // Create full palette
            Color[][] outPal = new Color[slots][];
            for (int y = 0; y < slots; y++)
            {
                outPal[y] = new Color[entriesPerSlot];
                for (int x = 0; x < entriesPerSlot; x++)
                {
                    int index = y * entriesPerSlot + x;
                    if (index < palette.Length)
                        outPal[y][x] = palette[index];
                    else
                        outPal[y][x] = Color.Black;
                }
            }
            return outPal;
        }

        /// <summary>
        /// Resize 1D palette to a specific number of colors
        /// </summary>
        /// <param name="palette">Palette</param>
        /// <param name="numColors">Number of colors</param>
        /// <returns>New palette</returns>
        static public Color[] Resize1DPalette(Color[] palette, int numColors)
        {
            if (palette == null)
            {
                Color[] blackPal = new Color[numColors];
                for (int i = 0; i < numColors; i++) blackPal[i] = Color.Black;
                return blackPal;
            }

            if (palette.Length == numColors) return palette;

            Color[] pal = new Color[numColors];
            for (int i = 0; i < numColors; i++)
            {
                if (i < palette.Length)
                    pal[i] = palette[i];
                else
                    pal[i] = Color.Black;
            }
            return pal;
        }

        #endregion

        #region [ Palette IO Formats ]

        /// <summary>
        /// Export palette in 32-bits PNG format
        /// </summary>
        /// <param name="fileName">Output filename</param>
        /// <param name="palette">Palette to export</param>
        static public void ExportPalettePNG32(string fileName, Color[] palette)
        {
            int palWidth = palette.Length % 16;
            int palHeight = (palette.Length + 15) / 16;
            if (palWidth == 0)
                palWidth = 16;

            Bitmap bmp = new Bitmap(palWidth * 8, palHeight * 8);

            // Write data
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    int index = (y >> 3) * 16 + (x >> 3);
                    bmp.SetPixel(x, y, palette[index]);
                }
            }

            // Save PNG
            bmp.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
            bmp.Dispose();
        }

        /// <summary>
        /// Export palette in index 8-bits PNG format
        /// </summary>
        /// <param name="fileName">Output filename</param>
        /// <param name="palette">Palette to export</param>
        static public void ExportPalettePNG8(string fileName, Color[] palette)
        {
            int palWidth = palette.Length % 16;
            int palHeight = (palette.Length + 15) / 16;
            if (palWidth == 0)
                palWidth = 16;

            Bitmap bmp = new Bitmap(palWidth * 8, palHeight * 8, PixelFormat.Format8bppIndexed);

            // Write data
            BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            IntPtr ptr = bmd.Scan0;
            int stride = bmd.Stride;
            for (int y = 0; y < bmp.Height; y++)
            {
                int offset = y * stride;
                for (int x = 0; x < bmp.Width; x++)
                {
                    int index = (y >> 3) * 16 + (x >> 3);
                    Marshal.WriteByte(ptr, offset++, (byte)index);
                }
            }
            bmp.UnlockBits(bmd);

            // Setup palette
            ColorPalette pal = bmp.Palette;
            for (int i = 0; i < palette.Length; i++)
                pal.Entries[i] = palette[i];
            bmp.Palette = pal;

            // Save PNG
            bmp.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
            bmp.Dispose();
        }

        /// <summary>
        /// Import palette from PNG, must have 16 entries per row
        /// </summary>
        /// <param name="fileName">PNG filename</param>
        /// <returns>Palette or null on error</returns>
        static public Color[] ImportPalettePNG(string fileName)
        {
            Bitmap bmp = (Bitmap)Image.FromFile(fileName);

            int entrySize = bmp.Width / 16;
            int slots = bmp.Height / Math.Max(entrySize, 1);
            if (entrySize < 1 || slots < 1 || entrySize * 16 != bmp.Width)
                return null;

            Color[] newpal = new Color[slots * 16];
            int srcY = entrySize / 2; // Sample center
            for (int y = 0; y < slots; y++)
            {
                int srcX = entrySize / 2; // Sample center
                for (int x = 0; x < 16; x++)
                {
                    int index = y * 16 + x;
                    newpal[index] = Color.FromArgb((int)(bmp.GetPixel(srcX, srcY).ToArgb() | 0xFF000000));
                    srcX += entrySize;
                }
                srcY += entrySize;
            }

            bmp.Dispose();
            return newpal;
        }

        /// <summary>
        /// Export palette in GIMP format
        /// </summary>
        /// <param name="fileName">Output filename</param>
        /// <param name="palette">Palette to export</param>
        static public void ExportPaletteGIMP(string fileName, Color[] palette)
        {
            StreamWriter sw = new StreamWriter(fileName);
            sw.WriteLine("GIMP Palette");
            sw.WriteLine("#");
            for (int i = 0; i < palette.Length; i++)
            {
                string name;
                if (i == 0)
                    name = "Transparency";
                else
                    name = string.Format("Index{0}", i);
                sw.WriteLine(string.Format("{0,3} {1,3} {2,3}   {3}", palette[i].R, palette[i].G, palette[i].B, name));
            }
            sw.Close();
        }

        /// <summary>
        /// Import palette in GIMP format
        /// </summary>
        /// <param name="fileName">GIMP file</param>
        /// <returns>Palette or null on error</returns>
        static public Color[] ImportPaletteGIMP(string fileName)
        {
            List<Color> colors = new List<Color>();

            // Use regex to decode
            Regex rgbregex = new Regex("^[\t ]*([0-9]+)[\t ]+([0-9]+)[\t ]+([0-9]+)");
            foreach (string line in File.ReadLines(fileName))
            {
                var match = rgbregex.Match(line);
                if (match.Success)
				{
                    int r, g, b;
                    if (int.TryParse(match.Groups[1].Value, out r) &&
                        int.TryParse(match.Groups[2].Value, out g) &&
                        int.TryParse(match.Groups[3].Value, out b))
                        colors.Add(Color.FromArgb(r, g, b));
                }
            }

            return colors.ToArray();
        }

        #endregion

        #region [ Palette Rendering ]

        /// <summary>
        /// Get Bitmap from 1D Palette
        /// </summary>
        /// <param name="width">Width of Bitmap</param>
        /// <param name="height">Height of Bitmap</param>
        /// <param name="palette">Palette</param>
        /// <param name="horzColors">Horizontal colors</param>
        /// <param name="vertColors">Vertical colors</param>
        /// <returns>Bitmap</returns>
        static public Bitmap Get1DPaletteBitmap(int width, int height, Color[] palette, int horzColors, int vertColors = 1)
        {
            if (palette.Length == 0)
                return new Bitmap(width, height);

            Bitmap bmp = new Bitmap(width, height);
            float deltaX = (float)horzColors / (float)width;
            float deltaY = (float)vertColors / (float)height;
            float indY = 0f;
            for (int y = 0; y < height; y++)
            {
                float indX = 0f;
                for (int x = 0; x < width; x++)
                {
                    int index = (int)indY * horzColors + (int)indX;
                    if ((int)index < palette.Length)
                        bmp.SetPixel(x, y, palette[(int)index]);
                    indX += deltaX;
                }
                indY += deltaY;
            }

            return bmp;
        }

        #endregion

        #region [ Tiles Import ]

        static private byte[][] Import4bppTilesetFrom4bpp(Bitmap bmp, int maxTiles)
        {
            // Write data
            BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format4bppIndexed);
            IntPtr ptr = bmd.Scan0;
            int stride = bmd.Stride;

            int columns = bmp.Width / 8;
            int rows = bmp.Height / 8;
            int numTiles = columns * rows;

            if (maxTiles >= 0 && numTiles > maxTiles)
                numTiles = maxTiles;

            // Read tiles
            byte[][] tiles = new byte[numTiles][];
            for (int ty = 0; ty < rows; ty++)
            {
                for (int tx = 0; tx < columns; tx++)
                {
                    int id = ty * columns + tx;
                    if (id >= tiles.Length) break;

                    byte[] tile = new byte[32];
                    for (int y = 0; y < 8; y++)
                    {
                        int offsetX = tx * 4;
                        int offsetY = ty * 8 + y;
                        for (int x = 0; x < 4; x++)
                        {
                            byte value = Marshal.ReadByte(ptr, offsetY * stride + offsetX++);
                            tile[y * 4 + x] = (byte)((value << 4) | (value >> 4));
                        }
                    }
                    tiles[id] = tile;
                }
            }
            bmp.UnlockBits(bmd);

            return tiles;
        }

        static private byte[][] Import4bppTilesetFrom8bpp(Bitmap bmp, int maxTiles)
        {
            // Write data
            BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            IntPtr ptr = bmd.Scan0;
            int stride = bmd.Stride;

            int columns = bmp.Width / 8;
            int rows = bmp.Height / 8;
            int numTiles = columns * rows;

            if (maxTiles >= 0 && numTiles > maxTiles)
                numTiles = maxTiles;

            // Read tiles
            byte[][] tiles = new byte[numTiles][];
            for (int ty = 0; ty < rows; ty++)
            {
                for (int tx = 0; tx < columns; tx++)
                {
                    int id = ty * columns + tx;
                    if (id >= tiles.Length) break;

                    byte[] tile = new byte[32];
                    for (int y = 0; y < 8; y++)
                    {
                        int offsetX = tx * 8;
                        int offsetY = ty * 8 + y;
                        for (int x = 0; x < 4; x++)
                        {
                            byte valueL = Marshal.ReadByte(ptr, offsetY * stride + offsetX++);
                            byte valueH = Marshal.ReadByte(ptr, offsetY * stride + offsetX++);
                            tile[y * 4 + x] = (byte)(((valueH & 15) << 4) | (valueL & 15));
                        }
                    }
                    tiles[id] = tile;
                }
            }
            bmp.UnlockBits(bmd);

            return tiles;
        }

        static private byte[][] Import4bppTilesetFrom32bpp(Bitmap bmp, Color[] palette, int maxTiles)
        {
            int columns = bmp.Width / 8;
            int rows = bmp.Height / 8;
            int numTiles = columns * rows;

            if (maxTiles >= 0 && numTiles > maxTiles)
                numTiles = maxTiles;

            // Read tiles
            byte[][] tiles = new byte[numTiles][];
            for (int ty = 0; ty < rows; ty++)
            {
                for (int tx = 0; tx < columns; tx++)
                {
                    int id = ty * columns + tx;
                    if (id >= tiles.Length) break;

                    byte[] tile = new byte[32];
                    for (int y = 0; y < 8; y++)
                    {
                        int offsetX = tx * 8;
                        int offsetY = ty * 8 + y;
                        for (int x = 0; x < 4; x++)
                        {
                            int valueL = FindBestColor(bmp.GetPixel(offsetX++, offsetY), palette);
                            int valueH = FindBestColor(bmp.GetPixel(offsetX++, offsetY), palette);
                            tile[y * 4 + x] = (byte)((valueH << 4) | (valueL & 15));
                        }
                    }
                    tiles[id] = tile;
                }
            }

            return tiles;
        }

        #endregion

        #region [ Textures Import ]

        static private byte[] Import4bppTextureFrom4bpp(Bitmap bmp)
        {
            // Lock bitmap
            BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format4bppIndexed);
            IntPtr ptr = bmd.Scan0;
            int stride = bmd.Stride;

            // Convert texture
            byte[] texture = new byte[bmp.Width * bmp.Height / 2];
            int offTex = 0;
            for (int y = 0; y < bmp.Height; y++)
            {
                int offBmp = y * stride;
                for (int x = 0; x < bmp.Width; x += 2)
                {
                    if (offTex >= texture.Length)
                        break;
                    byte value = Marshal.ReadByte(ptr, offBmp++);
                    value = (byte)((value >> 4) | (value << 4)); // Swap nibbles
                    texture[offTex++] = value;
                }
            }
            bmp.UnlockBits(bmd);

            return texture;
        }

        static private byte[] Import4bppTextureFrom8bpp(Bitmap bmp)
        {
            // Lock bitmap
            BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            IntPtr ptr = bmd.Scan0;
            int stride = bmd.Stride;

            // Convert texture
            byte[] texture = new byte[bmp.Width * bmp.Height / 2];
            int offTex = 0;
            for (int y = 0; y < bmp.Height; y++)
            {
                int offBmp = y * stride;
                for (int x = 0; x < bmp.Width; x += 2)
                {
                    if (offTex >= texture.Length)
                        break;
                    byte valueL = Marshal.ReadByte(ptr, offBmp++);
                    byte valueH = Marshal.ReadByte(ptr, offBmp++);
                    texture[offTex++] = (byte)(((valueH & 15) << 4) | (valueL & 15));
                }
            }
            bmp.UnlockBits(bmd);

            return texture;
        }

        static private byte[] Import4bppTextureFrom32bpp(Bitmap bmp, Color[] palette)
        {
            // Convert texture
            byte[] texture = new byte[bmp.Width * bmp.Height / 2];
            int offTex = 0;
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x += 2)
                {
                    if (offTex >= texture.Length)
                        break;

                    int valueL = FindBestColor(bmp.GetPixel(x, y), palette);
                    int valueH = FindBestColor(bmp.GetPixel(x, y), palette);
                    texture[offTex++] = (byte)(((valueH & 15) << 4) | (valueL & 15));
                }
            }

            return texture;
        }

        static private byte[] Import8bppTextureFrom4bpp(Bitmap bmp)
        {
            // Lock bitmap
            BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format4bppIndexed);
            IntPtr ptr = bmd.Scan0;
            int stride = bmd.Stride;

            // Convert texture
            byte[] texture = new byte[bmp.Width * bmp.Height];
            int offTex = 0;
            for (int y = 0; y < bmp.Height; y++)
            {
                int offBmp = y * stride;
                for (int x = 0; x < bmp.Width; x += 2)
                {
                    if (offTex >= texture.Length)
                        break;
                    byte value = Marshal.ReadByte(ptr, offBmp++);
                    texture[offTex++] = (byte)(value & 15);
                    texture[offTex++] = (byte)(value >> 4);
                }
            }
            bmp.UnlockBits(bmd);

            return texture;
        }

        static private byte[] Import8bppTextureFrom8bpp(Bitmap bmp)
        {
            // Lock bitmap
            BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            IntPtr ptr = bmd.Scan0;
            int stride = bmd.Stride;

            // Convert texture
            byte[] texture = new byte[bmp.Width * bmp.Height];
            int offTex = 0;
            for (int y = 0; y < bmp.Height; y++)
            {
                int offBmp = y * stride;
                for (int x = 0; x < bmp.Width; x++)
                {
                    if (offTex >= texture.Length)
                        break;
                    byte value = Marshal.ReadByte(ptr, offBmp++);
                    texture[offTex++] = value;
                }
            }
            bmp.UnlockBits(bmd);

            return texture;
        }

        static private byte[] Import8bppTextureFrom32bpp(Bitmap bmp, Color[] palette)
        {
            // Convert texture
            byte[] texture = new byte[bmp.Width * bmp.Height];
            int offTex = 0;
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    if (offTex >= texture.Length)
                        break;

                    int value = FindBestColor(bmp.GetPixel(x, y), palette);
                    texture[offTex++] = (byte)value;
                }
            }

            return texture;
        }

        #endregion

        #region [ Tiles Rendering ]

        /// <summary>
        /// Get Bitmap from 4bpp Tiles
        /// </summary>
        /// <param name="tiles">Tiles data</param>
        /// <param name="palette">4bpp Palette</param>
        /// <param name="columns">Tiles per row</param>
        /// <returns>Format4bppIndexed Bitmap</returns>
        static public Bitmap Get4bppTilesBitmap(byte[][] tiles, Color[] palette, int columns = 32)
        {
            if (palette.Length == 0 || tiles.Length == 0 || columns <= 0 || palette.Length < 16)
                return new Bitmap(8, 8);

            // Create bitmap and lock bits
            int tilesHeight = (tiles.Length + (columns - 1)) / columns;
            Bitmap bmp = new Bitmap(columns * 8, tilesHeight * 8, PixelFormat.Format4bppIndexed);
            BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format4bppIndexed);

            // Render all tiles
            IntPtr ptr = bmd.Scan0;
            int stride = bmd.Stride;
            for (int ty = 0; ty < tilesHeight; ty++)
            {
                for (int tx = 0; tx < columns; tx++)
                {
                    int id = ty * columns + tx;
                    if (id >= tiles.Length) continue;
                    byte[] tile = tiles[id];

                    for (int y = 0; y < 8; y++)
                    {
                        int offset = (ty * 8 + y) * stride + (tx * 4);
                        for (int x = 0; x < 4; x++)
                        {
                            byte value = tile[y * 4 + x];
                            value = (byte)((value >> 4) | (value << 4)); // Swap nibbles
                            Marshal.WriteByte(ptr, offset++, (byte)value);
                        }
                    }
                }
            }
            bmp.UnlockBits(bmd);

            // Setup palette
            ColorPalette pal = bmp.Palette;
            for (int i = 0; i < palette.Length; i++)
                pal.Entries[i] = palette[i];
            bmp.Palette = pal;

            return bmp;
        }

        static public Bitmap Get4bppTilesBitmapSlow(byte[][] tiles, Color[] palette, int columns) // To deprecate
        {
            if (palette.Length == 0 || tiles.Length == 0 || columns <= 0 || palette.Length < 16)
                return new Bitmap(1, 1);

            int tilesHeight = (tiles.Length + (columns - 1)) / columns;
            Bitmap bmp = new Bitmap(columns * 8, tilesHeight * 8);

            for (int ty = 0; ty < tilesHeight; ty++)
            {
                for (int tx = 0; tx < columns; tx++)
                {
                    int id = ty * columns + tx;
                    if (id >= tiles.Length) continue;
                    byte[] tile = tiles[id];

                    for (int y = 0; y < 8; y++)
                    {
                        int offsetX = tx * 8;
                        int offsetY = ty * 8 + y;
                        for (int x = 0; x < 4; x++)
                        {
                            byte value = tile[y * 4 + x];
                            bmp.SetPixel(offsetX++, offsetY, palette[value & 15]);
                            bmp.SetPixel(offsetX++, offsetY, palette[value >> 4]);
                        }
                    }
                }
            }

            return bmp;
        }

        #endregion

        #region [ Texture Rendering ]

        /// <summary>
        /// Get Bitmap from 4bpp Texture
        /// </summary>
        /// <param name="tiles">Texture data</param>
        /// <param name="palette">4bpp Palette</param>
        /// <param name="columns">Tiles per row</param>
        /// <returns>Format4bppIndexed Bitmap</returns>
        static public Bitmap Get4bppTextureBitmap(byte[] texture, int texWidth, int texHeight, Color[] palette)
        {
            if (palette.Length == 0 || texture.Length == 0 || texWidth == 0 || texHeight == 0 || palette.Length < 16)
                return new Bitmap(8, 8);

            // Create bitmap and lock bits
            Bitmap bmp = new Bitmap(texWidth, texHeight, PixelFormat.Format4bppIndexed);
            BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format4bppIndexed);

            // Render all tiles
            IntPtr ptr = bmd.Scan0;
            int stride = bmd.Stride;
            int offTex = 0;
            for (int y = 0; y < texHeight; y++)
            {
                int offBmp = y * stride;
                for (int x = 0; x < texWidth; x += 2)
                {
                    if (offTex >= texture.Length)
                        break;
                    byte value = texture[offTex++];
                    value = (byte)((value >> 4) | (value << 4)); // Swap nibbles
                    Marshal.WriteByte(ptr, offBmp++, (byte)value);
                }
            }
            bmp.UnlockBits(bmd);

            // Setup palette
            ColorPalette pal = bmp.Palette;
            for (int i = 0; i < palette.Length; i++)
                pal.Entries[i] = palette[i];
            bmp.Palette = pal;

            return bmp;
        }

        /// <summary>
        /// Get Bitmap from 8bpp Texture
        /// </summary>
        /// <param name="tiles">Texture data</param>
        /// <param name="palette">8bpp Palette</param>
        /// <param name="columns">Tiles per row</param>
        /// <returns>Format8bppIndexed Bitmap</returns>
        static public Bitmap Get8bppTextureBitmap(byte[] texture, int texWidth, int texHeight, Color[] palette)
        {
            if (palette.Length == 0 || texture.Length == 0 || texWidth == 0 || texHeight == 0 || palette.Length < 16)
                return new Bitmap(8, 8);

            // Create bitmap and lock bits
            Bitmap bmp = new Bitmap(texWidth, texHeight, PixelFormat.Format8bppIndexed);
            BitmapData bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            // Render all tiles
            IntPtr ptr = bmd.Scan0;
            int stride = bmd.Stride;
            int offTex = 0;
            for (int y = 0; y < texHeight; y++)
            {
                int offBmp = y * stride;
                for (int x = 0; x < texWidth; x++)
                {
                    if (offTex >= texture.Length)
                        break;
                    byte value = texture[offTex++];
                    Marshal.WriteByte(ptr, offBmp++, (byte)value);
                }
            }
            bmp.UnlockBits(bmd);

            // Setup palette
            ColorPalette pal = bmp.Palette;
            for (int i = 0; i < palette.Length; i++)
                pal.Entries[i] = palette[i];
            bmp.Palette = pal;

            return bmp;
        }

        #endregion

        #region [ Export / Import ]

        /// <summary>
        /// Export palette dialog
        /// </summary>
        /// <param name="fileName">Expected filename</param>
        /// <param name="format">[Out] Palette format</param>
        /// <returns>Filename choosen or string.Empty if canceled</returns>
        static public string ExportPaletteDialog(string fileName, out PalFormat format)
        {
            format = PalFormat.Cancel;

            SaveFileDialog o = new SaveFileDialog();
            o.AddExtension = true;
            o.CheckPathExists = true;
            o.DefaultExt = ".pal";
            o.Filter = "Windows Palette (*.pal)|*.pal|" +
                       "Gimp Palette (*.gpl)|*.gpl|" +
                       "Portable Network Graphics (*.png)|*.png|" +
                       "Adobe COlor (*.aco)|*.aco";
            o.OverwritePrompt = true;
            o.FileName = Path.ChangeExtension(fileName, null);

            if (o.ShowDialog() != DialogResult.OK)
                return string.Empty;

            format = PalFormat.PNG;
            if (o.FilterIndex == 1)
                format = PalFormat.WinPal;
            if (o.FilterIndex == 2)
                format = PalFormat.Gimp;
            if (o.FilterIndex == 3)
                format = PalFormat.PNG;
            if (o.FilterIndex == 4)
                format = PalFormat.ACO;

            return o.FileName;
        }

        /// <summary>
        /// Import palette dialog
        /// </summary>
        /// <param name="fileName">Expected filename</param>
        /// <param name="format">[Out] Palette format</param>
        /// <returns>Filename choosen or string.Empty if canceled</returns>
        static public string ImportPaletteDialog(string fileName, out PalFormat format)
        {
            format = PalFormat.Cancel;

            OpenFileDialog o = new OpenFileDialog();
            o.CheckFileExists = true;
            o.Filter = "All supported formats|*.pal;*.gpl;*.aco;*.png;*.bmp;*.jpg;*.jpeg;*.tif;*.tiff;*.gif;*.ico;*.icon|" +
                "Windows Palette (*.pal)|*.pal|" +
                "Gimp Palette (*.gpl)|*.gpl|" +
                "Adobe COlor (*.aco)|*.aco|" +
                "Palette from image|*.png;*.bmp;*.jpg;*.jpeg;*.tif;*.tiff;*.gif;*.ico;*.icon";
            if (o.ShowDialog() != DialogResult.OK)
                return string.Empty;

            string ext = Path.GetExtension(o.FileName).ToLower();
            if (string.IsNullOrEmpty(ext) || ext.Length == 0)
            {
                MessageBox.Show("File without extension... Aborting");
                return string.Empty;
            }

            if (ext.Contains("."))
                ext = ext.Substring(ext.LastIndexOf(".") + 1);

            format = PalFormat.PNG;
            if (ext == "pal")
                format = PalFormat.WinPal;
            else if (ext == "gpl")
                format = PalFormat.Gimp;
            else if (ext == "aco")
                format = PalFormat.ACO;
            else if (ext == "png")
                format = PalFormat.PNG;

            return o.FileName;
        }

        /// <summary>
        /// Export palette to a file
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="format">Format of the file</param>
        /// <param name="palette">Palette to export</param>
        static public void ExportPalette(string fileName, PalFormat format, Color[] palette)
        {
            if (palette.Length <= 0)
                return;

            if (format == PalFormat.WinPal)
            {
                PaletteWin palwin = new PaletteWin(palette);
                palwin.Gimp_Error = false;
                palwin.Write(fileName);
            }
            else if (format == PalFormat.Gimp)
			{
                ExportPaletteGIMP(fileName, palette);
			}
            else if (format == PalFormat.ACO)
            {
                ACO palaco = new ACO(palette);
                palaco.Write(fileName);
            }
            else if (format == PalFormat.PNG)
            {
                if (palette.Length > 256)
                    ExportPalettePNG32(fileName, palette);
                else
                    ExportPalettePNG8(fileName, palette);
            }
        }

        /// <summary>
        /// Import palette from a file
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="format">Format of the file</param>
        /// <returns>Imported palette or null on error</returns>
        static public Color[] ImportPalette(string fileName, PalFormat format)
        {
            if (format == PalFormat.WinPal)
            {
                PaletteBase newpal = new PaletteWin(fileName);
                // Seems to always imports a 1D palette at first slot
                if (newpal == null || newpal.Palette == null) return null;
                return newpal.Palette[0];
            }
            else if (format == PalFormat.Gimp)
			{
                return ImportPaletteGIMP(fileName);
            }
            else if (format == PalFormat.ACO)
            {
                PaletteBase newpal = new ACO(fileName);
                // Seems to be broken?
                if (newpal == null || newpal.Palette == null) return null;
                return newpal.Palette[0];
            }
            else if (format == PalFormat.PNG)
            {
                return ImportPalettePNG(fileName);
            }
            return null;
        }

        /// <summary>
        /// Export Tileset to a PNG file
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="tiles">Tiles to export</param>
        /// <param name="palette">Palette</param>
        /// <param name="columns">Tiles per row</param>
        static public void ExportTilesetPNG(string fileName, byte[][] tiles, Color[] palette, int columns)
        {
            if (palette.Length <= 0 || tiles.Length <= 0)
                return;

            // Save Tileset
            Bitmap bmp = Transform.Get4bppTilesBitmap(tiles, palette, columns);
            bmp.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
            bmp.Dispose();
        }

        /// <summary>
        /// Import Tileset from a file
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="palette">Palette (Hint)</param>
        /// <param name="numHoles">[Out] Number of holes due to bad pixels</param>
        /// <returns>Tileset</returns>
        static public byte[][] Import4bppTilesetFromImage(string fileName, Color[] palette, int maxTiles)
        {
            Bitmap bmp = (Bitmap)Image.FromFile(fileName, false);
            if (((bmp.Width | bmp.Height) & 7) != 0)
                return null;

            // Replace hint palette with actual palette
            byte[][] img = null;
            if (bmp.PixelFormat == PixelFormat.Format4bppIndexed)
                img = Import4bppTilesetFrom4bpp(bmp, maxTiles);
            else if (bmp.PixelFormat == PixelFormat.Format8bppIndexed)
                img = Import4bppTilesetFrom8bpp(bmp, maxTiles);
            else
                img = Import4bppTilesetFrom32bpp(bmp, palette, maxTiles);

            bmp.Dispose();
            return img;
        }

        /// <summary>
        /// Export 4bpp Texture to a PNG file
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="texRaw">Texture raw data</param>
        /// <param name="texWidth">Texture width</param>
        /// <param name="texHeight">Texture height</param>
        /// <param name="palette">Palette</param>
        static public void Export4bppTexturePNG(string fileName, byte[] texRaw, int texWidth, int texHeight, Color[] palette)
        {
            if (palette.Length <= 0 || texRaw.Length <= 0 || texWidth <= 0 || texHeight <= 0)
                return;

            // Save Tileset
            Bitmap bmp = Transform.Get4bppTextureBitmap(texRaw, texWidth, texHeight, palette);
            bmp.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
            bmp.Dispose();
        }

        /// <summary>
        /// Export 8bpp Texture to a file
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="texRaw">Texture raw data</param>
        /// <param name="texWidth">Texture width</param>
        /// <param name="texHeight">Texture height</param>
        /// <param name="palette">Palette</param>
        static public void Export8bppTexturePNG(string fileName, byte[] texRaw, int texWidth, int texHeight, Color[] palette)
        {
            if (palette.Length <= 0 || texRaw.Length <= 0 || texWidth <= 0 || texHeight <= 0)
                return;

            // Save Tileset
            Bitmap bmp = Transform.Get8bppTextureBitmap(texRaw, texWidth, texHeight, palette);
            bmp.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
            bmp.Dispose();
        }

        /// <summary>
        /// Import 4bpp Texture from a file
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="texWidth">[Out] Texture width</param>
        /// <param name="texHeight">[Out] Texture height</param>
        /// <param name="palette">Palette (Hint)</param>
        /// <returns></returns>
        static public byte[] Import4bppTextureFromImage(string fileName, out int texWidth, out int texHeight, Color[] palette)
        {
            texWidth = 0;
            texHeight = 0;
            Bitmap bmp = (Bitmap)Image.FromFile(fileName, false);
            if ((bmp.Width & 1) != 0) // Multiple of 2
                return null;
            texWidth = bmp.Width;
            texHeight = bmp.Height;

            // Replace hint palette with actual palette
            byte[] img = null;
            if (bmp.PixelFormat == PixelFormat.Format4bppIndexed)
                img = Import4bppTextureFrom4bpp(bmp);
            else if (bmp.PixelFormat == PixelFormat.Format8bppIndexed)
                img = Import4bppTextureFrom8bpp(bmp);
            else
                img = Import4bppTextureFrom32bpp(bmp, palette);

            bmp.Dispose();
            return img;
        }

        /// <summary>
        /// Import 8bpp Texture from a file
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="texWidth">[Out] Texture width</param>
        /// <param name="texHeight">[Out] Texture height</param>
        /// <param name="palette">Palette (Hint)</param>
        /// <returns>Tileset</returns>
        static public byte[] Import8bppTextureFromImage(string fileName, out int texWidth, out int texHeight, Color[] palette)
        {
            texWidth = 0;
            texHeight = 0;
            Bitmap bmp = (Bitmap)Image.FromFile(fileName, false);
            if ((bmp.Width & 1) != 0) // Multiple of 2
                return null;
            texWidth = bmp.Width;
            texHeight = bmp.Height;

            // Replace hint palette with actual palette
            byte[] img = null;
            if (bmp.PixelFormat == PixelFormat.Format4bppIndexed)
                img = Import8bppTextureFrom4bpp(bmp);
            else if (bmp.PixelFormat == PixelFormat.Format8bppIndexed)
                img = Import8bppTextureFrom8bpp(bmp);
            else
                img = Import8bppTextureFrom32bpp(bmp, palette);

            bmp.Dispose();
            return img;
        }

        #endregion
    }
}