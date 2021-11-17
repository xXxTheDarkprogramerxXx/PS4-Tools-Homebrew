//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices;
//using System.Text;

//namespace Assets.Code.SCE
//{
//    public class LncUtil
//    {
//        [DllImport("libSceLncUtil")]
//        // Token: 0x06001CEC RID: 7404
//        [MethodImpl(4096)]
//        public static extern void LaunchAppParamInit(ref LncUtil.LaunchAppParam param);

//        // Token: 0x06001CED RID: 7405
//        [DllImport("libSceLncUtil", EntryPoint = "sceLncUtilLaunchApp")]
//        public static extern int LaunchApp([MarshalAs(20)] string titleId, [MarshalAs(42, ArraySubType = (UnmanagedType)(20) , SizeParamIndex = 0)] string[] argv, ref LncUtil.LaunchAppParam param);

//        // Token: 0x06001CEE RID: 7406 RVA: 0x00094ABC File Offset: 0x00092CBC
//        public static int LaunchApp(string titleId, byte[] args, int argsSize, ref LncUtil.LaunchAppParam param)
//        {
//            string @string = Encoding.UTF8.GetString(args, 0, argsSize);
//            string text = @string;
//            char[] array = new char[1];
//            string[] array2 = text.Split(array);
//            array2[array2.Length - 1] = null;
//            return LncUtil.LaunchApp(titleId, array2, ref param);
//        }

//        // Token: 0x06001CEF RID: 7407
//        [DllImport("libSceLncUtil", EntryPoint = "sceLncUtilSetAppFocus")]
//        public static extern int SetAppFocus(int appId, LncUtil.Flag flag = LncUtil.Flag.None);

//        // Token: 0x06001CF0 RID: 7408
//        [DllImport("libSceLncUtil", EntryPoint = "sceLncUtilSetControllerFocus")]
//        public static extern int SetControllerFocus(int appId);

//        // Token: 0x06001CF1 RID: 7409
//        [DllImport("libSceLncUtil", EntryPoint = "sceLncUtilSuspendApp")]
//        public static extern int SuspendApp(int appId, LncUtil.Flag flag = LncUtil.Flag.None);

//        // Token: 0x06001CF2 RID: 7410
//        [DllImport("libSceLncUtil", EntryPoint = "sceLncUtilResumeApp")]
//        public static extern int ResumeApp(int appId, LncUtil.Flag flag = LncUtil.Flag.None);

//        // Token: 0x06001CF3 RID: 7411
//        [DllImport("libSceLncUtil", EntryPoint = "sceLncUtilGetAppStatus")]
//        public static extern int GetAppStatus(ref LncUtil.AppStatus status);

//        // Token: 0x06001CF4 RID: 7412
//        [DllImport("libSceLncUtil", EntryPoint = "sceLncUtilGetAppTitleId")]
//        public static extern int GetAppTitleId(int appId, StringBuilder titleId);

//        // Token: 0x06001CF5 RID: 7413
//        [DllImport("libSceLncUtil", EntryPoint = "sceLncUtilGetAppId")]
//        public static extern int GetAppId([MarshalAs(20)] string titleId);

//        // Token: 0x06001CF6 RID: 7414
//        [DllImport("libSceLncUtil", EntryPoint = "sceLncUtilKillApp")]
//        private static extern int _KillApp(int appId);

//        // Token: 0x06001CF7 RID: 7415
//        [DllImport("libSceLncUtil", EntryPoint = "sceLncUtilForceKillApp")]
//        private static extern int _ForceKillApp(int appId);

//        // Token: 0x06001CF8 RID: 7416 RVA: 0x00094AF8 File Offset: 0x00092CF8
//        public static int KillApp(int appId)
//        {
//            lock (LncUtil.s_killListLock)
//            {
//                LncUtil.s_killList.Add(appId);
//            }
//            int result = LncUtil._KillApp(appId);
//            lock (LncUtil.s_killListLock)
//            {
//                LncUtil.s_killList.Remove(appId);
//            }
//            return result;
//        }

//        // Token: 0x06001CF9 RID: 7417 RVA: 0x00094B7C File Offset: 0x00092D7C
//        public static int ForceKillApp(int appId)
//        {
//            lock (LncUtil.s_killListLock)
//            {
//                LncUtil.s_killList.Add(appId);
//            }
//            int result = LncUtil._ForceKillApp(appId);
//            lock (LncUtil.s_killListLock)
//            {
//                LncUtil.s_killList.Remove(appId);
//            }
//            return result;
//        }

//        // Token: 0x06001CFA RID: 7418 RVA: 0x00094C00 File Offset: 0x00092E00
//        public static bool IsKilling(int appId)
//        {
//            bool result;
//            lock (LncUtil.s_killListLock)
//            {
//                result = LncUtil.s_killList.Contains(appId);
//            }
//            return result;
//        }

//        // Token: 0x06001CFB RID: 7419
//        [DllImport("libSceLncUtil", EntryPoint = "sceLncUtilGetAppStatusListForShellUIReboot")]
//        public static extern int GetAppStatusListForShellUIReboot([In] [Out] LncUtil.AppStatusForShellUIReboot[] outStatusList, uint numEntries, ref uint outEntries);

//        // Token: 0x06001CFC RID: 7420
//        [DllImport("libSceLncUtil", EntryPoint = "sceLncUtilSystemSuspend")]
//        public static extern int SystemSuspend();

//        // Token: 0x06001CFD RID: 7421 RVA: 0x00094C48 File Offset: 0x00092E48
//        //public static int SystemShutdown(LncUtil.Boot bootFlag)
//        //{
//        //    if (bootFlag == LncUtil.Boot.Eap)
//        //    {
//        //        return SystemStateMgrWrapper.EnterStandby();
//        //    }
//        //    return SystemStateMgrWrapper.TurnOff();
//        //}

//        // Token: 0x06001CFE RID: 7422 RVA: 0x00094C59 File Offset: 0x00092E59
//        //public static int SystemReboot()
//        //{
//        //    return SystemStateMgrWrapper.Reboot();
//        //}

//        // Token: 0x06001CFF RID: 7423
//        [DllImport("libSceLncUtil", EntryPoint = "sceLncUtilGetCoredumpState")]
//        public static extern int GetCoredumpState(int appId, int appLocalPid, ref LncUtil.CoredumpState outState);

//        // Token: 0x06001D00 RID: 7424
//        [DllImport("libSceLncUtil", EntryPoint = "sceLncUtilNotifyCoredumpRequestProgress")]
//        public static extern int NotifyCoredumpRequestProgress(int appId, int appLocalPid, int percentage);

//        // Token: 0x06001D01 RID: 7425
//        [DllImport("libSceLncUtil", EntryPoint = "sceLncUtilNotifyCoredumpRequestEnd")]
//        public static extern int NotifyCoredumpRequestEnd(int appId, int appLocalPid, int result);

//        // Token: 0x06001D02 RID: 7426
//        [DllImport("libSceLncUtil", EntryPoint = "sceLncUtilRaiseException")]
//        public static extern int RaiseException(int appId, ulong exception);

//        // Token: 0x06001D03 RID: 7427
//        [DllImport("libSceLncUtil", EntryPoint = "sceLncUtilIsAppSuspended")]
//        public static extern int IsAppSuspended(int appId, ref bool pIsSuspended);

//        // Token: 0x04001159 RID: 4441
//        public const int MAX_TITLE_ID_LEN = 16;

//        // Token: 0x0400115A RID: 4442
//        public const int MAX_PATH_SIZE = 1024;

//        // Token: 0x0400115B RID: 4443
//        public const int MAX_ARGS_SIZE = 4096;

//        // Token: 0x0400115C RID: 4444
//        public const int InvalidAppId = -1;

//        // Token: 0x0400115D RID: 4445
//        private static object s_killListLock = new object();

//        // Token: 0x0400115E RID: 4446
//        private static List<int> s_killList = new List<int>();

//        // Token: 0x02000302 RID: 770
//        [Flags]
//        public enum Flag : ulong
//        {
//            // Token: 0x04001160 RID: 4448
//            None = 0UL,
//            // Token: 0x04001161 RID: 4449
//            SkipLaunchCheck = 1UL,
//            // Token: 0x04001162 RID: 4450
//            SkipResumeCheck = 1UL,
//            // Token: 0x04001163 RID: 4451
//            SkipSystemUpdateCheck = 2UL
//        }

//        // Token: 0x02000303 RID: 771
//        [Flags]
//        public enum Boot
//        {
//            // Token: 0x04001165 RID: 4453
//            None = 0,
//            // Token: 0x04001166 RID: 4454
//            Eap = 1
//        }

//        // Token: 0x02000304 RID: 772
//        [StructLayout(LayoutKind.Sequential, Pack = 4)]
//        public struct LaunchAppParam
//        {
//            // Token: 0x04001167 RID: 4455
//            public uint size;

//            // Token: 0x04001168 RID: 4456
//            public int userId;

//            // Token: 0x04001169 RID: 4457
//            public int appAttr;

//            // Token: 0x0400116A RID: 4458
//            public int enableCrashReport;

//            // Token: 0x0400116B RID: 4459
//            public LncUtil.Flag checkFlag;
//        }

//        // Token: 0x02000305 RID: 773
//        [StructLayout(LayoutKind.Sequential, Pack = 4)]
//        public struct AppStatus
//        {
//            // Token: 0x06001D06 RID: 7430 RVA: 0x00094C7E File Offset: 0x00092E7E
//            public AppStatus(int _appId = -1, int _launchRequestAppId = -1, int _dummy = -1)
//            {
//                this.appId = _appId;
//                this.launchRequestAppId = _launchRequestAppId;
//            }

//            // Token: 0x0400116C RID: 4460
//            public int appId;

//            // Token: 0x0400116D RID: 4461
//            public int launchRequestAppId;
//        }

//        // Token: 0x02000306 RID: 774
//        [StructLayout(LayoutKind.Sequential, Pack = 4)]
//        public struct AppStatusForShellUIReboot
//        {
//            // Token: 0x06001D07 RID: 7431 RVA: 0x00094C90 File Offset: 0x00092E90
//            public AppStatusForShellUIReboot(int _appId = -1, EventMonitor.AppType _appType = EventMonitor.AppType.Unknown, EventMonitor.AppAttr _appAttr = EventMonitor.AppAttr.None, int _launchRequestAppId = -1, int _userId = -1, int _isActiveCdlg = 0, string _path = "", int _isCoredumped = 0, int _appLocalPid = -1)
//            {
//                this.appId = _appId;
//                this.appType = _appType;
//                this.appAttr = _appAttr;
//                this.launchRequestAppId = _launchRequestAppId;
//                this.userId = _userId;
//                this.isActiveCdlg = _isActiveCdlg;
//                this.path = _path;
//                this.isCoredumped = _isCoredumped;
//                this.appLocalPid = _appLocalPid;
//            }

//            // Token: 0x0400116E RID: 4462
//            public int appId;

//            // Token: 0x0400116F RID: 4463
//            public EventMonitor.AppType appType;

//            // Token: 0x04001170 RID: 4464
//            public EventMonitor.AppAttr appAttr;

//            // Token: 0x04001171 RID: 4465
//            public int launchRequestAppId;

//            // Token: 0x04001172 RID: 4466
//            public int userId;

//            // Token: 0x04001173 RID: 4467
//            public int isActiveCdlg;

//            // Token: 0x04001174 RID: 4468
//            [MarshalAs(23, SizeConst = 1024)]
//            public string path;

//            // Token: 0x04001175 RID: 4469
//            public int isCoredumped;

//            // Token: 0x04001176 RID: 4470
//            public int appLocalPid;
//        }

//        // Token: 0x02000307 RID: 775
//        [StructLayout(LayoutKind.Sequential, Pack = 4)]
//        public struct CoredumpState
//        {
//            // Token: 0x06001D08 RID: 7432 RVA: 0x00094CE2 File Offset: 0x00092EE2
//            public CoredumpState(uint _progress = 0U, uint _isFinished = 0U, int _result = 0)
//            {
//                this.progress = _progress;
//                this.isFinished = _isFinished;
//                this.result = _result;
//            }

//            // Token: 0x04001177 RID: 4471
//            public uint progress;

//            // Token: 0x04001178 RID: 4472
//            public uint isFinished;

//            // Token: 0x04001179 RID: 4473
//            public int result;
//        }
//    }
//}
