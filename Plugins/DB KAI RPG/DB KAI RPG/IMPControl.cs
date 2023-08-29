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
    public partial class IMPControl : UserControl
    {
        IPluginHost pluginHost;
        IMP dIMP;

        public IMPControl()
        {
            InitializeComponent();
        }

        public IMPControl(Ekona.IPluginHost pluginHost, IMP dIMP)
        {
            InitializeComponent();

            this.pluginHost = pluginHost;
            this.dIMP = dIMP;

            Update_Palette();
            Update_Texture();
        }

        private void Write_File()
        {
            if (dIMP.ID > 0)
            {
                try
                {
                    String fileOut = pluginHost.Get_TempFile();
                    dIMP.Write(fileOut);
                    pluginHost.ChangeFile(dIMP.ID, fileOut);
                }
                catch (Exception ex) { MessageBox.Show("Error writing new dIMP:\n" + ex.Message); };
            }
        }

        public void Update_Palette()
        {
            if (dIMP.palette.Length != 0)
                picturePalette.Image = Transform.Get1DPaletteBitmap(picturePalette.Width, picturePalette.Height, dIMP.palette, 16, 16);
            else
                picturePalette.Image = null;
        }

        public void Update_Texture()
        {
            if (dIMP.palette.Length != 0) {
                pictureTileset.Image = Transform.Get8bppTextureBitmap(dIMP.texRaw, dIMP.texWidth, dIMP.texHeight, dIMP.palette);
                double ratio = (double)dIMP.texRaw.Length / (double)dIMP.data.Length;
                labelData.Text = string.Format("Tex. Res.: {0} x {1}\nTex. Size: {2} Bytes\nData Size: {3} Bytes\nCompression Ratio: {4:0.##}", dIMP.texWidth, dIMP.texHeight, dIMP.texRaw.Length, dIMP.data.Length, ratio);
            }
            else
            {
                pictureTileset.Image = null;
                labelData.Text = "No Info";
            }
        }

        private void numericPalette_ValueChanged(object sender, EventArgs e)
        {
            Update_Palette();
            Update_Texture();
        }

        private void buttonExportPal_Click(object sender, EventArgs e)
        {
            Transform.PalFormat format;
            string fileName = Transform.ExportPaletteDialog(dIMP.FileName, out format);
            if (String.IsNullOrEmpty(fileName)) return;

            Transform.ExportPalette(fileName, format, dIMP.palette);
        }

        private void buttonImportPal_Click(object sender, EventArgs e)
        {
            Transform.PalFormat format;
            string fileName = Transform.ImportPaletteDialog(dIMP.FileName, out format);
            if (String.IsNullOrEmpty(fileName)) return;

            Color[] palette = Transform.ImportPalette(fileName, format);
            if (palette == null)
			{
                MessageBox.Show("Invalid palette file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            dIMP.palette = Transform.Resize1DPalette(palette, 256);

            // Write file
            Write_File();

            Update_Palette();
            Update_Texture();
        }

		private void buttonExportTexture_Click(object sender, EventArgs e)
		{
            SaveFileDialog o = new SaveFileDialog();
            o.AddExtension = true;
            o.CheckPathExists = true;
            o.DefaultExt = ".png";
            o.Filter = "Portable Network Graphics (*.png)|*.png";
            o.OverwritePrompt = true;
            o.FileName = Path.ChangeExtension(dIMP.FileName, ".png");

            if (o.ShowDialog() != DialogResult.OK)
                return;

            Transform.Export8bppTexturePNG(o.FileName, dIMP.texRaw, dIMP.texWidth, dIMP.texHeight, dIMP.palette);
        }

        private void buttonImportTexture_Click(object sender, EventArgs e)
		{
            OpenFileDialog o = new OpenFileDialog();
            o.CheckFileExists = true;
            o.Filter = "All supported formats|*.png;*.bmp;*.jpg;*.jpeg;*.tif;*.tiff;*.gif;*.ico;*.icon";

            if (o.ShowDialog() != DialogResult.OK)
                return;

            int width, height;
            byte[] texture = Transform.Import8bppTextureFromImage(o.FileName, out width, out height, dIMP.palette);
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
            dIMP.texRaw = texture;
            dIMP.texWidth = width;
            dIMP.texHeight = height;
            dIMP.Recompress();

            // Write file
            Write_File();

            Update_Texture();
        }
    }
}
