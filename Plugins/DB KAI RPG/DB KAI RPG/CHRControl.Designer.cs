
namespace DB_KAI_RPG
{
	partial class CHRControl
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
			this.pictureSprite = new System.Windows.Forms.PictureBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.picturePalette = new System.Windows.Forms.PictureBox();
			this.label3 = new System.Windows.Forms.Label();
			this.buttonImportPal = new System.Windows.Forms.Button();
			this.buttonExportPal = new System.Windows.Forms.Button();
			this.labelNumPalettes = new System.Windows.Forms.Label();
			this.numericPalette = new System.Windows.Forms.NumericUpDown();
			this.pictureTileset = new System.Windows.Forms.PictureBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkMaxTiles = new System.Windows.Forms.CheckBox();
			this.numericMaxTiles = new System.Windows.Forms.NumericUpDown();
			this.numericPreviewWidth = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonImportTiles = new System.Windows.Forms.Button();
			this.buttonExportTiles = new System.Windows.Forms.Button();
			this.groupSprite = new System.Windows.Forms.GroupBox();
			this.labelNumLayers = new System.Windows.Forms.Label();
			this.listLayers = new System.Windows.Forms.ListBox();
			this.contextMenuLayers = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.deselectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.labelNumSprites = new System.Windows.Forms.Label();
			this.numericSprite = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.labelDebug = new System.Windows.Forms.Label();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.pictureSprite)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picturePalette)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericPalette)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureTileset)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericMaxTiles)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericPreviewWidth)).BeginInit();
			this.groupSprite.SuspendLayout();
			this.contextMenuLayers.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericSprite)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureSprite
			// 
			this.pictureSprite.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureSprite.Location = new System.Drawing.Point(260, 2);
			this.pictureSprite.Name = "pictureSprite";
			this.pictureSprite.Size = new System.Drawing.Size(250, 256);
			this.pictureSprite.TabIndex = 0;
			this.pictureSprite.TabStop = false;
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
			this.groupBox2.Controls.Add(this.checkMaxTiles);
			this.groupBox2.Controls.Add(this.numericMaxTiles);
			this.groupBox2.Controls.Add(this.numericPreviewWidth);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.buttonImportTiles);
			this.groupBox2.Controls.Add(this.buttonExportTiles);
			this.groupBox2.Location = new System.Drawing.Point(3, 359);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(192, 101);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Tileset";
			// 
			// checkMaxTiles
			// 
			this.checkMaxTiles.AutoSize = true;
			this.checkMaxTiles.Checked = true;
			this.checkMaxTiles.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkMaxTiles.Location = new System.Drawing.Point(21, 45);
			this.checkMaxTiles.Name = "checkMaxTiles";
			this.checkMaxTiles.Size = new System.Drawing.Size(78, 17);
			this.checkMaxTiles.TabIndex = 7;
			this.checkMaxTiles.Text = "Max Tiles*:";
			this.toolTip.SetToolTip(this.checkMaxTiles, "Max number of tiles on importing.");
			this.checkMaxTiles.UseVisualStyleBackColor = true;
			this.checkMaxTiles.CheckedChanged += new System.EventHandler(this.checkMaxTiles_CheckedChanged);
			// 
			// numericMaxTiles
			// 
			this.numericMaxTiles.Location = new System.Drawing.Point(103, 45);
			this.numericMaxTiles.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
			this.numericMaxTiles.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericMaxTiles.Name = "numericMaxTiles";
			this.numericMaxTiles.Size = new System.Drawing.Size(60, 20);
			this.numericMaxTiles.TabIndex = 6;
			this.numericMaxTiles.Value = new decimal(new int[] {
            2048,
            0,
            0,
            0});
			// 
			// numericPreviewWidth
			// 
			this.numericPreviewWidth.Location = new System.Drawing.Point(103, 19);
			this.numericPreviewWidth.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
			this.numericPreviewWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericPreviewWidth.Name = "numericPreviewWidth";
			this.numericPreviewWidth.Size = new System.Drawing.Size(60, 20);
			this.numericPreviewWidth.TabIndex = 1;
			this.numericPreviewWidth.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
			this.numericPreviewWidth.ValueChanged += new System.EventHandler(this.numericPreviewWidth_ValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(47, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(50, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Columns:";
			this.toolTip.SetToolTip(this.label1, "Number of columns on preview and exporting.");
			// 
			// buttonImportTiles
			// 
			this.buttonImportTiles.Location = new System.Drawing.Point(88, 71);
			this.buttonImportTiles.Name = "buttonImportTiles";
			this.buttonImportTiles.Size = new System.Drawing.Size(75, 23);
			this.buttonImportTiles.TabIndex = 4;
			this.buttonImportTiles.Text = "Import";
			this.buttonImportTiles.UseVisualStyleBackColor = true;
			this.buttonImportTiles.Click += new System.EventHandler(this.buttonImportTiles_Click);
			// 
			// buttonExportTiles
			// 
			this.buttonExportTiles.Location = new System.Drawing.Point(7, 71);
			this.buttonExportTiles.Name = "buttonExportTiles";
			this.buttonExportTiles.Size = new System.Drawing.Size(75, 23);
			this.buttonExportTiles.TabIndex = 3;
			this.buttonExportTiles.Text = "Export";
			this.buttonExportTiles.UseVisualStyleBackColor = true;
			this.buttonExportTiles.Click += new System.EventHandler(this.buttonExportTiles_Click);
			// 
			// groupSprite
			// 
			this.groupSprite.Controls.Add(this.labelNumLayers);
			this.groupSprite.Controls.Add(this.listLayers);
			this.groupSprite.Controls.Add(this.labelNumSprites);
			this.groupSprite.Controls.Add(this.numericSprite);
			this.groupSprite.Controls.Add(this.label2);
			this.groupSprite.Location = new System.Drawing.Point(201, 264);
			this.groupSprite.Name = "groupSprite";
			this.groupSprite.Size = new System.Drawing.Size(308, 245);
			this.groupSprite.TabIndex = 2;
			this.groupSprite.TabStop = false;
			this.groupSprite.Text = "Sprites";
			// 
			// labelNumLayers
			// 
			this.labelNumLayers.AutoSize = true;
			this.labelNumLayers.Location = new System.Drawing.Point(154, 45);
			this.labelNumLayers.Name = "labelNumLayers";
			this.labelNumLayers.Size = new System.Drawing.Size(45, 13);
			this.labelNumLayers.TabIndex = 4;
			this.labelNumLayers.Text = "No data";
			// 
			// listLayers
			// 
			this.listLayers.ContextMenuStrip = this.contextMenuLayers;
			this.listLayers.FormattingEnabled = true;
			this.listLayers.Location = new System.Drawing.Point(6, 45);
			this.listLayers.Name = "listLayers";
			this.listLayers.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listLayers.Size = new System.Drawing.Size(142, 186);
			this.listLayers.TabIndex = 3;
			this.listLayers.SelectedIndexChanged += new System.EventHandler(this.listLayers_SelectedIndexChanged);
			// 
			// contextMenuLayers
			// 
			this.contextMenuLayers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deselectToolStripMenuItem});
			this.contextMenuLayers.Name = "contextMenuLayers";
			this.contextMenuLayers.Size = new System.Drawing.Size(119, 26);
			// 
			// deselectToolStripMenuItem
			// 
			this.deselectToolStripMenuItem.Name = "deselectToolStripMenuItem";
			this.deselectToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
			this.deselectToolStripMenuItem.Text = "Deselect";
			this.deselectToolStripMenuItem.Click += new System.EventHandler(this.deselectToolStripMenuItem_Click);
			// 
			// labelNumSprites
			// 
			this.labelNumSprites.AutoSize = true;
			this.labelNumSprites.Location = new System.Drawing.Point(154, 21);
			this.labelNumSprites.Name = "labelNumSprites";
			this.labelNumSprites.Size = new System.Drawing.Size(25, 13);
			this.labelNumSprites.TabIndex = 2;
			this.labelNumSprites.Text = "of 0";
			// 
			// numericSprite
			// 
			this.numericSprite.Location = new System.Drawing.Point(88, 19);
			this.numericSprite.Name = "numericSprite";
			this.numericSprite.Size = new System.Drawing.Size(60, 20);
			this.numericSprite.TabIndex = 1;
			this.numericSprite.ValueChanged += new System.EventHandler(this.numericSprite_ValueChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(45, 21);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(37, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Sprite:";
			// 
			// labelDebug
			// 
			this.labelDebug.AutoSize = true;
			this.labelDebug.Location = new System.Drawing.Point(3, 463);
			this.labelDebug.Name = "labelDebug";
			this.labelDebug.Size = new System.Drawing.Size(116, 13);
			this.labelDebug.TabIndex = 3;
			this.labelDebug.Text = "Uninitialized debug info";
			// 
			// CHRControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.labelDebug);
			this.Controls.Add(this.groupSprite);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.pictureTileset);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.pictureSprite);
			this.Name = "CHRControl";
			this.Size = new System.Drawing.Size(512, 512);
			((System.ComponentModel.ISupportInitialize)(this.pictureSprite)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picturePalette)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericPalette)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureTileset)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericMaxTiles)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericPreviewWidth)).EndInit();
			this.groupSprite.ResumeLayout(false);
			this.groupSprite.PerformLayout();
			this.contextMenuLayers.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericSprite)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureSprite;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button buttonImportPal;
		private System.Windows.Forms.Button buttonExportPal;
		private System.Windows.Forms.Label labelNumPalettes;
		private System.Windows.Forms.NumericUpDown numericPalette;
		private System.Windows.Forms.PictureBox pictureTileset;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button buttonImportTiles;
		private System.Windows.Forms.Button buttonExportTiles;
		private System.Windows.Forms.GroupBox groupSprite;
		private System.Windows.Forms.Label labelDebug;
		private System.Windows.Forms.NumericUpDown numericPreviewWidth;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label labelNumSprites;
		private System.Windows.Forms.NumericUpDown numericSprite;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListBox listLayers;
		private System.Windows.Forms.Label labelNumLayers;
		private System.Windows.Forms.PictureBox picturePalette;
		private System.Windows.Forms.CheckBox checkMaxTiles;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.NumericUpDown numericMaxTiles;
		private System.Windows.Forms.ContextMenuStrip contextMenuLayers;
		private System.Windows.Forms.ToolStripMenuItem deselectToolStripMenuItem;
	}
}
