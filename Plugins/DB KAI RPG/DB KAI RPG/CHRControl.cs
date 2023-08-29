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
        bool dirty;
        int currSprite;
        int currFrame;
        static List<CHR.Layer> clipboardLayers = new List<CHR.Layer>();

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

            dirty = false;
            Update_Dirty(false);
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

            blockSignals = true;

            numericSprite.Maximum = Math.Max(dCHR.sprites.Count, 0);
            labelNumSprites.Text = string.Format("of {0}", dCHR.sprites.Count);

            int paletteSlot = (int)numericPalette.Value;

            Bitmap bmp = new Bitmap(pictureSprite.Width, pictureSprite.Height);
            Graphics g = Graphics.FromImage(bmp);
            int centerX = bmp.Width / 2;
            int centerY = bmp.Height * 3 / 4;

            Pen axisPen = new Pen(Color.FromArgb(128, 0, 0, 0));
            g.DrawLine(axisPen, 0, centerY, bmp.Width, centerY);
            g.DrawLine(axisPen, centerX, 0, centerX, bmp.Height);
            pictureSprite.Image = bmp;
            g.Dispose();

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

            numericFrameTicks.Value = chrFrame.ticks;

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
            else
            {
                for (int i = 0; i < listLayers.Items.Count; i++)
                {
                    CHR.Layer chrPart = chrFrame.layers[i];
                    bool selected = listLayers.GetSelected(i);
                    listLayers.Items[i] = chrPart.ToString();
                    listLayers.SetSelected(i, selected);
                }
            }

            // Setup layer components
            var indices = listLayers.SelectedIndices;
            if (indices.Count == 0)
            {
                numericTileID.Enabled = false;
                numericPosX.Enabled = false;
                numericPosY.Enabled = false;
                comboBoxObjSize.Enabled = false;
                numericTileID.Value = 0;
                numericPosX.Value = 0;
                numericPosY.Value = 0;
                comboBoxObjSize.SelectedIndex = 0;

                checkBoxHFlip.Enabled = false;
                checkBoxVFlip.Enabled = false;
                checkBoxTranslucent.Enabled = false;
                checkBoxHFlip.Checked = false;
                checkBoxVFlip.Checked = false;
                checkBoxTranslucent.Checked = false;
            }
            else if (indices.Count == 1)
            {
                CHR.Layer chrPart = chrFrame.layers[listLayers.SelectedIndex];
                numericTileID.Enabled = true;
                numericPosX.Enabled = true;
                numericPosY.Enabled = true;
                comboBoxObjSize.Enabled = true;
                numericTileID.Value = chrPart.tile;
                numericPosX.Value = chrPart.x;
                numericPosY.Value = chrPart.y;
                comboBoxObjSize.SelectedIndex = chrPart.objsize;

                checkBoxHFlip.Enabled = true;
                checkBoxVFlip.Enabled = true;
                checkBoxTranslucent.Enabled = true;
                checkBoxHFlip.Checked = chrPart.hflip;
                checkBoxVFlip.Checked = chrPart.vflip;
                checkBoxTranslucent.Checked = chrPart.translucent;
            }
            else
            {
                numericTileID.Enabled = true;
                numericPosX.Enabled = true;
                numericPosY.Enabled = true;
                comboBoxObjSize.Enabled = true;
                numericPosX.Value = 0;
                numericPosY.Value = 0;
                comboBoxObjSize.SelectedIndex = -1;

                checkBoxHFlip.Enabled = true;
                checkBoxVFlip.Enabled = true;
                checkBoxTranslucent.Enabled = true;
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

            blockSignals = false;

            pictureSprite.Update();
        }

        private void Update_Dirty(bool setDirty)
        {
            if (setDirty) dirty = true;
            buttonSave.Enabled = dirty;
            buttonRevert.Enabled = dirty;
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
            if (!blockSignals) Update_Animation(false);
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
            Update_Animation(false);
        }

        private void checkMaxTiles_CheckedChanged(object sender, EventArgs e)
        {
            numericMaxTiles.Enabled = checkMaxTiles.Checked;
        }

        private void numericFrameTicks_ValueChanged(object sender, EventArgs e)
        {
            if (blockSignals) return;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            chrFrame.ticks = (ushort)numericFrameTicks.Value;
            Update_Dirty(true);
        }

        private void numericTileID_ValueChanged(object sender, EventArgs e)
        {
            if (blockSignals) return;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            for (int i = chrFrame.layers.Count - 1; i >= 0; i--)
            {
                if (listLayers.GetSelected(i))
                {
                    CHR.Layer chrPart = chrFrame.layers[i];
                    chrPart.tile = (ushort)numericTileID.Value;
                }
            }

            Update_Animation(false);
            Update_Dirty(true);
        }

        private void numericPosX_ValueChanged(object sender, EventArgs e)
        {
            if (blockSignals) return;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            var indices = listLayers.SelectedIndices;
            if (indices.Count == 1)
            {
                CHR.Layer chrPart = chrFrame.layers[indices[0]];
                chrPart.x = (int)numericPosX.Value;
            }
            else
            {
                foreach (int index in indices)
                {
                    CHR.Layer chrPart = chrFrame.layers[index];
                    chrPart.x += (int)numericPosX.Value;
                }
                blockSignals = true;
                numericPosX.Value = 0;
                blockSignals = false;
            }

            Update_Animation(false);
            Update_Dirty(true);
        }

        private void numericPosY_ValueChanged(object sender, EventArgs e)
        {
            if (blockSignals) return;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            var indices = listLayers.SelectedIndices;
            if (indices.Count == 1)
            {
                CHR.Layer chrPart = chrFrame.layers[indices[0]];
                chrPart.y = (int)numericPosY.Value;
            }
            else
            {
                foreach (int index in indices)
                {
                    CHR.Layer chrPart = chrFrame.layers[index];
                    chrPart.y += (int)numericPosY.Value;
                }
                blockSignals = true;
                numericPosY.Value = 0;
                blockSignals = false;
            }

            Update_Animation(false);
            Update_Dirty(true);
        }

        private void comboBoxObjSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (blockSignals) return;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            var indices = listLayers.SelectedIndices;
            if (indices.Count == 1)
            {
                CHR.Layer chrPart = chrFrame.layers[indices[0]];
                chrPart.objsize = comboBoxObjSize.SelectedIndex;
            }
            else
            {
                foreach (int index in indices)
                {
                    CHR.Layer chrPart = chrFrame.layers[index];
                    chrPart.objsize = comboBoxObjSize.SelectedIndex;
                }
            }

            Update_Animation(false);
            Update_Dirty(true);
        }

        private void checkBoxHFlip_CheckStateChanged(object sender, EventArgs e)
        {
            if (blockSignals) return;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            var indices = listLayers.SelectedIndices;
            if (indices.Count == 1)
            {
                CHR.Layer chrPart = chrFrame.layers[indices[0]];
                chrPart.hflip = checkBoxHFlip.Checked;
            }
            else
            {
                foreach (int index in indices)
                {
                    CHR.Layer chrPart = chrFrame.layers[index];
                    chrPart.hflip = checkBoxHFlip.Checked;
                }
            }

            Update_Animation(false);
            Update_Dirty(true);
        }

        private void checkBoxVFlip_CheckStateChanged(object sender, EventArgs e)
        {
            if (blockSignals) return;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            var indices = listLayers.SelectedIndices;
            if (indices.Count == 1)
            {
                CHR.Layer chrPart = chrFrame.layers[indices[0]];
                chrPart.vflip = checkBoxVFlip.Checked;
            }
            else
            {
                foreach (int index in indices)
                {
                    CHR.Layer chrPart = chrFrame.layers[index];
                    chrPart.vflip = checkBoxVFlip.Checked;
                }
            }

            Update_Animation(false);
            Update_Dirty(true);
        }

        private void checkBoxTranslucent_CheckStateChanged(object sender, EventArgs e)
        {
            if (blockSignals) return;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            var indices = listLayers.SelectedIndices;
            if (indices.Count == 1)
            {
                CHR.Layer chrPart = chrFrame.layers[indices[0]];
                chrPart.translucent = checkBoxTranslucent.Checked;
            }
            else
            {
                foreach (int index in indices)
                {
                    CHR.Layer chrPart = chrFrame.layers[index];
                    chrPart.translucent = checkBoxTranslucent.Checked;
                }
            }

            Update_Animation(false);
            Update_Dirty(true);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            // Avoid empty objects
            if (dCHR.sprites.Count == 0)
            {
                MessageBox.Show("Atleast 1 sprite is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int s = 0; s < dCHR.sprites.Count; s++)
            {
                var sprite = dCHR.sprites[s];
                if (sprite.frames.Count == 0)
                {
                    MessageBox.Show(string.Format("Atleast 1 frame is required in sprite {0}.", s), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                for (int f = 0; f < sprite.frames.Count; f++)
                {
                    var frame = sprite.frames[f];
                    if (frame.layers.Count == 0)
                    {
                        MessageBox.Show(string.Format("Atleast 1 layer is required in sprite {0} frame {1}.", s, f), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            // Write file
            dCHR.EncodeCharacterData();
            Write_File();

            dirty = false;
            Update_Dirty(false);
        }

        private void buttonRevert_Click(object sender, EventArgs e)
        {
            dCHR.DecodeCharacterData();

            numericSprite.Value = 1;
            numericFrame.Value = 1;
            Update_Animation(true);

            dirty = false;
            Update_Dirty(false);
        }

        private void deselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listLayers.ClearSelected();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];
            var indices = listLayers.SelectedIndices;
            if (indices.Count == 0) return;

            clipboardLayers.Clear();
            foreach (int index in indices)
            {
                CHR.Layer chrPart = chrFrame.layers[index];
                clipboardLayers.Add(chrPart.Clone());
            }
            foreach (var layer in clipboardLayers)
            {
                chrFrame.layers.Remove(layer);
            }

            Update_Animation(true);
            Update_Dirty(true);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];
            var indices = listLayers.SelectedIndices;
            if (indices.Count == 0) return;

            clipboardLayers.Clear();
            foreach (int index in indices)
            {
                CHR.Layer chrPart = chrFrame.layers[index];
                clipboardLayers.Add(chrPart.Clone());
            }
        }

        private void pasteBeforeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];
            var indices = listLayers.SelectedIndices;

            if (indices.Count == 0)
            {
                int index = 0;
                foreach (var layer in clipboardLayers)
                {
                    chrFrame.layers.Insert(index++, layer.Clone());
                }
            }
            else
            {
                int index = indices[0];
                foreach (var layer in clipboardLayers)
                {
                    chrFrame.layers.Insert(index++, layer);
                }
            }

            Update_Animation(true);
            Update_Dirty(true);
        }

        private void pasteAfterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];
            var indices = listLayers.SelectedIndices;

            if (indices.Count == 0)
            {
                foreach (var layer in clipboardLayers)
                {
                    chrFrame.layers.Add(layer.Clone());
                }
            }
            else
            {
                int index = indices[indices.Count - 1] + 1;
                foreach (var layer in clipboardLayers)
                {
                    chrFrame.layers.Insert(index++, layer);
                }
            }

            Update_Animation(true);
            Update_Dirty(true);
        }

        private void newBlankLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];
            var indices = listLayers.SelectedIndices;

            if (indices.Count == 0)
            {
                chrFrame.layers.Add(CHR.Layer.Blank());
            }
            else
            {
                int index = indices[indices.Count - 1] + 1;
                chrFrame.layers.Insert(index, CHR.Layer.Blank());
            }

            Update_Animation(true);
            Update_Dirty(true);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];
            var indices = listLayers.SelectedIndices;
            if (indices.Count == 0) return;

            List<CHR.Layer> layers = new List<CHR.Layer>();
            foreach (int index in indices)
            {
                CHR.Layer chrPart = chrFrame.layers[index];
                layers.Add(chrPart);
            }
            foreach (var layer in layers)
            {
                chrFrame.layers.Remove(layer);
            }

            Update_Animation(true);
            Update_Dirty(true);
        }

        private void newFrameBeforeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            chrSprite.frames.Insert(currFrame, chrFrame.Clone());

            Update_Animation(true);
            Update_Dirty(true);
        }

        private void newFrameAfterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            chrSprite.frames.Insert(currFrame + 1, chrFrame.Clone());

            Update_Animation(true);
            Update_Dirty(true);

            numericFrame.Value++;
        }

        private void deleteThisFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];

            if (chrSprite.frames.Count <= 1)
            {
                MessageBox.Show("Cannot delete last frame!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            chrSprite.frames.RemoveAt(currFrame);

            Update_Animation(true);
            Update_Dirty(true);
        }

        private void newSpriteBeforeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];

            dCHR.sprites.Insert(currSprite, chrSprite.Clone());

            Update_Animation(true);
            Update_Dirty(true);
        }

        private void newSpriteAfterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];

            dCHR.sprites.Insert(currSprite + 1, chrSprite.Clone());

            Update_Animation(true);
            Update_Dirty(true);

            numericSprite.Value++;
        }

        private void deleteThisSpriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dCHR.sprites.Count <= 1)
            {
                MessageBox.Show("Cannot delete last sprite!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            dCHR.sprites.RemoveAt(currSprite);

            Update_Animation(true);
            Update_Dirty(true);
        }
    }
}
