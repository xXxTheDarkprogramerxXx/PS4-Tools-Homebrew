using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.Wrapper
{
    //since i gave up doing patches via universal this should do in the interm 
    //You will need to launch ps4debug for this
    public class Patcher
    {
        //ps4 debug holder
        private static PS4DBG ps4 = null;

        public static void Patch(int FW, string IpAddress)
        {
            try
            {
                try
                {
                    //setup ps4 debug
                    ps4 = new PS4DBG(IpAddress);
                    ps4.Connect();
                }
                catch (Exception ex1)
                {
                    Wrapper.Util.ShowMessageDialog("Error Connecting to ps4debug\n\n" + ex1.Message);

                    return;
                }
                if (!ps4.IsConnected)
                {
                    Wrapper.Util.ShowMessageDialog("Can't connect to ps4debug");

                    return;
                }

                var l = ps4.GetProcessList();
                var s = l.FindProcess("SceShellCore");
                var m = ps4.GetProcessMaps(s.pid);
                //Assets.Code.Wrapper.Util.SendMessageToPS4("Maps Found");
                var ex = m.FindEntry("executable");

                //else we begin
                switch (FW)
                {
                    case 505:
                        {
                            //SHELLCORE PATCHES
                            ps4.WriteMemory(s.pid, ex.start + 0xD42843, (byte)0x00); // 'sce_sdmemory' patch
                            ps4.WriteMemory(s.pid, ex.start + 0x7E4DC0, new byte[] { 0x48, 0x31, 0xC0, 0xC3 }); //verify keystone patch
                            ps4.WriteMemory(s.pid, ex.start + 0x68BA0, new byte[] { 0x31, 0xC0, 0xC3 }); //transfer mount permission patch eg mount foreign saves with write permission
                            ps4.WriteMemory(s.pid, ex.start + 0xC54F0, new byte[] { 0x31, 0xC0, 0xC3 });//patch psn check to load saves saves foreign to current account
                            ps4.WriteMemory(s.pid, ex.start + 0x6A349, new byte[] { 0x90, 0x90 }); // ^
                            ps4.WriteMemory(s.pid, ex.start + 0x686AE, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }); // something something patches... 
                            ps4.WriteMemory(s.pid, ex.start + 0x67FCA, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }); // don't even remember doing this
                            ps4.WriteMemory(s.pid, ex.start + 0x67798, new byte[] { 0x90, 0x90 }); //nevah jump
                            ps4.WriteMemory(s.pid, ex.start + 0x679D5, new byte[] { 0x90, 0xE9 }); //always jump
                        }
                        break;
                    case 65534://dont ask me why this is happening 
                    case 672:
                        {
                            ps4.WriteMemory(s.pid, ex.start + 0x01600060, (byte)0x00); // 'sce_sdmemory' patch
                            ps4.WriteMemory(s.pid, ex.start + 0x0087F840, new byte[] { 0x48, 0x31, 0xC0, 0xC3 }); //verify keystone patch
                            ps4.WriteMemory(s.pid, ex.start + 0x00071130, new byte[] { 0x31, 0xC0, 0xC3 }); //transfer mount permission patch eg mount foreign saves with write permission
                            ps4.WriteMemory(s.pid, ex.start + 0x000D6830, new byte[] { 0x31, 0xC0, 0xC3 });//patch psn check to load saves saves foreign to current account
                            ps4.WriteMemory(s.pid, ex.start + 0x0007379E, new byte[] { 0x90, 0x90 }); // ^
                            ps4.WriteMemory(s.pid, ex.start + 0x00070C38, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }); // something something patches... 
                            ps4.WriteMemory(s.pid, ex.start + 0x00070855, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }); // don't even remember doing this
                            ps4.WriteMemory(s.pid, ex.start + 0x00070054, new byte[] { 0x90, 0x90 }); //nevah jump
                            ps4.WriteMemory(s.pid, ex.start + 0x00070260, new byte[] { 0x90, 0xE9 }); //always jump
                        }
                        break;
                    case 702:
                        {
                            ps4.WriteMemory(s.pid, ex.start + 0x0130C060, (byte)0x00); // 'sce_sdmemory' patch
                            ps4.WriteMemory(s.pid, ex.start + 0x0083F4E0, new byte[] { 0x48, 0x31, 0xC0, 0xC3 }); //verify keystone patch
                            ps4.WriteMemory(s.pid, ex.start + 0x0006D580, new byte[] { 0x31, 0xC0, 0xC3 }); //transfer mount permission patch eg mount foreign saves with write permission
                            ps4.WriteMemory(s.pid, ex.start + 0x000CFAA0, new byte[] { 0x31, 0xC0, 0xC3 });//patch psn check to load saves saves foreign to current account
                            ps4.WriteMemory(s.pid, ex.start + 0x0006FF5F, new byte[] { 0x90, 0x90 }); // ^
                            ps4.WriteMemory(s.pid, ex.start + 0x0006D058, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }); // something something patches... 
                            ps4.WriteMemory(s.pid, ex.start + 0x0006C971, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }); // don't even remember doing this
                            ps4.WriteMemory(s.pid, ex.start + 0x0006C1A4, new byte[] { 0x90, 0x90 }); //nevah jump
                            ps4.WriteMemory(s.pid, ex.start + 0x0006C40C, new byte[] { 0x90, 0xE9 }); //always jump
                        }
                        break;
                    case 750:
                        {
                            //SHELLCORE PATCHES (SceShellCore)
                            ps4.WriteMemory(s.pid, ex.start + 0x01334060, (byte)0x00); // 'sce_sdmemory' patch
                            ps4.WriteMemory(s.pid, ex.start + 0x0084A300, new byte[] { 0x48, 0x31, 0xC0, 0xC3 }); //verify keystone patch
                            ps4.WriteMemory(s.pid, ex.start + 0x0006B860, new byte[] { 0x31, 0xC0, 0xC3 }); //transfer mount permission patch eg mount foreign saves with write permission
                            ps4.WriteMemory(s.pid, ex.start + 0x000C7280, new byte[] { 0x31, 0xC0, 0xC3 });//patch psn check to load saves saves foreign to current account
                            ps4.WriteMemory(s.pid, ex.start + 0x0006D26D, new byte[] { 0x90, 0x90 }); // ^
                            ps4.WriteMemory(s.pid, ex.start + 0x0006B338, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }); // something something patches... 
                            ps4.WriteMemory(s.pid, ex.start + 0x0006AC2D, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }); // don't even remember doing this
                            ps4.WriteMemory(s.pid, ex.start + 0x0006A494, new byte[] { 0x90, 0x90 }); //nevah jump
                            ps4.WriteMemory(s.pid, ex.start + 0x0006A6F0, new byte[] { 0x90, 0xE9 }); //always jump
                        }
                        break;
                    case 751:
                    case 755:
                        {
                            ps4.WriteMemory(s.pid, ex.start + 0x01334060, (byte)0x00); // 'sce_sdmemory' patch
                            ps4.WriteMemory(s.pid, ex.start + 0x0084A300, new byte[] { 0x48, 0x31, 0xC0, 0xC3 }); //verify keystone patch
                            ps4.WriteMemory(s.pid, ex.start + 0x0006B860, new byte[] { 0x31, 0xC0, 0xC3 }); //transfer mount permission patch eg mount foreign saves with write permission
                            ps4.WriteMemory(s.pid, ex.start + 0x000C7280, new byte[] { 0x31, 0xC0, 0xC3 });//patch psn check to load saves saves foreign to current account
                            ps4.WriteMemory(s.pid, ex.start + 0x0006D26D, new byte[] { 0x90, 0x90 }); // ^
                            ps4.WriteMemory(s.pid, ex.start + 0x0006B338, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }); // something something patches... 
                            ps4.WriteMemory(s.pid, ex.start + 0x0006AC2D, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }); // don't even remember doing this
                            ps4.WriteMemory(s.pid, ex.start + 0x0006A494, new byte[] { 0x90, 0x90 }); //nevah jump
                            ps4.WriteMemory(s.pid, ex.start + 0x0006A6F0, new byte[] { 0x90, 0xE9 }); //always jump
                        }
                        break;
                    case 900:
                        {
                            //credits to https://twitter.com/gNVjpMrfuN

                            ps4.WriteMemory(s.pid, ex.start + 0xE351D9, (byte)0x00); // 'sce_sdmemory' patch
                            ps4.WriteMemory(s.pid, ex.start + 0x08AEAE0, new byte[] { 0x48, 0x31, 0xC0, 0xC3 }); //verify keystone patch
                            ps4.WriteMemory(s.pid, ex.start + 0x06C560, new byte[] { 0x31, 0xC0, 0xC3 }); //transfer mount permission patch eg mount foreign saves with write permission
                            ps4.WriteMemory(s.pid, ex.start + 0x0C9000, new byte[] { 0x31, 0xC0, 0xC3 });//patch psn check to load saves saves foreign to current account
                            ps4.WriteMemory(s.pid, ex.start + 0x06DEFE, new byte[] { 0x90, 0x90 }); // ^
                            ps4.WriteMemory(s.pid, ex.start + 0x06C0A8, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }); // something something patches... 
                            ps4.WriteMemory(s.pid, ex.start + 0x06BA62, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }); // don't even remember doing this
                            ps4.WriteMemory(s.pid, ex.start + 0x06B2C4, new byte[] { 0x90, 0x90 }); //nevah jump
                            ps4.WriteMemory(s.pid, ex.start + 0x06B51E, new byte[] { 0x90, 0xE9 }); //always jump

                            //thanks to https://twitter.com/_ctn123
                            ps4.WriteMemory(s.pid, ex.start + 0xE35218, (byte)0x00); // 'sce_sdmemory2' patch
                            ps4.WriteMemory(s.pid, ex.start + 0xE35226, (byte)0x00); // 'sce_sdmemory3' patch
                            ps4.WriteMemory(s.pid, ex.start + 0xE35234, (byte)0x00); // 'sce_sdmemory4' patch

                        }
                        break;
                    default:
                        Wrapper.Util.ShowMessageDialog("Firmware not supported");
                        return;
                        break;

                }
            }
            catch(Exception ex)
            {
                Wrapper.Util.ShowMessageDialog("Error Connecting to ps4debug\n\n" + ex.Message +"\n\n"+ex.StackTrace);

                return;
            }
            //Wrapper.Util.ShowMessageDialog("Patched");

        }
    }
}
