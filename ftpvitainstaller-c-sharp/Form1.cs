using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using SharpCompress.Common;
using SharpCompress.Reader;
using SharpCompress.Archive;
using SharpCompress.Archive.Zip;
using WinSCP;
using PeXploit;
using System.IO;
using MadMilkman.Ini;


namespace ftpvitainstaller_c_sharp
{
    public partial class Form1 : Form
    {

        private PARAM_SFO param;
        private SessionOptions sessionOptions;
        private Session session;
        private TransferOperationResult transferResult;
        private TransferOptions transferOptions;
        private int number_files_to_upload = 0;
        private int files_uploaded = 0;
        private IniFile ini_file;

        public Form1()
        {
            InitializeComponent();
            progressBar1.Value = 0;
            button2.Enabled = false;

            // Load config.ini

            if (!File.Exists(@"config.ini"))
            {                
                ini_file = new IniFile();
                IniSection section = ini_file.Sections.Add("Configuration");
                IniKey key = section.Keys.Add("IP", "192.168.1.");
                IniKey keyport = section.Keys.Add("Port", "1337");
                ini_file.Save("config.ini");
            } else
            {
                ini_file = new IniFile();
                ini_file.Load(@"config.ini");
            }

            ipBox.Text = ini_file.Sections["Configuration"].Keys["IP"].Value;
            portBox.Text = ini_file.Sections["Configuration"].Keys["Port"].Value;  

        }

        private void SendFilesFromDirectory(string frompath, string topath, int num_files)
        {

            // Get List of files in a directory recursively

            foreach (string d in Directory.GetDirectories(frompath))
            {
                foreach (string f in Directory.GetFiles(d))
                {
                    progressLabel.Text = "Sending " + f;

                    transferResult = session.PutFiles(f, topath, false, transferOptions);
                    
                    // Throw on any error
                    transferResult.Check();

                    // Print results
                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {
                        Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
                        files_uploaded++;
                        progressBar1.Value = (100 / number_files_to_upload) * files_uploaded;
                    }

                }
                SendFilesFromDirectory(d, topath, number_files_to_upload);
            }
        }

        private void CountNumberFilesInPath(string path)
        {

            // Get List of files in a directory recursively

            foreach (string d in Directory.GetDirectories(path))
            {
                foreach (string f in Directory.GetFiles(d))
                {                    
                    number_files_to_upload++;

                }
                CountNumberFilesInPath(d);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            statusLabel.Text = "Reading File...";
            if (Directory.Exists(@"C:/temp"))
            {
                Console.Write("Temp directory deleted");
                Directory.Delete(@"C:/temp", true);
            }
            openFileDialog1.Title = "Open Game...";
            openFileDialog1.Filter = "VPK Files |*.vpk|Zip Files |*.zip";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                gameNameLabel.Text = openFileDialog1.FileName;
                bool isvpk = false;
                if (openFileDialog1.FileName.Contains(".vpk"))
                {
                    isvpk = true;
                    File.Move(openFileDialog1.FileName, Path.ChangeExtension(openFileDialog1.FileName, ".zip"));
                    openFileDialog1.FileName = Path.ChangeExtension(openFileDialog1.FileName, ".zip");
                }
                var archive = ArchiveFactory.Open(openFileDialog1.FileName);
                fileSizeLabel.Text = "File Size: " + archive.TotalSize + " bytes";
                
                foreach (var file_ in archive.Entries)
                {

                    if (file_.Key.Contains("sce_sys") || file_.Key == "eboot.bin")
                    {
                        file_.WriteToDirectory(@"C:\temp\install", ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                    }
                    else
                    {
                        file_.WriteToDirectory(@"C:\temp\data", ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                    }

                }

                archive.Dispose();
                
                pictureBox1.ImageLocation = @"C:\temp\install\sce_sys\pic0.png";
                statusLabel.Text = "Reading SFO...";

                param = new PARAM_SFO(@"C:\temp\install\sce_sys\param.sfo");

                gameNameLabel.Text = param.Title;

                statusLabel.Text = "Game Loaded";
                button2.Enabled = true;

                if (isvpk)
                {                    
                    File.Move(openFileDialog1.FileName, Path.ChangeExtension(openFileDialog1.FileName, ".vpk"));
                }
            }            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Install();

        }

        private void Install()
        {
            // CREATE VPK
            statusLabel.Text = @"Creating .vpk...";

            using (var archive = ZipArchive.Create())
            {
                archive.AddAllFromDirectory(@"C:\temp\install");
                archive.SaveTo(@"C:\\temp\\installer.vpk", CompressionType.None);
            }

            statusLabel.Text = @"VPK Created";
            progressBar1.Value = 25;

            System.Threading.Thread.Sleep(1000);

            // TRANSFER VPK

            statusLabel.Text = "Connecting...";
            try
            {
                // Setup session options
                sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = ipBox.Text,
                    PortNumber = Convert.ToInt32(portBox.Text),
                    UserName = "anonymous",
                    Password = ""
                };

                using (session = new Session())
                {

                    session.FileTransferProgress += SessionFileTransferProgress;

                    // Connect
                    session.Open(sessionOptions);

                    // Show Log
                    //var t = new Thread(new ThreadStart(this.ShowLog));
                    //t.Start();

                    statusLabel.Text = "Connected";
                    // Upload files
                    transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;
                                      
                    statusLabel.Text = "Copying installer...";
                    transferResult = session.PutFiles(@"c:\temp\installer.vpk", "ux0:/VPKs/", false, transferOptions);

                    // Throw on any error
                    transferResult.Check();
                    
                    // Print results
                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {
                        Console.WriteLine("Upload of {0} succeeded", transfer.FileName);

                    }
                    statusLabel.Text = "Install the VPK at ux0:/VPKs/";
                    progressBar1.Value = 50;
                    DialogResult result;
                    result = MessageBox.Show("Please go to ux0:/VPKs on the PSVita and install the installer.vpk file, then click OK. \n\nATTENTION! ONLY press OK when the installation is complete.", "Installer", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {

                        session.RemoveFiles("ux0:/VPKs/installer.vpk");

                        // Transfer data files
                        statusLabel.Text = "Installing, please wait...";
                        progressBar1.Value = 75;

                        string apppath = "ux0:/app/" + param.TitleID + "/";

                        files_uploaded = 0;
                        number_files_to_upload = 0;

                        progressBar1.Value = 0;
                        CountNumberFilesInPath(@"c:\temp\data\");            
                        SendFilesFromDirectory(@"c:\temp\data\", apppath, number_files_to_upload);
                       
                        MessageBox.Show(param.Title + " installed successfully.", "Congrats!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        progressBar1.Value = 100;
                        statusLabel.Text = "Completed!";
                        button2.Enabled = true;

                        // Clean temp directory
                        Directory.Delete(@"C:/temp", true);

                        progressLabel.Text = "";


                    }
                    else
                    {
                        button2.Enabled = true;
                        statusLabel.Text = "Installation aborted.";
                        if (Directory.Exists(@"C:/temp"))
                        {
                            Console.Write("Temp directory deleted");
                            Directory.Delete(@"C:/temp", true);
                        }
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex);
                statusLabel.Text = "CONNECTION ERROR";
                progressBar1.Value = 0;
                button2.Enabled = true;
                progressLabel.Text = "";
                if (Directory.Exists(@"C:/temp"))
                {
                    Console.Write("Temp directory deleted");
                    Directory.Delete(@"C:/temp", true);
                }
            }
        }

        private void SessionFileTransferProgress(object sender, FileTransferProgressEventArgs e)
        {
            // Print transfer progress            
            progressBar2.Value = (int)(e.FileProgress * 100);
            Console.Write("\r{0} ({1})", e.FileName, e.FileProgress);

        }

        private void ShowLog()
        {
            LogForm logf = new LogForm();
            logf.Show();
        }

        private void ipBox_TextChanged(object sender, EventArgs e)
        {
            ini_file.Sections["Configuration"].Keys["IP"].Value = ipBox.Text;
            ini_file.Save("config.ini");
        }

        private void portBox_TextChanged(object sender, EventArgs e)
        {
            ini_file.Sections["Configuration"].Keys["Port"].Value = portBox.Text;
            ini_file.Save("config.ini");
        }
    }
}

