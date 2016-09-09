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

namespace ftpvitainstaller_c_sharp
{
    public partial class LogForm : Form
    {

        public LogForm()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void LogForm_Load(object sender, EventArgs e)
        {
            FileSystemWatcher watcher = new FileSystemWatcher(@"C:\temp\");
            watcher.Filter = "ftp_log.txt";
            watcher.EnableRaisingEvents = true;
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
            watcher.Changed += new FileSystemEventHandler(watcher_Changed);
        }

        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath == @"C:\temp\ftp_log.txt")
            {
                Console.Write("\nLog Updated\n");
            }
        }

    }
}
