using System;
using System.Runtime.InteropServices;

namespace Sony
{
	namespace NP
	{
		/// <summary>
		/// Presence service related functionality.
		/// </summary>
		public class Presence
		{
			#region DLL Imports

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxDeletePresence(DeletePresenceRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxSetPresence(SetPresenceRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetPresence(GetPresenceRequest request, out APIResult result);

			#endregion

			#region Requests

			/// <summary>
			/// Parameters to delete the presence information of the calling user.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class DeletePresenceRequest : RequestBase
			{
				[MarshalAs(UnmanagedType.I1)]
				internal bool deleteGameData;
	
				[MarshalAs(UnmanagedType.I1)]
				internal bool deleteGameStatus;

				/// <summary>
				/// When set to true, game data of the calling user will be deleted. Defaults to true.
				/// </summary>
				public bool DeleteGameData { get { return deleteGameData; } }

				/// <summary>
				/// When set to true, all game statuses of the calling user (default and localized) will be deleted. Defaults to true.
				/// </summary>
				public bool DeleteGameStatus { get { return deleteGameStatus; } }

				/// <summary>
				/// Initializes a new instance of the <see cref="DeletePresenceRequest"/> class.
				/// </summary>
				public DeletePresenceRequest()
					: base(ServiceTypes.Presence, FunctionTypes.PresenceDeletePresence)
				{
					deleteGameData = true;
					deleteGameStatus = true;
				}
			}

			/// <summary>
			/// Information to specify a game status that is localized (language of localization and status itself).
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
			public struct LocalizedGameStatus
			{
				/// <summary>
				/// The maximum size of the GameStatus array
				/// </summary>
				public const int MAX_SIZE_LOCALIZED_GAME_STATUS = 96;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
				internal string languageCode; 

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_SIZE_LOCALIZED_GAME_STATUS+1)]
				internal string gameStatus;

				/// <summary>
				/// The localized game status provided is written in this language. Five digits format (countryCode-language)
				/// Takes a copy of the code, see remarks for details.
				/// </summary>
				/// <remarks>
				/// Takes a copy of the language code or returns a copy. The language code must be assign explicitly. 
				/// </remarks>
				public Core.LanguageCode LanguageCode
				{
					get
					{
						Core.LanguageCode cc = new Core.LanguageCode();
						cc.code = languageCode;
						return cc;
					}

					set
					{
						languageCode = value.code;
					}
				}

				/// <summary>
				/// A localized string for the game status. It will be provided when users have the paired language as the system software language. UTF-8 format
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the string is more than <see cref="MAX_SIZE_LOCALIZED_GAME_STATUS"/> characters.</exception>
				public string GameStatus
				{
					get { return gameStatus; }
					set 
					{
						if (value.Length > MAX_SIZE_LOCALIZED_GAME_STATUS)
						{
							throw new NpToolkitException("The size of the game stutus string is more than " + MAX_SIZE_LOCALIZED_GAME_STATUS + " characters.");
						}
						gameStatus = value; 
					}
				}
			}

			/// <summary>
			/// Parameters to set the presence information of the calling user.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
			public class SetPresenceRequest : RequestBase
			{
				/// <summary>
				/// The maximum size of localiszed statuses array
				/// </summary>
				public const int MAX_LOCALIZED_STATUSES = 50;

				/// <summary>
				/// The maximum size of the binary game data array
				/// </summary>
				public const int MAX_SIZE_GAME_DATA = 128;    // NpToolkit2::Presence::Request::SetPresence::MAX_SIZE_GAME_DATA

				/// <summary>
				/// The maximum size of the default game status string that will be shown for users whose localized game status is not found
				/// </summary>
				public const int MAX_SIZE_DEFAULT_GAME_STATUS = 191;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_SIZE_DEFAULT_GAME_STATUS)]
				internal string defaultGameStatus;

				///An array of localizations for the game status. If the system language on the console matches one of the languages, the game status used by the system and provided to the application will be the matching one. 
				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_LOCALIZED_STATUSES)]
				internal LocalizedGameStatus[] localizedGameStatuses;

				///The number of localizedGameStatus in the localizedGameStatuses array
				internal UInt32 numLocalizedGameStatuses;

				///Binary data the user can set for the game. It can be retrieved by other users (it can contain a session Id, room Id, or any information up to MAX_SIZE_GAME_DATA bytes)
				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SIZE_GAME_DATA)]
				internal byte[] binaryGameData;

				///The size of the binary data
				internal UInt32 binaryGameDataSize;

				/// <summary>
				/// A default string identifying the status (level, etc.) the user is in. It will be shown in "Now Playing" on the user profile. It can be a maximum of <see cref="MAX_SIZE_DEFAULT_GAME_STATUS"/> characters.
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the string is more than <see cref="MAX_SIZE_DEFAULT_GAME_STATUS"/> characters.</exception>
				public string DefaultGameStatus
				{
					get { return defaultGameStatus; }
					set
					{
						if (value.Length > MAX_SIZE_DEFAULT_GAME_STATUS)
						{
							throw new NpToolkitException("The size of the default game stutus string is more than " + MAX_SIZE_DEFAULT_GAME_STATUS + " characters.");
						}
						defaultGameStatus = value;
					}
				}

				/// <summary>
				/// An array of localizations for the game status. If the system language on the console matches one of the languages, the game status used by the system and provided to the application will be the matching one. 
				/// The array size must not exceed <see cref="MAX_LOCALIZED_STATUSES"/>.
				/// </summary>
				/// <remarks>
				/// Takes a copy of the array or returns a copy. The game statuses must be assign explicitly. Changing the array set or returned by this property won't change the stored data in this instance.
				/// </remarks>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is more than <see cref="MAX_LOCALIZED_STATUSES"/> characters.</exception>
				public LocalizedGameStatus[] LocalizedGameStatuses
				{
					get
					{
						if (numLocalizedGameStatuses == 0) return null;

						LocalizedGameStatus[] statuses = new LocalizedGameStatus[numLocalizedGameStatuses];

						Array.Copy(localizedGameStatuses, statuses, numLocalizedGameStatuses);

						return statuses;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_LOCALIZED_STATUSES)
							{
								throw new NpToolkitException("The size of the localized game statuses array is more than " + MAX_LOCALIZED_STATUSES);
							}
						
							value.CopyTo(localizedGameStatuses, 0);
							numLocalizedGameStatuses = (UInt32)value.Length;
						}
						else
						{
							numLocalizedGameStatuses = 0;
						}
					}
				}

				/// <summary>
				/// Binary data the user can set for the game. It can be retrieved by other users (it can contain a session Id, room Id, or any information up to <see cref="MAX_SIZE_GAME_DATA"/> bytes)
				/// </summary>
				/// <remarks>
				/// Takes a copy of the binary data or returns a copy. The binary data must be assign explicitly. Changing the array set or returned by this property won't change the stored data in this instance.
				/// </remarks>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is more than <see cref="MAX_SIZE_GAME_DATA"/> characters.</exception>
				public byte[] BinaryGameData
				{
					get 
					{
						if (binaryGameData == null) return null;

						byte[] gameData = new byte[binaryGameDataSize];

						Array.Copy(binaryGameData, gameData, binaryGameDataSize);

						return gameData; 
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_SIZE_GAME_DATA)
							{
								throw new NpToolkitException("The size of the binary game data is more than " + MAX_SIZE_GAME_DATA + " bytes.");
							}

							value.CopyTo(binaryGameData, 0);
							binaryGameDataSize = (UInt32)value.Length;
						}
						else
						{
							binaryGameDataSize = 0;
						}
					}
				}
						
				/// <summary>
				/// Initializes a new instance of the <see cref="SetPresenceRequest"/> class.
				/// </summary>		
				public SetPresenceRequest()
					: base(ServiceTypes.Presence, FunctionTypes.PresenceSetPresence)
				{
					defaultGameStatus = "";
					localizedGameStatuses = new LocalizedGameStatus[MAX_LOCALIZED_STATUSES];
					numLocalizedGameStatuses = 0;
					binaryGameData = new byte[MAX_SIZE_GAME_DATA];
					binaryGameDataSize = 0;
				}
			}

			/// <summary>
			/// Function to get the presence information for the target user specified.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class GetPresenceRequest : RequestBase
			{
				internal Core.NpAccountId fromUser;
				internal bool inContext;

				/// <summary>
				/// The user to get the presence from. It can be the calling user or a friend.
				/// </summary>
				public Core.NpAccountId FromUser
				{
					get { return fromUser; }
					set { fromUser = value; }
				}

				/// <summary>
				/// True by default. If the presence information to be obtained is regarding the game (true) or the system (false).
				/// </summary>
				public bool InContext
				{
					get { return inContext; }
					set { inContext = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetPresenceRequest"/> class.
				/// </summary>
				public GetPresenceRequest()
					: base(ServiceTypes.Presence, FunctionTypes.PresenceGetPresence)
				{
					fromUser.id = 0;
					inContext = true;
				}
			}
			#endregion

			#region classes
			/// <summary>
			/// This class contains the presence information of a user and title for one platform.
			/// </summary>
			public class PlatformPresence
			{
				/// <summary>
				/// The maximum size the name of the application can have
				/// </summary>
				public const int MAX_SIZE_TITLE_NAME = 127;

				/// <summary>
				/// The maximum size the status (playing a level, mission, etc.) can be
				/// </summary>
				public const int MAX_SIZE_GAME_STATUS = 191; // Request::SetPresence::MAX_SIZE_DEFAULT_GAME_STATUS; 

				/// <summary>
				/// The maximum size of binary data that can be set for a user
				/// </summary>
				public const int MAX_SIZE_GAME_DATA = 128; //Request::SetPresence::MAX_SIZE_GAME_DATA; 

				internal Core.OnlineStatus onlineStatusOnPlatform;
				internal Core.PlatformType platform;
				internal Core.TitleId titleId = new Core.TitleId();
				internal string titleName = "";
				internal string gameStatus = "";
				internal byte[] binaryGameData;

				/// <summary>
				/// If InContext is true, offline when the user is not playing the application or the user state (online, stand by) if the user is. If InContext is false, the state of the user on the platform
				/// </summary>
				public Core.OnlineStatus OnlineStatusOnPlatform { get { return onlineStatusOnPlatform; } }

				/// <summary>
				/// The platform the information in this class belongs to. See <see cref="Core.PlatformType"/> for values provided
				/// </summary>
				public Core.PlatformType Platform { get { return platform; } }

				/// <summary>
				/// The title Id being played by the user on the platform. Not provided if the <see cref="OnlineStatusOnPlatform"/> is offline.
				/// </summary>
				public Core.TitleId TitleId { get { return titleId; } }

				/// <summary>
				///  The name of the title the user is playing. Not provided if the <see cref="OnlineStatusOnPlatform"/> is offline.
				/// </summary>
				public string TitleName { get { return titleName; } }

				/// <summary>
				/// The status the user is in. It will match the calling user system language (or the default game status if a localized one is not found). It will only be retrieved on Title Ids that share the Presence service in DevNet
				/// </summary>
				public string GameStatus { get { return gameStatus; } }

				/// <summary>
				/// The binary data set by the user playing the application
				/// </summary>
				public byte[] BinaryGameData { get { return binaryGameData; } }

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.PlatformPresenceBegin);

					onlineStatusOnPlatform = (Core.OnlineStatus)buffer.ReadUInt32();
					platform = (Core.PlatformType)buffer.ReadUInt32();

					titleId.Read(buffer);

					buffer.ReadString(ref titleName);
					buffer.ReadString(ref gameStatus);
					buffer.ReadData(ref binaryGameData);

					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.PlatformPresenceEnd);
				}

				/// <summary>
				/// Returns the most common platform presence information.
				/// </summary>
				/// <returns>The string containing the online status, platform, and title info.</returns>
				public override string ToString()
				{
					string output = "";

					output += string.Format("\n: Platform Presence : OS ({0}) Platform ({1}) TitleId ({2}) ", onlineStatusOnPlatform, platform, titleId.ToString(), titleName);

					return output;
				}
			};

			/// <summary>
			/// This class represents the presence information of a user.
			/// </summary>
			public class UserPresence  // Equivilant to Presence in NpToolkit, but unlike C++ can't have a class named the same as it's parent type e.g. Presence::Presence
			{
				#region Properties

				/// <summary>
				/// The presence information can be retrieved for this maximum number of platforms
				/// </summary>
				public const int MAX_NUM_PLATFORM_PRESENCE = 3;

				internal Core.OnlineUser user = new Core.OnlineUser();
				internal Core.OnlineStatus psnOnlineStatus;
				internal Core.PlatformType mostRelevantPlatform;		
				internal PlatformPresence[] platforms;			

				/// <summary>
				/// The target user and its online Id (for display purposes) 
				/// </summary>
				public Core.OnlineUser User { get { return user; } }

				/// <summary>
				/// Indicates if the user appears as "Online", "Stand by" (Away) or "Offline" in PSN
				/// </summary>
				public Core.OnlineStatus PsnOnlineStatus { get { return psnOnlineStatus; } }

				/// <summary>
				/// The primary platform the user is using. Decided by the online status or, in case it is the same, the date/time of the presence update. See <see cref="Core.PlatformType"/> for values provided
				/// </summary>
				public Core.PlatformType MostRelevantPlatform { get { return mostRelevantPlatform; } }

				/// <summary>
				/// Information of the presence for the target user in different platforms. To know the platform, use the <see cref="PlatformPresence.Platform"/> data member of the object
				/// </summary>
				public PlatformPresence[] Platforms { get { return platforms; } }

				#endregion

				internal UserPresence()
				{

				}

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.PresenceBegin);

					user.Read(buffer);

					psnOnlineStatus = (Core.OnlineStatus)buffer.ReadUInt32();
					mostRelevantPlatform = (Core.PlatformType)buffer.ReadUInt32();

					UInt32 numberPlatforms = buffer.ReadUInt32();

					if (numberPlatforms == 0)
					{
						platforms = null;
					}
					else
					{
						platforms = new PlatformPresence[numberPlatforms];

						for (int i = 0; i < numberPlatforms; i++)
						{
							platforms[i] = new PlatformPresence();
							platforms[i].Read(buffer);
						}
					}

					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.PresenceEnd);
				}

				/// <summary>
				/// Create a string containg the most common data from the user presence.
				/// </summary>
				/// <returns>Returns the accountid, name, online static, relevant platform and number platforms in this presence.</returns>
				public override string ToString()
				{
					string output = "";

					int numPlatforms = 0;
					if (platforms != null) numPlatforms = platforms.Length;

					output += string.Format("0x{0:X} : {1} : PSN OS ({2}) MRP ({3}) #P ({4})'\n", User.accountId, User.onlineId.name, PsnOnlineStatus, MostRelevantPlatform, numPlatforms);

					for (int i = 0; i < numPlatforms; i++)
					{
						output += Platforms[i].ToString() + "\n";
					}

					return output;
				}

				
			}
			#endregion

			#region Delete Presence
			/// <summary>
			/// Function to delete the presence information for the calling user on the Presence server.
			/// </summary>
			/// <param name="request">The parameters with the information to delete.</param>
			/// <param name="response">This response does not have data, only return code.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int DeletePresence(DeletePresenceRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxDeletePresence(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}
			#endregion

			#region Set Presence
			/// <summary>
			/// Function to send to the server the game status (default and localized version) and the binary data for the calling user.
			/// </summary>
			/// <param name="request">The parameters with the information to be set for the calling user.</param>
			/// <param name="response">This response does not have data, only return code.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int SetPresence(SetPresenceRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxSetPresence(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}
			#endregion

			#region Get Presence

			/// <summary>
			/// This class represents the presence information of a user from a call to <see cref="GetPresence"/>
			/// </summary>
			public class PresenceResponse : ResponseBase
			{
				internal UserPresence userPresence = new UserPresence();

				/// <summary>
				/// Get the user presence.
				/// </summary>
				public UserPresence UserPresence { get { return userPresence; } }

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

					userPresence.Read(readBuffer);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// Function to obtain the presence information of a target user.
			/// </summary>
			/// <param name="request">The parameters with with the information to obtain the presence of a target user.</param>
			/// <param name="response">This response contains the return code and the presence information of the target user.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetPresence(GetPresenceRequest request, PresenceResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetPresence(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}
			#endregion


			#region Notification
			/// <summary>
			/// The type of presence update
			/// </summary>
			public enum PresenceUpdateType
			{
				/// <summary>Invalid presence update type</summary>
				invalid,
				/// <summary>Push event sent when game title information is updated</summary>
				gameTitle,
				/// <summary>Push event sent when status string related to gameplay is updated</summary>
				gameStatus,
				/// <summary>Push event sent when in-game presence arbitrarily-defined data is updated</summary>
				gameData	
			};

			/// <summary>
			/// Push notification that is sent to the NP Toolkit 2 callback when the presence of a users friend changes
			/// </summary>
			public class PresenceUpdateResponse : ResponseBase
			{
				/// <summary>
				/// The maximum size the status (playing a level, mission, etc.) can be
				/// </summary>
				public const int MAX_SIZE_GAME_STATUS = 191;

				/// <summary>
				/// The maximum size of binary data that can be set for a user
				/// </summary>
				public const int MAX_SIZE_GAME_DATA = 128;

				internal Core.OnlineUser localUpdatedUser = new Core.OnlineUser();
				internal Core.OnlineUser remoteUser = new Core.OnlineUser();
				internal Core.UserServiceUserId userId;
				internal PresenceUpdateType updateType;
				internal string gameStatus = "";
				internal byte[] binaryGameData;
				internal Core.PlatformType platform;

				/// <summary>
				/// IDs of local user whose friends presence was updated
				/// </summary>
				/// 
				public Core.OnlineUser LocalUpdatedUser { get { return localUpdatedUser; } }

				/// <summary>
				/// The user whose presence was updated
				/// </summary>
				public Core.OnlineUser RemoteUser { get { return remoteUser; } }

				/// <summary>
				/// The user Id of local user
				/// </summary>
				public Core.UserServiceUserId UserId { get { return userId; } }

				/// <summary>
				/// The type of presence notification
				/// </summary>
				public PresenceUpdateType UpdateType { get { return updateType; } }

				/// <summary>
				/// The status the user is in. This will only be set if <see cref="UpdateType"/> is set to <see cref="PresenceUpdateType.gameStatus"/> 
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if <see cref="UpdateType"/> isn't set to <see cref="PresenceUpdateType.gameStatus"/></exception>
				public string GameStatus 
				{ 
					get 
					{
						if (updateType != PresenceUpdateType.gameStatus)
						{
							throw new NpToolkitException("GameStatus isn't valid unless 'UpdateType' is set to " + PresenceUpdateType.gameStatus);
						}

						return gameStatus; 
					} 
				}

				/// <summary>
				/// The binary data set by the user playing the application. This will only be set if <see cref="UpdateType"/> is set to <see cref="PresenceUpdateType.gameData"/> 
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if <see cref="UpdateType"/> isn't set to <see cref="PresenceUpdateType.gameData"/></exception>
				public byte[] BinaryGameData 
				{
					get 
					{
						if (updateType != PresenceUpdateType.gameData)
						{
							throw new NpToolkitException("BinaryGameData isn't valid unless 'UpdateType' is set to " + PresenceUpdateType.gameData);
						}
						return binaryGameData; 
					} 
				}

				/// <summary>
				/// The platform the information in this class belongs to. See <see cref="Core.PlatformType"/> for values provided
				/// </summary>
				public Core.PlatformType Platform { get { return platform; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.PresenceUpdateBegin);

					localUpdatedUser.Read(readBuffer);
					remoteUser.Read(readBuffer);
					userId = readBuffer.ReadInt32();
					updateType = (PresenceUpdateType)readBuffer.ReadInt32();

					readBuffer.ReadString(ref gameStatus);
					readBuffer.ReadData(ref binaryGameData);

					platform = (Core.PlatformType)readBuffer.ReadInt32();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.PresenceUpdateEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}
			#endregion
		}

	}
}
