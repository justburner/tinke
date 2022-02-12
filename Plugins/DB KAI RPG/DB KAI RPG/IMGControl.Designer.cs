
namespace DB_KAI_RPG
{
	partial class IMGControl
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
			this.label3 = new System.Windows.Forms.Label();
			this.buttonImportPal = new System.Windows.Forms.Button();
			this.buttonExportPal = new System.Windows.Forms.Button();
			this.labelNumPalettes = new System.Windows.Forms.Label();
			this.numericPalette = new System.Windows.Forms.NumericUpDown();
			this.pictureTileset = new System.Windows.Forms.PictureBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.buttonImportTexture = new System.Windows.Forms.Button();
			this.buttonExportTexture = new System.Windows.Forms.Button();
			this.contextMenuLayers = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picturePalette)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericPalette)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureTileset)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.picturePalette);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.buttonImportPal);
			this.groupBox1.Controls.Add(this.buttonExportPal);
			this.groupBox1.Controls.Add(this.labelNumPalettes);
			this.groupBox1.Controls.Add(this.numericPalette);
			this.groupBox1.Location = new System.Drawing.Point(3, 264);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(192, 89);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Palette";
			// 
			// picturePalette
			// 
			this.picturePalette.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picturePalette.Location = new System.Drawing.Point(20, 45);
			this.picturePalette.Name = "picturePalette";
			this.picturePalette.Size = new System.Drawing.Size(128, 8);
			this.picturePalette.TabIndex = 5;
			this.picturePalette.TabStop = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(18, 21);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Palette Slot:";
			// 
			// buttonImportPal
			// 
			this.buttonImportPal.Location = new System.Drawing.Point(88, 59);
			this.buttonImportPal.Name = "buttonImportPal";
			this.buttonImportPal.Size = new System.Drawing.Size(75, 23);
			this.buttonImportPal.TabIndex = 4;
			this.buttonImportPal.Text = "Import";
			this.buttonImportPal.UseVisualStyleBackColor = true;
			this.buttonImportPal.Click += new System.EventHandler(this.buttonImportPal_Click);
			// 
			// buttonExportPal
			// 
			this.buttonExportPal.Location = new System.Drawing.Point(7, 59);
			this.buttonExportPal.Name = "buttonExportPal";
			this.buttonExportPal.Size = new System.Drawing.Size(75, 23);
			this.buttonExportPal.TabIndex = 3;
			this.buttonExportPal.Text = "Export";
			this.buttonExportPal.UseVisualStyleBackColor = true;
			this.buttonExportPal.Click += new System.EventHandler(this.buttonExportPal_Click);
			// 
			// labelNumPalettes
			// 
			this.labelNumPalettes.AutoSize = true;
			this.labelNumPalettes.Location = new System.Drawing.Point(154, 21);
			this.labelNumPalettes.Name = "labelNumPalettes";
			this.labelNumPalettes.Size = new System.Drawing.Size(25, 13);
			this.labelNumPalettes.TabIndex = 2;
			this.labelNumPalettes.Text = "of 0";
			// 
			// numericPalette
			// 
			this.numericPalette.Location = new System.Drawing.Point(88, 19);
			this.numericPalette.Name = "numericPalette";
			this.numericPalette.Size = new System.Drawing.Size(60, 20);
			this.numericPalette.TabIndex = 1;
			this.numericPalette.ValueChanged += new System.EventHandler(this.numericPalette_ValueChanged);
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
			this.groupBox2.Location = new System.Drawing.Point(3, 359);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(192, 131);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Texture";
			// 
			// buttonImportTexture
			// 
			this.buttonImportTexture.Location = new System.Drawing.Point(88, 19);
			this.buttonImportTexture.Name = "buttonImportTexture";
			this.buttonImportTexture.Size = new System.Drawing.Size(75, 23);
			this.buttonImportTexture.TabIndex = 4;
			this.buttonImportTexture.Text = "Import";
			this.buttonImportTexture.UseVisualStyleBackColor = true;
			this.buttonImportTexture.Click += new System.EventHandler(this.buttonImportTexture_Click);
			// 
			// buttonExportTexture
			// 
			this.buttonExportTexture.Location = new System.Drawing.Point(7, 19);
			this.buttonExportTexture.Name = "buttonExportTexture";
			this.buttonExportTexture.Size = new System.Drawing.Size(75, 23);
			this.buttonExportTexture.TabIndex = 3;
			this.buttonExportTexture.Text = "Export";
			this.buttonExportTexture.UseVisualStyleBackColor = true;
			this.buttonExportTexture.Click += new System.EventHandler(this.buttonExportTexture_Click);
			// 
			// contextMenuLayers
			// 
			this.contextMenuLayers.Name = "contextMenuLayers";
			this.contextMenuLayers.Size = new System.Drawing.Size(61, 4);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 58);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(172, 65);
			this.label1.TabIndex = 5;
			this.label1.Text = "Texture import requirements:\r\n\r\nWidth must be multiple of 4.\r\nHeight must be mult" +
    "iple of 2.\r\nPalette must be either 1 or 16 slots.";
			// 
			// IMGControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.pictureTileset);
			this.Controls.Add(this.groupBox1);
			this.Name = "IMGControl";
			this.Size = new System.Drawing.Size(512, 512);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picturePalette)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericPalette)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureTileset)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button buttonImportPal;
		private System.Windows.Forms.Button buttonExportPal;
		private System.Windows.Forms.Label labelNumPalettes;
		private System.Windows.Forms.NumericUpDown numericPalette;
		private System.Windows.Forms.PictureBox pictureTileset;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button buttonImportTexture;
		private System.Windows.Forms.Button buttonExportTexture;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.PictureBox picturePalette;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.ContextMenuStrip contextMenuLayers;
		private System.Windows.Forms.Label label1;
	}
}
