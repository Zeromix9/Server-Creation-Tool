using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Server_Creation_Tool.myClasses
{
    class global_Variables
    {
        #region General Stuff
        public string[] updateCheckingInfo = new string[] {
            "https://pastebin.com/raw/mVYcG7tc",//website which contains the new version number and other info
            "https://zeromix.itch.io/server-creation-tool",//Website to download new version
            "32" };//The version of this tool. Remove the comma.   e.g 3,2 is 32
        public string logsDir = @"\sctLogs";//name of the logs folder.
        public string steamCMDUrl = "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip";
        #endregion

        #region Dynamic vars
        public int lgNum;
        public int lgNumQuad;

        #endregion

        #region serverVars
        //--IMPORTANT INFO---//
        //Every time you add a new button for a server don't forget to change the Tag Property to the server's codename. e.g 7 days to die is "days7"
        //--------------

        //----7days----
        public string days7SrvType = "steam";
        public string days7RootFold = "7days";
        public string[] days7StartFilesLoc = new string[1] { @"\startdedicated.bat" };//here put the full path of all the files that the server can be started from. Don't forget to change the array size
        //if there are any batch files which contain settings, put them first. Put other files with settings (e.g. a config file) at the end.
        public string[] days7ConfFilesLoc = new string[1] { @"\serverconfig.xml" };//here put the full path of any files that contain settings. Don't forget to change the array size
        //root folder size value should be in MegaBytes
        public int days7FoldSize = 650; //not used
        public string days7InstCode = @"+login anonymous +force_install_dir ./7days/ +app_update 294420 validate";
        public string[] days7GuideLink = new string[] { "https://steamcommunity.com/sharedfiles/filedetails/?id=1728855550", "https://steamcommunity.com/sharedfiles/filedetails/?id=1731898014" };//English-German
        //------
        //----ARK----
        public string arkSrvType = "steam";
        public string arkRootFold = "ark";
        public string[] arkStartFilesLoc = new string[1] { @"\ShooterGame\Binaries\Win64\Run.bat" };
        public string[] arkConfFilesLoc = new string[2] { @"\ShooterGame\Binaries\Win64\Run.bat", @"\ShooterGame\Saved\Config\WindowsServer\GameUserSettings.ini" };
        public int arkFoldSize = 600;
        public string arkInstCode = @"+login anonymous +force_install_dir ./ark/ +app_update 376030 validate";
        public string[] arkGuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=694475841", "http://steamcommunity.com/sharedfiles/filedetails/?id=692129985" };
        public string arkStartBatFileCode = "start ShooterGameServer " + @"""TheIsland?listen?SessionName=Mein_Server?ServerPassword=TEST?ServerAdminPassword=RCONADMIN?MaxPlayers=50""" + Environment.NewLine + "exit";
        //------
        //----CoD:BO3----
        public string bo3SrvType = "steam";
        public string bo3RootFold = "bo3";
        public string[] bo3StartFilesLoc = new string[1] { @"\Launch_Server.bat" };
        public string[] bo3ConfFilesLoc = new string[1] { @"\machinecfg\playlists.info" };
        public int bo3FoldSize = 600;
        public string bo3InstCode = @"+login anonymous +force_install_dir ./bo3/ +app_update 545990 validate";
        public string[] bo3GuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=819848285", "http://steamcommunity.com/sharedfiles/filedetails/?id=818635244" };
        //------
        //----CS 1.6----
        public string csSrvType = "steam";
        public string csRootFold = "cs";
        public string[] csStartFilesLoc = new string[2] { @"\Run.bat", @"\hlds.exe" };
        public string[] csConfFilesLoc = new string[2] { @"\Run.bat", @"\cstrike\server.cfg" };//as a general rule, put the batch file first and the cfg second
        public int csFoldSize = 150;
        public string csInstCode = @"+login anonymous +force_install_dir ./cs/ +app_update 90 validate";
        public string[] csGuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=729345010", "http://steamcommunity.com/sharedfiles/filedetails/?id=710511353" };
        public string[] csCfgFilelink = new string[] { "https://pastebin.com/raw/JbsxsQuE", "https://pastebin.com/raw/XGtEw9Qp" };
        //------
        //----CS SOURCE----
        public string cssSrvType = "steam";
        public string cssRootFold = "css";
        public string[] cssStartFilesLoc = new string[1] { @"\Run.bat" };
        public string[] cssConfFilesLoc = new string[2] { @"\Run.bat", @"\cstrike\cfg\server.cfg" };
        public int cssFoldSize = 150;
        public string cssInstCode = @"+login anonymous +force_install_dir ./css/ +app_update 232330 validate";
        public string[] cssGuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=397365275", "http://steamcommunity.com/sharedfiles/filedetails/?id=394849912" };
        public string[] cssCfgFilelink = new string[] { "https://pastebin.com/raw/e3f89ijz", "https://pastebin.com/raw/mQCitqtv" };
        public string cssStartBatFileCode = "start srcds.exe -console -game cstrike -secure +maxplayers 22 +map de_dust";
        //------
        //----CSGO----
        public string csgoSrvType = "steam";
        public string csgoRootFold = "csgo";
        public string[] csgoStartFilesLoc = new string[1] { @"\Run.bat" };
        public string[] csgoConfFilesLoc = new string[2] { @"\Run.bat", @"\csgo\cfg\server.cfg" };
        public int csgoFoldSize = 150;
        public string csgoInstCode = @"+login anonymous +force_install_dir ./csgo/ +app_update 740 validate";
        public string[] csgoGuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=749588143", "http://steamcommunity.com/sharedfiles/filedetails/?id=711630778" };
        public string[] csgoCfgFilelink = new string[] { "https://pastebin.com/raw/n1XcFu5F", "https://pastebin.com/raw/n1XcFu5F" };
        //------
        //----GMOD----
        public string gmodSrvType = "steam";
        public string gmodRootFold = "gmod";
        public string[] gmodStartFilesLoc = new string[1] { @"\Run.bat" };
        public string[] gmodConfFilesLoc = new string[2] { @"\Run.bat", @"\garrysmod\cfg\server.cfg" };
        public int gmodFoldSize = 150;
        public string gmodInstCode = @"+login anonymous +force_install_dir ./gmod/ +app_update 4020 validate";
        public string[] gmodGuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=723644520", "http://steamcommunity.com/sharedfiles/filedetails/?id=719731406" };
        public string[] gmodCfgFilelink = new string[] { "https://pastebin.com/raw/vtdYsHLG", "https://pastebin.com/raw/YzEbTBVy" };
        public string gmodStartBatFileCode = "start srcds.exe -console -game cstrike -secure +maxplayers 22  +map gm_flatgrass";
        //------
        //----Hurtworld----
        public string hurtworldSrvType = "steam";
        public string hurtworldRootFold = "hurtworld";
        public string[] hurtworldStartFilesLoc = new string[1] { @"\host.bat" };
        public string[] hurtworldConfFilesLoc = new string[2] { @"\host.bat", @"\autoexec.cfg" };
        public int hurtworldFoldSize = 150;
        public string hurtworldInstCode = @"+login anonymous +force_install_dir ./hurtworld/ +app_update 405100 validate";
        public string[] hurtworldGuideLink = new string[] { "https://steamcommunity.com/sharedfiles/filedetails/?id=1893124522", "https://steamcommunity.com/sharedfiles/filedetails/?id=1912242476" };
        //------
        //----Killing floor 2----
        public string kf2SrvType = "steam";
        public string kf2RootFold = "kf2";
        public string[] kf2StartFilesLoc = new string[1] { @"\KF2Server.bat" };
        public string[] kf2ConfFilesLoc = new string[4] { @"\KFGame\Config\DefaultGame.bat", @"\KFGame\Config\PCServer-KFGame.ini", @"\KFGame\Config\KFWeb.ini", @"\KF2Server.bat" };
        public int kf2FoldSize = 150;
        public string kf2InstCode = @"+login anonymous +force_install_dir ./kf2/ +app_update 232130 validate";
        public string[] kf2GuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=804751117", "http://steamcommunity.com/sharedfiles/filedetails/?id=775342119" };
        //------
        //----Left 4 dead----
        public string l4dSrvType = "steam";
        public string l4dRootFold = "l4d";
        public string[] l4dStartFilesLoc = new string[1] { @"\Run.bat" };
        public string[] l4dConfFilesLoc = new string[4] { @"\Run.bat", @"\left4dead\cfg\server.cfg", @"\left4dead\motd.txt", @"\left4dead\host.txt" };
        public int l4dFoldSize = 150;
        public string l4dInstCode = @"+login anonymous +force_install_dir ./l4d/ +app_update 222840 validate";
        public string[] l4dGuideLink = new string[] { "https://steamcommunity.com/sharedfiles/filedetails/?id=1872593045", "https://steamcommunity.com/sharedfiles/filedetails/?id=1795236722" };
        public string[] l4dCfgFilelink = new string[] { "https://pastebin.com/raw/d2CJ08wX", "https://pastebin.com/raw/2EALxXDw" };
        public string l4dStartBatFileCode = "start srcds.exe -console -game left4dead -port 27015 +map l4d_hospital01_apartment +mp_gamemode coop";
        //------
        //----Left 4 dead 2----
        public string l4d2SrvType = "steam";
        public string l4d2RootFold = "l4d2";
        public string[] l4d2StartFilesLoc = new string[1] { @"\Run.bat" };
        public string[] l4d2ConfFilesLoc = new string[4] { @"\Run.bat", @"\left4dead2\cfg\server.cfg", @"\left4dead2\motd.txt", @"\left4dead2\host.txt" };
        public int l4d2FoldSize = 150;
        public string l4d2InstCode = @"+login anonymous +force_install_dir ./l4d2/ +app_update 222860 validate";
        public string[] l4d2GuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=764005219", "http://steamcommunity.com/sharedfiles/filedetails/?id=754702686" };
        public string[] l4d2CfgFilelink = new string[] { "https://pastebin.com/raw/1i8xK3L4", "https://pastebin.com/raw/NWjDxe4G" };
        public string l4d2StartBatFileCode = "start srcds.exe -console -game left4dead2 -secure +maxplayers 4  +map c1m1_hotel";
        //------
        //----Rust----
        public string rustSrvType = "steam";
        public string rustRootFold = "rust";
        public string[] rustStartFilesLoc = new string[1] { @"\Run.bat" };
        public string[] rustConfFilesLoc = new string[2] { @"\Run.bat", @"\server\my_server_identity\cfg\server.cfg" };
        public int rustFoldSize = 4000;
        public string rustInstCode = @"+login anonymous +force_install_dir ./rust/ +app_update 258550 validate";
        public string[] rustGuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=874293589", "http://steamcommunity.com/sharedfiles/filedetails/?id=874080531" };
        public string[] rustCfgFilelink = new string[] { "https://pastebin.com/raw/7AAK7itF", "https://pastebin.com/raw/7AAK7itF" };
        public string rustStartBatFileCode = "start RustDedicated.exe -batchmode +server.port 28015 +server.level " + @"""Procedural Map""" + " server.seed 1234 +server.worldsize 4000 +server.maxplayers 10";
        //------
        //----Sven Coop----
        public string svenSrvType = "steam";
        public string svenRootFold = "sven";
        public string[] svenStartFilesLoc = new string[2] { @"\Run.bat", @"\svends.exe" };
        public string[] svenConfFilesLoc = new string[2] { @"\Run.bat", @"\svencoop\server.cfg" };
        public int svenFoldSize = 150;
        public string svenInstCode = @"+login anonymous +force_install_dir ./sven/ +app_update 276060 validate";
        public string[] svenGuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=874293589", "http://steamcommunity.com/sharedfiles/filedetails/?id=874080531" };
        public string svenStartBatFileCode = "start svends.exe -console -port 27015 +maxplayers 8 -insecure -nomaster +sv_lan 1 +map _server_start";
        //------
        //----Unturned----
        public string unturnedSrvType = "steam";
        public string unturnedRootFold = "unturned";
        public string[] unturnedStartFilesLoc = new string[1] { @"\Run.bat" };
        public string[] unturnedConfFilesLoc = new string[1] { @"\Run.bat" };
        public int unturnedFoldSize = 1350;
        public string unturnedInstCode = @"+login anonymous +force_install_dir ./unturned/ +app_update 1110390 validate";
        public string[] unturnedSrvConfigHelpLink = new string[] { "https://unturned.fandom.com/wiki/Hosting_a_Dedicated_Server#Configuring_your_Server:" };
        public string unturnedStartBatFileCode = "start Unturned.exe - port:25444 - players:20 - nographics - pei - gold - nosync - pve - sv";
        //----ATLAS----
        public string atlasSrvType = "steam";
        public string atlasRootFold = ".emtpyF";
        public string[] atlasStartFilesLoc = new string[1] { @"\Run.bat" };
        public string[] atlasConfFilesLoc = new string[2] { @"\Run.bat", @"\svencoop\server.cfg" };
        public int atlasFoldSize = 1300;
        public string atlasInstCode = @"+login anonymous +force_install_dir ./atlas/ +app_update 1110390 validate";
        public string[] atlasGuideLink = new string[] {"https://steamcommunity.com/sharedfiles/filedetails/?id=2317120456","https://steamcommunity.com/sharedfiles/filedetails/?id=2317122651"
        };
        #endregion
    }
}
