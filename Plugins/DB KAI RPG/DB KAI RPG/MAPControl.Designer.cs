
namespace DB_KAI_RPG
{
	partial class MAPControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.pictureMsk = new System.Windows.Forms.PictureBox();
			this.pictureImg = new System.Windows.Forms.PictureBox();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.button1 = new System.Windows.Forms.Button();
			this.buttonImportMask = new System.Windows.Forms.Button();
			this.buttonExportMask = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonImportImage = new System.Windows.Forms.Button();
			this.buttonExportImage = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.buttonExportMaskPal = new System.Windows.Forms.Button();
			this.picturePalette = new System.Windows.Forms.PictureBox();
			this.buttonImportPal = new System.Windows.Forms.Button();
			this.buttonExportPal = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.labelExData = new System.Windows.Forms.Label();
			this.buttonImportExData = new System.Windows.Forms.Button();
			this.buttonExportExData = new System.Windows.Forms.Button();
			this.labelData = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureMsk)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureImg)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picturePalette)).BeginInit();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureMsk
			// 
			this.pictureMsk.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureMsk.Location = new System.Drawing.Point(260, 2);
			this.pictureMsk.Name = "pictureMsk";
			this.pictureMsk.Size = new System.Drawing.Size(250, 256);
			this.pictureMsk.TabIndex = 0;
			this.pictureMsk.TabStop = false;
			// 
			// pictureImg
			// 
			this.pictureImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureImg.Location = new System.Drawing.Point(2, 2);
			this.pictureImg.Name = "pictureImg";
			this.pictureImg.Size = new System.Drawing.Size(256, 256);
			this.pictureImg.TabIndex = 2;
			this.pictureImg.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.labelData);
			this.groupBox2.Controls.Add(this.button1);
			this.groupBox2.Controls.Add(this.buttonImportMask);
			this.groupBox2.Controls.Add(this.buttonExportMask);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.buttonImportImage);
			this.groupBox2.Controls.Add(this.buttonExportImage);
			this.groupBox2.Location = new System.Drawing.Point(201, 264);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(265, 150);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Textures";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(179, 19);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(80, 23);
			this.button1.TabIndex = 8;
			this.button1.Text = "Import Both";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.buttonImportBoth_Click);
			// 
			// buttonImportMask
			// 
			this.buttonImportMask.Location = new System.Drawing.Point(93, 48);
			this.buttonImportMask.Name = "buttonImportMask";
			this.buttonImportMask.Size = new System.Drawing.Size(80, 23);
			this.buttonImportMask.TabIndex = 7;
			this.buttonImportMask.Text = "Import Mask";
			this.buttonImportMask.UseVisualStyleBackColor = true;
			this.buttonImportMask.Click += new System.EventHandler(this.buttonImportMask_Click);
			// 
			// buttonExportMask
			// 
			this.buttonExportMask.Location = new System.Drawing.Point(6, 48);
			this.buttonExportMask.Name = "buttonExportMask";
			this.buttonExportMask.Size = new System.Drawing.Size(80, 23);
			this.buttonExportMask.TabIndex = 6;
			this.buttonExportMask.Text = "Export Mask";
			this.buttonExportMask.UseVisualStyleBackColor = true;
			this.buttonExportMask.Click += new System.EventHandler(this.buttonExportMask_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 72);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(248, 26);
			this.label1.TabIndex = 5;
			this.label1.Text = "Texture import requirements:\r\nWidth and Height must be multiple of 8. Colors: 256" +
    "";
			// 
			// buttonImportImage
			// 
			this.buttonImportImage.Location = new System.Drawing.Point(93, 19);
			this.buttonImportImage.Name = "buttonImportImage";
			this.buttonImportImage.Size = new System.Drawing.Size(80, 23);
			this.buttonImportImage.TabIndex = 4;
			this.buttonImportImage.Text = "Import Image";
			this.buttonImportImage.UseVisualStyleBackColor = true;
			this.buttonImportImage.Click += new System.EventHandler(this.buttonImportImage_Click);
			// 
			// buttonExportImage
			// 
			this.buttonExportImage.Location = new System.Drawing.Point(7, 19);
			this.buttonExportImage.Name = "buttonExportImage";
			this.buttonExportImage.Size = new System.Drawing.Size(80, 23);
			this.buttonExportImage.TabIndex = 3;
			this.buttonExportImage.Text = "Export Image";
			this.buttonExportImage.UseVisualStyleBackColor = true;
			this.buttonExportImage.Click += new System.EventHandler(this.buttonExportImage_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.buttonExportMaskPal);
			this.groupBox1.Controls.Add(this.picturePalette);
			this.groupBox1.Controls.Add(this.buttonImportPal);
			this.groupBox1.Controls.Add(this.buttonExportPal);
			this.groupBox1.Location = new System.Drawing.Point(3, 264);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(192, 225);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Palette";
			// 
			// buttonExportMaskPal
			// 
			this.buttonExportMaskPal.Location = new System.Drawing.Point(7, 190);
			this.buttonExportMaskPal.Name = "buttonExportMaskPal";
			this.buttonExportMaskPal.Size = new System.Drawing.Size(156, 23);
			this.buttonExportMaskPal.TabIndex = 6;
			this.buttonExportMaskPal.Text = "Export Mask Palette";
			this.buttonExportMaskPal.UseVisualStyleBackColor = true;
			this.buttonExportMaskPal.Click += new System.EventHandler(this.buttonExportMaskPal_Click);
			// 
			// picturePalette
			// 
			this.picturePalette.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picturePalette.Location = new System.Drawing.Point(19, 22);
			this.picturePalette.Name = "picturePalette";
			this.picturePalette.Size = new System.Drawing.Size(128, 128);
			this.picturePalette.TabIndex = 5;
			this.picturePalette.TabStop = false;
			// 
			// buttonImportPal
			// 
			this.buttonImportPal.Location = new System.Drawing.Point(88, 161);
			this.buttonImportPal.Name = "buttonImportPal";
			this.buttonImportPal.Size = new System.Drawing.Size(75, 23);
			this.buttonImportPal.TabIndex = 4;
			this.buttonImportPal.Text = "Import";
			this.buttonImportPal.UseVisualStyleBackColor = true;
			this.buttonImportPal.Click += new System.EventHandler(this.buttonImportPal_Click);
			// 
			// buttonExportPal
			// 
			this.buttonExportPal.Location = new System.Drawing.Point(7, 161);
			this.buttonExportPal.Name = "buttonExportPal";
			this.buttonExportPal.Size = new System.Drawing.Size(75, 23);
			this.buttonExportPal.TabIndex = 3;
			this.buttonExportPal.Text = "Export";
			this.buttonExportPal.UseVisualStyleBackColor = true;
			this.buttonExportPal.Click += new System.EventHandler(this.buttonExportPal_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.labelExData);
			this.groupBox3.Controls.Add(this.buttonImportExData);
			this.groupBox3.Controls.Add(this.buttonExportExData);
			this.groupBox3.Location = new System.Drawing.Point(201, 420);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(265, 69);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Extra Data";
			// 
			// labelExData
			// 
			this.labelExData.AutoSize = true;
			this.labelExData.Location = new System.Drawing.Point(6, 47);
			this.labelExData.Name = "labelExData";
			this.labelExData.Size = new System.Drawing.Size(71, 13);
			this.labelExData.TabIndex = 5;
			this.labelExData.Text = "No extra data";
			// 
			// buttonImportExData
			// 
			this.buttonImportExData.Location = new System.Drawing.Point(93, 19);
			this.buttonImportExData.Name = "buttonImportExData";
			this.buttonImportExData.Size = new System.Drawing.Size(80, 23);
			this.buttonImportExData.TabIndex = 4;
			this.buttonImportExData.Text = "Import";
			this.buttonImportExData.UseVisualStyleBackColor = true;
			this.buttonImportExData.Click += new System.EventHandler(this.buttonImportExData_Click);
			// 
			// buttonExportExData
			// 
			this.buttonExportExData.Location = new System.Drawing.Point(7, 19);
			this.buttonExportExData.Name = "buttonExportExData";
			this.buttonExportExData.Size = new System.Drawing.Size(80, 23);
			this.buttonExportExData.TabIndex = 3;
			this.buttonExportExData.Text = "Export";
			this.buttonExportExData.UseVisualStyleBackColor = true;
			this.buttonExportExData.Click += new System.EventHandler(this.buttonExportExData_Click);
			// 
			// labelData
			// 
			this.labelData.AutoSize = true;
			this.labelData.Location = new System.Drawing.Point(6, 104);
			this.labelData.Name = "labelData";
			this.labelData.Size = new System.Drawing.Size(42, 13);
			this.labelData.TabIndex = 9;
			this.labelData.Text = "No Info";
			// 
			// MAPControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.pictureImg);
			this.Controls.Add(this.pictureMsk);
			this.Name = "MAPControl";
			this.Size = new System.Drawing.Size(512, 512);
			((System.ComponentModel.ISupportInitialize)(this.pictureMsk)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureImg)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picturePalette)).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureMsk;
		private System.Windows.Forms.PictureBox pictureImg;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonImportImage;
		private System.Windows.Forms.Button buttonExportImage;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.PictureBox picturePalette;
		private System.Windows.Forms.Button buttonImportPal;
		private System.Windows.Forms.Button buttonExportPal;
		private System.Windows.Forms.Button buttonExportMask;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button buttonImportMask;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label labelExData;
		private System.Windows.Forms.Button buttonImportExData;
		private System.Windows.Forms.Button buttonExportExData;
		private System.Windows.Forms.Button buttonExportMaskPal;
		private System.Windows.Forms.Label labelData;
	}
}
