using System;

namespace Server_Creation_Tool.myClasses
{
    public static class lang
    {
    
        #region MainForm controls
        public static string[] welcomeLb = new string[]
      {   "Welcome to Server Creation Tool 4.2",
           "Willkommen beim Server Creation Tool 4.2",};
        public static string[] welcomeDescLb = new string[]
       {   "This tool allows you to download, install and manage" + System.Environment.NewLine + "      steam and non-steam servers quick and easy.",
           "    Dieses Programm erlaubt es dir Steam Server, sowie andere" + System.Environment.NewLine + "schnell und einfach herunterzuladen, einzurichten und zu starten"};
        public static string[] basicInstrLb = new string[]
       {   "Basic Instructions:",
           "Basis Informationen:"};
        public static string[] toolGrpB = new string[]
       {   "   Tool "+ System.Environment.NewLine +"      Group-Website",//English
           "   Tool Gruppe-"+ System.Environment.NewLine +"   Webseite"};//German
        public static string[] basicInstruct = new string[]
   {   @"1) Select the game you want and click the ""Install Server"" button."
         + System.Environment.NewLine + @"2) If its the first time you're installing a server, select the installation folder, and then wait for it to finish."
         + System.Environment.NewLine + @"3) Type ""quit"" in the command line after the installation has finished.(only for steam servers)"
         + System.Environment.NewLine + @"4) Configure the server the way you like by editing the settings files (optional)."
         + System.Environment.NewLine + "5) Start the server and enjoy!"
         + System.Environment.NewLine // empty line
         + System.Environment.NewLine + @"NOTE: If you need extra help, some servers include a guide in the ""Extra"" section. You can also go to the ""Help-Language"" tab for additional help or join our steam group and discord server to ask for help about your issue.."
        ,
        @"1) Wähle das Spiel aus, wovon du einen Server erstellen möchtest und klicke auf ""Server installieren"""
         + System.Environment.NewLine + @"2) Wenn du einen Server zum ersten mal installierst, wähle den Ordner aus und warte, bis die Installation fertiggestellt wurde."
         + System.Environment.NewLine + @"3) Schreibe ""quit"" in die Konsole, nachdem die Installation beendet wurde. (Nur bei Steam Servern)"
         + System.Environment.NewLine + @"4) Konfiguriere den Server, indem du die Konfigurationsdateien anpasst.(Optional)"
         + System.Environment.NewLine + "5) Starte den Server, viel Spaß!"
         + System.Environment.NewLine // emtpy line
         + System.Environment.NewLine + @"HINWEIß: Wenn du weitere Hilfe benötigt, so gehe unter ""Extras"" in dieser Kategorie findest du entsprechende Server Anleitungen. Desweiteren kannst du unserer Steam Gruppe für weitere Hilfe beitreten."};
        public static string[] aboutB = new string[]
       {   "       About",
           "       Über"};
        public static string[] donateB = new string[]
       {   "Donate",
           "Spenden"};
        public static string[] chngLogB = new string[]
       {   "     Changelog",
           "     Changelog"};
        public static string[] MORELb = new string[]
       {   "MORE:",
           "MORE:"};
        public static string[] appLogs = new string[]
       {   "   App Logs",
           "   App Logs"};
        public static string[] plannedFeatB = new string[]
       {   "    Planned "+ System.Environment.NewLine +"    Features",
           "Geplante Features"};
        public static string[] discordSrvB = new string[]
       {   "      Discord "+ System.Environment.NewLine +"      Server",
           "      Discord "+ System.Environment.NewLine +"      Server"};
        public static string[] extrHelpB = new string[]
      {   "    Extra help",
          "    Extra help"};
        public static string[] clearCache = new string[]
      {   "Clear cache",
          "Cache leeren"};
        public static string[] portFwRouterB = new string[]
       {   "How to port forward your router",
           "Wie man Ports im Router freigibt"};
        public static string[] langB = new string[]
       {   "       Language",
           "     Sprache"};
        public static string[] styleB = new string[]
       {   "Style",
           "Style"};
        public static string[] updCheckB = new string[]
       {   "     Check "+ Environment.NewLine +"     for updates",
           "       Suche nach " + Environment.NewLine + "      Updates"};
        public static string[] selectInstFoldB = new string[]
       {   "    Select Server " + Environment.NewLine + "     Installation Folder",
           "    Wähle den Server" + Environment.NewLine + "     Installationsordner aus"};
        public static string[] openSrvsFoldB = new string[]
       {   "Open servers folder",
           "Öffne den Server Ordner"};
        public static string[] grpBox2 = new string[]
       {   "Select a server",
           "Wähle einen Server"};
        public static string[] generalSrvOptGr = new string[]
       {   "General Server Options",
           "Allgemeine Server Einstellungen"};
        public static string[] otherOtpGr = new string[]
       {   "Other Options",
           "Andere Optionen"};
        public static string[] currentTaskGr = new string[]
       {   "Current Task",
           "Aktuelle Aufgabe"};
        public static string[] mainMenuTab = new string[]
       {   "Main Menu",
           "Hauptmenü"};
        public static string[] searchGameLb = new string[]
       {   "Search:",
           "Suche:"};
        public static string[] update = new string[]
       {   " Update",
           " Update"};
        public static string[] repairUpdt = new string[]
       {   "   Repair/Update",
           "     Reparieren/Update"};
        public static string[] openFoldB = new string[]
       {   " Open Folder",
           "     Öffne Verzeichnis"};
        public static string[] deleteB = new string[]
       {   " Delete",
           " Löschen"};
        public static string[] editStgB = new string[]
       {   "  Edit Settings",
           "Einstellungen"};
        public static string[] instGuideB = new string[]
       {   "  Installation Guide",
           "  Installation Guide"};
        public static string[] extraLb = new string[]
       {   "-Extra-",
           "-Extra-"};
        public static string[] actionLb = new string[]
       {   "-Actions-",
           "-Actions-"};
        public static string[] install = new string[]
       {   "   INSTALL",
           "   Installieren"};
        public static string[] forceStop = new string[]
       {   "    FORCE STOP",
           "    Abbruch"};
        public static string[] START = new string[]
       {   "  START",
           "  START"};
        public static string[] Start = new string[]
       {   "  Start",
           "  Start"};
        public static string[] installing = new string[]
       {   "Installing ",
           "Installiere "};
        public static string[] updating = new string[]
       {   "Updating ",
           "Update "};
        public static string[] downloading = new string[]
      {   "Downloading ",
          "Download läuft: "};
        public static string[] deleting = new string[]
       {   "Deleting ",
           "Lösche "};
        public static string[] grpBx7SrvInfo = new string[]
       {   "Server info",
           "Server info"};
        public static string[] srvSizeLb = new string[]
       {   "-Size-",
           "-Größe-"};
        public static string[] customConfigFile = new string[]
       {   "Custom config file",
           "Custom config file"};
        public static string[] customBatFile = new string[]
       {   "Custom bat file",
           "Custom bat file"};
        public static string[] selectedPath = new string[]
       {   "Selected Path",
           "Wähle Verzeichnis"};
        public static string[] error = new string[]
       {   "Error",
           "Fehler"};
        public static string[] installSrv = new string[]
       {   "Install Server",
           "Install Server"};
        public static string[] done = new string[]
       {   "Done!",
           "Fertig!"};
        public static string[] plsWait = new string[]
       {   "Please wait",
           "Please wait"};
        public static string[] configHelp = new string[]
       {   "  Configuration help",
           " Konfigurationshilfe"};
        public static string[] srvAdvancedManage = new string[]
       {   "  Advanced Management",
           " Konfigurationshilfe"};
        public static string[] addToFav = new string[]
       {   "Add to favourites",
           "Zu den Favoriten hinzufügen"};
        public static string[] showFavList = new string[]
       {   "Show favourite servers list",
           "Zeige Server Favoriten"};
        public static string[] click2Refresh = new string[]
       {   "Click to refresh",
           "Klicke zum aktualisieren",};
        public static string[] checkingInternetConn = new string[]
       {   "Checking internet connection",
           "Überprüfe die Internetverbindung",};

        #endregion

        #region Messageboxes
        public static string[] reinstSrvAsk = new string[]
       {   "Continue? Reinstalling the server will delete all of your settings and progress. Make sure to backup what you need and continue", "Reinstall Server",
           "Fortfahren? Eine Neuinstallation des Servers hat eine Löschung ihrer Einstellungen und Fortschritte zur Folge. Prüfen sie ob alles notwendige gesichert wurde und fahren sie fort.","Server neuinstallieren"};
        public static string[] delSrv = new string[]
       {   "Are you sure that you want to delete this server? Make sure to backup everything you need", "Delete Server",
           "Bist du dir sicher, dass du den Server löschen möchtest? Gehe sicher, dass du die wichtigsten Dateien gespeichert hast.","Lösche Server"};
        public static string[] srvDelSuccess = new string[]
       {   "The server has been successfully deleted", "Delete Server",
           "Der Server wurde erfolgreich gelöscht","Lösche Server"};
        public static string[] srvDelFail = new string[]
       {   "A program is probably using some of the server's files and some of them couldn't be deleted. The files have been corrupted. Try again.", "Delete Server",
           "Ein Programm greift auf einige der Server Dateien zu, daher konnten nicht alle Dateien gelöscht werden. Die Dateien wurden beschädigt. Versuch es erneut.","Lösche Server"};
        public static string[] forceStopInstSteamMsg = new string[]
       {   "Are you sure that you want to force it to quit? That could result in data corruption.", "Force Stop Installation",
           "Bist du sicher, dass du das abbrechen erzwingen willst? Dies könnte Datenverlust zur Folge haben.","Erzwinge Installations Stop"};
        public static string[] downloadFailed = new string[]
       {   "Download failed. Please check your internet connection!", "Download Server",
           "Download fehlgeschladen. Bitte stelle sicher, dass du mit dem Internet verbunden bist.", "Download Server"};
        public static string[] noInternetConn = new string[]
       {   "There is no internet connection!",
           "Es ist keine Internetverbindung vorhanden",};
        public static string[] downloadFailedHelp = new string[]
       {   "Download failed! You can join our steam Group if you need help", "Error",
           "Download fehlgeschlagen! Du kannst unserer Steam Gruppe beitreten, solltest du Hilfe benötigen","Fehler"};
        public static string[] RepairUpdtDone = new string[]
       {   "Repair/Update Completed!", "Server Repair/Update",
           "Reparatur/Update abgeschlossen","Server Reparatur/Update"};
        public static string[] installDone = new string[]
       {   "Installation Completed!",
           "Installation abgeschlossen",};
        public static string[] instFail = new string[]
       {   "Installation failed! You can retry reinstalling the server.",
           "Installation fehlgeschlagen! Du kannst die Installation erneut versuchen."};
        public static string[] generalError = new string[]
       {   "An error occured!", "Error",
           "Ein Fehler ist aufgetreten!","Fehler"};
        public static string[] runUnturnedSrvOnceMsg = new string[]
       {   "You must run your server at least once before you can edit the settings!",
           "Du musst den Server zumindest einmal gestartet haben, um die Einstellungen ändern zu können!"};
        public static string[] srvNotAvailb = new string[]
       {   @"This server is not available yet. You will be able to install it manually using the guide under the ""Extra"" section after it has been released very soon",
           @"Dieser Server ist noch nicht verfügbar. Du kannst diesen manuell mithilfe des Guides der unter ""Extra"" zu finden ist installieren, sobald er verfügbar ist."};
        public static string[] updtSrvMsg = new string[]
       {   @"Are you sure that you want to update this server? In some servers it might delete your settings. Make sure to backup everything you need",
           @"Bist du sicher, dass du den Server updaten möchtest? Bei manchen Servern kann es vorkommen, dass deine Einstellungen gelöscht werden. Gehe sicher, dass du ein Backup anfertigst."};
        public static string[] srvStartFail = new string[]
       {   "Failed to start the server!",
           "Starten des Servers fehlgeschlagen!"};
        public static string[] notice = new string[]
       {   "NOTICE!",
           "BEACHTE!"};
        public static string[] fileNotFound = new string[]
       {   "File not found!",
           "Datei nicht gefunden!"};
        public static string[] calculating = new string[]
       {   "Calculating...",
           "Berechne..."};
        public static string[] foldContainsSrvPossibleDamage = new string[]
       {   "The folder which you selected already contains an installation of the server that you're about to install. Continuing the installation will update the server and might possibly delete any settings and progress. Continue?",
           "Der Ordner, den du ausgewählt hast, enthält bereits eine Server Installation. Das Aktualisieren des Servers könnte eventuelle Einstellungen löschen. Fortfahren?"};
        public static string[] foldContainsSrv = new string[]
        {  "This server already exists in the installation folder which you just selected. Do you want to continue and reinstall the server?",
          "Dieser Server exestiert bereits im Installationsverzeichnis. Möchtest du fortsetzen und den Server reparieren/updaten?"};
        public static string[] toolUpdtAvailb = new string[]
       {  "An new update is available! Do you want to download it?", "Update Available",
          "Ein Update ist verfügbar! Möchtest du es downloaden?", "Update verfügbar"};
        public static string[] instFinishInfo = new string[]
       {   @"For more information and help, you can go under the ""Extra"" section and visit the installation guide or any other sources. You can also join our steam Group chat and ask for help",
           @"Für weitere Informationen und Hilfe gehe unter ""Extra"" und öffne die Installationsanleitung. Desweiteren kannst du unserem Steam Gruppen Chat beitreten für weitere Hilfe."};
        public static string[] generalSrvFileCreateFail = new string[]
       {   "Could not create the file! Close any other programs that might be using it or restart your PC and then retry. You can also create it manually by visiting the server's installation guide",
           "Die CFG Datei konnte nicht erstellt werden! Beende andere Programme, die gearde eventuell darauf zugreifen könnten oder starte deinen PC neu und versuche es erneut. Die Datei kann auch selbst erstellt werden anhand des Server Guides."};
        public static string[] batFail = new string[]
       {   "Could not create the .bat file! Close any other programs that might be using it or restart your PC and then retry. You can also create it manually by visiting the server's installation guide",
           "Die Datei konnte nicht erstellt worden! Beende andere Programme, die gearde eventuell darauf zugreifen könnten oder starte deinen PC neu und versuche es erneut. Die Datei kann auch selbst erstellt werden anhand des Server Guides."};
        public static string[] askCreateCfgFile = new string[]
       {   "Are you sure that you want to create a new configuration file? This file will have some settings preconfigured and explanations on how they work so that you can change your server's settings easily. Continuing will overwrite any existing file.","Create configuration file",
           "Bist du dir sicher, dass du eine neue Konfigurationsdatei erstellen möchtest? Dabei werden einige Optionen bereits festgelegt und erklärt, so das du diese einfach ändern kannst. Fortfahren wird die alte Datei überschreiben.","Erstelle Konfigurationsdatei"};
        public static string[] createBatFile = new string[]
       {   "Do you want to create a batch file for starting the server? This will allow the server to start immediately with some settings preconfigured (player slots, map etc). It is optional. If it already exists, it will be overwritten","Create batch file",
           "Möchtest du eine Batch Datei zum starten des Servers erstellen? Dies erlaubt dir den Server zu starten, und eine Einstellungen zu ändern. (Anzahl der Spieler, Karte etc.) Dies ist optional. Wenn diese Datei bereits existiert, wird diese überschrieben.", "Erstelle Batch Datei"};
        public static string[] noUpdtAvail = new string[]
       {   "There are no updates available!","Check for updates",
           "Es sind keine Updates verfügbar!", "Suche nach Updates"};
        public static string[] repairUpdateSrvMsg = new string[]
       {   "Do you want to continue? This will update your server, or repair any missing files.",
           "Möchtest du fortfahren? Dies wir dein Server updaten, sowie alle beschädigten Dateien reparieren."};
        public static string[] days7repairUpdateSrvMsg = new string[]
       {   "By updating or repairing this server you might break the current world that you are playing. Make sure to backup your world before you continue",
           "Durch das Updaten/Reparieren des Servers könnte die aktuelle Karte beschädigt werden. Stelle sicher, ein Backup anzufertigen, bevor du fortfährst."};
        public static string[] faq = new string[]
            {"The download failed, what should I do?" +
            Environment.NewLine +
            "Mostly you just need to retry the download!" +
            Environment.NewLine +
            Environment.NewLine +
            "Where can I suggest a Server?" +
            Environment.NewLine +
            "You can do this in the linked Group.",
            "Der Download ist fehlgeschlagen, was nun?" +
            Environment.NewLine +
            "Meisten reicht es den Download einfach erneut auszuführen!" +
            Environment.NewLine +
            Environment.NewLine +
            "Wo kann ich einen Server Vorschlag machen?" +
            Environment.NewLine +
            "Dies ist möglich in der verlinken Gruppe"};
        public static string[] cantStartSrv = new string[]
       {   "There was a problem starting the server!", "Start Server",
          "Es gab ein Problem beim starten des Servers!","Server starten"};
        public static string[] cacheCleanSuccess = new string[]
       {   "Cache was cleaned successfully", "Clear cache",
          "Cache wurde erfolgreich gelöscht.", "Leere Cache"};
        public static string[] cleaningCache = new string[]
       {   "Cleaning Cache",
          "Leere Cache Cache"};
        public static string[] generalAreYouSureContinue = new string[]
       {   "Are you sure that you want to continue?",
          "Bist du dir sicher, dass du fortfahren möchtest?"};
        public static string[] cacheCleanFail = new string[]
       {   "Not all cache files could be deleted. Make sure they are not being used by another running process.", "Clear cache",
          "Nicht alle Cache Dateien konnten gelöscht werden. Stelle sicher, dass sie nicht von anderen Prozessen verwendet werden.", "Leere Cache"};
        public static string[] createBatFileAskInternetOrLan = new string[]
       {   "Please select whether the server will run via lan or on the internet",
          "Wähle aus, ob der Server im Lan oder im Internet verfügbar sein soll." };
        public static string[] createBatFileAskToken = new string[]
       {   "Please enter a VALID server token to show" + System.Environment.NewLine + "your server in the server list(Optional)",
          "Bitte geben Sie ein GÜLTIGES Server-Token ein, um Ihren" + System.Environment.NewLine + "Server in der Serverliste anzuzeigen(optional)" };
        public static string[] installSrvUnderRoot = new string[]
       {   @"Its best to create an installation folder for your servers directly in the root folder of the selected drive. For example C:\my_Servers or D:\my_Servers ETC. Also make sure all the letters in the path are in ENGLISH","Select server installation folder",
           "Leere Cache Cache","Wähle Verzeichnis für die Server Installation"};
        public static string[] typeQuitNotify = new string[]
       {   @"After the installation is done, type ""quit"" in the command line to close it properly",
          @"Nachdem die Installation abgeschlossen ist, tippe ""quit"" ein, um das Programm zu schließen."};
        #endregion

        #region change the location and size of controls
        public static int[] welcLbLoc = new int[]
         {  34, 10,//English x - y
            11, 10 };//German x - y
        public static int[] welcDescLbLoc = new int[]
         {  42, 42,
            15, 42 };
        #endregion

        #region Other
        public static string[] askSrvInstDirDesc = new string[]
{          "Please select where would you want to download your servers:",//English
           "Wähle bitte aus, wo der Server erstellt werden soll:"};//German
        #endregion
    }
}
