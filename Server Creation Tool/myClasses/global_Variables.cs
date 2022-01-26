using statServer_Creation_Tool;
using System;

namespace Server_Creation_Tool.myClasses
{
    class global_Variables
    {
        #region local_vars
        static string appdt = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        static string appdt2 = appdt.Substring(0, appdt.LastIndexOf(@"\"));
       #endregion
        #region Various Stuff
        public string[] updateCheckingInfo = new string[] {
            "https://pastebin.com/raw/mVYcG7tc",//website which contains the new version number and other info
            "https://zeromix.itch.io/server-creation-tool",//Website to download new version
            "42" };//The version of this tool. Remove the comma.   e.g 3.2 is 32
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
        public string days7RootFold = "7days_ds";
        public string[] days7StartFilesLoc = new string[1] { @"\startdedicated.bat" };//here put the full path of all the files that the server can be started from.
        //if there are any batch files which contain settings, put them first in the array. Put other files with settings (e.g. a config file) last.
        public string[] days7ConfFilesLoc = new string[1] { @"\serverconfig.xml" };//here put the full path of any files that contain settings. Don't forget to change the array size
        public string days7SteamID = @"294420";
        public string[] days7GuideLink = new string[] { "https://steamcommunity.com/sharedfiles/filedetails/?id=1728855550", "https://steamcommunity.com/sharedfiles/filedetails/?id=1731898014" };//English-German
        //------
        //----ARK----
        public string arkSrvType = "steam";
        public string arkRootFold = "ARK_ds";
        public string[] arkStartFilesLoc = new string[1] { @"\Run.bat" };
        public string[] arkConfFilesLoc = new string[2] { @"\Run.bat", @"\ShooterGame\Saved\Config\WindowsServer\GameUserSettings.ini" };
        public string arkSteamID = @"376030";
        public string[] arkGuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=694475841", "http://steamcommunity.com/sharedfiles/filedetails/?id=692129985" };
        public string arkStartBatFileCode = @"start ShooterGame\Binaries\Win64\ShooterGameServer.exe ""TheIsland? listen?SessionName=ServerName? ServerPassword = test ? ServerAdminPassword = RCONADMIN""" + Environment.NewLine + "exit";
        //------
        //----CoD:BO3----
        public string bo3SrvType = "steam";
        public string bo3RootFold = "bo3_ds";
        public string[] bo3StartFilesLoc = new string[1] { @"\Launch_Server.bat" };
        public string[] bo3ConfFilesLoc = new string[1] { @"\machinecfg\playlists.info" };
        public string bo3SteamID = @"545990";
        public string[] bo3GuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=819848285", "http://steamcommunity.com/sharedfiles/filedetails/?id=818635244" };
        //------
        //----CS 1.6----
        public string csSrvType = "steam";
        public string csRootFold = "cs_ds";
        public string[] csStartFilesLoc = new string[2] { @"\Run.bat", @"\hlds.exe" };
        public string[] csConfFilesLoc = new string[2] { @"\Run.bat", @"\cstrike\server.cfg" };//as a general rule, put the batch file first and the cfg second in the array
        public string csSteamID = @"90";
        public string[] csGuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=729345010", "http://steamcommunity.com/sharedfiles/filedetails/?id=710511353" };
        public string[] csCfgFilelink = new string[] { "https://pastebin.com/raw/JbsxsQuE", "https://pastebin.com/raw/XGtEw9Qp" };
        //This game has a special batch file function in the "serverFuncs" class
        //------
        //----CS SOURCE----
        public string cssSrvType = "steam";
        public string cssRootFold = "css_ds";
        public string[] cssStartFilesLoc = new string[1] { @"\Run.bat" };
        public string[] cssConfFilesLoc = new string[2] { @"\Run.bat", @"\cstrike\cfg\server.cfg" };
        public string cssSteamID = @"232330";
        public string[] cssGuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=397365275", "http://steamcommunity.com/sharedfiles/filedetails/?id=394849912" };
        public string[] cssCfgFilelink = new string[] { "https://pastebin.com/raw/e3f89ijz", "https://pastebin.com/raw/mQCitqtv" };
        public string cssStartBatFileCode = "start srcds.exe -console -game cstrike -secure +maxplayers 22 +map de_dust";
        //------
        //----CSGO----
        public string csgoSrvType = "steam";
        public string csgoRootFold = "csgo_ds";
        public string[] csgoStartFilesLoc = new string[1] { @"\Run.bat" };
        public string[] csgoConfFilesLoc = new string[2] { @"\Run.bat", @"\csgo\cfg\server.cfg" };
        public string csgoSteamID = @"740";
        public string[] csgoGuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=749588143", "http://steamcommunity.com/sharedfiles/filedetails/?id=711630778" };
        public string[] csgoCfgFilelink = new string[] { "https://pastebin.com/raw/n1XcFu5F", "https://pastebin.com/raw/n1XcFu5F" };
        //This game has a special batch file function in the "serverFuncs" class
        //------
        //----GMOD----
        public string gmodSrvType = "steam";
        public string gmodRootFold = "gmod_ds";
        public string[] gmodStartFilesLoc = new string[1] { @"\Run.bat" };
        public string[] gmodConfFilesLoc = new string[2] { @"\Run.bat", @"\garrysmod\cfg\server.cfg" };
        public string gmodSteamID = @"4020";
        public string[] gmodGuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=723644520", "http://steamcommunity.com/sharedfiles/filedetails/?id=719731406" };
        public string[] gmodCfgFilelink = new string[] { "https://pastebin.com/raw/vtdYsHLG", "https://pastebin.com/raw/YzEbTBVy" };
        public string gmodStartBatFileCode = "start srcds.exe -console -game cstrike -secure +maxplayers 22  +map gm_flatgrass";
        //------
        //----Hurtworld----
        public string hurtworldSrvType = "steam";
        public string hurtworldRootFold = "hurtworld_ds";
        public string[] hurtworldStartFilesLoc = new string[1] { @"\host.bat" };
        public string[] hurtworldConfFilesLoc = new string[2] { @"\host.bat", @"\autoexec.cfg" };
        public string hurtworldSteamID = @"405100";
        public string[] hurtworldGuideLink = new string[] { "https://steamcommunity.com/sharedfiles/filedetails/?id=1893124522", "https://steamcommunity.com/sharedfiles/filedetails/?id=1912242476" };
        //------
        //----Killing floor 2----
        public string kf2SrvType = "steam";
        public string kf2RootFold = "kf2_ds";
        public string[] kf2StartFilesLoc = new string[1] { @"\KF2Server.bat" };
        public string[] kf2ConfFilesLoc = new string[4] { @"\KFGame\Config\DefaultGame.bat", @"\KFGame\Config\PCServer-KFGame.ini", @"\KFGame\Config\KFWeb.ini", @"\KF2Server.bat" };
        public string kf2SteamID = @"232130";
        public string[] kf2GuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=804751117", "http://steamcommunity.com/sharedfiles/filedetails/?id=775342119" };
        //------
        //----Left 4 dead----
        public string l4dSrvType = "steam";
        public string l4dRootFold = "l4d_ds";
        public string[] l4dStartFilesLoc = new string[1] { @"\Run.bat" };
        public string[] l4dConfFilesLoc = new string[4] { @"\Run.bat", @"\left4dead\cfg\server.cfg", @"\left4dead\motd.txt", @"\left4dead\host.txt" };
        public string l4dSteamID = @"222840";
        public string[] l4dGuideLink = new string[] { "https://steamcommunity.com/sharedfiles/filedetails/?id=1872593045", "https://steamcommunity.com/sharedfiles/filedetails/?id=1795236722" };
        public string[] l4dCfgFilelink = new string[] { "https://pastebin.com/raw/d2CJ08wX", "https://pastebin.com/raw/2EALxXDw" };
        public string l4dStartBatFileCode = "start srcds.exe -console -game left4dead -port 27015 +map l4d_hospital01_apartment +mp_gamemode coop";
        //------
        //----Left 4 dead 2----
        public string l4d2SrvType = "steam";
        public string l4d2RootFold = "l4d2_ds";
        public string[] l4d2StartFilesLoc = new string[1] { @"\Run.bat" };
        public string[] l4d2ConfFilesLoc = new string[4] { @"\Run.bat", @"\left4dead2\cfg\server.cfg", @"\left4dead2\motd.txt", @"\left4dead2\host.txt" };
        public string l4d2SteamID = @"222860";
        public string[] l4d2GuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=764005219", "http://steamcommunity.com/sharedfiles/filedetails/?id=754702686" };
        public string[] l4d2CfgFilelink = new string[] { "https://pastebin.com/raw/1i8xK3L4", "https://pastebin.com/raw/NWjDxe4G" };
        public string l4d2StartBatFileCode = "start srcds.exe -console -game left4dead2 -secure +maxplayers 4  +map c1m1_hotel";
        //------
        //----Rust----
        public string rustSrvType = "steam";
        public string rustRootFold = "rust_ds";
        public string[] rustStartFilesLoc = new string[1] { @"\Run.bat" };
        public string[] rustConfFilesLoc = new string[2] { @"\Run.bat", @"\server\my_server_identity\cfg\server.cfg" };
        public string rustSteamID = @"258550";
        public string[] rustGuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=874293589", "http://steamcommunity.com/sharedfiles/filedetails/?id=874080531" };
        public string[] rustCfgFilelink = new string[] { "https://pastebin.com/raw/7AAK7itF", "https://pastebin.com/raw/7AAK7itF" };
        public string rustStartBatFileCode = "start RustDedicated.exe -batchmode +server.port 28015 +server.level " + @"""Procedural Map""" + " server.seed 1234 +server.worldsize 4000 +server.maxplayers 10";
        //------
        //----Sven Coop----
        public string svenSrvType = "steam";
        public string svenRootFold = "sven_ds";
        public string[] svenStartFilesLoc = new string[1] { @"\Run.bat" };
        public string[] svenConfFilesLoc = new string[2] { @"\Run.bat", @"\svencoop\server.cfg" };
        public string svenSteamID = @"276060";
        public string[] svenGuideLink = new string[] { "http://steamcommunity.com/sharedfiles/filedetails/?id=874293589", "http://steamcommunity.com/sharedfiles/filedetails/?id=874080531" };
        //This game has a special batch file function in the "serverFuncs" class
        //------
        //----Unturned----
        public string unturnedSrvType = "steam";
        public string unturnedRootFold = "unturned_ds";
        public string[] unturnedStartFilesLoc = new string[1] { @"\Run.bat" };
        public string[] unturnedConfFilesLoc = new string[1] { @"\Run.bat" };
        public string unturnedSteamID = @"1110390";
        public string[] unturnedSrvConfigHelpLink = new string[] { "https://unturned.fandom.com/wiki/Hosting_a_Dedicated_Server#Configuring_your_Server:" };
        public string unturnedStartBatFileCode = "start Unturned.exe - port:25444 - players:20 - nographics - pei - gold - nosync - pve - sv";
        //----BeamNG.Drive----
        public string beamngSrvType = "non_steam";
        public string beamngRootFold = "beamng_ds";
        public string[] beamngStartFilesLoc = new string[1] { @"\BeamMP-Server.exe" };
        public string[] beamngConfFilesLoc = new string[1] { @"\ServerConfig.toml" };
        public string beamngDownloadLink = @"https://beammp.com/server/BeamMP_Server.zip";
        public string[] beamngSrvConfigHelpLink = new string[] { "https://wiki.beammp.com/en/home/server-installation" };
        //----ATLAS----
        public string atlasSrvType = "steam";
        public string atlasRootFold = ".emtpyF";
        public string[] atlasStartFilesLoc = new string[1] { @"\Run.bat" };
        public string[] atlasConfFilesLoc = new string[2] { @"\Run.bat", @"\svencoop\server.cfg" };
        public string atlasSteamID = @"1110390";
        public string[] atlasGuideLink = new string[] { "https://steamcommunity.com/sharedfiles/filedetails/?id=2317120456", "https://steamcommunity.com/sharedfiles/filedetails/?id=2317122651" };
        //----Craftopia----
        public string craftopiaSrvType = "steam";
        public string craftopiaRootFold = "craftopia_ds";
        public string[] craftopiaStartFilesLoc = new string[2] { @"\Run.bat", @"\Craftopia.exe" };
        public string[] craftopiaConfFilesLoc = new string[2] { @"\Run.bat", appdt2 + @"\LocalLow\PocketPair\Craftopia\ServerSetting.ini" };
        public string craftopiaSteamID = @"1670340";
        public string[] craftopiaGuideLink = new string[] {"https://steamcommunity.com/sharedfiles/filedetails/?id=2525156012","https://steamcommunity.com/sharedfiles/filedetails/?id=2559424770" };
        public string craftopiaStartBatFileCode = "start Craftopia.exe -batchmode -showlogs";
        //----No More Room in Hell----
        public string nmrihSrvType = "steam";
        public string nmrihRootFold = "nmrih_ds";
        public string[] nmrihStartFilesLoc = new string[1] { @"\Run.bat"};
        public string[] nmrihConfFilesLoc = new string[2] { @"\Run.bat", @"\nmrih\cfg\server.cfg" };
        public string nmrihSteamID = @"317670";
        public string[] nmrihGuideLink = new string[] { "https://steamcommunity.com/sharedfiles/filedetails/?id=2657050868", "https://steamcommunity.com/sharedfiles/filedetails/?id=2683410124" };
        public string nmrihStartBatFileCode = "start srcds.exe -game nmrih -secure -console +map nmo_boardwalk";
        #endregion
    }
}
