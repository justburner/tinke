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
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ekona;
using Ekona.Images;

namespace DB_KAI_RPG
{
    public partial class BSTControl : UserControl
    {
        IPluginHost pluginHost;
        BST dBST;
        bool blockSignals;

        public BSTControl()
        {
            InitializeComponent();
        }

        public BSTControl(Ekona.IPluginHost pluginHost, BST dBST)
        {
            InitializeComponent();

            this.pluginHost = pluginHost;
            this.dBST = dBST;

            Update_List(0, true);
            if (listBoxLayers.Items.Count != 0) listBoxLayers.SetSelected(0, true);
            Update_Palette();
            Update_Preview();
        }

        private void Write_File()
        {
            if (dBST.ID > 0)
            {
                try
                {
                    String fileOut = pluginHost.Get_TempFile();
                    dBST.Write(fileOut);
                    pluginHost.ChangeFile(dBST.ID, fileOut);
                }
                catch (Exception ex) { MessageBox.Show("Error writing new dBST:\n" + ex.Message); };
            }
        }

        public void Update_List(int editIndex, bool relistLayers)
		{
            blockSignals = true;

            int count = dBST.Count;

            List<int> visibleIndices = new List<int>();
            for (int i = 0; i < listBoxLayers.Items.Count; i++)
            {
                if (listBoxLayers.GetSelected(i)) visibleIndices.Add(i);
            }

            if (relistLayers)
            {
                listBoxLayers.Items.Clear();
                comboBoxLayers.Items.Clear();
            }

            for (int i = 0; i < count; i++)
            {
                BST.Layer layer = dBST.GetLayer(i);
                string layerInfo = string.Format("{0:X04} : X: {1}, Y: {2}, Size: {3} x {4}", layer.code, layer.posx, layer.posy, layer.width, layer.height);
                if (relistLayers)
                {
                    listBoxLayers.Items.Add(layerInfo);
                    comboBoxLayers.Items.Add(layerInfo);
                }
                else
				{
                    listBoxLayers.Items[i] = layerInfo;
                    comboBoxLayers.Items[i] = layerInfo;
                }
            }

            for (int i = 0; i < visibleIndices.Count; i++)
            {
                listBoxLayers.SetSelected(visibleIndices[i], true);
            }
            if (relistLayers)
            {
                if (editIndex >= 0 && editIndex < comboBoxLayers.Items.Count)
                    comboBoxLayers.SelectedIndex = editIndex;
            }

            blockSignals = false;
        }

        public void Update_Palette()
        {
            int index = comboBoxLayers.SelectedIndex;
            Color[] palette = dBST.GetPalette(index);

            if (palette != null)
                picturePalette.Image = Transform.Get1DPaletteBitmap(picturePalette.Width, picturePalette.Height, palette, 16, 16);
            else
                picturePalette.Image = null;
        }

        public Bitmap CreatePreview(int width, int height, bool border, bool highlight)
		{
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);

            ColorMatrix halfAlphaCM = new ColorMatrix();
            ImageAttributes halfAlphaIA = new ImageAttributes();
            halfAlphaCM.Matrix33 = 0.5f;
            halfAlphaIA.SetColorMatrix(halfAlphaCM, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int centerX = 75;
            int centerY = (height - 128) / 2;
            if (border) g.DrawRectangle(Pens.Black, new Rectangle(-1, centerY - 1, 257, 129));

            if (highlight)
            {
                int highlightId = comboBoxLayers.SelectedIndex;
                for (int i = 0; i < listBoxLayers.Items.Count; i++)
                {
                    if (i == highlightId) continue;
                    if (listBoxLayers.GetSelected(i))
                    {
                        BST.Layer layer = dBST.GetLayer(i);
                        Bitmap layerBmp = Transform.Get8bppTextureBitmap(layer.pixels, layer.width, layer.height, layer.palette);
                        Rectangle dstRect = new Rectangle(layer.posx + centerX, layer.posy + centerY, layer.width, layer.height);
                        g.DrawImage(layerBmp, dstRect, 0, 0, layer.width, layer.height, GraphicsUnit.Pixel, halfAlphaIA);
                        layerBmp.Dispose();
                    }
                }
				{
                    BST.Layer layer = dBST.GetLayer(highlightId);
                    if (layer != null)
					{
                        Bitmap layerBmp = Transform.Get8bppTextureBitmap(layer.pixels, layer.width, layer.height, layer.palette);
                        g.DrawImage(layerBmp, layer.posx + centerX, layer.posy + centerY);
                        layerBmp.Dispose();
                    }
                }
            }
            else
			{
                for (int i = 0; i < listBoxLayers.Items.Count; i++)
                {
                    if (listBoxLayers.GetSelected(i))
                    {
                        BST.Layer layer = dBST.GetLayer(i);
                        Bitmap layerBmp = Transform.Get8bppTextureBitmap(layer.pixels, layer.width, layer.height, layer.palette);
                        g.DrawImage(layerBmp, layer.posx + centerX, layer.posy + centerY);
                        layerBmp.Dispose();
                    }
                }
            }

            g.Dispose();

            return bmp;
        }

        public void Update_Preview()
        {
            picturePreview.Image = CreatePreview(picturePreview.Width, picturePreview.Height, true, checkBoxHighlight.Checked);
            labelData.Text = "No Info";

            int index = comboBoxLayers.SelectedIndex;
            BST.Layer layer = dBST.GetLayer(index);

            if (layer != null)
            {
                blockSignals = true;

                numericPosX.Value = layer.posx;
                numericPosY.Value = layer.posy;
                labelData.Text = string.Format("Tex. Res.: {0} x {1}\nTex. Size: {2} Bytes\nNum. Colors: {3}", layer.width, layer.height, layer.pixels.Length, layer.palette.Length);

                blockSignals = false;
            }
        }

        private void listBoxLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (blockSignals) return;

            Update_Palette();
            Update_Preview();
        }

        private void buttonExportPal_Click(object sender, EventArgs e)
        {
            int index = comboBoxLayers.SelectedIndex;
            BST.Layer layer = dBST.GetLayer(index);
            if (layer == null) return;

            Transform.PalFormat format;
            string fileName = Transform.ExportPaletteDialog(dBST.FileName, out format);
            if (String.IsNullOrEmpty(fileName)) return;

            Transform.ExportPalette(fileName, format, layer.palette);
        }

        private void buttonImportPal_Click(object sender, EventArgs e)
        {
            int index = comboBoxLayers.SelectedIndex;
            BST.Layer layer = dBST.GetLayer(index);
            if (layer == null) return;

            Transform.PalFormat format;
            string fileName = Transform.ImportPaletteDialog(dBST.FileName, out format);
            if (String.IsNullOrEmpty(fileName)) return;

            Color[] palette = Transform.ImportPalette(fileName, format);
            if (palette == null)
			{
                MessageBox.Show("Invalid palette file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            layer.palette = Transform.Resize1DPalette(palette, 256);
            layer.palette[0] = Color.Transparent;

            // Write file
            Write_File();

            Update_List(comboBoxLayers.SelectedIndex, false);
            Update_Palette();
            Update_Preview();
        }

		private void buttonExportTexture_Click(object sender, EventArgs e)
		{
            int index = comboBoxLayers.SelectedIndex;
            BST.Layer layer = dBST.GetLayer(index);
            if (layer == null) return;

            SaveFileDialog o = new SaveFileDialog();
            o.AddExtension = true;
            o.CheckPathExists = true;
            o.DefaultExt = ".png";
            o.Filter = "Portable Network Graphics (*.png)|*.png";
            o.OverwritePrompt = true;
            o.FileName = Path.ChangeExtension(dBST.FileName, ".png");

            if (o.ShowDialog() != DialogResult.OK)
                return;

            Transform.Export8bppTexturePNG(o.FileName, layer.pixels, layer.width, layer.height, layer.palette);
        }

        private void buttonImportTexture_Click(object sender, EventArgs e)
		{
            int index = comboBoxLayers.SelectedIndex;
            BST.Layer layer = dBST.GetLayer(index);
            if (layer == null) return;

            OpenFileDialog o = new OpenFileDialog();
            o.CheckFileExists = true;
            o.Filter = "All supported formats|*.png;*.bmp;*.jpg;*.jpeg;*.tif;*.tiff;*.gif;*.ico;*.icon";

            if (o.ShowDialog() != DialogResult.OK)
                return;

            int width, height;
            byte[] texture = Transform.Import8bppTextureFromImage(o.FileName, out width, out height, layer.palette);
            if (texture == null)
            {
                MessageBox.Show("Invalid image file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (width >= 256 || height >= 256)
			{
                MessageBox.Show("Invalid avatar size: too large.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            layer.pixels = texture;
            layer.width = width;
            layer.height = height;

            // Write file
            Write_File();

            Update_List(comboBoxLayers.SelectedIndex, false);
            Update_Palette();
            Update_Preview();
        }

        private void createNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BSTLayerCode dialog = new BSTLayerCode();
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            ushort code = dialog.GetCode();
            dialog.Dispose();

            int index = dBST.NewLayer(code);
            if (index == -1)
			{
                MessageBox.Show("Layer code already exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Write file
            Write_File();

            Update_List(index, true);
            Update_Palette();
            Update_Preview();
        }

        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dBST.DeleteLayer(comboBoxLayers.SelectedIndex))
			{
                // So selection fallthrough
                listBoxLayers.Items.RemoveAt(comboBoxLayers.SelectedIndex);

                // Write file
                Write_File();

                Update_List(comboBoxLayers.SelectedIndex, true);
                Update_Palette();
                Update_Preview();
            }
        }

        private void buttonRename_Click(object sender, EventArgs e)
        {
            int index = comboBoxLayers.SelectedIndex;
            BST.Layer layer = dBST.GetLayer(index);
            if (layer == null) return;

            BSTLayerCode dialog = new BSTLayerCode();
            dialog.SetCode(layer.code);
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            ushort code = dialog.GetCode();
            dialog.Dispose();

            if (!dBST.RenameLayer(index, code))
            {
                MessageBox.Show("Layer code already exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Update_List(comboBoxLayers.SelectedIndex, false);
            Update_Preview();
        }

        private void numericPosX_ValueChanged(object sender, EventArgs e)
		{
            if (blockSignals) return;

            int index = comboBoxLayers.SelectedIndex;
            BST.Layer layer = dBST.GetLayer(index);
            if (layer == null) return;

            layer.posx = (sbyte)numericPosX.Value;

            Update_List(comboBoxLayers.SelectedIndex, false);
            Update_Preview();

            // Write file
            Write_File();
        }

        private void numericPosY_ValueChanged(object sender, EventArgs e)
		{
            if (blockSignals) return;

            int index = comboBoxLayers.SelectedIndex;
            BST.Layer layer = dBST.GetLayer(index);
            if (layer == null) return;

            layer.posy = (sbyte)numericPosY.Value;

            Update_List(comboBoxLayers.SelectedIndex, false);
            Update_Preview();

            // Write file
            Write_File();
        }

        private void buttonSavePreview_Click(object sender, EventArgs e)
		{
            SaveFileDialog o = new SaveFileDialog();
            o.AddExtension = true;
            o.CheckPathExists = true;
            o.DefaultExt = ".png";
            o.Filter = "Portable Network Graphics (*.png)|*.png";
            o.OverwritePrompt = true;
            o.FileName = Path.ChangeExtension(dBST.FileName, ".png");

            if (o.ShowDialog() != DialogResult.OK)
                return;

            Bitmap bmp = CreatePreview(256, 128, false, false);
            bmp.Save(o.FileName);
            bmp.Dispose();
        }

        private void comboBoxLayers_SelectedIndexChanged(object sender, EventArgs e)
		{
            if (blockSignals) return;

            Update_Palette();
            Update_Preview();
        }

		private void checkBoxHighlight_CheckedChanged(object sender, EventArgs e)
		{
            if (blockSignals) return;

            Update_Preview();
        }
	}
}
