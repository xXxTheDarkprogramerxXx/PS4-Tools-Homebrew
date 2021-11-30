using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.Models
{
    public class Model_Settings
    {
        public string SettingName { get; set; }
        public string SettingDescription { get; set; }
        public string SettingValue { get; set; }
        public string SettingPref { get; set; }

        public bool SettingTitle { get; set; }
    }
}
