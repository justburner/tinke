﻿/*
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
    public class A_CHR
    {
        #region [ Variables ]

        protected string fileName;
        protected int id;

        public List<CHR> chrs;

        #endregion

        #region [ Properties ]

        public string FileName { get { return fileName; } }
        public int ID { get { return id; } }

        #endregion

        public A_CHR(string file, int id, string fileName = "")
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

            chrs = new List<CHR>();
            uint startVal = br.ReadUInt32();
            uint startOff = (startVal + 0x3F) & 0xFFFFFFC0;
            if ((startVal >> 2) == 0) return;

            // Hella weird format... wtf o_O
            for (int index = 0; index < (startVal >> 2); index++)
            {
                // Seek offset
                uint offset = startOff;
                if (index != 0)
				{
                    br.BaseStream.Seek(index * 4, SeekOrigin.Begin);
                    offset += (br.ReadUInt32() + 0x3F) & 0xFFFFFFC0;
                }

                // Read image...
                br.BaseStream.Seek(offset, SeekOrigin.Begin);
                chrs.Add(new CHR(br));
            }

            br.Close();
        }

        public void Write(string fileOut)
        {
            int numChrs = chrs.Count;
            if (numChrs >= 16)
			{
                MessageBox.Show("Saving more than 15 images isn't supported.\nWrite aborted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // ... not hard to implement but blame the dumb format.
                return;
			}

            BinaryWriter bw = new BinaryWriter(File.OpenWrite(fileOut));

            uint startVal = (uint)numChrs * 4U;
            uint[] offsets = new uint[16];
            offsets[0] = startVal;
            bw.Write(offsets.SelectMany(BitConverter.GetBytes).ToArray());  // Write dummy header

            for (int i = 0; i < 15;)
            {
                if (i >= chrs.Count) break;

                // Write position
                CHR chr = chrs[i];
                chr.Write(bw);

                // Pad into 64 bytes pages
                uint padStart = (uint)bw.BaseStream.Position;
                uint padEnd = (padStart + 0x3F) & 0xFFFFFFC0;
                for (uint j = padStart; j < padEnd; j++) bw.Write((byte)0);
                offsets[++i] = padEnd - 64U;
            }

            // Rewrite offsets
            bw.Seek(0, SeekOrigin.Begin);
            bw.Write(offsets.SelectMany(BitConverter.GetBytes).ToArray());

            bw.Close();
        }

        public int Count
        {
            get { return chrs.Count; }
        }

        public Color[][] GetPalette(int index)
		{
            if (index < 0 || index >= chrs.Count)
                return null;
            return chrs[index].palette;
		}

        public CHR GetCHR(int index)
        {
            if (index < 0 || index >= chrs.Count)
                return null;
            return chrs[index];
        }

        public bool SwapCHRs(int index1, int index2)
        {
            if (index1 < 0 || index1 >= chrs.Count)
                return false;
            if (index2 < 0 || index2 >= chrs.Count)
                return false;
            if (index1 == index2) return true;
            CHR tmp = chrs[index1];
            chrs[index1] = chrs[index2];
            chrs[index2] = tmp;
            return true;
        }

        public int NewCHR()
        {
            chrs.Add(new CHR(16, 1));
            return chrs.Count - 1;
        }

        public bool DeleteCHR(int index)
        {
            if (index < 0 || index >= chrs.Count)
                return false;
            chrs.RemoveAt(index);
            return true;
        }
    }
}
