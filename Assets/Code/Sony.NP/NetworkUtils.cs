using System;
using System.Runtime.InteropServices;

namespace Sony
{
	namespace NP
	{
		/// <summary>
		/// Network Utils service related functionality
		/// </summary>
		public class NetworkUtils
		{
			#region DLL Imports

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetBandwidthInfo(GetBandwidthInfoRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetBasicNetworkInfo(GetBasicNetworkInfoRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetDetailedNetworkInfo(GetDetailedNetworkInfoRequest request, out APIResult result);

            [DllImport("UnityNpToolkit2")]
            private static extern UInt64 PrxGetNetworkTime(out APIResult result);

			#endregion

			#region Requests

			/// <summary>
			/// Parameters required to measure a users bandwidth
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class GetBandwidthInfoRequest : RequestBase
			{
				/// <summary>
				/// Initializes a new instance of the <see cref="GetBandwidthInfoRequest"/> class.
				/// </summary>
				public GetBandwidthInfoRequest()
					: base(ServiceTypes.NetworkUtils, FunctionTypes.NetworkUtilsGetBandwidthInfo)
				{
				}
			}

			/// <summary>
			/// Parameters required to retrieve basic network information
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class GetBasicNetworkInfoRequest : RequestBase
			{
				/// <summary>
				/// Initializes a new instance of the <see cref="GetBasicNetworkInfoRequest"/> class.
				/// </summary>
				public GetBasicNetworkInfoRequest()
					: base(ServiceTypes.NetworkUtils, FunctionTypes.NetworkUtilsGetBasicNetworkInfo)
				{
				}
			}

			/// <summary>
			/// Parameters required to retrieve detailed network information
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class GetDetailedNetworkInfoRequest : RequestBase
			{
				/// <summary>
				/// Initializes a new instance of the <see cref="GetDetailedNetworkInfoRequest"/> class.
				/// </summary>
				public GetDetailedNetworkInfoRequest()
					: base(ServiceTypes.NetworkUtils, FunctionTypes.NetworkUtilsGetDetailedNetworkInfo)
				{
				}
			}

			#endregion

			#region Common Network Structures

			/// <summary>
			/// The connection status. 
			/// </summary>
			public enum NetworkConnectionState
			{
				/// <summary>Disconnected</summary>
				Disconnected = 0, // SCE_NET_CTL_STATE_DISCONNECTED
				/// <summary>Connecting (to cable or wireless device)</summary>
				Connecting = 1, // SCE_NET_CTL_STATE_CONNECTING
				/// <summary>Obtaining IP address</summary>
				ObtainingIP = 2, // SCE_NET_CTL_STATE_IPOBTAINING
				/// <summary>IP address obtained</summary>
				ObtainedIP = 3, // SCE_NET_CTL_STATE_IPOBTAINED
			}

			/// <summary>
			/// Results of bandwidth measurement
			/// </summary>
			public struct NpBandwidthTestResult  // Maps to SceNpBandwidthTestResult
			{
				internal double uploadBps;  // bit per second
				internal double downloadBps; // bit per second)
				internal Int32 result;

				/// <summary>
				/// Upload rate (bit per second)
				/// </summary>
				public double UploadBps { get { return uploadBps; } }

				/// <summary>
				/// Download rate (bit per second)
				/// </summary>
				public double DownloadBps { get { return downloadBps; } }

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					uploadBps = buffer.ReadDouble();
					downloadBps = buffer.ReadDouble();
					result = buffer.ReadInt32();
				}

				/// <summary>
				/// Return the upload and download bps as a string
				/// </summary>
				/// <returns>The upload/download bps string</returns>
				public override string ToString()
				{
					// The SceNpBandwidthTestResult shows 'result' should be 0, otherwise it will be a error code.
					return "Up Bps = " + uploadBps + " Down Bps = " + downloadBps; // +" Results = 0x" + result.ToString("X8");
				}
			};

			/// <summary>
			/// IPv4 address structure
			/// </summary>
			public struct NetInAddr  // Maps to SceNetInAddr
			{
				internal UInt32 addr;

				/// <summary>
				/// The IPv4 address as a 32bit value
				/// </summary>
				public UInt32 Addr 
				{ 
					get { return addr; }
					set { addr = value; } 
				}

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					addr = buffer.ReadUInt32();
				}

				/// <summary>
				/// The IPv4 address as a string
				/// </summary>
				/// <returns>IPv4 formatted address.</returns>
				public override string ToString()
				{
					// Should really convert this to 4 bytes and print out the addr in IPv4 format. Maybe there is a C# method that already does this.
					byte[] bytes = BitConverter.GetBytes(addr);

					string output = bytes[0].ToString();
					for (int i = 1; i < bytes.Length; i++)
					{
						output += "." + bytes[i].ToString();
					}

					return output;
				}
			};

			/// <summary>
			/// NAT type
			/// </summary>
			public enum RouterNatType
			{
				/// <summary>Type 1 NAT </summary>
				Type1 = 1,  // SCE_NET_CTL_NATINFO_NAT_TYPE_1
				/// <summary>Type 2 NAT </summary>
				Type2 = 2,  // SCE_NET_CTL_NATINFO_NAT_TYPE_2
				/// <summary>Type 3 NAT </summary>
				Type3 = 3  // SCE_NET_CTL_NATINFO_NAT_TYPE_3
			}

			/// <summary>
			/// STUN status.
			/// </summary>
			public enum RouterStun
			{
				/// <summary>STUN has not completed yet </summary>
				Unchecked = 0,  // SCE_NET_CTL_NATINFO_STUN_UNCHECKED
				/// <summary>STUN failed</summary>
				Failed, // SCE_NET_CTL_NATINFO_STUN_FAILED
				/// <summary>STUN succeeded</summary>
				OK // SCE_NET_CTL_NATINFO_STUN_OK
			}

			/// <summary>
			/// NAT router information structure
			/// </summary>
			public struct NatRouterInfo  // Maps to SceNetCtlNatInfo
			{
				internal RouterStun stunStatus;
				internal RouterNatType natType;
				internal NetInAddr mappedAddr;

				/// <summary>
				/// STUN status
				/// </summary>
				public RouterStun StunStatus { get { return stunStatus; } }

				/// <summary>
				/// NAT type
				/// </summary>
				public RouterNatType NatType { get { return natType; } }

				/// <summary>
				/// IP address of the PlayStation®4 device as seen from the global Internet side
				/// </summary>
				public NetInAddr MappedAddr { get { return mappedAddr; } }

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					stunStatus = (RouterStun)buffer.ReadInt32();
					natType = (RouterNatType)buffer.ReadInt32();
					mappedAddr.Read(buffer);
				}

				/// <summary>
				/// The router information structure as a string
				/// </summary>
				/// <returns>The router information structure</returns>
				public override string ToString()
				{
					return "Stun Status = " + stunStatus + " : Nat Type = " + natType + " : Mapped Addr = " + mappedAddr.ToString();
				}
			};

			/// <summary>
			/// Ethernet address structure (IPv6) as an array of bytes
			/// </summary>
			public struct NetEtherAddr  // Maps to SceNetEtherAddr
			{
				/// <summary>
				/// The number of bytes in the address. This is a 48bit-Ethernet address.
				/// </summary>
				public const int SCE_NET_ETHER_ADDR_LEN = 6;   // SCE_NET_ETHER_ADDR_LEN

				internal byte[] data;

				/// <summary>
				/// The array containing the IPv6 address
				/// </summary>
				public byte[] Data { get { return data; } }

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					data = new byte[SCE_NET_ETHER_ADDR_LEN];
					buffer.ReadData(ref data);
				}

				/// <summary>
				/// The IPv6 address as a string
				/// </summary>
				/// <returns>The IPv6 address</returns>
				public override string ToString()
				{
					if (data == null) return "0.0.0.0.0.0";

					string output = data[0].ToString();
					for (int i = 1; i < data.Length; i++)
					{
						output += "." + data[i].ToString();
					}
					return output;
				}
			};
			#endregion

			#region Get Bandwidth Info

			/// <summary>
			/// Bandwidth test result
			/// </summary>
			public class BandwidthInfoResponse : ResponseBase
			{
				internal NpBandwidthTestResult bandwidth;

				/// <summary>
				/// The results of the bandwidth test
				/// </summary>
				public NpBandwidthTestResult Bandwidth { get { return bandwidth; } }		

				/// <summary>
				/// Read the response data from the plug-in
				/// </summary>
				/// <param name="id">The request id.</param>
				/// <param name="apiCalled">The API called.</param>
				/// <param name="request">The Request instance.</param>
				protected internal override void ReadResult(UInt32 id, FunctionTypes apiCalled, RequestBase request)
				{
					base.ReadResult(id, apiCalled, request);

					APIResult result;

					MemoryBuffer readBuffer = BeginReadResponseBuffer(id, apiCalled, out result);

					if (result.RaiseException == true) throw new NpToolkitException(result);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.BandwidthInfoBegin);

					bandwidth.Read(readBuffer);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.BandwidthInfoEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// Measure a users bandwidth. The upload and download speeds are given in bits per second.
			/// </summary>
			/// <param name="request">The parameters required to retrieve a users bandwidth information</param>
			/// <param name="response">The users bandwidth results given in bits per second</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetBandwidthInfo(GetBandwidthInfoRequest request, BandwidthInfoResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetBandwidthInfo(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Get Basic Network Info

			/// <summary>
			/// Basic information about a users local network
			/// </summary>
			public class BasicNetworkInfoResponse : ResponseBase
			{
				internal string ipAddress;
				internal NatRouterInfo natInfo;
				internal NetworkConnectionState connectionStatus;

				/// <summary>
				/// The IP address of the network adapter
				/// </summary>
				public string IpAddress { get { return ipAddress; } }
	
				/// <summary>
				/// The NAT type
				/// </summary>
				public NatRouterInfo NatInfo { get { return natInfo; } }	

				/// <summary>
				/// The connection status. 
				/// </summary>
				public NetworkConnectionState ConnectionStatus { get { return connectionStatus; } }	

				/// <summary>
				/// Read the response data from the plug-in
				/// </summary>
				/// <param name="id">The request id.</param>
				/// <param name="apiCalled">The API called.</param>
				/// <param name="request">The Request object.</param>
				protected internal override void ReadResult(UInt32 id, FunctionTypes apiCalled, RequestBase request)
				{
					base.ReadResult(id, apiCalled, request);

					APIResult result;

					MemoryBuffer readBuffer = BeginReadResponseBuffer(id, apiCalled, out result);

					if (result.RaiseException == true) throw new NpToolkitException(result);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NetStateBasicBegin);

					readBuffer.ReadString(ref ipAddress);

					natInfo.Read(readBuffer);

					connectionStatus = (NetworkConnectionState)readBuffer.ReadUInt32();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NetStateBasicEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// Gets a users basic network information. This includes the users mapped IP address, NAT information and connection status.
			/// </summary>
			/// <param name="request">Parameters required to retrieve a users basic network information</param>
			/// <param name="response">The users network information upon successful completion</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetBasicNetworkInfoInfo(GetBasicNetworkInfoRequest request, BasicNetworkInfoResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetBasicNetworkInfo(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Get Detailed Network Info

			/// <summary>
			/// Device (cable or wireless)
			/// </summary>
			public enum NetworkDevice
			{
				/// <summary>Cable connection</summary>
				Wired = 0,        // SCE_NET_CTL_DEVICE_WIRED
				/// <summary>Wireless connection</summary>
				Wireless = 1,       // SCE_NET_CTL_DEVICE_WIRELESS
			}

			/// <summary>
			/// Link connection state (disconnected or connected)
			/// </summary>
			public enum NetworkLink
			{
				/// <summary>Disconnected</summary>
				Disconnected = 0,  // SCE_NET_CTL_LINK_DISCONNECTED
				/// <summary>Connected</summary>
				Connected = 1,      // SCE_NET_CTL_LINK_CONNECTED
			}

			/// <summary>
			/// Security measure for wireless connection
			/// </summary>
			public enum WfiSecurity
			{
				/// <summary>No security</summary>
				NoSecurity = 0,      // SCE_NET_CTL_WIFI_SECURITY_NOAUTH
				/// <summary>WEP</summary>
				WEP = 1,                 // SCE_NET_CTL_WIFI_SECURITY_WEP
				/// <summary>(Not used)</summary>
				WPAPSK_WPA2PSK = 2,    // SCE_NET_CTL_WIFI_SECURITY_WPAPSK_WPA2PSK
				/// <summary>WPA-PSK(TKIP)</summary>
				WPAPSK_TKIP = 3,      // SCE_NET_CTL_WIFI_SECURITY_WPAPSK_TKIP
				/// <summary>WPA-PSK(AES)</summary>
				WPAPSK_AES = 4,        // SCE_NET_CTL_WIFI_SECURITY_WPAPSK_AES
				/// <summary>WPA2-PSK(TKIP)</summary>
				WPA2PSK_TKIP = 5,    // SCE_NET_CTL_WIFI_SECURITY_WPA2PSK_TKIP
				/// <summary>WPA2-PSK(AES)</summary>
				WPA2PSK_AES = 6,      // SCE_NET_CTL_WIFI_SECURITY_WPA2PSK_AES
				/// <summary>(Not used)</summary>
				Unsupported = 7,     // SCE_NET_CTL_WIFI_SECURITY_UNSUPPORTED
			}

			/// <summary>
			/// IP setting (automatic, static, PPPoE)
			/// </summary>
			public enum NetworkIPConfig
			{
				/// <summary>Automatic (DHCP)</summary>
				DHCP = 0,  // SCE_NET_CTL_IP_DHCP
				/// <summary>Static IP</summary>
				Static = 1,      // SCE_NET_CTL_IP_STATIC
				/// <summary>PPPoE</summary>
				PPPoE = 2,      // SCE_NET_CTL_IP_PPPOE
			}

			/// <summary>
			/// Proxy server setting (do not use or use)
			/// </summary>
			public enum NetworkHTTPProxyConfig
			{
				/// <summary>HTTP proxy off</summary>
				Off = 0,  // SCE_NET_CTL_HTTP_PROXY_OFF
				/// <summary>HTTP proxy on</summary>
				On = 1,      // SCE_NET_CTL_HTTP_PROXY_ON
			}

			/// <summary>
			/// Detailed local network information. This contains all retrievable information about a users network.
			/// </summary>
			public class DetailedNetworkInfoResponse : ResponseBase
			{				
				internal NatRouterInfo natInfo;
				internal NetworkConnectionState connectionStatus; // SCE_NET_CTL_STATE_XXX		
				internal NetworkDevice device;   // SCE_NET_CTL_DEVICE_XXX			
				internal NetEtherAddr ethernetAddress;
				internal byte rssiPercentage;
				internal byte channel;					
				internal UInt32 mtu;		
				internal NetworkLink link;				   //  SCE_NET_CTL_LINK_XXX						
				internal WfiSecurity wifiSecurity;		// SCE_NET_CTL_WIFI_SECURITY_XXX		
				internal NetworkIPConfig ipConfig;					// SCE_NET_CTL_IP_XXX				
				internal NetworkHTTPProxyConfig httpProxyConfig;		// SCE_NET_CTL_HTTP_PROXY_XXX			
				internal UInt16 httpProxyPort;
				internal NetEtherAddr bssid;
				internal string ssid = "";
				internal string dhcpHostname = "";
				internal string pppoeAuthName = "";
				internal string ipAddress = "";
				internal string netmask = "";
				internal string defaultRoute = "";
				internal string primaryDNS = "";
				internal string secondaryDNS = "";
				internal string httpProxyServer = "";

				/// <summary>
				/// The NAT type
				/// </summary>
				public NatRouterInfo NatInfo { get { return natInfo; } }

				/// <summary>
				/// The connection status.
				/// </summary>
				public NetworkConnectionState ConnectionStatus { get { return connectionStatus; } }

				/// <summary>
				/// The network device being used.
				/// </summary>
				public NetworkDevice Device { get { return device; } }

				/// <summary>
				/// The MAC address       
				/// </summary>
				public NetEtherAddr EthernetAddress { get { return ethernetAddress; } }

				/// <summary>
				/// The signal strength
				/// </summary>
				public byte RssiPercentage { get { return rssiPercentage; } }

				/// <summary>
				/// The wireless channel used
				/// </summary>
				public byte Channel { get { return channel; } }

				/// <summary>
				/// MTU 		
				/// </summary>
				public UInt32 MTU { get { return mtu; } }

				/// <summary>
				/// The link connection state
				/// </summary>
				public NetworkLink Link { get { return link; } }

				/// <summary>
				/// Specifies whether wireless LAN is encrypted 	 	
				/// </summary>
				public WfiSecurity WifiSecurity { get { return wifiSecurity; } }

				/// <summary>
				/// Specifies how the IP address is configured
				/// </summary>
				public NetworkIPConfig IpConfig { get { return ipConfig; } }

				/// <summary>
				/// The configuration of the proxy server
				/// </summary>
				public NetworkHTTPProxyConfig HttpProxyConfig { get { return httpProxyConfig; } }

				/// <summary>
				/// The proxy server port address
				/// </summary>
				public UInt16 HttpProxyPort { get { return httpProxyPort; } }

				/// <summary>
				/// BSSID
				/// </summary>
				public NetEtherAddr BSSID { get { return bssid; } }

				/// <summary>
				/// SSID
				/// </summary>
				public string SSID { get { return ssid; } }

				/// <summary>
				/// The DHCP hostname
				/// </summary>
				public string DhcpHostname { get { return dhcpHostname; } }

				/// <summary>
				/// The PPPOE authentication name
				/// </summary>
				public string PPPoeAuthName { get { return pppoeAuthName; } }

				/// <summary>
				/// The devices IP address
				/// </summary>
				public string IpAddress { get { return ipAddress; } }

				/// <summary>
				/// The devices Net mask
				/// </summary>
				public string Netmask { get { return netmask; } }

				/// <summary>
				/// The default route IP address
				/// </summary>
				public string DefaultRoute { get { return defaultRoute; } }

				/// <summary>
				/// The primary domain name server IP address
				/// </summary>
				public string PrimaryDNS { get { return primaryDNS; } }

				/// <summary>
				/// The secondary domain name server IP address
				/// </summary>
				public string SecondaryDNS { get { return secondaryDNS; } }

				/// <summary>
				/// The IP address of the proxy
				/// </summary>
				public string HttpProxyServer { get { return httpProxyServer; } }

				/// <summary>
				/// Read the response data from the plug-in
				/// </summary>
				/// <param name="id">The request id.</param>
				/// <param name="apiCalled">The API called.</param>
				/// <param name="request">The Request object.</param>
				protected internal override void ReadResult(UInt32 id, FunctionTypes apiCalled, RequestBase request)
				{
					base.ReadResult(id, apiCalled, request);

					APIResult result;

					MemoryBuffer readBuffer = BeginReadResponseBuffer(id, apiCalled, out result);

					if (result.RaiseException == true) throw new NpToolkitException(result);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NetStateDetailedBegin);

					natInfo.Read(readBuffer);
					connectionStatus = (NetworkConnectionState)readBuffer.ReadUInt32();
					device = (NetworkDevice)readBuffer.ReadUInt32();
					ethernetAddress.Read(readBuffer);

					rssiPercentage = readBuffer.ReadUInt8();
					channel = readBuffer.ReadUInt8();

					mtu = readBuffer.ReadUInt32();
					link = (NetworkLink)readBuffer.ReadUInt32();
					wifiSecurity = (WfiSecurity)readBuffer.ReadUInt32();
					ipConfig = (NetworkIPConfig)readBuffer.ReadUInt32();
					httpProxyConfig = (NetworkHTTPProxyConfig)readBuffer.ReadUInt32();
					httpProxyPort = readBuffer.ReadUInt16();

					bssid.Read(readBuffer);

					readBuffer.ReadString(ref ssid);
					readBuffer.ReadString(ref dhcpHostname);
					readBuffer.ReadString(ref pppoeAuthName);
					readBuffer.ReadString(ref ipAddress);
					readBuffer.ReadString(ref netmask);
					readBuffer.ReadString(ref defaultRoute);
					readBuffer.ReadString(ref primaryDNS);
					readBuffer.ReadString(ref secondaryDNS);
					readBuffer.ReadString(ref httpProxyServer);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NetStateDetailedEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// Get detailed network information. This includes all obtainable information about a users local network. Most of
			/// this information is only useful during development and would not normally be used in the released application.
			/// </summary>
			/// <param name="request">Parameters required to retrieve a users detailed network information</param>
			/// <param name="response">Detailed information about a users local network</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetDetailedNetworkInfo(GetDetailedNetworkInfoRequest request, DetailedNetworkInfoResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetDetailedNetworkInfo(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

            #region Get Network Time

            /// <summary>
            /// Get network time (UTC) time
            /// </summary>
            /// <returns></returns>
            public static DateTime GetNetworkTime()
			{
				APIResult result;

                UInt64 rtcTicks = PrxGetNetworkTime(out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

                // rtcTicks are in microseconds and .net uses 100 nanosecond units
                DateTime dt = new DateTime((Int64)(rtcTicks*10), DateTimeKind.Utc);

                return dt;
			}

            #endregion

            #region Notifications

            /// <summary>
			/// Represents a network event
			/// </summary>
			public enum NetworkEvent
			{
				/// <summary>Event not set</summary>
				none,
				/// <summary>A network has been connected</summary>
				networkConnected,
				/// <summary>The network has been disconnected</summary>
				networkDisconnected		
			};

			/// <summary>
			/// Notification that is sent to the NP Toolkit callback in the event of a network state change. The notification will be
			/// sent when a network has been connected or disconnected.
			/// </summary>
			public class NetStateChangeResponse : ResponseBase
			{		
				internal NetworkEvent netEvent;

				/// <summary>
				/// The network event that occurred
				/// </summary>
				public NetworkEvent NetEvent { get { return netEvent; } }

				/// <summary>
				/// Read the response data from the plug-in
				/// </summary>
				/// <param name="id">The request id.</param>
				/// <param name="apiCalled">The API called.</param>
				/// <param name="request">The Request object.</param>
				protected internal override void ReadResult(UInt32 id, FunctionTypes apiCalled, RequestBase request)
				{
					base.ReadResult(id, apiCalled, request);

					APIResult result;

					MemoryBuffer readBuffer = BeginReadResponseBuffer(id, apiCalled, out result);

					if (result.RaiseException == true) throw new NpToolkitException(result);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NetStateChangeBegin);

					netEvent = (NetworkEvent)readBuffer.ReadInt32();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NetStateChangeEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			#endregion
		}
	}
}
