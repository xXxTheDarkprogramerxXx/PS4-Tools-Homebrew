using System;
using System.Runtime.InteropServices;

namespace Sony
{
	namespace NP
	{
		/// <summary>
		/// Trophy service related functionality.
		/// </summary>
		public class Trophies
		{
			#region DLL Imports

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxRegisterTrophyPack(RegisterTrophyPackRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxUnlockTrophy(UnlockTrophyRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxSetScreenshot(SetScreenshotRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetUnlockedTrophies(GetUnlockedTrophiesRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxDisplayTrophyListDialog(DisplayTrophyListDialogRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetTrophyPackSummary(GetTrophyPackSummaryRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetTrophyPackGroup(GetTrophyPackGroupRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetTrophyPackTrophy(GetTrophyPackTrophyRequest request, out APIResult result);

			#endregion

			#region Requests

			/// <summary>
			/// Parameters passed to register the trophy package for the calling user.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class RegisterTrophyPackRequest : RequestBase
			{
				/// <summary>
				/// Initializes a new instance of the <see cref="RegisterTrophyPackRequest"/> class.
				/// </summary>
				public RegisterTrophyPackRequest()
					: base(ServiceTypes.Trophy, FunctionTypes.TrophyRegisterTrophyPack)
				{
				}
			}

			/// <summary>
			/// Parameters passed to unlock a trophy for the calling user.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class UnlockTrophyRequest : RequestBase
			{
				internal Int32 trophyId;

				/// <summary>
				/// The trophy Id the user is going to unlock. The platinum trophy (0) cannot be unlocked manually if the title supports a platinum trophy. For small scope
				/// titles id 0 can be unlocked manually.
				/// </summary>
				public Int32 TrophyId
				{
					get { return trophyId; }
					set { trophyId = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="UnlockTrophyRequest"/> class.
				/// </summary>
				public UnlockTrophyRequest()
					: base(ServiceTypes.Trophy, FunctionTypes.TrophyUnlock)
				{
					trophyId = -1;
				}
			}

			/// <summary>
			/// Parameters passed to capture a screenshot to be linked with trophies.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class SetScreenshotRequest : RequestBase
			{
				/// <summary>
				/// A representation of an incorrect trophy Id
				/// </summary>
				public const Int32 INVALID_TROPHY_ID = -1;

				/// <summary>
				/// The maximum number of trophies the array <see cref="TrophiesIds"/> can have    
				/// </summary>
				public const Int32 MAX_NUMBER_TROPHIES = 4;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_NUMBER_TROPHIES)]
				internal Int32[] trophiesIds;      
          
				internal UInt32 numTrophiesIds;
         	
				[MarshalAs(UnmanagedType.I1)]
				internal bool assignToAllUsers;

				/// <summary>
				/// An array with the trophy Ids to be assigned to the screenshot once it is taken. A maximum of <see cref="MAX_NUMBER_TROPHIES"/>
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is more than <see cref="MAX_NUMBER_TROPHIES"/></exception>
				public Int32[] TrophiesIds
				{
					get
					{
						if (numTrophiesIds == 0) return null;

						Int32[] ids = new Int32[numTrophiesIds];

						Array.Copy(trophiesIds, ids, numTrophiesIds);

						return ids;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_NUMBER_TROPHIES)
							{
								throw new NpToolkitException("The size of the TrophyIds array is more than " + MAX_NUMBER_TROPHIES);
							}

							value.CopyTo(trophiesIds, 0);
							numTrophiesIds = (UInt32)value.Length;
						}
						else
						{
							numTrophiesIds = 0;
						}
					}
				}

				/// <summary>
				/// True by default. Specify if the screenshot taken should be associated with all logged in users for the trophies specified in the <see cref="TrophiesIds"/> array 
				/// </summary>
				public bool AssignToAllUsers
				{
					get { return assignToAllUsers; }
					set { assignToAllUsers = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="SetScreenshotRequest"/> class.
				/// </summary>
				public SetScreenshotRequest()
					: base(ServiceTypes.Trophy, FunctionTypes.TrophySetScreenshot)
				{
					trophiesIds = new Int32[MAX_NUMBER_TROPHIES];
					numTrophiesIds = 0;
					assignToAllUsers = true;
				}
			}

			/// <summary>
			/// Parameters passed to get the unlocked trophies of the calling user.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class GetUnlockedTrophiesRequest : RequestBase
			{
				/// <summary>
				/// Initializes a new instance of the <see cref="GetUnlockedTrophiesRequest"/> class.
				/// </summary>
				public GetUnlockedTrophiesRequest()
					: base(ServiceTypes.Trophy, FunctionTypes.TrophyGetUnlockedTrophies)
				{
				}
			}

			/// <summary>
			/// Parameters passed to open the trophy list for the title and service label.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class DisplayTrophyListDialogRequest : RequestBase
			{
				/// <summary>
				/// Initializes a new instance of the <see cref="DisplayTrophyListDialogRequest"/> class.
				/// </summary>
				public DisplayTrophyListDialogRequest()
					: base(ServiceTypes.Trophy, FunctionTypes.TrophyDisplayTrophyListDialog)
				{
				}
			}

			/// <summary>
			/// Parameters passed to get summary information of the entire trophy package.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class GetTrophyPackSummaryRequest : RequestBase
			{
				[MarshalAs(UnmanagedType.I1)]
				internal bool retrieveTrophyPackSummaryIcon;

				/// <summary>
				/// False by default. Set it to true to return the icon for the trophy package
				/// </summary>
				public bool RetrieveTrophyPackSummaryIcon
				{
					get { return retrieveTrophyPackSummaryIcon; }
					set { retrieveTrophyPackSummaryIcon = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetTrophyPackSummaryRequest"/> class.
				/// </summary>
				public GetTrophyPackSummaryRequest()
					: base(ServiceTypes.Trophy, FunctionTypes.TrophyGetTrophyPackSummary)
				{
					retrieveTrophyPackSummaryIcon = false;
				}
			}

			/// <summary>
			/// Parameters passed to get information regarding a specific group of the trophy package.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class GetTrophyPackGroupRequest : RequestBase
			{
				/// The group to return the information from. 
				internal Int32 groupId;					//SceNpTrophyGroupId   

				///False by default. Set it to true to return the icon for the group
				[MarshalAs(UnmanagedType.I1)]
				internal bool retrieveTrophyPackGroupIcon;

				/// <summary>
				/// The group to return the information from
				/// </summary>
				public Int32 GroupId
				{
					get { return groupId; }
					set { groupId = value; }
				}

				/// <summary>
				/// False by default. Set it to true to return the icon for the group
				/// </summary>
				public bool RetrieveTrophyPackGroupIcon
				{
					get { return retrieveTrophyPackGroupIcon; }
					set { retrieveTrophyPackGroupIcon = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetTrophyPackGroupRequest"/> class.
				/// </summary>
				public GetTrophyPackGroupRequest()
					: base(ServiceTypes.Trophy, FunctionTypes.TrophyGetTrophyPackGroup)
				{
					groupId = -1;
					retrieveTrophyPackGroupIcon = false;
				}
			}

			/// <summary>
			/// Parameters passed to get information regarding a specific trophy of the trophy package.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class GetTrophyPackTrophyRequest : RequestBase
			{
				///The trophy to return the information from 
				internal Int32 trophyId;						//  SceNpTrophyId
				///False by default. Set it to true to return the icon for the trophy 
				internal bool retrieveTrophyPackTrophyIcon;

				/// <summary>
				/// The trophy to return the information from
				/// </summary>
				public Int32 TrophyId
				{
					get { return trophyId; }
					set { trophyId = value; }
				}

				/// <summary>
				/// False by default. Set it to true to return the icon for the trophy 
				/// </summary>
				public bool RetrieveTrophyPackTrophyIcon
				{
					get { return retrieveTrophyPackTrophyIcon; }
					set { retrieveTrophyPackTrophyIcon = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetTrophyPackTrophyRequest"/> class.
				/// </summary>
				public GetTrophyPackTrophyRequest()
					: base(ServiceTypes.Trophy, FunctionTypes.TrophyGetTrophyPackTrophy)
				{
					trophyId = -1;
					retrieveTrophyPackTrophyIcon = false;
				}
			}
			#endregion

			#region Register Trophy Pack

			/// <summary>
			/// This function makes the trophy package visible to the user in the system software
			/// </summary>
			/// <param name="request">Parameters needed to register the trophy package for a user </param>
			/// <param name="response">This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int RegisterTrophyPack(RegisterTrophyPackRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxRegisterTrophyPack(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}
			#endregion

			#region Set Screenshot
			/// <summary>
			/// This function captures a screenshot and set it to the locked trophies specified, to be visible in the system when they are unlocked.
			/// </summary>
			/// <param name="request">Parameters needed to take the screenshot </param>
			/// <param name="response">This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int SetScreenshot(SetScreenshotRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxSetScreenshot(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}
			#endregion

			#region Unlock Trophy
			/// <summary>
			/// Function to unlock a trophy for the calling user.
			/// </summary>
			/// <param name="request">The parameters needed to register the trophy package for a user.</param>
			/// <param name="response">This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int UnlockTrophy(UnlockTrophyRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				if (request.trophyId < 0)
				{
					throw new NpToolkitException("Invalid trophy id has been used.");
				}

				int ret = PrxUnlockTrophy(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}
			#endregion

			#region Get Unlocked Trophies
			/// <summary>
			/// Class containing the Ids for the unlocked trophies the user making the request has.
			/// </summary>
			public class UnlockedTrophiesResponse : ResponseBase
			{
				internal Int32[] trophyIds;

				/// <summary>
				/// Array of trophy Ids already unlocked by the user making the request
				/// </summary>
				public Int32[] TrophyIds { get { return trophyIds; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.UnlockedTrophiesBegin);

					UInt32 numTrophiesIds = readBuffer.ReadUInt32();

					trophyIds = new Int32[numTrophiesIds];

					for (int i = 0; i < numTrophiesIds; i++)
					{
						trophyIds[i] = readBuffer.ReadInt32();
					}
						
					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.UnlockedTrophiesEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function is used to know which trophies have been already unlocked by the calling user.
			/// </summary>
			/// <param name="request">Parameters needed to obtain the unlocked trophy Ids for the calling user </param>
			/// <param name="response">This response contains the return code and an array with the unlocked trophy Ids</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetUnlockedTrophies(GetUnlockedTrophiesRequest request, UnlockedTrophiesResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetUnlockedTrophies(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}
			#endregion

			#region Display Trophy List Dialog

			/// <summary>
			/// This function opens the System Trophy list for the specified  title and service label.
			/// </summary>
			/// <param name="request">Parameters needed to open the System Trophy list </param>
			/// <param name="response">This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int DisplayTrophyListDialog(DisplayTrophyListDialogRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxDisplayTrophyListDialog(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}
			#endregion

			#region Get Trophy Pack Summary

			/// <summary>
			/// Trophy configuration data of a trophy set
			/// </summary>
			public struct NpTrophyGameDetails // Maps to SceNpTrophyGameDetails
			{
				internal UInt32 numGroups;
				internal UInt32 numTrophies;
				internal UInt32 numPlatinum;
				internal UInt32 numGold;
				internal UInt32 numSilver;
				internal UInt32 numBronze;
				internal string title;
				internal string description;

				/// <summary>
				/// Defined total number of trophy groups
				/// </summary>
				public UInt32 NumGroups { get { return numGroups; } }

				/// <summary>
				/// Defined total number of trophies
				/// </summary>
				public UInt32 NumTrophies { get { return numTrophies; } }

				/// <summary>
				/// Defined total number of platinum trophies
				/// </summary>
				public UInt32 NumPlatinum { get { return numPlatinum; } }

				/// <summary>
				/// Defined total number of gold trophies
				/// </summary>
				public UInt32 NumGold { get { return numGold; } }

				/// <summary>
				/// Defined total number of silver trophies
				/// </summary>
				public UInt32 NumSilver { get { return numSilver; } }

				/// <summary>
				/// Defined total number of bronze trophies
				/// </summary>
				public UInt32 NumBronze { get { return numBronze; } }

				/// <summary>
				/// Name of the trophy set
				/// </summary>
				public string Title { get { return title; } }

				/// <summary>
				/// Description of the trophy set
				/// </summary>
				public string Description { get { return description; } }

				internal void Read(MemoryBuffer buffer)
				{
					numGroups = buffer.ReadUInt32();
					numTrophies = buffer.ReadUInt32();
					numPlatinum = buffer.ReadUInt32();
					numGold = buffer.ReadUInt32();
					numSilver = buffer.ReadUInt32();
					numBronze = buffer.ReadUInt32();

					buffer.ReadString(ref title);
					buffer.ReadString(ref description);
				}
			}

			/// <summary>
			/// The user progress regarding the trophy package
			/// </summary>
			public struct NpTrophyGameData // Maps to SceNpTrophyGameData
			{
				internal UInt32 unlockedTrophies;
				internal UInt32 unlockedPlatinum;
				internal UInt32 unlockedGold;
				internal UInt32 unlockedSilver;
				internal UInt32 unlockedBronze;
				internal UInt32 progressPercentage;

				/// <summary>
				/// Number of unlocked trophies
				/// </summary>
				public UInt32 UnlockedTrophies { get { return unlockedTrophies; } }

				/// <summary>
				/// Number of unlocked platinum trophies
				/// </summary>
				public UInt32 UnlockedPlatinum { get { return unlockedPlatinum; } }

				/// <summary>
				/// Number of unlocked gold trophies
				/// </summary>
				public UInt32 UnlockedGold { get { return unlockedGold; } }

				/// <summary>
				/// Number of unlocked silver trophies
				/// </summary>
				public UInt32 UnlockedSilver { get { return unlockedSilver; } }

				/// <summary>
				/// Number of unlocked bronze trophies
				/// </summary>
				public UInt32 UnlockedBronze { get { return unlockedBronze; } }

				/// <summary>
				/// Progress of the processing (%)
				/// </summary>
				public UInt32 ProgressPercentage { get { return progressPercentage; } }

				internal void Read(MemoryBuffer buffer)
				{
					unlockedTrophies = buffer.ReadUInt32();
					unlockedPlatinum = buffer.ReadUInt32();
					unlockedGold = buffer.ReadUInt32();
					unlockedSilver = buffer.ReadUInt32();
					unlockedBronze = buffer.ReadUInt32();
					progressPercentage = buffer.ReadUInt32();
				}
			}

			/// <summary>
			///  Class containing extensive information of the trophy package (static configuration, user progress and, if requested, icon).
			/// </summary>
			public class TrophyPackSummaryResponse : ResponseBase
			{
				internal Icon icon = null;
				internal NpTrophyGameDetails staticConfiguration;
				internal NpTrophyGameData userProgress;

				/// <summary>
				/// The icon retrieved in case it was explicitely specified in the request
				/// </summary>
				public Icon Icon { get { return icon; } }

				/// <summary>
				/// Generic configuration written when the package was created
				/// </summary>
				public NpTrophyGameDetails StaticConfiguration { get { return staticConfiguration; } }

				/// <summary>
				/// The user progress regarding the trophy package
				/// </summary>
				public NpTrophyGameData UserProgress { get { return userProgress; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.TrophyPackSummaryBegin);

					icon = Icon.ReadAndCreate(readBuffer);
					staticConfiguration.Read(readBuffer);
					userProgress.Read(readBuffer);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.TrophyPackSummaryEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function is used to obtain information about the trophy package and the user interaction with it.
			/// </summary>
			/// <param name="request">Parameters needed to obtain the trophy package summary information </param>
			/// <param name="response">This response contains the return code and the icon (if requested), the configuration  data, and the user specific data, of the trophy package</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetTrophyPackSummary(GetTrophyPackSummaryRequest request, TrophyPackSummaryResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetTrophyPackSummary(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}
			#endregion

			#region Get Trophy Pack Group

			/// <summary>
			/// Generic configuration written when the group was created
			/// </summary>
			public struct NpTrophyGroupDetails // Maps to SceNpTrophyGroupDetails
			{
				internal Int32 groupId;
				internal UInt32 numTrophies;
				internal UInt32 numPlatinum;
				internal UInt32 numGold;
				internal UInt32 numSilver;
				internal UInt32 numBronze;
				internal string title;
				internal string description;

				/// <summary>
				/// Trophy group ID
				/// </summary>
				public Int32 GroupId { get { return groupId; } }

				/// <summary>
				/// Defined total number of trophies
				/// </summary>
				public UInt32 NumTrophies { get { return numTrophies; } }

				/// <summary>
				/// Defined total number of platinum trophies
				/// </summary>
				public UInt32 NumPlatinum { get { return numPlatinum; } }

				/// <summary>
				/// Defined total number of gold trophies
				/// </summary>
				public UInt32 NumGold { get { return numGold; } }

				/// <summary>
				/// Defined total number of silver trophies
				/// </summary>
				public UInt32 NumSilver { get { return numSilver; } }

				/// <summary>
				/// Defined total number of bronze trophies
				/// </summary>
				public UInt32 NumBronze { get { return numBronze; } }

				/// <summary>
				/// Name of the trophy group
				/// </summary>
				public string Title { get { return title; } }

				/// <summary>
				/// Description of the trophy group
				/// </summary>
				public string Description { get { return description; } }

				internal void Read(MemoryBuffer buffer)
				{
					groupId = buffer.ReadInt32();
					numTrophies = buffer.ReadUInt32();
					numPlatinum = buffer.ReadUInt32();
					numGold = buffer.ReadUInt32();
					numSilver = buffer.ReadUInt32();
					numBronze = buffer.ReadUInt32();

					buffer.ReadString(ref title);
					buffer.ReadString(ref description);
				}
			}

			/// <summary>
			/// The user progress regarding the group
			/// </summary>
			public struct NpTrophyGroupData // Maps to SceNpTrophyGroupData
			{
				internal Int32 groupId;
				internal UInt32 unlockedTrophies;
				internal UInt32 unlockedPlatinum;
				internal UInt32 unlockedGold;
				internal UInt32 unlockedSilver;
				internal UInt32 unlockedBronze;
				internal UInt32 progressPercentage;

				/// <summary>
				/// Trophy group ID
				/// </summary>
				public Int32 GroupId { get { return groupId; } }

				/// <summary>
				/// Number of unlocked trophies
				/// </summary>
				public UInt32 UnlockedTrophies { get { return unlockedTrophies; } }

				/// <summary>
				/// Number of unlocked platinum trophies
				/// </summary>
				public UInt32 UnlockedPlatinum { get { return unlockedPlatinum; } }

				/// <summary>
				/// Number of unlocked gold trophies
				/// </summary>
				public UInt32 UnlockedGold { get { return unlockedGold; } }

				/// <summary>
				/// Number of unlocked silver trophies
				/// </summary>
				public UInt32 UnlockedSilver { get { return unlockedSilver; } }

				/// <summary>
				/// Number of unlocked bronze trophies
				/// </summary>
				public UInt32 UnlockedBronze { get { return unlockedBronze; } }

				/// <summary>
				/// Progress of the processing (%)
				/// </summary>
				public UInt32 ProgressPercentage { get { return progressPercentage; } }

				internal void Read(MemoryBuffer buffer)
				{
					groupId = buffer.ReadInt32();
					unlockedTrophies = buffer.ReadUInt32();
					unlockedPlatinum = buffer.ReadUInt32();
					unlockedGold = buffer.ReadUInt32();
					unlockedSilver = buffer.ReadUInt32();
					unlockedBronze = buffer.ReadUInt32();
					progressPercentage = buffer.ReadUInt32();
				}
			}

			/// <summary>
			/// Class containing extensive information of a group (static configuration, user progress and, if requested, icon).
			/// </summary>
			public class TrophyPackGroupResponse : ResponseBase
			{
				internal Icon icon = null;
				internal NpTrophyGroupDetails staticConfiguration;
				internal NpTrophyGroupData userProgress;

				/// <summary>
				/// The icon retrieved in case it was explicitely specified in the request
				/// </summary>
				public Icon Icon { get { return icon; } }

				/// <summary>
				/// Generic configuration written when the group was created
				/// </summary>
				public NpTrophyGroupDetails StaticConfiguration { get { return staticConfiguration; } }

				/// <summary>
				/// The user progress regarding the group
				/// </summary>
				public NpTrophyGroupData UserProgress { get { return userProgress; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.TrophyPackGroupBegin);

					icon = Icon.ReadAndCreate(readBuffer);
					staticConfiguration.Read(readBuffer);
					userProgress.Read(readBuffer);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.TrophyPackGroupEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function is used to obtain information about a group and the user interaction with it.
			/// </summary>
			/// <param name="request">Parameters needed to obtain the group information.</param>
			/// <param name="response">This response contains the return code and the icon (if requested), the configuration data, and the user specific data, of the group</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetTrophyPackGroup(GetTrophyPackGroupRequest request, TrophyPackGroupResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetTrophyPackGroup(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}
			#endregion

			#region Get Trophy Pack Trophy

			/// <summary>
			/// Trophy grade
			/// </summary>
			public enum TrophyGrade
			{
				/// <summary>Grade is unknown</summary>
				Unknown,      // SCE_NP_TROPHY_GRADE_UNKNOWN
				/// <summary>Platinum trophy: Trophy that is automatically unlocked by the system when all the required trophies are awarded</summary>
				Platinum,     // SCE_NP_TROPHY_GRADE_PLATINUM
				/// <summary>Gold trophy: Most difficult trophy to be awarded</summary>
				Gold,         // SCE_NP_TROPHY_GRADE_GOLD
				/// <summary>Silver trophy: Relatively difficult trophy to be awarded</summary>
				Silver,       // SCE_NP_TROPHY_GRADE_SILVER
				/// <summary>Bronze trophy: Most typical trophy that is easily awarded</summary>
				Bronze       // SCE_NP_TROPHY_GRADE_BRONZE
			}

			/// <summary>
			/// Trophy configuration data of a trophy
			/// </summary>
			public struct NpTrophyDetails // Maps to SceNpTrophyDetails
			{
				internal Int32 trophyId;
				internal TrophyGrade trophyGrade;
				internal Int32 groupId;
				internal bool hidden;
				internal string name;
				internal string description;

				/// <summary>
				/// Trophy ID
				/// </summary>
				public Int32 TrophyId { get { return trophyId; } }

				/// <summary>
				/// Grade of the trophy
				/// </summary>
				public TrophyGrade TrophyGrade { get { return trophyGrade; } }

				/// <summary>
				/// Trophy group ID to which this trophy belongs
				/// </summary>
				public Int32 GroupId { get { return groupId; } }

				/// <summary>
				/// Hidden flag
				/// </summary>
				public bool Hidden { get { return hidden; } }

				/// <summary>
				/// Name of the trophy
				/// </summary>
				public string Name { get { return name; } }

				/// <summary>
				/// Description of the trophy
				/// </summary>
				public string Description { get { return description; } }

				internal void Read(MemoryBuffer buffer)
				{
					trophyId = buffer.ReadInt32();
					trophyGrade = (TrophyGrade)buffer.ReadInt32();
					groupId = buffer.ReadInt32();
					hidden = buffer.ReadBool();
					buffer.ReadString(ref name);
					buffer.ReadString(ref description);
				}
			}

			/// <summary>
			/// Trophy record of a trophy
			/// </summary>
			public struct NpTrophyData // Maps to SceNpTrophyData
			{
				internal Int32 trophyId;
				internal bool unlocked;
				internal DateTime timestamp;    // SceRtcTick - Same tick value used by DateTime (number of ticks from 12:00:00 a.m. (UTC) January 1 0001)

				/// <summary>
				/// Trophy ID
				/// </summary>
				public Int32 TrophyId { get { return trophyId; } }

				/// <summary>
				/// Whether or not the trophy is unlocked
				/// </summary>
				public bool Unlocked { get { return unlocked; } }

				/// <summary>
				/// The time stamp of when the trophy was first unlocked, or 0 if the trophy has not been unlocked
				/// </summary>
				public DateTime Timestamp { get { return timestamp; } }

				internal void Read(MemoryBuffer buffer)
				{
					trophyId = buffer.ReadInt32();
					unlocked = buffer.ReadBool();
					timestamp = Core.ReadRtcTick(buffer);
				}
			}

			/// <summary>
			/// Class containing extensive information of a trophy (static configuration, user progress and, if requested, icon).
			/// </summary>
			public class TrophyPackTrophyResponse : ResponseBase
			{
				internal Icon icon = null;
				internal NpTrophyDetails staticConfiguration;
				internal NpTrophyData userProgress;

				/// <summary>
				/// The icon retrieved in case it was explicitely specified in the request
				/// </summary>
				public Icon Icon { get { return icon; } }

				/// <summary>
				/// Generic configuration written when the trophy was created
				/// </summary>
				public NpTrophyDetails StaticConfiguration { get { return staticConfiguration; } }

				/// <summary>
				/// The user progress regarding the trophy
				/// </summary>
				public NpTrophyData UserProgress { get { return userProgress; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.TrophyPackTrophyBegin);

					icon = Icon.ReadAndCreate(readBuffer);
					staticConfiguration.Read(readBuffer);
					userProgress.Read(readBuffer);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.TrophyPackTrophyEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function is used to obtain information about a trophy and the user interaction with it.
			/// </summary>
			/// <param name="request">Parameters needed to obtain the trophy information </param>
			/// <param name="response">This response contains the return code and the icon (if requested), the configuration data, and the user specific data, of the trophy</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetTrophyPackTrophy(GetTrophyPackTrophyRequest request, TrophyPackTrophyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetTrophyPackTrophy(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}
			#endregion

		}
	}
}
