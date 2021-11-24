
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor.PS4;
using System.IO;
using System;
using System.Net;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Threading;
using PS4_Tools;
using System.Text;
using Assets.Code.Data;

public class MainClass : MonoBehaviour
{


    #region << Universal PRX calls >>
    [DllImport("universal")]
    private static extern int FreeUnjail(int FWVersion);
    [DllImport("universal")]
    private static extern int GetUid();
    [DllImport("universal")]
    private static extern string GetIDPS();
    [DllImport("universal")]
    private static extern string GetPSID();
    [DllImport("universal")]
    private static extern int SendMessageToPS4(string Message);
    [DllImport("universal")]
    private static extern string GetString();
    [DllImport("universal")]
    private static extern string GetKernelVersion();
    [DllImport("universal")]
    private static extern string KernelGetOpenPsId();
    [DllImport("universal")]
    private static extern int UnlockTrophies(string NPComID);

    [DllImport("universal")]
    [return: MarshalAs(UnmanagedType.LPStr)]
    private static extern string GetUsername();
    [DllImport("universal")]
    private static extern string GetUserId();
    [DllImport("universal")]
    private static extern string GetListOfServices();
    [DllImport("universal")]
    private static extern int MountSaveData(string TITLEID, string fingerprint);
    [DllImport("universal")]
    private static extern int MountSaveData_Path(string TITLEID, string SaveFile, string fingerprint);
    [DllImport("universal")]
    private static extern int UnMountSaveData();
    [DllImport("universal")]
    private static extern int Change_Controller_Color(int r, int g, int b);
    [DllImport("universal")]
    private static extern string DevelopedBy();

    [DllImport("universal")]
    //will return the version as e.g.905 //this version can be spoofed
    private static extern int get_fw();

    [DllImport("universal")]
    //will return the version as e.g.905 //this version can be spoofed
    private static extern string firmware_version_kernel();

    [DllImport("universal")]
    //will return the version as e.g.905 //this version can't be spoofed
    private static extern string firmware_version_libc();


    [DllImport("universal")]
    //will return the version as e.g.905 //this version can't be spoofed
    private static extern UInt16 get_firmware();

    [DllImport("universal")]
    private static extern int MountandLoad();

    [DllImport("universal")]
    private static extern int MakeCusaAppReadWrite();

    [DllImport("universal")]
    private static extern int FreeMount();

    [DllImport("universal")]
    private static extern int FreeFTP();

    [DllImport("universal")]
    private static extern int Temperature();

    [DllImport("universal")]
    private static extern bool JailbreakMe();//new firmware agnostic mode

    [DllImport("universal")]
    private static extern int InstallPKG(string path, string name, string imgpath);
    [DllImport("universal")]
    private static extern int UnloadPKGModule();

    [DllImport("universal")]
    private static extern bool LaunchApp(string titleId);

    [DllImport("universal")]
    private static extern void SetDebuggerTrue();


    [DllImport("universal")]
    private static extern string GetSandboxPath();

    [DllImport("universal")]
    private static extern bool Unity_Plugin();

    [DllImport("universal")]
    private static extern int MountSaveData2(string TitleId, string SaveToMount);

    [DllImport("universal")]
    private static extern int GetInteger();


    [DllImport("universal")]
    private static extern bool DeleteSaveData(string SaveDirToDelete);

    [DllImport("universal")]
    private static extern bool DeleteSaveDataGame(string TitleId);

    [DllImport("universal")]
    private static extern bool DeletAllSavesForUser();

    /// <summary>
    /// Gets the Info of each module in the system with their respective infos
    /// </summary>
    /// <returns></returns>
    [DllImport("universal")]
    private static extern string GetCallableList();

    [DllImport("universal")]
    private static extern void PlaySoundControler(string Path);


    [DllImport("universal")]
    private static extern bool ShowSaveDataDialog();


    [DllImport("universal")]
    private static extern int MakeSymLink(string source, string destination);


    #endregion << Universal PRX >>


    //[DLLImport("libSceNpTrophy")]
    //private static extern int //call sony's own function against itself
    public AudioClip[] clips;
    private AudioSource audiosource;
    public bool randomPlay = false;
    private int currentClipIndex = 0;




    public enum GameScreen
    {
        MainScreen,
        PKGScreen,
        PKGInfoScreen,
        FileExplorer,
        SaveData,
        SaveDataSelected,
        RecoveryTools,
        TrophyScreen,
        TrophyInfoScreen,
        USBPKGMenu,
    }

    public GameScreen CurrentSCreen = GameScreen.MainScreen;

    #region << Canvas >>

    public Text txtFirm;

    public GameObject CreditPanel;

    public Canvas CanvasExplorer;
    public Canvas MainMenu;

    #endregion << Canvas >>

    #region << Save Data >>

    public Canvas CanvasSaveData;
    public Canvas SaveSelectionCanvas;

    public UnityEngine.UI.Image SaveIMage;
    public Text SaveTitle;
    public Text SaveSize;
    public Text SaveContentID;
    public Text SaveStatus;
    public GameObject SaveOptionsList;
    #endregion << Save Data >>


    #region << PKG Section >>

    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public static int CurrentItem = 0;

    public Canvas PKGCanvas;
    public Canvas PKGSelectionCanvas;
    public UnityEngine.UI.Image PKGIMage;
    public Text PKGTitle;
    public Text PKGSize;
    public Text PKGContentID;
    public Text PKGStatus;
    public GameObject PKGPanel;
    public GameObject PKGOptions;
    public GameObject PKGOptionsList;
    public Text PKGAditionalInfo;
    public UnityEngine.UI.Image BGImg;
    List<GameObject> PKGItemGameObjectList = new List<GameObject>();
    public static List<GameObject> SaveItemGameObjectList = new List<GameObject>();
    List<GameObject> TrpItemGameObjectList = new List<GameObject>();

    #endregion << PKG Section >>


    #region << Save Data >>

    public ScrollRect savescrollRect;
    public RectTransform savecontentPanel;

    public Dropdown ddSave;
    public Text txtSaveInfo;

    #endregion << Save Data >>

    #region << Trophy Items >>

    public Canvas TrophyCanvas;
    public Canvas TrophyInfoCanvas;

    public ScrollRect trophycrollRect;
    public RectTransform trophycontentPanel;


    public UnityEngine.UI.Image TrophyIMage;
    public Text TrophyTitle;
    public Text TrophySize;
    public Text TrophyContentID;
    public Text TrophyStatus;
    public Text TrophyOptionsList;

    #endregion << Trophy Items >>

    #region << UsbMenu Items >>

    public Canvas UsbCanvas;

    public ScrollRect usbcrollRect;
    public RectTransform usbcontentPanel;

    #endregion << UsbMenu Items >>

    #region << Recovery Tools >>

    public Canvas RecoveryTools;

    #endregion << Recovery Tools >>

    public Text txtError;

    public List<GameObject> ObjectsCreate = new List<GameObject>();
    List<PS4_Tools.PKG.SceneRelated.Unprotected_PKG> listofpkgs = new List<PS4_Tools.PKG.SceneRelated.Unprotected_PKG>();
    List<Assets.Code.Models.AppBrowse> systempkglist = new List<Assets.Code.Models.AppBrowse>();
    List<string> listofpkgsfiles = new List<string>();
    public static List<SaveDataMain> savedatafileitems = new List<SaveDataMain>();

    public List<string> TODOS = new List<string>();

    public GameObject FilePKGFab;
    public GameObject FileSaveFab;

    public int FirmHolder = 0;


    public static string Get_UserId()
    {
        if (Application.platform == RuntimePlatform.PS4)
        {
            return GetUserId();
        }
        else
        {
            return "";
        }
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "0.0.0.0";
        //throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    // Use this for initialization
    void Start()
    {

        //int holder = GetInteger();

        audiosource = FindObjectOfType<AudioSource>();
        audiosource.loop = false;
        if (!audiosource.isPlaying)
        {

            audiosource.clip = GetRandomClip();
            audiosource.Play();
        }

        if (Application.platform == RuntimePlatform.PS4)
        {
            /*Uncomment when test is sucsesfull */
            try
            {
                //launch unjail function
                //HENorNah();
                try
                {
                    //first we want to see what version the system is 
                    FirmHolder = get_firmware();
                    txtFirm.text = "Firmware :" + get_firmware().ToString() + " / " + Temperature().ToString() + " ºC";
                    //for some reason unity apps won't allow this to work geussing it has something to do with the way we do dll import
                    //try
                    //{
                    //    JailbreakMe();                
                    //}
                    //catch
                    {
                        //JailbreakMe();

                        FreeUnjail(get_firmware());
                    }

                    //now do freemount so we have full read write access 
                    FreeMount();


                    FreeFTP(); //DO FTP NOW THANKS BYE

                    //try and FIND FTP 


                }
                catch (Exception ex)
                {
                    txtError.text += ex.Message;
                }
            }
            catch (Exception ex)
            {
                txtError.text += "Could not escape sandbox :" + ex.Message;
            }
        }

        //try
        //{
        //    var IPAddress = GameObject.Find("txtIP");
        //    if (IPAddress != null)
        //    {
        //        IPAddress.gameObject.GetComponent<Text>().text = "IP :" + GetLocalIPAddress() + ":21";
        //    }
        //}
        //catch (Exception ex)
        //{

        //}
    }
    private AudioClip GetRandomClip()
    {
        int item = UnityEngine.Random.Range(0, clips.Length);
        currentClipIndex = item;
        return clips[item];
    }
    private AudioClip GetNextClip()
    {
        int item = ((currentClipIndex + 1) % clips.Length);
        currentClipIndex = item;
        return clips[item];
    }

    public static Sprite CreateSpriteFromBytes(byte[] bytearry)
    {
        Texture2D texture = new Texture2D(512, 512);
        texture.LoadImage(bytearry);

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5F, .5f));
    }

    static long GetDirectorySize(string p)
    {
        // 1.
        // Get array of all file names.
        string[] a = Directory.GetFiles(p, "*.*");

        // 2.
        // Calculate total bytes of all files in a loop.
        long b = 0;
        foreach (string name in a)
        {
            // 3.
            // Use FileInfo to get length of each file.
            FileInfo info = new FileInfo(name);
            b += info.Length;
        }
        // 4.
        // Return total size
        return b;
    }
    void CopyDir(string sourceFolder, string destFolder)
    {
        if (!Directory.Exists(destFolder))
            Directory.CreateDirectory(destFolder);

        // Get Files & Copy
        string[] files = Directory.GetFiles(sourceFolder);
        foreach (string file in files)
        {
            string name = Path.GetFileName(file);

            // ADD Unique File Name Check to Below!!!!
            string dest = Path.Combine(destFolder, name);
            File.Copy(file, dest);
        }

        // Get dirs recursively and copy files
        string[] folders = Directory.GetDirectories(sourceFolder);
        foreach (string folder in folders)
        {
            string name = Path.GetFileName(folder);
            string dest = Path.Combine(destFolder, name);
            CopyDir(folder, dest);
        }
    }


    public static Color hexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }

    public class SaveDataMain
    {
        public string TitleId { get; set; }
        public string TITLE { get; set; }
        public string SaveFilePath { get; set; }
        public string SaveMetaFilePath { get; set; }
        public List<SaveDataHolder> ListOfSaveItesm = new List<SaveDataHolder>();
        //public List<string> 
    }

    public class SaveDataHolder
    {
        public string SaveDataFile { get; set; }
        public string SealedKeyFile { get; set; }
        public string ImageLocation { get; set; }
    }





    public void CreateSaveDataView(List<SaveDataMain> SaveDirs = null, RectTransform savecontentPanel = null, ScrollRect savescrollRect = null)
    {
        SaveItemGameObjectList.Clear();
        GameObject objetc = null;

        int count = savecontentPanel.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject.Destroy(savecontentPanel.GetChild(i).gameObject);
        }
        for (int i = 0; i < SaveDirs.Count; i++)
        {
            objetc = Instantiate(FileSaveFab, savecontentPanel);
            Text textholder = objetc.GetComponentInChildren<Text>();

            textholder.text = SaveDirs[i].TITLE;

            UnityEngine.UI.Image[] HolderforPic = objetc.GetComponentsInChildren<UnityEngine.UI.Image>();
            if (SaveDirs[i].ListOfSaveItesm.Count != 0)
            {
                try
                {
                    byte[] data = File.ReadAllBytes(SaveDirs[i].ListOfSaveItesm[0].ImageLocation);
                    HolderforPic[1].sprite = CreateSpriteFromBytes(data);
                }
                catch
                {
                    //cant find the pic dont though a fit
                }
            }
            //HolderforPic.sprite =
            //objetc.transform.localScale = new Vector3 (1, 1, 1);
            objetc.transform.SetParent(savecontentPanel);
            //objetc.transform.GetChild (0).gameObject.SetActive (true);
            UnityEngine.UI.Image imgholder = objetc.GetComponent<UnityEngine.UI.Image>();
            imgholder.color = hexToColor("#FFFFFF00");

            SaveItemGameObjectList.Add(objetc);
        }
        UnityEngine.UI.Image imgholder1 = SaveItemGameObjectList[0].GetComponent<UnityEngine.UI.Image>();
        imgholder1.color = hexToColor("#9F9F9FFF");
        savescrollRect.verticalNormalizedPosition = 0;
        ScrollToTop(savescrollRect);
    }
    public static string getBetween(string strSource, string strStart, string strEnd)
    {
        int Start, End;
        if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        {
            Start = strSource.IndexOf(strStart, 0) + strStart.Length;
            End = strSource.IndexOf(strEnd, Start);
            return strSource.Substring(Start, End - Start);
        }
        else
        {
            return "";
        }
    }
    List<Trophy_File> trplist = new List<Trophy_File>();
    List<string> DirFiles = new List<string>();

    public class CustomTrophyHolder
    {
        public string MetaInfo { get; set; }

        public List<Assets.Code.Models.AppInfo> AppInfo = new List<Assets.Code.Models.AppInfo>();
    }

    List<CustomTrophyHolder> lstTrophyFiles = new List<CustomTrophyHolder>();



    void CreateTrophyView(List<string> TrpFiles = null, List<string> IniFiles = null)
    {
        TrpItemGameObjectList.Clear();
        GameObject objetc = null;

        int count = trophycontentPanel.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject.Destroy(trophycontentPanel.GetChild(i).gameObject);
        }
        DirFiles = TrpFiles;
        for (int i = 0; i < TrpFiles.Count; i++)
        {

            Trophy_File trp = new Trophy_File();
            trp.Load(File.ReadAllBytes(TrpFiles[i]));
            PS4_Tools.TROPHY.TROPCONF tconf;
            PS4_Tools.TROPHY.TROPTRNS tpsn;
            PS4_Tools.TROPHY.TROPUSR tusr;

            DateTime ps3Time = new DateTime(2008, 1, 1);
            DateTime randomEndTime = DateTime.Now;

            string NPCOMID = "";
            //string Diroftrpy = Path.GetDirectoryName(IniFiles[i]);
            if (File.Exists(IniFiles[i]))
            {
                string inifile = File.ReadAllText(IniFiles[i]);
                NPCOMID = getBetween(inifile, "NPCOMMID=", "TROPAPPVER=");
                if (NPCOMID == "")
                {
                    NPCOMID = getBetween(inifile, "TROPTITLEID=", "TROPAPPVER=");
                    if (NPCOMID == "")
                    {
                        continue;
                    }
                }
                NPCOMID = NPCOMID.TrimEnd();
            }

            objetc = Instantiate(FileSaveFab, trophycontentPanel);


            Text textholder = objetc.GetComponentInChildren<Text>();
            //now we implement unlock all !
            //okay lets begin

            //if (trp.trophyItemList[i].Name == "TROPCONF.ESFM")
            //{

            //    var itembytes = trp.ExtractFileToMemory(trp.trophyItemList[i].Name);
            //    var itemcontainer = PS4_Tools.Trophy_File.ESFM.LoadAndDecrypt(itembytes, NPCOMID);
            //    tconf = new PS4_Tools.TROPHY.TROPCONF(itemcontainer);
            //}
            for (int ix = 0; ix < trp.trophyItemList.Count; ix++)
            {
                if (trp.trophyItemList[ix].Name == "TROP.ESFM")
                {
                    var itembytes = trp.ExtractFileToMemory(trp.trophyItemList[ix].Name);
                    var itemcontainer = PS4_Tools.Trophy_File.ESFM.LoadAndDecrypt(itembytes, NPCOMID);
                    try
                    {
                        var tconf2 = new PS4_Tools.TROPHY.TROPCONF(itemcontainer);
                        textholder.text = tconf2.title_name.Replace("???", "").Replace("??", "");
                        trp.trophyconf = tconf2;
                    }
                    catch
                    {
                        try
                        {
                            //try it without encryption
                            itembytes = trp.ExtractFileToMemory(trp.trophyItemList[ix].Name);
                            var tconf2 = new PS4_Tools.TROPHY.TROPCONF(itembytes);
                            textholder.text = tconf2.title_name.Replace("???", "").Replace("??", "");
                            trp.trophyconf = tconf2;
                        }
                        catch
                        {
                            //we don't know what it is 
                            textholder.text = NPCOMID;
                        }
                    }

                    //break;
                }

                UnityEngine.UI.Image[] HolderforPic = objetc.GetComponentsInChildren<UnityEngine.UI.Image>();
                if (trp.trophyItemList[ix].Name == "ICON0.PNG")
                {
                    try
                    {
                        byte[] data = trp.ExtractFileToMemory(trp.trophyItemList[ix].Name);
                        HolderforPic[1].sprite = CreateSpriteFromBytes(data);
                    }
                    catch
                    {
                        //cant find the pic dont though a fit
                    }
                }
            }

            //HolderforPic.sprite =
            //objetc.transform.localScale = new Vector3 (1, 1, 1);
            objetc.transform.SetParent(trophycontentPanel);
            //objetc.transform.GetChild (0).gameObject.SetActive (true);
            UnityEngine.UI.Image imgholder = objetc.GetComponent<UnityEngine.UI.Image>();
            imgholder.color = Color.black;

            TrpItemGameObjectList.Add(objetc);
            trplist.Add(trp);
        }
        UnityEngine.UI.Image imgholder1 = TrpItemGameObjectList[0].GetComponent<UnityEngine.UI.Image>();
        imgholder1.color = hexToColor("#9F9F9FFF");
        savescrollRect.verticalNormalizedPosition = 0;
        ScrollToTop(savescrollRect);
    }


    void CreateTrophyView(List<CustomTrophyHolder> MetaFiles = null)
    {
        try
        {
            TrpItemGameObjectList.Clear();
            GameObject objetc = null;

            int count = trophycontentPanel.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                GameObject.Destroy(trophycontentPanel.GetChild(i).gameObject);
            }

            for (int i = 0; i < MetaFiles.Count; i++)
            {

                try
                {
                    //we just want apps that include nptitle.dat

                    string Name = new DirectoryInfo(MetaFiles[i].MetaInfo).Name;
                    if (Name == "XDPX20004" || Name == "XDPX20002")
                    {
                        //we don't count
                        continue;
                    }

                    //SendMessageToPS4(Name);
                    string MetaDataLocation = MetaFiles[i].MetaInfo;
                    //SendMessageToPS4("Getting App Info");
                    MetaFiles[i].AppInfo = DataProcsessing.AppInfo.GetAppInfo(Name);
                    //SendMessageToPS4("Got App Info");



                    if (Application.platform != RuntimePlatform.PS4)
                    {
                        MetaDataLocation = MetaFiles[i].MetaInfo + @"\nptitle.dat";

                    }
                    else
                    {
                        //if (MetaFiles[i].AppInfo == null)
                        //{
                        //    SendMessageToPS4("App Info is blank");
                        //}

                        //ps4 app info holds meta info location
                        MetaDataLocation = MetaFiles[i].MetaInfo + "/nptitle.dat";
                    }
                    ///system_data/priv/appmeta/
                    if (!File.Exists(MetaDataLocation))
                    {
                        //SendMessageToPS4("MetaDataLocation does not exist");
                        continue;
                    }
                    objetc = Instantiate(FileSaveFab, trophycontentPanel);


                    Text textholder = objetc.GetComponentInChildren<Text>();

                    //now get the cusa info 

                    var ListOfStr = DataProcsessing.AppInfo.GetAppInfo(Name);


                    var Title = ListOfStr.Find(x => x.key == "TITLE");
                    if (Title != null)
                    {
                        textholder.text = Title.val.TrimEnd();
                    }
                    else
                    {
                        textholder.text = Name;
                    }

                    UnityEngine.UI.Image[] HolderforPic = objetc.GetComponentsInChildren<UnityEngine.UI.Image>();
                    string IconPath = "";

                    var _MetaInfoForImage = MetaFiles[i].AppInfo.Find(x => x.key == "_metadata_path");
                    if (_MetaInfoForImage != null)
                    {
                        IconPath = _MetaInfoForImage.val.TrimEnd() + "/icon0.png";
                    }

                    if (Application.platform != RuntimePlatform.PS4)
                    {
                        IconPath = @"C:\Users\3de Echelon\Desktop\ps4\user\appmeta\" + Name + @"\icon0.png";
                    }
                    if (File.Exists(IconPath))
                    {
                        try
                        {

                            HolderforPic[1].sprite = LoadSprite(IconPath);
                        }
                        catch
                        {
                            //cant find the pic dont though a fit
                        }
                    }


                    //HolderforPic.sprite =
                    //objetc.transform.localScale = new Vector3 (1, 1, 1);
                    objetc.transform.SetParent(trophycontentPanel);
                    //objetc.transform.GetChild (0).gameObject.SetActive (true);
                    UnityEngine.UI.Image imgholder = objetc.GetComponent<UnityEngine.UI.Image>();
                    imgholder.color = Color.black;

                    TrpItemGameObjectList.Add(objetc);
                    lstTrophyFiles.Add(MetaFiles[i]);
                }
                catch (Exception ex)
                {
                    SendMessageToPS4(ex.Message);
                }
            }
            UnityEngine.UI.Image imgholder1 = TrpItemGameObjectList[0].GetComponent<UnityEngine.UI.Image>();
            imgholder1.color = hexToColor("#9F9F9FFF");
            savescrollRect.verticalNormalizedPosition = 0;
            ScrollToTop(savescrollRect);
        }
        catch (Exception ex)
        {
            Assets.Code.MessageBox.Show(ex.Message + ex.StackTrace);
        }
    }

    void CreateSaveDataView(List<string> SaveDirs = null)
    {
        SaveItemGameObjectList.Clear();
        GameObject objetc = null;

        int count = savecontentPanel.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject.Destroy(savecontentPanel.GetChild(i).gameObject);
        }
        for (int i = 0; i < SaveDirs.Count; i++)
        {
            objetc = Instantiate(FileSaveFab, savecontentPanel);
            Text textholder = objetc.GetComponentInChildren<Text>();

            textholder.text = new DirectoryInfo(SaveDirs[i]).Name;

            UnityEngine.UI.Image[] HolderforPic = objetc.GetComponentsInChildren<UnityEngine.UI.Image>();
            //HolderforPic[1].sprite = CreateSpriteFromBytes(items [i].Icon);

            //HolderforPic.sprite =
            //objetc.transform.localScale = new Vector3 (1, 1, 1);
            objetc.transform.SetParent(savecontentPanel);
            //objetc.transform.GetChild (0).gameObject.SetActive (true);
            UnityEngine.UI.Image imgholder = objetc.GetComponent<UnityEngine.UI.Image>();
            imgholder.color = Color.black;

            SaveItemGameObjectList.Add(objetc);
        }
        UnityEngine.UI.Image imgholder1 = SaveItemGameObjectList[0].GetComponent<UnityEngine.UI.Image>();
        imgholder1.color = hexToColor("#9F9F9FFF");
        savescrollRect.verticalNormalizedPosition = 0;
        ScrollToTop(savescrollRect);
    }
    public static void ScrollToTop(ScrollRect scrollRect)
    {
        scrollRect.normalizedPosition = new Vector2(0, 1);
    }

    void CreatePKGViewList(List<PS4_Tools.PKG.SceneRelated.Unprotected_PKG> items = null)
    {
        PKGItemGameObjectList.Clear();
        GameObject objetc = null;

        int count = contentPanel.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject.Destroy(contentPanel.GetChild(i).gameObject);
        }

        for (int i = 0; i < items.Count; i++)
        {
            objetc = Instantiate(FilePKGFab, contentPanel);
            Text textholder = objetc.GetComponentInChildren<Text>();
            textholder.text = items[i].PS4_Title;

            UnityEngine.UI.Image[] HolderforPic = objetc.GetComponentsInChildren<UnityEngine.UI.Image>();
            HolderforPic[1].sprite = CreateSpriteFromBytes(items[i].Icon);
            //objetc.transform.localScale = new Vector3 (1, 1, 1);
            objetc.transform.SetParent(contentPanel);
            //objetc.transform.GetChild (0).gameObject.SetActive (true);
            UnityEngine.UI.Image imgholder = objetc.GetComponent<UnityEngine.UI.Image>();
            imgholder.color = Color.black;

            PKGItemGameObjectList.Add(objetc);
        }
        UnityEngine.UI.Image imgholder1 = PKGItemGameObjectList[0].GetComponent<UnityEngine.UI.Image>();
        imgholder1.color = hexToColor("#9F9F9FFF");
        scrollRect.verticalNormalizedPosition = 1;
        //yield return null;
        //LoadAdditionalInfo();

    }

    void AddItemsToPKGViewList(PS4_Tools.PKG.SceneRelated.Unprotected_PKG items = null)
    {

        GameObject objetc = null;

        int count = contentPanel.transform.childCount;
        //for (int i = 0; i < count; i++)
        //{
        //    GameObject.Destroy(contentPanel.GetChild(i).gameObject);
        //}

        //for (int i = 0; i < items.Count; i++)
        {
            objetc = Instantiate(FilePKGFab, contentPanel);
            Text textholder = objetc.GetComponentInChildren<Text>();
            textholder.text = items.PS4_Title;

            UnityEngine.UI.Image[] HolderforPic = objetc.GetComponentsInChildren<UnityEngine.UI.Image>();
            HolderforPic[1].sprite = CreateSpriteFromBytes(items.Icon);
            //objetc.transform.localScale = new Vector3 (1, 1, 1);
            objetc.transform.SetParent(contentPanel);
            //objetc.transform.GetChild (0).gameObject.SetActive (true);
            UnityEngine.UI.Image imgholder = objetc.GetComponent<UnityEngine.UI.Image>();
            imgholder.color = Color.black;

            PKGItemGameObjectList.Add(objetc);
        }
        UnityEngine.UI.Image imgholder1 = PKGItemGameObjectList[0].GetComponent<UnityEngine.UI.Image>();
        imgholder1.color = hexToColor("#9F9F9FFF");
        scrollRect.verticalNormalizedPosition = 1;
        //yield return null;
        //StartCoroutine("LoadAdditionalInfo");
    }


    private Sprite LoadSprite(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }

    void AddItemsToPKGViewListSystem(Assets.Code.Models.AppBrowse items = null)
    {

        GameObject objetc = null;

        int count = contentPanel.transform.childCount;
        //for (int i = 0; i < count; i++)
        //{
        //    GameObject.Destroy(contentPanel.GetChild(i).gameObject);
        //}

        //for (int i = 0; i < items.Count; i++)
        {
            objetc = Instantiate(FilePKGFab, contentPanel);
            Text textholder = objetc.GetComponentInChildren<Text>();
            textholder.text = items.titleName;

            UnityEngine.UI.Image[] HolderforPic = objetc.GetComponentsInChildren<UnityEngine.UI.Image>();
            HolderforPic[1].sprite = LoadSprite(items.metaDataPath + "/icon0.png");
            //objetc.transform.localScale = new Vector3 (1, 1, 1);
            objetc.transform.SetParent(contentPanel);
            //objetc.transform.GetChild (0).gameObject.SetActive (true);
            UnityEngine.UI.Image imgholder = objetc.GetComponent<UnityEngine.UI.Image>();
            imgholder.color = Color.black;

            PKGItemGameObjectList.Add(objetc);
        }
        UnityEngine.UI.Image imgholder1 = PKGItemGameObjectList[0].GetComponent<UnityEngine.UI.Image>();
        imgholder1.color = hexToColor("#9F9F9FFF");
        scrollRect.verticalNormalizedPosition = 1;
        //yield return null;
        //StartCoroutine("LoadAdditionalInfo");
    }

    void CreatePKGViewListUsb(List<PS4_Tools.PKG.SceneRelated.Unprotected_PKG> items = null)
    {
        PKGItemGameObjectList.Clear();
        GameObject objetc = null;

        int count = usbcontentPanel.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject.Destroy(usbcontentPanel.GetChild(i).gameObject);
        }

        for (int i = 0; i < items.Count; i++)
        {
            objetc = Instantiate(FilePKGFab, usbcontentPanel);
            Text textholder = objetc.GetComponentInChildren<Text>();
            textholder.text = items[i].PS4_Title;

            UnityEngine.UI.Image[] HolderforPic = objetc.GetComponentsInChildren<UnityEngine.UI.Image>();
            HolderforPic[1].sprite = CreateSpriteFromBytes(items[i].Icon);
            //objetc.transform.localScale = new Vector3 (1, 1, 1);
            objetc.transform.SetParent(usbcontentPanel);
            //objetc.transform.GetChild (0).gameObject.SetActive (true);
            UnityEngine.UI.Image imgholder = objetc.GetComponent<UnityEngine.UI.Image>();
            imgholder.color = Color.black;

            PKGItemGameObjectList.Add(objetc);
        }
        UnityEngine.UI.Image imgholder1 = PKGItemGameObjectList[0].GetComponent<UnityEngine.UI.Image>();
        imgholder1.color = hexToColor("#9F9F9FFF");
        usbcrollRect.verticalNormalizedPosition = 1;
        //yield return null;
        //LoadAdditionalInfo();

    }

    string GetHumanReadableSize(long filesize)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = filesize;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }

        // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
        // show a single decimal place, and no space.
        string result = String.Format("{0:0.##} {1}", len, sizes[order]);
        return result;
    }

    bool SaveFilesMounted = false;
    private float[] ConvertByteToFloat(byte[] array)
    {
        float[] floatArr = new float[array.Length / 4];
        for (int i = 0; i < floatArr.Length; i++)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(array, i * 4, 4);
            floatArr[i] = BitConverter.ToSingle(array, i * 4) / 0x80000000;
        }
        return floatArr;
    }
    private float[] ConvertByteToFloat16(byte[] array)
    {
        float[] floatArr = new float[array.Length / 2];
        for (int i = 0; i < floatArr.Length; i++)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(array, i * 2, 2);
            }
            floatArr[i] = (float)(BitConverter.ToInt16(array, i * 2) / 32767f);
        }
        return floatArr;
    }

    /// <summary>
    /// Normalizes the values within this array.
    /// </summary>
    /// <param name="data">The array which holds the values to be normalized.</param>
    static float[] Normalize(float[] data)
    {
        float max = float.MinValue;

        // Find maximum
        for (int i = 0; i < data.Length; i++)
        {
            if (Math.Abs(data[i]) > max)
            {
                max = Math.Abs(data[i]);
            }
        }

        // Divide all by max
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = data[i] / max;
        }
        return data;
    }


    private static string[] FindFileDir(string beginpath)
    {
        List<string> findlist = new List<string>();

        /* I begin a recursion, following the order:
         * - Insert all the files in the current directory with the recursion
         * - Insert all subdirectories in the list and rebegin the recursion from there until the end
         */
        RecurseFind(beginpath, findlist);

        return findlist.ToArray();
    }

    private static void RecurseFind(string path, List<string> list)
    {
        if (path == "/data/" || path == "/mnt/sandbox/")
        {

        }
        else
        {
            string[] fl = Directory.GetFiles(path);
            string[] dl = Directory.GetDirectories(path);
            if (fl.Length > 0 || dl.Length > 0)
            {
                //I begin with the files, and store all of them in the list
                foreach (string s in fl)
                    list.Add(s);
                //I then add the directory and recurse that directory, the process will repeat until there are no more files and directories to recurse
                foreach (string s in dl)
                {
                    list.Add(s);
                    RecurseFind(s, list);
                }
            }
        }
    }
    public void LoadAdditionalInfo()
    {
        // PS4_Tools.PKG.SceneRelated.Unprotected_PKG pkginfo = new PKG.SceneRelated.Unprotected_PKG();

        var pkginfo = systempkglist[CurrentItem];
        // pkginfo;
        string locklevel = "";
        var t = DataProcsessing.AppInfo.GetAppInfo(pkginfo.titleId);
        for (int i = 0; i < t.Count; i++)
        {
            if (t[i].key == "PARENTAL_LEVEL")
            {
                locklevel = t[i].val;
                break;
            }
        }

        try
        {
            if (File.Exists(pkginfo.metaDataPath + "/pic1.png"))
            {
                BGImg.sprite = LoadSprite(pkginfo.metaDataPath + "/pic1.png");
            }
            else
            {
                BGImg.sprite = LoadSprite(pkginfo.metaDataPath + "/pic0.png");
            }
        }
        catch
        {

        }

        //var PFsKey = PS4_Tools.SaveData.GetSaveDataPFSKey();
        string NpTitleId = "";
        string NpBind = "";
        byte[] NpTitleIdSecret = new byte[64];
        try
        {

            PKG.SceneRelated.NP_Title npdataholder = new PKG.SceneRelated.NP_Title();
            npdataholder = new PS4_Tools.PKG.SceneRelated.NP_Title("/system_data/priv/appmeta/" + pkginfo.titleId + "/nptitle.dat");
            if (npdataholder != null)
            {
                NpTitleId = npdataholder.Nptitle;
                NpTitleIdSecret = npdataholder.NpTitleSecret;
            }
            PKG.SceneRelated.NP_Bind npbindholder = new PKG.SceneRelated.NP_Bind();
            npbindholder = new PS4_Tools.PKG.SceneRelated.NP_Bind("/system_data/priv/appmeta/" + pkginfo.titleId + "/npbind.dat");
            if (npbindholder != null)
            {
                NpBind = npbindholder.Nptitle;
            }
        }
        catch (Exception ex)
        {

        }
        //Licensing.Sealedkey sldkey = new  Licensing.Sealedkey();
        //yield return sldkey = Licensing.LoadSealedKey(Sealedkeylocation);
        //if(sldkey.KEY != null)
        //{

        //}

        //NpTitleIdSecret = npdataholder.NpTitleSecret;

        PKGAditionalInfo.text = string.Format("Additional Info\nLock Level : {1}\nNpTitle : {2}\nNpBind: {3}", "", locklevel, NpTitleId, NpBind);//Sealed Key : {0}
    }


    public static void LogInfo(string ToLog)
    {
        string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
        message += Environment.NewLine;
        message += "-----------------------------------------------------------";
        message += Environment.NewLine;
        message += string.Format("Message: {0}", ToLog);
        message += Environment.NewLine;
        message += "-----------------------------------------------------------";
        message += Environment.NewLine;
        try
        {
            string path = "/mnt/usb0/Log.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }
        catch
        {
            string path = "/mnt/usb1/Log.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }

    }


    bool playing_Snd0 = false;

    bool FTPAddress = false;

    public void ModifyTrophyData()
    {
        var trophyitem = lstTrophyFiles[CurrentItem];

        string MetaDataLocation = "";
        string NpBindLocation = "";

        var _MetaInfo = trophyitem.AppInfo.Find(x => x.key == "TITLE_ID");
        if (_MetaInfo != null)
        {
            MetaDataLocation = "/system_data/priv/appmeta/" + _MetaInfo.val.TrimEnd() + @"/nptitle.dat";
        }
        if (Application.platform != RuntimePlatform.PS4)
        {
            MetaDataLocation = @"C:\Users\3de Echelon\Desktop\ps4\system_data\priv\appmeta\" + _MetaInfo.val.TrimEnd() + @"\nptitle.dat";
        }
        if (_MetaInfo != null)
        {
            NpBindLocation = "/system_data/priv/appmeta/" + _MetaInfo.val.TrimEnd() + @"/npbind.dat";
        }
        if (Application.platform != RuntimePlatform.PS4)
        {
            NpBindLocation = @"C:\Users\3de Echelon\Desktop\ps4\system_data\priv\appmeta\" + _MetaInfo.val.TrimEnd() + @"\npbind.dat";
        }

        string AppLocation = "/system_data/priv/appmeta/XDPX20004/nptitle.dat";
        if (Application.platform != RuntimePlatform.PS4)
        {
            AppLocation = @"C:\Users\3de Echelon\Desktop\ps4\system_data\priv\appmeta\XDPX20004\nptitle.dat";
        }
        //Quickly read the trophy Np id 
        PKG.SceneRelated.NP_Title npdataholder = new PKG.SceneRelated.NP_Title();
        npdataholder = new PS4_Tools.PKG.SceneRelated.NP_Title(MetaDataLocation);
        var TrophyInfo = GameObject.Find("TrophyInfoBottom");
        PKG.SceneRelated.NP_Bind npBind = new PKG.SceneRelated.NP_Bind();
        if (File.Exists(NpBindLocation))
        {
            npBind = new PS4_Tools.PKG.SceneRelated.NP_Bind(NpBindLocation);
        }

        //FIRST COPY INFO
        //fixing trophy info > just replace it in the console directory
        File.Copy(NpBindLocation, "/system_data/priv/appmeta/XDPX20004/npbind.dat", true);//replace this item


        byte[] nptitle = File.ReadAllBytes(MetaDataLocation);
        byte[] orginaltit = Encoding.UTF8.GetBytes(_MetaInfo.val.TrimEnd() + "_00");
        byte[] newtit = Encoding.UTF8.GetBytes("XDPX20004_00");
        nptitle = Assets.Code.StringExtensions.ReplaceBytes(nptitle,
        orginaltit,
        newtit);

        File.WriteAllBytes(AppLocation, nptitle);//we now have the right item
    }



    // Update is called once per frame
    void Update()
    {
        try
        {
            //We want to update the Temprature here cause its cool
            if (Application.platform == RuntimePlatform.PS4)
            {
                txtFirm.text = "Firmware :" + FirmHolder.ToString() + " / " + Temperature().ToString() + " ºC";
                if (FTPAddress == false)
                {
                    try
                    {
                        var IPAddress = GameObject.Find("txtIP");
                        if (IPAddress != null)
                        {
                            IPAddress.gameObject.GetComponent<Text>().text = "IP :" + GetLocalIPAddress() + ":21";
                        }
                        FTPAddress = true;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            if (playing_Snd0 == false)
            {
                if (!audiosource.isPlaying)
                {
                    AudioClip nextClip;
                    if (randomPlay)
                    {
                        nextClip = GetRandomClip();
                    }
                    else
                    {
                        nextClip = GetNextClip();
                    }

                    audiosource.clip = nextClip;
                    audiosource.Play();
                }
            }
            //should be cross
            if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                if (CurrentSCreen == GameScreen.RecoveryTools)
                {
                    int UserId = 0;
                    int.TryParse(GetUserId(), out UserId);
                    string userDirectory = UserId.ToString("x");

                    //Spesific Trophy Unlocking
                    //PS4_Tools.Trophy_File.Unlock_All_Title_Id("NPWR09395_00", "/user/home/" + userDirectory + "/trophy/db/trophy_local.db", "/user/home/"+ userDirectory + "/trophy/data/sce_trop/trpsummary.dat");
                    //Full Trophy Unlcoking 
                    PS4_Tools.Recovery.FixTrophySummary("/user/home/" + userDirectory + "/trophy/db/trophy_local.db", "/user/home/" + userDirectory + "/trophy/data/sce_trop/trpsummary.dat");
                    Assets.Code.MessageBox.Show("Trophy summary fixed!");

                    return;
                }
                if (CurrentSCreen == GameScreen.MainScreen)
                {
                    try
                    {
                        if (Application.platform == RuntimePlatform.PS4)
                        {
                            Assets.Code.LoadingDialog.Show("Loading all pkg's this might take some time");
                        }
                        {
                            try
                            {
                                CurrentItem = 0;
                                usbcontentPanel.gameObject.SetActive(false);
                                MainMenu.gameObject.SetActive(false);//hide main menu
                                PKGSelectionCanvas.gameObject.SetActive(true);//SHow PKG Screen
                                CurrentSCreen = GameScreen.PKGScreen;
                                PS4_Tools.PKG.SceneRelated.Unprotected_PKG pkginfo = new PKG.SceneRelated.Unprotected_PKG();
                                if (Application.platform != RuntimePlatform.PS4)
                                {

                                    //string[] files = System.IO.Directory.GetFiles(@"C:\Users\3deEchelon\Desktop\PS4\RE\Ps4 Save Data Backup", "*.pkg",SearchOption.AllDirectories);
                                    //string[] files = System.IO.Directory.GetFiles(@"F:\Games\Playstation\PS4", "*.pkg", SearchOption.AllDirectories);

                                    //Now we process using SQLIte


                                    var items = DataProcsessing.AppInfo.GetAllApps("268435456");
                                    StartCoroutine(ProcessPKGs(items));


                                    //StartCoroutine(ProcessPKGs(files));

                                    //pkginfo = PS4_Tools.PKG.SceneRelated.Read_PKG (@"C:\Users\3deEchelon\Desktop\PS4\RE\Ps4 Save Data Backup\CUSA00265\app.pkg");

                                }
                                else
                                {
                                    //try and read pkg from here
                                    //how about we just include ps4debug and do all our fancy stuff here 
                                    //pkginfo = PS4_Tools.PKG.SceneRelated.Read_PKG (@"/user/app/CUSA00135/app.pkg");
                                    //string[] files = System.IO.Directory.GetFiles(@"/user/app/", "*.pkg", SearchOption.AllDirectories);
                                    //StartCoroutine(ProcessPKGs(files));
                                    //this has now changed how we do things
                                    var items = DataProcsessing.AppInfo.GetAllApps(GetUserId());
                                    StartCoroutine(ProcessPKGs(items));

                                }
                            }
                            catch (Exception ex)
                            {
                                Assets.Code.LoadingDialog.Close();
                                Assets.Code.MessageBox.Show("Error Loading PKG Files" + ex.Message + "\n" + ex.Data + "\n" + ex.StackTrace);
                                //txtError.text = "Error process could not start " + ex.Message + "\n" + ex.Data + "\n" + ex.StackTrace;
                            }
                        }
                    }
                    catch
                    { // (System.Exception ex)
                        ; //LOG = "Error " + ex.Message;
                    }
                    return;
                }
                else if (CurrentSCreen == GameScreen.PKGScreen)
                {
                    //go to the pkg selector screen
                    PKGSelectionCanvas.gameObject.SetActive(false);//hide PKG Screen
                    PKGCanvas.gameObject.SetActive(true);//show pkg info
                    CanvasSaveData.gameObject.SetActive(false);//Hide Save Screen
                    CurrentSCreen = GameScreen.PKGInfoScreen;

                    var pkgholder = systempkglist[CurrentItem];
                    try
                    {
                        var pkginfo = PS4_Tools.PKG.SceneRelated.Read_PKG("/user/app/" + pkgholder.titleId + "/app.pkg");
                        PKGContentID.text = pkginfo.Content_ID;


                        PKGIMage.sprite = CreateSpriteFromBytes(pkginfo.Icon);
                        PKGTitle.text = pkginfo.PS4_Title;

                        PKGSize.text = pkginfo.Size;
                        PKGStatus.text = pkginfo.PKGState.ToString();

                        PKGPanel.GetComponent<UnityEngine.UI.Image>().color = Color.white;
                        PKGPanel.GetComponent<UnityEngine.UI.Image>().sprite = CreateSpriteFromBytes(pkginfo.Image);
                        // Declaration
                        Shader transparent;

                        // Finding shader in Awake()
                        transparent = Shader.Find("Custom/BLUR");
                        //PKGPanel.GetComponent<UnityEngine.UI.Image>().material.shader = transparent;

                        byte[] bytes = null;
                        //only play Audio if the file exists
                        if (Application.platform == RuntimePlatform.PS4)
                        {
                            //onlt if the user wants this so we should add settings menus
                            return;
                            if (File.Exists(@"/user/appmeta/" + pkginfo.Param.TITLEID + "/snd0.at9"))
                            {
                                //Thread thread = new Thread(delegate ()
                                //{
                                playing_Snd0 = true;
                                //SendMessageToPS4("Loading snd0.at9");
                                //bytes = PS4_Tools.Media.Atrac9.LoadAt9(@"/user/appmeta/" + pkginfo.Param.TITLEID + "/snd0.at9");
                                //AudioClip audioClip = Assets.Code.WavUtility.ToAudioClip(bytes);

                                // WAV wav = new WAV(bytes);
                                //// Debug.Log(wav);
                                // AudioClip audioClip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false, false);
                                // audioClip.SetData(wav.LeftChannel, 0);
                                // //audio.clip = audioClip;
                                // //audio.Play();

                                //audiosource = FindObjectOfType<AudioSource>();
                                //audiosource.loop = false;
                                //if (audiosource.isPlaying)
                                //{
                                //    audiosource.Stop();
                                //    //  AudioSource.PlayClipAtPoint(audioClip, new Vector3(100, 100, 0), 1.0f);
                                //    audiosource.clip = (audioClip);
                                //    audiosource.Play();
                                //}
                                //else
                                //{
                                //    audiosource.clip = (audioClip);
                                //    audiosource.Play();
                                //}
                                //});
                                //thread.Start();
                            }

                        }
                        else
                        {
                            if (File.Exists(@"C:\Users\3de Echelon\Downloads\OrbisTitleMetadataDatabase-master\OrbisTitleMetadataDatabase-master\OrbisTitleMetadataDatabase\bin\Debug\EP9000-CUSA00002_00-KZ4RELEASE000041\snd0.at9"))
                            {
                                //Thread thread = new Thread(delegate ()
                                //{

                                bytes = PS4_Tools.Media.Atrac9.LoadAt9(@"C:\Users\3de Echelon\Downloads\OrbisTitleMetadataDatabase-master\OrbisTitleMetadataDatabase-master\OrbisTitleMetadataDatabase\bin\Debug\EP9000-CUSA00002_00-KZ4RELEASE000041\snd0.at9");
                                AudioClip audioClip = Assets.Code.WavUtility.ToAudioClip(bytes);

                                // WAV wav = new WAV(bytes);
                                //// Debug.Log(wav);
                                // AudioClip audioClip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false, false);
                                // audioClip.SetData(wav.LeftChannel, 0);
                                // //audio.clip = audioClip;
                                // //audio.Play();

                                audiosource = FindObjectOfType<AudioSource>();
                                audiosource.loop = false;
                                if (audiosource.isPlaying)
                                {
                                    audiosource.Stop();
                                    //  AudioSource.PlayClipAtPoint(audioClip, new Vector3(100, 100, 0), 1.0f);
                                    audiosource.clip = (audioClip);
                                    audiosource.Play();
                                }
                                else
                                {
                                    audiosource.clip = (audioClip);
                                    audiosource.Play();
                                }
                                //});
                                //thread.Start();
                            }
                        }

                    }
                    catch
                    {
                        PKGContentID.text = pkgholder.contentId;


                        PKGIMage.sprite = LoadSprite(pkgholder.metaDataPath + "icon0.png");



                        PKGTitle.text = pkgholder.titleName;

                        PKGSize.text = GetHumanReadableSize(pkgholder.contentSize);
                        PKGStatus.text = "unknown";

                        PKGPanel.GetComponent<UnityEngine.UI.Image>().color = Color.white;
                        if (File.Exists(pkgholder.metaDataPath + "/pic1.png"))
                        {
                            PKGPanel.GetComponent<UnityEngine.UI.Image>().sprite = LoadSprite(pkgholder.metaDataPath + "/pic1.png");
                        }
                        else
                        {
                            PKGPanel.GetComponent<UnityEngine.UI.Image>().sprite = LoadSprite(pkgholder.metaDataPath + "/pic0.png");
                        }
                        // Declaration
                        Shader transparent;

                        // Finding shader in Awake()
                        transparent = Shader.Find("Custom/BLUR");
                    }
                    return;
                }
                else if (CurrentSCreen == GameScreen.USBPKGMenu)
                {
                    var pkginfo = listofpkgs[CurrentItem];
                    if (Application.platform == RuntimePlatform.PS4)
                    {
                        var fileonusb = listofpkgsfiles[CurrentItem];
                        FileInfo fi = new FileInfo(fileonusb);
                        //dump the image from the SFO to the /data folder and ask the system to read it from there.
                        //a bit of customization for the pkg reading i think its quite cool
                        try
                        {
                            if (File.Exists("/data/" + pkginfo.Content_ID + ".png"))
                            {
                                File.Delete("/data/" + pkginfo.Content_ID + ".png");
                            }
                            if (pkginfo.Icon != null)
                            {
                                File.WriteAllBytes("/data/" + pkginfo.Content_ID + ".png", pkginfo.Icon);
                            }
                        }
                        catch (Exception ex)
                        {
                            SendMessageToPS4(ex.Message);
                        }
                        string imagepath = "";
                        Thread.Sleep(100);
                        if (File.Exists("/data/" + pkginfo.Content_ID + ".png"))
                        {
                            imagepath = "/data/" + pkginfo.Content_ID + ".png";
                        }
                        //SendMessageToPS4(imagepath);
                        int ps4install = InstallPKG(fileonusb, pkginfo.PS4_Title, imagepath);
                        if (ps4install != 0)
                        {
                            txtError.text = "Error installing the game";
                        }
                    }
                    return;
                }
                else if (CurrentSCreen == GameScreen.SaveData)
                {
                    //go to the save selector screen
                    CanvasSaveData.gameObject.SetActive(false);//hide PKG Screen
                    SaveSelectionCanvas.gameObject.SetActive(true);//show pkg info
                    PKGSelectionCanvas.gameObject.SetActive(false);//Hide Save Screen
                    CurrentSCreen = GameScreen.SaveDataSelected;
                    var savedata = savedatafileitems[CurrentItem];
                    SaveContentID.text = savedata.TITLE;

                    if (savedata.ListOfSaveItesm.Count != 0)
                    {
                        try
                        {
                            SaveIMage.sprite = CreateSpriteFromBytes(File.ReadAllBytes(savedata.ListOfSaveItesm[0].ImageLocation));
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    SaveTitle.text = "Total Saves : " + savedata.ListOfSaveItesm.Count;
                    try
                    {
                        SaveSize.text = GetHumanReadableSize(DataProcsessing.SaveData.GetAppSize(savedata.TitleId));
                    }
                    catch
                    {
                        SaveSize.text = "Unknown size";
                    }
                    SaveStatus.text = "Sealed key : comming soon";
                    ddSave.options.Clear();
                    List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
                    Dropdown.OptionData data = new Dropdown.OptionData();
                    data.text = "All";
                    options.Add(data);

                    for (int i = 0; i < savedata.ListOfSaveItesm.Count; i++)
                    {
                        data = new Dropdown.OptionData();
                        data.text = Path.GetFileName(savedata.ListOfSaveItesm[i].SaveDataFile) + "-" + new FileInfo(savedata.ListOfSaveItesm[i].SaveDataFile).LastWriteTimeUtc;
                        try
                        {
                            data.image = CreateSpriteFromBytes(File.ReadAllBytes(savedata.ListOfSaveItesm[i].ImageLocation));
                        }
                        catch
                        {

                        }
                        options.Add(data);
                    }

                    ddSave.AddOptions(options);
                    ddSave.onValueChanged.AddListener(delegate
                    {
                        DropdownValueChanged(ddSave);
                    });
                    txtSaveInfo.text = "All items selected";
                    //PKGPanel.GetComponent<UnityEngine.UI.Image> ().sprite = CreateSpriteFromBytes (pkginfo.Image);
                    return;

                }
                else if (CurrentSCreen == GameScreen.TrophyScreen)
                {
                    try
                    {
                        CanvasSaveData.gameObject.SetActive(false);//hide PKG Screen
                        SaveSelectionCanvas.gameObject.SetActive(false);//show pkg info
                        PKGSelectionCanvas.gameObject.SetActive(false);//Hide Save Screen
                        TrophyCanvas.gameObject.SetActive(false);
                        TrophyInfoCanvas.gameObject.SetActive(true);
                        CurrentSCreen = GameScreen.TrophyInfoScreen;


                        if (Application.platform == RuntimePlatform.PS4)
                        {
                            Assets.Code.LoadingDialog.Show("Prepping Trophy For Mount and Modification");
                        }

                        var trophyitem = lstTrophyFiles[CurrentItem];

                        string MetaDataLocation = "";
                        string NpBindLocation = "";

                        if (Application.platform == RuntimePlatform.PS4)
                        {
                            Assets.Code.LoadingDialog.Close();
                            Assets.Code.LoadingDialog.Show("Getting Trophy TITLE_ID");
                        }
                        var _MetaInfo = trophyitem.AppInfo.Find(x => x.key == "TITLE_ID");
                        if (_MetaInfo != null)
                        {
                            MetaDataLocation = "/system_data/priv/appmeta/" + _MetaInfo.val.TrimEnd() + @"/nptitle.dat";
                        }
                        if (Application.platform == RuntimePlatform.PS4)
                        {
                            Assets.Code.LoadingDialog.Close();
                            Assets.Code.LoadingDialog.Show("Mapping NP Info");
                        }

                        if (Application.platform != RuntimePlatform.PS4)
                        {
                            MetaDataLocation = @"C:\Users\3de Echelon\Desktop\ps4\system_data\priv\appmeta\" + _MetaInfo.val.TrimEnd() + @"\nptitle.dat";
                        }
                        if (_MetaInfo != null)
                        {
                            NpBindLocation = "/system_data/priv/appmeta/" + _MetaInfo.val.TrimEnd() + @"/npbind.dat";
                        }
                        if (Application.platform != RuntimePlatform.PS4)
                        {
                            NpBindLocation = @"C:\Users\3de Echelon\Desktop\ps4\system_data\priv\appmeta\" + _MetaInfo.val.TrimEnd() + @"\npbind.dat";
                        }
                        if (Application.platform == RuntimePlatform.PS4)
                        {
                            Assets.Code.LoadingDialog.Close();
                            Assets.Code.LoadingDialog.Show("Reading NP Info");
                        }
                        //Quickly read the trophy Np id 
                        PKG.SceneRelated.NP_Title npdataholder = new PKG.SceneRelated.NP_Title();
                        npdataholder = new PS4_Tools.PKG.SceneRelated.NP_Title(MetaDataLocation);
                        var TrophyInfo = GameObject.Find("TrophyInfoBottom");
                        PKG.SceneRelated.NP_Bind npBind = new PKG.SceneRelated.NP_Bind();
                        if (File.Exists(NpBindLocation))
                        {
                            npBind = new PS4_Tools.PKG.SceneRelated.NP_Bind(NpBindLocation);
                        }
                        else
                        {
                            //error out here
                            Assets.Code.MessageBox.Show("npbind could not be found you wont be able to use this option");

                        }
                        //if (npdataholder != null)
                        //{

                        //    //TrophyInfo.gameObject.GetComponent<Text>().text = "NpTitle:" + npdataholder.Nptitle + "\nNpTitleSecret:" + System.Text.Encoding.ASCII.GetString(npdataholder.NpTitleSecret);
                        //    //TrophyOptionsList.text = "NpTitle:" + npdataholder.Nptitle + "\nNpTitleSecret:" + System.Text.Encoding.ASCII.GetString(npdataholder.NpTitleSecret);
                        //}
                        //else
                        //{
                        //    TrophyInfo.gameObject.GetComponent<Text>().text = "Could not load info";
                        //}
                        if (Application.platform == RuntimePlatform.PS4)
                        {
                            Assets.Code.LoadingDialog.Close();
                            Assets.Code.LoadingDialog.Show("Setting Meta Data...");
                        }
                        string IconPath = "";
                        var imageinfo = trophyitem.AppInfo.Find(x => x.key == "_metadata_path");
                        if (imageinfo != null)
                        {
                            IconPath = imageinfo.val.TrimEnd() + "/icon0.png";
                        }

                        if (Application.platform != RuntimePlatform.PS4)
                        {
                            IconPath = @"C:\Users\3de Echelon\Desktop\ps4\user\appmeta\" + _MetaInfo.val.TrimEnd() + @"\icon0.png";
                        }

                        if (File.Exists(IconPath))
                        {
                            //set the icon 

                            var TrophyImage = GameObject.Find("TrophyImage");
                            TrophyImage.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = LoadSprite(IconPath);

                        }

                        var ddTrophyHolder = GameObject.Find("ddTrophy");

                        Dropdown ddTrophy = ddTrophyHolder.gameObject.GetComponent<Dropdown>();

                        ddTrophy.options.Clear();
                        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
                        Dropdown.OptionData data = new Dropdown.OptionData();
                        data.text = "All";
                        options.Add(data);

                        //we need to load all trophy info per trophy_title_id
                        string NpTitle = npBind.Nptitle;
                        if (Application.platform == RuntimePlatform.PS4)
                        {
                            Assets.Code.LoadingDialog.Close();
                            Assets.Code.LoadingDialog.Show("Getting Trophy Info...");
                        }
                        var lstoftrp1 = DataProcsessing.Trophy.GetAllTrophyFlags(NpTitle.Trim());

                        var lstoftrp = lstoftrp1.FindAll(x => x.trophy_title_id == NpTitle);

                        for (int i = 0; i < lstoftrp.Count; i++)
                        {
                            string Grade = "";
                            switch (lstoftrp[i].grade)
                            {
                                case "1":
                                    Grade = "Platinum";
                                    break;
                                case "2":
                                    Grade = "Gold";
                                    break;
                                case "3":
                                    Grade = "Silver";
                                    break;
                                case "4":
                                    Grade = "Bronze";
                                    break;
                                default:
                                    Grade = "Unknown";
                                    break;

                            }

                            data = new Dropdown.OptionData();
                            data.text = "(" + lstoftrp[i].trophyid + ") " + lstoftrp[i].title + " | Grade : " + Grade;
                            try
                            {
                                data.image = LoadSprite(IconPath);
                            }
                            catch
                            {

                            }
                            options.Add(data);
                        }




                        ddTrophy.AddOptions(options);
                        ddTrophy.onValueChanged.AddListener(delegate
                        {
                            DropdownValueChanged(ddTrophy);
                        });

                        Text TrophyInfoBottom = TrophyInfo.gameObject.GetComponent<Text>();
                        TrophyInfoBottom.text = "All Selected";

                        ddTrophy.value = 0;//got to the first item thanks 


                        var TrophySealedKeyHolder = GameObject.Find("TrophySealedKey");
                        if (TrophySealedKeyHolder != null)
                        {
                            Text TrophySealedKey = TrophySealedKeyHolder.gameObject.GetComponent<Text>();
                            TrophySealedKey.text = "Secret : " + BitConverter.ToString(npdataholder.NpTitleSecret);
                        }


                        var TrophyTitleHolder = GameObject.Find("TrophyTitle");
                        if (TrophyTitleHolder != null)
                        {
                            Text TrophyTitle = TrophyTitleHolder.gameObject.GetComponent<Text>();
                            TrophyTitle.text = "Total Trophies :" + lstoftrp.Count;
                        }

                        var TrophySizeHolder = GameObject.Find("TrophySize");
                        if (TrophySizeHolder != null)
                        {
                            Text TrophySize = TrophySizeHolder.gameObject.GetComponent<Text>();
                            TrophySize.text = "Size : Unknown";
                        }

                        var TrophyContentIDHolder = GameObject.Find("TrophyContentID");
                        if (TrophyContentIDHolder != null)
                        {
                            Text TrophyContentID = TrophyContentIDHolder.gameObject.GetComponent<Text>();
                            TrophyContentID.text = "Title ID : " + _MetaInfo.val.TrimEnd() + "\tTrophy Title ID:" + NpTitle;
                        }
                        if (Application.platform == RuntimePlatform.PS4)
                        {
                            Assets.Code.LoadingDialog.Close();
                            Assets.Code.LoadingDialog.Show("Mount and Modifing Trophy Info...");
                        }
                        ModifyTrophyData();
                        if (Application.platform == RuntimePlatform.PS4)
                        {
                            try
                            {
                                //we need to create the context and register it here
                                Assets.Code.Wrapper.TrophyUtil.CreateAndRegister();

                                Assets.Code.LoadingDialog.Close();
                            }
                            catch (Exception ex)
                            {
                                Assets.Code.MessageBox.Show(ex.Message);
                            }
                        }

                        ////check the dir info as well
                        //string path = DirFiles[CurrentItem];
                        //string DirInfo = Path.GetDirectoryName(path);
                        //if (trplist[CurrentItem].trophyconf != null)
                        //{

                        //    int UserId = 0;

                        //    if (Application.platform == RuntimePlatform.PS4)
                        //    {
                        //        int.TryParse(GetUserId(), out UserId);
                        //    }

                        //    string userDirectory = UserId.ToString("x");
                        //    string nptitleid = trplist[CurrentItem].trophyconf.npcommid;
                        //    if (!nptitleid.Contains("_00"))
                        //    {
                        //        nptitleid = nptitleid + "_00";
                        //    }

                        //    //Copy nptitle.dat to app
                        //    //well thanks sony for being special

                        //    string PathOfNpTitleId = "/system_data/priv/appmeta/" + trplist[CurrentItem].trophyconf.npcommid + "/nptitle.dat";
                        //    string PathOfNpBind = "";
                        //    if (File.Exists(""))
                        //    {



                        //        //int MakeSymLink
                        //    }

                        //PS4_Tools.Trophy_File.Unlock_All_Title_Id(nptitleid, "/user/home/" + userDirectory + "/trophy/db/trophy_local.db", "/user/home/" + userDirectory + "/trophy/data/sce_trop/trpsummary.dat");
                        //SendMessageToPS4("Unlocked all for " + nptitleid + " via DB");

                    }
                    catch (Exception ex)
                    {
                        Assets.Code.MessageBox.Show("Error on Trophy\n" + ex.Message);

                        //SendMessageToPS4("Could not unlock trophy(s)" + ex.Message);
                    }
                    return;
                }

                else if (CurrentSCreen == GameScreen.PKGInfoScreen)
                {
                    //public static Utility.AppLauncher LaunchApp(string titleId, string arg = "", EventMonitor.AppAttr appAttr = EventMonitor.AppAttr.None)
                    //{
                    //    return Utility.AppLauncher.LaunchApp(titleId, arg, appAttr);
                    //}

                    //lets try the mount stuff agian
                    if (Application.platform == RuntimePlatform.PS4)
                    {
                        //SendMessageToPS4("Trying to run this app wish me lunck");
                    }
                    var pkginfo = systempkglist[CurrentItem];
                    try
                    {
                        LaunchApp(pkginfo.titleId);
                        //Assets.Code.SCE.LncUtil.LaunchAppParam LaunchAppParam = default(Assets.Code.SCE.LncUtil.LaunchAppParam);
                        //Assets.Code.SCE.LncUtil.LaunchAppParamInit(ref LaunchAppParam);

                        //LaunchAppParam.userId = Convert.ToInt32(MainClass.Get_UserId());
                        //// LaunchAppParam.checkFlag = Assets.Code.SCE.LncUtil.Flag.SkipLaunchCheck;
                        //LaunchAppParam.appAttr = (int)Assets.Code.SCE.EventMonitor.AppAttr.LaunchByDebugger;

                        //SendMessageToPS4("Trying to run this app wish me lunck");
                        //Assets.Code.SCE.LncUtil.LaunchApp("CUSA01084", new string[] { "" }, ref LaunchAppParam);
                    }
                    catch (Exception ex)
                    {
                        SendMessageToPS4(ex.Message);
                    }
                    return;
                }
                else if (CurrentSCreen == GameScreen.TrophyInfoScreen)
                {
                    //try and unlock spesific trophy
                    if (Application.platform == RuntimePlatform.PS4)
                    {
                        //SendMessageToPS4("Trying to run this app wish me lunck");
                    }


                    var ddTrophyHolder = GameObject.Find("ddTrophy");

                    Dropdown ddTrophy = ddTrophyHolder.gameObject.GetComponent<Dropdown>();

                    if (ddTrophy.value != 0)
                    {
                        //spesific trophy 
                        int TrophyId = -1;
                        int.TryParse(ddTrophy.options[ddTrophy.value].text.ToString()[1].ToString(), out TrophyId);
                        if (TrophyId != -1)
                        {
                            Assets.Code.Wrapper.TrophyUtil.UnlockSpesificTrophy(TrophyId);
                        }
                    }
                    else
                    {
                        //else do a foreach
                        for (int i = 1; i < ddTrophy.options.Count; i++)
                        {
                            int TrophyId = -1;
                            int.TryParse(ddTrophy.options[i].text.ToString()[1].ToString(), out TrophyId);
                            if (TrophyId != -1)
                            {
                                Assets.Code.Wrapper.TrophyUtil.UnlockSpesificTrophy(TrophyId);
                                Thread.Sleep(TimeSpan.FromSeconds(5));//sleep for 5 seconds gives the system enough time to take a screenshot
                            }
                        }
                    }

                    return;
                }

            }
            //Remote O
            else if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Keypad6))
            {
                if (CurrentSCreen == GameScreen.PKGScreen || CurrentSCreen == GameScreen.SaveData)
                {
                    //close screen and go back to main menu
                    MainMenu.gameObject.SetActive(true);//Show main menu
                    PKGCanvas.gameObject.SetActive(false);//hide PKG Screen
                    CanvasSaveData.gameObject.SetActive(false);//Hide Save Screen
                    CurrentSCreen = GameScreen.MainScreen;


                    return;
                }
                if (CurrentSCreen == GameScreen.RecoveryTools)
                {
                    RecoveryTools.gameObject.SetActive(false);
                    MainMenu.gameObject.SetActive(true);
                    CurrentSCreen = GameScreen.MainScreen;
                    return;
                }
                if (CurrentSCreen == GameScreen.TrophyScreen)
                {
                    TrophyCanvas.gameObject.SetActive(false);
                    MainMenu.gameObject.SetActive(true);
                    CurrentSCreen = GameScreen.MainScreen;
                    return;
                }
                if (CurrentSCreen == GameScreen.TrophyInfoScreen)
                {
                    TrophyCanvas.gameObject.SetActive(true);
                    MainMenu.gameObject.SetActive(false);
                    TrophyInfoCanvas.gameObject.SetActive(false);
                    PKGCanvas.gameObject.SetActive(false);//hide PKG Screen
                    CanvasSaveData.gameObject.SetActive(false);//Hide Save Screen


                    if (Application.platform == RuntimePlatform.PS4)
                    {
                        SendMessageToPS4("Trying to terminate handle");
                        //if you don't do this you will cause a kpanic
                        Assets.Code.Wrapper.TrophyUtil.DestroyAndTerminate();
                    }
                    CurrentSCreen = GameScreen.TrophyScreen;
                    return;
                }


                if (CurrentSCreen == GameScreen.SaveDataSelected)
                {
                    MainMenu.gameObject.SetActive(false);//Hide main mene
                    CanvasSaveData.gameObject.SetActive(true);//Show Save Screen
                    PKGSelectionCanvas.gameObject.SetActive(false);//hide PKG Screen
                    SaveSelectionCanvas.gameObject.SetActive(false);//hide PKG Screen
                    CurrentSCreen = GameScreen.SaveData;
                    return;
                }
                if (CurrentSCreen == GameScreen.USBPKGMenu)
                {
                    MainMenu.gameObject.SetActive(true);//Hide main mene
                    usbcontentPanel.gameObject.SetActive(false);//Show Save Screen
                    PKGSelectionCanvas.gameObject.SetActive(false);//hide PKG Screen
                    SaveSelectionCanvas.gameObject.SetActive(false);//hide PKG Screen
                    CurrentSCreen = GameScreen.MainScreen;
                    return;
                }
                if (CurrentSCreen == GameScreen.PKGInfoScreen)
                {
                    PKGCanvas.gameObject.SetActive(false);//Show main menu
                    PKGSelectionCanvas.gameObject.SetActive(true);//hide PKG Screen
                    CanvasSaveData.gameObject.SetActive(false);//Hide Save Screen
                    CurrentSCreen = GameScreen.PKGScreen;
                    if (playing_Snd0 == true)
                    {
                        audiosource.Stop();//stop the background music and play our custom music
                        if (!audiosource.isPlaying)
                        {
                            playing_Snd0 = false;
                            AudioClip nextClip;
                            if (randomPlay)
                            {
                                nextClip = GetRandomClip();
                            }
                            else
                            {
                                nextClip = GetNextClip();
                            }

                            audiosource.clip = nextClip;
                            audiosource.Play();
                        }
                    }
                    return;
                }
                if (CurrentSCreen == GameScreen.MainScreen)
                {
                    //Change the GameScreen
                    CurrentSCreen = GameScreen.SaveData;

                    //if (Application.platform == RuntimePlatform.PS4)
                    //{
                    //    //ShowSaveDataDialog();
                    //}
                    //else
                    {
                        Assets.Code.Scenes.SaveData sd = new Assets.Code.Scenes.SaveData();
                        Assets.Code.Scenes.SaveData.Load load = new Assets.Code.Scenes.SaveData.Load();
                        load.ToDisplay(MainMenu, CanvasSaveData, PKGSelectionCanvas, savecontentPanel, savescrollRect, this);
                    }
                    return;
                }
            }
            //remote /\
            else if (Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.Keypad8))
            {
                if (CurrentSCreen == GameScreen.RecoveryTools)
                {
                    int UserId = 0;
                    int.TryParse(GetUserId(), out UserId);
                    string userDirectory = UserId.ToString("x");
                    PS4_Tools.Recovery.TrophyTimeStampFix("/user/home/" + userDirectory + "/trophy/db/trophy_local.db");
                    Assets.Code.MessageBox.Show("Timestamps have been fixed");
                }
                if (CurrentSCreen == GameScreen.PKGInfoScreen)
                {
                    if (PKGOptions.activeSelf == false)
                    {
                        PKGOptions.SetActive(true);//show it
                    }
                    else
                    {
                        PKGOptions.SetActive(false);
                    }
                }
                if (CurrentSCreen == GameScreen.PKGScreen)
                {
                    if (PKGOptionsList.activeSelf == false)
                    {
                        PKGOptionsList.SetActive(true);//show it
                    }
                    else
                    {
                        PKGOptionsList.SetActive(false);
                    }
                }
                if (CurrentSCreen == GameScreen.SaveDataSelected)
                {
                    if (SaveOptionsList.activeSelf == false)
                    {
                        SaveOptionsList.SetActive(true);//show it
                    }
                    else
                    {
                        SaveOptionsList.SetActive(false);
                    }
                }

                if (CurrentSCreen == GameScreen.MainScreen)
                {
                    //not going to allow this now since we want to do a small release first
                    //SendMessageToPS4("Soooon check my github for a manual");
                    //return;
                    //do trophy unlock for all
                    //to do this ill be using a hook functions that looks for the dam trophy call (similair to how silica does it on the vita)
                    //int ret = UnlockTrophies("NPWR12115_00");//must have the NPComid
                    //if (ret != 0) {
                    //error somehwere
                    //txtError.text = "Error on Trophy Unlocker "+ ret.ToString();
                    //}

                    try
                    {
                        //here is to the new one 
                        //int UserId = 0;
                        //int.TryParse(GetUserId(), out UserId);
                        //string userDirectory = UserId.ToString("x");

                        ////Spesific Trophy Unlocking
                        ////PS4_Tools.Trophy_File.Unlock_All_Title_Id("NPWR09395_00", "/user/home/" + userDirectory + "/trophy/db/trophy_local.db", "/user/home/"+ userDirectory + "/trophy/data/sce_trop/trpsummary.dat");
                        ////Full Trophy Unlcoking 
                        //PS4_Tools.Trophy_File.Unlock_All_Trophies("/user/home/" + userDirectory + "/trophy/db/trophy_local.db", "/user/home/" + userDirectory + "/trophy/data/sce_trop/trpsummary.dat");
                        //SendMessageToPS4("All Trophies should now be unlocked!");
                        usbcontentPanel.gameObject.SetActive(false);
                        MainMenu.gameObject.SetActive(false);//hide main menu
                        CanvasSaveData.gameObject.SetActive(false);//hide PKG Screen
                        PKGSelectionCanvas.gameObject.SetActive(false);//Hide Save Screen
                        TrophyCanvas.gameObject.SetActive(true);//show trophy canvas
                        CurrentSCreen = GameScreen.TrophyScreen;


                        //So i'm not exactly sure how to do this
                        /*****************************************

                        My idea is this. we basically just need the nptitle and npbind to make this work

                        So the theory is to do somehting like this maybe.

                        Load all items from /system_data/priv/appmeta/

                        this will give us all the CUSA_ITEMS

                        Load them from the app db to get display info.

                        *********************************************/

                        lstTrophyFiles = new List<CustomTrophyHolder>();




                        //load all trophyfiles here
                        //load all files in the trophy directory
                        if (Application.platform == RuntimePlatform.PS4)
                        {
                            try
                            {
                                Assets.Code.LoadingDialog.Show("Loading all trophy's this might take some time");
                                //SendMessageToPS4("Loading all trophy's this might take some time");
                                List<string> METAINFO = System.IO.Directory.GetDirectories("/system_data/priv/appmeta/", "*.*", SearchOption.TopDirectoryOnly).ToList();

                                List<CustomTrophyHolder> lstTophy = new List<CustomTrophyHolder>();

                                //SendMessageToPS4(METAINFO.Count.ToString());
                                string MetaList = "";
                                for (int i = 0; i < METAINFO.Count; i++)
                                {
                                    CustomTrophyHolder item = new CustomTrophyHolder();
                                    item.MetaInfo = METAINFO[i];
                                    MetaList += "\n" + METAINFO[i];
                                    lstTophy.Add(item);


                                }

                                CreateTrophyView(lstTophy);
                                //Assets.Code.MessageBox.Show(MetaList);
                                Assets.Code.LoadingDialog.Close();

                                //string[] files = System.IO.Directory.GetFiles("/user/trophy/conf/", "*.TRP", SearchOption.AllDirectories);
                                //string[] inifiles = System.IO.Directory.GetFiles("/user/trophy/conf/", "*.INI", SearchOption.AllDirectories);
                                //if (files.Length != 0)
                                //{
                                //    //now build the new items here
                                //    CreateTrophyView(files.ToList(), inifiles.ToList());
                                //    Assets.Code.LoadingDialog.Close();
                                //}
                                //else
                                //{
                                //    Assets.Code.MessageBox.Show("No Trophy Files found on the ps4");
                                //    //SendMessageToPS4("No Trophy Files found on the ps4");
                                //}
                            }
                            catch (Exception ex)
                            {
                                Assets.Code.MessageBox.Show(ex.Message);
                                txtError.text = ex.Message;
                            }
                        }
                        else
                        {
                            List<string> METAINFO = System.IO.Directory.GetDirectories(@"C:\Users\3de Echelon\Desktop\ps4\system_data\priv\appmeta", "*.*", SearchOption.TopDirectoryOnly).ToList();

                            List<CustomTrophyHolder> lstTophy = new List<CustomTrophyHolder>();
                            for (int i = 0; i < METAINFO.Count; i++)
                            {
                                CustomTrophyHolder item = new CustomTrophyHolder();
                                item.MetaInfo = METAINFO[i];
                                lstTophy.Add(item);

                            }

                            CreateTrophyView(lstTophy);

                        }

                    }
                    catch (Exception ex)
                    {
                        txtError.text = "Error " + ex.ToString();
                    }

                    return;
                }
                if (CurrentSCreen == GameScreen.TrophyScreen)
                {

                    //Old no longer valid
                    //try
                    //{
                    //    if (trplist[CurrentItem].trophyconf != null)
                    //    {
                    //        int UserId = 0;
                    //        int.TryParse(GetUserId(), out UserId);
                    //        string userDirectory = UserId.ToString("x");

                    //        PS4_Tools.Trophy_File.Unlock_All_Trophies("/user/home/" + userDirectory + "/trophy/db/trophy_local.db", "/user/home/" + userDirectory + "/trophy/data/sce_trop/trpsummary.dat");
                    //        SendMessageToPS4("All Unlocked via DB");
                    //    }
                    //}
                    //catch
                    //{

                    //}
                    return;
                }

                else if (CurrentSCreen == GameScreen.USBPKGMenu)
                {
                    //var pkginfo = listofpkgs[CurrentItem];
                    if (Application.platform == RuntimePlatform.PS4)
                    {
                        string ErrorList = "";
                        for (int i = 0; i < listofpkgsfiles.Count; i++)
                        {
                            var fileonusb = listofpkgsfiles[i];
                            var pkginfo = listofpkgs[i];
                            FileInfo fi = new FileInfo(fileonusb);
                            int ps4install = InstallPKG(fileonusb, pkginfo.PS4_Title, "");
                            if (ps4install != 0)
                            {
                                ErrorList += pkginfo.PS4_Title + "\n";
                            }
                        }
                        if (!string.IsNullOrEmpty(ErrorList))
                        {
                            if (Application.platform == RuntimePlatform.PS4)
                            {
                                Assets.Code.MessageBox.Show("Error installing the follwing game(s)\n\n" + ErrorList);
                            }
                        }
                    }

                }
            }
            //remote []
            else if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                if (CurrentSCreen == GameScreen.RecoveryTools)
                {
                    //we rebuild the appdb here
                    if (Application.platform == RuntimePlatform.PS4)
                    {
                        Assets.Code.LoadingDialog.Show("Rebuilding App Database");
                        PS4_Tools.Recovery.RebuildAppDb("/system_data/priv/mms/app.db");
                        Assets.Code.MessageBox.Show("Your database has been Rebuilt");
                    }
                    return;
                }
                else if (CurrentSCreen == GameScreen.MainScreen)
                {
                    try
                    {

                        string Display = "System Info:\n\n";

                        //txtError.text += "Developed by "+  DevelopedBy();
                        Display += "IDPS : " + GetIDPS().ToUpper();

                        try
                        {
                            Display += "\nPSID : " + GetPSID().ToUpper();
                            Display += "\nUsername : " + GetUsername();
                            Display += "\nUserId : " + GetUserId();
                            Display += "\nFirmware(Can be spoofed) :" + get_fw().ToString("x");//this works cool now we can use it on the top somwhere idk
                            Display += "\nFirmware : " + get_firmware().ToString();
                            //txtError.text += "\nMountTest : " + MountandLoad();
                            //txtError.text = "\nOpenPSid : " + KernelGetOpenPsId().ToString();
                            //txtError.text += "\nfirmware_version_kernel :" + firmware_version_kernel();
                            //string version = "";
                            //firmware_version_libc(ref version);
                            //txtError.text += "\nfirmware_version_libc :" + version;
                            //GetCallableList();
                        }
                        catch (Exception ex)
                        {

                        }
                        //					txtError.text += "\nFirmware : "+GetKernelVersion();
                        //					txtError.text += "\nKPSID : " + KernelGetOpenPsId().ToUpper();
                        //					txtError.text += "\nUserID : " +GetUserID();
                        //					txtError.text += "\nUserName : " +GetUserName();

                        if (Application.platform == RuntimePlatform.PS4)
                        {
                            Assets.Code.MessageBox.Show(Display);
                        }
                        else
                        {
                            txtError.text = Display;
                        }


                        return;

                    }
                    catch (Exception ex)
                    {
                        txtError.text += "universal crash " + ex.Message;
                    }
                }
                else if (CurrentSCreen == GameScreen.PKGInfoScreen)
                {
                    var pkgitem = systempkglist[CurrentItem];

                    //meta info location
                    string soundlocation = pkgitem.metaDataPath + "/snd0.at9";
                    if (Application.platform != RuntimePlatform.PS4)
                    {
                        soundlocation = @"C:\Program Files (x86)\SCE\ORBIS SDKs\4.500\target\samples\data\audio_video\sound\wave\pad_spk.wav";
                    }

                    byte[] bytes = null;
                    //PS4 Stuff here
                    if (Application.platform == RuntimePlatform.PS4)
                    {
                        //var pkginfo = listofpkgs[CurrentItem];
                        if (File.Exists(soundlocation))
                        {
                            //Thread thread = new Thread(delegate ()
                            //{
                            playing_Snd0 = true;
                            Assets.Code.LoadingDialog.Show("Loading snd0.at9...");
                            bytes = PS4_Tools.Media.Atrac9.LoadAt9(soundlocation);
                            AudioClip audioClip = Assets.Code.WavUtility.ToAudioClip(bytes);

                            // WAV wav = new WAV(bytes);
                            //// Debug.Log(wav);
                            // AudioClip audioClip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false, false);
                            // audioClip.SetData(wav.LeftChannel, 0);
                            // //audio.clip = audioClip;
                            // //audio.Play();

                            audiosource = FindObjectOfType<AudioSource>();
                            audiosource.loop = false;
                            if (audiosource.isPlaying)
                            {
                                audiosource.Stop();
                                //  AudioSource.PlayClipAtPoint(audioClip, new Vector3(100, 100, 0), 1.0f);
                                audiosource.clip = (audioClip);
                                audiosource.Play();
                            }
                            else
                            {
                                audiosource.clip = (audioClip);
                                audiosource.Play();

                                //audiosource.PlayOnDualShock4(Convert.ToInt32(GetUserId()));
                            }



                            //PlaySoundControler(soundlocation);

                            //bytes = PS4_Tools.Media.Atrac9.LoadAt9(soundlocation);
                            //AudioClip audioClip = Assets.Code.WavUtility.ToAudioClip(bytes);
                            Assets.Code.LoadingDialog.Close();
                            // WAV wav = new WAV(bytes);
                            //// Debug.Log(wav);
                            // AudioClip audioClip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false, false);
                            // audioClip.SetData(wav.LeftChannel, 0);
                            // //audio.clip = audioClip;
                            // //audio.Play();

                            //audiosource = FindObjectOfType<AudioSource>();
                            //audiosource.loop = false;
                            //if (audiosource.isPlaying)
                            //{
                            //    audiosource.Stop();
                            //    //  AudioSource.PlayClipAtPoint(audioClip, new Vector3(100, 100, 0), 1.0f);
                            //    audiosource.clip = (audioClip);
                            //    audiosource.Play();
                            //}
                            //else
                            //{
                            //    audiosource.clip = (audioClip);
                            //    audiosource.Play();
                            //}
                            //});
                            //thread.Start();
                        }

                    }
                    else
                    {
                        if (File.Exists(soundlocation))
                        {
                            //Thread thread = new Thread(delegate ()
                            //{
                            playing_Snd0 = true;

                            if (Application.platform == RuntimePlatform.PS4)
                            {
                                Assets.Code.LoadingDialog.Show("Loading snd0.at9...");
                                PlaySoundControler(soundlocation);
                                Assets.Code.LoadingDialog.Close();
                            }
                            else
                            {
                                WWW www = new WWW("file://" + soundlocation);
                                if (www.error != null)
                                {
                                    Debug.Log(www.error);
                                }
                                else
                                {
                                    var audioClip = www.GetAudioClip();

                                    //Assets.Code.LoadingDialog.Close();
                                    // WAV wav = new WAV(bytes);
                                    //// Debug.Log(wav);
                                    // AudioClip audioClip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false, false);
                                    // audioClip.SetData(wav.LeftChannel, 0);
                                    // //audio.clip = audioClip;
                                    // //audio.Play();

                                    audiosource = FindObjectOfType<AudioSource>();
                                    audiosource.loop = false;
                                    if (audiosource.isPlaying)
                                    {
                                        audiosource.Stop();
                                        //  AudioSource.PlayClipAtPoint(audioClip, new Vector3(100, 100, 0), 1.0f);
                                        audiosource.clip = (audioClip);
                                        audiosource.Play();

                                        //audiosource.PlayOnDualShock4(Convert.ToInt32(GetUserId()));
                                    }
                                    else
                                    {
                                        audiosource.clip = (audioClip);
                                        audiosource.Play();

                                        // audiosource.PlayOnDualShock4(Convert.ToInt32(GetUserId()));
                                    }

                                }
                            }
                            //

                            //bytes = PS4_Tools.Media.Atrac9.LoadAt9(soundlocation);
                            //AudioClip audioClip = Assets.Code.WavUtility.ToAudioClip(bytes);
                            //Assets.Code.LoadingDialog.Close();
                            // WAV wav = new WAV(bytes);
                            //// Debug.Log(wav);
                            // AudioClip audioClip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false, false);
                            // audioClip.SetData(wav.LeftChannel, 0);
                            // //audio.clip = audioClip;
                            // //audio.Play();

                            //audiosource = FindObjectOfType<AudioSource>();
                            //audiosource.loop = false;
                            //if (audiosource.isPlaying)
                            //{
                            //    audiosource.Stop();
                            //    //  AudioSource.PlayClipAtPoint(audioClip, new Vector3(100, 100, 0), 1.0f);
                            //    audiosource.clip = (audioClip);
                            //    audiosource.Play();
                            //}
                            //else
                            //{
                            //    audiosource.clip = (audioClip);
                            //    audiosource.Play();
                            //}
                            //});
                            //thread.Start();
                        }
                    }

                    return;
                }
                else if (CurrentSCreen == GameScreen.PKGScreen)
                {
                    var pkgitem = systempkglist[CurrentItem];

                    //meta info location
                    string soundlocation = pkgitem.metaDataPath + "/snd0.at9";


                    byte[] bytes = null;
                    //PS4 Stuff here
                    if (Application.platform == RuntimePlatform.PS4)
                    {
                        //var pkginfo = systempkglist[CurrentItem];
                        if (File.Exists(soundlocation))
                        {
                            //Thread thread = new Thread(delegate ()
                            //{
                            playing_Snd0 = true;
                            Assets.Code.LoadingDialog.Show("Loading snd0.at9...");
                            bytes = PS4_Tools.Media.Atrac9.LoadAt9(soundlocation);
                            AudioClip audioClip = Assets.Code.WavUtility.ToAudioClip(bytes);
                            Assets.Code.LoadingDialog.Close();
                            // WAV wav = new WAV(bytes);
                            //// Debug.Log(wav);
                            // AudioClip audioClip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false, false);
                            // audioClip.SetData(wav.LeftChannel, 0);
                            // //audio.clip = audioClip;
                            // //audio.Play();

                            audiosource = FindObjectOfType<AudioSource>();
                            audiosource.loop = false;
                            if (audiosource.isPlaying)
                            {
                                audiosource.Stop();
                                //  AudioSource.PlayClipAtPoint(audioClip, new Vector3(100, 100, 0), 1.0f);
                                audiosource.clip = (audioClip);
                                audiosource.Play();
                            }
                            else
                            {
                                audiosource.clip = (audioClip);
                                audiosource.Play();
                            }
                            //});
                            //thread.Start();
                        }

                    }

                    else
                    {
                        if (File.Exists(@"C:\Users\3de Echelon\Downloads\OrbisTitleMetadataDatabase-master\OrbisTitleMetadataDatabase-master\OrbisTitleMetadataDatabase\bin\Debug\EP9000-CUSA00002_00-KZ4RELEASE000041\snd0.at9"))
                        {
                            //Thread thread = new Thread(delegate ()
                            //{

                            bytes = PS4_Tools.Media.Atrac9.LoadAt9(@"C:\Users\3de Echelon\Downloads\OrbisTitleMetadataDatabase-master\OrbisTitleMetadataDatabase-master\OrbisTitleMetadataDatabase\bin\Debug\EP9000-CUSA00002_00-KZ4RELEASE000041\snd0.at9");
                            AudioClip audioClip = Assets.Code.WavUtility.ToAudioClip(bytes);

                            // WAV wav = new WAV(bytes);
                            //// Debug.Log(wav);
                            // AudioClip audioClip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false, false);
                            // audioClip.SetData(wav.LeftChannel, 0);
                            // //audio.clip = audioClip;
                            // //audio.Play();

                            audiosource = FindObjectOfType<AudioSource>();
                            audiosource.loop = false;
                            if (audiosource.isPlaying)
                            {
                                audiosource.Stop();
                                //  AudioSource.PlayClipAtPoint(audioClip, new Vector3(100, 100, 0), 1.0f);
                                audiosource.clip = (audioClip);
                                audiosource.Play();
                            }
                            else
                            {
                                audiosource.clip = (audioClip);
                                audiosource.Play();
                            }
                            //});
                            //thread.Start();
                        }
                    }
                }
                else if (CurrentSCreen == GameScreen.SaveDataSelected)
                {
                    var saveitem = savedatafileitems[CurrentItem];
                    int savetest = MountSaveData2(Path.GetFileName(saveitem.SaveFilePath), Path.GetFileName(saveitem.ListOfSaveItesm[ddSave.value - 1].SaveDataFile).Replace("sdimg_", ""));
                    if (savetest != 0)
                    {
                        if (savetest == -2137063421)
                        {
                            //didnt error it actually mounted to the pfs path 
                        }
                        else
                        {
                            if (Application.platform == RuntimePlatform.PS4)
                            {
                                Assets.Code.MessageBox.Show("Error\n\n" + savetest.ToString());
                            }
                            else
                            {
                                txtError.text = savetest.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (Application.platform == RuntimePlatform.PS4)
                        {
                            Assets.Code.MessageBox.Show("Save has been mounted to /mnt/pfs/");
                        }
                        else
                        {
                            txtError.text = "Save has been mounted to /mnt/pfs/";
                        }
                        //SendMessageToPS4("Save has been mounted to /mnt/pfs/");
                    }

                    return;
                }
            }
            //remote [     ]//middle bar
            else if (Input.GetKeyDown(KeyCode.Joystick1Button6) || Input.GetKeyDown(KeyCode.Space))
            {
                try
                {
                    //test Klog and the rest
                    //Unity_Plugin();

                }
                catch (Exception ex)
                {

                }
            }
            //remote options
            else if (Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.RightAlt))
            {
                if (CurrentSCreen == GameScreen.MainScreen)
                {

                    //we are redoing this now
                    Assets.Code.Wrapper.Util.ShowMessageDialog(@"A huge thanks to every developer who contributed to the dev wiki! 
Your work made alot of this possible.

Thanks to:
The Open Orbis Team
ChendoChap
Specter
Diwidog
LightningMods
theorywrong
DefaultDNB
DarkElement
and many many more
");
                    ////Still desiding to either show a pannel or not
                    //if (CreditPanel.activeSelf == false)
                    //{
                    //    CreditPanel.SetActive(true);//show it
                    //}
                    //else
                    //{
                    //    CreditPanel.SetActive(false);
                    //}
                }
                //txtError.text += "\nServiceList " + GetListOfServices ();
            }
            //remote L1
            else if (Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (CurrentSCreen == GameScreen.SaveDataSelected || CurrentSCreen == GameScreen.PKGInfoScreen)
                {
                    try
                    {
                        //just copy all content from pfs to the usb
                        bool coppied = false;
                        try
                        {
                            CopyDir(@"/mnt/pfs/", "/mnt/usb0/SaveData/");
                            coppied = true;
                            Assets.Code.MessageBox.Show("Save Data coppied to usb0");
                        }
                        catch
                        {
                        }
                        if (coppied == false)
                        {
                            CopyDir(@"/mnt/pfs/", "/mnt/usb1/SaveData/");
                            coppied = true;
                            Assets.Code.MessageBox.Show("Save Data coppied to usb1");
                        }
                    }
                    catch (Exception ex)
                    {
                        Assets.Code.MessageBox.Show("Error\n\n" + ex.Message);
                    }
                }
            }
            //remote R1
            else if (Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.RightControl))
            {
                if (CurrentSCreen == GameScreen.SaveDataSelected)
                {
                    try
                    {
                        if (SaveFilesMounted == true)
                        {
                            int unmountret = UnMountSaveData();
                            if (unmountret != 0)
                            {
                                txtError.text = unmountret.ToString();
                            }
                            Assets.Code.MessageBox.Show("Save Data Unmounted");
                            SaveFilesMounted = false;
                        }
                        else
                        {
                            //SetDebuggerTrue();//we want DEBUG INFO 
                            //mount a save
                            if (ddSave.value == 0)
                            {
                                var saveitem = savedatafileitems[CurrentItem];
                                int savetest = MountSaveData(Path.GetFileName(saveitem.SaveFilePath), "0000000000000000000000000000000000000000000000000000000000000000");
                                if (savetest != 0)
                                {
                                    if (savetest == -2137063421)
                                    {
                                        //didnt error it actually mounted to the pfs path 
                                    }
                                    else
                                    {
                                        Assets.Code.MessageBox.Show("Error\n\n" + savetest.ToString());
                                    }
                                }
                                else
                                {
                                    Assets.Code.MessageBox.Show("Save has been mounted to /mnt/pfs/");
                                }
                                SaveFilesMounted = true;
                            }
                            else
                            {
                                //we only want the one 
                                var saveitem = savedatafileitems[CurrentItem];
                                int savetest = MountSaveData_Path(Path.GetFileName(saveitem.SaveFilePath), Path.GetFileName(saveitem.ListOfSaveItesm[ddSave.value - 1].SaveDataFile).Replace("sdimg_", ""), "0000000000000000000000000000000000000000000000000000000000000000");
                                if (savetest != 0)
                                {
                                    if (savetest == -2137063421)
                                    {
                                        //didnt error it actually mounted to the pfs path 
                                    }
                                    else
                                    {
                                        Assets.Code.MessageBox.Show("Error\n\n" + savetest.ToString());
                                    }
                                }
                                else
                                {
                                    Assets.Code.MessageBox.Show("Save has been mounted to /mnt/pfs/");
                                }
                                SaveFilesMounted = true;
                            }
                        }
                    }
                    catch (Exception ermnt)
                    {
                        Assets.Code.MessageBox.Show(ermnt.Message);
                    }
                }
                if (CurrentSCreen == GameScreen.PKGInfoScreen)
                {
                    try
                    {

                    }
                    catch (Exception ex)
                    {
                        Assets.Code.MessageBox.Show(ex.Message);
                    }
                }
            }
            //remote right arrow
            else if (Input.GetKeyDown(KeyCode.Joystick1Button11) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (CurrentSCreen == GameScreen.PKGInfoScreen)
                {
                    /*I'm going to be making a few small ps4 applications*/
                    //Assets.Code.MessageBox.Show("Please download PS4 PKG Manager to lock games");
                    try
                    {
                        var pkginfo = systempkglist[CurrentItem];
                        PS4_Tools.PKG.LockGame(pkginfo.titleId, "/system_data/priv/mms/app.db");
                        Assets.Code.MessageBox.Show("Game locked");
                    }
                    catch (Exception ex)
                    {
                        Assets.Code.MessageBox.Show("Error Locking\n" + ex.Message);
                    }
                    return;
                }
                else if (CurrentSCreen == GameScreen.MainScreen)
                {
                    //time to make a usb screen
                    //yay it works.
                    //if the pakcage is already installed it just won't show anything
                    try
                    {
                        if (Application.platform == RuntimePlatform.PS4)
                        {
                            Assets.Code.LoadingDialog.Show("Loading all pkg's this might take some time");
                        }
                        {
                            try
                            {
                                CurrentItem = 0;
                                MainMenu.gameObject.SetActive(false);//hide main menu
                                UsbCanvas.gameObject.SetActive(true);//SHow PKG Screen
                                CurrentSCreen = GameScreen.USBPKGMenu;
                                PS4_Tools.PKG.SceneRelated.Unprotected_PKG pkginfo = new PKG.SceneRelated.Unprotected_PKG();
                                if (Application.platform != RuntimePlatform.PS4)
                                {

                                    //string[] files = System.IO.Directory.GetFiles(@"C:\Users\3deEchelon\Desktop\PS4\RE\Ps4 Save Data Backup", "*.pkg",SearchOption.AllDirectories);
                                    string[] files = System.IO.Directory.GetFiles(@"F:\Games\Playstation\PS4", "*.pkg", SearchOption.AllDirectories);
                                    listofpkgs = new List<PS4_Tools.PKG.SceneRelated.Unprotected_PKG>();
                                    listofpkgsfiles = new List<string>();
                                    for (int i = 0; i < files.Length; i++)
                                    {
                                        try
                                        {
                                            pkginfo = PS4_Tools.PKG.SceneRelated.Read_PKG(files[i]);
                                            listofpkgs.Add(pkginfo);
                                            listofpkgsfiles.Add(files[i]);
                                        }
                                        catch (Exception ex)
                                        {

                                        }

                                    }
                                    CreatePKGViewListUsb(listofpkgs);
                                    //pkginfo = PS4_Tools.PKG.SceneRelated.Read_PKG (@"C:\Users\3deEchelon\Desktop\PS4\RE\Ps4 Save Data Backup\CUSA00265\app.pkg");

                                }
                                else
                                {
                                    //try and read pkg from here
                                    //how about we just include ps4debug and do all our fancy stuff here 
                                    //pkginfo = PS4_Tools.PKG.SceneRelated.Read_PKG (@"/user/app/CUSA00135/app.pkg");
                                    string[] files1 = System.IO.Directory.GetFiles(@"/mnt/usb0/", "*.pkg", SearchOption.AllDirectories);
                                    string[] files2 = System.IO.Directory.GetFiles(@"/mnt/usb1/", "*.pkg", SearchOption.AllDirectories);
                                    //SendMessageToPS4("Files1:"+files1.Length.ToString() + "Files2:" + files2.Length.ToString());
                                    string[] files = new string[files1.Length + files2.Length];
                                    for (int i = 0; i < files.Length; i++)
                                    {
                                        if (i >= files1.Length)
                                            files[i] = files2[i - files1.Length];
                                        else
                                            files[i] = files1[i];
                                    }
                                    //SendMessageToPS4("files:" + files.Length.ToString());
                                    listofpkgs = new List<PS4_Tools.PKG.SceneRelated.Unprotected_PKG>();
                                    listofpkgsfiles = new List<string>();
                                    for (int i = 0; i < files.Length; i++)
                                    {
                                        try
                                        {
                                            pkginfo = PS4_Tools.PKG.SceneRelated.Read_PKG(files[i]);
                                            listofpkgs.Add(pkginfo);
                                            listofpkgsfiles.Add(files[i]);
                                        }
                                        catch (Exception ex)
                                        {

                                        }

                                    }
                                    CreatePKGViewListUsb(listofpkgs);
                                    if (listofpkgs.Count == 0)
                                    {
                                        MainMenu.gameObject.SetActive(true);//hide main menu
                                        UsbCanvas.gameObject.SetActive(false);//Show PKG Screen
                                        CurrentSCreen = GameScreen.MainScreen;
                                        Assets.Code.MessageBox.Show("No pkg files found on usb do you have a device inserted ?");
                                    }
                                    Assets.Code.LoadingDialog.Close();
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (listofpkgs.Count == 0)
                                {
                                    MainMenu.gameObject.SetActive(true);//hide main menu
                                    UsbCanvas.gameObject.SetActive(false);//Show PKG Screen
                                    CurrentSCreen = GameScreen.MainScreen;
                                    Assets.Code.MessageBox.Show("No pkg files found on usb do you have a device inserted ?");
                                    //Assets.Code.LoadingDialog.Close();
                                }
                                else
                                {
                                    Assets.Code.MessageBox.Show("Error\n\n" + ex.Message);
                                }
                                //SendMessageToPS4("No pkg files found on usb do you have a device inserted ?");
                                //txtError.text = "Error process could not start " + ex.Message + "\n" + ex.Data + "\n" + ex.StackTrace;
                            }
                        }
                    }
                    catch
                    { // (System.Exception ex)
                        ; //LOG = "Error " + ex.Message;
                    }
                    return;
                    //int installing = InstallPKG("/mnt/usb0/EP2107-CUSA00327_00-DONTSTARVEPS4V01.pkg", "P2107-CUSA00327_00-DONTSTARVEPS4V01.pkg");
                    //if (installing != 0)
                    //{
                    //    txtError.text = "Could not install from usb";
                    //}
                }
                //txtError.text = DevelopedBy();
            }
            //remote left arrow
            else if (Input.GetKeyDown(KeyCode.Joystick1Button13) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (CurrentSCreen == GameScreen.PKGInfoScreen)
                {
                    try
                    {
                        var pkginfo = systempkglist[CurrentItem];
                        PS4_Tools.PKG.UnlockGame(pkginfo.titleId, "/system_data/priv/mms/app.db");
                        Assets.Code.MessageBox.Show("Game Unlocked");
                    }
                    catch (Exception ex)
                    {
                        Assets.Code.MessageBox.Show("Error Unlocking\n" + ex.Message);
                        //txtError.text = ex.Message;
                    }
                    return;
                }
                if (CurrentSCreen == GameScreen.MainScreen)
                {
                    //New/Old Method Show ALL PKG's

                    //int result = MakeCusaAppReadWrite();
                    //if (result == 1)
                    //{
                    //    SendMessageToPS4("YAY!");
                    //}
                    //else
                    //{
                    //    SendMessageToPS4("Nope");
                    //}
                    /*We want to just test file writing here now*/
                    //File.WriteAllText("/mnt/sandbox/XDPX20004_000/app0/sce_sys/test.txt", "testing");
                }
            }
            //remote up arrow
            else if (Input.GetKeyDown(KeyCode.Joystick1Button10) || Input.GetKeyDown(KeyCode.UpArrow))
            {

                if (CurrentSCreen == GameScreen.PKGInfoScreen)
                {
                    //this is the time
                    if (SaveFilesMounted == true)
                    {
                        int unmountret = UnMountSaveData();
                        if (unmountret != 0)
                        {
                            txtError.text = unmountret.ToString();
                        }
                        SaveFilesMounted = false;
                    }
                    else
                    {
                        //mount a save
                        var pkginfo = listofpkgs[CurrentItem];
                        var pkgtitleid = listofpkgs[CurrentItem].Param.TITLEID;

                        int savetest = MountSaveData(pkgtitleid, "0000000000000000000000000000000000000000000000000000000000000000");

                        if (savetest != 0)
                        {
                            if (savetest == -2137063421)
                            {
                                //didnt error it actually mounted to the pfs path 
                            }
                            else
                            {
                                txtError.text = savetest.ToString();
                            }
                        }
                        else
                        {
                            SendMessageToPS4("Save has been mounted to /mnt/pfs/");
                        }
                        SaveFilesMounted = true;
                    }
                    return;
                }

                if (CurrentSCreen == GameScreen.PKGScreen)
                {
                    if (CurrentItem > 0 && PKGOptions.activeSelf == false)
                    {
                        UnityEngine.UI.Image imgholder1 = PKGItemGameObjectList[CurrentItem].GetComponent<UnityEngine.UI.Image>();
                        imgholder1.color = Color.black;
                        CurrentItem--;
                        imgholder1 = PKGItemGameObjectList[CurrentItem].GetComponent<UnityEngine.UI.Image>();
                        imgholder1.color = hexToColor("#9F9F9FFF");

                        //					float scrollValue = 1 + contentPanel.anchoredPosition.y/scrollRect.content.rect.height;
                        //					scrollRect.verticalScrollbar.value = scrollValue;					
                        if (scrollRect.verticalNormalizedPosition >= 0 && CurrentItem < 4)
                        {
                            contentPanel.anchoredPosition += new Vector2(0, 146.6F);
                        }

                        if (contentPanel.anchoredPosition.y > 0)
                        {
                            contentPanel.anchoredPosition -= new Vector2(0, 146.6F);
                        }
                        if (contentPanel.anchoredPosition.y < 0)
                        {
                            contentPanel.anchoredPosition = new Vector2(0, 0);
                        }

                        LoadAdditionalInfo();

                        //					var pkginfo = listofpkgs [CurrentItem];
                        //					byte[] output = new byte[32];
                        //					PS4_Tools.Util.SCEUtil.sceSblSsDecryptSealedKey(
                        //
                        //					PKGAditionalInfo.text = "Aditional Information : ";
                        //					PKGAditionalInfo.text += "\nSealedKey : " + ;
                    }
                    else
                    {
                        //this is the time
                        if (SaveFilesMounted == true)
                        {
                            int unmountret = UnMountSaveData();
                            if (unmountret != 0)
                            {
                                txtError.text = unmountret.ToString();
                            }
                            SaveFilesMounted = false;
                        }
                        else
                        {
                            //mount a save
                            var pkginfo = listofpkgs[CurrentItem];
                            var pkgtitleid = listofpkgs[CurrentItem].Param.TITLEID;

                            int savetest = MountSaveData(pkgtitleid, "0000000000000000000000000000000000000000000000000000000000000000");

                            if (savetest != 0)
                            {
                                if (savetest == -2137063421)
                                {
                                    //didnt error it actually mounted to the pfs path 
                                }
                                else
                                {
                                    txtError.text = savetest.ToString();
                                }
                            }
                            else
                            {
                                SendMessageToPS4("Save has been mounted to /mnt/pfs/");
                            }
                            SaveFilesMounted = true;
                        }
                    }
                    return;
                }
                if (CurrentSCreen == GameScreen.USBPKGMenu)
                {

                    if (CurrentItem > 0)
                    {
                        UnityEngine.UI.Image imgholder1 = PKGItemGameObjectList[CurrentItem].GetComponent<UnityEngine.UI.Image>();
                        imgholder1.color = Color.black;
                        CurrentItem--;
                        imgholder1 = PKGItemGameObjectList[CurrentItem].GetComponent<UnityEngine.UI.Image>();
                        imgholder1.color = hexToColor("#9F9F9FFF");

                        //					float scrollValue = 1 + contentPanel.anchoredPosition.y/scrollRect.content.rect.height;
                        //					scrollRect.verticalScrollbar.value = scrollValue;					
                        if (usbcrollRect.verticalNormalizedPosition >= 0 && CurrentItem < 4)
                        {
                            usbcontentPanel.anchoredPosition += new Vector2(0, 146.6F);
                        }

                        if (usbcontentPanel.anchoredPosition.y > 0)
                        {
                            usbcontentPanel.anchoredPosition -= new Vector2(0, 146.6F);
                        }
                        if (usbcontentPanel.anchoredPosition.y < 0)
                        {
                            usbcontentPanel.anchoredPosition = new Vector2(0, 0);
                        }

                        // LoadAdditionalInfo();

                        //					var pkginfo = listofpkgs [CurrentItem];
                        //					byte[] output = new byte[32];
                        //					PS4_Tools.Util.SCEUtil.sceSblSsDecryptSealedKey(
                        //
                        //					PKGAditionalInfo.text = "Aditional Information : ";
                        //					PKGAditionalInfo.text += "\nSealedKey : " + ;
                    }
                    return;
                }

                else if (CurrentSCreen == GameScreen.SaveData)
                {
                    if (CurrentItem > 0)
                    {
                        UnityEngine.UI.Image imgholder1 = SaveItemGameObjectList[CurrentItem].GetComponent<UnityEngine.UI.Image>();
                        imgholder1.color = hexToColor("#FFFFFF00");
                        CurrentItem--;
                        imgholder1 = SaveItemGameObjectList[CurrentItem].GetComponent<UnityEngine.UI.Image>();
                        imgholder1.color = hexToColor("#9F9F9FFF");

                        //					float scrollValue = 1 + contentPanel.anchoredPosition.y/scrollRect.content.rect.height;
                        //					scrollRect.verticalScrollbar.value = scrollValue;					
                        if (savescrollRect.verticalNormalizedPosition >= 0 && CurrentItem < 4)
                        {
                            savecontentPanel.anchoredPosition += new Vector2(0, 146.6F);
                        }

                        if (savecontentPanel.anchoredPosition.y > 0)
                        {
                            savecontentPanel.anchoredPosition -= new Vector2(0, 146.6F);
                        }
                        if (savecontentPanel.anchoredPosition.y < 0)
                        {
                            savecontentPanel.anchoredPosition = new Vector2(0, 0);
                        }
                        //					var pkginfo = listofpkgs [CurrentItem];
                        //					byte[] output = new byte[32];
                        //					PS4_Tools.Util.SCEUtil.sceSblSsDecryptSealedKey(
                        //
                        //					PKGAditionalInfo.text = "Aditional Information : ";
                        //					PKGAditionalInfo.text += "\nSealedKey : " + ;
                    }
                    return;
                }
                else if (CurrentSCreen == GameScreen.SaveDataSelected)
                {
                    //get current ddvalue
                    int CurrentValue = ddSave.value;
                    if (CurrentValue - 1 > -1)
                    {
                        //on down press move to next item in ddsave
                        ddSave.value = CurrentValue - 1;
                    }
                    return;
                }
                else if (CurrentSCreen == GameScreen.TrophyInfoScreen)
                {
                    var ddTrophyHolder = GameObject.Find("ddTrophy");

                    Dropdown ddTrophy = ddTrophyHolder.gameObject.GetComponent<Dropdown>();
                    int CurrentValue = ddTrophy.value;
                    if (CurrentValue - 1 > -1)
                    {
                        //on down press move to next item in ddsave
                        ddTrophy.value = CurrentValue - 1;
                    }
                    return;
                }

                else if (CurrentSCreen == GameScreen.TrophyScreen)
                {
                    if (CurrentItem > 0)
                    {
                        UnityEngine.UI.Image imgholder1 = TrpItemGameObjectList[CurrentItem].GetComponent<UnityEngine.UI.Image>();
                        imgholder1.color = Color.black;
                        CurrentItem--;
                        imgholder1 = TrpItemGameObjectList[CurrentItem].GetComponent<UnityEngine.UI.Image>();
                        imgholder1.color = hexToColor("#9F9F9FFF");

                        //					float scrollValue = 1 + contentPanel.anchoredPosition.y/scrollRect.content.rect.height;
                        //					scrollRect.verticalScrollbar.value = scrollValue;					
                        if (trophycrollRect.verticalNormalizedPosition >= 0 && CurrentItem < 4)
                        {
                            trophycontentPanel.anchoredPosition += new Vector2(0, 146.6F);
                        }

                        if (trophycontentPanel.anchoredPosition.y > 0)
                        {
                            trophycontentPanel.anchoredPosition -= new Vector2(0, 146.6F);
                        }
                        if (trophycontentPanel.anchoredPosition.y < 0)
                        {
                            trophycontentPanel.anchoredPosition = new Vector2(0, 0);
                        }
                        //					var pkginfo = listofpkgs [CurrentItem];
                        //					byte[] output = new byte[32];
                        //					PS4_Tools.Util.SCEUtil.sceSblSsDecryptSealedKey(
                        //
                        //					PKGAditionalInfo.text = "Aditional Information : ";
                        //					PKGAditionalInfo.text += "\nSealedKey : " + ;
                    }
                    return;
                }
            }
            //remote down arrow
            else if (Input.GetKeyDown(KeyCode.Joystick1Button12) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (CurrentSCreen == GameScreen.PKGScreen)
                {
                    if (CurrentItem < PKGItemGameObjectList.Count - 1)
                    {
                        UnityEngine.UI.Image imgholder1 = PKGItemGameObjectList[CurrentItem].GetComponent<UnityEngine.UI.Image>();
                        imgholder1.color = Color.black;
                        CurrentItem++;
                        imgholder1 = PKGItemGameObjectList[CurrentItem].GetComponent<UnityEngine.UI.Image>();
                        imgholder1.color = hexToColor("#9F9F9FFF");
                        //					float scrollValue = 1 + contentPanel.anchoredPosition.y/scrollRect.content.rect.height;
                        //					scrollRect.verticalScrollbar.value = scrollValue;
                        //
                        if (scrollRect.verticalNormalizedPosition >= 0 && CurrentItem > 4)
                        {
                            contentPanel.anchoredPosition += new Vector2(0, 146.6F);
                        }
                        else if (contentPanel.anchoredPosition.y > 0)
                        {
                            contentPanel.anchoredPosition -= new Vector2(0, 146.6F);
                        }
                        else if (contentPanel.anchoredPosition.y < 0)
                        {
                            contentPanel.anchoredPosition = new Vector2(0, 0);
                        }

                        LoadAdditionalInfo();
                    }
                    return;
                }
                if (CurrentSCreen == GameScreen.USBPKGMenu)
                {
                    if (CurrentItem < PKGItemGameObjectList.Count - 1)
                    {
                        UnityEngine.UI.Image imgholder1 = PKGItemGameObjectList[CurrentItem].GetComponent<UnityEngine.UI.Image>();
                        imgholder1.color = Color.black;
                        CurrentItem++;
                        imgholder1 = PKGItemGameObjectList[CurrentItem].GetComponent<UnityEngine.UI.Image>();
                        imgholder1.color = hexToColor("#9F9F9FFF");
                        //					float scrollValue = 1 + contentPanel.anchoredPosition.y/scrollRect.content.rect.height;
                        //					scrollRect.verticalScrollbar.value = scrollValue;
                        //
                        if (usbcrollRect.verticalNormalizedPosition >= 0 && CurrentItem > 4)
                        {
                            usbcontentPanel.anchoredPosition += new Vector2(0, 146.6F);
                        }
                        else if (usbcontentPanel.anchoredPosition.y > 0)
                        {
                            usbcontentPanel.anchoredPosition -= new Vector2(0, 146.6F);
                        }
                        else if (usbcontentPanel.anchoredPosition.y < 0)
                        {
                            usbcontentPanel.anchoredPosition = new Vector2(0, 0);
                        }

                        //LoadAdditionalInfo();
                    }
                    return;
                }
                else if (CurrentSCreen == GameScreen.SaveData)
                {
                    if (CurrentItem < SaveItemGameObjectList.Count - 1)
                    {
                        UnityEngine.UI.Image imgholder1 = SaveItemGameObjectList[CurrentItem].GetComponent<UnityEngine.UI.Image>();
                        imgholder1.color = hexToColor("#FFFFFF00");
                        CurrentItem++;
                        imgholder1 = SaveItemGameObjectList[CurrentItem].GetComponent<UnityEngine.UI.Image>();
                        imgholder1.color = hexToColor("#9F9F9FFF");

                        //					float scrollValue = 1 + contentPanel.anchoredPosition.y/scrollRect.content.rect.height;
                        //					scrollRect.verticalScrollbar.value = scrollValue;					
                        if (savescrollRect.verticalNormalizedPosition >= 0 && CurrentItem > 4)
                        {
                            savecontentPanel.anchoredPosition += new Vector2(0, 146.6F);
                        }
                        else if (savecontentPanel.anchoredPosition.y > 0)
                        {
                            savecontentPanel.anchoredPosition -= new Vector2(0, 146.6F);
                        }
                        else if (savecontentPanel.anchoredPosition.y < 0)
                        {
                            savecontentPanel.anchoredPosition = new Vector2(0, 0);
                        }
                        //					var pkginfo = listofpkgs [CurrentItem];
                        //					byte[] output = new byte[32];
                        //					PS4_Tools.Util.SCEUtil.sceSblSsDecryptSealedKey(
                        //
                        //					PKGAditionalInfo.text = "Aditional Information : ";
                        //					PKGAditionalInfo.text += "\nSealedKey : " + ;
                    }
                    return;
                }
                else if (CurrentSCreen == GameScreen.SaveDataSelected)
                {
                    //get current ddvalue
                    int CurrentValue = ddSave.value;
                    if (ddSave.options.Count > CurrentValue + 1)
                    {
                        //on down press move to next item in ddsave
                        ddSave.value = CurrentValue + 1;
                    }
                    return;
                }

                else if (CurrentSCreen == GameScreen.TrophyInfoScreen)
                {
                    var ddTrophyHolder = GameObject.Find("ddTrophy");

                    Dropdown ddTrophy = ddTrophyHolder.gameObject.GetComponent<Dropdown>();
                    int CurrentValue = ddTrophy.value;
                    if (ddTrophy.options.Count > CurrentValue + 1)
                    {
                        //on down press move to next item in ddsave
                        ddTrophy.value = CurrentValue + 1;
                    }
                    return;
                }

                else if (CurrentSCreen == GameScreen.TrophyScreen)
                {
                    if (CurrentItem < TrpItemGameObjectList.Count - 1)
                    {
                        UnityEngine.UI.Image imgholder1 = TrpItemGameObjectList[CurrentItem].GetComponent<UnityEngine.UI.Image>();
                        imgholder1.color = Color.black;
                        CurrentItem++;
                        imgholder1 = TrpItemGameObjectList[CurrentItem].GetComponent<UnityEngine.UI.Image>();
                        imgholder1.color = hexToColor("#9F9F9FFF");

                        //					float scrollValue = 1 + contentPanel.anchoredPosition.y/scrollRect.content.rect.height;
                        //					scrollRect.verticalScrollbar.value = scrollValue;					
                        if (trophycrollRect.verticalNormalizedPosition >= 0 && CurrentItem > 4)
                        {
                            trophycontentPanel.anchoredPosition += new Vector2(0, 146.6F);
                        }
                        else if (trophycontentPanel.anchoredPosition.y > 0)
                        {
                            trophycontentPanel.anchoredPosition -= new Vector2(0, 146.6F);
                        }
                        else if (trophycontentPanel.anchoredPosition.y < 0)
                        {
                            trophycontentPanel.anchoredPosition = new Vector2(0, 0);
                        }
                        //					var pkginfo = listofpkgs [CurrentItem];
                        //					byte[] output = new byte[32];
                        //					PS4_Tools.Util.SCEUtil.sceSblSsDecryptSealedKey(
                        //
                        //					PKGAditionalInfo.text = "Aditional Information : ";
                        //					PKGAditionalInfo.text += "\nSealedKey : " + ;
                    }
                    return;
                }
                else if (CurrentSCreen == GameScreen.MainScreen)
                {
                    FreeMount();

                    //after freemount check file/folder

                    if (Directory.Exists("/system_sam"))
                    {
                        SendMessageToPS4("system_sam exists");
                    }

                    FreeFTP();

                    try
                    {
                        var IPAddress = GameObject.Find("txtIP");
                        if (IPAddress != null)
                        {
                            IPAddress.gameObject.GetComponent<Text>().text = "IP :" + GetLocalIPAddress() + ":21";
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return;
                }
            }
            //remote L3
            else if (Input.GetKeyDown(KeyCode.Joystick1Button8) || Input.GetKey(KeyCode.LeftAlt))
            {
                //SendMessageToPS4 ("Testing code from Uniity");
                //cause why not have random disco lights
                System.Random r = new System.Random();
                int rInt = r.Next(13, 255);
                int gInt = r.Next(13, 255);
                int bInt = r.Next(13, 255);
                int ret = Change_Controller_Color(rInt, gInt, bInt);
                if (ret != 0)
                {
                    Assets.Code.MessageBox.Show("Error on change color");
                }
            }
            //remote R3
            else if (Input.GetKeyDown(KeyCode.Joystick1Button9) || Input.GetKey(KeyCode.Mouse0))
            {
                if (CurrentSCreen == GameScreen.MainScreen)
                {

                    RecoveryTools.gameObject.SetActive(true);
                    MainMenu.gameObject.SetActive(false);
                    CurrentSCreen = GameScreen.RecoveryTools;

                }
                if (CurrentSCreen == GameScreen.SaveDataSelected)
                {
                    if (Application.platform == RuntimePlatform.PS4)
                    {
                        SendMessageToPS4("Trying to delete");
                    }
                    var saveitem = savedatafileitems[CurrentItem];
                    if (ddSave.value == 0)
                    {
                        //delete all
                        bool savetest = DeleteSaveDataGame(Path.GetFileName(saveitem.SaveFilePath));
                        if (savetest == true)
                        {
                            SendMessageToPS4("All Saves has been deleted");
                        }
                        else
                        {
                            SendMessageToPS4("Delete Save Failed");
                        }
                    }
                    else
                    {
                        //delete just this one
                        //, Path.GetFileName(, "0000000000000000000000000000000000000000000000000000000000000000");
                        bool savetest = DeleteSaveData((saveitem.ListOfSaveItesm[ddSave.value - 1].SaveDataFile).Replace("sdimg_", ""));
                        if (savetest == true)
                        {
                            SendMessageToPS4("Save has been deleted");
                        }
                        else
                        {
                            SendMessageToPS4("Delete Save Failed");
                        }
                    }
                    return;
                }
            }
            else
            {
                foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(vKey))
                    {
                        //your code here
                        //txtError.text = "Key Down : " + vKey.ToString();
                        //SendMessageToPS4("Key pressed :"+ vKey.ToString());
                    }
                }

            }
        }
        catch (Exception ex)
        {
            txtError.text = ex.Message;
        }
    }

    void OnApplicationQuit()
    {
        try
        {
            //if the saves are mounted we need to unmount them else we will cause a kpanic and some other issues 
            if (SaveFilesMounted == true)
            {
                int unmountret = UnMountSaveData();
                if (unmountret != 0)
                {
                    txtError.text = unmountret.ToString();
                }
                SaveFilesMounted = false;
            }
            UnloadPKGModule();
        }
        catch (Exception ex)
        {
            //don't do anytthing
        }
    }

    //Ouput the new value of the Dropdown into Text
    void DropdownValueChanged(Dropdown change)
    {
        if (savedatafileitems.Count != 0)
        {
            if (change.value != 0)
            {
                txtSaveInfo.text = "Save Data File : " + Path.GetFileName(savedatafileitems[CurrentItem].ListOfSaveItesm[change.value - 1].SaveDataFile)
                    + "\nSealedKeyFile : " + Path.GetFileName(savedatafileitems[CurrentItem].ListOfSaveItesm[change.value - 1].SealedKeyFile);
            }
            else
            {
                txtSaveInfo.text = "All items selected";
            }
        }
        else if (change.name == "ddTrophy")
        {
            //var ddTrophyHolder = GameObject.Find("ddTrophy");

            //Dropdown ddTrophy = ddTrophyHolder.gameObject.GetComponent<Dropdown>();

            var TrophyInfoBottom = GameObject.Find("TrophyInfoBottom");
            if (TrophyInfoBottom != null)
            {

                if (change.value != 0)
                {
                    var trophyitem = lstTrophyFiles[CurrentItem];

                    string MetaDataLocation = "";
                    string NpBindLocation = "";

                    var _MetaInfo = trophyitem.AppInfo.Find(x => x.key == "TITLE_ID");
                    if (_MetaInfo != null)
                    {
                        MetaDataLocation = "/system_data/priv/appmeta/" + _MetaInfo.val.TrimEnd() + @"/nptitle.dat";
                    }
                    if (Application.platform != RuntimePlatform.PS4)
                    {
                        MetaDataLocation = @"C:\Users\3de Echelon\Desktop\ps4\system_data\priv\appmeta\" + _MetaInfo.val.TrimEnd() + @"\nptitle.dat";
                    }
                    if (_MetaInfo != null)
                    {
                        NpBindLocation = "/system_data/priv/appmeta/" + _MetaInfo.val.TrimEnd() + @"/npbind.dat";
                    }
                    if (Application.platform != RuntimePlatform.PS4)
                    {
                        NpBindLocation = @"C:\Users\3de Echelon\Desktop\ps4\system_data\priv\appmeta\" + _MetaInfo.val.TrimEnd() + @"\npbind.dat";
                    }

                    //Quickly read the trophy Np id 
                    PKG.SceneRelated.NP_Title npdataholder = new PKG.SceneRelated.NP_Title();
                    npdataholder = new PS4_Tools.PKG.SceneRelated.NP_Title(MetaDataLocation);

                    PKG.SceneRelated.NP_Bind npbindHolder = new PKG.SceneRelated.NP_Bind();
                    npbindHolder = new PKG.SceneRelated.NP_Bind(NpBindLocation);

                    string NpTitle = npbindHolder.Nptitle;

                    var lstoftrp1 = DataProcsessing.Trophy.GetAllTrophyFlags(NpTitle.Trim());
                    var TrophyId = change.options[change.value].text.ToString()[1];
                    var lstoftrp = lstoftrp1.FindAll(x => x.trophy_title_id == NpTitle);

                    var LastItem = lstoftrp.Find(x => x.trophyid == TrophyId.ToString());

                    string Grade = "";
                    switch (LastItem.grade)
                    {
                        case "1":
                            Grade = "Platinum";
                            break;
                        case "2":
                            Grade = "Gold";
                            break;
                        case "3":
                            Grade = "Silver";
                            break;
                        case "4":
                            Grade = "Bronze";
                            break;
                        default:
                            Grade = "Unknown";
                            break;

                    }



                    Text TrophyInfoBottomText = TrophyInfoBottom.gameObject.GetComponent<Text>();
                    TrophyInfoBottomText.text = "Title :" + LastItem.title + "\tDescription :" + LastItem.description + "\nGrade :" + Grade + "\tStatus :" + (LastItem.unlocked == "0" ? "LOCKED" : "UNLOCKED");

                }
                else
                {
                    Text TrophyInfoBottomText = TrophyInfoBottom.gameObject.GetComponent<Text>();
                    TrophyInfoBottomText.text = "All Selected";
                }
            }
        }


    }


    IEnumerator ProcessPKGs(string[] files)
    {
        PS4_Tools.PKG.SceneRelated.Unprotected_PKG pkginfo = new PKG.SceneRelated.Unprotected_PKG();
        PKGItemGameObjectList.Clear();
        listofpkgs = new List<PS4_Tools.PKG.SceneRelated.Unprotected_PKG>();

        //clear the list first
        int count = contentPanel.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject.Destroy(contentPanel.GetChild(i).gameObject);
        }

        for (int i = 0; i < files.Length; i++)
        {

            yield return pkginfo = PS4_Tools.PKG.SceneRelated.Read_PKG(files[i]);
            if (pkginfo != null)
            {
                listofpkgs.Add(pkginfo);

                AddItemsToPKGViewList(pkginfo);
            }

        }

        Assets.Code.LoadingDialog.Close();
        //CreatePKGViewList(listofpkgs);
        LoadAdditionalInfo();
        yield return listofpkgs;

    }

    IEnumerator ProcessPKGs(List<Assets.Code.Models.AppBrowse> files)
    {
        //PS4_Tools.PKG.SceneRelated.Unprotected_PKG pkginfo = new PKG.SceneRelated.Unprotected_PKG();
        PKGItemGameObjectList.Clear();
        systempkglist = new List<Assets.Code.Models.AppBrowse>();

        //clear the list first
        int count = contentPanel.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject.Destroy(contentPanel.GetChild(i).gameObject);
        }

        for (int i = 0; i < files.Count; i++)
        {


            if (files[i] != null)
            {
                systempkglist.Add(files[i]);

                AddItemsToPKGViewListSystem(files[i]);
            }

        }
        if (Application.platform == RuntimePlatform.PS4)
        {
            Assets.Code.LoadingDialog.Close();
        }
        //CreatePKGViewList(listofpkgs);
        LoadAdditionalInfo();
        yield return listofpkgs;

    }


    //	public static System.Data.DataTable getDataTable(String qry,SQLiteConnection con)
    //	{
    //		try
    //		{
    //		   System.Data.DataTable dt = new System.Data.DataTable();
    //			using (SQLiteDataAdapter da = new SQLiteDataAdapter(qry, con))
    //			{
    //				da.Fill(dt);
    //			}
    //			return dt;
    //		}
    //		catch (Exception ex)
    //		{
    //			return new System.Data.DataTable();
    //		}
    //		
    //	}
    //
    //	public List<string> GetSaveDirs(string ProductCode,string savedbloc = "/system_data/savedata/10000000/db/user/savedata.db")
    //	{
    //		try {
    //			//string cs = "Data Source =" + savedbloc;
    //			List<string> rtnlist = new List<string> ();
    //			string cs = string.Format(@"Data Source={0};", savedbloc);
    //
    //			string stm = "select * from savedata where title_id = '"+ProductCode+"'";
    //
    //			var con = new SQLiteConnection (cs);
    //			con.Open ();
    //			var dt = getDataTable(stm,con);
    //			for (int i = 0; i < dt.Rows.Count; i++) {
    //				rtnlist.Add(dt.Rows[i]["dir_name"].ToString());
    //			}
    //
    //			con.Close ();//cant keep this open for to long
    //			return rtnlist;
    //		} catch (Exception ex) {
    //			string error = ex.Message;
    //			Debug.LogError (error);
    //		}
    //		return new List<string>();
    //	}}
}

public class WAV
{

    // convert two bytes to one float in the range -1 to 1
    static float bytesToFloat(byte firstByte, byte secondByte)
    {
        // convert two bytes to one short (little endian)
        short s = (short)((secondByte << 8) | firstByte);
        // convert to range from -1 to (just below) 1
        return s / 32768.0F;
    }

    static int bytesToInt(byte[] bytes, int offset = 0)
    {
        int value = 0;
        for (int i = 0; i < 4; i++)
        {
            value |= ((int)bytes[offset + i]) << (i * 8);
        }
        return value;
    }

    private static byte[] GetBytes(string filename)
    {
        return File.ReadAllBytes(filename);
    }
    // properties
    public float[] LeftChannel { get; internal set; }
    public float[] RightChannel { get; internal set; }
    public int ChannelCount { get; internal set; }
    public int SampleCount { get; internal set; }
    public int Frequency { get; internal set; }

    // Returns left and right double arrays. 'right' will be null if sound is mono.
    public WAV(string filename) :
        this(GetBytes(filename))
    { }

    public WAV(byte[] wav)
    {

        // Determine if mono or stereo
        ChannelCount = wav[22];     // Forget byte 23 as 99.999% of WAVs are 1 or 2 channels

        // Get the frequency
        Frequency = bytesToInt(wav, 24);

        // Get past all the other sub chunks to get to the data subchunk:
        int pos = 12;   // First Subchunk ID from 12 to 16

        // Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))
        while (!(wav[pos] == 100 && wav[pos + 1] == 97 && wav[pos + 2] == 116 && wav[pos + 3] == 97))
        {
            pos += 4;
            int chunkSize = wav[pos] + wav[pos + 1] * 256 + wav[pos + 2] * 65536 + wav[pos + 3] * 16777216;
            pos += 4 + chunkSize;
        }
        pos += 8;

        // Pos is now positioned to start of actual sound data.
        SampleCount = (wav.Length - pos) / 2;     // 2 bytes per sample (16 bit sound mono)
        if (ChannelCount == 2) SampleCount /= 2;        // 4 bytes per sample (16 bit stereo)

        // Allocate memory (right will be null if only mono sound)
        LeftChannel = new float[SampleCount];
        if (ChannelCount == 2) RightChannel = new float[SampleCount];
        else RightChannel = null;

        // Write to double array/s:
        int i = 0;
        while (pos < wav.Length)
        {
            LeftChannel[i] = bytesToFloat(wav[pos], wav[pos + 1]);
            pos += 2;
            if (ChannelCount == 2)
            {
                RightChannel[i] = bytesToFloat(wav[pos], wav[pos + 1]);
                pos += 2;
            }
            i++;
        }
    }

    public override string ToString()
    {
        return string.Format("[WAV: LeftChannel={0}, RightChannel={1}, ChannelCount={2}, SampleCount={3}, Frequency={4}]", LeftChannel, RightChannel, ChannelCount, SampleCount, Frequency);
    }
}
