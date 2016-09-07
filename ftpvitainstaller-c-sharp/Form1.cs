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


namespace ftpvitainstaller_c_sharp
{
    public partial class Form1 : Form
    {

        private PARAM_SFO param;

        public Form1()
        {
            InitializeComponent();
            progressBar1.Value = 0;
            button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
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
                statusLabel.Text = "Reading File...";
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
            this.button2.Enabled = false;
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
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = ipBox.Text,
                    PortNumber = Convert.ToInt32(portBox.Text),
                    UserName = "anonymous",
                    Password = ""
                };

                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);
                    statusLabel.Text = "Connected";
                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
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
                        // Transfer data files
                        statusLabel.Text = "Installing, please wait...";
                        progressBar1.Value = 75;
                        transferResult = session.PutFiles(@"c:\temp\data\*", "ux0:/app/" + param.TitleID + "/", false, transferOptions);

                        // Throw on any error
                        transferResult.Check();

                        // Print results
                        foreach (TransferEventArgs transfer in transferResult.Transfers)
                        {
                            Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
                            
                            
                        }
                        MessageBox.Show(param.Title + " installed successfully.", "Congrats!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        progressBar1.Value = 100;
                        statusLabel.Text = "Completed!";
                        button2.Enabled = true;

                        // Clean temp directory
                        Directory.Delete(@"C:/temp", true);
                    }
                    else
                    {
                        button2.Enabled = true;
                        statusLabel.Text = "Installation aborted.";
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
            }
        }

    }
}

