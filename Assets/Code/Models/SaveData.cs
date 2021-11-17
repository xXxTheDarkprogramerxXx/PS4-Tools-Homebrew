using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.Models
{
    public class SaveData
    {
        public int id { get; set; }
        public string title_id { get; set; }
        public string dir_name { get; set; }
        public string main_title { get; set; }
        public string sub_title { get; set; }
        public string detail { get; set; }
        public string tmp_dir_name { get; set; }
        public string is_broken { get; set; }
        public string user_param { get; set; }
        public string blocks { get; set; }
        public string free_blocks { get; set; }
        public string size_kib { get; set; }
        public string mtime { get; set; }
        public string fake_broken { get; set; }
        public string account_id { get; set; }
        public string user_id { get; set; }
        public string faked_owner { get; set; }
        public string cloud_icon_url { get; set; }
        public string cloud_revision { get; set; }
        public string game_title_id { get; set; }
    }
}
