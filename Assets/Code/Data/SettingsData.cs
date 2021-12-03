using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.Data
{
    public static class SettingsData
    {
        public static void CheckIfItemExists(string Value,string Default)
        {

            string Path = "/data/PS4Tools.ini";
            if (Application.platform != RuntimePlatform.PS4)
            {
                Path = "C:/temp/PS4Tools.ini";
            }
            string FileItesm = File.ReadAllText(Path);
            if (!FileItesm.Contains(Value))
            {
                FileItesm += "\n" + Value + "=" + Default;
                //doesn't exist 
                File.WriteAllText(Path, FileItesm);
            }
        }

        public static void CreateSettingsIni(bool delete = false,List<Models.Model_Settings> settings = null)
        {
            string IniFile = @"EnableSaveDB=Enabled
EnableChihiroAPI=Enabled
EnableTrophyUtilWarning=Enabled
EnableTrophyDBUnlocking=Enabled
EnableTrophyDBUnlocking=Disabled
EnableTrophySDKUnlocking=Enabled
EnableFTPOnBoot=Enabled";

            string Path = "/data/PS4Tools.ini";
            if (Application.platform != RuntimePlatform.PS4)
            {
                Path = "C:/temp/PS4Tools.ini";
            }
            if (delete == true)
            {
                if (File.Exists(Path))
                {
                    File.Delete(Path);
                }
            }
            if (!File.Exists(Path))
            {
                File.WriteAllText(Path, IniFile);
            }

            CheckIfItemExists("EnableSaveDataWarning", "Enabled");

        }

        public static string GetSettingValue(string Setting, string Default)
        {
            string Path = "/data/PS4Tools.ini";
            if (Application.platform != RuntimePlatform.PS4)
            {
                Path = "C:/temp/PS4Tools.ini";
            }
            if (File.Exists(Path))
            {
                


                var lines = File.ReadAllLines(Path);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains(Setting))
                    {
                        return lines[i].Replace(Setting + "=", "");
                    }
                }
                
            }

            return Default;
        }

        public static bool SetSettingValue(string Setting, string Value)
        {
            string Path = "/data/PS4Tools.ini";
            if (Application.platform != RuntimePlatform.PS4)
            {
                Path = "C:/temp/PS4Tools.ini";
            }
            if (File.Exists(Path))
            {
                var lines = File.ReadAllLines(Path);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains(Setting))
                    {
                        lines[i] = Setting + "=" + Value;//change the line

                        File.WriteAllLines(Path, lines);
                        return true;
                    }
                }
            }
            return false;
        }

        public static List<Assets.Code.Models.Model_Settings> GetAllSettings()
        {
            List<Assets.Code.Models.Model_Settings> rnt = new List<Models.Model_Settings>();
            Assets.Code.Models.Model_Settings settings = new Models.Model_Settings();

            

            //we have to go one by one nice /since we don't want to create a SQL table and just use the PS4/Unity save folder


            settings = new Models.Model_Settings();
            settings.SettingName = "Save Data Settings";
            settings.SettingTitle = true;
            rnt.Add(settings);

            settings = new Models.Model_Settings();
            settings.SettingName = "Use PS4 Save DB";
            settings.SettingDescription = "Use the PS4 Save DB to load save information";
            settings.SettingPref = "EnableSaveDB";
            settings.SettingValue = GetSettingValue("EnableSaveDB", "Enabled");
            rnt.Add(settings);

            settings = new Models.Model_Settings();
            settings.SettingName = "Enable Chihiro API";
            settings.SettingDescription = "This will enable the PSN store API (used for save data)";
            settings.SettingPref = "EnableChihiroAPI";
            settings.SettingValue = GetSettingValue("EnableChihiroAPI", "Enabled");
            rnt.Add(settings);

            settings = new Models.Model_Settings();
            settings.SettingName = "Enable Warning Message On Save Data";
            settings.SettingDescription = "This will show a warning message on the save data ulitiy";
            settings.SettingPref = "EnableSaveDataWarning";
            settings.SettingValue = GetSettingValue("EnableSaveDataWarning", "Enabled");
            rnt.Add(settings);

            settings = new Models.Model_Settings();
            settings.SettingName = "Trophy Util Settings";
            settings.SettingTitle = true;
            rnt.Add(settings);

            settings = new Models.Model_Settings();
            settings.SettingName = "Enable Warning Message On Trophy Util";
            settings.SettingDescription = "This will show a warning message on the trophy util";
            settings.SettingPref = "EnableTrophyUtilWarning";
            settings.SettingValue = GetSettingValue("EnableTrophyUtilWarning", "Enabled");
            rnt.Add(settings);

            settings = new Models.Model_Settings();
            settings.SettingName = "Use PS4 Database Trophy Unlock Method";
            settings.SettingDescription = "Use this method to unlock trophies via the trophy database";
            settings.SettingPref = "EnableTrophyDBUnlocking";
            settings.SettingValue = GetSettingValue("EnableTrophyDBUnlocking", "Disabled");
            rnt.Add(settings);

            settings = new Models.Model_Settings();
            settings.SettingName = "Use PS4 Trophy Patching Unlock Method";
            settings.SettingDescription = "Use this method to unlock trophies the same way games would";
            settings.SettingPref = "EnableTrophySDKUnlocking";
            settings.SettingValue = GetSettingValue("EnableTrophySDKUnlocking", "Enabled");
            rnt.Add(settings);

            settings = new Models.Model_Settings();
            settings.SettingName = "System Settings";
            settings.SettingTitle = true;
            rnt.Add(settings);

            settings = new Models.Model_Settings();
            settings.SettingName = "Load FTP on startup";
            settings.SettingDescription = "This will load the FTP Server on app load";
            settings.SettingPref = "EnableFTPOnBoot";
            settings.SettingValue = GetSettingValue("EnableFTPOnBoot", "Enabled");
            rnt.Add(settings);

            //Credits
            settings = new Models.Model_Settings();
            settings.SettingName = "Credits";
            settings.SettingDescription = "This will display the credits";
            settings.SettingTitle = false;
            rnt.Add(settings);

            //create default
            CreateSettingsIni();

            return rnt;
        }

    }
}
