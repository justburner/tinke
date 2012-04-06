﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using PluginInterface;
using PluginInterface.Images;

namespace Images
{
    public class NCGR : ImageBase
    {
        sNCGR ncgr;

        public NCGR(IPluginHost pluginHost, string file, int id) : base(pluginHost, file, id) { }

        public override void Read(string fileIn)
        {
            BinaryReader br = new BinaryReader(File.OpenRead(fileIn));
            ncgr = new sNCGR();

            // Read the common header
            ncgr.header.id = br.ReadChars(4);
            ncgr.header.endianess = br.ReadUInt16();
            if (ncgr.header.endianess == 0xFFFE)
                ncgr.header.id.Reverse<char>();
            ncgr.header.constant = br.ReadUInt16();
            ncgr.header.file_size = br.ReadUInt32();
            ncgr.header.header_size = br.ReadUInt16();
            ncgr.header.nSection = br.ReadUInt16();

            // Read the first section: CHAR (CHARacter data)
            ncgr.rahc.id = br.ReadChars(4);
            ncgr.rahc.size_section = br.ReadUInt32();
            ncgr.rahc.nTilesY = br.ReadUInt16();
            ncgr.rahc.nTilesX = br.ReadUInt16();
            ncgr.rahc.depth = (ColorFormat)br.ReadUInt32();
            ncgr.rahc.unknown1 = br.ReadUInt16();
            ncgr.rahc.unknown2 = br.ReadUInt16();
            ncgr.rahc.tiledFlag = br.ReadUInt32();
            if ((ncgr.rahc.tiledFlag & 0xFF) == 0x0)
                ncgr.order = TileForm.Horizontal;
            else
                ncgr.order = TileForm.Lineal;

            ncgr.rahc.size_tiledata = br.ReadUInt32();
            ncgr.rahc.unknown3 = br.ReadUInt32();
            ncgr.rahc.data = br.ReadBytes((int)ncgr.rahc.size_tiledata);

            if (ncgr.rahc.nTilesX != 0xFFFF)
            {
                ncgr.rahc.nTilesX *= 8;
                ncgr.rahc.nTilesY *= 8;
            }
            else
            {
                ncgr.rahc.nTilesX = 0x40;
                ncgr.rahc.nTilesY = (ushort)(ncgr.rahc.size_tiledata / 0x40);
            }


            if (ncgr.header.nSection == 2 && br.BaseStream.Position < br.BaseStream.Length)   // If there isn't SOPC section
            {
                // Read the second section: SOPC
                ncgr.sopc.id = br.ReadChars(4);
                ncgr.sopc.size_section = br.ReadUInt32();
                ncgr.sopc.unknown1 = br.ReadUInt32();
                ncgr.sopc.charSize = br.ReadUInt16();
                ncgr.sopc.nChar = br.ReadUInt16();
            }

            br.Close();
            Set_Tiles(ncgr.rahc.data, ncgr.rahc.nTilesX, ncgr.rahc.nTilesY, ncgr.rahc.depth, ncgr.order, true);
        }
        public override void Write(string fileOut, PaletteBase palette)
        {
            Update_Struct();
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileOut));

            // Common header
            bw.Write(ncgr.header.id);
            bw.Write(ncgr.header.endianess);
            bw.Write(ncgr.header.constant);
            bw.Write(ncgr.header.file_size);
            bw.Write(ncgr.header.header_size);
            bw.Write(ncgr.header.nSection);

            // RAHC section
            bw.Write(ncgr.rahc.id);
            bw.Write(ncgr.rahc.size_section);
            bw.Write(ncgr.rahc.nTilesY);
            bw.Write(ncgr.rahc.nTilesX);
            bw.Write((uint)(ncgr.rahc.depth));
            bw.Write(ncgr.rahc.unknown1);
            bw.Write(ncgr.rahc.unknown2);
            bw.Write(ncgr.rahc.tiledFlag);
            bw.Write(ncgr.rahc.size_tiledata);
            bw.Write(ncgr.rahc.unknown3);
            bw.Write(ncgr.rahc.data);

            // SOPC section
            if (ncgr.header.nSection == 2)
            {
                bw.Write(ncgr.sopc.id);
                bw.Write(ncgr.sopc.size_section);
                bw.Write(ncgr.sopc.unknown1);
                bw.Write(ncgr.sopc.charSize);
                bw.Write(ncgr.sopc.nChar);
            }

            bw.Flush();
            bw.Close();
        }

        private void Update_Struct()
        {
            ncgr.rahc.nTilesX = (ushort)(Width / 8);
            ncgr.rahc.nTilesY = (ushort)(Height / 8);

            ncgr.rahc.data = Tiles;
            if (this.TileForm == PluginInterface.Images.TileForm.Lineal && ncgr.order == PluginInterface.Images.TileForm.Horizontal)
            {
                ncgr.rahc.data = Actions.HorizontalToLineal(Tiles, ncgr.rahc.nTilesX, ncgr.rahc.nTilesY, TileWidth);
                Set_Tiles(ncgr.rahc.data, this.Width, this.Height, this.ColorFormat, ncgr.order, true);
            }

            ncgr.rahc.depth = ColorFormat;

            ncgr.rahc.size_tiledata = (uint)ncgr.rahc.data.Length;
            ncgr.rahc.size_section = ncgr.rahc.size_tiledata + 0x24;
            ncgr.header.file_size = ncgr.rahc.size_section + 0x10;
        }

        public struct sNCGR  // Nintendo Character Graphic Resource
        {
            public Header header;
            public RAHC rahc;
            public SOPC sopc;
            public TileForm order;
            public Object other;
            public UInt32 id;
        }
        public struct RAHC  // CHARacter
        {
            public char[] id;               // Always RAHC = 0x52414843
            public UInt32 size_section;
            public UInt16 nTilesY;
            public UInt16 nTilesX;
            public ColorFormat depth;
            public UInt16 unknown1;
            public UInt16 unknown2;
            public UInt32 tiledFlag;
            public UInt32 size_tiledata;
            public UInt32 unknown3;         // Always 0x18 (24) (data offset?)
            public byte[] data;             // image data
        }
        public struct SOPC  // Unknown section
        {
            public char[] id;
            public UInt32 size_section;
            public UInt32 unknown1;
            public UInt16 charSize;
            public UInt16 nChar;
        }

    }
}