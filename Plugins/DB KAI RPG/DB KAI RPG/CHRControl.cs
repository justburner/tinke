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
    public partial class CHRControl : UserControl
    {
        IPluginHost pluginHost;
        CHR dCHR;

        public CHRControl()
        {
            InitializeComponent();
        }

        public CHRControl(Ekona.IPluginHost pluginHost, CHR dCHR)
        {
            InitializeComponent();

            this.pluginHost = pluginHost;
            this.dCHR = dCHR;

            Update_Palette();
            Update_Tileset();
            Update_Animation(true);
        }

        private void Write_File()
        {
            if (dCHR.ID > 0)
            {
                try
                {
                    String fileOut = pluginHost.Get_TempFile();
                    dCHR.Write(fileOut);
                    pluginHost.ChangeFile(dCHR.ID, fileOut);
                }
                catch (Exception ex) { MessageBox.Show("Error writing new dCHR:\n" + ex.Message); };
            }
        }

        public void Update_Palette()
        {
            numericPalette.Maximum = dCHR.palette.Length - 1;
            labelNumPalettes.Text = string.Format("of {0}", dCHR.palette.Length - 1);
            if (numericPalette.Value < dCHR.palette.Length)
                picturePalette.Image = Transform.Get1DPaletteBitmap(picturePalette.Width, picturePalette.Height, dCHR.palette[(int)numericPalette.Value], 16);
            else
                picturePalette.Image = null;

            if (dCHR.chrFlags == 0)
                labelDebug.Text = "";
            else
                labelDebug.Text = string.Format("Data Off.: ${0:X06}\nData Size: {1} bytes", dCHR.chrDataPos, dCHR.chrData.Length);
        }

        public void Update_Tileset()
        {
            numericMaxTiles.Value = dCHR.tiles.Length;
            if (numericPalette.Value < dCHR.palette.Length)
                pictureTileset.Image = Transform.Get4bppTilesBitmap(dCHR.tiles, dCHR.palette[(int)numericPalette.Value], (int)numericPreviewWidth.Value);
            else
                pictureTileset.Image = null;
        }

        public void Update_Animation(bool relistLayers)
        {
            groupSprite.Visible = (dCHR.sprites.Count > 0);
            pictureSprite.Visible = (dCHR.sprites.Count > 0);

            if (dCHR.sprites.Count <= 0)
                return;

            numericSprite.Maximum = Math.Max(dCHR.sprites.Count, 0);
            labelNumSprites.Text = string.Format("of {0}", dCHR.sprites.Count);

            int sprite = (int)numericSprite.Value - 1;
            int paletteSlot = (int)numericPalette.Value;

            Bitmap bmp = new Bitmap(pictureSprite.Width, pictureSprite.Height);
            Graphics g = Graphics.FromImage(bmp);
            int centerX = bmp.Width / 2;
            int centerY = bmp.Height / 2;

            Pen axisPen = new Pen(Color.FromArgb(128, 0, 0, 0));
            g.DrawLine(axisPen, 0, centerX, bmp.Width, centerX);
            g.DrawLine(axisPen, centerY, 0, centerY, bmp.Height);
            pictureSprite.Image = bmp;
            g.Dispose();

            if (sprite >= dCHR.sprites.Count)
                return;
            CHR.Sprite chrSprite = dCHR.sprites[sprite];

            numericFrame.Maximum = Math.Max(chrSprite.frames.Count, 0);
            labelNumFrames.Text = string.Format("of {0}", chrSprite.frames.Count);

            int frame = (int)numericFrame.Value - 1;

            if (frame >= chrSprite.frames.Count)
                return;
            CHR.Frame chrFrame = chrSprite.frames[frame];

            labelNumLayers.Text = "Character data info:\n";
            labelNumLayers.Text += string.Format("\nTotal layers: {0}\nExtended?: {1}\nFrame Ticks: {2}\n\nData offset: ${3:X06}\nData remain: {4}", chrFrame.layers.Count, chrFrame.extended ? "yes" : "no", chrFrame.ticks, dCHR.chrDataPos + chrSprite.debugOffset, chrSprite.debugRemain);

            // List all layers
            if (relistLayers)
            {
                listLayers.Items.Clear();
                listLayers.ClearSelected();
                for (int i = 0; i < chrFrame.layers.Count; i++)
                {
                    CHR.Layer chrPart = chrFrame.layers[i];
                    listLayers.Items.Add(chrPart.ToString());
                }
            }

            // Render last to first, non selected to selected
            if (listLayers.SelectedIndex != -1)
            {
                for (int i = chrFrame.layers.Count - 1; i >= 0; i--)
                {
                    bool selected = listLayers.GetSelected(i);
                    if (!selected)
                    {
                        CHR.Layer chrPart = chrFrame.layers[i];
                        dCHR.DrawLayer(bmp, centerX, centerY, ref chrPart, paletteSlot, false);
                    }
                }
            }
            for (int i = chrFrame.layers.Count - 1; i >= 0; i--)
            {
                bool selected = listLayers.GetSelected(i) || listLayers.SelectedIndex == -1;
                if (selected)
                {
                    CHR.Layer chrPart = chrFrame.layers[i];
                    dCHR.DrawLayer(bmp, centerX, centerY, ref chrPart, paletteSlot, true);
                }
            }
            pictureSprite.Update();
        }

        private void numericPalette_ValueChanged(object sender, EventArgs e)
        {
            Update_Palette();
            Update_Tileset();
            Update_Animation(false);
        }

        private void numericPreviewWidth_ValueChanged(object sender, EventArgs e)
        {
            Update_Tileset();
        }

        private void numericSprite_ValueChanged(object sender, EventArgs e)
        {
            numericFrame.Value = 1;
            Update_Animation(true);
        }

        private void numericFrame_ValueChanged(object sender, EventArgs e)
        {
            Update_Animation(true);
        }

        private void listLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            Update_Animation(false);
        }

        private void buttonExportPal_Click(object sender, EventArgs e)
        {
            Transform.PalFormat format;
            string fileName = Transform.ExportPaletteDialog(dCHR.FileName, out format);
            if (String.IsNullOrEmpty(fileName)) return;

            Color[] fullPal = Transform.Get1DPalette(dCHR.palette, 16);
            Transform.ExportPalette(fileName, format, fullPal);
        }

        private void buttonImportPal_Click(object sender, EventArgs e)
        {
            Transform.PalFormat format;
            string fileName = Transform.ImportPaletteDialog(dCHR.FileName, out format);
            if (String.IsNullOrEmpty(fileName)) return;

            Color[] palette = Transform.ImportPalette(fileName, format);
            if (palette == null)
            {
                MessageBox.Show("Invalid palette file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int slots = (palette.Length + 15) / 16;
            palette = Transform.Resize1DPalette(palette, slots * 16);
            dCHR.palette = Transform.Get2DPalette(palette, 16);
            numericPalette.Value = 0;

            // Write file
            Write_File();

            Update_Palette();
            Update_Tileset();
            Update_Animation(false);
        }

        private void buttonExportTiles_Click(object sender, EventArgs e)
        {
            SaveFileDialog o = new SaveFileDialog();
            o.AddExtension = true;
            o.CheckPathExists = true;
            o.DefaultExt = ".pal";
            o.Filter = "Portable Network Graphics (*.png)|*.png";
            o.OverwritePrompt = true;
            o.FileName = Path.ChangeExtension(dCHR.FileName, ".png");

            if (o.ShowDialog() != DialogResult.OK)
                return;

            Transform.ExportTilesetPNG(o.FileName, dCHR.tiles, dCHR.palette[(int)numericPalette.Value], (int)numericPreviewWidth.Value);
        }

        private void buttonImportTiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.CheckFileExists = true;
            o.Filter = "All supported formats|*.png;*.bmp;*.jpg;*.jpeg;*.tif;*.tiff;*.gif;*.ico;*.icon";

            if (o.ShowDialog() != DialogResult.OK)
                return;

            int maxTiles = (int)numericMaxTiles.Value;
            if (!checkMaxTiles.Checked)
                maxTiles = -1;

            byte[][] tiles = Transform.Import4bppTilesetFromImage(o.FileName, dCHR.palette[(int)numericPalette.Value], maxTiles);
            if (tiles == null)
            {
                MessageBox.Show("Invalid tileset file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            dCHR.tiles = tiles;

            // Write file
            Write_File();

            Update_Tileset();
            Update_Animation(false);
        }

        private void checkMaxTiles_CheckedChanged(object sender, EventArgs e)
        {
            numericMaxTiles.Enabled = checkMaxTiles.Checked;
        }

        private void deselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listLayers.ClearSelected();
        }
    }
}
