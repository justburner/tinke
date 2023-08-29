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

        bool blockSignals;

        int currSprite;
        int currFrame;
        int playFrame;

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
            Update_List(true);
            Update_Animation();
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

        public void Update_Animation()
		{
            Size size = pictureSprite.ClientSize;
            Bitmap bmp = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(bmp);
            int centerX = bmp.Width / 2;
            int centerY = bmp.Height * 3 / 4;

            Pen axisPen = new Pen(Color.FromArgb(128, 0, 0, 0));
            g.DrawLine(axisPen, 0, centerY, bmp.Width, centerY);
            g.DrawLine(axisPen, centerX, 0, centerX, bmp.Height);

            if (pictureSprite.Image != null) pictureSprite.Image.Dispose();
            pictureSprite.Image = bmp;

            g.Dispose();

            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            int paletteSlot = (int)numericPalette.Value;

            if (checkBoxPlay.Checked)
            {
                if (playFrame >= chrSprite.frames.Count)
                    playFrame = 0;

                CHR.Frame chrFrame = chrSprite.frames[playFrame];

                for (int i = chrFrame.layers.Count - 1; i >= 0; i--)
                {
                    CHR.Layer chrPart = chrFrame.layers[i];
                    dCHR.DrawLayer(bmp, centerX, centerY, ref chrPart, paletteSlot);
                }
            }
            else
            {
                if (currFrame >= chrSprite.frames.Count)
                    currFrame = 0;

                CHR.Frame chrFrame = chrSprite.frames[currFrame];

                for (int i = chrFrame.layers.Count - 1; i >= 0; i--)
                {
                    CHR.Layer chrPart = chrFrame.layers[i];
                    dCHR.DrawLayer(bmp, centerX, centerY, ref chrPart, paletteSlot);
                }
            }

            pictureSprite.Update();
        }

        public void Update_List(bool relistLayers)
        {
            groupSprite.Visible = (dCHR.sprites.Count > 0);
            pictureSprite.Visible = (dCHR.sprites.Count > 0);

            if (dCHR.sprites.Count <= 0)
                return;

            blockSignals = true;

            numericSprite.Maximum = Math.Max(dCHR.sprites.Count, 0);
            labelNumSprites.Text = string.Format("of {0}", dCHR.sprites.Count);

            int paletteSlot = (int)numericPalette.Value;

            currSprite = (int)numericSprite.Value - 1;
            if (currSprite >= dCHR.sprites.Count)
                currSprite = 0;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];

            numericFrame.Maximum = Math.Max(chrSprite.frames.Count, 0);
            labelNumFrames.Text = string.Format("of {0}", chrSprite.frames.Count);

            currFrame = (int)numericFrame.Value - 1;
            if (currFrame >= chrSprite.frames.Count)
                currFrame = 0;
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            blockSignals = false;
        }

        private void numericPalette_ValueChanged(object sender, EventArgs e)
        {
            Update_Palette();
            Update_Tileset();
            Update_List(false);
            Update_Animation();

        }

        private void numericPreviewWidth_ValueChanged(object sender, EventArgs e)
        {
            Update_Tileset();
        }

        private void numericSprite_ValueChanged(object sender, EventArgs e)
        {
            numericFrame.Value = 1;

            SetupTimer();
            Update_List(true);
            Update_Animation();
        }

        private void numericFrame_ValueChanged(object sender, EventArgs e)
        {
            Update_List(true);
            Update_Animation();
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
            Update_List(false);
            Update_Animation();
        }

        private void buttonExportTiles_Click(object sender, EventArgs e)
        {
            SaveFileDialog o = new SaveFileDialog();
            o.AddExtension = true;
            o.CheckPathExists = true;
            o.DefaultExt = ".png";
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
            Update_List(false);
            Update_Animation();
        }

        private void checkMaxTiles_CheckedChanged(object sender, EventArgs e)
        {
            numericMaxTiles.Enabled = checkMaxTiles.Checked;
        }

        private bool CheckAnimation()
		{
            // Avoid empty objects
            if (dCHR.sprites.Count == 0)
            {
                MessageBox.Show("Atleast 1 sprite is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            for (int s = 0; s < dCHR.sprites.Count; s++)
            {
                var sprite = dCHR.sprites[s];
                if (sprite.frames.Count == 0)
                {
                    MessageBox.Show(string.Format("Atleast 1 frame is required in sprite {0}.", s), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                for (int f = 0; f < sprite.frames.Count; f++)
                {
                    var frame = sprite.frames[f];
                    if (frame.layers.Count == 0)
                    {
                        MessageBox.Show(string.Format("Atleast 1 layer is required in sprite {0} frame {1}.", s, f), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            return true;
        }

        private void buttonEditor_Click(object sender, EventArgs e)
		{
            CHRAnimator animator = new CHRAnimator(dCHR);

            timerPlayer.Enabled = false;
            if (animator.RunDialog() == DialogResult.OK)
			{
                if (CheckAnimation())
				{
                    // Encode and Write file
                    dCHR.EncodeCharacterData();
                    Write_File();
                }
            }

            // Restore character data
            dCHR.DecodeCharacterData();
            timerPlayer.Enabled = checkBoxPlay.Checked;
        }

        public void SetupTimer()
        {
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];

            // Increment frame
            if (playFrame >= chrSprite.frames.Count)
                playFrame = 0;
            CHR.Frame chrFrame = chrSprite.frames[playFrame];

            // Set interval for next frame
            int ticks = Math.Max((int)chrFrame.ticks, 1);
            timerPlayer.Interval = (ticks * 1000) / 60;
        }

        private void checkBoxPlay_CheckedChanged(object sender, EventArgs e)
        {
            playFrame = currFrame;
            SetupTimer();

            Update_Animation();
            timerPlayer.Enabled = checkBoxPlay.Checked;
            numericFrame.Enabled = !checkBoxPlay.Checked;
        }

        private void timerPlayer_Tick(object sender, EventArgs e)
        {
            playFrame++;
            SetupTimer();

            Update_Animation();
        }
    }
}
