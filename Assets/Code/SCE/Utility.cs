//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Assets.Code.SCE
//{
//    public static class Utility
//    {
//        // Token: 0x06001C09 RID: 7177 RVA: 0x0009161D File Offset: 0x0008F81D
//        public static Utility.AppLauncher LaunchApp(string titleId, string arg = "", EventMonitor.AppAttr appAttr = EventMonitor.AppAttr.None)
//        {
//            return Utility.AppLauncher.LaunchApp(titleId, arg, appAttr);
//        }

//        // Token: 0x06001C0A RID: 7178 RVA: 0x00091627 File Offset: 0x0008F827
//        public static Utility.AppLauncher LaunchApp(string titleId, byte[] args, int argsSize, LncUtil.LaunchAppParam param, int errorCode)
//        {
//            return Utility.AppLauncher.LaunchApp(titleId, args, argsSize, param, errorCode);
//        }


//        public class AppLauncher
//        {
//            // Token: 0x14000069 RID: 105
//            // (add) Token: 0x06001C32 RID: 7218 RVA: 0x000923A0 File Offset: 0x000905A0
//            // (remove) Token: 0x06001C33 RID: 7219 RVA: 0x000923D4 File Offset: 0x000905D4
//            internal static event Action<List<string>> TitleNotFoundAction;

//            // Token: 0x1400006A RID: 106
//            // (add) Token: 0x06001C34 RID: 7220 RVA: 0x00092408 File Offset: 0x00090608
//            // (remove) Token: 0x06001C35 RID: 7221 RVA: 0x0009243C File Offset: 0x0009063C
//            internal static event Action<string, byte[], int, LncUtil.LaunchAppParam> AppKillingAction;

//            // Token: 0x17000302 RID: 770
//            // (get) Token: 0x06001C36 RID: 7222 RVA: 0x0009246F File Offset: 0x0009066F
//            // (set) Token: 0x06001C37 RID: 7223 RVA: 0x00092476 File Offset: 0x00090676
//            internal static int JobCount { get; private set; }

//            // Token: 0x1400006B RID: 107
//            // (add) Token: 0x06001C38 RID: 7224 RVA: 0x00092480 File Offset: 0x00090680
//            // (remove) Token: 0x06001C39 RID: 7225 RVA: 0x000924B8 File Offset: 0x000906B8
//            public event Action<Utility.AppLauncher> LaunchFinishedAction;

//            // Token: 0x06001C3A RID: 7226 RVA: 0x000924F0 File Offset: 0x000906F0
//            public static Utility.AppLauncher LaunchApp(string titleId, string arg = "", EventMonitor.AppAttr appAttr = EventMonitor.AppAttr.None)
//            {
//                Utility.AppLauncher appLauncher = new Utility.AppLauncher(titleId, arg, appAttr);
//                appLauncher.Start();
//                return appLauncher;
//            }

//            // Token: 0x06001C3B RID: 7227 RVA: 0x00092510 File Offset: 0x00090710
//            public static Utility.AppLauncher LaunchApp(string titleId, byte[] args, int argsSize, LncUtil.LaunchAppParam param, int errorCode)
//            {
//                Utility.AppLauncher appLauncher = new Utility.AppLauncher(titleId, args, argsSize, param, errorCode);
//                appLauncher.Start();
//                return appLauncher;
//            }

//            // Token: 0x06001C3C RID: 7228 RVA: 0x00092530 File Offset: 0x00090730
//            //public static Utility.AppLauncher LaunchApp(List<string> titleIdList, AppMessagingWrapper.SceAppMessage? appMessage)
//            //{
//            //    Utility.AppLauncher appLauncher = new Utility.AppLauncher(titleIdList, appMessage);
//            //    appLauncher.Start();
//            //    return appLauncher;
//            //}

//            // Token: 0x06001C3D RID: 7229 RVA: 0x0009254C File Offset: 0x0009074C
//            public void Cancel()
//            {
//                //if (this.m_job != null)
//                //{
//                //    this.m_job.Cancel();
//                //    this.m_job = null;
//                //}
//                //Utility.AppLauncher.UnregisterLaunchMessage(this);
//            }

//            // Token: 0x06001C3E RID: 7230 RVA: 0x0009256E File Offset: 0x0009076E
//            public void ContinueSession()
//            {
//                this.IsNewSession = false;
//            }

//            // Token: 0x17000303 RID: 771
//            // (get) Token: 0x06001C3F RID: 7231 RVA: 0x00092577 File Offset: 0x00090777
//            // (set) Token: 0x06001C40 RID: 7232 RVA: 0x0009257F File Offset: 0x0009077F
//            public int AppId { get; private set; }

//            // Token: 0x06001C41 RID: 7233 RVA: 0x00092588 File Offset: 0x00090788
//            private AppLauncher()
//            {
//                LncUtil.LaunchAppParamInit(ref this.LaunchAppParam);
//                this.LaunchAppParam.userId = Convert.ToInt32(MainClass.Get_UserId());//get forgeground user here;
//            }

//            // Token: 0x06001C42 RID: 7234 RVA: 0x000925F8 File Offset: 0x000907F8
//            private AppLauncher(string titleId, string arg = "", EventMonitor.AppAttr appAttr = EventMonitor.AppAttr.None) : this()
//            {
//                this.TitleIdList.Add(titleId);
//                this.LaunchAppParam.appAttr = (int)appAttr;
//                this.Args = Encoding.UTF8.GetBytes(arg);
//            }

//            // Token: 0x06001C43 RID: 7235 RVA: 0x00092649 File Offset: 0x00090849
//            private AppLauncher(string titleId, byte[] args, int argsSize, LncUtil.LaunchAppParam param, int errorCode) : this()
//            {
//                this.TitleIdList.Add(titleId);
//                this.Args = new byte[argsSize];
//                Buffer.BlockCopy(args, 0, this.Args, 0, argsSize);
//                this.LaunchAppParam = param;
//                this.m_errorCode = errorCode;
//            }

//            // Token: 0x06001C44 RID: 7236 RVA: 0x00092688 File Offset: 0x00090888
//            //private AppLauncher(List<string> titleIdList, AppMessagingWrapper.SceAppMessage? appMessage) : this()
//            //{
//            //    if (titleIdList != null)
//            //    {
//            //        this.TitleIdList.AddRange(titleIdList);
//            //    }
//            //    if (appMessage != null)
//            //    {
//            //        this.AppMessage = appMessage.Value;
//            //        this.ExistAppMessage = true;
//            //    }
//            //}

//            // Token: 0x06001C45 RID: 7237 RVA: 0x000927D8 File Offset: 0x000909D8
//            private void Start()
//            {
//                Utility.AppLauncher.EventLocker locker = new Utility.AppLauncher.EventLocker();
//                Utility.AppLauncher.JobCount++;
//                // this.m_job = JobQueuePool.LauncherQueue.Enqueue(delegate ()
//                {
//                    if (!this.CheckInitDaemons())
//                    {
//                        return;
//                    }
//                    //Utility.AppLauncher.RegisterLaunchMessage(this);
//                    //if (this.SendAppMessageIfExist())
//                    //{
//                    //    return;
//                    //}
//                    //string launchTitle = this.GetLaunchTitle();
//                    //if (launchTitle == null)
//                    //{
//                    //    return;
//                    //}
//                    locker.StartLayerChangeMonitor();
//                    //LncUtil.LaunchApp(launchTitle, this.Args, this.Args.Length, ref this.LaunchAppParam);
//                    return;
//                    //}, delegate (JobCompletedEventArgs obj)
//                    //{
//                    //    locker.Dispose();
//                    //    Utility.AppLauncher.JobCount--;
//                    //    if (!obj.Cancelled)
//                    //    {
//                    //        this.m_job = null;
//                    //        if (this.LaunchFinishedAction != null)
//                    //        {
//                    //            this.LaunchFinishedAction.Invoke(this);
//                    //        }
//                    //    }
//                    //}, "");
//                }
//            }

//            // Token: 0x06001C46 RID: 7238 RVA: 0x00092838 File Offset: 0x00090A38
//            private bool CheckInitDaemons()
//            {
//                //long elapsedMilliseconds = UT.ElapsedMilliseconds;
//                //while (UT.ElapsedMilliseconds - elapsedMilliseconds < 60000L)
//                //{
//                //    if (PowerManager.IsStateChanging)
//                //    {
//                //        return false;
//                //    }
//                //    if (BootManager.GetEventFlag(BootManager.EventFlag.BackgroundInitFinishedInitDaemons))
//                //    {
//                //        return true;
//                //    }
//                //    Thread.Sleep(100);
//                //}
//                return false;
//            }

//            // Token: 0x06001C47 RID: 7239 RVA: 0x00092878 File Offset: 0x00090A78
//            private int SendAppMessage(int appId)
//            {
//                return 1;
//                //return AppMessagingWrapper.sceAppMessagingSendMsg(appId, this.AppMessage.msgType, this.AppMessage.payload, this.AppMessage.payloadSize, 1U);
//            }

//            // Token: 0x06001C48 RID: 7240 RVA: 0x000929AC File Offset: 0x00090BAC
//            private bool SendAppMessageIfExist()
//            {
//                //bool found = false;
//                //if (this.ExistAppMessage)
//                //{
//                //    UT.CallMain(delegate ()
//                //    {
//                //        foreach (string titleId in this.TitleIdList)
//                //        {
//                //            int appId = ApplicationMonitor.GetAppId(titleId);
//                //            if (0 <= appId && !LncUtil.IsKilling(appId))
//                //            {
//                //                Utility.PostTask(delegate
//                //                {
//                //                    this.SendAppMessage(appId);
//                //                });
//                //                ApplicationCoordinator.Instance.SetFocusApp(ApplicationMonitor.GetAppEvent(appId).AppType);
//                //                this.AppId = appId;
//                //                LayerManager.SetFocusLayer(LayerManager.FocusLayer.Game, this);
//                //                found = true;
//                //                break;
//                //            }
//                //        }
//                //    });
//                //}
//                //if (found)
//                //{
//                //    CompanionManager.NotifyAbortByAlreadyRunning(this.TitleIdList);
//                //}
//                return false;
//            }

//            // Token: 0x06001C49 RID: 7241 RVA: 0x00092AC0 File Offset: 0x00090CC0
//            //private string GetLaunchTitle()
//            //{
//            //    if (Util.IsTitleIdReplaced((EventMonitor.AppAttr)this.LaunchAppParam.appAttr) && 0 < this.TitleIdList.Count)
//            //    {
//            //        return this.TitleIdList[0];
//            //    }
//            //    foreach (string text in this.TitleIdList)
//            //    {
//            //        ApplicationMonitor.Util.AppInfo appInfo = new ApplicationMonitor.Util.AppInfo(text);
//            //        if (appInfo.Metadata != null)
//            //        {
//            //            return text;
//            //        }
//            //    }
//            //    foreach (string text2 in this.TitleIdList)
//            //    {
//            //        if (ApplicationMonitor.AppConfig.IsLaunchable(text2))
//            //        {
//            //            return text2;
//            //        }
//            //    }
//            //    bool found = false;
//            //    UT.CallMain(delegate ()
//            //    {
//            //        foreach (string titleId in this.TitleIdList)
//            //        {
//            //            int appId = ApplicationMonitor.GetAppId(titleId);
//            //            if (0 <= appId)
//            //            {
//            //                ApplicationCoordinator.Instance.SetFocusApp(ApplicationMonitor.GetAppEvent(appId).AppType);
//            //                this.AppId = appId;
//            //                LayerManager.SetFocusLayer(LayerManager.FocusLayer.Game, this);
//            //                found = true;
//            //                break;
//            //            }
//            //        }
//            //    });
//            //    if (found)
//            //    {
//            //        return null;
//            //    }
//            //    if (Utility.AppLauncher.TitleNotFoundAction != null)
//            //    {
//            //        CompanionManager.NotifyAbortByNotInstalled(this.TitleIdList);
//            //        UT.CallMain(delegate ()
//            //        {
//            //            if (Utility.AppLauncher.TitleNotFoundAction != null)
//            //            {
//            //                Utility.AppLauncher.TitleNotFoundAction.Invoke(this.TitleIdList);
//            //            }
//            //        });
//            //        return null;
//            //    }
//            //    if (this.TitleIdList.Count != 0)
//            //    {
//            //        return this.TitleIdList[0];
//            //    }
//            //    return null;
//            //}

//            // Token: 0x06001C4A RID: 7242 RVA: 0x00092C3C File Offset: 0x00090E3C
//            //private bool LaunchWebLink()
//            //{
//            //    if (this.m_errorCode != -2136735697)
//            //    {
//            //        return false;
//            //    }
//            //    if (this.TitleIdList.Count == 0)
//            //    {
//            //        return false;
//            //    }
//            //    string titleId = this.TitleIdList[0];
//            //    string uri = ApplicationMonitor.Util.AppInfo.GetWeblinkUri(titleId);
//            //    if (!UT.Empty(uri))
//            //    {
//            //        UT.CallMain(delegate ()
//            //        {
//            //            BootHelper.Boot(uri, BootHelper.Option.AddApp, titleId, null);
//            //        });
//            //    }
//            //    return true;
//            //}

//            // Token: 0x06001C4B RID: 7243 RVA: 0x00092CB8 File Offset: 0x00090EB8
//            //private bool IsRandomWalkBootBlocked(string titleId)
//            //{
//            //    if (!UIConsoleRandomWalk.IsRunning())
//            //    {
//            //        return false;
//            //    }
//            //    ApplicationMonitor.Util.AppInfo appInfo = new ApplicationMonitor.Util.AppInfo(titleId);
//            //    if (appInfo == null || appInfo.Metadata == null || appInfo.Icon0Path == null)
//            //    {
//            //        return false;
//            //    }
//            //    JsonArray configAs = DebugPlugin.GetConfigAs<JsonArray>("AppSystem.app_launcher.randomwalk_bootable", null);
//            //    if (configAs == null)
//            //    {
//            //        return false;
//            //    }
//            //    foreach (string text in UT.GetEnumerator<string>(configAs))
//            //    {
//            //        if (text == titleId)
//            //        {
//            //            return false;
//            //        }
//            //    }
//            //    return true;
//            //}

//            // Token: 0x06001C4C RID: 7244 RVA: 0x00092DBC File Offset: 0x00090FBC
//            //private bool IsAppKilling(string titleId)
//            //{
//            //    bool isKilling = false;
//            //    UT.CallMain(delegate ()
//            //    {
//            //        int appId = ApplicationMonitor.GetAppId(titleId);
//            //        if (appId != -1 && LncUtil.IsKilling(appId) && Utility.AppLauncher.AppKillingAction != null)
//            //        {
//            //            Utility.AppLauncher.AppKillingAction.Invoke(titleId, this.Args, this.Args.Length, this.LaunchAppParam);
//            //            isKilling = true;
//            //        }
//            //    });
//            //    return isKilling;
//            //}

//            // Token: 0x06001C4D RID: 7245 RVA: 0x00092E60 File Offset: 0x00091060
//            //private static void RegisterLaunchMessage(Utility.AppLauncher launcher)
//            //{
//            //    UT.CallMain(delegate ()
//            //    {
//            //        if (!launcher.IsNewSession)
//            //        {
//            //            return;
//            //        }
//            //        if (!Utility.AppLauncher.s_eventRegistered)
//            //        {
//            //            Utility.AppLauncher.s_eventRegistered = true;
//            //            EventMonitor.Instance.AppLaunchEnd += new EventHandler<EventMonitor.EventArgs>(Utility.AppLauncher.OnAppLaunchEnd);
//            //        }
//            //        Utility.AppLauncher.s_lastLauncher = (launcher.ExistAppMessage ? launcher : null);
//            //    });
//            //}

//            // Token: 0x06001C4E RID: 7246 RVA: 0x00092EA8 File Offset: 0x000910A8
//            //private static void UnregisterLaunchMessage(Utility.AppLauncher launcher)
//            //{
//            //    UT.CallMain(delegate ()
//            //    {
//            //        if (Utility.AppLauncher.s_lastLauncher == launcher)
//            //        {
//            //            Utility.AppLauncher.s_lastLauncher = null;
//            //        }
//            //    });
//            //}

//            // Token: 0x06001C4F RID: 7247 RVA: 0x00092EF0 File Offset: 0x000910F0
//            //private static void OnAppLaunchEnd(object sender, EventMonitor.EventArgs e)
//            //{
//            //    if (Utility.AppLauncher.s_lastLauncher == null)
//            //    {
//            //        return;
//            //    }
//            //    if (e.AppType != EventMonitor.AppType.MiniApp && e.AppType != EventMonitor.AppType.BigApp)
//            //    {
//            //        return;
//            //    }
//            //    if (!Utility.AppLauncher.s_lastLauncher.TitleIdList.Contains(e.TitleId) || (e.AppAttr & EventMonitor.AppAttr.LaunchByDebugger) != EventMonitor.AppAttr.None)
//            //    {
//            //        Utility.AppLauncher.UnregisterLaunchMessage(Utility.AppLauncher.s_lastLauncher);
//            //        return;
//            //    }
//            //    if (0 <= e.ErrorCode || e.ErrorCode == -2137784308)
//            //    {
//            //        Utility.AppLauncher launcher = Utility.AppLauncher.s_lastLauncher;
//            //        Utility.AppLauncher.UnregisterLaunchMessage(Utility.AppLauncher.s_lastLauncher);
//            //        int appId = (e.ErrorCode == -2137784308) ? e.ExistingAppId : e.AppId;
//            //        Utility.PostTask(delegate
//            //        {
//            //            launcher.SendAppMessage(appId);
//            //        });
//            //    }
//            //}

//            // Token: 0x040010FA RID: 4346
//            private List<string> TitleIdList = new List<string>();

//            // Token: 0x040010FB RID: 4347
//            private byte[] Args = Encoding.UTF8.GetBytes("");

//            // Token: 0x040010FC RID: 4348
//            private LncUtil.LaunchAppParam LaunchAppParam = default(LncUtil.LaunchAppParam);

//            // Token: 0x040010FD RID: 4349
//            //private AppMessagingWrapper.SceAppMessage AppMessage = default(AppMessagingWrapper.SceAppMessage);

//            // Token: 0x040010FE RID: 4350
//            private bool ExistAppMessage;

//            // Token: 0x040010FF RID: 4351
//            private bool IsNewSession = true;

//            // Token: 0x04001100 RID: 4352
//            //private Job m_job;

//            // Token: 0x04001101 RID: 4353
//            private int m_errorCode;

//            // Token: 0x04001102 RID: 4354
//            private static bool s_eventRegistered;

//            // Token: 0x04001103 RID: 4355
//            private static Utility.AppLauncher s_lastLauncher;

//            // Token: 0x020002E9 RID: 745
//            private class EventLocker
//            {
//                // Token: 0x06001C51 RID: 7249 RVA: 0x00092FA8 File Offset: 0x000911A8
//                public EventLocker()
//                {
//                    //this.m_superKeyLocker = new Locker(LockType.PSButton | LockType.PSButtonDouble | LockType.ShareButton | LockType.ShareButtonLong | LockType.ShareButtonDouble);
//                    //List<string> list = new List<string>();
//                    //list.Add("Game");
//                    //list.Add("TopMenu");
//                    //list.Add("ShellApp");
//                    //this.m_uiLocker = new Utility.InputLocker(list);
//                    //VoiceRecognitionUtil.LockVoiceRecognition();
//                }

//                // Token: 0x06001C52 RID: 7250 RVA: 0x00093000 File Offset: 0x00091200
//                //protected override void DisposeManagedResource()
//                //{
//                //    LayerManager.FocusLayerChangedAction -= new Action<LayerManager.FocusLayer, object>(this.OnFocusLayerChanged);
//                //    VoiceRecognitionUtil.UnLockVoiceRecognition();
//                //    if (this.m_uiLocker != null)
//                //    {
//                //        this.m_uiLocker.Dispose();
//                //        this.m_uiLocker = null;
//                //    }
//                //    if (this.m_superKeyLocker != null)
//                //    {
//                //        this.m_superKeyLocker.Dispose();
//                //        this.m_superKeyLocker = null;
//                //    }
//                //    base.DisposeManagedResource();
//                //}

//                // Token: 0x06001C53 RID: 7251 RVA: 0x00093078 File Offset: 0x00091278
//                public void StartLayerChangeMonitor()
//                {
//                    //UT.CallMain(delegate ()
//                    //{
//                    //    if (LayerManager.GetFocusLayer() != LayerManager.FocusLayer.Game)
//                    //    {
//                    //        LayerManager.FocusLayerChangedAction += new Action<LayerManager.FocusLayer, object>(this.OnFocusLayerChanged);
//                    //    }
//                    //});
//                }

//                // Token: 0x06001C54 RID: 7252 RVA: 0x0009308B File Offset: 0x0009128B
//                //private void OnFocusLayerChanged(LayerManager.FocusLayer focusLayer, object reason)
//                //{
//                //    if (focusLayer == LayerManager.FocusLayer.Game)
//                //    {
//                //        base.Dispose();
//                //    }
//                //}

//                //// Token: 0x04001106 RID: 4358
//                //private Locker m_superKeyLocker;

//                //// Token: 0x04001107 RID: 4359
//                //private Utility.InputLocker m_uiLocker;
//            }
//        }

//    }
//}
