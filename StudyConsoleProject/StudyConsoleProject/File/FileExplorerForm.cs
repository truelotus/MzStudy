using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace StudyConsoleProject.File
{
    public partial class FileExplorerForm : Form
    {
        public FileExplorerForm()
        {
            InitializeComponent();

            this.Load += new EventHandler(FileExplorerForm_Load);

            this.listView1.MouseDoubleClick += new MouseEventHandler(listView1_MouseDoubleClick);

            this.treeView1.BeforeExpand += new TreeViewCancelEventHandler(treeView1_BeforeExpand);
            this.treeView1.AfterSelect += new TreeViewEventHandler(treeView1_AfterSelect);
        }

        void FileExplorerForm_Load(object sender, EventArgs e)
        {

            foreach (DriveInfo drv in DriveInfo.GetDrives())
            {
                if (drv.IsReady)
                {
                    TreeNode treeNode = new TreeNode();
                    treeNode.Text = drv.Name;
                    treeNode.Nodes.Add("first");
                    treeView1.Nodes.Add(treeNode);
                }
            }
        }

        void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode current = e.Node;
                string path = current.FullPath;
                string[] Files = Directory.GetFiles(path);
                string[] Directories = Directory.GetDirectories(path);

                string[] subinfo = new string[3];

                listView1.Clear();

                listView1.Columns.Add("Name", 255);

                foreach (string name in Directories)
                {
                    subinfo[0] = GetFileName(name);
                    subinfo[1] = "";
                    subinfo[2] = "FOLDER";
                    ListViewItem Items = new ListViewItem(subinfo);
                    listView1.Items.Add(Items);
                }

                foreach (string filePath in Files)
                {
                    subinfo[0] = GetFileName(filePath);
                    ListViewItem Items = new ListViewItem(subinfo);
                    listView1.Items.Add(Items);
                    this.textBox1.Text = filePath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!!");
            }
        }

        void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                TreeNode parentnode = e.Node;
                DirectoryInfo dr = new DirectoryInfo(parentnode.FullPath);
                parentnode.Nodes.Clear();
                foreach (DirectoryInfo dir in dr.GetDirectories())
                {
                    TreeNode node = new TreeNode();
                    node.Text = dir.Name;
                    node.Nodes.Add("");
                    parentnode.Nodes.Add(node);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!!");
            }
        }

        void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine(sender.ToString());
        }

        public string GetFileName(string path)
        {
            int Nameindex = path.LastIndexOf('\\');
            return path.Substring(Nameindex + 1);
        }
    }
}
