using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server_creation_tool.classes
{
    internal class fileDownloader
    {
        public fileDownloader(ProgressBar bar = null, Label percentLabel = null)
        {
            progressBar = bar;
            label = percentLabel;
        }
        ProgressBar progressBar = null;
        Label label = null;
        funcsClass funcs = new funcsClass();
        public bool failed = false;

        public bool downloadCompleted = false;
        public async Task download(string url, string downloadPath, bool makeFolderIfMissing = false)
        {

            if (!Directory.Exists(Path.GetDirectoryName(downloadPath)))
            {
                if (makeFolderIfMissing)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(downloadPath));
                    Debug.WriteLine(Path.GetDirectoryName(downloadPath));
                }
                else
                {
                    failed = true;
                }
            }

            downloadCompleted = false;
            WebClient client = new WebClient();
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileAsync(new Uri(url), downloadPath);

            //create timeout timer
            System.Timers.Timer aTimer = new System.Timers.Timer();
            System.Timers.ElapsedEventHandler handler = null;
            handler = ((sender, args) =>
            {
                if (downloadProgPercent[0] == -1)
                {
                    aTimer.Elapsed -= handler;
                    failed = true;
                    client.CancelAsync();
                }
            });
            aTimer.Elapsed += handler;
            aTimer.Interval = 10000;
            aTimer.Enabled = true;
        }
        public int[] downloadProgPercent = new int[] { -1 };
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {

            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            downloadProgPercent = new int[] { int.Parse(Math.Truncate(percentage).ToString()) };
            if (progressBar != null)
            {
                try
                {
                    funcs.InvokeIfRequired(progressBar, () =>
                    { progressBar.Value = downloadProgPercent[0]; });
                }
                catch { }
            }
            if (label != null)
            {
                funcs.InvokeIfRequired(label, () =>
                { label.Text = downloadProgPercent[0] + "%"; });

            }
            //may add bytes of file in the future
        }
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            downloadCompleted = true;
        }
    }
}
