using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                string cs = string.Format("Version=3;UseUTF16Encoding=True;uri=file:{0}", AppInfoDBLocation);//sony is format 3
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

         

            public static List<Models.AppBrowse> GetAllApps(string UserID, bool OnlyVisible = true)
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

                string SQL = @"SELECT * FROM tbl_appbrowse_0" + UserID + @" Where visible = " + Convert.ToInt32(OnlyVisible) + @"
  and contentId is not null";


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

                if(rtnval != 0)
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



                string SQL = @"select * from savedata
Order by title_id";

                var DataTable = SqlHelper.GetDataTable(SQL, cs);
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
    }
}
