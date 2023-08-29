
namespace DB_KAI_RPG
{
	partial class IMPControl
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.picturePalette = new System.Windows.Forms.PictureBox();
			this.buttonImportPal = new System.Windows.Forms.Button();
			this.buttonExportPal = new System.Windows.Forms.Button();
			this.pictureTileset = new System.Windows.Forms.PictureBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonImportTexture = new System.Windows.Forms.Button();
			this.buttonExportTexture = new System.Windows.Forms.Button();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.labelData = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picturePalette)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureTileset)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.picturePalette);
			this.groupBox1.Controls.Add(this.buttonImportPal);
			this.groupBox1.Controls.Add(this.buttonExportPal);
			this.groupBox1.Location = new System.Drawing.Point(3, 264);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(192, 193);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Palette";
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
			this.buttonImportPal.TabIndex = 1;
			this.buttonImportPal.Text = "Import";
			this.buttonImportPal.UseVisualStyleBackColor = true;
			this.buttonImportPal.Click += new System.EventHandler(this.buttonImportPal_Click);
			// 
			// buttonExportPal
			// 
			this.buttonExportPal.Location = new System.Drawing.Point(7, 161);
			this.buttonExportPal.Name = "buttonExportPal";
			this.buttonExportPal.Size = new System.Drawing.Size(75, 23);
			this.buttonExportPal.TabIndex = 0;
			this.buttonExportPal.Text = "Export";
			this.buttonExportPal.UseVisualStyleBackColor = true;
			this.buttonExportPal.Click += new System.EventHandler(this.buttonExportPal_Click);
			// 
			// pictureTileset
			// 
			this.pictureTileset.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureTileset.Location = new System.Drawing.Point(2, 2);
			this.pictureTileset.Name = "pictureTileset";
			this.pictureTileset.Size = new System.Drawing.Size(256, 256);
			this.pictureTileset.TabIndex = 2;
			this.pictureTileset.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.buttonImportTexture);
			this.groupBox2.Controls.Add(this.buttonExportTexture);
			this.groupBox2.Location = new System.Drawing.Point(201, 264);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(192, 131);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Texture";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 58);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(158, 65);
			this.label1.TabIndex = 2;
			this.label1.Text = "Texture import requirements:\r\n\r\nWidth must be multiple of 2.\r\nHeight must be mult" +
    "iple of 2.\r\n256 colors matching the palette.";
			// 
			// buttonImportTexture
			// 
			this.buttonImportTexture.Location = new System.Drawing.Point(88, 19);
			this.buttonImportTexture.Name = "buttonImportTexture";
			this.buttonImportTexture.Size = new System.Drawing.Size(75, 23);
			this.buttonImportTexture.TabIndex = 1;
			this.buttonImportTexture.Text = "Import";
			this.buttonImportTexture.UseVisualStyleBackColor = true;
			this.buttonImportTexture.Click += new System.EventHandler(this.buttonImportTexture_Click);
			// 
			// buttonExportTexture
			// 
			this.buttonExportTexture.Location = new System.Drawing.Point(7, 19);
			this.buttonExportTexture.Name = "buttonExportTexture";
			this.buttonExportTexture.Size = new System.Drawing.Size(75, 23);
			this.buttonExportTexture.TabIndex = 0;
			this.buttonExportTexture.Text = "Export";
			this.buttonExportTexture.UseVisualStyleBackColor = true;
			this.buttonExportTexture.Click += new System.EventHandler(this.buttonExportTexture_Click);
			// 
			// labelData
			// 
			this.labelData.AutoSize = true;
			this.labelData.Location = new System.Drawing.Point(202, 402);
			this.labelData.Name = "labelData";
			this.labelData.Size = new System.Drawing.Size(42, 13);
			this.labelData.TabIndex = 2;
			this.labelData.Text = "No Info";
			// 
			// IMPControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.labelData);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.pictureTileset);
			this.Controls.Add(this.groupBox1);
			this.Name = "IMPControl";
			this.Size = new System.Drawing.Size(512, 512);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picturePalette)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureTileset)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button buttonImportPal;
		private System.Windows.Forms.Button buttonExportPal;
		private System.Windows.Forms.PictureBox pictureTileset;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button buttonImportTexture;
		private System.Windows.Forms.Button buttonExportTexture;
		private System.Windows.Forms.PictureBox picturePalette;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelData;
	}
}
