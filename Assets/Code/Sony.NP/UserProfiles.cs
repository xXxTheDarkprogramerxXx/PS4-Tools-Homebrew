using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Sony
{
	namespace NP
	{
		/// <summary>
		/// User Profile service related functionality.
		/// </summary>
		public class UserProfiles
		{
			#region DLL Imports

			[DllImport("UnityNpToolkit2")]
			private static extern void PrxGetLocalLoginUserIds([MarshalAs(UnmanagedType.LPArray, SizeConst = LocalUsers.MaxLocalUsers), Out] LocalLoginUserId[] users, Int32 maxSize, out APIResult result);

			// User Profiles
			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetNpProfiles(GetNpProfilesRquest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetVerifiedAccountsForTitle(GetVerifiedAccountsForTitleRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxDisplayUserProfileDialog(DisplayUserProfileDialogRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxDisplayGriefReportingDialog(DisplayGriefReportingDialogRequest request, out APIResult result);

			#endregion

			#region Get Local Users

			/// <summary>
			/// Contains details about all local users, up to a maximum of <see cref="MaxLocalUsers"/>
			/// </summary>
			public class LocalUsers
			{
				/// <summary>
				/// The maximum number of local users
				/// </summary>
				public const int MaxLocalUsers = 4;

				internal LocalLoginUserId[] localUsers = new LocalLoginUserId[MaxLocalUsers];

				/// <summary>
				/// The array of local users, with a fixed size of <see cref="MaxLocalUsers"/>
				/// </summary>
				/// <remarks>
				/// Returns a full array of local users. The array may contain invalid users as the full compliment of users might not be logged into the PS4 system.
				/// Use <see cref="Core.UserServiceUserId.UserIdInvalid"/> to test if the local user information is valid.
				/// </remarks>
				public LocalLoginUserId[] LocalUsersIds { get { return localUsers; } }
			}

			/// <summary>
			/// Mapping for a local user from their local service id  to their account id. 
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public struct LocalLoginUserId
			{
				internal Core.UserServiceUserId userId;
				internal Core.NpAccountId accountId;
				internal Int32 sceErrorCode;

				/// <summary>
				/// The local user id.
				/// </summary>
				public Core.UserServiceUserId UserId { get { return userId; } }

				/// <summary>
				/// The unique 64bit account id if the user is logged into PSN
				/// </summary>
				public Core.NpAccountId AccountId { get { return accountId; } }

				/// <summary>
				/// Any error code returned by the system when checking for the users Account Id using sceNpGetAccountIdA. Use this to detemine if it retuned errors such as SCE_NP_ERROR_SIGNED_OUT or SCE_NP_ERROR_NOT_SIGNED_UP
				/// </summary>
				public Int32 SceErrorCode { get { return sceErrorCode; } }
			}

			/// <summary>
			/// Fill in the local users. This is a synchronous method and will return the results immediately. 
			/// </summary>
			/// <param name="users">The <see cref="LocalUsers"/> instance to fill in.</param>
			/// <exception cref="NpToolkitException">Will throw an exception if one or more of the users in the array has an error code regarding the status of the user.</exception>
			public static void GetLocalUsers(LocalUsers users)
			{
				APIResult result;

				PrxGetLocalLoginUserIds(users.localUsers, LocalUsers.MaxLocalUsers, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);
			}

			#endregion

			#region NpProfiles Response
			/// <summary>
			/// Contains the profile information of the target users.
			/// </summary>
			public class NpProfilesResponse : ResponseBase
			{
				internal Profiles.Profile[] profiles;

				/// <summary>
				/// The array of profiles retrieved
				/// </summary>
				public Profiles.Profile[] Profiles { get { return profiles; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NpProfilesBegin);

					UInt32 numNpProfiles = readBuffer.ReadUInt32();

					profiles = new Profiles.Profile[numNpProfiles];

					for (int i = 0; i < numNpProfiles; i++)
					{
						profiles[i] = new Profiles.Profile();
						profiles[i].Read(readBuffer);
					}

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NpProfilesEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}
			#endregion

			#region Get NpProfiles

			/// <summary>
			/// Parameters to obtain profile information of a set of users provided as input.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class GetNpProfilesRquest : RequestBase
			{
				/// <summary>
				/// The maximum size of the <c><i>accountIds</i></c> array
				/// </summary>
				public const int MAX_SIZE_ACCOUNT_IDS = 50;

				///A list of users to retrieve profile information from
				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SIZE_ACCOUNT_IDS)]
				internal Core.NpAccountId[] accountIds;

				internal UInt32 numValidAccountIds;

				/// <summary>
				/// A list of users to retrieve profile information from
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is more than <see cref="MAX_SIZE_ACCOUNT_IDS"/>.</exception>
				public Core.NpAccountId[] AccountIds
				{
					get
					{
						if (numValidAccountIds == 0) return null;

						Core.NpAccountId[] ids = new Core.NpAccountId[numValidAccountIds];

						Array.Copy(accountIds, ids, numValidAccountIds);

						return ids;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_SIZE_ACCOUNT_IDS)
							{
								throw new NpToolkitException("The size of the Account ids array is more than " + MAX_SIZE_ACCOUNT_IDS);
							}

							value.CopyTo(accountIds, 0);
							numValidAccountIds = (UInt32)value.Length;
						}
						else
						{
							numValidAccountIds = 0;
						}
					}
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetNpProfilesRquest"/> class.
				/// </summary>
				public GetNpProfilesRquest()
					: base(ServiceTypes.UserProfile, FunctionTypes.UserProfileGetNpProfiles)
				{
					accountIds = new Core.NpAccountId[MAX_SIZE_ACCOUNT_IDS];
					numValidAccountIds = 0;
				}
			}

			/// <summary>
			/// Function to obtain the user profile information for a set ofusers specified in the request.
			/// </summary>
			/// <param name="request">Parameters with the target users to get the profiles information from.</param>
			/// <param name="response">This response contains the information of the profiles for the previously specified users as response data, and also contains the return code.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetNpProfiles(GetNpProfilesRquest request, NpProfilesResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetNpProfiles(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}
			#endregion

			#region Get Verified Accounts for Title

			/// <summary>
			/// Obtain profile information of Verified Accounts related to the current title.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class GetVerifiedAccountsForTitleRequest : RequestBase
			{
			
				internal UInt32 limit;

				/// <summary>
				/// The maximum number of profiles from Verified Accounts to retrieve. Defaults to 10
				/// </summary>
				public UInt32 Limit 
				{ 
					get { return limit; }
					set { limit = value; } 
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetVerifiedAccountsForTitleRequest"/> class.
				/// </summary>
				public GetVerifiedAccountsForTitleRequest()
					: base(ServiceTypes.UserProfile, FunctionTypes.UserProfileGetVerifiedAccountsForTitle)
				{
					limit = 10;
				}
			}

			/// <summary>
			/// Function to obtain the profile information for Verified Accounts of the running application (Title Id).
			/// </summary>
			/// <param name="request">Parameters with the limit of the maximum number of Verified Accounts linked to a title to retrieve in one call</param>
			/// <param name="response">This response contains the information of the verified account profiles linked to the title, and also contains the return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetVerifiedAccountsForTitle(GetVerifiedAccountsForTitleRequest request, NpProfilesResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetVerifiedAccountsForTitle(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Display User Profile Dialog

			/// <summary>
			/// Parameters passed to open the user profile dialog of a target user.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class DisplayUserProfileDialogRequest : RequestBase
			{
				internal Core.NpAccountId targetAccountId;

				/// <summary>
				/// The user profile dialog will be opened for this account
				/// </summary>
				public Core.NpAccountId TargetAccountId
				{
					get { return targetAccountId; }
					set { targetAccountId = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="DisplayUserProfileDialogRequest"/> class.
				/// </summary>
				public DisplayUserProfileDialogRequest()
					: base(ServiceTypes.UserProfile, FunctionTypes.UserProfileDisplayUserProfileDialog)
				{
					
				}
			}

			/// <summary>
			/// This function opens the System User Profile Dialog for a specified target user. The user can be the calling user in case its own profile wants to be opened.
			/// </summary>
			/// <param name="request">Parameters with the target user and reasons to report </param>
			/// <param name="response">This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int DisplayUserProfileDialog(DisplayUserProfileDialogRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxDisplayUserProfileDialog(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Display Grief Reporting Dialog

			/// <summary>
			/// Parameters passed to open the grief reporting dialog of a target user.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class DisplayGriefReportingDialogRequest : RequestBase
			{
				///The maximum number of profiles from Verified Accounts to retrieve. Defaults to 10   
				public Core.NpAccountId targetAccountId;

				///true when the Online Id should be reported
				[MarshalAs(UnmanagedType.I1)]
				public bool reportOnlineId;

				///true when the Name should be reported
				[MarshalAs(UnmanagedType.I1)]
				public bool reportName;

				///true when the Picture of the profile should be reported
				[MarshalAs(UnmanagedType.I1)]
				public bool reportPicture;

				///true when the About Me section of the profile should be reported
				[MarshalAs(UnmanagedType.I1)]
				public bool reportAboutMe;

				/// <summary>
				/// The grief reporting dialog will be opened to report this account
				/// </summary>
				public Core.NpAccountId TargetAccountId
				{
					get { return targetAccountId; }
					set { targetAccountId = value; }
				}

				/// <summary>
				/// True when the Online Id should be reported. Defaults to false.
				/// </summary>
				public bool ReportOnlineId
				{
					get { return reportOnlineId; }
					set { reportOnlineId = value; }
				}

				/// <summary>
				/// True when the Name should be reported. Defaults to false.
				/// </summary>
				public bool ReportName
				{
					get { return reportName; }
					set { reportName = value; }
				}

				/// <summary>
				/// True when the Picture should be reported. Defaults to false.
				/// </summary>
				public bool ReportPicture
				{
					get { return reportPicture; }
					set { reportPicture = value; }
				}

				/// <summary>
				/// True when the About Me section should be reported. Defaults to false.
				/// </summary>
				public bool ReportAboutMe
				{
					get { return reportAboutMe; }
					set { reportAboutMe = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="DisplayGriefReportingDialogRequest"/> class.
				/// </summary>
				public DisplayGriefReportingDialogRequest()
					: base(ServiceTypes.UserProfile, FunctionTypes.UserProfileDisplayGriefReportingDialog)
				{

				}
			}

			/// <summary>
			/// This function opens directly the Grief Reporting section of the System User Profile Dialog for a specified target user.
			/// </summary>
			/// <param name="request">Parameters with the target user and reasons to report </param>
			/// <param name="response">This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int DisplayGriefReportingDialog(DisplayGriefReportingDialogRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked.");
				}

				if (request.reportAboutMe == false && 
					request.reportName == false && 
					request.reportOnlineId == false && 
					request.reportAboutMe == false)
				{
					throw new NpToolkitException("It is mandatory to specify at least one reason for the report.");
				}

				int ret = PrxDisplayGriefReportingDialog(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion
		}
	}
}
