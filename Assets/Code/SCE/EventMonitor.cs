//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;

//namespace Assets.Code.SCE
//{
//    public class EventMonitor
//    {
//        // Token: 0x1400006D RID: 109
//        // (add) Token: 0x06001D0E RID: 7438 RVA: 0x00094E14 File Offset: 0x00093014
//        // (remove) Token: 0x06001D0F RID: 7439 RVA: 0x00094E4C File Offset: 0x0009304C
//        public event EventHandler<EventMonitor.EventArgs> AppLaunchStart;

//        // Token: 0x1400006E RID: 110
//        // (add) Token: 0x06001D10 RID: 7440 RVA: 0x00094E84 File Offset: 0x00093084
//        // (remove) Token: 0x06001D11 RID: 7441 RVA: 0x00094EBC File Offset: 0x000930BC
//        public event EventHandler<EventMonitor.EventArgs> AppLaunchEnd;

//        // Token: 0x1400006F RID: 111
//        // (add) Token: 0x06001D12 RID: 7442 RVA: 0x00094EF4 File Offset: 0x000930F4
//        // (remove) Token: 0x06001D13 RID: 7443 RVA: 0x00094F2C File Offset: 0x0009312C
//        public event EventHandler<EventMonitor.EventArgs> AppKillStart;

//        // Token: 0x14000070 RID: 112
//        // (add) Token: 0x06001D14 RID: 7444 RVA: 0x00094F64 File Offset: 0x00093164
//        // (remove) Token: 0x06001D15 RID: 7445 RVA: 0x00094F9C File Offset: 0x0009319C
//        public event EventHandler<EventMonitor.EventArgs> AppKillEnd;

//        // Token: 0x14000071 RID: 113
//        // (add) Token: 0x06001D16 RID: 7446 RVA: 0x00094FD4 File Offset: 0x000931D4
//        // (remove) Token: 0x06001D17 RID: 7447 RVA: 0x0009500C File Offset: 0x0009320C
//        public event EventHandler<EventMonitor.EventArgs> AppSuspending;

//        // Token: 0x14000072 RID: 114
//        // (add) Token: 0x06001D18 RID: 7448 RVA: 0x00095044 File Offset: 0x00093244
//        // (remove) Token: 0x06001D19 RID: 7449 RVA: 0x0009507C File Offset: 0x0009327C
//        public event EventHandler<EventMonitor.EventArgs> AppSuspended;

//        // Token: 0x14000073 RID: 115
//        // (add) Token: 0x06001D1A RID: 7450 RVA: 0x000950B4 File Offset: 0x000932B4
//        // (remove) Token: 0x06001D1B RID: 7451 RVA: 0x000950EC File Offset: 0x000932EC
//        public event EventHandler<EventMonitor.EventArgs> AppResuming;

//        // Token: 0x14000074 RID: 116
//        // (add) Token: 0x06001D1C RID: 7452 RVA: 0x00095124 File Offset: 0x00093324
//        // (remove) Token: 0x06001D1D RID: 7453 RVA: 0x0009515C File Offset: 0x0009335C
//        public event EventHandler<EventMonitor.EventArgs> AppResumed;

//        // Token: 0x14000075 RID: 117
//        // (add) Token: 0x06001D1E RID: 7454 RVA: 0x00095194 File Offset: 0x00093394
//        // (remove) Token: 0x06001D1F RID: 7455 RVA: 0x000951CC File Offset: 0x000933CC
//        public event EventHandler<EventMonitor.EventArgs> CdlgActive;

//        // Token: 0x14000076 RID: 118
//        // (add) Token: 0x06001D20 RID: 7456 RVA: 0x00095204 File Offset: 0x00093404
//        // (remove) Token: 0x06001D21 RID: 7457 RVA: 0x0009523C File Offset: 0x0009343C
//        public event EventHandler<EventMonitor.EventArgs> CdlgDeactive;

//        // Token: 0x14000077 RID: 119
//        // (add) Token: 0x06001D22 RID: 7458 RVA: 0x00095274 File Offset: 0x00093474
//        // (remove) Token: 0x06001D23 RID: 7459 RVA: 0x000952AC File Offset: 0x000934AC
//        public event EventHandler<EventMonitor.EventArgs> CoredumpStart;

//        // Token: 0x14000078 RID: 120
//        // (add) Token: 0x06001D24 RID: 7460 RVA: 0x000952E4 File Offset: 0x000934E4
//        // (remove) Token: 0x06001D25 RID: 7461 RVA: 0x0009531C File Offset: 0x0009351C
//        public event EventHandler<EventMonitor.EventArgs> AppFocusStart;

//        // Token: 0x14000079 RID: 121
//        // (add) Token: 0x06001D26 RID: 7462 RVA: 0x00095354 File Offset: 0x00093554
//        // (remove) Token: 0x06001D27 RID: 7463 RVA: 0x0009538C File Offset: 0x0009358C
//        public event EventHandler<EventMonitor.EventArgs> AppFocusEnd;

//        // Token: 0x1400007A RID: 122
//        // (add) Token: 0x06001D28 RID: 7464 RVA: 0x000953C4 File Offset: 0x000935C4
//        // (remove) Token: 0x06001D29 RID: 7465 RVA: 0x000953FC File Offset: 0x000935FC
//        public event EventHandler<EventMonitor.EventArgs> ControllerFocusStart;

//        // Token: 0x1400007B RID: 123
//        // (add) Token: 0x06001D2A RID: 7466 RVA: 0x00095434 File Offset: 0x00093634
//        // (remove) Token: 0x06001D2B RID: 7467 RVA: 0x0009546C File Offset: 0x0009366C
//        public event EventHandler<EventMonitor.EventArgs> ControllerFocusEnd;

//        // Token: 0x1700032B RID: 811
//        // (get) Token: 0x06001D2C RID: 7468 RVA: 0x000954A1 File Offset: 0x000936A1
//        public static EventMonitor Instance
//        {
//            get
//            {
//                return EventMonitor.s_instance;
//            }
//        }

//        // Token: 0x06001D2D RID: 7469 RVA: 0x000954A8 File Offset: 0x000936A8
//        public void Update()
//        {
//            int num = EventMonitor.UpdateEvent();
//            if (num != 0)
//            {
//                return;
//            }
//            EventMonitor.Event @event = default(EventMonitor.Event);
//            EventMonitor.GetEvent(ref @event);
//            EventMonitor.EventArgs eventArgs = new EventMonitor.EventArgs(ref @event);
//            EventMonitor.EventType eventType = @event.eventType;
//            switch (eventType)
//            {
//                case EventMonitor.EventType.Invalid:
//                case (EventMonitor.EventType)9:
//                case (EventMonitor.EventType)10:
//                case (EventMonitor.EventType)11:
//                case (EventMonitor.EventType)12:
//                case (EventMonitor.EventType)13:
//                case (EventMonitor.EventType)14:
//                case (EventMonitor.EventType)15:
//                case (EventMonitor.EventType)16:
//                    break;
//                case EventMonitor.EventType.AppLaunchStart:
//                    if (this.AppLaunchStart != null)
//                    {
//                        this.AppLaunchStart.Invoke(this, eventArgs);
//                        return;
//                    }
//                    return;
//                case EventMonitor.EventType.AppLaunchEnd:
//                    if (this.AppLaunchEnd != null)
//                    {
//                        this.AppLaunchEnd.Invoke(this, eventArgs);
//                        return;
//                    }
//                    return;
//                case EventMonitor.EventType.AppKillStart:
//                    if (this.AppKillStart != null)
//                    {
//                        this.AppKillStart.Invoke(this, eventArgs);
//                        return;
//                    }
//                    return;
//                case EventMonitor.EventType.AppKillEnd:
//                    if (this.AppKillEnd != null)
//                    {
//                        this.AppKillEnd.Invoke(this, eventArgs);
//                        return;
//                    }
//                    return;
//                case EventMonitor.EventType.AppSuspending:
//                    if (this.AppSuspending != null)
//                    {
//                        this.AppSuspending.Invoke(this, eventArgs);
//                        return;
//                    }
//                    return;
//                case EventMonitor.EventType.AppSuspended:
//                    if (this.AppSuspended != null)
//                    {
//                        this.AppSuspended.Invoke(this, eventArgs);
//                        return;
//                    }
//                    return;
//                case EventMonitor.EventType.AppResuming:
//                    if (this.AppResuming != null)
//                    {
//                        this.AppResuming.Invoke(this, eventArgs);
//                        return;
//                    }
//                    return;
//                case EventMonitor.EventType.AppResumed:
//                    if (this.AppResumed != null)
//                    {
//                        this.AppResumed.Invoke(this, eventArgs);
//                        return;
//                    }
//                    return;
//                case EventMonitor.EventType.CdlgActive:
//                    if (this.CdlgActive != null)
//                    {
//                        this.CdlgActive.Invoke(this, eventArgs);
//                        return;
//                    }
//                    return;
//                case EventMonitor.EventType.CdlgDeactive:
//                    if (this.CdlgDeactive != null)
//                    {
//                        this.CdlgDeactive.Invoke(this, eventArgs);
//                        return;
//                    }
//                    return;
//                default:
//                    if (eventType != EventMonitor.EventType.CoredumpStart)
//                    {
//                        switch (eventType)
//                        {
//                            case EventMonitor.EventType.AppFocusStart:
//                                if (this.AppFocusStart != null)
//                                {
//                                    this.AppFocusStart.Invoke(this, eventArgs);
//                                    return;
//                                }
//                                return;
//                            case EventMonitor.EventType.AppFocusEnd:
//                                if (this.AppFocusEnd != null)
//                                {
//                                    this.AppFocusEnd.Invoke(this, eventArgs);
//                                    return;
//                                }
//                                return;
//                            case EventMonitor.EventType.ControllerFocusStart:
//                                if (this.ControllerFocusStart != null)
//                                {
//                                    this.ControllerFocusStart.Invoke(this, eventArgs);
//                                    return;
//                                }
//                                return;
//                            case EventMonitor.EventType.ControllerFocusEnd:
//                                if (this.ControllerFocusEnd != null)
//                                {
//                                    this.ControllerFocusEnd.Invoke(this, eventArgs);
//                                    return;
//                                }
//                                return;
//                        }
//                    }
//                    else
//                    {
//                        if (this.CoredumpStart != null)
//                        {
//                            this.CoredumpStart.Invoke(this, eventArgs);
//                            return;
//                        }
//                        return;
//                    }
//                    break;
//            }
//            throw new InvalidOperationException();
//        }

//        // Token: 0x06001D2E RID: 7470
//        [DllImport("libSceLncUtil", EntryPoint = "LncProxy::UpdateEvent")]
//        private static extern int UpdateEvent();

//        // Token: 0x06001D2F RID: 7471
//        [DllImport("libSceLncUtil", EntryPoint = "LncProxy::GetEvent")]
//        private static extern int GetEvent(ref EventMonitor.Event e);

//        // Token: 0x04001189 RID: 4489
//        private static EventMonitor s_instance = new EventMonitor();

//        // Token: 0x0200030A RID: 778
//        public enum EventType
//        {
//            // Token: 0x0400118B RID: 4491
//            Invalid,
//            // Token: 0x0400118C RID: 4492
//            AppLaunchStart,
//            // Token: 0x0400118D RID: 4493
//            AppLaunchEnd,
//            // Token: 0x0400118E RID: 4494
//            AppKillStart,
//            // Token: 0x0400118F RID: 4495
//            AppKillEnd,
//            // Token: 0x04001190 RID: 4496
//            AppSuspending,
//            // Token: 0x04001191 RID: 4497
//            AppSuspended,
//            // Token: 0x04001192 RID: 4498
//            AppResuming,
//            // Token: 0x04001193 RID: 4499
//            AppResumed,
//            // Token: 0x04001194 RID: 4500
//            CdlgActive = 17,
//            // Token: 0x04001195 RID: 4501
//            CdlgDeactive,
//            // Token: 0x04001196 RID: 4502
//            CoredumpStart = 241,
//            // Token: 0x04001197 RID: 4503
//            AppFocusStart = 4097,
//            // Token: 0x04001198 RID: 4504
//            AppFocusEnd,
//            // Token: 0x04001199 RID: 4505
//            ControllerFocusStart,
//            // Token: 0x0400119A RID: 4506
//            ControllerFocusEnd
//        }

//        // Token: 0x0200030B RID: 779
//        public enum AppType
//        {
//            // Token: 0x0400119C RID: 4508
//            Unknown,
//            // Token: 0x0400119D RID: 4509
//            ShellUI,
//            // Token: 0x0400119E RID: 4510
//            Daemon,
//            // Token: 0x0400119F RID: 4511
//            CDLG,
//            // Token: 0x040011A0 RID: 4512
//            MiniApp,
//            // Token: 0x040011A1 RID: 4513
//            BigApp,
//            // Token: 0x040011A2 RID: 4514
//            ShellCore,
//            // Token: 0x040011A3 RID: 4515
//            ShellApp
//        }

//        // Token: 0x0200030C RID: 780
//        [Flags]
//        public enum AppAttr
//        {
//            // Token: 0x040011A5 RID: 4517
//            None = 0,
//            // Token: 0x040011A6 RID: 4518
//            DisableSystemBG = 1,
//            // Token: 0x040011A7 RID: 4519
//            LaunchByDebugger = 2,
//            // Token: 0x040011A8 RID: 4520
//            LaunchByAppHomeData = 4,
//            // Token: 0x040011A9 RID: 4521
//            LaunchByAppHomeHost = 8
//        }

//        // Token: 0x0200030D RID: 781
//        public class EventArgs : System.EventArgs
//        {
//            // Token: 0x06001D32 RID: 7474 RVA: 0x000956E7 File Offset: 0x000938E7
//            public EventArgs(ref EventMonitor.Event e)
//            {
//                this.e = e;
//            }

//            // Token: 0x1700032C RID: 812
//            // (get) Token: 0x06001D33 RID: 7475 RVA: 0x000956FB File Offset: 0x000938FB
//            public EventMonitor.EventType EventType
//            {
//                get
//                {
//                    return this.e.eventType;
//                }
//            }

//            // Token: 0x1700032D RID: 813
//            // (get) Token: 0x06001D34 RID: 7476 RVA: 0x00095708 File Offset: 0x00093908
//            public string TitleId
//            {
//                get
//                {
//                    return this.e.titleId;
//                }
//            }

//            // Token: 0x1700032E RID: 814
//            // (get) Token: 0x06001D35 RID: 7477 RVA: 0x00095715 File Offset: 0x00093915
//            public int AppId
//            {
//                get
//                {
//                    return this.e.appId;
//                }
//            }

//            // Token: 0x1700032F RID: 815
//            // (get) Token: 0x06001D36 RID: 7478 RVA: 0x00095722 File Offset: 0x00093922
//            public EventMonitor.AppType AppType
//            {
//                get
//                {
//                    return this.e.appType;
//                }
//            }

//            // Token: 0x17000330 RID: 816
//            // (get) Token: 0x06001D37 RID: 7479 RVA: 0x0009572F File Offset: 0x0009392F
//            public EventMonitor.AppAttr AppAttr
//            {
//                get
//                {
//                    return this.e.appAttr;
//                }
//            }

//            // Token: 0x17000331 RID: 817
//            // (get) Token: 0x06001D38 RID: 7480 RVA: 0x0009573C File Offset: 0x0009393C
//            public int LaunchRequestAppId
//            {
//                get
//                {
//                    return this.e.launchRequestAppId;
//                }
//            }

//            // Token: 0x17000332 RID: 818
//            // (get) Token: 0x06001D39 RID: 7481 RVA: 0x00095749 File Offset: 0x00093949
//            public int UserId
//            {
//                get
//                {
//                    return this.e.userId;
//                }
//            }

//            // Token: 0x17000333 RID: 819
//            // (get) Token: 0x06001D3A RID: 7482 RVA: 0x00095756 File Offset: 0x00093956
//            public int ErrorCode
//            {
//                get
//                {
//                    return this.e.errorCode;
//                }
//            }

//            // Token: 0x17000334 RID: 820
//            // (get) Token: 0x06001D3B RID: 7483 RVA: 0x00095763 File Offset: 0x00093963
//            public int ErrorData
//            {
//                get
//                {
//                    return this.e.errorData;
//                }
//            }

//            // Token: 0x17000335 RID: 821
//            // (get) Token: 0x06001D3C RID: 7484 RVA: 0x00095770 File Offset: 0x00093970
//            public int AppLocalPid
//            {
//                get
//                {
//                    return this.e.appLocalPid;
//                }
//            }

//            // Token: 0x17000336 RID: 822
//            // (get) Token: 0x06001D3D RID: 7485 RVA: 0x0009577D File Offset: 0x0009397D
//            public string Path
//            {
//                get
//                {
//                    return this.e.path;
//                }
//            }

//            // Token: 0x17000337 RID: 823
//            // (get) Token: 0x06001D3E RID: 7486 RVA: 0x0009578A File Offset: 0x0009398A
//            public byte[] Args
//            {
//                get
//                {
//                    return this.e.args;
//                }
//            }

//            // Token: 0x17000338 RID: 824
//            // (get) Token: 0x06001D3F RID: 7487 RVA: 0x00095797 File Offset: 0x00093997
//            public int ArgSize
//            {
//                get
//                {
//                    return this.e.argsSize;
//                }
//            }

//            // Token: 0x17000339 RID: 825
//            // (get) Token: 0x06001D40 RID: 7488 RVA: 0x000957A4 File Offset: 0x000939A4
//            //public EventMonitor.CauseCode CauseCode
//            //{
//            //    get
//            //    {
//            //        return UT.GetEnumValue<EventMonitor.CauseCode>(this.e.causeCode, EventMonitor.CauseCode.Unaviable);
//            //    }
//            //}

//            // Token: 0x1700033A RID: 826
//            // (get) Token: 0x06001D41 RID: 7489 RVA: 0x000957B7 File Offset: 0x000939B7
//            public int ExistingAppId
//            {
//                get
//                {
//                    return this.e.existingAppId;
//                }
//            }

//            // Token: 0x1700033B RID: 827
//            // (get) Token: 0x06001D42 RID: 7490 RVA: 0x000957C4 File Offset: 0x000939C4
//            public ulong CheckFlag
//            {
//                get
//                {
//                    return this.e.checkFlag;
//                }
//            }

//            // Token: 0x1700033C RID: 828
//            // (get) Token: 0x06001D43 RID: 7491 RVA: 0x000957D1 File Offset: 0x000939D1
//            //public EventMonitor.CoredumpReason CoredumpReason
//            //{
//            //    get
//            //    {
//            //        return UT.GetEnumValue<EventMonitor.CoredumpReason>(this.e.coredumpReason, EventMonitor.CoredumpReason.None);
//            //    }
//            //}

//            // Token: 0x040011AA RID: 4522
//            private EventMonitor.Event e;
//        }

//        // Token: 0x0200030E RID: 782
//        public enum CauseCode
//        {
//            // Token: 0x040011AC RID: 4524
//            Unaviable,
//            // Token: 0x040011AD RID: 4525
//            BigMiniMutuallyExclusive,
//            // Token: 0x040011AE RID: 4526
//            LogoutOfLastUser,
//            // Token: 0x040011AF RID: 4527
//            LogoutOfInitialUser,
//            // Token: 0x040011B0 RID: 4528
//            LogoutOfDrmOwner,
//            // Token: 0x040011B1 RID: 4529
//            InstantAppSuspending,
//            // Token: 0x040011B2 RID: 4530
//            KillApp,
//            // Token: 0x040011B3 RID: 4531
//            HWVideoDecoderArbitration,
//            // Token: 0x040011B4 RID: 4532
//            VideooutNotSupported,
//            // Token: 0x040011B5 RID: 4533
//            ParamsfoSuspendInBgState,
//            // Token: 0x040011B6 RID: 4534
//            SuspendRequestFromDebugger,
//            // Token: 0x040011B7 RID: 4535
//            ResumeRequestFromDebugger = 256
//        }

//        // Token: 0x0200030F RID: 783
//        public enum CoredumpReason
//        {
//            // Token: 0x040011B9 RID: 4537
//            None,
//            // Token: 0x040011BA RID: 4538
//            AppCrash,
//            // Token: 0x040011BB RID: 4539
//            CoredumpReqestFromDebugger
//        }

//        // Token: 0x02000310 RID: 784
//        [StructLayout(LayoutKind.Sequential, Pack = 4)]
//        public struct Event
//        {
//            // Token: 0x06001D44 RID: 7492 RVA: 0x000957E4 File Offset: 0x000939E4
//            public Event(EventMonitor.EventType eventType = EventMonitor.EventType.Invalid, string titleId = "", int appId = -1, EventMonitor.AppType appType = EventMonitor.AppType.Unknown, EventMonitor.AppAttr appAttr = EventMonitor.AppAttr.None, int launchRequestAppId = -1, int userId = -1, byte[] args = null, int argsSize = 0, int errorCode = -1, int errorData = 0, int appLocalPid = -1, string path = "", int causeCode = 0, int existingAppId = -1, ulong checkFlag = 0UL, int coredumpReason = 0)
//            {
//                this.eventType = eventType;
//                this.titleId = titleId;
//                this.appId = appId;
//                this.appType = appType;
//                this.appAttr = appAttr;
//                this.launchRequestAppId = launchRequestAppId;
//                this.userId = userId;
//                this.args = args;
//                this.argsSize = argsSize;
//                this.errorCode = errorCode;
//                this.errorData = errorData;
//                this.appLocalPid = appLocalPid;
//                this.path = path;
//                this.causeCode = causeCode;
//                this.existingAppId = existingAppId;
//                this.checkFlag = checkFlag;
//                this.coredumpReason = coredumpReason;
//            }

//            // Token: 0x040011BC RID: 4540
//            public EventMonitor.EventType eventType;

//            // Token: 0x040011BD RID: 4541
//            [MarshalAs(23, SizeConst = 16)]
//            public string titleId;

//            // Token: 0x040011BE RID: 4542
//            public int appId;

//            // Token: 0x040011BF RID: 4543
//            public EventMonitor.AppType appType;

//            // Token: 0x040011C0 RID: 4544
//            public EventMonitor.AppAttr appAttr;

//            // Token: 0x040011C1 RID: 4545
//            public int launchRequestAppId;

//            // Token: 0x040011C2 RID: 4546
//            public int userId;

//            // Token: 0x040011C3 RID: 4547
//            [MarshalAs(30, SizeConst = 4096)]
//            public byte[] args;

//            // Token: 0x040011C4 RID: 4548
//            public int argsSize;

//            // Token: 0x040011C5 RID: 4549
//            public int errorCode;

//            // Token: 0x040011C6 RID: 4550
//            public int errorData;

//            // Token: 0x040011C7 RID: 4551
//            public int appLocalPid;

//            // Token: 0x040011C8 RID: 4552
//            [MarshalAs(23, SizeConst = 1024)]
//            public string path;

//            // Token: 0x040011C9 RID: 4553
//            public int causeCode;

//            // Token: 0x040011CA RID: 4554
//            public int existingAppId;

//            // Token: 0x040011CB RID: 4555
//            public ulong checkFlag;

//            // Token: 0x040011CC RID: 4556
//            public int coredumpReason;
//        }
//    }
//}
