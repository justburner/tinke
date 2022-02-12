/*
 * Copyright (C) 2022  Justburner
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
                catch (Exception ex) { MessageBox.Show("Error writing new palette:\n" + ex.Message); };
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
                pictureTileset.Image = Transform.Get4bppTextureBitmap(dIMG.texRaw, dIMG.texWidth, dIMG.texHeight, dIMG.palette[(int)numericPalette.Value]);
            else
                pictureTileset.Image = null;
        }

        private void numericPalette_ValueChanged(object sender, EventArgs e)
        {
            Update_Palette();
            Update_Texture();
        }

        private void buttonExportPal_Click(object sender, EventArgs e)
        {
            SaveFileDialog o = new SaveFileDialog();
            o.AddExtension = true;
            o.CheckPathExists = true;
            o.DefaultExt = ".pal";
            o.Filter = "Windows Palette for Gimp 2.8 (*.pal)|*.pal|" +
                       "Windows Palette (*.pal)|*.pal|" +
                       "Portable Network Graphics (*.png)|*.png|" +
                       "Adobe COlor (*.aco)|*.aco";
            o.OverwritePrompt = true;
            o.FileName = Path.ChangeExtension(dIMG.FileName, null);

            if (o.ShowDialog() != DialogResult.OK)
                return;

            Transform.PalFormat format = Transform.PalFormat.PNG;
            if (o.FilterIndex == 1)
                format = Transform.PalFormat.GimpPal;
            if (o.FilterIndex == 2)
                format = Transform.PalFormat.WinPal;
            if (o.FilterIndex == 3)
                format = Transform.PalFormat.PNG;
            if (o.FilterIndex == 4)
                format = Transform.PalFormat.ACO;

            Color[] fullPal = Transform.Get1DPalette(dIMG.palette, 16);
            Transform.ExportPalette(o.FileName, format, fullPal);
        }

        private void buttonImportPal_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.CheckFileExists = true;
            o.Filter = "All supported formats|*.pal;*.aco;*.png;*.bmp;*.jpg;*.jpeg;*.tif;*.tiff;*.gif;*.ico;*.icon|" +
                "Windows Palette (*.pal)|*.pal|" +
                "Adobe COlor (*.aco)|*.aco|" +
                "Palette from image|*.png;*.bmp;*.jpg;*.jpeg;*.tif;*.tiff;*.gif;*.ico;*.icon";
            if (o.ShowDialog() != DialogResult.OK)
                return;

            string ext = Path.GetExtension(o.FileName).ToLower();
            if (string.IsNullOrEmpty(ext) || ext.Length == 0)
            {
                MessageBox.Show("File without extension... Aborting");
                return;
            }

            if (ext.Contains("."))
                ext = ext.Substring(ext.LastIndexOf(".") + 1);
            Console.WriteLine("File extension:" + ext);

            Transform.PalFormat format = Transform.PalFormat.PNG;
            if (ext == "pal")
                format = Transform.PalFormat.WinPal;
            else if (ext == "aco")
                format = Transform.PalFormat.ACO;
            else if (ext == "png")
                format = Transform.PalFormat.PNG;

            Color[][] palette = Transform.ImportPalette(o.FileName, format);
            if (palette == null)
			{
                MessageBox.Show("Invalid palette file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            dIMG.palette = palette;
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
            o.DefaultExt = ".pal";
            o.Filter = "Portable Network Graphics (*.png)|*.png";
            o.OverwritePrompt = true;
            o.FileName = Path.ChangeExtension(dIMG.FileName, ".png");

            if (o.ShowDialog() != DialogResult.OK)
                return;

            Transform.ExportTexture(o.FileName, dIMG.texRaw, dIMG.texWidth, dIMG.texHeight, dIMG.palette[(int)numericPalette.Value]);
        }

        private void buttonImportTexture_Click(object sender, EventArgs e)
		{
            OpenFileDialog o = new OpenFileDialog();
            o.CheckFileExists = true;
            o.Filter = "All supported formats|*.png;*.bmp;*.jpg;*.jpeg;*.tif;*.tiff;*.gif;*.ico;*.icon";

            if (o.ShowDialog() != DialogResult.OK)
                return;

            int width, height;
            byte[] texture = Transform.ImportTexture(o.FileName, out width, out height, dIMG.palette[(int)numericPalette.Value]);
            dIMG.texRaw = texture;
            dIMG.texWidth = width;
            dIMG.texHeight = height;

            // Write file
            Write_File();

            Update_Texture();
        }
    }
}
