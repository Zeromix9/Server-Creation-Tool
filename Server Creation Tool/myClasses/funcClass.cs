using Ionic.Zip;
using Server_Creation_Tool.myClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace statServer_Creation_Tool
{

    public class funcClass
    {
        global_Variables gVars = new global_Variables();
        public StringBuilder sb = new StringBuilder();
        public bool HasInternet()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            { return false; }
        }
        public bool check4Updates(string updtCheckURL, int currentVer)
        {
            var url = updtCheckURL;
            string textFromFile;
            try { textFromFile = (new WebClient()).DownloadString(url); }
            catch (Exception a)
            { log.Append(a.ToString()); return false; }
            int latestVer = int.Parse(textFromFile);
            if (latestVer > currentVer)
            { return true; }
            else
            { return false; }
        }
        public string[] GetArrayStrsLastIndexes(string[] str)
        {
            List<string> list = new List<string>();
            foreach (string s in str)
            {
                string subS = s.Substring(s.LastIndexOf(@"\") + 1);
                list.Add(subS);
            }
            return list.ToArray();
        }
        public string extractFileName(string path)
        {
            return path.Substring(path.LastIndexOf(@"\") + 1);
        }
        public double ShowFolderSize(string path)
        {
            Type GetFoldType = Type.GetTypeFromProgID("Scripting.FileSystemObject");
            dynamic GetFoldInst = Activator.CreateInstance(GetFoldType);
            double result;
            try
            {
                if (System.IO.Directory.Exists(path))
                {
                    var f = GetFoldInst.GetFolder(path);
                    //var convrt = ByteSize.FromBytes(f.size);
                    // convrt.MegaBytes;
                    result = f.size;
                }
                else { result = 0; }
            }
            catch (Exception e) { log.Append(e.ToString()); return 0; }
            return result;
        }
        public bool writeTxtFile(string path, string content)
        {
            try
            { File.WriteAllText(path, content); return true; }
            catch (Exception a)
            {
                log.Append(a.ToString()); MessageBox.Show(path);
                return false;
            }
        }
        public bool downloadCompleted = false;
        public async Task downloadnWriteFile(string url, string downloadPath)
        {
            downloadCompleted = false;
            WebClient client = new WebClient();
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
            client.DownloadFileAsync(new Uri(url), downloadPath);
        }
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            downloadCompleted = true;
        }
        public string lg(string[] stringArray, int pos = 0)
        {
            if (stringArray.Length == 2)
            { return stringArray[gVars.lgNum]; }
            else if (stringArray.Length == 4)
            { return stringArray[gVars.lgNumQuad + pos]; }
            return "Missing word";
        }
        public int getLangNum()
        {
            int lang = 0;
            switch (Server_Creation_Tool.Properties.Settings.Default.language)
            {
                case "english":
                    lang = 0;
                    break;
                case "german":
                    lang = 1;
                    break;
            }
            return lang;
        }
        public void RemoveClickEvent(Button b)
        {
            FieldInfo f1 = typeof(Control).GetField("EventClick",
                BindingFlags.Static | BindingFlags.NonPublic);

            object obj = f1.GetValue(b);
            PropertyInfo pi = b.GetType().GetProperty("Events",
                BindingFlags.NonPublic | BindingFlags.Instance);

            EventHandlerList list = (EventHandlerList)pi.GetValue(b, null);
            list.RemoveHandler(obj, list[obj]);
        }

        public bool steamCMDDownloadDone = false;
        public async Task downloadSteamCMD(string downloadPath)
        {
            steamCMDDownloadDone = false;
            await downloadnWriteFile(gVars.steamCMDUrl, downloadPath + "\\steamcmd.zip");
            while (downloadCompleted == false) await Task.Delay(50);//wait till  the download is complete
            //Extact the zip file
            if (unzipFileAndDel(downloadPath + "\\steamcmd.zip", downloadPath) == true)
            { steamCMDDownloadDone = true; }
            else
            { steamCMDDownloadDone = false; }
        }
        public bool unzipFileAndDel(string ZipPath, string extractPath)
        {
            //Extact the zip file
            try
            {
                using (Ionic.Zip.ZipFile zip1 = Ionic.Zip.ZipFile.Read(ZipPath))
                {
                    foreach (ZipEntry e in zip1)
                    { e.Extract(extractPath, ExtractExistingFileAction.OverwriteSilently); }
                }
            }
            catch (Exception a)
            { log.Append(a.ToString()); return false; }
            //Delete the zip file
            try
            { File.Delete(ZipPath); return true; }
            catch { return true; }
        }
        public Thread StartThread(Action action)
        {
            Thread thread = new Thread(() => { action(); });
            thread.Start();
            return thread;
        }
        public void InvokeIfRequired(Control control, MethodInvoker action)
        {
            if (control.InvokeRequired)
            { control.Invoke(action); }
            else
            { action(); }
        }
        public void showMenuStripAtBtn(ContextMenuStrip menuStrip, Button btn)
        {
            menuStrip.Show(btn, new Point(btn.Width - btn.Width, btn.Height));
        }

        public string getVarByStr(string varName)//this method can access a variable using its name as a dynamic string (used for strings)
        {
            return gVars.GetType().GetField(varName).GetValue(gVars) as string;
        }
        public string[] getArVarByStr(string varName)//this method can access a variable using its name as a dynamic string (used for string arrays)
        {
            if (gVars.GetType().GetField(varName) != null)
            { return gVars.GetType().GetField(varName).GetValue(gVars) as string[]; }
            return new string[] { };
        }
        public void saveFavList(List<string> str)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, str);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                Server_Creation_Tool.Properties.Settings.Default.favServers = Convert.ToBase64String(buffer);
                Server_Creation_Tool.Properties.Settings.Default.Save();
            }
        }

        public List<string> loadFavList()
        {
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Server_Creation_Tool.Properties.Settings.Default.favServers)))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (List<string>)bf.Deserialize(ms);
            }
        }
        public string getAppData()
        {
            string appdt = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return appdt.Substring(0, appdt.LastIndexOf(@"\"));
        }
    }
}
