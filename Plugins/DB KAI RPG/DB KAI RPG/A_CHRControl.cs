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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ekona;
using Ekona.Images;

namespace DB_KAI_RPG
{
    public partial class A_CHRControl : UserControl
    {
        IPluginHost pluginHost;
        A_CHR dACHR;
        bool listRefreshBlock;

        public A_CHRControl()
        {
            InitializeComponent();
        }

        public A_CHRControl(Ekona.IPluginHost pluginHost, A_CHR dACHR)
        {
            InitializeComponent();

            this.pluginHost = pluginHost;
            this.dACHR = dACHR;
            this.listRefreshBlock = false;

            Update_List(0);
            Update_Palette();
            Update_Tileset();
        }

        private void Write_File()
        {
            if (dACHR.ID > 0)
            {
                try
                {
                    String fileOut = pluginHost.Get_TempFile();
                    dACHR.Write(fileOut);
                    pluginHost.ChangeFile(dACHR.ID, fileOut);
                }
                catch (Exception ex) { MessageBox.Show("Error writing new dACHR:\n" + ex.Message); };
            }
        }

        public void Update_List(int index)
		{
            listRefreshBlock = true;

            listBoxChrs.Items.Clear();

            int count = dACHR.chrs.Count;
            for (int i = 0; i < count; i++)
            {
                CHR chr = dACHR.chrs[i];
                listBoxChrs.Items.Add(string.Format("{0}: Pals={1}, Tiles={2}", i, chr.palette.Length, chr.tiles.Length));
            }

            if (index >= 0 && index < listBoxChrs.Items.Count)
                listBoxChrs.SelectedIndex = index;

            listRefreshBlock = false;
        }

        public void Update_Palette()
        {
            int index = listBoxChrs.SelectedIndex;
            CHR chr = dACHR.GetCHR(index);

            if (chr != null)
			{
                numericPalette.Maximum = chr.palette.Length - 1;
                labelNumPalettes.Text = string.Format("of {0}", chr.palette.Length - 1);
                if (numericPalette.Value < chr.palette.Length)
                    picturePalette.Image = Transform.Get1DPaletteBitmap(picturePalette.Width, picturePalette.Height, chr.palette[(int)numericPalette.Value], 16);
                else
                    picturePalette.Image = null;
            }
            else
			{
                numericPalette.Maximum = 0;
                labelNumPalettes.Text = string.Format("of 0");
                picturePalette.Image = null;
            }
        }

        public void Update_Tileset()
        {
            int index = listBoxChrs.SelectedIndex;
            CHR chr = dACHR.GetCHR(index);

            if (chr != null)
            {
                numericMaxTiles.Value = chr.tiles.Length;
                if (numericPalette.Value < chr.palette.Length)
                    pictureTileset.Image = Transform.Get4bppTilesBitmap(chr.tiles, chr.palette[(int)numericPalette.Value], (int)numericPreviewWidth.Value);
                else
                    pictureTileset.Image = null;
            }
            else
            {
                pictureTileset.Image = null;
            }
        }

        private void numericPalette_ValueChanged(object sender, EventArgs e)
        {
            Update_Palette();
            Update_Tileset();
        }

        private void numericPreviewWidth_ValueChanged(object sender, EventArgs e)
        {
            Update_Tileset();
        }

        private void listBoxChrs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listRefreshBlock) return;
            Update_Palette();
            Update_Tileset();
        }

        private void buttonExportPal_Click(object sender, EventArgs e)
        {
            int index = listBoxChrs.SelectedIndex;
            CHR chr = dACHR.GetCHR(index);
            if (chr == null) return;

            Transform.PalFormat format;
            string fileName = Transform.ExportPaletteDialog(dACHR.FileName, out format);
            if (String.IsNullOrEmpty(fileName)) return;

            Color[] fullPal = Transform.Get1DPalette(chr.palette, 16);
            Transform.ExportPalette(fileName, format, fullPal);
        }

        private void buttonImportPal_Click(object sender, EventArgs e)
        {
            int index = listBoxChrs.SelectedIndex;
            CHR chr = dACHR.GetCHR(index);
            if (chr == null) return;

            Transform.PalFormat format;
            string fileName = Transform.ImportPaletteDialog(dACHR.FileName, out format);
            if (String.IsNullOrEmpty(fileName)) return;

            Color[] palette = Transform.ImportPalette(fileName, format);
            if (palette == null)
			{
                MessageBox.Show("Invalid palette file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int slots = (palette.Length + 15) / 16;
            palette = Transform.Resize1DPalette(palette, slots * 16);
            chr.palette = Transform.Get2DPalette(palette, 16);
            numericPalette.Value = 0;

            // Write file
            Write_File();

            Update_List(listBoxChrs.SelectedIndex);
            Update_Palette();
        }

        private void buttonExportTiles_Click(object sender, EventArgs e)
        {
            int index = listBoxChrs.SelectedIndex;
            CHR chr = dACHR.GetCHR(index);
            if (chr == null) return;

            SaveFileDialog o = new SaveFileDialog();
            o.AddExtension = true;
            o.CheckPathExists = true;
            o.DefaultExt = ".png";
            o.Filter = "Portable Network Graphics (*.png)|*.png";
            o.OverwritePrompt = true;
            o.FileName = Path.ChangeExtension(chr.FileName, ".png");

            if (o.ShowDialog() != DialogResult.OK)
                return;

            Transform.ExportTilesetPNG(o.FileName, chr.tiles, chr.palette[(int)numericPalette.Value], (int)numericPreviewWidth.Value);
        }

        private void buttonImportTiles_Click(object sender, EventArgs e)
        {
            int index = listBoxChrs.SelectedIndex;
            CHR chr = dACHR.GetCHR(index);
            if (chr == null) return;

            OpenFileDialog o = new OpenFileDialog();
            o.CheckFileExists = true;
            o.Filter = "All supported formats|*.png;*.bmp;*.jpg;*.jpeg;*.tif;*.tiff;*.gif;*.ico;*.icon";

            if (o.ShowDialog() != DialogResult.OK)
                return;

            int maxTiles = (int)numericMaxTiles.Value;
            if (!checkMaxTiles.Checked)
                maxTiles = -1;

            byte[][] tiles = Transform.Import4bppTilesetFromImage(o.FileName, chr.palette[(int)numericPalette.Value], maxTiles);
            if (tiles == null)
            {
                MessageBox.Show("Invalid tileset file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            chr.tiles = tiles;

            // Write file
            Write_File();

            Update_List(listBoxChrs.SelectedIndex);
            Update_Tileset();
        }

        private void checkMaxTiles_CheckedChanged(object sender, EventArgs e)
        {
            numericMaxTiles.Enabled = checkMaxTiles.Checked;
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = listBoxChrs.SelectedIndex;
            if (dACHR.SwapCHRs(index, index - 1))
            {
                listBoxChrs.SelectedIndex--;

                // Write file
                Write_File();

                Update_List(listBoxChrs.SelectedIndex);
                Update_Palette();
                Update_Tileset();
            }
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = listBoxChrs.SelectedIndex;
            if (dACHR.SwapCHRs(index, index + 1))
			{
                listBoxChrs.SelectedIndex++;

                // Write file
                Write_File();

                Update_List(listBoxChrs.SelectedIndex);
                Update_Palette();
                Update_Tileset();
            }
        }

        private void createNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = dACHR.NewCHR();

            // Write file
            Write_File();

            Update_List(index);
            Update_Palette();
            Update_Tileset();
        }

        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dACHR.DeleteCHR(listBoxChrs.SelectedIndex))
			{
                // Write file
                Write_File();

                Update_List(listBoxChrs.SelectedIndex);
                Update_Palette();
                Update_Tileset();
            }
        }
    }
}
