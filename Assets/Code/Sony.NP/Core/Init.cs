using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Sony
{
    namespace NP
    {
        /// <summary>
        /// Set the age restriction for a specific country
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct AgeRestriction
        {
            public string countryCode;
            public Int32 age;

            /// <summary>
            /// Set the country code to apply this age restricition. . Takes a copy of the code, see remarks for details.
            /// </summary>
            /// <remarks>
            /// Takes a copy of the country code or returns a copy.
            /// The country code must be assign explicitly. 
            /// </remarks>
            public Core.CountryCode CountryCode
            {
                get
                {
                    Core.CountryCode cc = new Core.CountryCode();
                    cc.code = countryCode;
                    return cc;
                }

                set
                {
                    countryCode = value.code;
                }
            }

            /// <summary>
            /// The minimum age restriction for the given country code
            /// </summary>
            public Int32 Age
            {
                get { return age; }
                set { age = value; }
            }

            /// <summary>
            /// Initialise an age restricition for the specified country code
            /// </summary>
            /// <param name="age">The minimum age restriction.</param>
            /// <param name="countryCode">The country code</param>
            public AgeRestriction(Int32 age, Core.CountryCode countryCode)
            {
                this.age = age;
                this.countryCode = countryCode.code;
            }

            /// <summary>
            /// Initialise with no age restricition and country code 
            /// </summary>
            public void Init()
            {
                countryCode = "";
                age = ContentRestriction.NP_NO_AGE_RESTRICTION;
            }
        }

        /// <summary>
        /// Configure the age restrictions for this title by setting a default age.
        /// Additional age restrictions for specific countries can also be set.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ContentRestriction
        {
            /// <summary>Maximum number of Age restrictions with country codes</summary>
            public const int MAX_AGE_RESTICTIONS = 32;
            /// <summary>No age restricition default value</summary>
            public const int NP_NO_AGE_RESTRICTION = 0;    // SCE_NP_NO_AGE_RESTRICTION

            public Int32 defaultAgeRestriction;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_AGE_RESTICTIONS)]
            public AgeRestriction[] ageRestrictions;  // Fixed size array so marshalling works.

            public Int32 numAgeRestictions;

            [MarshalAs(UnmanagedType.I1)]
            public bool applyContentRestriction;

            /// <summary>
            /// The default age restrictioon for the title. Use NP_NO_AGE_RESTRICTION to speicify no age restriction.
            /// </summary>
            public Int32 DefaultAgeRestriction
            {
                get { return defaultAgeRestriction; }
                set { defaultAgeRestriction = value; }
            }

            /// <summary>
            /// Set the array of age restrictions for defined countries.
            /// </summary>
            public AgeRestriction[] AgeRestrictions
            {
                get
                {
                    if (numAgeRestictions == 0) return null;

                    AgeRestriction[] output = new AgeRestriction[numAgeRestictions];

                    Array.Copy(ageRestrictions, output, (int)numAgeRestictions);

                    return output;
                }
                set
                {
                    if (value != null)
                    {
                        if (value.Length > MAX_AGE_RESTICTIONS)
                        {
                            throw new NpToolkitException("The size of the array is larger than " + MAX_AGE_RESTICTIONS);
                        }
                        value.CopyTo(ageRestrictions, 0);
                        numAgeRestictions = (Int32)value.Length;
                    }
                    else
                    {
                        numAgeRestictions = 0;
                    }
                }
            }

            /// <summary>
            /// Defaults to True. When true will set content restrictions, otherwise will not call sceNpSetContentRestriction. Declaring age restriction is a requirement for any product doing network access. Before disabling this please refer to TRC R4109 and check if your product can ignore age restrictions. Note this is not the same as setting the age restriction to 0.
            /// </summary>
            public bool ApplyContentRestriction
            {
                get { return applyContentRestriction; }
                set { applyContentRestriction = value; }
            }

            /// <summary>
            /// Initialise the title to use no default age strictions or country specific restrictions.
            /// </summary>
            public void Init()
            {
                defaultAgeRestriction = NP_NO_AGE_RESTRICTION;
                ageRestrictions = new AgeRestriction[MAX_AGE_RESTICTIONS];
                numAgeRestictions = 0;
                applyContentRestriction = true;

                for (int i = 0; i < MAX_AGE_RESTICTIONS; i++)
                {
                    ageRestrictions[i].Init();
                }
            }
        }

        /// <summary>
        /// Activate server push notifications from the PSN servers.
        /// </summary>
        [System.Obsolete("ServerPushNotifications is deprecated, please use PushNotificationsFlags instead.")]
        [StructLayout(LayoutKind.Sequential)]
        public struct ServerPushNotifications
        {
            [MarshalAs(UnmanagedType.I1)]
            public bool newGameDataMessage;

            [MarshalAs(UnmanagedType.I1)]
            public bool newInvitation;

            [MarshalAs(UnmanagedType.I1)]
            public bool updateBlockedUsersList;

            [MarshalAs(UnmanagedType.I1)]
            public bool updateFriendPresence;

            [MarshalAs(UnmanagedType.I1)]
            public bool updateFriendsList;

            [MarshalAs(UnmanagedType.I1)]
            public bool newInGameMessage;

            /// <summary>
            /// Indicates if the application wishes to receive a notification when a game data message is received
            /// </summary>
            public bool NewGameDataMessage
            {
                get { return newGameDataMessage; }
                set { newGameDataMessage = value; }
            }

            /// <summary>
            /// Indicates if the application wishes to receive a notification when an invitation is received
            /// </summary>
            public bool NewInvitation
            {
                get { return newInvitation; }
                set { newInvitation = value; }
            }

            /// <summary>
            /// Indicates if the application wishes to receive a notification when the blocked users list is modified
            /// </summary>
            public bool UpdateBlockedUsersList
            {
                get { return updateBlockedUsersList; }
                set { updateBlockedUsersList = value; }
            }

            /// <summary>
            /// Indicates if the application wishes to receive a notification when the presence information of a friend is modified
            /// </summary>
            public bool UpdateFriendPresence
            {
                get { return updateFriendPresence; }
                set { updateFriendPresence = value; }
            }

            /// <summary>
            /// Indicates if the application wishes to receive a notification when the friends list is modified
            /// </summary>
            public bool UpdateFriendsList
            {
                get { return updateFriendsList; }
                set { updateFriendsList = value; }
            }

            /// <summary>
            /// Indicates if the application wishes to receive a notification when new in-game messages are received
            /// </summary>
            /// <remarks>
            /// This is also required to use the <see cref="Messaging.SendInGameMessage"/> method, otherwise it will result in error SCE_NP_IN_GAME_MESSAGE_ERROR_LIB_CONTEXT_NOT_FOUND
            /// </remarks>
            public bool NewInGameMessage
            {
                get { return newInGameMessage; }
                set { newInGameMessage = value; }
            }

            /// <summary>
            /// Initialise so all services push notifictions are supported.
            /// </summary>
            public void Init()
            {
                newGameDataMessage = true;
                newInvitation = true;
                updateBlockedUsersList = true;
                updateFriendPresence = true;
                updateFriendsList = true;
                newInGameMessage = true;
            }
        }

        /// <summary>
        /// Flags to activate server push notifications from the PSN servers. See <see cref="InitToolkit.SetPushNotificationsFlags"/>
        /// </summary>
        public enum PushNotificationsFlags
        {
            /// <summary>Used to disable all push notifications. Note this is not a flag as is only provided for readability when calling <see cref="InitToolkit.SetPushNotificationsFlags"/> and requiring all flags to be disabled.</summary>
            None = 0,
            /// <summary>Indicates if the application wishes to receive a notification when a game data message is received</summary>
            NewGameDataMessage = (1 << 0),
            /// <summary>Indicates if the application wishes to receive a notification when an invitation is received</summary>
            NewInvitation = (1 << 1),
            /// <summary>Indicates if the application wishes to receive a notification when the blocked users list is modified</summary>
            UpdateBlockedUsersList = (1 << 2),
            /// <summary>Indicates if the application wishes to receive a notification when the presence information of a friend is modified</summary>
            UpdateFriendPresence = (1 << 3),
            /// <summary>Indicates if the application wishes to receive a notification when the friends list is modified</summary>
            UpdateFriendsList = (1 << 4),
            /// <summary>Indicates if the application wishes to receive a notification when new in-game messages are received</summary>
            /// <remarks>This is also required to use the <see cref="Messaging.SendInGameMessage"/> method, otherwise it will result in error SCE_NP_IN_GAME_MESSAGE_ERROR_LIB_CONTEXT_NOT_FOUND</remarks>
            NewInGameMessage = (1 << 5),
        }

        /// <summary>
        /// Set the affinity mask to enable NpToolkit to run on multiple cores
        /// Important - Core0 and Core1 and not provided as these are the main Update and Gfx cores and should not be used.
        /// </summary>
        public enum Affinity
        {
            /// <summary>Allow native NpToolkit plug-in to run on Core 2</summary>
            Core2 = (1 << 2),
            /// <summary>Allow native NpToolkit plug-in to run on Core 3</summary>
            Core3 = (1 << 3),
            /// <summary>Allow native NpToolkit plug-in to run on Core 4</summary>
            Core4 = (1 << 4),
            /// <summary>Allow native NpToolkit plug-in to run on Core 5</summary>
            Core5 = (1 << 5),

            /// <summary>Allow native NpToolkit plug-in to run on Core 2,3,4, and 5</summary>
            AllCores = Core2 | Core3 | Core4 | Core5,
        }

        /// <summary>
        /// Set the thread settings for the NpToolkit plug-in
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ThreadSettings
        {
            /// <summary>
            /// The affinity mask.
            /// </summary>
            public Affinity affinity;

            /// <summary>
            /// By default initialise the thread settings to use cores 2,3,4 and 5
            /// </summary>
            public void Init()
            {
                affinity = Affinity.AllCores;
            }
        }

        /// <summary>
        ///  Represents the sizes of the ToolkitNp2 library memory pools.
        /// </summary>
        /// <remarks>
        /// Represents the sizes of the ToolkitNp2 library memory pools.
        /// This class is used as a member of the <see cref="InitToolkit"/> class, and the values it contains are used to set the sizes of the ToolkitNp2 library memory pools.
        /// 
        /// The library has 9 different memory pools:
        ///   - Basic network related pools: Net, SSL, HTTP.
        ///   - WebAPI related pools: WebAPI, JSON.
        ///   - Matching related pools: SSL-NpMatching2 and NpMatching2.
        ///   - InGameMessage related pools: NpInGameMessage. 
        ///   - The public memory pool used by the library itself.
        ///
        /// All memory pools will be initialized as soon as the library starts except for the Matching pool, which will be
        /// initialized only when the Matching service starts.
        /// 
        /// All sizes, except for the Net pool size, must be multiples of 16 KiB.
        /// 
        /// You may need to adjust these values when services are
        /// used especially during development.
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct MemoryPools
        {
            /// <summary>The default value for the public memory pool used by the ToolkitNp2 library.</summary>
            public const int TOOLKIT_MEM_DEFAULT_SIZE = 16 * 1024 * 1024; // NP_TOOLKIT_MEM_DEFAULT_SIZE

            /// <summary>The minimum value required by the JSON memory pool.</summary>
            public const int JSON_MEM_MINIMUM_SIZE = 16 * 1024;    // JSON_MEM_MINIMUM_SIZE

            /// <summary>The default value for the JSON memory pool.</summary>
            public const int JSON_MEM_DEFAULT_SIZE = 4 * 1024 * 1024;    // JSON_MEM_DEFAULT_SIZE

            /// <summary>The default value for the WebAPI memory pool.</summary>
            public const int WEB_API_MEM_DEFAULT_SIZE = 1 * 1024 * 1024;    // WEB_API_MEM_DEFAULT_SIZE

            /// <summary>The default value for the HTTP memory pool.</summary>
            public const int HTTP_MEM_DEFAULT_SIZE = 64 * 1024;    // HTTP_MEM_DEFAULT_SIZE

            /// <summary>The minimum value required by the HTTP memory pool.</summary>
            public const int HTTP_MEM_MINIMUM_SIZE = 16 * 1024;    // HTTP_MEM_MINIMUM_SIZE

            /// <summary>The default value for the SSL memory pool.</summary>
            public const int SSL_MEM_DEFAULT_SIZE = 256 * 1024;    // SSL_MEM_DEFAULT_SIZE

            /// <summary>The minimum value required by the SSL memory pool.</summary>
            public const int SSL_MEM_MINIMUM_SIZE = 32 * 1024;    // SSL_MEM_MINIMUM_SIZE

            /// <summary>The default value for the Net memory pool.</summary>
            public const int NET_MEM_DEFAULT_SIZE = 32 * 1024;    // NET_MEM_DEFAULT_SIZE

            /// <summary>The minimum value required by the Net memory pool.</summary>
            public const int NET_MEM_MINIMUM_SIZE = 4 * 1024;    // NET_MEM_MINIMUM_SIZE

            /// <summary>The default value for the NpMatching2 memory pool.</summary>
            public const int MATCHING_MEM_DEFAULT_SIZE = 512 * 1024;    // MATCHING_MEM_DEFAULT_SIZE

            /// <summary>The default value for the SSL memory pool of the NpMatching2 library.</summary>
            public const int MATCHING_SSL_MEM_DEFAULT_SIZE = 192 * 1024;    // MATCHING_SSL_MEM_DEFAULT_SIZE = SCE_NP_MATCHING2_SSL_POOLSIZE_DEFAULT

            /// <summary>The default value for the NpInGameMessage memory pool.</summary>
            public const int IN_GAME_MESSAGE_MEM_DEFAULT_SIZE = 16 * 1024;    // IN_GAME_MESSAGE_MEM_DEFAULT_SIZE

            UInt64 toolkitPoolSize;
            UInt64 jsonPoolSize;
            UInt64 webApiPoolSize;
            UInt64 httpPoolSize;
            UInt64 sslPoolSize;
            UInt64 netPoolSize;
            UInt64 matchingPoolSize;
            UInt64 matchingSslPoolSize;
            UInt64 inGameMessagePoolSize;

            /// <summary>
            /// The size of the public memory pool used by the ToolkitNp2 library.
            /// </summary>
            public UInt64 ToolkitPoolSize
            {
                get { return toolkitPoolSize; }
                set
                {
                    Validate("ToolkitPoolSize", value, 0, "", true);

                    toolkitPoolSize = value;
                }
            }

            /// <summary>
            /// The size of the JSON memory pool.
            /// </summary>
            public UInt64 JsonPoolSize
            {
                get { return jsonPoolSize; }
                set
                {
                    Validate("JsonPoolSize", value, JSON_MEM_MINIMUM_SIZE, "JSON_MEM_MINIMUM_SIZE", true);

                    jsonPoolSize = value;
                }
            }

            /// <summary>
            /// The size of the WebAPI memory pool.
            /// </summary>
            public UInt64 WebApiPoolSize
            {
                get { return webApiPoolSize; }
                set
                {
                    Validate("WebApiPoolSize", value, 0, "", true);

                    webApiPoolSize = value;
                }
            }

            /// <summary>
            /// The size of the HTTP memory pool.
            /// </summary>
            public UInt64 HttpPoolSize
            {
                get { return httpPoolSize; }
                set
                {
                    Validate("HttpPoolSize", value, HTTP_MEM_MINIMUM_SIZE, "HTTP_MEM_MINIMUM_SIZE", true);

                    httpPoolSize = value;
                }
            }

            /// <summary>
            /// The size of the SSL memory pool.
            /// </summary>
            public UInt64 SslPoolSize
            {
                get { return sslPoolSize; }
                set
                {
                    Validate("SslPoolSize", value, SSL_MEM_MINIMUM_SIZE, "SSL_MEM_MINIMUM_SIZE", true);

                    sslPoolSize = value;
                }
            }

            /// <summary>
            /// The size of the Net memory pool. Doesn't need to be a multiple of 16 Kib
            /// </summary>
            public UInt64 NetPoolSize
            {
                get { return netPoolSize; }
                set
                {
                    Validate("NetPoolSize", value, NET_MEM_MINIMUM_SIZE, "NET_MEM_MINIMUM_SIZE", true);

                    netPoolSize = value;
                }
            }

            /// <summary>
            /// The size of the memory pool of the NpMatching2 library. This is ignored if the Matching service is not used.
            /// </summary>
            public UInt64 MatchingPoolSize
            {
                get { return matchingPoolSize; }
                set
                {
                    Validate("MatchingPoolSize", value, 0, "", true);

                    matchingPoolSize = value;
                }
            }

            /// <summary>
            /// The size of the SSL memory pool of the NpMatching2 library. This is ignored if the Matching service is not used.
            /// </summary>
            public UInt64 MatchingSslPoolSize
            {
                get { return matchingSslPoolSize; }
                set
                {
                    Validate("MatchingSslPoolSize", value, 0, "", true);

                    matchingSslPoolSize = value;
                }
            }

            /// <summary>
            /// The size of the memory pool of the NpInGameMessage library. This is ignored if In-Game messages are not used.	
            /// </summary>
            public UInt64 InGameMessagePoolSize
            {
                get { return inGameMessagePoolSize; }
                set
                {
                    Validate("InGameMessagePoolSize", value, 0, "", true);

                    inGameMessagePoolSize = value;
                }
            }

            private void Validate(string propertyName, UInt64 size, UInt64 minSize, string minSizeName, bool mustBe16kbAlligned)
            {
                if (mustBe16kbAlligned == true && (size % (16 * 1024)) != 0)
                {
                    // Not a multiple of 16 kbs.
                    throw new NpToolkitException("The size of the " + propertyName + " must be a multiple of 16 kbs (16384 bytes).");
                }

                if (minSize > 0 && size < minSize)
                {
                    throw new NpToolkitException("The size of the " + propertyName + " must be greater than " + minSizeName + ".");
                }
            }

            /// <summary>
            /// Initialise all memory pool sizes to the default values.
            /// </summary>
            public void Init()
            {
                toolkitPoolSize = TOOLKIT_MEM_DEFAULT_SIZE;
                jsonPoolSize = JSON_MEM_DEFAULT_SIZE;
                webApiPoolSize = WEB_API_MEM_DEFAULT_SIZE;
                httpPoolSize = HTTP_MEM_DEFAULT_SIZE;
                sslPoolSize = SSL_MEM_DEFAULT_SIZE;
                netPoolSize = NET_MEM_DEFAULT_SIZE;
                matchingPoolSize = MATCHING_MEM_DEFAULT_SIZE;
                matchingSslPoolSize = MATCHING_SSL_MEM_DEFAULT_SIZE;
                inGameMessagePoolSize = IN_GAME_MESSAGE_MEM_DEFAULT_SIZE;
            }
        }

        /// <summary>
        /// List of convient values for different SDK versions. Can be used to test against SDK value returned from <see cref="InitResult.SceSDKVersionValue"/>
        /// </summary>
        public enum SDKVersions
        {
            /// <summary> </summary>
            SDK_4 = 0x04000000, // 
        }

        /// <summary>
        /// Sce SDK Version
        /// </summary>
        public struct SceSDKVersion
        {
            /// <summary>
            /// Major version
            /// </summary>
            public UInt32 Major;

            /// <summary>
            /// Minor verson
            /// </summary>
            public UInt32 Minor;

            /// <summary>
            /// Patch version
            /// </summary>
            public UInt32 Patch;

            /// <summary>
            /// Return the SDK version as a string seperated into Major, Minor and Patch values
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Major.ToString("X2") + "." + Minor.ToString("X3") + "." + Patch.ToString("X3");
            }
        }

        /// <summary>
        /// The native initialization state of NpToolkit
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NativeInitResult
        {
            [MarshalAs(UnmanagedType.I1)]
            public bool initialized;

            public UInt32 sceSDKVersion; // SCE_ORBIS_SDK_VERSION 
        }

        /// <summary>
		/// The initialisation state of NpToolkit
		/// </summary>
		public struct InitResult
        {
            public bool initialized;

            public UInt32 sceSDKVersion; // SCE_ORBIS_SDK_VERSION 

            public Version dllVersion;

            /// <summary>
            /// Has NpToolkit been initialize correctly
            /// </summary>
            public bool Initialized
            {
                get { return initialized; }
            }

            /// <summary>
            /// The current SDK version the native plugin is built with
            /// </summary>
            public UInt32 SceSDKVersionValue
            {
                get { return sceSDKVersion; }
            }

            /// <summary>
            /// The current Version number for the SonyNp assembly
            /// </summary>
            public Version DllVersion
            {
                get { return dllVersion; }
            }

            /// <summary>
            /// The current SDK version as Major, Minor and Patch values.
            /// </summary>
            public SceSDKVersion SceSDKVersion
            {
                get
                {
                    SceSDKVersion version;

                    version.Patch = sceSDKVersion & 0x00000FFF;
                    version.Minor = (sceSDKVersion >> 12) & 0x00000FFF;
                    version.Major = (sceSDKVersion >> 24);

                    return version;
                }
            }

            public void Initialise(NativeInitResult nativeResult)
            {
                initialized = nativeResult.initialized;
                sceSDKVersion = nativeResult.sceSDKVersion;

                dllVersion = Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

        /// <summary>
        /// Initialisation settings for the NpToolkit system.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class InitToolkit
        {
            /// <summary>
            /// Content restrictions for the title. Defaulted to no age restriction.
            /// </summary>
            public ContentRestriction contentRestrictions;

            /// <summary>
            /// Server push notifications. Defaulted to the enabled all server push notifications types.
            /// </summary>
            [System.Obsolete("serverPushNotifications is deprecated, please use SetPushNotificationsFlags instead.")]
            public ServerPushNotifications serverPushNotifications;

            /// <summary>
            ///  Server push notifications. Defaulted to the enabled all server push notifications types.
            /// </summary>
            private PushNotificationsFlags serverPushNotificationsFlags; // uses 

            [MarshalAs(UnmanagedType.I1)]
            private bool notificationsFlagsSet;

            /// <summary>
            /// Thread settings. Defaulted to enable the NpToolkit thread to run on cores 2,3,4 and 5.
            /// </summary>
            public ThreadSettings threadSettings;

            /// <summary>
            /// Memory Pools. The size of all memory pools required by the ToolkitNp2 library.	
            /// </summary>
            public MemoryPools memoryPools;

            /// <summary>
            /// Initialise the settings to their defaults.
            /// </summary>
            public InitToolkit()
            {
                contentRestrictions.Init();

#pragma warning disable 618  // disable obsolete warning
                serverPushNotifications.Init();
#pragma warning restore 618

                threadSettings.Init();
                memoryPools.Init();

                serverPushNotificationsFlags = 0;
                notificationsFlagsSet = false;
            }

            /// <summary>
            ///  Server push notifications. Defaulted to the enabled all server push notifications types.
            /// </summary>
            /// <param name="pushNotifications">Flags to set</param>
            public void SetPushNotificationsFlags(PushNotificationsFlags pushNotifications)
            {
                serverPushNotificationsFlags = pushNotifications;
                notificationsFlagsSet = true;
            }

            /// <summary>
            /// Check if settings are valid. Will throw an exception is settings are invalid.
            /// Currently checks if the thread affinity mask is set to use cores 0 or 1 as this is not allowed.
            /// </summary>
            /// <exception cref="NpToolkitException">Will throw an exception if the <see cref="ThreadSettings.affinity"/> mask flags Core 0 or 1</exception>
            public void CheckValid()
            {
                if ((((int)threadSettings.affinity) & 0x3) != 0)
                {
                    throw new NpToolkitException("Can't set thread affinity to Core 0 or Core 1 as this will interfer with the main loop and gfx threads.");
                }
            }
        }
    }
}
