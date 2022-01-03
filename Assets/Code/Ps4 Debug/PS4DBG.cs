using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace Assets.Code
{
    public partial class PS4DBG
    {

        
        private Socket sock = null;
        private IPEndPoint enp = null;

        public bool IsConnected { get; private set; }

        public bool IsDebugging { get; private set; }

        private Thread debugThread = null;

        // some global values
        private const string LIBRARY_VERSION = "1.2";
        private const int PS4DBG_PORT = 744;
        private const int PS4DBG_DEBUG_PORT = 755;
        private const int NET_MAX_LENGTH = 8192;

        private const int BROADCAST_PORT = 1010;
        private const uint BROADCAST_MAGIC = 0xFFFFAAAA;

        // from protocol.h
        // each packet starts with the magic
        // each C# base type can translate into a packet field
        // some packets, such as write take an additional data whose length will be specified in the cmd packet data field structure specific to that cmd type
        // ushort - 2 bytes | uint - 4 bytes | ulong - 8 bytes
        private const uint CMD_PACKET_MAGIC = 0xFFAABBCC;

        // from debug.h
        //struct debug_breakpoint {
        //    uint32_t valid;
        //    uint64_t address;
        //    uint8_t original;
        //};
        public static uint MAX_BREAKPOINTS = 10;
        public static uint MAX_WATCHPOINTS = 4;

        //  struct cmd_packet {
        //    uint32_t magic;
        //    uint32_t cmd;
        //    uint32_t datalen;
        //    // (field not actually part of packet, comes after)
        //    uint8_t* data;
        //  }
        //  __attribute__((packed));
        //  #define CMD_PACKET_SIZE 12
        private const int CMD_PACKET_SIZE = 12;
        public enum CMDS : uint
        {
            CMD_VERSION = 0xBD000001,

            CMD_PROC_LIST = 0xBDAA0001,
            CMD_PROC_READ = 0xBDAA0002,
            CMD_PROC_WRITE = 0xBDAA0003,
            CMD_PROC_MAPS = 0xBDAA0004,
            CMD_PROC_INTALL = 0xBDAA0005,
            CMD_PROC_CALL = 0xBDAA0006,
            CMD_PROC_ELF = 0xBDAA0007,
            CMD_PROC_PROTECT = 0xBDAA0008,
            CMD_PROC_SCAN = 0xBDAA0009,
            CMD_PROC_INFO = 0xBDAA000A,
            CMD_PROC_ALLOC = 0xBDAA000B,
            CMD_PROC_FREE = 0xBDAA000C,

            CMD_DEBUG_ATTACH = 0xBDBB0001,
            CMD_DEBUG_DETACH = 0xBDBB0002,
            CMD_DEBUG_BREAKPT = 0xBDBB0003,
            CMD_DEBUG_WATCHPT = 0xBDBB0004,
            CMD_DEBUG_THREADS = 0xBDBB0005,
            CMD_DEBUG_STOPTHR = 0xBDBB0006,
            CMD_DEBUG_RESUMETHR = 0xBDBB0007,
            CMD_DEBUG_GETREGS = 0xBDBB0008,
            CMD_DEBUG_SETREGS = 0xBDBB0009,
            CMD_DEBUG_GETFPREGS = 0xBDBB000A,
            CMD_DEBUG_SETFPREGS = 0xBDBB000B,
            CMD_DEBUG_GETDBGREGS = 0xBDBB000C,
            CMD_DEBUG_SETDBGREGS = 0xBDBB000D,
            CMD_DEBUG_STOPGO = 0xBDBB0010,
            CMD_DEBUG_THRINFO = 0xBDBB0011,
            CMD_DEBUG_SINGLESTEP = 0xBDBB0012,

            CMD_KERN_BASE = 0xBDCC0001,
            CMD_KERN_READ = 0xBDCC0002,
            CMD_KERN_WRITE = 0xBDCC0003,

            CMD_CONSOLE_REBOOT = 0xBDDD0001,
            CMD_CONSOLE_END = 0xBDDD0002,
            CMD_CONSOLE_PRINT = 0xBDDD0003,
            CMD_CONSOLE_NOTIFY = 0xBDDD0004,
            CMD_CONSOLE_INFO = 0xBDDD0005,
        };

        public enum CMD_STATUS : uint
        {
            CMD_SUCCESS = 0x80000000,
            CMD_ERROR = 0xF0000001,
            CMD_TOO_MUCH_DATA = 0xF0000002,
            CMD_DATA_NULL = 0xF0000003,
            CMD_ALREADY_DEBUG = 0xF0000004,
            CMD_INVALID_INDEX = 0xF0000005
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct CMDPacket
        {
            public uint magic;
            public uint cmd;
            public uint datalen;
        }

        // enums
        public enum VM_PROTECTIONS : uint
        {
            VM_PROT_NONE = 0x00,
            VM_PROT_READ = 0x01,
            VM_PROT_WRITE = 0x02,
            VM_PROT_EXECUTE = 0x04,
            VM_PROT_DEFAULT = (VM_PROT_READ | VM_PROT_WRITE),
            VM_PROT_ALL = (VM_PROT_READ | VM_PROT_WRITE | VM_PROT_EXECUTE),
            VM_PROT_NO_CHANGE = 0x08,
            VM_PROT_COPY = 0x10,
            VM_PROT_WANTS_COPY = 0x10
        };
        public enum WATCHPT_LENGTH : uint
        {
            DBREG_DR7_LEN_1 = 0x00,	/* 1 byte length */
            DBREG_DR7_LEN_2 = 0x01,
            DBREG_DR7_LEN_4 = 0x03,
            DBREG_DR7_LEN_8 = 0x02,
        };
        public enum WATCHPT_BREAKTYPE : uint
        {
            DBREG_DR7_EXEC = 0x00,	/* break on execute       */
            DBREG_DR7_WRONLY = 0x01,	/* break on write         */
            DBREG_DR7_RDWR = 0x03,	/* break on read or write */
        };

        // General helper functions, make code cleaner
        public static string ConvertASCII(byte[] data, int offset)
        {
            int length = Array.IndexOf<byte>(data, 0, offset) - offset;
            if (length < 0)
            {
                length = data.Length - offset;
            }

            return Encoding.ASCII.GetString(data, offset, length);
        }
        public static byte[] SubArray(byte[] data, int offset, int length)
        {
            byte[] bytes = new byte[length];
            Buffer.BlockCopy(data, offset, bytes, 0, length);
            return bytes;
        }
        public static object GetObjectFromBytes(byte[] buffer, Type type)
        {
            int size = Marshal.SizeOf(type);

            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(buffer, 0, ptr, size);
            object r = Marshal.PtrToStructure(ptr, type);

            Marshal.FreeHGlobal(ptr);

            return r;
        }
        public static byte[] GetBytesFromObject(object obj)
        {
            int size = Marshal.SizeOf(obj);

            byte[] bytes = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(obj, ptr, false);
            Marshal.Copy(ptr, bytes, 0, size);

            Marshal.FreeHGlobal(ptr);

            return bytes;
        }

        // General networking functions
        private static IPAddress GetBroadcastAddress(IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }

            return new IPAddress(broadcastAddress);
        }
        private void SendCMDPacket(CMDS cmd, int length, params object[] fields)
        {
            CMDPacket packet = new CMDPacket
            {
                magic = CMD_PACKET_MAGIC,
                cmd = (uint)cmd,
                datalen = (uint)length
            };

            byte[] data = null;

            if (length > 0)
            {
                MemoryStream rs = new MemoryStream();
                foreach (object field in fields)
                {
                    byte[] bytes = null;

                    var type = field.GetType();

                    if(type == typeof(char))
                    {
                        char c = (char)field;
                        bytes = BitConverter.GetBytes(c);
                    }
                    if(type == typeof(byte))
                    {
                        byte b = (byte)field;
                        bytes = BitConverter.GetBytes(b);
                    }
                    if(type == typeof(short))
                    {
                        short s = (short)field;
                        bytes = BitConverter.GetBytes(s);
                    }
                    if (type == typeof(ushort))
                    {
                        ushort us = (ushort)field;
                        bytes = BitConverter.GetBytes(us);
                    }
                    if (type == typeof(int))
                    {
                        int i = (int)field;
                        bytes = BitConverter.GetBytes(i);
                    }
                    if (type == typeof(uint))
                    {
                        uint u = (uint)field;
                        bytes = BitConverter.GetBytes(u);
                    }
                    if (type == typeof(long))
                    {
                        long l = (long)field;
                        bytes = BitConverter.GetBytes(l);
                    }
                    if (type == typeof(ulong))
                    {
                        ulong ul = (ulong)field;
                        bytes = BitConverter.GetBytes(ul);
                    }
                    if (type == typeof(byte[]))
                    {
                        byte[] ba = (byte[])field;
                        bytes = ba;
                    }
                    if (bytes != null) rs.Write(bytes, 0, bytes.Length);
                }

                data = rs.ToArray();
                rs.Dispose();
            }

            SendData(GetBytesFromObject(packet), CMD_PACKET_SIZE);

            if (data != null)
            {
                SendData(data, length);
            }
            else
            {
                //Assets.Code.Wrapper.Util.SendMessageToPS4("Data is null");
            }
        }
        private void SendData(byte[] data, int length)
        {
            int left = length;
            int offset = 0;
            int sent = 0;

            while (left > 0)
            {
                if (left > NET_MAX_LENGTH)
                {
                    byte[] bytes = SubArray(data, offset, NET_MAX_LENGTH);
                    sent = sock.Send(bytes, NET_MAX_LENGTH, SocketFlags.None);
                }
                else
                {
                    byte[] bytes = SubArray(data, offset, left);
                    sent = sock.Send(bytes, left, SocketFlags.None);
                }

                offset += sent;
                left -= sent;
            }
        }
        private byte[] ReceiveData(int length)
        {
            MemoryStream s = new MemoryStream();

            int left = length;
            int recv = 0;
            while (left > 0)
            {
                byte[] b = new byte[NET_MAX_LENGTH];
                recv = sock.Receive(b, NET_MAX_LENGTH, SocketFlags.None);
                s.Write(b, 0, recv);
                left -= recv;
            }

            byte[] data = s.ToArray();

            s.Dispose();
            GC.Collect();

            return data;
        }
        private CMD_STATUS ReceiveStatus()
        {
            byte[] status = new byte[4];
            sock.Receive(status, 4, SocketFlags.None);
            return (CMD_STATUS)BitConverter.ToUInt32(status, 0);
        }
        private void CheckStatus()
        {
            CMD_STATUS status = ReceiveStatus();
            if (status != CMD_STATUS.CMD_SUCCESS)
            {
                throw new Exception("libdbg status " + ((uint)status).ToString("X"));
            }
        }

        private void CheckConnected()
        {
            if (!IsConnected)
            {
                throw new Exception("libdbg: not connected");
            }
        }
        private void CheckDebugging()
        {
            if (!IsDebugging)
            {
                throw new Exception("libdbg: not debugging");
            }
        }



        /// <summary>
        /// Initializes PS4DBG class
        /// </summary>
        /// <param name="addr">PlayStation 4 address</param>
        public PS4DBG(IPAddress addr)
        {
            IsConnected = false;
            IsDebugging = false;
            enp = new IPEndPoint(addr, PS4DBG_PORT);
            sock = new Socket(enp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Initializes PS4DBG class
        /// </summary>
        /// <param name="ip">PlayStation 4 ip address</param>
        public PS4DBG(string ip)
        {

            IsConnected = false;
            IsDebugging = false;
            IPAddress addr = null;
            try
            {
                addr = IPAddress.Parse(ip);
            }
            catch (FormatException ex)
            {
                throw ex;
            }

            enp = new IPEndPoint(addr, PS4DBG_PORT);
            sock = new Socket(enp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Find the playstation ip
        /// </summary>
        public static string FindPlayStation()
        {
            UdpClient uc = new UdpClient();
            IPEndPoint server = new IPEndPoint(IPAddress.Any, 0);
            uc.EnableBroadcast = true;
            uc.Client.ReceiveTimeout = 4000;

            byte[] magic = BitConverter.GetBytes(BROADCAST_MAGIC);

            IPAddress addr = null;
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    addr = ip;
                }
            }

            if (addr == null)
            {
                throw new Exception("libdbg broadcast error: could not get host ip");
            }

            uc.Send(magic, magic.Length, new IPEndPoint(GetBroadcastAddress(addr, IPAddress.Parse("255.255.255.0")), BROADCAST_PORT));

            byte[] resp = uc.Receive(ref server);
            if (BitConverter.ToUInt32(resp, 0) != BROADCAST_MAGIC)
            {
                throw new Exception("libdbg broadcast error: wrong magic on udp server");
            }

            return server.Address.ToString();
        }

        /// <summary>
        /// Connects to PlayStation 4
        /// </summary>
        public void Connect()
        {
            if (!IsConnected)
            {
                sock.NoDelay = true;
                sock.ReceiveBufferSize = NET_MAX_LENGTH;
                sock.SendBufferSize = NET_MAX_LENGTH;

                sock.ReceiveTimeout = 1000 * 10;

                sock.Connect(enp);
                IsConnected = true;
            }
        }

        /// <summary>
        /// Disconnects from PlayStation 4
        /// </summary>
        public void Disconnect()
        {
            SendCMDPacket(CMDS.CMD_CONSOLE_END, 0);
            sock.Shutdown(SocketShutdown.Both);
            sock.Close();
            IsConnected = false;
        }

        /// <summary>
        /// Get current ps4debug version from library
        /// </summary>
        public string GetLibraryDebugVersion()
        {
            return LIBRARY_VERSION;
        }

        /// <summary>
        /// Get the current ps4debug version from console
        /// </summary>
        public string GetConsoleDebugVersion()
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_VERSION, 0);

            byte[] ldata = new byte[4];
            sock.Receive(ldata, 4, SocketFlags.None);

            int length = BitConverter.ToInt32(ldata, 0);

            byte[] data = new byte[length];
            sock.Receive(data, length, SocketFlags.None);

            return ConvertASCII(data, 0);
        }
    }

    public partial class PS4DBG
    {
        //debug
        // packet sizes
        //send size
        private const int CMD_DEBUG_ATTACH_PACKET_SIZE = 4;
        private const int CMD_DEBUG_BREAKPT_PACKET_SIZE = 16;
        private const int CMD_DEBUG_WATCHPT_PACKET_SIZE = 24;
        private const int CMD_DEBUG_STOPTHR_PACKET_SIZE = 4;
        private const int CMD_DEBUG_RESUMETHR_PACKET_SIZE = 4;
        private const int CMD_DEBUG_GETREGS_PACKET_SIZE = 4;
        private const int CMD_DEBUG_SETREGS_PACKET_SIZE = 8;
        private const int CMD_DEBUG_STOPGO_PACKET_SIZE = 4;
        private const int CMD_DEBUG_THRINFO_PACKET_SIZE = 4;
        //receive size
        private const int DEBUG_INTERRUPT_SIZE = 0x4A0;
        private const int DEBUG_THRINFO_SIZE = 40;
        private const int DEBUG_REGS_SIZE = 0xB0;
        private const int DEBUG_FPREGS_SIZE = 0x340;
        private const int DEBUG_DBGREGS_SIZE = 0x80;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct DebuggerInterruptPacket
        {
            public uint lwpid;
            public uint status;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
            public string tdname;
            public regs reg64;
            public fpregs1 savefpu;
            public dbregs dbreg64;
        }
        /// <summary>
        /// Debugger interrupt callback
        /// </summary>
        /// <param name="lwpid">Thread identifier</param>
        /// <param name="status">status</param>
        /// <param name="tdname">Thread name</param>
        /// <param name="regs">Registers</param>
        /// <param name="fpregs">Floating point registers</param>
        /// <param name="dbregs">Debug registers</param>
        public delegate void DebuggerInterruptCallback(uint lwpid, uint status, string tdname, regs regs, fpregs1 fpregs, dbregs dbregs);
        private void DebuggerThread(object obj)
        {
            DebuggerInterruptCallback callback = (DebuggerInterruptCallback)obj;

            IPAddress ip = IPAddress.Parse("0.0.0.0");
            IPEndPoint endpoint = new IPEndPoint(ip, PS4DBG_DEBUG_PORT);

            Socket server = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            server.Bind(endpoint);
            server.Listen(0);

            IsDebugging = true;

            Socket cl = server.Accept();

            cl.NoDelay = true;
            cl.Blocking = false;

            while (IsDebugging)
            {
                if (cl.Available == DEBUG_INTERRUPT_SIZE)
                {
                    byte[] data = new byte[DEBUG_INTERRUPT_SIZE];
                    int bytes = cl.Receive(data, DEBUG_INTERRUPT_SIZE, SocketFlags.None);
                    if (bytes == DEBUG_INTERRUPT_SIZE)
                    {
                        DebuggerInterruptPacket packet = (DebuggerInterruptPacket)GetObjectFromBytes(data, typeof(DebuggerInterruptPacket));
                        callback(packet.lwpid, packet.status, packet.tdname, packet.reg64, packet.savefpu, packet.dbreg64);
                    }
                }

                Thread.Sleep(100);
            }

            server.Close();
        }
        /// <summary>
        /// Attach the debugger
        /// </summary>
        /// <param name="pid">Process ID</param>
        /// <param name="callback">DebuggerInterruptCallback implementation</param>
        /// <returns></returns>
        public void AttachDebugger(int pid, DebuggerInterruptCallback callback)
        {
            CheckConnected();

            if (IsDebugging || debugThread != null)
            {
                throw new Exception("libdbg: debugger already running?");
            }

            IsDebugging = false;

            debugThread = new Thread(DebuggerThread) { IsBackground = true };
            debugThread.Start(callback);

            // wait until server is started
            while (!IsDebugging)
            {
                Thread.Sleep(100);
            }

            SendCMDPacket(CMDS.CMD_DEBUG_ATTACH, CMD_DEBUG_ATTACH_PACKET_SIZE, pid);
            CheckStatus();
        }

        /// <summary>
        /// Detach the debugger
        /// </summary>
        /// <returns></returns>
        public void DetachDebugger()
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_DEBUG_DETACH, 0);
            CheckStatus();

            if (IsDebugging && debugThread != null)
            {
                IsDebugging = false;

                debugThread.Join();
                debugThread = null;
            }
        }

        /// <summary>
        /// Stop the current process
        /// </summary>
        /// <returns></returns>
        public void ProcessStop()
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_STOPGO, CMD_DEBUG_STOPGO_PACKET_SIZE, 1);
            CheckStatus();
        }

        /// <summary>
        /// Kill the current process, it will detach before doing so
        /// </summary>
        /// <returns></returns>
        public void ProcessKill()
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_STOPGO, CMD_DEBUG_STOPGO_PACKET_SIZE, 2);
            CheckStatus();
        }

        /// <summary>
        /// Resume the current process
        /// </summary>
        /// <returns></returns>
        public void ProcessResume()
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_STOPGO, CMD_DEBUG_STOPGO_PACKET_SIZE, 0);
            CheckStatus();
        }

        /// <summary>
        /// Change breakpoint, to remove said breakpoint send the same index but disable it (address is ignored)
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="enabled">Enabled</param>
        /// <param name="address">Address</param>
        /// <returns></returns>
        public void ChangeBreakpoint(int index, bool enabled, ulong address)
        {
            CheckConnected();
            CheckDebugging();

            if (index >= MAX_BREAKPOINTS)
            {
                throw new Exception("libdbg: breakpoint index out of range");
            }

            SendCMDPacket(CMDS.CMD_DEBUG_BREAKPT, CMD_DEBUG_BREAKPT_PACKET_SIZE, index, Convert.ToInt32(enabled), address);
            CheckStatus();
        }

        /// <summary>
        /// Change watchpoint
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="enabled">Enabled</param>
        /// <param name="length">Length</param>
        /// <param name="breaktype">Break type</param>
        /// <param name="address">Address</param>
        /// <returns></returns>
        public void ChangeWatchpoint(int index, bool enabled, WATCHPT_LENGTH length, WATCHPT_BREAKTYPE breaktype, ulong address)
        {
            CheckConnected();
            CheckDebugging();

            if (index >= MAX_WATCHPOINTS)
            {
                throw new Exception("libdbg: watchpoint index out of range");
            }

            SendCMDPacket(CMDS.CMD_DEBUG_WATCHPT, CMD_DEBUG_WATCHPT_PACKET_SIZE, index, Convert.ToInt32(enabled), (uint)length, (uint)breaktype, address);
            CheckStatus();
        }

        /// <summary>
        /// Get a list of threads from the current process
        /// </summary>
        /// <returns></returns>
        public uint[] GetThreadList()
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_THREADS, 0);
            CheckStatus();

            byte[] data = new byte[sizeof(int)];
            sock.Receive(data, sizeof(int), SocketFlags.None);
            int number = BitConverter.ToInt32(data, 0);

            byte[] threads = ReceiveData(number * sizeof(uint));
            uint[] thrlist = new uint[number];
            for (int i = 0; i < number; i++)
            {
                thrlist[i] = BitConverter.ToUInt32(threads, i * sizeof(uint));
            }

            return thrlist;
        }

        /// <summary>
        /// Get thread information
        /// </summary>
        /// <returns></returns>
        /// <param name="lwpid">Thread identifier</param>
        public ThreadInfo GetThreadInfo(uint lwpid)
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_THRINFO, CMD_DEBUG_THRINFO_PACKET_SIZE, lwpid);
            CheckStatus();

            return (ThreadInfo)GetObjectFromBytes(ReceiveData(DEBUG_THRINFO_SIZE), typeof(ThreadInfo));
        }

        /// <summary>
        /// Stop a thread from running
        /// </summary>
        /// <param name="lwpid">Thread id</param>
        /// <returns></returns>
        public void StopThread(uint lwpid)
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_STOPTHR, CMD_DEBUG_STOPTHR_PACKET_SIZE, lwpid);
            CheckStatus();
        }

        /// <summary>
        /// Resume a thread from being stopped
        /// </summary>
        /// <param name="lwpid">Thread id</param>
        /// <returns></returns>
        public void ResumeThread(uint lwpid)
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_RESUMETHR, CMD_DEBUG_RESUMETHR_PACKET_SIZE, lwpid);
            CheckStatus();
        }

        /// <summary>
        /// Get registers from thread
        /// </summary>
        /// <param name="lwpid">Thread id</param>
        /// <returns></returns>
        public regs GetRegisters(uint lwpid)
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_GETREGS, CMD_DEBUG_GETREGS_PACKET_SIZE, lwpid);
            CheckStatus();

            return (regs)GetObjectFromBytes(ReceiveData(DEBUG_REGS_SIZE), typeof(regs));
        }

        /// <summary>
        /// Set thread registers
        /// </summary>
        /// <param name="lwpid">Thread id</param>
        /// <param name="regs">Register data</param>
        /// <returns></returns>
        public void SetRegisters(uint lwpid, regs regs)
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_SETREGS, CMD_DEBUG_SETREGS_PACKET_SIZE, lwpid, DEBUG_REGS_SIZE);
            CheckStatus();
            SendData(GetBytesFromObject(regs), DEBUG_REGS_SIZE);
            CheckStatus();
        }

        /// <summary>
        /// Get floating point registers from thread
        /// </summary>
        /// <param name="lwpid">Thread id</param>
        /// <returns></returns>
        public fpregs1 GetFloatRegisters(uint lwpid)
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_GETFPREGS, CMD_DEBUG_GETREGS_PACKET_SIZE, lwpid);
            CheckStatus();

            return (fpregs1)GetObjectFromBytes(ReceiveData(DEBUG_FPREGS_SIZE), typeof(fpregs1));
        }

        /// <summary>
        /// Set floating point thread registers
        /// </summary>
        /// <param name="lwpid">Thread id</param>
        /// <param name="fpregs">Floating point register data</param>
        /// <returns></returns>
        public void SetFloatRegisters(uint lwpid, fpregs1 fpregs)
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_SETFPREGS, CMD_DEBUG_SETREGS_PACKET_SIZE, lwpid, DEBUG_FPREGS_SIZE);
            CheckStatus();
            SendData(GetBytesFromObject(fpregs), DEBUG_FPREGS_SIZE);
            CheckStatus();
        }

        /// <summary>
        /// Get debug registers from thread
        /// </summary>
        /// <param name="lwpid">Thread id</param>
        /// <returns></returns>
        public dbregs GetDebugRegisters(uint lwpid)
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_GETDBGREGS, CMD_DEBUG_GETREGS_PACKET_SIZE, lwpid);
            CheckStatus();

            return (dbregs)GetObjectFromBytes(ReceiveData(DEBUG_DBGREGS_SIZE), typeof(dbregs));
        }

        /// <summary>
        /// Set debug thread registers
        /// </summary>
        /// <param name="lwpid">Thread id</param>
        /// <param name="dbregs">debug register data</param>
        /// <returns></returns>
        public void SetDebugRegisters(uint lwpid, dbregs dbregs)
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_SETDBGREGS, CMD_DEBUG_SETREGS_PACKET_SIZE, lwpid, DEBUG_DBGREGS_SIZE);
            CheckStatus();
            SendData(GetBytesFromObject(dbregs), DEBUG_DBGREGS_SIZE);
            CheckStatus();
        }

        /// <summary>
        /// Executes a single instruction
        /// </summary>
        public void SingleStep()
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_SINGLESTEP, 0);
            CheckStatus();
        }
    }

    public partial class PS4DBG
    {
        //console
        // packet sizes
        // send size
        private const int CMD_CONSOLE_PRINT_PACKET_SIZE = 4;
        private const int CMD_CONSOLE_NOTIFY_PACKET_SIZE = 8;


        // console
        // note: the disconnect command actually uses the console api to end the connection
        /// <summary>
        /// Reboot console
        /// </summary>
        public void Reboot()
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_CONSOLE_REBOOT, 0);
            IsConnected = false;
        }

        /// <summary>
        /// Print to serial port
        /// </summary>
        public void Print(string str)
        {
            CheckConnected();

            string raw = str + "\0";

            SendCMDPacket(CMDS.CMD_CONSOLE_PRINT, CMD_CONSOLE_PRINT_PACKET_SIZE, raw.Length);
            SendData(Encoding.ASCII.GetBytes(raw), raw.Length);
            CheckStatus();
        }

        /// <summary>
        /// Notify console
        /// </summary>
        public void Notify(int messageType, string message)
        {
            CheckConnected();

            string raw = message + "\0";

            SendCMDPacket(CMDS.CMD_CONSOLE_NOTIFY, CMD_CONSOLE_NOTIFY_PACKET_SIZE, messageType, raw.Length);
            SendData(Encoding.ASCII.GetBytes(raw), raw.Length);
            CheckStatus();
        }

        /// <summary>
        /// Console information
        /// </summary>
        public void GetConsoleInformation()
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_CONSOLE_INFO, 0);
            CheckStatus();

            // TODO return the data
        }
    }

    public partial class PS4DBG
    {
        // kernel
        //packet sizes
        //send size
        private const int CMD_KERN_READ_PACKET_SIZE = 12;
        private const int CMD_KERN_WRITE_PACKET_SIZE = 12;
        //receive size
        private const int KERN_BASE_SIZE = 8;


        /// <summary>
        /// Get kernel base address
        /// </summary>
        /// <returns></returns>
        public ulong KernelBase()
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_KERN_BASE, 0);
            CheckStatus();
            return BitConverter.ToUInt64(ReceiveData(KERN_BASE_SIZE), 0);
        }

        /// <summary>
        /// Read memory from kernel
        /// </summary>
        /// <param name="address">Memory address</param>
        /// <param name="length">Data length</param>
        /// <returns></returns>
        public byte[] KernelReadMemory(ulong address, int length)
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_KERN_READ, CMD_KERN_READ_PACKET_SIZE, address, length);
            CheckStatus();
            return ReceiveData(length);
        }

        /// <summary>
        /// Write memory in kernel
        /// </summary>
        /// <param name="address">Memory address</param>
        /// <param name="data">Data</param>
        public void KernelWriteMemory(ulong address, byte[] data)
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_KERN_WRITE, CMD_KERN_WRITE_PACKET_SIZE, address, data.Length);
            CheckStatus();
            SendData(data, data.Length);
            CheckStatus();
        }
    }

    public partial class PS4DBG
    {
        //proc
        // packet sizes
        // send size
        private const int CMD_PROC_READ_PACKET_SIZE = 16;
        private const int CMD_PROC_WRITE_PACKET_SIZE = 16;
        private const int CMD_PROC_MAPS_PACKET_SIZE = 4;
        private const int CMD_PROC_INSTALL_PACKET_SIZE = 4;
        private const int CMD_PROC_CALL_PACKET_SIZE = 68;
        private const int CMD_PROC_ELF_PACKET_SIZE = 8;
        private const int CMD_PROC_PROTECT_PACKET_SIZE = 20;
        private const int CMD_PROC_SCAN_PACKET_SIZE = 10;
        private const int CMD_PROC_INFO_PACKET_SIZE = 4;
        private const int CMD_PROC_ALLOC_PACKET_SIZE = 8;
        private const int CMD_PROC_FREE_PACKET_SIZE = 16;
        // receive size
        private const int PROC_LIST_ENTRY_SIZE = 36;
        private const int PROC_MAP_ENTRY_SIZE = 58;
        private const int PROC_INSTALL_SIZE = 8;
        private const int PROC_CALL_SIZE = 12;
        private const int PROC_PROC_INFO_SIZE = 188;
        private const int PROC_ALLOC_SIZE = 8;


        /// <summary>
        /// Get current process list
        /// </summary>
        /// <returns></returns>
        public ProcessList GetProcessList()
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_PROC_LIST, 0);
            CheckStatus();

            // recv count
            byte[] bytes = new byte[4];
            sock.Receive(bytes, 4, SocketFlags.None);
            int number = BitConverter.ToInt32(bytes, 0);

            // recv data
            byte[] data = ReceiveData(number * PROC_LIST_ENTRY_SIZE);

            // parse data
            string[] names = new string[number];
            int[] pids = new int[number];
            for (int i = 0; i < number; i++)
            {
                int offset = i * PROC_LIST_ENTRY_SIZE;
                names[i] = ConvertASCII(data, offset);
                pids[i] = BitConverter.ToInt32(data, offset + 32);
            }

            return new ProcessList(number, names, pids);
        }

        /// <summary>
        /// Read memory
        /// </summary>
        /// <param name="pid">Process ID</param>
        /// <param name="address">Memory address</param>
        /// <param name="length">Data length</param>
        /// <returns></returns>
        public byte[] ReadMemory(int pid, ulong address, int length)
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_PROC_READ, CMD_PROC_READ_PACKET_SIZE, pid, address, length);
            CheckStatus();
            return ReceiveData(length);
        }

        /// <summary>
        /// Write memory
        /// </summary>
        /// <param name="pid">Process ID</param>
        /// <param name="address">Memory address</param>
        /// <param name="data">Data</param>
        public void WriteMemory(int pid, ulong address, byte[] data)
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_PROC_WRITE, CMD_PROC_WRITE_PACKET_SIZE, pid, address, data.Length);
            CheckStatus();
            SendData(data, data.Length);
            CheckStatus();
        }

        /// <summary>
        /// Get process memory maps
        /// </summary>
        /// <param name="pid">Process ID</param>
        /// <returns></returns>
        public ProcessMap GetProcessMaps(int pid)
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_PROC_MAPS, CMD_PROC_MAPS_PACKET_SIZE, pid);
            CheckStatus();

            // recv count
            byte[] bnumber = new byte[4];
            sock.Receive(bnumber, 4, SocketFlags.None);
            int number = BitConverter.ToInt32(bnumber, 0);

            // recv data
            byte[] data = ReceiveData(number * PROC_MAP_ENTRY_SIZE);

            // parse data
            MemoryEntry[] entries = new MemoryEntry[number];
            for (int i = 0; i < number; i++)
            {
                int offset = i * PROC_MAP_ENTRY_SIZE;
                entries[i] = new MemoryEntry
                {
                    name = ConvertASCII(data, offset),
                    start = BitConverter.ToUInt64(data, offset + 32),
                    end = BitConverter.ToUInt64(data, offset + 40),
                    offset = BitConverter.ToUInt64(data, offset + 48),
                    prot = BitConverter.ToUInt16(data, offset + 56)
                };

            }

            return new ProcessMap(pid, entries);
        }

        /// <summary>
        /// Install RPC into a process, this returns a stub address that you should pass into call functions
        /// </summary>
        /// <param name="pid">Process ID</param>
        /// <returns></returns>
        public ulong InstallRPC(int pid)
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_PROC_INTALL, CMD_PROC_INSTALL_PACKET_SIZE, pid);
            CheckStatus();

            return BitConverter.ToUInt64(ReceiveData(PROC_INSTALL_SIZE), 0);
        }

        /// <summary>
        /// Call function (returns rax)
        /// </summary>
        /// <param name="pid">Process ID</param>
        /// <param name="rpcstub">Stub address from InstallRPC</param>
        /// <param name="address">Address to call</param>
        /// <param name="args">Arguments array</param>
        /// <returns></returns>
        public ulong Call(int pid, ulong rpcstub, ulong address, params object[] args)
        {
            CheckConnected();

            // need to do this in a custom format
            CMDPacket packet = new CMDPacket
            {
                magic = CMD_PACKET_MAGIC,
                cmd = (uint)CMDS.CMD_PROC_CALL,
                datalen = (uint)CMD_PROC_CALL_PACKET_SIZE
            };
            SendData(GetBytesFromObject(packet), CMD_PACKET_SIZE);

            MemoryStream rs = new MemoryStream();
            rs.Write(BitConverter.GetBytes(pid), 0, sizeof(int));
            rs.Write(BitConverter.GetBytes(rpcstub), 0, sizeof(ulong));
            rs.Write(BitConverter.GetBytes(address), 0, sizeof(ulong));

            int num = 0;
            foreach (object arg in args)
            {
                byte[] bytes = new byte[8];

                switch (arg.GetType().ToString())
                {
                    case "char":
                        {
                            char c = (char)arg;
                            byte[] tmp = BitConverter.GetBytes(c);
                            Buffer.BlockCopy(tmp, 0, bytes, 0, sizeof(char));

                            byte[] pad = new byte[sizeof(ulong) - sizeof(char)];
                            Buffer.BlockCopy(pad, 0, bytes, sizeof(char), pad.Length);
                            break;
                        }
                    case "byte":
                        {
                            byte b = (byte)arg;
                            byte[] tmp = BitConverter.GetBytes(b);
                            Buffer.BlockCopy(tmp, 0, bytes, 0, sizeof(byte));

                            byte[] pad = new byte[sizeof(ulong) - sizeof(byte)];
                            Buffer.BlockCopy(pad, 0, bytes, sizeof(byte), pad.Length);
                            break;
                        }
                    case "short":
                        {
                            short s = (short)arg;
                            byte[] tmp = BitConverter.GetBytes(s);
                            Buffer.BlockCopy(tmp, 0, bytes, 0, sizeof(short));

                            byte[] pad = new byte[sizeof(ulong) - sizeof(short)];
                            Buffer.BlockCopy(pad, 0, bytes, sizeof(short), pad.Length);
                            break;
                        }
                    case "ushort":
                        {
                            ushort us = (ushort)arg;
                            byte[] tmp = BitConverter.GetBytes(us);
                            Buffer.BlockCopy(tmp, 0, bytes, 0, sizeof(ushort));

                            byte[] pad = new byte[sizeof(ulong) - sizeof(ushort)];
                            Buffer.BlockCopy(pad, 0, bytes, sizeof(ushort), pad.Length);
                            break;
                        }
                    case "int":
                        {
                            int i = (int)arg;
                            byte[] tmp = BitConverter.GetBytes(i);
                            Buffer.BlockCopy(tmp, 0, bytes, 0, sizeof(int));

                            byte[] pad = new byte[sizeof(ulong) - sizeof(int)];
                            Buffer.BlockCopy(pad, 0, bytes, sizeof(int), pad.Length);
                            break;
                        }
                    case "uint":
                        {
                            uint ui = (uint)arg;
                            byte[] tmp = BitConverter.GetBytes(ui);
                            Buffer.BlockCopy(tmp, 0, bytes, 0, sizeof(uint));

                            byte[] pad = new byte[sizeof(ulong) - sizeof(uint)];
                            Buffer.BlockCopy(pad, 0, bytes, sizeof(uint), pad.Length);
                            break;
                        }
                    case "long":
                        {
                            long l = (long)arg;
                            byte[] tmp = BitConverter.GetBytes(l);
                            Buffer.BlockCopy(tmp, 0, bytes, 0, sizeof(long));
                            break;
                        }
                    case "ulong":
                        {
                            ulong ul = (ulong)arg;
                            byte[] tmp = BitConverter.GetBytes(ul);
                            Buffer.BlockCopy(tmp, 0, bytes, 0, sizeof(ulong));
                            break;
                        }
                }

                rs.Write(bytes, 0, bytes.Length);
                num++;
            }

            if (num > 6)
            {
                throw new Exception("libdbg: too many arguments");
            }

            if (num < 6)
            {
                for (int i = 0; i < (6 - num); i++)
                {
                    rs.Write(BitConverter.GetBytes((ulong)0), 0, sizeof(ulong));
                }
            }

            SendData(rs.ToArray(), CMD_PROC_CALL_PACKET_SIZE);
            rs.Dispose();

            CheckStatus();

            byte[] data = ReceiveData(PROC_CALL_SIZE);
            return BitConverter.ToUInt64(data, 4);
        }

        /// <summary>
        /// Load elf
        /// </summary>
        /// <param name="pid">Process ID</param>
        /// <param name="elf">Elf</param>
        public void LoadElf(int pid, byte[] elf)
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_PROC_ELF, CMD_PROC_ELF_PACKET_SIZE, pid, (uint)elf.Length);
            CheckStatus();
            SendData(elf, elf.Length);
            CheckStatus();
        }

        /// <summary>
        /// Load elf
        /// </summary>
        /// <param name="pid">Process ID</param>
        /// <param name="filename">Elf filename</param>
        public void LoadElf(int pid, string filename)
        {
            LoadElf(pid, File.ReadAllBytes(filename));
        }

        public enum ScanValueType
        {
            valTypeUInt8 = 0,
            valTypeInt8,
            valTypeUInt16,
            valTypeInt16,
            valTypeUInt32,
            valTypeInt32,
            valTypeUInt64,
            valTypeInt64,
            valTypeFloat,
            valTypeDouble,
            valTypeArrBytes,
            valTypeString
        }

        public enum ScanCompareType : byte
        {
            ExactValue = 0,
            FuzzyValue,
            BiggerThan,
            SmallerThan,
            ValueBetween,
            IncreasedValue,
            IncreasedValueBy,
            DecreasedValue,
            DecreasedValueBy,
            ChangedValue,
            UnchangedValue,
            UnknownInitialValue
        }
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        public List<ulong> ScanProcess<T>(int pid, ScanCompareType compareType, T value, T extraValue)
        {
            CheckConnected();

            int typeLength = 0;
            ScanValueType valueType;
            byte[] valueBuffer, extraValueBuffer = null;

            // fill in variables
            switch (value.GetType().ToString())
            {
                case "bool":
                    {
                        bool b = Convert.ToBoolean(value);
                        valueType = ScanValueType.valTypeUInt8;
                        typeLength = 1;
                        valueBuffer = BitConverter.GetBytes(b);
                        if (extraValue != null)
                            extraValueBuffer = BitConverter.GetBytes((bool)(object)extraValue);
                    }
                    break;
                case "sbyte":
                    sbyte sb = Convert.ToSByte(value);
                    valueType = ScanValueType.valTypeInt8;
                    valueBuffer = BitConverter.GetBytes(sb);
                    typeLength = 1;
                    if (extraValue != null)
                        extraValueBuffer = BitConverter.GetBytes((sbyte)(object)extraValue);
                    break;
                case "byte":
                    {
                        byte b = Convert.ToByte(value);
                        valueType = ScanValueType.valTypeUInt8;
                        valueBuffer = BitConverter.GetBytes(b);
                        typeLength = 1;
                        if (extraValue != null)
                            extraValueBuffer = BitConverter.GetBytes((byte)(object)extraValue);
                    }
                    break;
                case "short":
                    {
                        short s = Convert.ToInt16(value);
                        valueType = ScanValueType.valTypeInt16;
                        valueBuffer = BitConverter.GetBytes(s);
                        typeLength = 2;
                        if (extraValue != null)
                            extraValueBuffer = BitConverter.GetBytes((short)(object)extraValue);
                    }
                    break;
                case "ushort":
                    ushort us = Convert.ToUInt16(value);
                    valueType = ScanValueType.valTypeUInt16;
                    valueBuffer = BitConverter.GetBytes(us);
                    typeLength = 2;
                    if (extraValue != null)
                        extraValueBuffer = BitConverter.GetBytes((ushort)(object)extraValue);
                    break;
                case "int":
                    int i = Convert.ToInt32(value);
                    valueType = ScanValueType.valTypeInt32;
                    valueBuffer = BitConverter.GetBytes(i);
                    typeLength = 4;
                    if (extraValue != null)
                        extraValueBuffer = BitConverter.GetBytes((int)(object)extraValue);
                    break;
                case "uint":
                    uint ui = Convert.ToUInt32(value);
                    valueType = ScanValueType.valTypeUInt32;
                    valueBuffer = BitConverter.GetBytes(ui);
                    typeLength = 4;
                    if (extraValue != null)
                        extraValueBuffer = BitConverter.GetBytes((uint)(object)extraValue);
                    break;
                case "long":
                    long l = Convert.ToInt64(value);
                    valueType = ScanValueType.valTypeInt64;
                    valueBuffer = BitConverter.GetBytes(l);
                    typeLength = 8;
                    if (extraValue != null)
                        extraValueBuffer = BitConverter.GetBytes((long)(object)extraValue);
                    break;
                case "ulong":
                    ulong ul = Convert.ToUInt64(value);
                    valueType = ScanValueType.valTypeUInt64;
                    valueBuffer = BitConverter.GetBytes(ul);
                    typeLength = 8;
                    if (extraValue != null)
                        extraValueBuffer = BitConverter.GetBytes((ulong)(object)extraValue);
                    break;
                case "float":
                    float f = Convert.ToSingle(value);
                    valueType = ScanValueType.valTypeFloat;
                    valueBuffer = BitConverter.GetBytes(f);
                    typeLength = 4;
                    if (extraValue != null)
                        extraValueBuffer = BitConverter.GetBytes((float)(object)extraValue);
                    break;
                case "double":
                    double d = Convert.ToDouble(value);
                    valueType = ScanValueType.valTypeDouble;
                    valueBuffer = BitConverter.GetBytes(d);
                    typeLength = 8;
                    if (extraValue != null)
                        extraValueBuffer = BitConverter.GetBytes((double)(object)extraValue);
                    break;
                case "string":
                    {
                        string s = value.ToString();
                        valueType = ScanValueType.valTypeString;
                        valueBuffer = Encoding.ASCII.GetBytes(s);
                        typeLength = valueBuffer.Length;
                    }
                    break;
                case "byte[]":
                    byte[] ba = ObjectToByteArray(value);
                    valueType = ScanValueType.valTypeArrBytes;
                    valueBuffer = ba;
                    typeLength = valueBuffer.Length;
                    break;
                default:
                    throw new NotSupportedException("Requested scan value type is not supported! (Feed in Byte[] instead.)");

            }
            // send packet
            SendCMDPacket(CMDS.CMD_PROC_SCAN, CMD_PROC_SCAN_PACKET_SIZE, pid, (byte)valueType, (byte)compareType, (int)(extraValue == null ? typeLength : typeLength * 2));
            CheckStatus();

            SendData(valueBuffer, typeLength);
            if (extraValueBuffer != null)
            {
                SendData(extraValueBuffer, typeLength);
            }

            CheckStatus();

            // receive results
            int save = sock.ReceiveTimeout;
            sock.ReceiveTimeout = Int32.MaxValue;
            List<ulong> results = new List<ulong>();
            while (true)
            {
                ulong result = BitConverter.ToUInt64(ReceiveData(sizeof(ulong)), 0);
                if (result == 0xFFFFFFFFFFFFFFFF)
                {
                    break;
                }

                results.Add(result);
            }

            sock.ReceiveTimeout = save;

            return results;
        }

        /// <summary>
        /// Changes protection on pages in range
        /// </summary>
        /// <param name="pid">Process ID</param>
        /// <param name="address">Address</param>
        /// <param name="length">Length</param>
        /// <param name="newprot">New protection</param>
        /// <returns></returns>
        public void ChangeProtection(int pid, ulong address, uint length, VM_PROTECTIONS newProt)
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_PROC_PROTECT, CMD_PROC_PROTECT_PACKET_SIZE, pid, address, length, (uint)newProt);
            CheckStatus();
        }

        /// <summary>
        /// Get process information
        /// </summary>
        /// <param name="pid">Process ID</param>
        /// <returns></returns>
        public ProcessInfo GetProcessInfo(int pid)
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_PROC_INFO, CMD_PROC_INFO_PACKET_SIZE, pid);
            CheckStatus();

            byte[] data = ReceiveData(PROC_PROC_INFO_SIZE);
            return (ProcessInfo)GetObjectFromBytes(data, typeof(ProcessInfo));
        }

        /// <summary>
        /// Allocate RWX memory in the process space
        /// </summary>
        /// <param name="pid">Process ID</param>
        /// <param name="length">Size of memory allocation</param>
        /// <returns></returns>
        public ulong AllocateMemory(int pid, int length)
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_PROC_ALLOC, CMD_PROC_ALLOC_PACKET_SIZE, pid, length);
            CheckStatus();
            return BitConverter.ToUInt64(ReceiveData(PROC_ALLOC_SIZE), 0);
        }

        /// <summary>
        /// Free memory in the process space
        /// </summary>
        /// <param name="pid">Process ID</param>
        /// <param name="address">Address of the memory allocation</param>
        /// <param name="length">Size of memory allocation</param>
        /// <returns></returns>
        public void FreeMemory(int pid, ulong address, int length)
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_PROC_FREE, CMD_PROC_FREE_PACKET_SIZE, pid, address, length);
            CheckStatus();
        }

        public T ReadMemory<T>(int pid, ulong address)
        {
            if (typeof(T) == typeof(string))
            {
                string str = "";
                ulong i = 0;

                while (true)
                {
                    byte value = ReadMemory(pid, address + i, sizeof(byte))[0];
                    if (value == 0)
                    {
                        break;
                    }
                    str += Convert.ToChar(value);
                    i++;
                }

                return (T)(object)str;
            }

            if (typeof(T) == typeof(byte[]))
            {
                throw new NotSupportedException("byte arrays are not supported, use ReadMemory(int pid, ulong address, int size)");
            }

            return (T)GetObjectFromBytes(ReadMemory(pid, address, Marshal.SizeOf(typeof(T))), typeof(T));
        }

        public void WriteMemory<T>(int pid, ulong address, T value)
        {
            if (typeof(T) == typeof(string))
            {
                WriteMemory(pid, address, Encoding.ASCII.GetBytes((string)(object)value + (char)0x0));
                return;
            }

            if (typeof(T) == typeof(byte[]))
            {
                WriteMemory(pid, address, (byte[])(object)value);
                return;
            }

            WriteMemory(pid, address, GetBytesFromObject(value));
        }
    }

    public class Process
    {
        public string name;
        public int pid;

        /// <summary>
        /// Initializes Process class
        /// </summary>
        /// <param name="name">Process name</param>
        /// <param name="pid">Process ID</param>
        /// <returns></returns>
        public Process(string name, int pid)
        {
            this.name = name;
            this.pid = pid;
        }
        public override string ToString()
        {
            return string.Format("[{0}] {1}",pid, name);
        }
    }

    public class ProcessList
    {
        public Process[] processes;

        /// <summary>
        /// Initializes ProcessList class
        /// </summary>
        /// <param name="number">Number of processes</param>
        /// <param name="names">Process names</param>
        /// <param name="pids">Process IDs</param>
        /// <returns></returns>
        public ProcessList(int number, string[] names, int[] pids)
        {
            processes = new Process[number];
            for (int i = 0; i < number; i++)
            {
                processes[i] = new Process(names[i], pids[i]);
            }
        }

        /// <summary>
        /// Finds a process based off name
        /// </summary>
        /// <param name="name">Process name</param>
        /// <param name="contains">Condition to check if process name contains name</param>
        /// <returns></returns>
        public Process FindProcess(string name, bool contains = false)
        {
            foreach (Process p in processes)
            {
                if (contains)
                {
                    if (p.name.Contains(name))
                    {
                        return p;
                    }
                }
                else
                {
                    if (p.name == name)
                    {
                        return p;
                    }
                }
            }

            return null;
        }
    }

    public class MemoryEntry
    {
        public string name;
        public ulong start;
        public ulong end;
        public ulong offset;
        public uint prot;
    }

    public class ProcessMap
    {
        public int pid;
        public MemoryEntry[] entries;

        /// <summary>
        /// Initializes ProcessMap class with memory entries and process ID
        /// </summary>
        /// <param name="pid">Process ID</param>
        /// <param name="entries">Process memory entries</param>
        /// <returns></returns>
        public ProcessMap(int pid, MemoryEntry[] entries)
        {
            this.pid = pid;
            this.entries = entries;
        }

        /// <summary>
        /// Finds a virtual memory entry based off name
        /// </summary>
        /// <param name="name">Virtual memory entry name</param>
        /// <param name="contains">Condition to check if entry name contains name</param>
        /// <returns></returns>
        public MemoryEntry FindEntry(string name, bool contains = false)
        {
            foreach (MemoryEntry entry in entries)
            {
                if (contains)
                {
                    if (entry.name.Contains(name))
                    {
                        return entry;
                    }
                }
                else
                {
                    if (entry.name == name)
                    {
                        return entry;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a virtual memory entry based off size
        /// </summary>
        /// <param name="size">Virtual memory entry size</param>
        /// <returns></returns>
        public MemoryEntry FindEntry(ulong size)
        {
            foreach (MemoryEntry entry in entries)
            {
                if ((entry.start - entry.end) == size)
                {
                    return entry;
                }
            }

            return null;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ProcessInfo
    {
        public int pid;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string name;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string path;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string titleid;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string contentid;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ThreadInfo
    {
        public int pid;
        public int priority;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string name;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct regs
    {
        public ulong r_r15;
        public ulong r_r14;
        public ulong r_r13;
        public ulong r_r12;
        public ulong r_r11;
        public ulong r_r10;
        public ulong r_r9;
        public ulong r_r8;
        public ulong r_rdi;
        public ulong r_rsi;
        public ulong r_rbp;
        public ulong r_rbx;
        public ulong r_rdx;
        public ulong r_rcx;
        public ulong r_rax;
        public uint r_trapno;
        public ushort r_fs;
        public ushort r_gs;
        public uint r_err;
        public ushort r_es;
        public ushort r_ds;
        public ulong r_rip;
        public ulong r_cs;
        public ulong r_rflags;
        public ulong r_rsp;
        public ulong r_ss;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct envxmm
    {
        public ushort en_cw; /* control word (16bits) */
        public ushort en_sw; /* status word (16bits) */
        public byte en_tw; /* tag word (8bits) */
        public byte en_zero;
        public ushort en_opcode; /* opcode last executed (11 bits ) */
        public ulong en_rip; /* floating point instruction pointer */
        public ulong en_rdp; /* floating operand pointer */
        public uint en_mxcsr; /* SSE sontorol/status register */
        public uint en_mxcsr_mask; /* valid bits in mxcsr */
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct acc
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] fp_bytes;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        private byte[] fp_pad;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct xmmacc
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] xmm_bytes;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ymmacc
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] ymm_bytes;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct xstate_hdr
    {
        public ulong xstate_bv;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        private byte[] xstate_rsrv0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        private byte[] xstate_rsrv;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct savefpu_xstate
    {
        public xstate_hdr sx_hd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public ymmacc[] sx_ymm;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct fpregs1
    {
        public envxmm svn_env;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public acc[] sv_fp;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public xmmacc[] sv_xmm;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 96)]
        private byte[] sv_pad;
        public savefpu_xstate sv_xstate;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct dbregs
    {
        public ulong dr0;
        public ulong dr1;
        public ulong dr2;
        public ulong dr3;
        public ulong dr4;
        public ulong dr5;
        public ulong dr6;
        public ulong dr7;
        public ulong dr8;
        public ulong dr9;
        public ulong dr10;
        public ulong dr11;
        public ulong dr12;
        public ulong dr13;
        public ulong dr14;
        public ulong dr15;
    }
}
