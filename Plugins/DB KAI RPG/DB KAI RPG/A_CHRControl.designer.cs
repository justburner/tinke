
namespace DB_KAI_RPG
{
	partial class A_CHRControl
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
			this.pictureTileset = new System.Windows.Forms.PictureBox();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.checkMaxTiles = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.contextMenuBsts = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.manageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.createNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.button4 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.listBoxChrs = new System.Windows.Forms.ListBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.numericMaxTiles = new System.Windows.Forms.NumericUpDown();
			this.numericPreviewWidth = new System.Windows.Forms.NumericUpDown();
			this.buttonImportTiles = new System.Windows.Forms.Button();
			this.buttonExportTiles = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.picturePalette = new System.Windows.Forms.PictureBox();
			this.label3 = new System.Windows.Forms.Label();
			this.buttonImportPal = new System.Windows.Forms.Button();
			this.buttonExportPal = new System.Windows.Forms.Button();
			this.labelNumPalettes = new System.Windows.Forms.Label();
			this.numericPalette = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.pictureTileset)).BeginInit();
			this.contextMenuBsts.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericMaxTiles)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericPreviewWidth)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picturePalette)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericPalette)).BeginInit();
			this.SuspendLayout();
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
			// checkMaxTiles
			// 
			this.checkMaxTiles.AutoSize = true;
			this.checkMaxTiles.Checked = true;
			this.checkMaxTiles.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkMaxTiles.Location = new System.Drawing.Point(21, 45);
			this.checkMaxTiles.Name = "checkMaxTiles";
			this.checkMaxTiles.Size = new System.Drawing.Size(78, 17);
			this.checkMaxTiles.TabIndex = 2;
			this.checkMaxTiles.Text = "Max Tiles*:";
			this.toolTip.SetToolTip(this.checkMaxTiles, "Max number of tiles on importing.");
			this.checkMaxTiles.UseVisualStyleBackColor = true;
			this.checkMaxTiles.CheckedChanged += new System.EventHandler(this.checkMaxTiles_CheckedChanged);
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
			// contextMenuBsts
			// 
			this.contextMenuBsts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem,
            this.toolStripSeparator1,
            this.manageToolStripMenuItem});
			this.contextMenuBsts.Name = "contextMenuLayers";
			this.contextMenuBsts.Size = new System.Drawing.Size(139, 76);
			// 
			// moveUpToolStripMenuItem
			// 
			this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
			this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
			this.moveUpToolStripMenuItem.Text = "Move Up";
			this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpToolStripMenuItem_Click);
			// 
			// moveDownToolStripMenuItem
			// 
			this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
			this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
			this.moveDownToolStripMenuItem.Text = "Move Down";
			this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.moveDownToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(135, 6);
			// 
			// manageToolStripMenuItem
			// 
			this.manageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createNewToolStripMenuItem,
            this.deleteSelectedToolStripMenuItem});
			this.manageToolStripMenuItem.Name = "manageToolStripMenuItem";
			this.manageToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
			this.manageToolStripMenuItem.Text = "Manage";
			// 
			// createNewToolStripMenuItem
			// 
			this.createNewToolStripMenuItem.Name = "createNewToolStripMenuItem";
			this.createNewToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.createNewToolStripMenuItem.Text = "Create New";
			this.createNewToolStripMenuItem.Click += new System.EventHandler(this.createNewToolStripMenuItem_Click);
			// 
			// deleteSelectedToolStripMenuItem
			// 
			this.deleteSelectedToolStripMenuItem.Name = "deleteSelectedToolStripMenuItem";
			this.deleteSelectedToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.deleteSelectedToolStripMenuItem.Text = "Delete Selected";
			this.deleteSelectedToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectedToolStripMenuItem_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.button4);
			this.groupBox3.Controls.Add(this.button3);
			this.groupBox3.Controls.Add(this.button2);
			this.groupBox3.Controls.Add(this.button1);
			this.groupBox3.Controls.Add(this.listBoxChrs);
			this.groupBox3.Location = new System.Drawing.Point(264, 3);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(245, 255);
			this.groupBox3.TabIndex = 0;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Characters Archive";
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(139, 226);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(100, 23);
			this.button4.TabIndex = 4;
			this.button4.Text = "Delete Selected";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.deleteSelectedToolStripMenuItem_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(139, 200);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(100, 23);
			this.button3.TabIndex = 2;
			this.button3.Text = "Create New";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.createNewToolStripMenuItem_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(6, 226);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(100, 23);
			this.button2.TabIndex = 3;
			this.button2.Text = "Move Down";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.moveDownToolStripMenuItem_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(6, 200);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(100, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Move Up";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.moveUpToolStripMenuItem_Click);
			// 
			// listBoxChrs
			// 
			this.listBoxChrs.ContextMenuStrip = this.contextMenuBsts;
			this.listBoxChrs.FormattingEnabled = true;
			this.listBoxChrs.Location = new System.Drawing.Point(6, 19);
			this.listBoxChrs.Name = "listBoxChrs";
			this.listBoxChrs.Size = new System.Drawing.Size(233, 173);
			this.listBoxChrs.TabIndex = 0;
			this.listBoxChrs.SelectedIndexChanged += new System.EventHandler(this.listBoxChrs_SelectedIndexChanged);
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
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Tileset (Selected Image)";
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
			this.numericMaxTiles.TabIndex = 3;
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
			// buttonImportTiles
			// 
			this.buttonImportTiles.Location = new System.Drawing.Point(88, 71);
			this.buttonImportTiles.Name = "buttonImportTiles";
			this.buttonImportTiles.Size = new System.Drawing.Size(75, 23);
			this.buttonImportTiles.TabIndex = 5;
			this.buttonImportTiles.Text = "Import";
			this.buttonImportTiles.UseVisualStyleBackColor = true;
			// 
			// buttonExportTiles
			// 
			this.buttonExportTiles.Location = new System.Drawing.Point(7, 71);
			this.buttonExportTiles.Name = "buttonExportTiles";
			this.buttonExportTiles.Size = new System.Drawing.Size(75, 23);
			this.buttonExportTiles.TabIndex = 4;
			this.buttonExportTiles.Text = "Export";
			this.buttonExportTiles.UseVisualStyleBackColor = true;
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
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Palette (Selected Image)";
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
			// A_CHRControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.pictureTileset);
			this.Name = "A_CHRControl";
			this.Size = new System.Drawing.Size(512, 512);
			((System.ComponentModel.ISupportInitialize)(this.pictureTileset)).EndInit();
			this.contextMenuBsts.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericMaxTiles)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericPreviewWidth)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picturePalette)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericPalette)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.PictureBox pictureTileset;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.ContextMenuStrip contextMenuBsts;
		private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem manageToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem createNewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteSelectedToolStripMenuItem;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ListBox listBoxChrs;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox checkMaxTiles;
		private System.Windows.Forms.NumericUpDown numericMaxTiles;
		private System.Windows.Forms.NumericUpDown numericPreviewWidth;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonImportTiles;
		private System.Windows.Forms.Button buttonExportTiles;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.PictureBox picturePalette;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button buttonImportPal;
		private System.Windows.Forms.Button buttonExportPal;
		private System.Windows.Forms.Label labelNumPalettes;
		private System.Windows.Forms.NumericUpDown numericPalette;
	}
}
