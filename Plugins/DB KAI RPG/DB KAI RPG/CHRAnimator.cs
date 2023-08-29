using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_KAI_RPG
{
	public partial class CHRAnimator : Form
	{
		CHR dCHR;
		bool blockSignals;

		int currSprite;
		int currFrame;
        int currExMeta;
        int playFrame;

        static List<CHR.Layer> clipboardLayers = new List<CHR.Layer>();
        static CHR.ExMeta clipboardExMeta = new CHR.ExMeta();

        public CHRAnimator(CHR dCHR)
		{
			InitializeComponent();

			this.dCHR = dCHR;
		}

		public DialogResult RunDialog()
		{
            numericPalette.Maximum = dCHR.palette.Length - 1;
            labelNumPalettes.Text = string.Format("of {0}", dCHR.palette.Length - 1);

            numericSprite.Value = 1;
            numericFrame.Value = 1;

            Update_List(true);
            Update_Animation();

            return ShowDialog();
		}

        public void Update_Animation()
		{
            Size size = pictureSprite.ClientSize;
            Bitmap bmp = new Bitmap(size.Width, size.Height);

            int centerX = bmp.Width / 2;
            int centerY = bmp.Height * 3 / 4;

            if (checkBoxAxisLines.Checked)
			{
                Graphics g = Graphics.FromImage(bmp);

                Pen axisPen = new Pen(Color.FromArgb(128, 0, 0, 0));
                g.DrawLine(axisPen, 0, centerY, bmp.Width, centerY);
                g.DrawLine(axisPen, centerX, 0, centerX, bmp.Height);

                g.Dispose();
            }

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

                // Render last to first, non selected to selected
                if (listBoxLayers.SelectedIndex != -1)
                {
                    for (int i = chrFrame.layers.Count - 1; i >= 0; i--)
                    {
                        bool selected = listBoxLayers.GetSelected(i);
                        if (!selected)
                        {
                            CHR.Layer chrPart = chrFrame.layers[i];
                            dCHR.DrawLayer(bmp, centerX, centerY, ref chrPart, paletteSlot, false);
                        }
                    }
                }
                for (int i = chrFrame.layers.Count - 1; i >= 0; i--)
                {
                    bool selected = listBoxLayers.GetSelected(i) || listBoxLayers.SelectedIndex == -1;
                    if (selected)
                    {
                        CHR.Layer chrPart = chrFrame.layers[i];
                        dCHR.DrawLayer(bmp, centerX, centerY, ref chrPart, paletteSlot, true);
                    }
                }

                // Show active marker
                if (currExMeta >= 0)
                {
                    Graphics g = Graphics.FromImage(bmp);

                    CHR.ExMeta exmeta = chrFrame.exmetas[currExMeta];

                    int x = centerX + exmeta.X;
                    int y = centerY + exmeta.Y;

                    g.DrawRectangle(Pens.Blue, x - 2, y - 2, 4, 4);
                    g.DrawLine(Pens.Red, x - 3, y, x + 3, y);
                    g.DrawLine(Pens.Green, x, y - 3, x, y + 3);

                    g.Dispose();
                }
            }

            if (pictureSprite.Image != null) pictureSprite.Image.Dispose();
            pictureSprite.Image = bmp;

            pictureSprite.Update();
        }

        public void Update_List(bool relistLayers)
        {
            blockSignals = true;

            numericSprite.Maximum = Math.Max(dCHR.sprites.Count, 0);
            labelNumSprites.Text = string.Format("of {0}", dCHR.sprites.Count);

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

            playFrame %= chrSprite.frames.Count;
            numericFrameTicks.Value = chrFrame.ticks;

            labelTickTime.Text = string.Format("{0} ms", ((int)chrFrame.ticks * 1000) / 60);

            // List all layers
            if (relistLayers)
            {
                listBoxLayers.Items.Clear();
                listBoxLayers.ClearSelected();
                for (int i = 0; i < chrFrame.layers.Count; i++)
                {
                    CHR.Layer chrPart = chrFrame.layers[i];
                    listBoxLayers.Items.Add(chrPart.ToString());
                }

                listBoxExMeta.Items.Clear();
                listBoxExMeta.ClearSelected();
                for (int i = 0; i < chrFrame.exmetas.Count; i++)
                {
                    CHR.ExMeta exmeta = chrFrame.exmetas[i];
                    listBoxExMeta.Items.Add(exmeta.ToString());
                }
            }
            else
            {
                for (int i = 0; i < listBoxLayers.Items.Count; i++)
                {
                    CHR.Layer chrPart = chrFrame.layers[i];
                    bool selected = listBoxLayers.GetSelected(i);
                    listBoxLayers.Items[i] = chrPart.ToString();
                    listBoxLayers.SetSelected(i, selected);
                }

                int exSelected = listBoxExMeta.SelectedIndex;
                for (int i = 0; i < listBoxExMeta.Items.Count; i++)
                {
                    CHR.ExMeta exmeta = chrFrame.exmetas[i];
                    listBoxExMeta.Items[i] = exmeta.ToString();
                }
                listBoxExMeta.SelectedIndex = exSelected;
            }

            // Setup layer components
            var indices = listBoxLayers.SelectedIndices;
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
                CHR.Layer chrPart = chrFrame.layers[listBoxLayers.SelectedIndex];
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

            currExMeta = listBoxExMeta.SelectedIndex;
            numericExID.Enabled = (currExMeta >= 0);
            numericExPosX.Enabled = (currExMeta >= 0);
            numericExPosY.Enabled = (currExMeta >= 0);

            blockSignals = false;
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
        }

        private void timerPlayer_Tick(object sender, EventArgs e)
        {
            playFrame++;
            SetupTimer();

            Update_Animation();
        }

        private void checkBoxAxisLines_CheckedChanged(object sender, EventArgs e)
        {
            Update_Animation();
        }

        private void numericPalette_ValueChanged(object sender, EventArgs e)
        {
            Update_List(false);
            Update_Animation();
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

		private void numericFrameTicks_ValueChanged(object sender, EventArgs e)
		{
            if (blockSignals) return;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            chrFrame.ticks = (ushort)numericFrameTicks.Value;
            labelTickTime.Text = string.Format("{0} ms", ((int)chrFrame.ticks * 1000) / 60);

            SetupTimer();
        }

        private void listBoxLayers_SelectedIndexChanged(object sender, EventArgs e)
		{
            if (blockSignals) return;

            Update_List(false);
            Update_Animation();
        }

		private void numericTileID_ValueChanged(object sender, EventArgs e)
		{
            if (blockSignals) return;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            for (int i = chrFrame.layers.Count - 1; i >= 0; i--)
            {
                if (listBoxLayers.GetSelected(i))
                {
                    CHR.Layer chrPart = chrFrame.layers[i];
                    chrPart.tile = (ushort)numericTileID.Value;
                }
            }

            Update_List(false);
            Update_Animation();
        }

		private void numericPosX_ValueChanged(object sender, EventArgs e)
		{
            if (blockSignals) return;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            var indices = listBoxLayers.SelectedIndices;
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

            Update_List(false);
            Update_Animation();
        }

		private void numericPosY_ValueChanged(object sender, EventArgs e)
		{
            if (blockSignals) return;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            var indices = listBoxLayers.SelectedIndices;
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

            Update_List(false);
            Update_Animation();
        }

		private void comboBoxObjSize_SelectedIndexChanged(object sender, EventArgs e)
		{
            if (blockSignals) return;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            var indices = listBoxLayers.SelectedIndices;
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

            Update_List(false);
            Update_Animation();
        }

		private void checkBoxHFlip_CheckedChanged(object sender, EventArgs e)
		{
            if (blockSignals) return;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            var indices = listBoxLayers.SelectedIndices;
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

            Update_List(false);
            Update_Animation();
        }

		private void checkBoxVFlip_CheckedChanged(object sender, EventArgs e)
		{
            if (blockSignals) return;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            var indices = listBoxLayers.SelectedIndices;
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

            Update_List(false);
            Update_Animation();
        }

		private void checkBoxTranslucent_CheckedChanged(object sender, EventArgs e)
		{
            if (blockSignals) return;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            var indices = listBoxLayers.SelectedIndices;
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

            Update_List(false);
            Update_Animation();
        }

		private void deselectToolStripMenuItem_Click(object sender, EventArgs e)
		{
            listBoxLayers.ClearSelected();
        }

		private void cutToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];
            var indices = listBoxLayers.SelectedIndices;
            if (indices.Count == 0) return;

            // Copy
            clipboardLayers.Clear();
            foreach (int index in indices)
            {
                CHR.Layer chrPart = chrFrame.layers[index];
                clipboardLayers.Add(chrPart.Clone());
            }

            // Delete
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

            Update_List(true);
            Update_Animation();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];
            var indices = listBoxLayers.SelectedIndices;
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
            var indices = listBoxLayers.SelectedIndices;

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
                    chrFrame.layers.Insert(index++, layer.Clone());
                }
            }

            Update_List(true);
            Update_Animation();
        }

        private void pasteAfterToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];
            var indices = listBoxLayers.SelectedIndices;

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
                    chrFrame.layers.Insert(index++, layer.Clone());
                }
            }

            Update_List(true);
            Update_Animation();
        }

        private void newBlankToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];
            var indices = listBoxLayers.SelectedIndices;

            if (indices.Count == 0)
            {
                chrFrame.layers.Add(CHR.Layer.Blank());
            }
            else
            {
                int index = indices[indices.Count - 1] + 1;
                chrFrame.layers.Insert(index, CHR.Layer.Blank());
            }

            Update_List(true);
            Update_Animation();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];
            var indices = listBoxLayers.SelectedIndices;
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

            Update_List(true);
            Update_Animation();
        }

		private void listBoxExMeta_SelectedIndexChanged(object sender, EventArgs e)
		{
            if (blockSignals) return;

            blockSignals = true;

            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            currExMeta = listBoxExMeta.SelectedIndex;
            if (currExMeta < 0) return;

            CHR.ExMeta exmeta = chrFrame.exmetas[currExMeta];

            numericExID.Value = exmeta.ID;
            numericExPosX.Value = exmeta.X;
            numericExPosY.Value = exmeta.Y;

            blockSignals = false;

            Update_List(false);
            Update_Animation();
        }

        private void numericExID_ValueChanged(object sender, EventArgs e)
		{
            if (currExMeta < 0) return;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];
            CHR.ExMeta exmeta = chrFrame.exmetas[currExMeta];

            exmeta.ID = (int)numericExID.Value;

            Update_List(false);
            Update_Animation();
        }

        private void numericExPosX_ValueChanged(object sender, EventArgs e)
		{
            if (blockSignals || currExMeta < 0) return;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];
            CHR.ExMeta exmeta = chrFrame.exmetas[currExMeta];

            exmeta.X = (int)numericExPosX.Value;

            Update_List(false);
            Update_Animation();
        }

        private void numericExPosY_ValueChanged(object sender, EventArgs e)
		{
            if (blockSignals || currExMeta < 0) return;
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];
            CHR.ExMeta exmeta = chrFrame.exmetas[currExMeta];

            exmeta.Y = (int)numericExPosY.Value;

            Update_List(false);
            Update_Animation();
        }

        private void deselectToolStripMenuItemEx_Click(object sender, EventArgs e)
        {
            listBoxExMeta.ClearSelected();
        }

        private void cutToolStripMenuItemEx_Click(object sender, EventArgs e)
		{
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];
            if (currExMeta < 0) return;
            CHR.ExMeta exmeta = chrFrame.exmetas[currExMeta];

            // Copy
            clipboardExMeta = exmeta.Clone();

            // Delete
            chrFrame.exmetas.Remove(exmeta);

            Update_List(true);
            Update_Animation();
        }

        private void copyToolStripMenuItemEx_Click(object sender, EventArgs e)
		{
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];
            if (currExMeta < 0) return;
            CHR.ExMeta exmeta = chrFrame.exmetas[currExMeta];

            clipboardExMeta = exmeta.Clone();
        }

        private void pasteBeforeToolStripMenuItemEx_Click(object sender, EventArgs e)
		{
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            if (currExMeta < 0)
            {
                chrFrame.exmetas.Add(clipboardExMeta.Clone());
            }
            else
            {
                chrFrame.exmetas.Insert(currExMeta, clipboardExMeta.Clone());
            }

            Update_List(true);
            Update_Animation();
        }

        private void pasteAfterToolStripMenuItemEx_Click(object sender, EventArgs e)
		{
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            if (currExMeta < 0)
            {
                chrFrame.exmetas.Add(clipboardExMeta.Clone());
            }
            else
            {
                chrFrame.exmetas.Insert(currExMeta + 1, clipboardExMeta.Clone());
            }

            Update_List(true);
            Update_Animation();
        }

        private void newBlankToolStripMenuItemEx_Click(object sender, EventArgs e)
		{
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            if (currExMeta < 0)
            {
                chrFrame.exmetas.Add(CHR.ExMeta.Blank());
            }
            else
            {
                chrFrame.exmetas.Insert(currExMeta + 1, CHR.ExMeta.Blank());
            }

            Update_List(true);
            Update_Animation();
        }

        private void deleteToolStripMenuItemEx_Click(object sender, EventArgs e)
		{
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];
            if (currExMeta < 0) return;
            CHR.ExMeta exmeta = chrFrame.exmetas[currExMeta];

            chrFrame.exmetas.Remove(exmeta);

            Update_List(true);
            Update_Animation();
        }

        private void newSpriteBeforeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];

            dCHR.sprites.Insert(currSprite, chrSprite.Clone());

            Update_List(true);
            Update_Animation();
        }

        private void newSpriteAfterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];

            dCHR.sprites.Insert(currSprite + 1, chrSprite.Clone());

            Update_List(true);
            Update_Animation();

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

            Update_List(true);
            Update_Animation();
        }

        private void newFrameBeforeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            chrSprite.frames.Insert(currFrame, chrFrame.Clone());

            Update_List(true);
            Update_Animation();
        }

        private void newFrameAfterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CHR.Sprite chrSprite = dCHR.sprites[currSprite];
            CHR.Frame chrFrame = chrSprite.frames[currFrame];

            chrSprite.frames.Insert(currFrame + 1, chrFrame.Clone());

            Update_List(true);
            Update_Animation();

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

            Update_List(true);
            Update_Animation();
        }
	}
}
