using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace VCXProjRename
{
	public class VcxProjRenameList :ListBox
	{
		private string m_dir = "";
		private string m_target = "";
		public string Target
		{
			get { return m_target; }
		}
		public string Dir
		{
			get { return m_dir; }
		}
		private List<string> m_list = new List<string>();
		private List<string> m_dirlist = new List<string>();
		private string[] m_Exts = new string[]
		{
			".sln",
			".cpp",
			".h",
			".r",
			".rc",
			".vcxproj",
			".filters",
			".user"
		};
		private string[] m_IgDirs = new string[]
		{
			"Debug",
			"Release",
			"x64",
			"x32",
			"bin"
		};
		public VcxProjRenameList()
		{

		}
		public bool Exec(string n)
		{
			bool ret = false;
			n = n.Trim();
			if((m_target==n)||(n==""))return ret;

			if(m_list.Count>0)
			{
				foreach(string s in m_list)
				{
					Encoding enc = Encodechecker.GetJpEncoding(s);

					string str = File.ReadAllText(s,enc);
					str = str.Replace(m_target,n);
					File.WriteAllText(s,str,enc);
					string p = Path.GetDirectoryName(s);
					string nn = Path.GetFileName(s).Replace(m_target, n);
					 p = Path.Combine(p,nn);
					if (s != p)
					{
						File.Move(s, p);
					}
				}
			}
			if(m_dirlist.Count>0)
			{
				for(int i= m_dirlist.Count-1;i>=0;i--)
				{
					string n1 = m_dirlist[i];
					string p1 = Path.GetDirectoryName(n1);
					string nn = Path.GetFileName(n1).Replace(m_target, n);
					p1 = Path.Combine(p1, nn);
					if (p1 != n1)
					{
						Directory.Move(n1, p1);
						m_dirlist[i] = p1;
					}
				}
			}
			Listup(m_dirlist[0]);

			return ret;
		}
		private bool IsTargetExt(string p)
		{
			bool ret = false;
			p = p.ToLower();
			for(int i = 0; i < m_Exts.Length;i++)
			{
				if (p == m_Exts[i])
				{
					ret = true;
					break;
				}
			}
			return ret;
		}
		private bool IsIgDir(string p)
		{
			bool ret = false;
			for (int i = 0; i < m_IgDirs.Length; i++)
			{
				if (p == m_IgDirs[i])
				{
					ret = true;
					break;
				}
			}
			return ret;
		}
		public bool Listup(string p)
		{
			m_dir = "";
			m_target = "";
			m_list.Clear();
			m_dirlist.Clear();

			this.Items.Clear();

			bool ret = false;
			if(File.Exists(p))
			{
				p = Path.GetDirectoryName(p);
				ret = true;
			}else if(Directory.Exists(p))
			{
				ret =true;
			}
			
			if (ret == false) return ret;
			ListupSub(p);
			if(m_list.Count>0)
			{
				if(m_target!="")
				{
					List<string> list = new List<string>();
					foreach (string s in m_list)
					{
						string s2 = s.Replace(p+"\\", "");
						list.Add(s2);
					}
					this.Items.AddRange(list.ToArray());
					m_dir = p;
					ret = true;
				}
				else
				{
					m_list.Clear();
				}
			}
			return ret;
		}
		private void ListupSub(string p)
		{
			if(Directory.Exists(p)==false) {return;}

			string[] fs = Directory.GetFiles(p,"*.*");
			if (fs.Length>0)
			{
				int cnt = 0;
				foreach (string s in fs)
				{
					string e = Path.GetExtension(s).ToLower();
					if (IsTargetExt(e))
					{
						if (m_target=="")
						{
							if ((e == ".vcxproj") || (e == ".sln"))
							{
								m_target = Path.GetFileNameWithoutExtension(s);
							}
						}
						m_list.Add(s);
						cnt++;
					}
				}
				if (cnt>0)
				{
					m_dirlist.Add(p);
				}
			}
			string[] ds = Directory.GetDirectories(p ,"*.*");
			if (ds.Length > 0)
			{
				foreach (string s in ds)
				{
					string n =Path.GetFileName(s);
					if (IsIgDir(n)==false)
					{
						ListupSub(s);
					}
				}
			}
		}

		
	}
}
