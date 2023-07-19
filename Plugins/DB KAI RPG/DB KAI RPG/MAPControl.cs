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
    public partial class MAPControl : UserControl
    {
        IPluginHost pluginHost;
        MAP dMAP;
        Color[] maskPalette = new Color[256];

        public MAPControl()
        {
            InitializeComponent();
        }

        public MAPControl(Ekona.IPluginHost pluginHost, MAP dMAP)
        {
            InitializeComponent();

            this.pluginHost = pluginHost;
            this.dMAP = dMAP;

            // Mask palette
            for (int y = 0; y < 32; y++)
                for (int x = 0; x < 8; x++)
				{
                    int index = y * 8 + x;
                    if (y > 0)
                        maskPalette[index] = Color.FromArgb(x * 32, 248 - y * 8, y * 8);
                    else
                        maskPalette[index] = Color.FromArgb(x * 32, 0, 0);
                }

            Update_Palette();
            Update_Textures();
            Update_ExData();
        }

        private void Write_File()
        {
            if (dMAP.ID > 0)
            {
                try
                {
                    String fileOut = pluginHost.Get_TempFile();
                    dMAP.Write(fileOut);
                    pluginHost.ChangeFile(dMAP.ID, fileOut);
                }
                catch (Exception ex) { MessageBox.Show("Error writing new palette:\n" + ex.Message); };
            }
        }

        public void Update_Palette()
        {
            if (dMAP.palette.Length != 0)
                picturePalette.Image = Transform.Get1DPaletteBitmap(picturePalette.Width, picturePalette.Height, dMAP.palette, 16, 16);
            else
                picturePalette.Image = null;
        }

        public void Update_Textures()
        {
            if (dMAP.palette.Length != 0)
            {
                pictureImg.Image = Transform.Get8bppTextureBitmap(dMAP.texImg, dMAP.texWidth, dMAP.texHeight, dMAP.palette);
                pictureMsk.Image = Transform.Get8bppTextureBitmap(dMAP.texMsk, dMAP.texWidth, dMAP.texHeight, maskPalette);
            }
            else
            {
                pictureImg.Image = null;
                pictureMsk.Image = null;
            }
        }

        public void Update_ExData()
        {
            labelExData.Text = string.Format("Extra data: {0} bytes", dMAP.dataEx.Length);
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
            o.FileName = Path.ChangeExtension(dMAP.FileName, null);

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

            Transform.ExportPalette(o.FileName, format, dMAP.palette);
        }

        private void buttonExportMaskPal_Click(object sender, EventArgs e)
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
            o.FileName = Path.Combine(Path.GetDirectoryName(dMAP.FileName), "MaskPalette");

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

            Transform.ExportPalette(o.FileName, format, maskPalette);
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
            dMAP.palette = Transform.Get1DPalette(palette, 16);

            // Write file
            Write_File();

            Update_Palette();
            Update_Textures();
        }

        private void buttonExportImage_Click(object sender, EventArgs e)
        {
            SaveFileDialog o = new SaveFileDialog();
            o.AddExtension = true;
            o.CheckPathExists = true;
            o.DefaultExt = ".pal";
            o.Filter = "Portable Network Graphics (*.png)|*.png";
            o.OverwritePrompt = true;
            o.FileName = Path.ChangeExtension(Path.ChangeExtension(dMAP.FileName, null) + "_image", ".png");

            if (o.ShowDialog() != DialogResult.OK)
                return;

            Transform.Export8bppTexture(o.FileName, dMAP.texImg, dMAP.texWidth, dMAP.texHeight, dMAP.palette);
        }

        private void buttonExportMask_Click(object sender, EventArgs e)
        {
            SaveFileDialog o = new SaveFileDialog();
            o.AddExtension = true;
            o.CheckPathExists = true;
            o.DefaultExt = ".pal";
            o.Filter = "Portable Network Graphics (*.png)|*.png";
            o.OverwritePrompt = true;
            o.FileName = Path.ChangeExtension(Path.ChangeExtension(dMAP.FileName, null) + "_mask", ".png");

            if (o.ShowDialog() != DialogResult.OK)
                return;

            Transform.Export8bppTexture(o.FileName, dMAP.texMsk, dMAP.texWidth, dMAP.texHeight, maskPalette);
        }

        private void buttonImportBoth_Click(object sender, EventArgs e)
        {
            OpenFileDialog oI = new OpenFileDialog();
            oI.CheckFileExists = true;
            oI.Filter = "All supported formats|*.png;*.bmp;*.jpg;*.jpeg;*.tif;*.tiff;*.gif;*.ico;*.icon";

            if (oI.ShowDialog() != DialogResult.OK)
                return;

            OpenFileDialog oM = new OpenFileDialog();
            oM.CheckFileExists = true;
            oM.Filter = "All supported formats|*.png;*.bmp;*.jpg;*.jpeg;*.tif;*.tiff;*.gif;*.ico;*.icon";

            if (oM.ShowDialog() != DialogResult.OK)
                return;

            int widthI, heightI;
            byte[] texImg = Transform.Import8bppTexture(oI.FileName, out widthI, out heightI, dMAP.palette);
            if ((widthI & 7) != 0 || (heightI & 7) != 0)
            {
                MessageBox.Show("Invalid image size: must be multiple of 8.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((widthI > 2040) || (heightI > 2040))
            {
                MessageBox.Show("Invalid image size: too large.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int widthM, heightM;
            byte[] texMsk = Transform.Import8bppTexture(oM.FileName, out widthM, out heightM, maskPalette);
            if ((widthM & 7) != 0 || (heightM & 7) != 0)
            {
                MessageBox.Show("Invalid mask size: must be multiple of 8.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((widthM > 2040) || (heightM > 2040))
            {
                MessageBox.Show("Invalid mask size: too large.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (widthI != widthM || heightI != heightM)
            {
                MessageBox.Show("Invalid image size: must match mask resolution.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dMAP.texImg = texImg;
            dMAP.texMsk = texMsk;
            dMAP.texWidth = widthI;
            dMAP.texHeight = heightI;
            dMAP.RecompressImg();
            dMAP.RecompressMsk();

            // Write file
            Write_File();

            Update_Textures();
        }

        private void buttonImportImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.CheckFileExists = true;
            o.Filter = "All supported formats|*.png;*.bmp;*.jpg;*.jpeg;*.tif;*.tiff;*.gif;*.ico;*.icon";

            if (o.ShowDialog() != DialogResult.OK)
                return;

            int width, height;
            byte[] texture = Transform.Import8bppTexture(o.FileName, out width, out height, dMAP.palette);
            if (width != dMAP.texWidth || height != dMAP.texHeight)
            {
                MessageBox.Show("Invalid image size: must match mask resolution.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            dMAP.texImg = texture;
            dMAP.RecompressImg();

            // Write file
            Write_File();

            Update_Textures();
        }

        private void buttonImportMask_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.CheckFileExists = true;
            o.Filter = "All supported formats|*.png;*.bmp;*.jpg;*.jpeg;*.tif;*.tiff;*.gif;*.ico;*.icon";

            if (o.ShowDialog() != DialogResult.OK)
                return;

            int width, height;
            byte[] texture = Transform.Import8bppTexture(o.FileName, out width, out height, maskPalette);
            if (width != dMAP.texWidth || height != dMAP.texHeight)
            {
                MessageBox.Show("Invalid mask size: must match image resolution.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            dMAP.texMsk = texture;
            dMAP.RecompressMsk();

            // Write file
            Write_File();

            Update_Textures();
        }

        private void buttonExportExData_Click(object sender, EventArgs e)
        {
            SaveFileDialog o = new SaveFileDialog();
            o.AddExtension = true;
            o.CheckPathExists = true;
            o.DefaultExt = ".mapdat";
            o.Filter = "Map extra data (*.mapdat)|*.mapdat";
            o.OverwritePrompt = true;
            o.FileName = Path.ChangeExtension(dMAP.FileName, ".mapdat");

            if (o.ShowDialog() != DialogResult.OK)
                return;

            // Write to file
            FileStream fs = File.Open(o.FileName, FileMode.Create);
            fs.Write(dMAP.dataEx, 0, dMAP.dataEx.Length);
            fs.Close();
        }

        private void buttonImportExData_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.CheckFileExists = true;
            o.Filter = "Map extra data (*.mapdat)|*.mapdat";

            if (o.ShowDialog() != DialogResult.OK)
                return;

            // Read from file
            dMAP.dataEx = File.ReadAllBytes(o.FileName);

            // Write file
            Write_File();

            Update_ExData();
        }
    }
}
