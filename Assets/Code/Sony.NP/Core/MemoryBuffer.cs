using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Sony
{
    namespace NP
    {
        [StructLayout(LayoutKind.Sequential)]
        struct NpMemoryBuffer
        {
            public UInt32 size;
            public IntPtr data;
        };

        internal class MemoryBuffer
        {
            #region Integrity Markers
            // Must match the BufferIntegrityChecks enum in native MemoryBufferManaged.h
            public enum BufferIntegrityChecks
            {
                BufferBegin = 0,        // The start marker in the buffer
                BufferEnd,              // The end marker of the buffer

                // Core
                OnlineUserBegin,
                OnlineUserEnd,

                NpOnlineIdBegin,
                NpOnlineIdEnd,

                SceNpIdBegin,
                SceNpIdEnd,

                NpCountryCodeBegin,
                NpCountryCodeEnd,

                NpTitleIdBegin,
                NpTitleIdEnd,

                NpLanguageCodeBegin,
                NpLanguageCodeEnd,

                PNGBegin,
                PNGEnd,

                // Friends
                FriendsBegin,
                FriendsEnd,

                FriendBegin,
                FriendEnd,

                FriendsOfFriendsBegin,
                FriendsOfFriendsEnd,

                BlockedUsersBegin,
                BlockedUsersEnd,

                // Profile
                ProfileBegin,
                ProfileEnd,

                RealNameBegin,
                RealNameEnd,

                // Presence
                PresenceBegin,
                PresenceEnd,

                PlatformPresenceBegin,
                PlatformPresenceEnd,

                // User Profiles
                NpProfilesBegin,
                NpProfilesEnd,

                // Network Utils
                BandwidthInfoBegin,
                BandwidthInfoEnd,

                NetStateBasicBegin,
                NetStateBasicEnd,

                NetStateDetailedBegin,
                NetStateDetailedEnd,

                // Trophies
                UnlockedTrophiesBegin,
                UnlockedTrophiesEnd,

                TrophyPackSummaryBegin,
                TrophyPackSummaryEnd,

                TrophyPackGroupBegin,
                TrophyPackGroupEnd,

                TrophyPackTrophyBegin,
                TrophyPackTrophyEnd,

                // Ranking
                TempRankBegin,
                TempRankEnd,

                RangeOfRanksBegin,
                RangeOfRanksEnd,

                FriendsRanksBegin,
                FriendsRanksEnd,

                UsersRanksBegin,
                UsersRanksEnd,

                SetGameDataBegin,
                SetGameDataEnd,

                GetGameDataBegin,
                GetGameDataEnd,

                // Matching
                WorldsBegin,
                WorldsEnd,

                CreateRoomBegin,
                CreateRoomEnd,

                RoomBegin,
                RoomEnd,

                RoomsBegin,
                RoomsEnd,

                RoomPingTimeBegin,
                RoomPingTimeEnd,

                GetDataBegin,
                GetDataEnd,

                // Tss	
                TssDataBegin,
                TssDataEnd,

                // Tus	
                TusVariablesBegin,
                TusVariablesEnd,

                TusAtomicAddToAndGetVariableBegin,
                TusAtomicAddToAndGetVariableEnd,

                TusDataBegin,
                TusDataEnd,

                // Messaging
                GameDataMessagesBegin,
                GameDataMessagesEnd,

                GameDataMessageThumbnailBegin,
                GameDataMessageThumbnailEnd,

                GameDataMessageAttachmentBegin,
                GameDataMessageAttachmentEnd,

                GameDataMessageBegin,
                GameDataMessageEnd,

                GameDataMessageDetailsBegin,
                GameDataMessageDetailsEnd,

                // Commerce
                CategoriesBegin,
                CategoriesEnd,

                CategoryBegin,
                CategoryEnd,

                SubCategoryBegin,
                SubCategoryEnd,

                ProductsBegin,
                ProductsEnd,

                ProductBegin,
                ProductEnd,

                ProductDetailsBegin,
                ProductDetailsEnd,

                SkuInfoBegin,
                SkuInfoEnd,

                ServiceEntitlementsBegin,
                ServiceEntitlementsEnd,

                ServiceEntitlementBegin,
                ServiceEntitlementEnd,

                // Auth
                AuthCodeBegin,
                AuthCodeEnd,

                IdTokenBegin,
                IdTokenEnd,

                // WordFilter

                WordFilterBegin,
                WordFilterEnd,

                // Notifications
                FriendListUpdateBegin,
                FriendListUpdateEnd,

                BlocklistUpdateBegin,
                BlocklistUpdateEnd,

                PresenceUpdateBegin,
                PresenceUpdateEnd,

                UserStateChangeBegin,
                UserStateChangeEnd,

                NetStateChangeBegin,
                NetStateChangeEnd,

                RefreshRoomBegin,
                RefreshRoomEnd,

                InvitationReceivedBegin,
                InvitationReceivedEnd,

                NewRoomMessageBegin,
                NewRoomMessageEnd,

                NewInGameMessageBegin,
                NewInGameMessageEnd,

                NewGameDataMessageBegin,
                NewGameDataMessageEnd,

                // Custom Notifications
                SessionInvitationEventBegin,
                SessionInvitationEventEnd,

                PlayTogetherHostEventBegin,
                PlayTogetherHostEventEnd,

                GameCustomDataEventBegin,
                GameCustomDataEventEnd,

                // Custom Requests
                CheckPlusBegin,
                CheckPlusEnd,

                GetParentalControlInfoBegin,
                GetParentalControlInfoEnd,
            };
            #endregion

            NpMemoryBuffer rawBuffer;

            IntPtr pos;

            public MemoryBuffer(NpMemoryBuffer pointer)
            {
                rawBuffer.data = pointer.data;
                rawBuffer.size = pointer.size;

                pos = rawBuffer.data;

                //Console.WriteLine("Buffer Ptr = " + rawBuffer.data.ToString());
            }

            public void CheckStartMarker()
            {
                CheckMarker(BufferIntegrityChecks.BufferBegin);
            }

            public void CheckEndMarker()
            {
                CheckMarker(BufferIntegrityChecks.BufferEnd);
            }

            public void CheckMarker(BufferIntegrityChecks value)
            {
                // Should be 255,254 and 253 as sanity check
                byte v1 = Marshal.ReadByte(pos);
                pos = new IntPtr(pos.ToInt64() + 1);

                byte v2 = Marshal.ReadByte(pos);
                pos = new IntPtr(pos.ToInt64() + 1);

                byte v3 = Marshal.ReadByte(pos);
                pos = new IntPtr(pos.ToInt64() + 1);

                byte marker = Marshal.ReadByte(pos);
                pos = new IntPtr(pos.ToInt64() + 1);

                if (v1 == 255 && v2 == 254 && v3 == 253 &&
                    (BufferIntegrityChecks)marker == value)
                {
                    return;
                }

                throw new NpToolkitException("MemoryBuffer - CheckMarker error - Expecting " + value.ToString());
            }

            public void CheckBufferOverflow(string method)
            {
                Int64 bytesRead = pos.ToInt64() - rawBuffer.data.ToInt64();
                if ((UInt32)bytesRead > rawBuffer.size)
                {
                    throw new NpToolkitException("MemoryBuffer - Overflow error detected. (" + method + ") (" + bytesRead + "," + rawBuffer.size + ")");
                }
            }

            public bool ReadBool()
            {
                CheckBufferOverflow("ReadBool");

                byte v = Marshal.ReadByte(pos);
                pos = new IntPtr(pos.ToInt64() + 1);

                if (v == 0) return false;
                return true;
            }

            public sbyte ReadInt8()
            {
                CheckBufferOverflow("ReadInt8");

                sbyte v = (sbyte)Marshal.ReadByte(pos);
                pos = new IntPtr(pos.ToInt64() + 1);
                return v;
            }

            public byte ReadUInt8()
            {
                CheckBufferOverflow("ReadUInt8");

                byte v = Marshal.ReadByte(pos);
                pos = new IntPtr(pos.ToInt64() + 1);
                return v;
            }

            public Int16 ReadInt16()
            {
                CheckBufferOverflow("ReadInt16");

                Int16 v = Marshal.ReadInt16(pos);
                pos = new IntPtr(pos.ToInt64() + 2);
                return v;
            }

            public UInt16 ReadUInt16()
            {
                CheckBufferOverflow("ReadUInt16");

                UInt16 v = (UInt16)Marshal.ReadInt16(pos);
                pos = new IntPtr(pos.ToInt64() + 2);
                return v;
            }

            public Int32 ReadInt32()
            {
                CheckBufferOverflow("ReadInt32");

                Int32 v = Marshal.ReadInt32(pos);
                pos = new IntPtr(pos.ToInt64() + 4);
                return v;
            }

            public UInt32 ReadUInt32()
            {
                CheckBufferOverflow("ReadUInt32");

                UInt32 v = (UInt32)Marshal.ReadInt32(pos);
                pos = new IntPtr(pos.ToInt64() + 4);
                return v;
            }

            public Int64 ReadInt64()
            {
                CheckBufferOverflow("ReadInt64");

                Int64 v = Marshal.ReadInt64(pos);
                pos = new IntPtr(pos.ToInt64() + 8);
                return v;
            }

            public UInt64 ReadUInt64()
            {
                CheckBufferOverflow("ReadUInt64");

                UInt64 v = (UInt64)Marshal.ReadInt64(pos);
                pos = new IntPtr(pos.ToInt64() + 8);
                return v;
            }

            public IntPtr ReadPtr()
            {
                CheckBufferOverflow("ReadPtr");

                Int64 v = (Int64)Marshal.ReadInt64(pos);
                pos = new IntPtr(pos.ToInt64() + 8);

                IntPtr result = new IntPtr(v);
                return result;
            }

            public double ReadDouble()
            {
                CheckBufferOverflow("ReadDouble");

                // Rather annoyingly the only way to marshal a double is to read it as an array.
                double[] destination = new double[1];

                Marshal.Copy(pos, destination, 0, 1);
                pos = new IntPtr(pos.ToInt64() + 8);

                return destination[0];
            }

            public UInt32 ReadData(ref byte[] data)
            {
                CheckBufferOverflow("ReadData");

                UInt32 size = ReadUInt32();

                if (size == 0) return 0;

                if (data == null || data.Length != size)
                {
                    data = new byte[size];
                }

                Marshal.Copy(pos, data, 0, (int)size);

                pos = new IntPtr(pos.ToInt64() + size);

                return size;
            }

            public UInt32 ReadData(ref byte[] data, UInt32 startIndex)
            {
                CheckBufferOverflow("ReadData");

                UInt32 size = ReadUInt32();

                if (size == 0) return 0;

                // If the data array is null or too small to hold the new data
                // expand the size of the array.
                // If necessary copy the contents of the old array into the new one.
                if (data == null || (startIndex + size) > data.Length)
                {
                    byte[] newData = new byte[size];

                    if (data != null)
                    {
                        Array.Copy(data, newData, startIndex);
                    }

                    data = newData;
                }

                Marshal.Copy(pos, data, (int)startIndex, (int)size);

                pos = new IntPtr(pos.ToInt64() + size);

                return size;
            }

            public void ReadString(ref string str)
            {
                CheckBufferOverflow("ReadString");

                byte[] data = null;
                UInt32 size = ReadData(ref data);

                if (size == 0)
                {
                    str = "";
                    return;
                }

                str = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);
            }

            public override string ToString()
            {
                Int64 bytesRead = pos.ToInt64() - rawBuffer.data.ToInt64();
                Int64 data = rawBuffer.data.ToInt64();

                return "Memorry buffer : Data = (" + data.ToString("X") + ") Size = (" + rawBuffer.size + ") Read = (" + bytesRead + ")";
            }
        }
    }
}
