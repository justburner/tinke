
namespace DB_KAI_RPG
{
	partial class BSTLayerCode
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.numericCode = new System.Windows.Forms.NumericUpDown();
			this.labelCode = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numericCode)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Location = new System.Drawing.Point(11, 46);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "&OK";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(92, 46);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 3;
			this.button2.Text = "&Cancel";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// numericCode
			// 
			this.numericCode.Location = new System.Drawing.Point(11, 12);
			this.numericCode.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.numericCode.Name = "numericCode";
			this.numericCode.Size = new System.Drawing.Size(75, 20);
			this.numericCode.TabIndex = 1;
			this.numericCode.ValueChanged += new System.EventHandler(this.numericCode_ValueChanged);
			// 
			// labelCode
			// 
			this.labelCode.Location = new System.Drawing.Point(89, 14);
			this.labelCode.Name = "labelCode";
			this.labelCode.Size = new System.Drawing.Size(78, 18);
			this.labelCode.TabIndex = 0;
			this.labelCode.Text = "0000";
			this.labelCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// BSTLayerCode
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(179, 81);
			this.Controls.Add(this.labelCode);
			this.Controls.Add(this.numericCode);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BSTLayerCode";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Layer Code";
			((System.ComponentModel.ISupportInitialize)(this.numericCode)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.NumericUpDown numericCode;
		private System.Windows.Forms.Label labelCode;
	}
}