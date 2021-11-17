using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Scenes
{
    public class SaveData
    {

        public class Load
        {



            public void ToDisplay(Canvas MainMenu, Canvas CanvasSaveData, Canvas PKGSelectionCanvas ,RectTransform savecontentPanel, ScrollRect savescrollRect, MainClass main)
            {
                                 //SendMessageToPS4("Something broke");
                MainMenu.gameObject.SetActive(false);//Hide main mene
                CanvasSaveData.gameObject.SetActive(true);//Show Save Screen
                PKGSelectionCanvas.gameObject.SetActive(false);//hide PKG Screen

                MainClass.CurrentItem = 0;//do this for item click handler
                try
                {
                    #region << Old No Longer Required >>
                    /*Old no longer required*/
                    //if (Application.platform == RuntimePlatform.PS4)
                    //{
                    //    Assets.Code.LoadingDialog.Show("Loading save files\nplease wait ...");

                    //    //SendMessageToPS4("Something broke 3");
                    //    int UserId = 0;
                    //    int.TryParse(MainClass.Get_UserId(), out UserId);
                    //    string userDirectory = UserId.ToString("x");
                    //    //SendMessageToPS4("Something broke "+userDirectory);
                    //    //SendMessageToPS4("Loading saves");
                    //    //you can get the userdirectory like this or ask the ps4 for it but this is just easier
                    //    string[] files = System.IO.Directory.GetDirectories(@"/user/home/" + userDirectory + "/savedata/", "*", SearchOption.TopDirectoryOnly);
                    //    string[] savemetafiles = System.IO.Directory.GetDirectories(@"/user/home/" + userDirectory + "/savedata_meta/user", "*", SearchOption.TopDirectoryOnly);

                    //    MainClass.savedatafileitems = new List<MainClass.SaveDataMain>();

                    //    for (int i = 0; i < files.Length; i++)
                    //    {
                    //        MainClass.SaveDataMain datitem = new MainClass.SaveDataMain();
                    //        datitem.SaveFilePath = files[i];
                    //        for (int x = 0; x < savemetafiles.Length; x++)
                    //        {

                    //            string DirSaveName = new DirectoryInfo(datitem.SaveFilePath).Name;
                    //            string DirSaveMetaName = new DirectoryInfo(savemetafiles[x]).Name;
                    //            if (DirSaveName == DirSaveMetaName)//Directories match
                    //            {
                    //                datitem.SaveMetaFilePath = DirSaveMetaName;
                    //                //get all saveenc files in the savedir
                    //                string[] encsavefiles = System.IO.Directory.GetFiles(files[i], "*.bin", SearchOption.TopDirectoryOnly);//get all encyrption files
                    //                for (int ix = 0; ix < encsavefiles.Length; ix++)
                    //                {
                    //                    try
                    //                    {
                    //                        //set the data inside
                    //                        //sony was really cute left us the image in the meta path 
                    //                        //sce_icon0png1
                    //                        MainClass.SaveDataHolder holderitem = new MainClass.SaveDataHolder();
                    //                        var allpngfiles = Directory.GetFiles(savemetafiles[x], "*.png", SearchOption.TopDirectoryOnly)
                    //                            .ToList();
                    //                        if (allpngfiles.Count != 0)
                    //                        {
                    //                            holderitem.ImageLocation = allpngfiles[0].ToString();
                    //                        }
                    //                        string binfname = Path.GetFileNameWithoutExtension(encsavefiles[ix]);//since we know its bin
                    //                        holderitem.SaveDataFile = files[i] + "/sdimg_" + binfname;//is the bigger file correctly named sdimg_sce_
                    //                        holderitem.SealedKeyFile = encsavefiles[ix];//bin file location
                    //                        datitem.ListOfSaveItesm.Add(holderitem);
                    //                    }
                    //                    catch (Exception exfile)
                    //                    {
                    //                        Assets.Code.MessageBox.Show(exfile.Message);
                    //                        return;
                    //                    }
                    //                }
                    //            }
                    //        }
                    //        MainClass.savedatafileitems.Add(datitem);

                    //    }



                    //    main.CreateSaveDataView(MainClass.savedatafileitems, savecontentPanel,savescrollRect);
                    //    Assets.Code.LoadingDialog.Close();
                    //    return;
                    //    //this works like a bomb now

                    //}
                    //else
                    #endregion << Old No Longer Required >>
                    {

                        if(Application.platform == RuntimePlatform.PS4)
                        {
                            Assets.Code.LoadingDialog.Show("Loading save files\nplease wait ...");
                        }

                        //Clean the SaveData Loader
                        MainClass.savedatafileitems = new List<MainClass.SaveDataMain>();

                        var saves = Data.DataProcsessing.SaveData.GetAllSaves();
                        for (int i = 0; i < saves.Count; i++)
                        {


                            int UserId = 0;
                            int.TryParse(MainClass.Get_UserId(), out UserId);
                            string userDirectory = UserId.ToString("x");
                            MainClass.SaveDataMain datitem = new MainClass.SaveDataMain();


                            string SaveFileLocation = @"/user/home/" + userDirectory + "/savedata/";
                            string SaveFileMetaData = @"/user/home/" + userDirectory + "/savedata_meta/user/";

                            if (Application.platform != RuntimePlatform.PS4)
                            {
                                SaveFileLocation = @"D:\Users\3deEchelon\Desktop\PS4\RE\Ps4 Save Data Backup\10000000\savedata\";
                                SaveFileMetaData = @"D:\Users\3deEchelon\Desktop\PS4\RE\Ps4 Save Data Backup\10000000\savedata_meta\user\";
                            }

                            //does the savedataholder contain this save info already ?
                            var Found = MainClass.savedatafileitems.FirstOrDefault(x => x.SaveFilePath == SaveFileLocation + saves[i].title_id);
                            if(Found != null)
                            {
                                continue;
                            }

                            //per save we need to find the application info
                            var SaveInfo = Data.DataProcsessing.AppInfo.GetAppInfo(saves[i].title_id);
                            string DisplayName = "";

                            var Title = SaveInfo.Find(x => x.key == "TITLE");
                            if(Title != null)
                            {
                                DisplayName = Title.val + "(" + saves[i].title_id + ")";
                            }
                            else
                            {
                                DisplayName = "unknown" + "(" + saves[i].title_id + ")";
                            }
                            datitem.TITLE = DisplayName;
                            datitem.TitleId = saves[i].title_id;
                            datitem.SaveFilePath = SaveFileLocation + saves[i].title_id;
                            datitem.SaveMetaFilePath = SaveFileMetaData + saves[i].title_id;
                            var saves_with_id = saves.Where(x => x.title_id == saves[i].title_id).ToList();
                            for (int ix = 0; ix < saves_with_id.Count; ix++)
                            {
                                try
                                {
                                    MainClass.SaveDataHolder holderitem = new MainClass.SaveDataHolder();
                                    //var iconholder = Directory.GetFiles(datitem.SaveMetaFilePath, saves_with_id[ix].dir_name + "*.png");
                                    //now we will do some things to ensure this works
                                    holderitem.ImageLocation = datitem.SaveMetaFilePath + "/sce_icon0png1";//sony saves the image in this file

                                    if(!File.Exists(holderitem.ImageLocation))
                                    {
                                        //use the png file 
                                        holderitem.ImageLocation = datitem.SaveMetaFilePath +"/"+ saves_with_id[ix].dir_name + "_icon0.png";
                                    }

                                    string binfname = saves_with_id[ix].dir_name;//easy stuff now
                                    holderitem.SaveDataFile = datitem.SaveFilePath + "/sdimg_" + binfname;
                                    holderitem.SealedKeyFile = datitem.SaveFilePath + "/" + binfname + ".bin";
                                    //string binfname = Path.GetFileNameWithoutExtension(encsavefiles[ix]);//since we know its bin
                                    //holderitem.SaveDataFile = files[i] + "/sdimg_" + binfname;//is the bigger file correctly named sdimg_sce_
                                    //holderitem.SealedKeyFile = encsavefiles[ix];//bin file location
                                    datitem.ListOfSaveItesm.Add(holderitem);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            //datitem.ListOfSaveItesm
                            MainClass.savedatafileitems.Add(datitem);


                        }

                        //string[] files = System.IO.Directory.GetDirectories(@"D:\Users\3deEchelon\Desktop\PS4\RE\Ps4 Save Data Backup\10000000\savedata", "*", SearchOption.TopDirectoryOnly);
                        //string[] savemetafiles = System.IO.Directory.GetDirectories(@"D:\Users\3deEchelon\Desktop\PS4\RE\Ps4 Save Data Backup\10000000\savedata_meta\user", "*", SearchOption.TopDirectoryOnly);



                        //for (int i = 0; i < files.Length; i++)
                        //{
                        //    MainClass.SaveDataMain datitem = new MainClass.SaveDataMain();
                        //    datitem.SaveFilePath = files[i];
                        //    for (int x = 0; x < savemetafiles.Length; x++)
                        //    {

                        //        string DirSaveName = new DirectoryInfo(datitem.SaveFilePath).Name;
                        //        string DirSaveMetaName = new DirectoryInfo(savemetafiles[x]).Name;
                        //        if (DirSaveName == DirSaveMetaName)//Directories match
                        //        {
                        //            datitem.SaveMetaFilePath = DirSaveMetaName;
                        //            //get all saveenc files in the savedir
                        //            string[] encsavefiles = System.IO.Directory.GetFiles(files[i], "*.bin", SearchOption.TopDirectoryOnly);//get all encyrption files
                        //            for (int ix = 0; ix < encsavefiles.Length; ix++)
                        //            {
                        //                try
                        //                {
                        //                    //set the data inside
                        //                    //sony was really cute left us the image in the meta path 
                        //                    //sce_icon0png1
                        //                    MainClass.SaveDataHolder holderitem = new MainClass.SaveDataHolder();
                        //                    holderitem.ImageLocation = savemetafiles[x] + "/sce_icon0png1";
                        //                    string binfname = Path.GetFileNameWithoutExtension(encsavefiles[ix]);//since we know its bin
                        //                    holderitem.SaveDataFile = files[i] + "/sdimg_" + binfname;//is the bigger file correctly named sdimg_sce_
                        //                    holderitem.SealedKeyFile = encsavefiles[ix];//bin file location
                        //                    datitem.ListOfSaveItesm.Add(holderitem);
                        //                }
                        //                catch (Exception exfile)
                        //                {
                        //                    string errogolder = exfile.Message;
                        //                }
                        //            }
                        //        }
                        //    }
                        //    MainClass.savedatafileitems.Add(datitem);

                        //}



                        main.CreateSaveDataView(MainClass.savedatafileitems, savecontentPanel, savescrollRect);
                        if (Application.platform == RuntimePlatform.PS4)
                        {
                            Assets.Code.LoadingDialog.Close();
                        }
                        //test db reader
                        //GetSaveDirs
                        //var list = GetSaveDirs("CUSA00135",@"C:\Users\3deEchelon\Desktop\PS4\RE\savedata\10000000\savedata.db");

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    //SendMessageToPS4(ex.Message);
                }


                return;
            }
        }
    }
}
