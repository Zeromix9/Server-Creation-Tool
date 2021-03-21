using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Creation_Tool
{
    class non_steamGameInstallFunc
    {
        string[] installFailMsg;
        private void setMsgLang()
        {
            if (Properties.Settings.Default.language == "english")
            {
                installFailMsg = new string[] { "Installation failed! Please retry the installation or join our steam group for help", "Error" };
            }
            else
            {
                installFailMsg = new string[] { "Installation fehlgeschlagen! Bitte versuche es erneut oder tritt unserer Steam Gruppe für Hilfe bei.", "Fehler" };
            }
        }

        //--------------check if a game is installed--------------
        public bool checkIfGameInstalledNON_STEAM(string steamCMDandServersFolder, string gameFolder)
        {
            return false;
        }
        //________________________________________________________________

        public void finishInstallationNON_STEAM(string steamCMDandServersFolder, string serverFolderName)
        {
            setMsgLang();
            //check if server was installed
            if (checkIfGameInstalledNON_STEAM(steamCMDandServersFolder, serverFolderName) != true)
            { Elegant.Ui.MessageBox.Show(installFailMsg[0], installFailMsg[1], Elegant.Ui.MessageBoxButtons.OK, Elegant.Ui.MessageBoxIcon.Error); }

            //here is a switch statement. it works like the IF statement
            switch (serverFolderName)
            {
                case "":
                    break;
            }
        }
        //_________________________________________________________________________________________________________//


    }
}
