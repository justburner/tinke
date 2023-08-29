using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_KAI_RPG
{
	public partial class BSTLayerCode : Form
	{
		public BSTLayerCode()
		{
			InitializeComponent();
		}

		public void SetCode(ushort code)
		{
			numericCode.Value = code;
		}

		public ushort GetCode()
		{
			return (ushort)numericCode.Value;
		}

		private void numericCode_ValueChanged(object sender, EventArgs e)
		{
			int code = (int)numericCode.Value;
			labelCode.Text = string.Format("{0:X04}", code);
		}
	}
}
