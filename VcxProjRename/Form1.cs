using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace VCXProjRename
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			AllowDrop = true;
		}
		protected override void OnDragEnter(DragEventArgs drgevent)
		{
			if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
				drgevent.Effect = DragDropEffects.Copy;
			else
				drgevent.Effect = DragDropEffects.None;
			base.OnDragEnter(drgevent);
		}
		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			string[] fileNames =
				(string[])drgevent.Data.GetData(DataFormats.FileDrop, false);
			if((fileNames != null )&&(fileNames.Length>0))
			{
				foreach (string file in fileNames)
				{
					if (slnRenameList1.Listup(file))
					{
						tbTarget.Text = slnRenameList1.Target;
						tbNew.Text = slnRenameList1.Target;
						break;
					}
				}
			}
			base.OnDragDrop(drgevent);
		}

		private void btnExec_Click(object sender, EventArgs e)
		{
			slnRenameList1.Exec(tbNew.Text);
			tbTarget.Text = slnRenameList1.Target;
			tbNew.Text = slnRenameList1.Target;
		}
	}
}
