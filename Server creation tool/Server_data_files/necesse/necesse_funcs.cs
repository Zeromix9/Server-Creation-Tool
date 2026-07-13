using Server_creation_tool.classes;
using System;
using System.IO;
using System.Windows.Forms;

namespace Server_creation_tool.Server_data_files.necesse
{
    internal class necesse_funcs
    {
        funcsClass funcs = new funcsClass();

        public necesse_funcs(MainForm mainForm)
        {
            mainFrm = mainForm;
        }
        MainForm mainFrm;
        //DataTable generalLang = new DataTable();

        public void setup()
        {

        }

        public bool installServer(string updateOrInstall)
        {
            //add -localdir argument at the end of the server starting batch file
            string filepath = mainFrm.getCurrentInstancePath() + mainFrm.getServerDataStr("start_file_path");
            try
            {
                if (File.Exists(filepath))
                {
                    string content = File.ReadAllText(filepath).TrimEnd();

                    File.WriteAllText(filepath, content + " -localdir");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
