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
    public partial class IMGControl : UserControl
    {
        IPluginHost pluginHost;
        IMG dIMG;

        public IMGControl()
        {
            InitializeComponent();
        }

        public IMGControl(Ekona.IPluginHost pluginHost, IMG dIMG)
        {
            InitializeComponent();

            this.pluginHost = pluginHost;
            this.dIMG = dIMG;

            Update_Palette();
            Update_Texture();
        }

        private void Write_File()
        {
            if (dIMG.ID > 0)
            {
                try
                {
                    String fileOut = pluginHost.Get_TempFile();
                    dIMG.Write(fileOut);
                    pluginHost.ChangeFile(dIMG.ID, fileOut);
                }
                catch (Exception ex) { MessageBox.Show("Error writing new dIMG:\n" + ex.Message); };
            }
        }

        public void Update_Palette()
        {
            numericPalette.Maximum = dIMG.palette.Length - 1;
            labelNumPalettes.Text = string.Format("of {0}", dIMG.palette.Length - 1);
            if (numericPalette.Value < dIMG.palette.Length)
                picturePalette.Image = Transform.Get1DPaletteBitmap(picturePalette.Width, picturePalette.Height, dIMG.palette[(int)numericPalette.Value], 16);
            else
                picturePalette.Image = null;
        }

        public void Update_Texture()
        {
            if (numericPalette.Value < dIMG.palette.Length)
			{
                pictureTileset.Image = Transform.Get4bppTextureBitmap(dIMG.texRaw, dIMG.texWidth, dIMG.texHeight, dIMG.palette[(int)numericPalette.Value]);
                int slots = (dIMG.palette.Length > 1) ? 16 : 1;
                labelData.Text = string.Format("Pal Slots: {0}\nTex. Res.: {1} x {2}\nTex. Size: {3} Bytes", slots, dIMG.texWidth, dIMG.texHeight, dIMG.texRaw.Length);
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
            string fileName = Transform.ExportPaletteDialog(dIMG.FileName, out format);
            if (String.IsNullOrEmpty(fileName)) return;

            Color[] fullPal = Transform.Get1DPalette(dIMG.palette, 16);
            Transform.ExportPalette(fileName, format, fullPal);
        }

        private void buttonImportPal_Click(object sender, EventArgs e)
        {
            Transform.PalFormat format;
            string fileName = Transform.ImportPaletteDialog(dIMG.FileName, out format);
            if (String.IsNullOrEmpty(fileName)) return;

            Color[] palette = Transform.ImportPalette(fileName, format);
            if (palette == null)
			{
                MessageBox.Show("Invalid palette file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (palette.Length > 16)
                palette = Transform.Resize1DPalette(palette, 256);
            else
                palette = Transform.Resize1DPalette(palette, 16);
            dIMG.palette = Transform.Get2DPalette(palette, 16);
            numericPalette.Value = 0;

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
            o.FileName = Path.ChangeExtension(dIMG.FileName, ".png");

            if (o.ShowDialog() != DialogResult.OK)
                return;

            Transform.Export4bppTexturePNG(o.FileName, dIMG.texRaw, dIMG.texWidth, dIMG.texHeight, dIMG.palette[(int)numericPalette.Value]);
        }

        private void buttonImportTexture_Click(object sender, EventArgs e)
		{
            OpenFileDialog o = new OpenFileDialog();
            o.CheckFileExists = true;
            o.Filter = "All supported formats|*.png;*.bmp;*.jpg;*.jpeg;*.tif;*.tiff;*.gif;*.ico;*.icon";

            if (o.ShowDialog() != DialogResult.OK)
                return;

            int width, height;
            byte[] texture = Transform.Import4bppTextureFromImage(o.FileName, out width, out height, dIMG.palette[(int)numericPalette.Value]);
            if (texture == null)
            {
                MessageBox.Show("Invalid image file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((width & 3) != 0)
            {
                MessageBox.Show("Invalid texture size: width must be multiple of 4.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((height & 1) != 0)
            {
                MessageBox.Show("Invalid texture size: height must be multiple of 2.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((width > 510) || (height > 510))
            {
                MessageBox.Show("Invalid texture size: too large.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            dIMG.texRaw = texture;
            dIMG.texWidth = width;
            dIMG.texHeight = height;

            // Write file
            Write_File();

            Update_Texture();
        }
    }
}
