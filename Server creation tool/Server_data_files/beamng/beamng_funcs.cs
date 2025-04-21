using Server_creation_tool.classes;
using System.Windows.Forms;

namespace Server_creation_tool.Server_data_files.beamng
{
    internal class beamng_funcs
    {
        funcsClass funcs = new funcsClass();

        public beamng_funcs(MainForm mainForm)
        {
            mainFrm = mainForm;
        }
        MainForm mainFrm;
        //DataTable generalLang = new DataTable();

        public void setup()
        {

        }

        public void installServer(string updateOrInstall)
        {
            mainFrm.disableUI();
            string message;
            if (updateOrInstall == "update")
            {
                message = mainFrm.getGeneralLang("srv_updating_repairing")[1];
            }
            else
            {
                message = mainFrm.getGeneralLang("installing_srv")[1];
            }
            mainFrm.taskStarted(message, true, true);
            string message2;
            MessageBoxIcon icon;
            //download server and check if it went well
            if (mainFrm.easyDownloadFile(mainFrm.getServerDataStr("download_link"), mainFrm.getCurrentInstancePath() + @"\BeamMP-Server.exe", true))
            {
                if (updateOrInstall == "update")
                {
                    message2 = mainFrm.getGeneralLang("srv_update_repair_finished")[1];
                }
                else
                {
                    message2 = mainFrm.getGeneralLang("srv_install_finished")[1];
                }
                icon = MessageBoxIcon.Information;
            }
            else//failed
            {
                if (updateOrInstall == "update")
                {
                    message2 = mainFrm.getGeneralLang("srv_update_repair_failed")[1];
                }
                else
                {
                    message2 = mainFrm.getGeneralLang("srv_install_failed")[1];
                }
                icon = MessageBoxIcon.Error;

            }
            mainFrm.MsgBox.Show(message2, mainFrm.getGeneralLang("info")[1], MessageBoxButtons.OK, icon);
            mainFrm.enableUI(true);
            mainFrm.taskEnded();
            mainFrm.refresh();
        }
    }
}
