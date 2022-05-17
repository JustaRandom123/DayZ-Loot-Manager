using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LootManager
{
    public partial class mapDownloader : Form
    {
        int counter = 5;
        public mapDownloader()
        {
            InitializeComponent();
            FormClosing += ClosingForm;
        }

        private void ClosingForm(Object sender, EventArgs e)
        {
            this.Hide();
            Form1 frm = new Form1();
            frm.Show();
        }

        private void mapDownloader_Load(object sender, EventArgs e)
        {
            timer1.Start();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (counter != 0)
            {
                label3.Text = "Starting in: " + counter.ToString();
                counter--;
            }
            else
            {
                timer1.Stop();
                timer1.Enabled = false;

                using (WebClient wc = new WebClient())
                {
                    wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                    wc.DownloadFileCompleted += wc_DownloadFileCompleted;
                    wc.DownloadFileAsync(new Uri("http://185.223.31.43/dayzlootmanager/SpawnCreater/ChernarusHighQuality.jpg"), Application.StartupPath + "\\ChernarusHighQuality.jpg");
                }
            }
        }

        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label3.Text = progressBar1.Value + " %";
            long totalbytes = e.TotalBytesToReceive / 1024 / 1024;
            long totalbytesKB = e.TotalBytesToReceive / 1024;
            long bytes = e.BytesReceived / 1024 / 1024;
            long bytesKB = e.BytesReceived / 1024;
            if (e.BytesReceived >= 999)
            {
                label2.Text = bytes.ToString() + " / " + totalbytes.ToString() + " MB ";
            }
            else
            {
                label2.Text = bytesKB.ToString() + " / " + totalbytesKB.ToString() + " KB ";
            }
        }

        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Download Complete!");
            LootManager.Properties.Settings.Default.chernarusMapDownloaded = true;
            LootManager.Properties.Settings.Default.Save();
            this.Hide();
            PlayerspawnCreater frm = new PlayerspawnCreater();
            frm.Show();
        }
    }
}
