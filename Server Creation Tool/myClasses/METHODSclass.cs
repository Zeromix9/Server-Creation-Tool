using ByteSizeLib;
using Server_Creation_Tool.myClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace statServer_Creation_Tool
{

    public class METHODSandFUNCTIONS
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
        public string askUserInstallDir()
        {
            // Create a new instance of FolderBrowserDialog.
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            //Check Language
            if (Server_Creation_Tool.Properties.Settings.Default.language == "english")
            {
                folderBrowserDialog1.Description = "Please select where would you want to download your servers:";
            }

            else if (Server_Creation_Tool.Properties.Settings.Default.language == "german")
            {
                folderBrowserDialog1.Description = "Wähle bitte aus, wo der Server erstellt werden soll:";
            }

            // A new folder button will display in FolderBrowserDialog.
            folderBrowserDialog1.ShowNewFolderButton = true;
            //Show FolderBrowserDialog
            DialogResult dlgResult = folderBrowserDialog1.ShowDialog();
            if (dlgResult == DialogResult.OK)
            { return folderBrowserDialog1.SelectedPath; }
            else
            { return ""; }
        }
        public bool check4Updates(string updtCheckURL, int currentVer)
        {
            var url = updtCheckURL;
            string textFromFile;
            try { textFromFile = (new WebClient()).DownloadString(url); }
            catch (Exception a)
            { log.LogAppend(a.ToString()); return false; }
            int latestVer = int.Parse(textFromFile);
            if (latestVer > currentVer)
            { return true; }
            else
            { return false; }
        }

        //server searching method for navigation bars
        public void searchGameInNavBar(Elegant.Ui.NavigationBar steamSvrNavBar, string txtToSearch)
        {
            foreach (Elegant.Ui.ToggleButton toggleBtn in steamSvrNavBar.Controls)
            {
                if (toggleBtn.Text.Trim().ToLower().Contains(txtToSearch.Trim().ToLower()) && txtToSearch.Trim() != "" && !toggleBtn.Text.Contains("--Welcome--"))
                {
                    steamSvrNavBar.AutoScrollPosition = new Point(steamSvrNavBar.AutoScrollPosition.X, toggleBtn.Location.Y);
                    steamSvrNavBar.VerticalScroll.Value = toggleBtn.Location.Y;
                    toggleBtn.Select();
                }
            }
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
        public string getFlName(string path)
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
                var f = GetFoldInst.GetFolder(path);
                var convrt = ByteSize.FromBytes(f.size);
                result = convrt.MegaBytes;
            }
            catch (DirectoryNotFoundException) { return 0; }
            return result;
        }
        public bool downloadnWriteFile(string link, string writePath,bool createFold=true)
        {
            try
            {
                //DOWNLOAD FILE AND WRITE IT
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(link);
                StreamReader reader = new StreamReader(stream);
                String content = reader.ReadToEnd();
                string foldPath = Path.GetDirectoryName(writePath);
                if (!System.IO.Directory.Exists(foldPath) && createFold)
                {
                    System.IO.Directory.CreateDirectory(foldPath); 
                }
                System.IO.File.WriteAllText(writePath, content);
                return true;
            }
            catch (Exception a)
            { log.LogAppend(a.ToString()); return false; }
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
        public void RemoveClickEvent(Elegant.Ui.Button b)
        {
            FieldInfo f1 = typeof(Control).GetField("EventClick",
                BindingFlags.Static | BindingFlags.NonPublic);

            object obj = f1.GetValue(b);
            PropertyInfo pi = b.GetType().GetProperty("Events",
                BindingFlags.NonPublic | BindingFlags.Instance);

            EventHandlerList list = (EventHandlerList)pi.GetValue(b, null);
            list.RemoveHandler(obj, list[obj]);
        }
    }
}

