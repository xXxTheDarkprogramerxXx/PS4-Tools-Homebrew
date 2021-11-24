using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Assets.Code.Data
{
    public class DataProcsessing
    {

        public static class AppInfo
        {
            //db save path
            private static string AppInfoDBLocation = @"/system_data/priv/mms/app.db";
            public static List<Models.AppInfo> GetAppInfo(string TitleId)
            {
                if (Application.platform != RuntimePlatform.PS4)
                {
                    //this is windows
                    AppInfoDBLocation = @"C:\Users\3de Echelon\Desktop\ps4\AppDb\app.db";
                }


                List<Models.AppInfo> dbInfo = new List<Models.AppInfo>();
                string cs = string.Format("Version=3;uri=file:{0}", AppInfoDBLocation);//sony is format 3
                if (!File.Exists(AppInfoDBLocation))
                    throw new Exception("Could not load db");

                string SQL = @"SELECT titleId,
       [key],
        cast(val as TEXT) val
  FROM tbl_appinfo where titleId = '" + TitleId.ToString() + "';";


                System.Data.DataTable dtTemp = SqlHelper.GetDataTable(SQL, cs);
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    Models.AppInfo Info = new Models.AppInfo();
                    Info.titleId = dtTemp.Rows[i].GetValue<string>("titleId");
                    Info.key = dtTemp.Rows[i].GetValue<string>("key");
                    Info.val = dtTemp.Rows[i].GetValue<string>("val");
                    dbInfo.Add(Info);
                }
                return dbInfo;
            }



            public static List<Models.AppBrowse> GetAllApps(string UserID, bool OnlyVisible = false, bool ExcludeFolders = true)
            {
                if (Application.platform != RuntimePlatform.PS4)
                {
                    //this is windows
                    AppInfoDBLocation = @"C:\Users\3de Echelon\Desktop\ps4\AppDb\app.db";
                }


                List<Models.AppBrowse> dbInfo = new List<Models.AppBrowse>();
                string cs = string.Format("Version=3;uri=file:{0}", AppInfoDBLocation);//sony is format 3
                if (!File.Exists(AppInfoDBLocation))
                    throw new Exception("Could not load db");

                string SQL = @"SELECT * FROM tbl_appbrowse_0" + UserID + @" Where visible = " + Convert.ToInt32(OnlyVisible) + @" and folderType = " + Convert.ToInt32(!ExcludeFolders) + @"
  and contentId is not null";

                if (OnlyVisible == false)
                {
                    SQL = @"SELECT * FROM tbl_appbrowse_0" + UserID + @" Where folderType = " + Convert.ToInt32(!ExcludeFolders) + @"
  and contentId is not null and contentSize > 0";
                }

                System.Data.DataTable dtTemp = SqlHelper.GetDataTable(SQL, cs);
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    try
                    {
                        Models.AppBrowse Info = new Models.AppBrowse();
                        Info.titleId = dtTemp.Rows[i].GetValue<string>("titleId");
                        Info.contentId = dtTemp.Rows[i].GetValue<string>("contentId");
                        Info.titleName = dtTemp.Rows[i].GetValue<string>("titleName");
                        Info.metaDataPath = dtTemp.Rows[i].GetValue<string>("metaDataPath");
                        Info.lastAccessTime = dtTemp.Rows[i].GetValue<string>("lastAccessTime");
                        Info.ContentStatus = int.Parse(dtTemp.Rows[i].GetValue<string>("contentStatus"));
                        Info.OnDisc = int.Parse(dtTemp.Rows[i].GetValue<string>("onDisc"));
                        Info.parentalLevel = int.Parse(dtTemp.Rows[i].GetValue<string>("parentalLevel"));
                        Info.visible = Convert.ToBoolean(Convert.ToInt16(dtTemp.Rows[i].GetValue<string>("visible")));
                        Info.sortPriority = int.Parse(dtTemp.Rows[i].GetValue<string>("sortPriority"));
                        Info.pathInfo = long.Parse(dtTemp.Rows[i].GetValue<string>("pathInfo"));
                        Info.lastAccessIndex = int.Parse(dtTemp.Rows[i].GetValue<string>("lastAccessIndex"));
                        Info.dispLocation = int.Parse(dtTemp.Rows[i].GetValue<string>("dispLocation"));
                        Info.canRemove = Convert.ToBoolean(Convert.ToInt16(dtTemp.Rows[i].GetValue<string>("canRemove")));
                        Info.category = dtTemp.Rows[i].GetValue<string>("category");
                        Info.contentType = int.Parse(dtTemp.Rows[i].GetValue<string>("contentType"));
                        Info.pathInfo2 = int.Parse(dtTemp.Rows[i].GetValue<string>("pathInfo2"));
                        Info.presentBoxStatus = int.Parse(dtTemp.Rows[i].GetValue<string>("presentBoxStatus"));
                        Info.entitlement = int.Parse(dtTemp.Rows[i].GetValue<string>("entitlement"));
                        Info.thumbnailUrl = dtTemp.Rows[i].GetValue<string>("thumbnailUrl");
                        Info.lastUpdateTime = dtTemp.Rows[i].GetValue<string>("lastUpdateTime");
                        DateTime playableDate = new DateTime();
                        DateTime.TryParse(dtTemp.Rows[i].GetValue<string>("playableDate"), out playableDate);
                        Info.playableDate = playableDate;
                        Info.contentSize = long.Parse(dtTemp.Rows[i].GetValue<string>("contentSize"));
                        DateTime installDate = new DateTime();
                        DateTime.TryParse(dtTemp.Rows[i].GetValue<string>("installDate"), out installDate);
                        Info.installDate = installDate;
                        Info.platform = int.Parse(dtTemp.Rows[i].GetValue<string>("platform"));
                        Info.uiCategory = dtTemp.Rows[i].GetValue<string>("uiCategory");
                        Info.skuId = dtTemp.Rows[i].GetValue<string>("skuId");
                        Info.disableLiveDetail = int.Parse(dtTemp.Rows[i].GetValue<string>("disableLiveDetail"));
                        Info.linkType = int.Parse(dtTemp.Rows[i].GetValue<string>("linkType"));
                        Info.linkUri = dtTemp.Rows[i].GetValue<string>("linkUri");
                        Info.serviceIdAddCont1 = dtTemp.Rows[i].GetValue<string>("serviceIdAddCont1");
                        Info.serviceIdAddCont2 = dtTemp.Rows[i].GetValue<string>("serviceIdAddCont2");
                        Info.serviceIdAddCont3 = dtTemp.Rows[i].GetValue<string>("serviceIdAddCont3");
                        Info.serviceIdAddCont4 = dtTemp.Rows[i].GetValue<string>("serviceIdAddCont4");
                        Info.serviceIdAddCont5 = dtTemp.Rows[i].GetValue<string>("serviceIdAddCont5");
                        Info.serviceIdAddCont6 = dtTemp.Rows[i].GetValue<string>("serviceIdAddCont6");
                        Info.serviceIdAddCont7 = dtTemp.Rows[i].GetValue<string>("serviceIdAddCont7");
                        Info.folderType = int.Parse(dtTemp.Rows[i].GetValue<string>("folderType"));
                        Info.folderInfo = dtTemp.Rows[i].GetValue<string>("folderInfo");
                        Info.parentFolderId = dtTemp.Rows[i].GetValue<string>("parentFolderId");
                        Info.positionInFolder = dtTemp.Rows[i].GetValue<string>("positionInFolder");
                        DateTime activeDate = new DateTime();
                        DateTime.TryParse(dtTemp.Rows[i].GetValue<string>("activeDate"), out activeDate);
                        Info.activeDate = activeDate;
                        Info.entitlementTitleName = dtTemp.Rows[i].GetValue<string>("entitlementTitleName");
                        Info.hddLocation = int.Parse(dtTemp.Rows[i].GetValue<string>("hddLocation"));
                        Info.externalHddAppStatus = int.Parse(dtTemp.Rows[i].GetValue<string>("externalHddAppStatus"));
                        Info.entitlementIdKamaji = dtTemp.Rows[i].GetValue<string>("entitlementIdKamaji");
                        DateTime mTime = new DateTime();
                        DateTime.TryParse(dtTemp.Rows[i].GetValue<string>("mTime"), out mTime);
                        Info.mTime = mTime;
                        dbInfo.Add(Info);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return dbInfo;
            }

        }

        public static class SaveData
        {
            /// <summary>
            /// Location of save data db on PS4 System
            /// Note: its per user
            /// </summary>
            private static string SaveDataoDBLocation = @"/system_data/savedata/" + MainClass.Get_UserId() + "/db/user/savedata.db";
            public static long GetAppSize(string TitleId)
            {
                if (Application.platform != RuntimePlatform.PS4)
                {
                    //this is windows
                    SaveDataoDBLocation = @"C:\Users\3de Echelon\Desktop\ps4\Saves\savedata.db";

                    //testing MWF
                    SaveDataoDBLocation = @"C:\Users\3de Echelon\Downloads\savedata.db";
                }
                else
                {
                    int UserId = 0;
                    int.TryParse(MainClass.Get_UserId(), out UserId);
                    string userDirectory = UserId.ToString("x");

                    SaveDataoDBLocation = @"/system_data/savedata/" + userDirectory + "/db/user/savedata.db";
                }

                long rtnval = 0;
                string cs = string.Format("Version=3;UseUTF16Encoding=True;uri=file:{0}", SaveDataoDBLocation);//sony is format 3
                if (!File.Exists(SaveDataoDBLocation))
                    throw new Exception("Could not load db");

                string SQL = @"SELECT SUM(size_kib) Size
  FROM savedata where title_id = '" + TitleId + "';";

                var val = SqlHelper.GetSingleValue(SQL, cs);

                long.TryParse(val.ToString(), out rtnval);

                if (rtnval != 0)
                {
                    rtnval = rtnval * 1024;
                }

                return rtnval;
            }
            public static List<Models.SaveData> GetAllSaves()
            {
                List<Models.SaveData> saves = new List<Models.SaveData>();

                if (Application.platform != RuntimePlatform.PS4)
                {
                    SaveDataoDBLocation = @"C:\Users\3de Echelon\Desktop\ps4\Saves\savedata.db";

                    SaveDataoDBLocation = @"C:\Users\3de Echelon\Downloads\savedata.db";
                }
                else
                {
                    int UserId = 0;
                    int.TryParse(MainClass.Get_UserId(), out UserId);
                    string userDirectory = UserId.ToString("x");

                    SaveDataoDBLocation = @"/system_data/savedata/" + userDirectory + "/db/user/savedata.db";
                }

                string cs = string.Format("Version=3;uri=file:{0}", SaveDataoDBLocation);//sony is format 3
                if (!File.Exists(SaveDataoDBLocation))
                    throw new Exception("Could not load db");

                string SQLLookup = "PRAGMA table_info(savedata);";
                var dt = SqlHelper.GetDataTable(SQLLookup, cs);
                string ColNames = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["type"].ToString() == "")
                    {
                        ColNames += "cast(" + dt.Rows[i]["name"].ToString() + " as TEXT) '" + dt.Rows[i]["name"].ToString() + "', ";
                    }
                    else
                    {
                        ColNames += dt.Rows[i]["name"].ToString() + " , ";
                    }

                }
                ColNames = ColNames.Remove(ColNames.Length - 2, 2);//remove last ,


                string SQL = @"select " + ColNames + @" from savedata
Order by title_id";


                System.Data.DataTable DT = new System.Data.DataTable();
                Models.SaveData item = new Models.SaveData();
                Type type = item.GetType();

                IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());

                foreach (PropertyInfo prop in props)
                {
                    object propValue = prop.GetValue(item, null);

                    //switch(prop.PropertyType.ToString())
                    //{
                    //    case "System.Int32":
                    //        DT.Columns.Add(, typeof(string));
                    //        break;
                    //    default:
                    //        break;
                    //}

                    // Do something with propValue

                    //check what is propvalue
                    //var holder = propValue.GetType();
                    //this is cool
                    DT.Columns.Add(prop.Name, prop.PropertyType);


                }

                var DataTable = SqlHelper.GetDataTable(SQL, cs, DT);
                for (int i = 0; i < DataTable.Rows.Count; i++)
                {
                    Models.SaveData saveitem = new Models.SaveData();
                    saveitem.account_id = DataTable.Rows[i].GetValue<string>("account_id");
                    saveitem.blocks = DataTable.Rows[i].GetValue<string>("blocks");
                    saveitem.cloud_icon_url = DataTable.Rows[i].GetValue<string>("cloud_icon_url");
                    saveitem.cloud_revision = DataTable.Rows[i].GetValue<string>("cloud_revision");
                    saveitem.detail = DataTable.Rows[i].GetValue<string>("detail");
                    saveitem.dir_name = DataTable.Rows[i].GetValue<string>("dir_name");
                    saveitem.faked_owner = DataTable.Rows[i].GetValue<string>("faked_owner");
                    saveitem.fake_broken = DataTable.Rows[i].GetValue<string>("fake_broken");
                    saveitem.free_blocks = DataTable.Rows[i].GetValue<string>("free_blocks");
                    saveitem.game_title_id = DataTable.Rows[i].GetValue<string>("game_title_id");
                    saveitem.id = Convert.ToInt32(DataTable.Rows[i].GetValue<string>("id"));
                    saveitem.is_broken = DataTable.Rows[i].GetValue<string>("is_broken");
                    saveitem.main_title = DataTable.Rows[i].GetValue<string>("main_title");
                    saveitem.mtime = DataTable.Rows[i].GetValue<string>("mtime");
                    saveitem.size_kib = DataTable.Rows[i].GetValue<string>("size_kib");
                    saveitem.sub_title = DataTable.Rows[i].GetValue<string>("sub_title");
                    saveitem.title_id = DataTable.Rows[i].GetValue<string>("title_id");
                    saveitem.tmp_dir_name = DataTable.Rows[i].GetValue<string>("tmp_dir_name");
                    saveitem.user_id = DataTable.Rows[i].GetValue<string>("user_id");
                    saveitem.user_param = DataTable.Rows[i].GetValue<string>("user_param");
                    saves.Add(saveitem);
                }

                return saves;
            }
        }


        public static class Trophy
        {
            //Location of TrophyDB
            private static string TrophyDBLocation = @"/user/home/" + MainClass.Get_UserId() + "/trophy/db/trophy_local.db";

            public static List<Models.Trophy_title> GetAllTrophies()
            {
                List<Models.Trophy_title> saves = new List<Models.Trophy_title>();

                if (Application.platform != RuntimePlatform.PS4)
                {
                    TrophyDBLocation = @"C:\Publish\Sony\trophy_local.db";
                }
                else
                {
                    int UserId = 0;
                    int.TryParse(MainClass.Get_UserId(), out UserId);
                    string userDirectory = UserId.ToString("x");

                    TrophyDBLocation = @"/user/home/" + userDirectory + "/trophy/db/trophy_local.db";
                }

                string cs = string.Format("Version=3;uri=file:{0}", TrophyDBLocation);//sony is format 3
                if (!File.Exists(TrophyDBLocation))
                    throw new Exception("Could not load db");




                string SQLLookup = "PRAGMA table_info(tbl_trophy_title);";
                var dt = SqlHelper.GetDataTable(SQLLookup, cs);
                string ColNames = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["type"].ToString() == "")
                    {
                        ColNames += "cast(" + dt.Rows[i]["name"].ToString() + " as TEXT) '" + dt.Rows[i]["name"].ToString() + "', ";
                    }
                    else
                    {
                        ColNames += dt.Rows[i]["name"].ToString() + " , ";
                    }

                }
                ColNames = ColNames.Remove(ColNames.Length - 2, 2);//remove last ,


                string SQL = @"SELECT "+ ColNames + " FROM tbl_trophy_title";

                var DataTable = SqlHelper.GetDataTable(SQL, cs);
                for (int i = 0; i < DataTable.Rows.Count; i++)
                {
                    Models.Trophy_title saveitem = new Models.Trophy_title();
                    saveitem.bronze_num = DataTable.Rows[i].GetValue<string>("bronze_num");
                    saveitem.bronze_set = DataTable.Rows[i].GetValue<string>("bronze_set");
                    saveitem.conf_file_revision = DataTable.Rows[i].GetValue<string>("conf_file_revision");
                    saveitem.data_file_revision = DataTable.Rows[i].GetValue<string>("data_file_revision");
                    saveitem.description = DataTable.Rows[i].GetValue<string>("description");
                    saveitem.gold_set = DataTable.Rows[i].GetValue<string>("gold_set");
                    saveitem.group_num = DataTable.Rows[i].GetValue<string>("group_num");
                    saveitem.group_set = DataTable.Rows[i].GetValue<string>("group_set");
                    saveitem.icon_location = DataTable.Rows[i].GetValue<string>("icon_location");
                    saveitem.id = DataTable.Rows[i].GetValue<string>("id");
                    saveitem.instsrc_attr = DataTable.Rows[i].GetValue<string>("instsrc_attr");
                    saveitem.instsrc_digest = DataTable.Rows[i].GetValue<string>("instsrc_digest");
                    saveitem.instsrc_type = DataTable.Rows[i].GetValue<string>("instsrc_type");
                    saveitem.net_trans_file_revision = DataTable.Rows[i].GetValue<string>("net_trans_file_revision");
                    saveitem.platform = DataTable.Rows[i].GetValue<string>("platform");
                    saveitem.platinum_num = DataTable.Rows[i].GetValue<string>("platinum_num");
                    saveitem.platinum_set = DataTable.Rows[i].GetValue<string>("platinum_set");
                    saveitem.progress = DataTable.Rows[i].GetValue<string>("progress");
                    saveitem.revision = DataTable.Rows[i].GetValue<string>("revision");
                    saveitem.silver_num = DataTable.Rows[i].GetValue<string>("silver_num");
                    saveitem.silver_set = DataTable.Rows[i].GetValue<string>("silver_set");
                    saveitem.status = DataTable.Rows[i].GetValue<string>("status");
                    saveitem.sync_pending_num = DataTable.Rows[i].GetValue<string>("sync_pending_num");
                    saveitem.sync_req_num = DataTable.Rows[i].GetValue<string>("sync_req_num");
                    saveitem.time_last_synced = DataTable.Rows[i].GetValue<string>("time_last_synced");
                    saveitem.time_last_unlocked = DataTable.Rows[i].GetValue<string>("time_last_unlocked");
                    saveitem.time_last_update = DataTable.Rows[i].GetValue<string>("time_last_update");
                    saveitem.time_last_update_uc = DataTable.Rows[i].GetValue<string>("time_last_update_uc");
                    saveitem.title = DataTable.Rows[i].GetValue<string>("title");
                    saveitem.trophies_conf_file_revision = DataTable.Rows[i].GetValue<string>("trophies_conf_file_revision");
                    saveitem.trophies_data_file_revision = DataTable.Rows[i].GetValue<string>("trophies_data_file_revision");
                    saveitem.trophies_net_trans_file_revision = DataTable.Rows[i].GetValue<string>("trophies_net_trans_file_revision");
                    saveitem.trophies_trop_info_revision = DataTable.Rows[i].GetValue<string>("trophies_trop_info_revision");
                    saveitem.trophy_num = DataTable.Rows[i].GetValue<string>("trophy_num");
                    saveitem.trophy_set_version = DataTable.Rows[i].GetValue<string>("trophy_set_version");
                    saveitem.trophy_title_id = DataTable.Rows[i].GetValue<string>("trophy_title_id");
                    saveitem.trop_info_revision = DataTable.Rows[i].GetValue<string>("trop_info_revision");
                    saveitem.unlocked_bronze_num = DataTable.Rows[i].GetValue<string>("unlocked_bronze_num");
                    saveitem.unlocked_gold_num = DataTable.Rows[i].GetValue<string>("unlocked_gold_num");
                    saveitem.unlocked_platinum_num = DataTable.Rows[i].GetValue<string>("unlocked_platinum_num");
                    saveitem.unlocked_silver_num = DataTable.Rows[i].GetValue<string>("unlocked_silver_num");
                    saveitem.unlocked_trophy_num = DataTable.Rows[i].GetValue<string>("unlocked_trophy_num");
                    saveitem.user_id = DataTable.Rows[i].GetValue<string>("user_id");
                    saves.Add(saveitem);
                }

                return saves;
            }

            public static Models.Trophy_title GetSpesificTrophy(string trophy_title_id)
            {
                Models.Trophy_title saves = new Models.Trophy_title();

                if (Application.platform != RuntimePlatform.PS4)
                {
                    TrophyDBLocation = @"C:\Publish\Sony\trophy_local.db";
                }
                else
                {
                    int UserId = 0;
                    int.TryParse(MainClass.Get_UserId(), out UserId);
                    string userDirectory = UserId.ToString("x");

                    TrophyDBLocation = @"/user/home/" + userDirectory + "/trophy/db/trophy_local.db";
                }

                string cs = string.Format("Version=3;uri=file:{0}", TrophyDBLocation);//sony is format 3
                if (!File.Exists(TrophyDBLocation))
                    throw new Exception("Could not load db");

                string SQLLookup = "PRAGMA table_info(tbl_trophy_title);";
                var dt = SqlHelper.GetDataTable(SQLLookup, cs);
                string ColNames = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["type"].ToString() == "")
                    {
                        ColNames += "cast(" + dt.Rows[i]["name"].ToString() + " as TEXT) '" + dt.Rows[i]["name"].ToString() + "', ";
                    }
                    else
                    {
                        ColNames += dt.Rows[i]["name"].ToString() + " , ";
                    }

                }
                ColNames = ColNames.Remove(ColNames.Length - 2, 2);//remove last ,


                string SQL = @"SELECT "+ ColNames + @"FROM tbl_trophy_title
Where trophy_title_id = '" + trophy_title_id + "'";

                var DataTable = SqlHelper.GetDataTable(SQL, cs);
                //for (int i = 0; i < DataTable.Rows.Count; i++)
                {
                    Models.Trophy_title saveitem = new Models.Trophy_title();
                    saveitem.bronze_num = DataTable.Rows[0].GetValue<string>("bronze_num");
                    saveitem.bronze_set = DataTable.Rows[0].GetValue<string>("bronze_set");
                    saveitem.conf_file_revision = DataTable.Rows[0].GetValue<string>("conf_file_revision");
                    saveitem.data_file_revision = DataTable.Rows[0].GetValue<string>("data_file_revision");
                    saveitem.description = DataTable.Rows[0].GetValue<string>("description");
                    saveitem.gold_set = DataTable.Rows[0].GetValue<string>("gold_set");
                    saveitem.group_num = DataTable.Rows[0].GetValue<string>("group_num");
                    saveitem.group_set = DataTable.Rows[0].GetValue<string>("group_set");
                    saveitem.icon_location = DataTable.Rows[0].GetValue<string>("icon_location");
                    saveitem.id = DataTable.Rows[0].GetValue<string>("id");
                    saveitem.instsrc_attr = DataTable.Rows[0].GetValue<string>("instsrc_attr");
                    saveitem.instsrc_digest = DataTable.Rows[0].GetValue<string>("instsrc_digest");
                    saveitem.instsrc_type = DataTable.Rows[0].GetValue<string>("instsrc_type");
                    saveitem.net_trans_file_revision = DataTable.Rows[0].GetValue<string>("net_trans_file_revision");
                    saveitem.platform = DataTable.Rows[0].GetValue<string>("platform");
                    saveitem.platinum_num = DataTable.Rows[0].GetValue<string>("platinum_num");
                    saveitem.platinum_set = DataTable.Rows[0].GetValue<string>("platinum_set");
                    saveitem.progress = DataTable.Rows[0].GetValue<string>("progress");
                    saveitem.revision = DataTable.Rows[0].GetValue<string>("revision");
                    saveitem.silver_num = DataTable.Rows[0].GetValue<string>("silver_num");
                    saveitem.silver_set = DataTable.Rows[0].GetValue<string>("silver_set");
                    saveitem.status = DataTable.Rows[0].GetValue<string>("status");
                    saveitem.sync_pending_num = DataTable.Rows[0].GetValue<string>("sync_pending_num");
                    saveitem.sync_req_num = DataTable.Rows[0].GetValue<string>("sync_req_num");
                    saveitem.time_last_synced = DataTable.Rows[0].GetValue<string>("time_last_synced");
                    saveitem.time_last_unlocked = DataTable.Rows[0].GetValue<string>("time_last_unlocked");
                    saveitem.time_last_update = DataTable.Rows[0].GetValue<string>("time_last_update");
                    saveitem.time_last_update_uc = DataTable.Rows[0].GetValue<string>("time_last_update_uc");
                    saveitem.title = DataTable.Rows[0].GetValue<string>("title");
                    saveitem.trophies_conf_file_revision = DataTable.Rows[0].GetValue<string>("trophies_conf_file_revision");
                    saveitem.trophies_data_file_revision = DataTable.Rows[0].GetValue<string>("trophies_data_file_revision");
                    saveitem.trophies_net_trans_file_revision = DataTable.Rows[0].GetValue<string>("trophies_net_trans_file_revision");
                    saveitem.trophies_trop_info_revision = DataTable.Rows[0].GetValue<string>("trophies_trop_info_revision");
                    saveitem.trophy_num = DataTable.Rows[0].GetValue<string>("trophy_num");
                    saveitem.trophy_set_version = DataTable.Rows[0].GetValue<string>("trophy_set_version");
                    saveitem.trophy_title_id = DataTable.Rows[0].GetValue<string>("trophy_title_id");
                    saveitem.trop_info_revision = DataTable.Rows[0].GetValue<string>("trop_info_revision");
                    saveitem.unlocked_bronze_num = DataTable.Rows[0].GetValue<string>("unlocked_bronze_num");
                    saveitem.unlocked_gold_num = DataTable.Rows[0].GetValue<string>("unlocked_gold_num");
                    saveitem.unlocked_platinum_num = DataTable.Rows[0].GetValue<string>("unlocked_platinum_num");
                    saveitem.unlocked_silver_num = DataTable.Rows[0].GetValue<string>("unlocked_silver_num");
                    saveitem.unlocked_trophy_num = DataTable.Rows[0].GetValue<string>("unlocked_trophy_num");
                    saveitem.user_id = DataTable.Rows[0].GetValue<string>("user_id");
                    return saveitem;
                }

                return saves;
            }

            public static List<Models.Trophy_flag> GetAllTrophyFlags(string trophy_title_id)
            {
                List<Models.Trophy_flag> saves = new List<Models.Trophy_flag>();

                if (Application.platform != RuntimePlatform.PS4)
                {
                    TrophyDBLocation = @"C:\Publish\Sony\trophy_local.db";
                }
                else
                {
                    int UserId = 0;
                    int.TryParse(MainClass.Get_UserId(), out UserId);
                    string userDirectory = UserId.ToString("x");

                    TrophyDBLocation = @"/user/home/" + userDirectory + "/trophy/db/trophy_local.db";
                }

                string cs = string.Format("Version=3;uri=file:{0}", TrophyDBLocation);//sony is format 3
                if (!File.Exists(TrophyDBLocation))
                    throw new Exception("Could not load db");




                //SQL += "'";
                //string tmp = trophy_title_id.Replace("_00", "");
                string tmp = string.Copy(trophy_title_id.Replace("_00", ""));

                string SQLLookup = "PRAGMA table_info(tbl_trophy_flag);";
                var dt = SqlHelper.GetDataTable(SQLLookup, cs);
                string ColNames = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["type"].ToString() == "")
                    {
                        ColNames += "cast(" + dt.Rows[i]["name"].ToString() + " as TEXT) '" + dt.Rows[i]["name"].ToString() + "', ";
                    }
                    else
                    {
                        ColNames += dt.Rows[i]["name"].ToString() + " , ";
                    }

                }
                ColNames = ColNames.Remove(ColNames.Length - 2, 2);//remove last ,

                string SQL = @"SELECT "+ ColNames + @" FROM tbl_trophy_flag";
                //where trophy_title_id = '" + tmp + @"'";
                //SQL = string.Concat(SQL, "\n'");
                var DataTable = SqlHelper.GetDataTable(SQL, cs);
                for (int i = 0; i < DataTable.Rows.Count; i++)
                {
                    try
                    {
                        Models.Trophy_flag tmpitem = new Models.Trophy_flag();
                        tmpitem.description = DataTable.Rows[i].GetValue<string>("description");
                        tmpitem.grade = DataTable.Rows[i].GetValue<string>("grade");
                        tmpitem.groupid = DataTable.Rows[i].GetValue<string>("groupid");
                        tmpitem.hidden = DataTable.Rows[i].GetValue<string>("hidden");
                        tmpitem.id = DataTable.Rows[i].GetValue<string>("id");
                        tmpitem.revision = DataTable.Rows[i].GetValue<string>("revision");
                        tmpitem.time_unlocked = DataTable.Rows[i].GetValue<string>("time_unlocked");
                        tmpitem.time_unlocked_uc = DataTable.Rows[i].GetValue<string>("time_unlocked_uc");
                        tmpitem.title = DataTable.Rows[i].GetValue<string>("title");
                        tmpitem.title_id = DataTable.Rows[i].GetValue<string>("title_id");
                        tmpitem.trophyid = DataTable.Rows[i].GetValue<string>("trophyid");
                        tmpitem.trophy_title_id = DataTable.Rows[i].GetValue<string>("trophy_title_id");
                        tmpitem.unlocked = DataTable.Rows[i].GetValue<string>("unlocked");
                        tmpitem.unlock_attribute = DataTable.Rows[i].GetValue<string>("unlock_attribute");
                        tmpitem.visible = DataTable.Rows[i].GetValue<string>("visible");
                        saves.Add(tmpitem);
                    }
                    catch (Exception e)
                    {

                    }
                }

                return saves;
            }
        }
    }
}
