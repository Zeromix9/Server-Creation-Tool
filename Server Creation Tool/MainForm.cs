/*
 * Created by SharpDevelop.
 * User: Zeromix
 * Date: 28.05.2016
 * Time: 16:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Forms;
using System.IO;

namespace Server_Creation_Tool
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		//muss mal kurz googlen... war in letzter zeit nur in Java am entwickeln
		String steamCmdSpeicherort = null;
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			
			if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
			{
			    using (System.Net.WebClient client = new System.Net.WebClient())
			    {

		            //try
		            //{
	                // Create a new instance of FolderBrowserDialog.
	                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
	                // A new folder button will display in FolderBrowserDialog.
	                folderBrowserDialog.ShowNewFolderButton = true;
	                //Show FolderBrowserDialog
	                DialogResult dlgResult = folderBrowserDialog.ShowDialog();
	                if (dlgResult.Equals(DialogResult.OK))
	                {
	                    //Send content to string
	                    String folder = folderBrowserDialog.SelectedPath;
	                    MessageBox.Show(folder.ToString());
	                    client.DownloadFileAsync(new Uri("https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip"),folder+"\\steamcmd.zip");
	               
	                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadSteamCmdZip);
	                    steamCmdSpeicherort = Path.Combine(folder, "steamcmd.exe");
	                }
	                else
	                {
	                	MessageBox.Show("No folder for storing the downloaded files selected. Can not continue.");
	                }
	          
	 			
	             
			 			
			    }                  
			}
			
			
		}
		
		public void DownloadSteamCmdZip(object sender, AsyncCompletedEventArgs e)
		{
			//nun wurde die zip-Datei fertig gedownloadet.
			MessageBox.Show("Download completed!");
		}
		
		
		
		void Button2Click(object sender, EventArgs e)
		{	
			try{ if(OpenSteamCmd() == true) { System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./ark/ +app_update 376030 validate"); }
			}
				catch (Exception) { ;} //ARK: Survivel Evolved  
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			try{ if(OpenSteamCmd() == true) { System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./css/ +app_update 232330 validate"); }
			}
				catch (Exception) { ;} //CS:S
	
		}
		
		void Button4Click(object sender, EventArgs e)
		{
				try
				{
				if(OpenSteamCmd() == true) 
				{ 
					System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./csgo/ +app_update 740 validate"); }
				}
				catch (Exception) { ;} //CS:GO
		
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			try
			{
				if(OpenSteamCmd() == true)
				{
					System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./cs/ +app_update 90 validate"); 
				}
			}
            catch (Exception) { ;} //CS 1.6
		
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			try{ 
				if(OpenSteamCmd() == true) 
				{ 
					System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./gmod/ +app_update 4020 validate"); 
				}
			}
				catch (Exception) { ;} //Garry´s Mod
	
		}
		
		void Button7Click(object sender, EventArgs e)
		{
			try{ 
				if(OpenSteamCmd() == true) 
				{ 
					System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./7days/ +app_update 294420 validate"); 
				}
			}
				catch (Exception) { ;} //7 Days to Die
		
		}
		
		void Button8Click(object sender, EventArgs e)
		{
			try{ 
				if(OpenSteamCmd() == true) 
				{ 
					System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./kf2/ +app_update 232130 validate"); 
				}
			}
				catch (Exception) { ;} //Killing Floor 2
		
		}
		
		void Button9Click(object sender, EventArgs e)
		{
			try{ 
				if(OpenSteamCmd() == true) 
				{ 
					System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./l4d/ +app_update 222840 validate"); 
				}
			}
				catch (Exception) { ;} //Left 4 Dead
	
		}
		
		void Button10Click(object sender, EventArgs e)
		{
			try{ 
				if(OpenSteamCmd() == true) 
				{ 
					System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./l4d2/ +app_update 222860 validate"); 
				}
			}
				catch (Exception) { ;} //Left 4 Dead 2
	
		}
		
		void Button11Click(object sender, EventArgs e)
		{
			try{ 
				if(OpenSteamCmd() == true) 
				{ 
					System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./svencoop/ +app_update 276060 validate"); 
				}
			}
				catch (Exception) { ;} //Sven Coop
	
		}
		
		void Button12Click(object sender, EventArgs e)
		{
			try{ 
				if(OpenSteamCmd() == true) 
				{ 
					System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./rust/ +app_update 258550 validate"); 
				}
			}
				catch (Exception) { ;} //Rust
		
		}
		
		void Button13Click(object sender, EventArgs e)
		{
			try{ 
				if(OpenSteamCmd() == true) 
				{ 
					System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./sniperelitev2/ +app_update 208050 validate"); 
				}
			}
				catch (Exception) { ;} //Sniper Elite V2
		
		}
		
		void Button14Click(object sender, EventArgs e)
		{
			try{ 
				if(OpenSteamCmd() == true) 
				{ 
					System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./sniperelite3/ +app_update 266910 validate"); 
				}
			}
				catch (Exception) { ;} //Sniper Elite 3
		
		}
		
		void Button15Click(object sender, EventArgs e)
		{
			try{ 
				if(OpenSteamCmd() == true) 
				{ 
					System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./synergy/ +app_update 17525 validate");
				}
			}
				catch (Exception) { ;} //Synergy
	
		}
		
		void Button16Click(object sender, EventArgs e)
		{
			try{ 
				if(OpenSteamCmd() == true) 
				{
					System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./tf2/ +app_update 232250 validate"); 
				}
			}
				catch (Exception) { ;} //Team Fortress 2
			
		}
		
		
		void AboutToolStripMenuItem1Click(object sender, EventArgs e)
		{
		Environment.Exit(0); //Schließen des Programmes	
		}
		

		
		void ÖToolStripMenuItemClick(object sender, EventArgs e)
		{
		System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=694475841");
		//ARK: Survival Evolved Guide English
		}
		
		void FToolStripMenuItemClick(object sender, EventArgs e)
		{
		System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=397365275");
		//Counter-Strike: Source Guide English			
		}
		
		void ARKSurvivalEvolcvToolStripMenuItemClick(object sender, EventArgs e)
		{
		System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=692129985");
		//ARK: Survival Evolved Guide German	
		}
		
		void CounterStrike16ToolStripMenuItemClick(object sender, EventArgs e)
		{
		System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=710511353");
		//Counter-Strike 1.6 Guide German
		}
		
		void CounterStrikeSourceToolStripMenuItemClick(object sender, EventArgs e)
		{
		System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=394849912");
		//Counter-Strike Source Guide German			
		}
		
		void ChangelogToolStripMenuItemClick(object sender, EventArgs e)
		{
			MessageBox.Show("03.03.2019 - Version 2.5 - Enabled Counter-Strike 1.6 again" +
                            Environment.NewLine +
                            Environment.NewLine +
                            "24.11.2018 - Version 2.4 - Added Rust German Server Guide, also deleted a message box" +
                            Environment.NewLine +
                            Environment.NewLine +
                            "09.07.2018 - Version 2.3 - Added Rust Server and Rust English Server Guide, also deleted Left4Dead" +
                            Environment.NewLine +
                            Environment.NewLine +
                            "26.04.2018 - Version 2.2 - Added Forwarding Ports to Guides and disabled Counter-Strike" +
                            Environment.NewLine +
                            Environment.NewLine +
                            "17.04.2017 - Version 2.1 - Added Left 4 Dead Server and updated the FAQ" +
							Environment.NewLine +
                            Environment.NewLine +
                            "28.03.2017 - Version 2.0 - Updated File Dialog" +
							Environment.NewLine +
                            Environment.NewLine +
                            "04.03.2017 - Version 1.9 - Added Sven Co-op Server and Guides" +
							Environment.NewLine +
                            Environment.NewLine +
                            "17.12.2016 - Version 1.8 - Added Call of Duty: Black Ops 3 Guides" +
							Environment.NewLine +
                            Environment.NewLine +
                            "15.12.2016 - Version 1.7 - Added Call of Duty: Black Ops 3 Server" +
			                Environment.NewLine +
                            Environment.NewLine +
                            "23.11.2016 - Version 1.6 - Added Killing Floor 2 Guide English" +
			                Environment.NewLine +
                            Environment.NewLine +
                            "05.10.2016 - Version 1.5 - Added Killing Floor 2 Guide German + Server" +
							Environment.NewLine +
                            Environment.NewLine +
                            "18.09.2016 - Version 1.4 - Added Left 4 Dead 2 Guides + Server" +
							Environment.NewLine +
                            Environment.NewLine +
                            "22.08.2016 - Version 1.3 - Added Counter-Strike: Global Offensive Guides" +
			                Environment.NewLine +
                            Environment.NewLine +
                            "22.07.2016 - Version 1.2 - Added Counter-Strike 1.6 Guide English" +
			                 Environment.NewLine +
                             Environment.NewLine +
                            "19.07.2016 - Version 1.1 - Added Garry´s Mod Guides" +
							 Environment.NewLine +
                             Environment.NewLine +
                            "27.06.2016 - Version 1.0 - Release");
		}
		
		void GarrysModToolStripMenuItemClick(object sender, EventArgs e)
		{
		System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=719731406");
		//Garry´s Mod Guide German	
		}
		
		void GarrysModToolStripMenuItem1Click(object sender, EventArgs e)
		{
		System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=723644520");
		//Garry´s Mod Guide Englisch	
		}
		
		void CounterStrike16ToolStripMenuItem1Click(object sender, EventArgs e)
		{
		System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=729345010");
		//Counter-Strike: 1.6 Guide Englisch
		}
		
		void CounterStrikeGlobalOffensiveToolStripMenuItemClick(object sender, EventArgs e)
		{
		System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=711630778");
		//Counter-Strike: Global Offensive Guide German		
		}
		
		void CounterStrikeGlobalOffensiveToolStripMenuItem1Click(object sender, EventArgs e)
		{
		System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=749588143");
		//Counter-Strike: Global Offensive Guide Englisch			
		}
		
		void FAQToolStripMenuItemClick(object sender, EventArgs e)
		{
		MessageBox.Show("The download failed, what should I do?" +
		Environment.NewLine +	
		"Mostly you just need to retry the download!" +
		Environment.NewLine +
		Environment.NewLine +		
		"Where can I suggest a Server?" +
		Environment.NewLine +		
		"You can do this in the linked Group.");
		}
		
		void Left4Dead2ToolStripMenuItemClick(object sender, EventArgs e)
		{
		System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=764005219");
		//Left 4 Dead 2 Guide Englisch		
		}
		
		void Left4Dead2ToolStripMenuItem1Click(object sender, EventArgs e)
		{
		System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=754702686");
		//Left 4 Dead 2 Guide German
		}
		
		void Button17Click(object sender, EventArgs e)
		{
		MessageBox.Show("More Server" +
		                Environment.NewLine +
		                "Installing SourceMod etc." +
						 Environment.NewLine +
						"Maybe skins");	
		}
		
		void KillingFloor2ToolStripMenuItemClick(object sender, EventArgs e)
		{
		System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=775342119");
		//Killing Floor 2 Guide German			
		}
		
		void KillingFloor2ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=804751117");
			//Killing Floor 2 Guide Englisch
		}
		
		void Button18Click(object sender, EventArgs e)
		{
		try{ 
				if(OpenSteamCmd() == true) 
				{ 
					System.Diagnostics.Process.Start(steamCmdSpeicherort, "+login anonymous +force_install_dir ./bo3/ +app_update 545990 validate"); 
				}
			}
				catch (Exception) { ;} //Call of Duty: Black Ops 3		
		}
		
		void AboutToolStripMenuItem2Click(object sender, EventArgs e)
		{
		MessageBox.Show("This Tool was created by Zeromix" +
		Environment.NewLine +                
		"http://steamcommunity.com/id/zeromix" +
		Environment.NewLine +
		Environment.NewLine +
		"And BasisBit" +
         Environment.NewLine +
         "https://github.com/basisbit");

        }
		
		void CallOfDutyBlackOps3ToolStripMenuItemClick(object sender, EventArgs e)
		{
		System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=818635244");
		//Call of Duty: Black Ops 3 Guide German
		}
		
		void CallOfDutyBlackOps3ToolStripMenuItem1Click(object sender, EventArgs e)
		{
		System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=819848285");
		//Call of Duty: Black Ops 3 Guide English	
		}
		
		void SvenCoopToolStripMenuItem1Click(object sender, EventArgs e)
		{
		System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=874080531");
		//Sven Co-op Guide German	
		}
		
		void SvenCoopToolStripMenuItemClick(object sender, EventArgs e)
		{
		System.Diagnostics.Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=874293589");
		//Sven Co-op Guide English
		}
		
		Boolean OpenSteamCmd()
		{
			// wenn Dateiauswahldialog nur anzeigen, wenn kein Pfad, oder ungültige Datei oder nicht steamcmd.exe
			if(steamCmdSpeicherort == null || 
			   !(File.Exists(steamCmdSpeicherort)) ||
			   Directory.Exists(steamCmdSpeicherort) ||
			   !Path.GetExtension(steamCmdSpeicherort).Equals("exe"))
			{
				//TODO Datei auswahl Dialog jetzt anzeigen
				OpenFileDialog auswahlDialog = new OpenFileDialog();
				if(steamCmdSpeicherort != null && Directory.Exists(steamCmdSpeicherort))
				{
				auswahlDialog.InitialDirectory = steamCmdSpeicherort;
				}
				//TODO Filter auf *.exe setzen
				auswahlDialog.Filter = "Steamcmd executable|steamcmd.exe|Any executable (*.exe)|*.exe"; 
				auswahlDialog.FilterIndex = 1; //setzt die Standardauswahl auf steamcmd.exe
				auswahlDialog.RestoreDirectory = true;
				auswahlDialog.Multiselect = false; //erlaubt nur die Auswahl einer einzelnen Datei
				auswahlDialog.CheckFileExists = true;
				if(auswahlDialog.ShowDialog() == DialogResult.OK)
				{
					try
					{
						String result = auswahlDialog.FileName;
						if(!File.Exists(result))
						{
							MessageBox.Show("No file selected. Can not continue.");
							return false; //Der Nutzer hat keine gültige Datei ausgewählt.
						}
						steamCmdSpeicherort = result;
						return true;
					}
					catch(Exception ex)
					{
						MessageBox.Show("Error in OpenSteamCmd! Details: " + ex.Message);
						return false;
					}
				}
				else
				{
					//nutzer hat keine Datei ausgewählt.
					MessageBox.Show("No file selected. Can not continue.");
					return false;
				}
			}
			//laut dem if muss die Datei eine existierende .exe Datei sein. Wir vermuten, dass die steamcmd korrekt gewählt wurde.
			return true;
		}
		
		void ServerGroupToolStripMenuItemClick(object sender, EventArgs e)
		{
System.Diagnostics.Process.Start("http://steamcommunity.com/groups/ServerTool");			
		}

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void forwardingPortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://portforward.com/router.htm");
        }

        private void rustToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://steamcommunity.com/sharedfiles/filedetails/?id=1436222938");
            //Rust Guide English
        }

        private void rUstToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://steamcommunity.com/sharedfiles/filedetails/?id=1444135543");
            //Rust Guide German
        }
    }
}