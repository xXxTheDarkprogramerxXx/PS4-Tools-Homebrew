using System;
using System.Runtime.InteropServices;

namespace Sony
{
	namespace NP
	{
		/// <summary>
		/// Np Utils service related functionality.
		/// </summary>
		public class NpUtils
		{
			#region DLL Imports

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxSetTitleIdForDevelopment(SetTitleIdForDevelopmentRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxDisplaySigninDialog(DisplaySigninDialogRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxCheckAvailablity(CheckAvailablityRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxCheckPlus(CheckPlusRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetParentalControlInfo(GetParentalControlInfoRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern void PrxNotifyPlusFeature(Int32 userId, UInt64 features, out APIResult result);

			#endregion

			#region Requests

			/// <summary>
			/// This class is passed through to the library to set the Title
			/// Id and Title Secret that will be used in DevKits in
			/// Development Mode.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
			public class SetTitleIdForDevelopmentRequest : RequestBase
			{
				const Int32 SCE_NP_TITLE_ID_LEN = 12;

				///The title id to be set
				public string titleId;
				///The title secret to be set. This is the same string set in the .txt file given on DevNet
				public string titleSecretString;
				///The size of titleSecretString
				public UInt32 titleSecretStringSize;	

				/// <summary>
				/// Initializes a new instance of the <see cref="SetTitleIdForDevelopmentRequest"/> class.
				/// </summary>
				public SetTitleIdForDevelopmentRequest()
					: base(ServiceTypes.NpUtils, FunctionTypes.NpUtilsSetTitleIdForDevelopment)
				{
					titleId = "";
					titleSecretString = "";
					titleSecretStringSize = 0;
				}
			}

			/// <summary>
			/// Parameters passed to open the sign in dialog.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class DisplaySigninDialogRequest : RequestBase
			{
				/// <summary>
				/// Initializes a new instance of the <see cref="DisplaySigninDialogRequest"/> class.
				/// </summary>
				public DisplaySigninDialogRequest()
					: base(ServiceTypes.NpUtils, FunctionTypes.NpUtilsDisplaySigninDialog)
				{
				}
			}

			/// <summary>
			/// This class is passed through to the library to check that the calling user can communicate to PSN servers.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class CheckAvailablityRequest : RequestBase
			{
				/// <summary>
				/// Initializes a new instance of the <see cref="CheckAvailablityRequest"/> class.
				/// </summary>
				public CheckAvailablityRequest()
					: base(ServiceTypes.NpUtils, FunctionTypes.NpUtilsCheckAvailability)
				{
				}
			}

			/// <summary>
			/// PlayStation®Plus feature usage entitlement check
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class CheckPlusRequest : RequestBase
			{
				// Default to SCE_NP_PLUS_FEATURE_REALTIME_MULTIPLAY. This is also the only flag so it is hard coded at the moment.
				// This is provide for completeness as additional flags maybe added in the future to the SDK.
				internal UInt64 features; 

				/// <summary>
				/// Initializes a new instance of the <see cref="CheckPlusRequest"/> class.
				/// </summary>
				public CheckPlusRequest()
					: base(ServiceTypes.NpUtils, FunctionTypes.NpUtilsCheckPlus)
				{
					features = 0x1;
				}
			}

			/// <summary>
			/// Get parental control information
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class GetParentalControlInfoRequest : RequestBase
			{
				/// <summary>
				/// Initializes a new instance of the <see cref="GetParentalControlInfoRequest"/> class.
				/// </summary>
				public GetParentalControlInfoRequest()
					: base(ServiceTypes.NpUtils, FunctionTypes.NpUtilsGetParentalControlInfo)
				{

				}
			}


			#endregion

			#region Set TitleId For Developement

			/// <summary>
			/// This function sets a Title ID and Title Secret to be used during development (on a DevKit in Development Mode).
			/// </summary>
			/// <param name="request">The Title Id and Title Secret to be set</param>
			/// <param name="response">This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int SetTitleIdForDevelopment(SetTitleIdForDevelopmentRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxSetTitleIdForDevelopment(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Display Signin Dialog

			/// <summary>
			/// This function opens the System Sign-In Dialog if the user is
			/// not signed in to PlayStation Network.
			/// </summary>
			/// <param name="request">Parameters needed to open the Sign-In Dialog </param>
			/// <param name="response">This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int DisplaySigninDialog(DisplaySigninDialogRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxDisplaySigninDialog(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Check Availablity

			/// <summary>
			/// This function checks the calling user passes all restrictions necessary to access the PSN servers.
			/// </summary>
			/// <param name="request">Parameters needed to perform the checking operation </param>
			/// <param name="response"> This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int CheckAvailablity(CheckAvailablityRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxCheckAvailablity(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Check Plus

			/// <summary>
			/// PlayStation Plus feature usage entitlement check results
			/// </summary>
			public class CheckPlusResponse : ResponseBase
			{
				internal bool authorized;

				/// <summary>
				/// PlayStation Plus feature usage entitlement check results
				/// </summary>
				public bool Authorized
				{
					get { return authorized; }
				}

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.CheckPlusBegin);

					authorized = readBuffer.ReadBool();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.CheckPlusEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function checks the calling user has a PS plus account.
			/// </summary>
			/// <remarks>
			/// This function determines whether a user has PlayStation®Plus feature usage entitlements or not, it cannot be used for determining whether a user has joined the PlayStation®Plus service or not. 
			/// There are cases where a user that has not joined the PlayStation®Plus service is granted usage entitlements for real-time multiplayer play; in such cases this function will return results that state that entitlements are present.
			/// Do not overinterpret the results returned by this function.
			/// This function is an extension to the NpToolkit library. As such it is possible for the request to finish before other pending requests as this is handled seperately to other NpToolkit request.
			/// </remarks>
			/// <param name="request">Parameters needed to perform the checking operation </param>
			/// <param name="response">This response contains the return code and CheckPlus results.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int CheckPlus(CheckPlusRequest request, CheckPlusResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxCheckPlus(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Get Parental Control Info

			/// <summary>
			/// Get parental control information
			/// </summary>
			public class GetParentalControlInfoResponse : ResponseBase
			{
				int age;

				internal bool contentRestriction; // This is not used in Sony lib, so included for completness, but no public interface exposed.
				internal bool chatRestriction;
				internal bool ugcRestriction;

				/// <summary>
				/// Users age
				/// </summary>
				public int Age
				{
					get { return age; }
				}

				/// <summary>
				/// Chat restriction
				/// </summary>
				public bool ChatRestriction
				{
					get { return chatRestriction; }
				}

				/// <summary>
				/// User-generated media restriction
				/// </summary>
				public bool UGCRestriction
				{
					get { return ugcRestriction; }
				}

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.GetParentalControlInfoBegin);

					age = readBuffer.ReadInt32();

					// contentRestriction is not used in Sony lib, so included for completness, but no public interface exposed.
					contentRestriction = readBuffer.ReadBool();

					chatRestriction = readBuffer.ReadBool();
					ugcRestriction = readBuffer.ReadBool();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.GetParentalControlInfoEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// Get parental control information
			/// </summary>
			/// <remarks>
			/// This function obtains the parental control information. In other words, 
			/// it obtains the chat restrictions and usage restriction settings for user-generated media. 
			/// This function carries out synchronous or asynchronous processing depending on the request argument.
			/// </remarks>
			/// <param name="request">Parameters needed to perform the operation </param>
			/// <param name="response">This response contains the return code and CheckPlus results.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetParentalControlInfo(GetParentalControlInfoRequest request, GetParentalControlInfoResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetParentalControlInfo(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			/// <summary>
			/// Notification for use of PlayStation®Plus features
			/// </summary>
			/// <param name="userId">User ID</param>
			public static void NotifyPlusFeature(Core.UserServiceUserId userId)
			{
				APIResult result;

				// Features param is as set to  SCE_NP_PLUS_FEATURE_REALTIME_MULTIPLAY = 0x01
				PrxNotifyPlusFeature(userId.id, 0x01, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);
			}

			#endregion

			#region Notifications

			/// <summary>
			/// Defines the possible sign in states a user can have on a PlayStation 4.
			/// </summary>
			public enum SignInState 
			{
				/// <summary>Representation of an unknown signed in state. It shouldn't happen</summary>
				unknown = 0, // SCE_NP_STATE_UNKNOWN,		
				/// <summary>Representation of the signed out state of a user</summary>
				signedOut = 1,  // SCE_NP_STATE_SIGNED_OUT,	
				/// <summary>Representation of the signed in state of a user</summary>
				signedIn = 2, // = SCE_NP_STATE_SIGNED_IN		
			};

			/// <summary>
			/// Defines the possible log in states a user can have on a PlayStation 4.
			/// </summary>
			public enum LogInState 
			{
				/// <summary>Representation of the logged in state of a user</summary>
				loggedIn = 0, //SCE_USER_SERVICE_EVENT_TYPE_LOGIN,	
				/// <summary>Representation of the logged out state of a user</summary>
				loggedOut = 1, //SCE_USER_SERVICE_EVENT_TYPE_LOGOUT,	
				/// <summary>Representation of an unknown logged in state. It shouldn't happen</summary>
				unknown = 2										
			};

			/// <summary>
			/// Defines the type of change that has triggered this notification.
			/// </summary>
			public enum StateChanged 
			{
				/// <summary>No states have changed. This should never happen as otherwise there would be no notification</summary>
				none = 0,
				/// <summary>The signed in state has changed. See SignInState</summary>
				signedInState,
				/// <summary>The logged in state has changed. See LoggedInState</summary>
				loggedInState		
			};

			/// <summary>
			/// This notification represents a change in a user logged in or signed in state.
			/// </summary>
			public class UserStateChangeResponse : ResponseBase
			{
				internal Core.UserServiceUserId userId;
				internal SignInState currentSignInState;
				internal LogInState currentLogInState;
				internal StateChanged stateChanged;

				/// <summary>
				/// The user Id whose state has changed		
				/// </summary>
				public Core.UserServiceUserId UserId { get { return userId; } }

				/// <summary>
				/// The sign in state of the user in userId after the change
				/// </summary>
				public SignInState CurrentSignInState { get { return currentSignInState; } }

				/// <summary>
				/// The log in state of the user in userId after the change
				/// </summary>
				public LogInState CurrentLogInState { get { return currentLogInState; } }

				/// <summary>
				/// The state that has changed
				/// </summary>
				public StateChanged StateChanged { get { return stateChanged; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.UserStateChangeBegin);

					userId = readBuffer.ReadInt32();
					currentSignInState = (SignInState)readBuffer.ReadInt32();
					currentLogInState = (LogInState)readBuffer.ReadInt32();
					stateChanged = (StateChanged)readBuffer.ReadInt32();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.UserStateChangeEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			#endregion
		}
	}
}
