using System;
using System.Windows.Forms;
using libfintx;

namespace WinformsTest
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void btnHKSAL_Click(object sender, EventArgs e)
		{
			textBox1.Text = HKSAL.Init_HKSAL(Test.GetConnectionDetails());
			
		}
	}
}
