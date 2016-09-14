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
        private int number_files_to_upload = 0;
        private int files_uploaded = 0;
        private IniFile ini_file;
        private bool rip_manual_ = false;

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
        SessionOptions sessionOptions;
        Session session;
        TransferOperationResult transferResult;
        TransferOptions transferOptions;

        // Get List of files in a directory recursively

        Console.Write("FromPath " + frompath + " topath " + topath);

            foreach (string d in Directory.GetDirectories(frompath))
            {
                foreach (string f in Directory.GetFiles(d))
                {
                    this.Invoke(new Action<string>(progressLabel_ChangeText), "Sending " + f);

                    Console.Write("\nUploading " + f + " -> " + topath + f.Replace(@"c:\temp\data\", "").Replace(@"\", "/") + "\n");

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

                            session.Timeout = new TimeSpan(0, 3, 30);

                            // Connect
                            session.Open(sessionOptions);

                            transferOptions = new TransferOptions();
                            transferOptions.TransferMode = TransferMode.Binary;

                            transferResult = session.PutFiles(f, topath + f.Replace(@"c:\temp\data\", "").Replace(@"\", "/"), true, transferOptions);
                            // Throw on any error
                            transferResult.Check();

                            this.Invoke(new Action<string>(statusLabel_ChangeText), "Installing...");

                            // Print results
                            foreach (TransferEventArgs transfer in transferResult.Transfers)
                            {
                                Console.WriteLine("\nUpload of {0} succeeded", transfer.FileName);
                                this.Invoke(new Action<int>(numberFilesUploaded_Change), getNumberFilesUploaded() + 1);                                
                                this.Invoke(new Action<int>(progressBar1_ChangeValue), (100 / number_files_to_upload) * files_uploaded);
                            }

                            session.Close();
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: {0}", ex);
                        Console.WriteLine("Error: {0}", ex);
                        this.Invoke(new Action<string>(statusLabel_ChangeText), @"ERROR");
                        this.Invoke(new Action<int>(progressBar1_ChangeValue), 0);
                        this.Invoke(new Action<bool>(button2_ChangeState), true);
                        this.Invoke(new Action<string>(progressLabel_ChangeText), @"");
                        MessageBox.Show(ex.Message, "error");

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
                    this.Invoke(new Action<int>(numberFilestoUpload_Change), getNumberFilesToUpload() + 1);
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

                DialogResult dialogResult = MessageBox.Show("Do you want to rip the game manual?", "Manual Rip", MessageBoxButtons.YesNo);

                rip_manual_ = false;

                if (dialogResult == DialogResult.Yes)
                {
                    rip_manual_ = true;
                }

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
                        if (rip_manual_ && file_.Key.Contains("manual"))
                        {
                            Console.Write("\nManual File Ripped");
                        } else
                        {
                            file_.WriteToDirectory(@"C:\temp\install", ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                        }
                        
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

            Thread t = new Thread(new ThreadStart(Install));
            t.Start();

        }

        public void statusLabel_ChangeText(string text)
        {
            statusLabel.Text = text;
        }

        public void progressBar2_ChangeValue(int value)
        {
            progressBar2.Value = value;
        }

        public void progressLabel_ChangeText(string text)
        {
            progressLabel.Text = text;
        }

        public void progressBar1_ChangeValue(int value)
        {
            progressBar1.Value = value;
        }

        public void button1_ChangeState(bool state)
        {
            button1.Enabled = state;
        }

        public void button2_ChangeState(bool state)
        {
            button2.Enabled = state;
        }

        public void numberFilesUploaded_Change(int value)
        {
            files_uploaded = value;
        }

        public void numberFilestoUpload_Change(int value)
        {
            number_files_to_upload = value;
        }

        public int getNumberFilesToUpload()
        {
            return number_files_to_upload;
        }

        public int getNumberFilesUploaded()
        {
            return files_uploaded;
        }

        private void Install()
        {
            SessionOptions sessionOptions;
            Session session;
            TransferOperationResult transferResult;
            TransferOptions transferOptions;
            // CREATE VPK
            this.Invoke(new Action<string>(statusLabel_ChangeText), @"Creating .vpk...");
            this.Invoke(new Action<bool>(button2_ChangeState), false);
            this.Invoke(new Action<bool>(button1_ChangeState), false);

            using (var archive = ZipArchive.Create())
            {
                archive.AddAllFromDirectory(@"C:\temp\install");
                archive.SaveTo(@"C:\\temp\\installer.vpk", CompressionType.None);
            }

            this.Invoke(new Action<string>(statusLabel_ChangeText), @"VPK Created");
            this.Invoke(new Action<int>(progressBar1_ChangeValue), 25);            

            System.Threading.Thread.Sleep(1000);

            // TRANSFER VPK

            this.Invoke(new Action<string>(statusLabel_ChangeText), @"Connecting...");
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
                    session.Timeout = TimeSpan.FromSeconds(120000);

                    // Show Log
                    //var t = new Thread(new ThreadStart(this.ShowLog));
                    //t.Start();

                    this.Invoke(new Action<string>(statusLabel_ChangeText), @"Connected");
                    // Upload files
                    transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    this.Invoke(new Action<string>(statusLabel_ChangeText), @"Copying Installer...");
                    transferResult = session.PutFiles(@"c:\temp\installer.vpk", "ux0:/VPKs/", true, transferOptions);

                    // Throw on any error
                    transferResult.Check();
                    
                    // Print results
                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {
                        Console.WriteLine("Upload of {0} succeeded", transfer.FileName);

                    }
                    this.Invoke(new Action<int>(progressBar1_ChangeValue), 50);

                    this.Invoke(new Action<string>(statusLabel_ChangeText), @"Installing VPK...");

                    // Auto-install VPK
                    Console.Write("\n PROMOTING INSTALLER.VPK! \n");
                    session.ExecuteCommand("PROM ux0:/VPKs/installer.vpk");

                    
                    session.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex);
                this.Invoke(new Action<string>(statusLabel_ChangeText), @"ERROR");
                this.Invoke(new Action<int>(progressBar1_ChangeValue), 0);
                this.Invoke(new Action<bool>(button2_ChangeState), true);
                this.Invoke(new Action<string>(progressLabel_ChangeText), @"");
                this.Invoke(new Action<bool>(button1_ChangeState), true);
                MessageBox.Show(ex.Message, "error");

            }

            // UPLOAD DATA
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
                    session.Open(sessionOptions);
                    bool vpkinstalled = false;

                    while (!vpkinstalled)
                    {
                        if (session.FileExists("ux0:/app/" + param.TitleID + "/eboot.bin"))
                        {
                            vpkinstalled = true;
                        }
                    }

                    Console.Write("\n VPK Installed \n");
                    
                    session.RemoveFiles("ux0:/VPKs/installer.vpk");
                   
                    if (Directory.Exists("C:\\temp\\install")) {
                        Console.Write("\nTemp install directory deleted");
                        Directory.Delete(@"C:/temp/install", true);
                    }

                    session.Close();

                }
            }
            catch (Exception ex)
            {                
                Console.Write("\n ERROR UPLOADING DATA \n");
                Console.WriteLine("Error: {0}", ex);
                this.Invoke(new Action<string>(statusLabel_ChangeText), @"ERROR");
                this.Invoke(new Action<int>(progressBar1_ChangeValue), 0);
                this.Invoke(new Action<bool>(button2_ChangeState), true);
                this.Invoke(new Action<string>(progressLabel_ChangeText), @"");
                this.Invoke(new Action<bool>(button1_ChangeState), true);
                MessageBox.Show(ex.Message, "error");
            }

            string apppath = "/ux0:/app/" + param.TitleID + "/";

            this.Invoke(new Action<int>(numberFilesUploaded_Change), 0);
            this.Invoke(new Action<int>(numberFilestoUpload_Change), 0);
            
            this.Invoke(new Action<int>(progressBar1_ChangeValue), 0);

            CountNumberFilesInPath(@"c:\temp\");
            SendFilesFromDirectory(@"c:\temp\", apppath, number_files_to_upload);

            MessageBox.Show(param.Title + " installed successfully.", "Congrats!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Invoke(new Action<int>(progressBar1_ChangeValue), 100);
            this.Invoke(new Action<string>(statusLabel_ChangeText), @"Completed");
            this.Invoke(new Action<bool>(button2_ChangeState), true);
            this.Invoke(new Action<bool>(button1_ChangeState), true);

            // Clean temp directory
            Directory.Delete(@"C:/temp", true);

            this.Invoke(new Action<string>(progressLabel_ChangeText), @"");



        }
        
        private void SessionFileTransferProgress(object sender, FileTransferProgressEventArgs e)
        {
            // Print transfer progress           
            this.Invoke(new Action<int>(progressBar2_ChangeValue), (int)(e.FileProgress * 100));
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Donation Button

            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=RMFDRTBU49E8E");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=RMFDRTBU49E8E");

        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Remove temp data
            if (Directory.Exists(@"C:/temp"))
            {
                Console.Write("Temp directory deleted");
                Directory.Delete(@"C:/temp", true);
            }
        }
    }
}

