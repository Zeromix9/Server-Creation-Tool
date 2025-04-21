using Server_creation_tool.classes;
using Server_creation_tool.Properties;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace Server_creation_tool.Server_data_files.palworld
{
    internal class palworld_funcs
    {
        funcsClass funcs = new funcsClass();

        public palworld_funcs(MainForm mainForm)
        {
            mainFrm = mainForm;
        }
        MainForm mainFrm;
        public void setup()
        {

            mainFrm.addExtraBtn(mainFrm.getGeneralLang("load_default_cfg_msg")[2], () =>
              {
                  if (mainFrm.MsgBox.Show(mainFrm.getGeneralLang("load_default_cfg_msg")[1], mainFrm.getGeneralLang("load_default_cfg_msg")[2], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                  {
                      //copy default ini file
                      try
                      {
                          File.Copy(mainFrm.getCurrentInstancePath() + @"\DefaultPalWorldSettings.ini", mainFrm.getCurrentInstancePath() + mainFrm.getServerDataStr("conf_file_path_1"), true);
                          mainFrm.MsgBox.quickMsg(mainFrm.getGeneralLang("load_default_cfg_msg")[2], mainFrm.getGeneralLang("done")[1], 30);
                      }
                      catch
                      {
                          mainFrm.MsgBox.Show(mainFrm.getGeneralLang("error_occured")[1], mainFrm.getGeneralLang("load_default_cfg_msg")[2], MessageBoxButtons.OK, MessageBoxIcon.Error);
                      }
                  }
              }, null, true);
        }

        public void quickSaveSetting(string name, object value)
        {

        }

        public bool startServer()
        {
            bool dontShow;
            try
            {
                dontShow = (bool)Properties.Settings.Default["palworld_recommend_create_bat_file"];
            }
            catch
            {
                SettingsProperty prop = new SettingsProperty(Settings.Default.Properties["baseBool"]);
                prop.Name = "palworld_recommend_create_bat_file";
                prop.DefaultValue = false;
                Properties.Settings.Default.Properties.Add(prop);
            }
            dontShow = (bool)Properties.Settings.Default["palworld_recommend_create_bat_file"];
            mainFrm.MsgBox.Dontshow(ref dontShow, mainFrm.getGeneralLang("recommend_create_bat_file")[1], mainFrm.getControlsLang("create_custom_start_bat_file")[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
            Properties.Settings.Default["palworld_recommend_create_bat_file"] = dontShow;
            Properties.Settings.Default.Save();
            return false;
        }
    }
}
