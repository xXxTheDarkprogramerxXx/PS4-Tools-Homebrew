using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.Models
{
    public class Trophy_flag
    {
        public string id { get; set; }
        public string title_id { get; set; }
        public string revision { get; set; }
        public string trophy_title_id { get; set; }
        public string trophyid { get; set; }
        public string groupid { get; set; }
        public string visible { get; set; }
        public string unlocked { get; set; }
        public string unlock_attribute { get; set; }
        public string time_unlocked { get; set; }
        public string time_unlocked_uc { get; set; }
        public string grade { get; set; }
        public string hidden { get; set; }
        public string title { get; set; }
        public string description { get; set; }

    }

    public class Trophy_title
    {
        public string id { get; set; }
        public string user_id { get; set; }
        public string revision { get; set; }
        public string trophy_title_id { get; set; }
        public string status { get; set; }
        public string conf_file_revision { get; set; }
        public string data_file_revision { get; set; }
        public string trop_info_revision { get; set; }
        public string net_trans_file_revision { get; set; }
        public string trophies_conf_file_revision { get; set; }
        public string trophies_data_file_revision { get; set; }
        public string trophies_trop_info_revision { get; set; }
        public string trophies_net_trans_file_revision { get; set; }
        public string progress { get; set; }
        public string unlocked_trophy_num { get; set; }
        public string unlocked_platinum_num { get; set; }
        public string unlocked_gold_num { get; set; }
        public string unlocked_silver_num { get; set; }
        public string unlocked_bronze_num { get; set; }
        public string sync_req_num { get; set; }
        public string sync_pending_num { get; set; }
        public string time_last_synced { get; set; }
        public string time_last_unlocked { get; set; }
        public string time_last_update { get; set; }
        public string time_last_update_uc { get; set; }
        public string instsrc_type { get; set; }
        public string instsrc_attr { get; set; }
        public string instsrc_digest { get; set; }
        public string icon_location { get; set; }
        public string platform { get; set; }
        public string trophy_set_version { get; set; }
        public string trophy_num { get; set; }
        public string platinum_num { get; set; }
        public string silver_num { get; set; }
        public string bronze_num { get; set; }
        public string platinum_set { get; set; }
        public string gold_set { get; set; }
        public string silver_set { get; set; }
        public string bronze_set { get; set; }
        public string group_num { get; set; }
        public string group_set { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }
}
