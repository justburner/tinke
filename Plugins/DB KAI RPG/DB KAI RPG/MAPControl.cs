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
                catch (Exception ex) { MessageBox.Show("Error writing new dMAP:\n" + ex.Message); };
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
                double ratioImg = (double)dMAP.texImg.Length / (double)dMAP.dataImg.Length;
                double ratioMsk = (double)dMAP.texMsk.Length / (double)dMAP.dataMsk.Length;
                labelData.Text = string.Format(
                    "Image Compression: {0} Bytes; {1:0.##} Ratio\n" +
                    "Mask Compression: {2} Bytes; {3:0.##} Ratio\n" +
                    "Each Texture: {4} x {5} :: {6} Bytes",
                    dMAP.dataImg.Length, ratioImg,
                    dMAP.dataMsk.Length, ratioMsk,
                    dMAP.texWidth, dMAP.texHeight, dMAP.texImg.Length);
            }
            else
            {
                pictureImg.Image = null;
                pictureMsk.Image = null;
                labelData.Text = "No Info";
            }
        }

        public void Update_ExData()
        {
            labelExData.Text = string.Format("Extra data: {0} bytes", dMAP.dataEx.Length);
        }

        private void buttonExportPal_Click(object sender, EventArgs e)
        {
            Transform.PalFormat format;
            string fileName = Transform.ExportPaletteDialog(dMAP.FileName, out format);
            if (String.IsNullOrEmpty(fileName)) return;

            Transform.ExportPalette(fileName, format, dMAP.palette);
        }

        private void buttonExportMaskPal_Click(object sender, EventArgs e)
        {
            Transform.PalFormat format;
            string fileName = Transform.ExportPaletteDialog(Path.Combine(Path.GetDirectoryName(dMAP.FileName), "MaskPalette"), out format);
            if (String.IsNullOrEmpty(fileName)) return;

            Transform.ExportPalette(fileName, format, maskPalette);
        }

        private void buttonImportPal_Click(object sender, EventArgs e)
        {
            Transform.PalFormat format;
            string fileName = Transform.ImportPaletteDialog(dMAP.FileName, out format);
            if (String.IsNullOrEmpty(fileName)) return;

            Color[] palette = Transform.ImportPalette(fileName, format);
            if (palette == null)
            {
                MessageBox.Show("Invalid palette file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            dMAP.palette = Transform.Resize1DPalette(palette, 256);

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
            o.DefaultExt = ".png";
            o.Filter = "Portable Network Graphics (*.png)|*.png";
            o.OverwritePrompt = true;
            o.FileName = Path.ChangeExtension(Path.ChangeExtension(dMAP.FileName, null) + "_image", ".png");

            if (o.ShowDialog() != DialogResult.OK)
                return;

            Transform.Export8bppTexturePNG(o.FileName, dMAP.texImg, dMAP.texWidth, dMAP.texHeight, dMAP.palette);
        }

        private void buttonExportMask_Click(object sender, EventArgs e)
        {
            SaveFileDialog o = new SaveFileDialog();
            o.AddExtension = true;
            o.CheckPathExists = true;
            o.DefaultExt = ".png";
            o.Filter = "Portable Network Graphics (*.png)|*.png";
            o.OverwritePrompt = true;
            o.FileName = Path.ChangeExtension(Path.ChangeExtension(dMAP.FileName, null) + "_mask", ".png");

            if (o.ShowDialog() != DialogResult.OK)
                return;

            Transform.Export8bppTexturePNG(o.FileName, dMAP.texMsk, dMAP.texWidth, dMAP.texHeight, maskPalette);
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
            byte[] texImg = Transform.Import8bppTextureFromImage(oI.FileName, out widthI, out heightI, dMAP.palette);
            if (texImg == null)
            {
                MessageBox.Show("Invalid image file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
            byte[] texMsk = Transform.Import8bppTextureFromImage(oM.FileName, out widthM, out heightM, maskPalette);
            if (texMsk == null)
            {
                MessageBox.Show("Invalid mask file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
            byte[] texture = Transform.Import8bppTextureFromImage(o.FileName, out width, out height, dMAP.palette);
            if (texture == null)
            {
                MessageBox.Show("Invalid image file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
            byte[] texture = Transform.Import8bppTextureFromImage(o.FileName, out width, out height, maskPalette);
            if (texture == null)
            {
                MessageBox.Show("Invalid image file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
