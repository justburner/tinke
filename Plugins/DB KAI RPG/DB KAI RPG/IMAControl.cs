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
    public partial class IMAControl : UserControl
    {
        IPluginHost pluginHost;
        IMA dIMA;
        bool listRefreshBlock;

        public IMAControl()
        {
            InitializeComponent();
        }

        public IMAControl(Ekona.IPluginHost pluginHost, IMA dIMA)
        {
            InitializeComponent();

            this.pluginHost = pluginHost;
            this.dIMA = dIMA;
            this.listRefreshBlock = false;

            Update_List(0);
            Update_Palette();
            Update_Texture();
        }

        private void Write_File()
        {
            if (dIMA.ID > 0)
            {
                try
                {
                    String fileOut = pluginHost.Get_TempFile();
                    dIMA.Write(fileOut);
                    pluginHost.ChangeFile(dIMA.ID, fileOut);
                }
                catch (Exception ex) { MessageBox.Show("Error writing new dIMA:\n" + ex.Message); };
            }
        }

        public void Update_List(int index)
		{
            listRefreshBlock = true;

            listBoxImps.Items.Clear();

            int count = dIMA.imps.Count;
            for (int i = 0; i < count; i++)
            {
                IMP imp = dIMA.imps[i];
                listBoxImps.Items.Add(string.Format("{0}: {1} x {2}", i, imp.texWidth, imp.texHeight));
            }

            if (index >= 0 && index < listBoxImps.Items.Count)
                listBoxImps.SelectedIndex = index;

            listRefreshBlock = false;
        }

        public void Update_Palette()
        {
            int index = listBoxImps.SelectedIndex;
            Color[] palette = dIMA.GetPalette(index);

            if (palette != null)
                picturePalette.Image = Transform.Get1DPaletteBitmap(picturePalette.Width, picturePalette.Height, palette, 16, 16);
            else
                picturePalette.Image = null;
        }

        public void Update_Texture()
        {
            int index = listBoxImps.SelectedIndex;
            IMP imp = dIMA.GetIMP(index);

            if (imp != null)
            {
                pictureTileset.Image = Transform.Get8bppTextureBitmap(imp.texRaw, imp.texWidth, imp.texHeight, imp.palette);
                double ratio = (double)imp.texRaw.Length / (double)imp.data.Length;
                labelData.Text = string.Format("Tex. Res.: {0} x {1}\nTex. Size: {2} Bytes\nData Size: {3} Bytes\nCompression Ratio: {4:0.##}", imp.texWidth, imp.texHeight, imp.texRaw.Length, imp.data.Length, ratio);
            }
            else
            {
                pictureTileset.Image = null;
                labelData.Text = "No Info";
            }
        }

        private void listBoxImps_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listRefreshBlock) return;
            Update_Palette();
            Update_Texture();
        }

        private void buttonExportPal_Click(object sender, EventArgs e)
        {
            int index = listBoxImps.SelectedIndex;
            IMP imp = dIMA.GetIMP(index);
            if (imp == null) return;

            Transform.PalFormat format;
            string fileName = Transform.ExportPaletteDialog(dIMA.FileName, out format);
            if (String.IsNullOrEmpty(fileName)) return;

            Transform.ExportPalette(fileName, format, imp.palette);
        }

        private void buttonImportPal_Click(object sender, EventArgs e)
        {
            int index = listBoxImps.SelectedIndex;
            IMP imp = dIMA.GetIMP(index);
            if (imp == null) return;

            Transform.PalFormat format;
            string fileName = Transform.ImportPaletteDialog(dIMA.FileName, out format);
            if (String.IsNullOrEmpty(fileName)) return;

            Color[] palette = Transform.ImportPalette(fileName, format);
            if (palette == null)
			{
                MessageBox.Show("Invalid palette file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            imp.palette = Transform.Resize1DPalette(palette, 256);

            // Write file
            Write_File();

            Update_List(listBoxImps.SelectedIndex);
            Update_Palette();
            Update_Texture();
        }

		private void buttonExportTexture_Click(object sender, EventArgs e)
		{
            int index = listBoxImps.SelectedIndex;
            IMP imp = dIMA.GetIMP(index);
            if (imp == null) return;

            SaveFileDialog o = new SaveFileDialog();
            o.AddExtension = true;
            o.CheckPathExists = true;
            o.DefaultExt = ".png";
            o.Filter = "Portable Network Graphics (*.png)|*.png";
            o.OverwritePrompt = true;
            o.FileName = Path.ChangeExtension(dIMA.FileName, ".png");

            if (o.ShowDialog() != DialogResult.OK)
                return;

            Transform.Export8bppTexturePNG(o.FileName, imp.texRaw, imp.texWidth, imp.texHeight, imp.palette);
        }

        private void buttonImportTexture_Click(object sender, EventArgs e)
		{
            int index = listBoxImps.SelectedIndex;
            IMP imp = dIMA.GetIMP(index);
            if (imp == null) return;

            OpenFileDialog o = new OpenFileDialog();
            o.CheckFileExists = true;
            o.Filter = "All supported formats|*.png;*.bmp;*.jpg;*.jpeg;*.tif;*.tiff;*.gif;*.ico;*.icon";

            if (o.ShowDialog() != DialogResult.OK)
                return;

            int width, height;
            byte[] texture = Transform.Import8bppTextureFromImage(o.FileName, out width, out height, imp.palette);
            if (texture == null)
            {
                MessageBox.Show("Invalid image file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (width >= 510 || height >= 510)
			{
                if ((width & 7) != 0 || (height & 7) != 0)
                {
                    MessageBox.Show("Invalid large texture size: must be multiple of 8.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if ((width > 2040) || (height > 2040))
				{
                    MessageBox.Show("Invalid large texture size: too large.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
			{
                if ((width & 1) != 0 || (height & 1) != 0)
                {
                    MessageBox.Show("Invalid texture size: must be multiple of 2.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if ((width > 510) || (height > 510))
                {
                    MessageBox.Show("Invalid texture size: too large.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            imp.texRaw = texture;
            imp.texWidth = width;
            imp.texHeight = height;
            imp.Recompress();

            // Write file
            Write_File();

            Update_List(listBoxImps.SelectedIndex);
            Update_Palette();
            Update_Texture();
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = listBoxImps.SelectedIndex;
            if (dIMA.SwapIMPs(index, index - 1))
			{
                listBoxImps.SelectedIndex--;

                // Write file
                Write_File();

                Update_List(listBoxImps.SelectedIndex);
                Update_Palette();
                Update_Texture();
            }
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = listBoxImps.SelectedIndex;
            if (dIMA.SwapIMPs(index, index + 1))
			{
                listBoxImps.SelectedIndex++;

                // Write file
                Write_File();

                Update_List(listBoxImps.SelectedIndex);
                Update_Palette();
                Update_Texture();
            }
        }

        private void createNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = dIMA.NewIMP();

            // Write file
            Write_File();

            Update_List(index);
            Update_Palette();
            Update_Texture();
        }

        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dIMA.DeleteIMP(listBoxImps.SelectedIndex))
			{
                // Write file
                Write_File();

                Update_List(listBoxImps.SelectedIndex);
                Update_Palette();
                Update_Texture();
            }
        }
    }
}
