
namespace DB_KAI_RPG
{
	partial class IMAControl
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
			this.contextMenuImps = new System.Windows.Forms.ContextMenuStrip(this.components);
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
			this.listBoxImps = new System.Windows.Forms.ListBox();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picturePalette)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureTileset)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.contextMenuImps.SuspendLayout();
			this.groupBox3.SuspendLayout();
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
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Palette (Selected Image)";
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
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Texture (Selected Image)";
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
			this.labelData.TabIndex = 3;
			this.labelData.Text = "No Info";
			// 
			// contextMenuImps
			// 
			this.contextMenuImps.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem,
            this.toolStripSeparator1,
            this.manageToolStripMenuItem});
			this.contextMenuImps.Name = "contextMenuLayers";
			this.contextMenuImps.Size = new System.Drawing.Size(139, 76);
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
			this.groupBox3.Controls.Add(this.listBoxImps);
			this.groupBox3.Location = new System.Drawing.Point(264, 3);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(245, 255);
			this.groupBox3.TabIndex = 0;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Images Archive";
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
			// listBoxImps
			// 
			this.listBoxImps.ContextMenuStrip = this.contextMenuImps;
			this.listBoxImps.FormattingEnabled = true;
			this.listBoxImps.Location = new System.Drawing.Point(6, 19);
			this.listBoxImps.Name = "listBoxImps";
			this.listBoxImps.Size = new System.Drawing.Size(233, 173);
			this.listBoxImps.TabIndex = 0;
			this.listBoxImps.SelectedIndexChanged += new System.EventHandler(this.listBoxImps_SelectedIndexChanged);
			// 
			// IMAControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.labelData);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.pictureTileset);
			this.Controls.Add(this.groupBox1);
			this.Name = "IMAControl";
			this.Size = new System.Drawing.Size(512, 512);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picturePalette)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureTileset)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.contextMenuImps.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
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
		private System.Windows.Forms.ContextMenuStrip contextMenuImps;
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
		private System.Windows.Forms.ListBox listBoxImps;
	}
}
