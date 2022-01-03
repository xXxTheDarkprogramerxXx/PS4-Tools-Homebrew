using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Sony
{
	namespace NP
	{
		/// <summary>
		/// Friend service related functionality
		/// </summary>
		public class Friends
		{
			#region DLL Imports

			// Friends
			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetFriends(GetFriendsRequest request, out APIResult result);

			// Friends Of Friends
			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetFriendsOfFriends(GetFriendsOfFriendsRequest request, out APIResult result);

			// Blocked Users
			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetBlockedUsers(GetBlockedUsersRquest request, out APIResult result);

			// Display Friend Request Dialog
			[DllImport("UnityNpToolkit2")]
			private static extern int PrxDisplayFriendRequestDialog(DisplayFriendRequestDialogRequest request, out APIResult result);

			// Display Friend Request Dialog
			[DllImport("UnityNpToolkit2")]
			private static extern int PrxDisplayBlockUserDialog(DisplayBlockUserDialogRequest request, out APIResult result);

			#endregion

			#region Get Friends
			/// <summary>
			/// Represents a users friend. Contains online profile and current presence information.
			/// </summary>
			public class Friend
			{
				internal Profiles.Profile profile = new Profiles.Profile();
				internal Presence.UserPresence presence = new Presence.UserPresence();

				/// <summary>
				/// The friends profile information. 'About me', 'languages used' are not returned
				/// </summary>
				public Profiles.Profile Profile { get { return profile; } }

				/// <summary>
				/// The friends presence information
				/// </summary>
				public Presence.UserPresence Presence { get { return presence; } }

				/// <summary>
				/// Construct friend object
				/// </summary>
				public Friend()
				{

				}

				/// <summary>
				/// Generate string from profile and presence data
				/// </summary>
				/// <returns>Profile and presence string.</returns>
				public override string ToString()
				{
					string output = "Profile:\n" + profile.ToString();
					output += "\nPresence:\n" + presence.ToString();
					return output;
				}

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.FriendBegin);

					profile.Read(buffer);
					presence.Read(buffer);

					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.FriendEnd);
				}
			}

			/// <summary>
			/// The mode in which the list of friends will be retrieved in
			/// </summary>
			public enum FriendsRetrievalModes
			{
				/// <summary>Value not set </summary>
				invalid = 0,
				/// <summary>Retrieve a complete list of friends</summary>
				all,
				/// <summary>Retrieve friends which are currently online</summary>
				online,
				/// <summary>Retrieve a list of friends who are currently playing the same game</summary>
				inContext,
				/// <summary>Retrieve a cached list of all friends. If cache is unavailable, the complete list will be retrieved from the server</summary>
				tryCached			
			};

			/// <summary>
			/// Parameters required to retrieve a list of a users friends
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class GetFriendsRequest : RequestBase
			{
				internal FriendsRetrievalModes mode;
				internal uint limit;
				internal uint offset;

				/// <summary>
				/// The specific mode for the request
				/// </summary>
				public FriendsRetrievalModes Mode
				{
					get { return mode; }
					set { mode = value; }
				}

				/// <summary>
				/// The number of friends to be requested in a single call. If set to 0, all friends will be retrieved and offset will be ignored as well
				/// </summary>
				public uint Limit
				{
					get { return limit; }
					set { limit = value; }
				}

				/// <summary>
				/// The offset into the list of the users friends at which to start retrieving friends
				/// </summary>
				public uint Offset
				{
					get { return offset; }
					set { offset = value; }
				}

				/// <summary>
				/// Construct friends request parameters
				/// </summary>
				public GetFriendsRequest()
					: base(ServiceTypes.Friends, FunctionTypes.FriendsGetFriends)
				{
					mode = FriendsRetrievalModes.invalid;
					limit = 0;
					offset = 0;
				}
			}

			/// <summary>
			/// An array of the users friends
			/// </summary>
			public class FriendsResponse : ResponseBase
			{
				internal Friend[] friends;

				/// <summary>
				/// List of the users friends
				/// </summary>
				public Friend[] Friends { get { return friends; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.FriendsBegin);

					UInt32 numberFriends = readBuffer.ReadUInt32();

					friends = new Friend[numberFriends];

					for(int i = 0; i < numberFriends; i++)
					{
						friends[i] = new Friend();
						friends[i].Read(readBuffer);
					}

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.FriendsEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// Get a list of a users friends
			/// </summary>
			/// <param name="request">The parameters required to obtain a list of the users friends</param>
			/// <param name="response">The list of friends</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetFriends(GetFriendsRequest request, FriendsResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetFriends(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}
			#endregion

			#region Get Friends of Friends

			/// <summary>
			/// A friend along with a list of their friends
			/// </summary>
			public class FriendsOfFriend
			{
				internal Core.OnlineUser originalFriend;
				internal Core.OnlineUser[] users;

				/// <summary>
				/// The friend of the requested user
				/// </summary>
				public Core.OnlineUser OriginalFriend { get { return originalFriend; } }

				/// <summary>
				/// The friends of the user friend
				/// </summary>
				public Core.OnlineUser[] Users { get { return users; } }

				/// <summary>
				/// Initialise empty friends of friend list
				/// </summary>
				public FriendsOfFriend()
				{
					originalFriend = new Core.OnlineUser();
				}
			}

			/// <summary>
			/// Parameters required to retrieve friends of a users friends
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class GetFriendsOfFriendsRequest : RequestBase
			{
				/// <summary>The maximum number of accounts that can be sent per request</summary>
				public const int MAX_ACCOUNT_IDS = 10;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_ACCOUNT_IDS)]
				internal Core.NpAccountId[] accountIds;

				internal UInt32 numAccountIds;

				/// <summary>
				/// The account IDs of the user's friends.
				/// </summary>
				public Core.NpAccountId[] AccountIds 
				{
					get
					{
						if (numAccountIds == 0) return null;

						Core.NpAccountId[] output = new Core.NpAccountId[numAccountIds];

						Array.Copy(accountIds, output, numAccountIds);

						return output;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_ACCOUNT_IDS)
							{
								throw new NpToolkitException("The size of the array is more than " + MAX_ACCOUNT_IDS);
							}

							value.CopyTo(accountIds, 0);
							numAccountIds = (UInt32)value.Length;
						}
						else
						{
							numAccountIds = 0;
						}
					}
				}

				/// <summary>
				/// Construct friends of friends parameters
				/// </summary>
				public GetFriendsOfFriendsRequest()
					: base(ServiceTypes.Friends, FunctionTypes.FriendsGetFriendsOfFriends)
				{
					accountIds = new Core.NpAccountId[MAX_ACCOUNT_IDS];
					numAccountIds = 0;
				}
			}

			/// <summary>
			/// A list of friends of friends
			/// </summary>
			public class FriendsOfFriendsResponse : ResponseBase
			{
				internal FriendsOfFriend[] friendsOfFriends;

				/// <summary>
				/// Friends of friends of a user
				/// </summary>
				public FriendsOfFriend[] FriendsOfFriends { get { return friendsOfFriends; } }

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
	
					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.FriendsOfFriendsBegin);

					UInt32 numFriendsOfFriends = readBuffer.ReadUInt32();

					friendsOfFriends = new FriendsOfFriend[numFriendsOfFriends];

					for (int i = 0; i < numFriendsOfFriends; i++)
					{
						friendsOfFriends[i] = new FriendsOfFriend();

						friendsOfFriends[i].originalFriend.Read(readBuffer);

						UInt32 numUsers = readBuffer.ReadUInt32();

						friendsOfFriends[i].users = new Core.OnlineUser[numUsers];

						for (int u = 0; u < numUsers; u++)
						{
							friendsOfFriends[i].users[u] = new Core.OnlineUser();
							friendsOfFriends[i].users[u].Read(readBuffer);
						}
					}

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.FriendsOfFriendsEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// Get friends of friends.
			/// </summary>
			/// <param name="request">Parameters required to retrieve friends of friends.</param>
			/// <param name="response">The friends of the requested friends.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetFriendsOfFriends(GetFriendsOfFriendsRequest request, FriendsOfFriendsResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetFriendsOfFriends(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}
			#endregion

			#region Get Blocked Users
			/// <summary>
			/// The mode in which blocked users will be retrieved
			/// </summary>
			public enum BlockedUsersRetrievalMode
			{
				/// <summary>Value not set</summary>
				invalid = 0,
				/// <summary>Will retrieve blocked user list</summary>
				all,
				/// <summary>Retrieve a cached list of all blocked users. If cache is unavailable, the complete list will be retrieved from the server</summary>
				tryCached		
			};

			/// <summary>
			/// Parameters required to retrieve a users block list
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class GetBlockedUsersRquest : RequestBase
			{
				internal BlockedUsersRetrievalMode mode;
				internal uint limit;
				internal uint offset;

				/// <summary>
				/// The specific mode for the request
				/// </summary>
				public BlockedUsersRetrievalMode Mode
				{
					get { return mode; }
					set { mode = value; }
				}

				/// <summary>
				/// The number of users in block list to be requested in a single call. If this is set to 0, then all blocked users are retrieved
				/// </summary>
				public uint Limit
				{
					get { return limit; }
					set { limit = value; }
				}

				/// <summary>
				/// The offset into the list of the blocked users at which to start retrieving another entry
				/// </summary>
				public uint Offset
				{
					get { return offset; }
					set { offset = value; }
				}

				/// <summary>
				/// Construct blocked users parameters
				/// </summary>
				public GetBlockedUsersRquest()
					: base(ServiceTypes.Friends, FunctionTypes.FriendsGetBlockedUsers)
				{
					mode = BlockedUsersRetrievalMode.invalid;
					limit = 0;
					offset = 0;
				}
			}

			/// <summary>
			/// A list of users that have been blocked
			/// </summary>
			public class BlockedUsersResponse : ResponseBase
			{
				internal Core.OnlineUser[] users;

				/// <summary>
				/// The list of blocked users
				/// </summary>
				public Core.OnlineUser[] Users { get { return users; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.BlockedUsersBegin);

					// Copy the data from the response buffer
					UInt32 numUsers = readBuffer.ReadUInt32();

					users = new Core.OnlineUser[numUsers];

					for (int i = 0; i < numUsers; i++)
					{
						users[i] = new Core.OnlineUser();

						users[i].Read(readBuffer);
					}

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.BlockedUsersEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// Get a list of blocked users.
			/// </summary>
			/// <param name="request">Parameters required to retrieve a list of blocked users.</param>
			/// <param name="response">The list of users that have been blocked.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetBlockedUsers(GetBlockedUsersRquest request, BlockedUsersResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetBlockedUsers(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Display Friend Request Dialog

			/// <summary>
			/// Parameters required to display a dialog where the user can send a friend request
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class DisplayFriendRequestDialogRequest : RequestBase
			{
				internal Core.NpAccountId targetUser;

				/// <summary>
				/// The user to send a friend request to
				/// </summary>
				public Core.NpAccountId TargetUser 
				{ 
					get { return targetUser; } 
					set { targetUser = value; } 
				}

				/// <summary>
				///  Construct Display Friend Request Dialog parameters
				/// </summary>
				public DisplayFriendRequestDialogRequest()
					: base(ServiceTypes.Friends, FunctionTypes.FriendsDisplayFriendRequestDialog)
				{
					targetUser.id = 0;
				}
			}

			/// <summary>
			/// Display a dialog where a local user can send a friend request to a target PlayStation Network user
			/// </summary>
			/// <param name="request">Target user to display in the friend request dialog.</param>
			/// <param name="response">This response does not have data, only return code.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int DisplayFriendRequestDialog(DisplayFriendRequestDialogRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxDisplayFriendRequestDialog(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Display Block User Dialog

			/// <summary>
			/// Parameters required to display a dialog where a user can block another user
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class DisplayBlockUserDialogRequest : RequestBase
			{			
				internal Core.NpAccountId targetUser;

				/// <summary>
				/// The user to send a friend request to
				/// </summary>
				public Core.NpAccountId TargetUser
				{
					get { return targetUser; }
					set { targetUser = value; }
				}

				/// <summary>
				///  Construct Display Block User Dialog parameters
				/// </summary>
				public DisplayBlockUserDialogRequest()
					: base(ServiceTypes.Friends, FunctionTypes.FriendsDisplayBlockUserDialog)
				{
					targetUser.id = 0;
				}
			}

			/// <summary>
			/// Display a dialog to block another PlayStation Network user.
			/// </summary>
			/// <param name="request">The target user to display in the block user dialog.</param>
			/// <param name="response">This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int DisplayBlockUserDialog(DisplayBlockUserDialogRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxDisplayBlockUserDialog(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Notifications
			/// <summary>
			/// Event type when a friend notification is received
			/// </summary>
			public enum FriendListUpdateEvents
			{
				/// <summary>No states have changed. This should never happen as otherwise there would be no notification</summary>
				none = 0,
				/// <summary>A friend has been added to the users friend list</summary>
				friendAdded,
				/// <summary>A friend has been removed from the users friend list</summary>
				friendRemoved,
				/// <summary>The online status of a friend has changed</summary>
				friendOnlineStatusChanged	
			};

			/// <summary>
			/// Notification that is sent to the NP Toolkit callback when a users friend list has changed
			/// </summary>
			public class FriendListUpdateResponse : ResponseBase
			{
				internal Core.OnlineUser localUpdatedUser = new Core.OnlineUser();
				internal Core.OnlineUser remoteUser = new Core.OnlineUser();
				internal Core.UserServiceUserId userId;
				internal FriendListUpdateEvents eventType;

				/// <summary>
				/// IDs of user whose friends list was updated
				/// </summary>
				public Core.OnlineUser LocalUpdatedUser { get { return localUpdatedUser; } }

				/// <summary>
				/// The friend that was added or removed
				/// </summary>
				public Core.OnlineUser RemoteUser { get { return remoteUser; } }

				/// <summary>
				/// The user ID of the user whose state has changed
				/// </summary>
				public Core.UserServiceUserId UserId { get { return userId; } }

				/// <summary>
				/// Specifies whether a friend has been added or removed
				/// </summary>
				public FriendListUpdateEvents EventType { get { return eventType; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.FriendListUpdateBegin);

					localUpdatedUser.Read(readBuffer);
					remoteUser.Read(readBuffer);

					userId = readBuffer.ReadInt32();
					eventType = (FriendListUpdateEvents)readBuffer.ReadInt32();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.FriendListUpdateEnd);
					
					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// Notification that is sent to the NP Toolkit callback in the event where a user has been added or removed from the block list
			/// </summary>
			public class BlocklistUpdateResponse : ResponseBase
			{
				internal Core.OnlineUser localUpdatedUser = new Core.OnlineUser();
				internal Core.UserServiceUserId userId;

				/// <summary>
				/// IDs of user whose friends list was updated
				/// </summary>
				public Core.OnlineUser LocalUpdatedUser { get { return localUpdatedUser; } }

				/// <summary>
				/// The user ID of the user whose state has changed
				/// </summary>
				public Core.UserServiceUserId UserId { get { return userId; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.BlocklistUpdateBegin);

					localUpdatedUser.Read(readBuffer);
					userId = readBuffer.ReadInt32();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.BlocklistUpdateEnd);
					
					EndReadResponseBuffer(readBuffer);
				}
			}
			#endregion

		}

	}
}
