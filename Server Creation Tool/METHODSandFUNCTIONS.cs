using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace statServer_Creation_Tool
{
    public class METHODSandFUNCTIONS
    {

        //-------------SERVER CONTEX MENU STRIP------------
        public Point serverMenuStripShow(object sender)
        {
            Button btnSender = (Button)sender;
            Point ptLowerLeft = new Point(0, btnSender.Height);
            ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
            return ptLowerLeft;
        }
        //_____________________________________________________


        //--------------check if a game is installed--------------
        public bool checkIfGameInstalled(string steamCMDFolder, string gameFolder)
        {
            bool result = false;
            switch (gameFolder)
            {
                case "css":
                    //Counter Strike Source
                    if (System.IO.Directory.Exists(steamCMDFolder + @"\css\cstrike") && System.IO.Directory.Exists(steamCMDFolder + @"\css\hl2") && System.IO.File.Exists(steamCMDFolder + @"\css\srcds.exe"))
                    { result = true; }
                    else { result = false; }
                    break;

                case "sven":
                    //Sven Coop
                    if (System.IO.File.Exists(steamCMDFolder + @"\sven\svends.exe") && System.IO.Directory.Exists(steamCMDFolder + @"\sven\svencoop") && System.IO.File.Exists(steamCMDFolder + @"\sven\steamclient.dll") && System.IO.File.Exists(steamCMDFolder + @"\sven\libcef.dll"))
                    { result = true; }
                    else { result = false; }
                    break;

                case "ark":
                    //ARK: Survival Evolved
                    if (System.IO.File.Exists(steamCMDFolder + @"\ark\ShooterGame\Binaries\Win64\ShooterGameServer.exe") && System.IO.Directory.Exists(steamCMDFolder + @"\ark\Engine") && System.IO.File.Exists(steamCMDFolder + @"\ark\steamclient.dll"))
                    { result = true; }
                    else { result = false; }
                    break;

                case "csgo":
                    //CS:GO
                    if (System.IO.File.Exists(steamCMDFolder + @"\csgo\srcds.exe") && System.IO.File.Exists(steamCMDFolder + @"\csgo\srcds_run") && System.IO.Directory.Exists(steamCMDFolder + @"\csgo\csgo") && System.IO.Directory.Exists(steamCMDFolder + @"\csgo\platform"))
                    { result = true; }
                    else { result = false; }
                    break;

                case "l4d":
                    //Left 4 dead
                    if (System.IO.File.Exists(steamCMDFolder + @"\l4d\srcds.exe") & System.IO.File.Exists(steamCMDFolder + @"\l4d\left4dead.exe") && System.IO.Directory.Exists(steamCMDFolder + @"\l4d\left4dead") && System.IO.File.Exists(steamCMDFolder + @"\l4d\steamclient.dll") && System.IO.Directory.Exists(steamCMDFolder + @"\l4d\hl2"))
                    { result = true; }
                    else { result = false; }
                    break;

                case "gmod":
                    //Garry's Mod
                    if (System.IO.File.Exists(steamCMDFolder + @"\gmod\srcds.exe") && System.IO.Directory.Exists(steamCMDFolder + @"\gmod\garrysmod") && System.IO.Directory.Exists(steamCMDFolder + @"\gmod\sourceengine"))
                    { result = true; }
                    else { result = false; }
                    break;

                case "l4d2":
                    //Left 4 Dead 2
                    if (System.IO.File.Exists(steamCMDFolder + @"\l4d2\srcds.exe") && System.IO.File.Exists(steamCMDFolder + @"\l4d2\left4dead2.exe") && System.IO.Directory.Exists(steamCMDFolder + @"\l4d2\left4dead2") && System.IO.Directory.Exists(steamCMDFolder + @"\l4d2\hl2"))
                    { result = true; }
                    else { result = false; }
                    break;

                case "hurtworld":
                    //HurtWorld
                    if (System.IO.File.Exists(steamCMDFolder + @"\hurtworld\Hurtworld.exe") && System.IO.File.Exists(steamCMDFolder + @"\hurtworld\steamclient.dll") && System.IO.File.Exists(steamCMDFolder + @"\hurtworld\tier0_s.dll") && System.IO.Directory.Exists(steamCMDFolder + @"\hurtworld\Hurtworld_Data") && System.IO.File.Exists(steamCMDFolder + @"\hurtworld\host.bat"))
                    { result = true; }
                    else { result = false; }
                    break;

                case "cs":
                    //Couter Strike 1.6
                    if (System.IO.File.Exists(steamCMDFolder + @"\cs\hlds.exe") && System.IO.File.Exists(steamCMDFolder + @"\cs\steamclient.dll") && System.IO.File.Exists(steamCMDFolder + @"\cs\tier0_s.dll") && System.IO.Directory.Exists(steamCMDFolder + @"\cs\platform") && System.IO.Directory.Exists(steamCMDFolder + @"\cs\cstrike"))
                    { result = true; }
                    else { result = false; }
                    break;

                case "rust":
                    //Rust
                    if (System.IO.File.Exists(steamCMDFolder + @"\rust\RustDedicated.exe") && System.IO.Directory.Exists(steamCMDFolder + @"\rust\RustDedicated_Data") && System.IO.Directory.Exists(steamCMDFolder + @"\rust\userdata"))
                    { result = true; }
                    else { result = false; }
                    break;

                case "7days":
                    //7 days to die
                    if (System.IO.File.Exists(steamCMDFolder + @"\7days\startdedicated.bat"))
                    { result = true; }
                    else { result = false; }
                    break;

                case "bo3":
                    //CoD Black Ops 3
                    if (System.IO.File.Exists(steamCMDFolder + @"\bo3\UnrankedServer\Launch_Server.bat"))
                    { result = true; }
                    else { result = false; }
                    break;

                case "kf2":
                    //Killing Floor 2
                    if (System.IO.File.Exists(steamCMDFolder + @"\kf2\KF2Server.bat") && System.IO.File.Exists(steamCMDFolder + @"\kf2\steamclient.dll") && System.IO.Directory.Exists(steamCMDFolder + @"\kf2\Engine"))
                    { result = true; }
                    else { result = false; }
                    break;

                case "synergy":
                    //Synergy
                    if (System.IO.File.Exists(""))
                    { result = true; }
                    else { result = false; }
                    break;
            }
            return result;
        }
        //______________________________________________________________________________

        //------------This function checks if steamCMD exist, and if it does then it procceds to check if the selected game is installed or not, and if it is installed, then it shows the contexMenu Strip. If it isn't installed, it will install it
        public string checkIfGameInstalledAndCheckSteamCMD(Button gameBtn, object sender, ContextMenuStrip ServerMenuStrip, string steamCmdSpeicherort)
        {
            string result = "NotInstalled";
            try
            {
                //check if server is installed or not
                if (gameBtn.BackColor == Color.LimeGreen)
                {
                    ServerMenuStrip.Show(serverMenuStripShow(sender));
                    result = "Installed";
                }
                else
                {
                    if (steamCmdSpeicherort != null)
                    {
                        //check if steamCMD exists
                        if (File.Exists(steamCmdSpeicherort))
                        {
                            //in the designer, but i cant view it
                            result = "NotInstalled";
                        }
                        else
                        {
                            result = "steamCMDNotFound";
                        }
                    }
                }
            }
            catch (Exception) {; }
            return result;
        }
        //_____________________________________________________________________________________

        public bool HasInternet()
        {

            try
            {

                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

