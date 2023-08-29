
namespace DB_KAI_RPG
{
	partial class BSTControl
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
			this.picturePreview = new System.Windows.Forms.PictureBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonImportTexture = new System.Windows.Forms.Button();
			this.buttonExportTexture = new System.Windows.Forms.Button();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.labelData = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.buttonSavePreview = new System.Windows.Forms.Button();
			this.buttonDelete = new System.Windows.Forms.Button();
			this.buttonCreateNew = new System.Windows.Forms.Button();
			this.listBoxLayers = new System.Windows.Forms.ListBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.numericPosY = new System.Windows.Forms.NumericUpDown();
			this.numericPosX = new System.Windows.Forms.NumericUpDown();
			this.comboBoxLayers = new System.Windows.Forms.ComboBox();
			this.checkBoxHighlight = new System.Windows.Forms.CheckBox();
			this.buttonRename = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picturePalette)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picturePreview)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericPosY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericPosX)).BeginInit();
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
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Layer Palette (Edit Layer)";
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
			// picturePreview
			// 
			this.picturePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picturePreview.Location = new System.Drawing.Point(2, 2);
			this.picturePreview.Name = "picturePreview";
			this.picturePreview.Size = new System.Drawing.Size(258, 172);
			this.picturePreview.TabIndex = 2;
			this.picturePreview.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.buttonImportTexture);
			this.groupBox2.Controls.Add(this.buttonExportTexture);
			this.groupBox2.Location = new System.Drawing.Point(201, 326);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(192, 131);
			this.groupBox2.TabIndex = 8;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Layer Texture (Edit Layer)";
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
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(38, 13);
			this.label4.TabIndex = 0;
			this.label4.Text = "Offset:";
			this.toolTip.SetToolTip(this.label4, "Number of columns on preview and exporting.");
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(5, 206);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(68, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Layer to edit:";
			this.toolTip.SetToolTip(this.label2, "Number of columns on preview and exporting.");
			// 
			// labelData
			// 
			this.labelData.AutoSize = true;
			this.labelData.Location = new System.Drawing.Point(202, 464);
			this.labelData.Name = "labelData";
			this.labelData.Size = new System.Drawing.Size(42, 13);
			this.labelData.TabIndex = 9;
			this.labelData.Text = "No Info";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.buttonSavePreview);
			this.groupBox3.Controls.Add(this.buttonCreateNew);
			this.groupBox3.Controls.Add(this.listBoxLayers);
			this.groupBox3.Location = new System.Drawing.Point(264, 3);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(245, 255);
			this.groupBox3.TabIndex = 0;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Layers Visibility";
			// 
			// buttonSavePreview
			// 
			this.buttonSavePreview.Location = new System.Drawing.Point(6, 226);
			this.buttonSavePreview.Name = "buttonSavePreview";
			this.buttonSavePreview.Size = new System.Drawing.Size(100, 23);
			this.buttonSavePreview.TabIndex = 1;
			this.buttonSavePreview.Text = "Export Preview";
			this.buttonSavePreview.UseVisualStyleBackColor = true;
			this.buttonSavePreview.Click += new System.EventHandler(this.buttonSavePreview_Click);
			// 
			// buttonDelete
			// 
			this.buttonDelete.Location = new System.Drawing.Point(172, 229);
			this.buttonDelete.Name = "buttonDelete";
			this.buttonDelete.Size = new System.Drawing.Size(87, 23);
			this.buttonDelete.TabIndex = 5;
			this.buttonDelete.Text = "Delete";
			this.buttonDelete.UseVisualStyleBackColor = true;
			this.buttonDelete.Click += new System.EventHandler(this.deleteSelectedToolStripMenuItem_Click);
			// 
			// buttonCreateNew
			// 
			this.buttonCreateNew.Location = new System.Drawing.Point(139, 226);
			this.buttonCreateNew.Name = "buttonCreateNew";
			this.buttonCreateNew.Size = new System.Drawing.Size(100, 23);
			this.buttonCreateNew.TabIndex = 2;
			this.buttonCreateNew.Text = "Create New...";
			this.buttonCreateNew.UseVisualStyleBackColor = true;
			this.buttonCreateNew.Click += new System.EventHandler(this.createNewToolStripMenuItem_Click);
			// 
			// listBoxLayers
			// 
			this.listBoxLayers.FormattingEnabled = true;
			this.listBoxLayers.Location = new System.Drawing.Point(6, 22);
			this.listBoxLayers.Name = "listBoxLayers";
			this.listBoxLayers.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.listBoxLayers.Size = new System.Drawing.Size(233, 199);
			this.listBoxLayers.TabIndex = 0;
			this.listBoxLayers.SelectedIndexChanged += new System.EventHandler(this.listBoxLayers_SelectedIndexChanged);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.label4);
			this.groupBox4.Controls.Add(this.numericPosY);
			this.groupBox4.Controls.Add(this.numericPosX);
			this.groupBox4.Location = new System.Drawing.Point(201, 264);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(192, 56);
			this.groupBox4.TabIndex = 7;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Layer Position (Edit Layer)";
			// 
			// numericPosY
			// 
			this.numericPosY.Location = new System.Drawing.Point(116, 22);
			this.numericPosY.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
			this.numericPosY.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
			this.numericPosY.Name = "numericPosY";
			this.numericPosY.Size = new System.Drawing.Size(60, 20);
			this.numericPosY.TabIndex = 2;
			this.numericPosY.ValueChanged += new System.EventHandler(this.numericPosY_ValueChanged);
			// 
			// numericPosX
			// 
			this.numericPosX.Location = new System.Drawing.Point(50, 22);
			this.numericPosX.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
			this.numericPosX.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
			this.numericPosX.Name = "numericPosX";
			this.numericPosX.Size = new System.Drawing.Size(60, 20);
			this.numericPosX.TabIndex = 1;
			this.numericPosX.ValueChanged += new System.EventHandler(this.numericPosX_ValueChanged);
			// 
			// comboBoxLayers
			// 
			this.comboBoxLayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxLayers.FormattingEnabled = true;
			this.comboBoxLayers.Location = new System.Drawing.Point(79, 203);
			this.comboBoxLayers.Name = "comboBoxLayers";
			this.comboBoxLayers.Size = new System.Drawing.Size(179, 21);
			this.comboBoxLayers.TabIndex = 3;
			this.comboBoxLayers.SelectedIndexChanged += new System.EventHandler(this.comboBoxLayers_SelectedIndexChanged);
			// 
			// checkBoxHighlight
			// 
			this.checkBoxHighlight.AutoSize = true;
			this.checkBoxHighlight.Location = new System.Drawing.Point(79, 180);
			this.checkBoxHighlight.Name = "checkBoxHighlight";
			this.checkBoxHighlight.Size = new System.Drawing.Size(117, 17);
			this.checkBoxHighlight.TabIndex = 1;
			this.checkBoxHighlight.Text = "Highlight Edit Layer";
			this.checkBoxHighlight.UseVisualStyleBackColor = true;
			this.checkBoxHighlight.CheckedChanged += new System.EventHandler(this.checkBoxHighlight_CheckedChanged);
			// 
			// buttonRename
			// 
			this.buttonRename.Location = new System.Drawing.Point(79, 229);
			this.buttonRename.Name = "buttonRename";
			this.buttonRename.Size = new System.Drawing.Size(87, 23);
			this.buttonRename.TabIndex = 4;
			this.buttonRename.Text = "Rename";
			this.buttonRename.UseVisualStyleBackColor = true;
			this.buttonRename.Click += new System.EventHandler(this.buttonRename_Click);
			// 
			// BSTControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.buttonRename);
			this.Controls.Add(this.buttonDelete);
			this.Controls.Add(this.checkBoxHighlight);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.comboBoxLayers);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.labelData);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.picturePreview);
			this.Controls.Add(this.groupBox1);
			this.Name = "BSTControl";
			this.Size = new System.Drawing.Size(512, 512);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picturePalette)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picturePreview)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericPosY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericPosX)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button buttonImportPal;
		private System.Windows.Forms.Button buttonExportPal;
		private System.Windows.Forms.PictureBox picturePreview;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button buttonImportTexture;
		private System.Windows.Forms.Button buttonExportTexture;
		private System.Windows.Forms.PictureBox picturePalette;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelData;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button buttonDelete;
		private System.Windows.Forms.Button buttonCreateNew;
		private System.Windows.Forms.ListBox listBoxLayers;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.NumericUpDown numericPosY;
		private System.Windows.Forms.NumericUpDown numericPosX;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button buttonSavePreview;
		private System.Windows.Forms.ComboBox comboBoxLayers;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox checkBoxHighlight;
		private System.Windows.Forms.Button buttonRename;
	}
}
