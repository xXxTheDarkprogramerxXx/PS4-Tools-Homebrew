using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.Data
{
    public static class SettingsData
    {
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
            settings.SettingValue = PlayerPrefs.GetString("EnableSaveDB", "Enabled");
            rnt.Add(settings);

            settings = new Models.Model_Settings();
            settings.SettingName = "Enable Chihiro API";
            settings.SettingDescription = "This will enable the PSN store API (used for save data)";
            settings.SettingPref = "EnableChihiroAPI";
            settings.SettingValue = PlayerPrefs.GetString("EnableChihiroAPI", "Enabled");
            rnt.Add(settings);

            settings = new Models.Model_Settings();
            settings.SettingName = "Trophy Util Settings";
            settings.SettingTitle = true;
            rnt.Add(settings);

            settings = new Models.Model_Settings();
            settings.SettingName = "Enable Warning Message On Trophy Util";
            settings.SettingDescription = "This will show a warning message on the trophy util";
            settings.SettingPref = "EnableTrophyUtilWarning";
            settings.SettingValue = PlayerPrefs.GetString("EnableTrophyUtilWarning", "Enabled");
            rnt.Add(settings);

            settings = new Models.Model_Settings();
            settings.SettingName = "Use PS4 Database Trophy Unlock Method";
            settings.SettingDescription = "Use this method to unlock trophies via the trophy database";
            settings.SettingPref = "EnableTrophyDBUnlocking";
            settings.SettingValue = PlayerPrefs.GetString("EnableTrophyDBUnlocking", "Disabled");
            rnt.Add(settings);

            settings = new Models.Model_Settings();
            settings.SettingName = "Use PS4 Trophy Patching Unlock Method";
            settings.SettingDescription = "Use this method to unlock trophies the same way games would";
            settings.SettingPref = "EnableTrophySDKUnlocking";
            settings.SettingValue = PlayerPrefs.GetString("EnableTrophySDKUnlocking", "Enabled");
            rnt.Add(settings);

            settings = new Models.Model_Settings();
            settings.SettingName = "System Settings";
            settings.SettingTitle = true;
            rnt.Add(settings);

            settings = new Models.Model_Settings();
            settings.SettingName = "Load FTP on startup";
            settings.SettingDescription = "This will load the FTP Server on app load";
            settings.SettingPref = "EnableFTPOnBoot";
            settings.SettingValue = PlayerPrefs.GetString("EnableFTPOnBoot", "Enabled");
            rnt.Add(settings);


            return rnt;
        }

    }
}
