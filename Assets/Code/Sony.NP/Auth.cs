using System;
using System.Runtime.InteropServices;

namespace Sony
{
	namespace NP
	{
		/// <summary>
		/// Authentication service related functionality.
		/// </summary>
		public class Auth
		{
			#region DLL Imports

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetAuthCode(GetAuthCodeRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetIdToken(GetIdTokenRequest request, out APIResult result);

			#endregion

			#region Common

			/// <summary>
			/// This structure represents the client ID.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct NpClientId
			{
				/// <summary>
				/// The maximum size the of the client id
				/// </summary>
				public const int NP_CLIENT_ID_MAX_LEN = 128;  // SCE_NP_CLIENT_ID_MAX_LEN

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = NP_CLIENT_ID_MAX_LEN + 1)]
				internal string id;

				/// <summary>
				/// Client ID string (specify the value issued by the PlayStation®4 Developer Network)
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the string is more than <see cref="NP_CLIENT_ID_MAX_LEN"/> characters.</exception>
				public string Id
				{
					get { return id; }
					set
					{
						if (value.Length > NP_CLIENT_ID_MAX_LEN)
						{
							throw new NpToolkitException("The size of the string is more than " + NP_CLIENT_ID_MAX_LEN + " characters.");
						}
						id = value;
					}
				}
			}

			/// <summary>
			/// This structure represents the client secret.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct NpClientSecret
			{
				/// <summary>
				/// The maximum size of the client secret string
				/// </summary>
				public const int NP_CLIENT_SECRET_MAX_LEN = 256;  // SCE_NP_CLIENT_SECRET_MAX_LEN

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = NP_CLIENT_SECRET_MAX_LEN + 1)]
				internal string secret;

				/// <summary>
				/// Client secret string (specify the value issued by the PlayStation®4 Developer Network)
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the string is more than <see cref="NP_CLIENT_SECRET_MAX_LEN"/> characters.</exception>
				public string Secret
				{
					get { return secret; }
					set
					{
						if (value.Length > NP_CLIENT_SECRET_MAX_LEN)
						{
							throw new NpToolkitException("The size of the string is more than " + NP_CLIENT_SECRET_MAX_LEN + " characters.");
						}
						secret = value;
					}
				}
			}

			#endregion

			#region Requests		

			/// <summary>
			/// Parameters passed to get the auth. code from the PSN server to send it to the application server.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
			public class GetAuthCodeRequest : RequestBase
			{
				/// <summary>
				/// The maximum size the scope string can be
				/// </summary>
				public const int MAX_SIZE_SCOPE = 511;

				internal NpClientId clientId;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_SIZE_SCOPE + 1)]
				internal string scope;

				/// <summary>
				/// Client Id provided on DevNet upon registration of the application server
				/// </summary>
				public NpClientId ClientId
				{
					get { return clientId; }
					set { clientId = value; }
				}

				/// <summary>
				/// Depending on the scope value, the auth.code and its token will be able to access or not certain information
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the path is more than <see cref="MAX_SIZE_SCOPE"/> characters.</exception>
				public string Scope
				{
					get { return scope; }
					set
					{
						if (value.Length > MAX_SIZE_SCOPE)
						{
							throw new NpToolkitException("The size of the string is more than " + MAX_SIZE_SCOPE + " characters.");
						}
						scope = value;
					}
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetAuthCodeRequest"/> class.
				/// </summary>
				public GetAuthCodeRequest()
					: base(ServiceTypes.Auth, FunctionTypes.AuthGetAuthCode)
				{

				}
			}

			/// <summary>
			/// Parameters passed to get the Id Token from the PSN server to send it to the application server.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class GetIdTokenRequest : RequestBase
			{
				/// <summary>
				/// The maximum size the scope string can be
				/// </summary>
				public const int MAX_SIZE_SCOPE = GetAuthCodeRequest.MAX_SIZE_SCOPE;

				internal NpClientId clientId;
				internal NpClientSecret clientSecret;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_SIZE_SCOPE + 1)]
				internal string scope;

				/// <summary>
				/// Client Id provided on DevNet upon registration of the application server
				/// </summary>
				public NpClientId ClientId
				{
					get { return clientId; }
					set { clientId = value; }
				}

				/// <summary>
				/// Client Secret provided on DevNet upon registration of the application server and services
				/// </summary>
				public NpClientSecret ClientSecret
				{
					get { return clientSecret; }
					set { clientSecret = value; }
				}

				/// <summary>
				/// Depending on the scope value, the token will contain or not contain information
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the path is more than <see cref="MAX_SIZE_SCOPE"/> characters.</exception>
				public string Scope
				{
					get { return scope; }
					set
					{
						if (value.Length > MAX_SIZE_SCOPE)
						{
							throw new NpToolkitException("The size of the string is more than " + MAX_SIZE_SCOPE + " characters.");
						}
						scope = value;
					}
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetIdTokenRequest"/> class.
				/// </summary>
				public GetIdTokenRequest()
					: base(ServiceTypes.Auth, FunctionTypes.AuthGetIdToken)
				{

				}
			}

			#endregion

			#region Get Auth Code

			/// <summary>
			/// Enum representing the three type of environments the validated code can be from.
			/// </summary>
			public enum IssuerIdType
			{
				/// <summary> The environment is not recognized. It should never be returned </summary>
				Invalid = -1,
				/// <summary> Environment used while the application is being developed </summary>
				Development = 1,
				/// <summary> Environment used while the application is being tested by Format QA after submission </summary>
				Certification = 8,
				/// <summary> Environment used by final users for applications that are already released </summary>
				Live = 256
			}

			/// <summary>
			/// Response object containing the authorization code that will be sent to the application server.
			/// </summary>
			public class AuthCodeResponse : ResponseBase
			{
				internal string authCode;
				internal IssuerIdType issuerId;

				/// <summary>
				/// Auth. Code returned by the PSN server. It needs to be sent to the application server to obtain a valid token and be able to communicate with PSN servers
				/// </summary>
				public string AuthCode { get { return authCode; } }

				/// <summary>
				/// The environment the <see cref="AuthCode"/> is from
				/// </summary>
				public IssuerIdType IssuerId { get { return issuerId; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.AuthCodeBegin);

					readBuffer.ReadString(ref authCode);

					issuerId = (IssuerIdType)readBuffer.ReadUInt32();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.AuthCodeEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// Gets an authorization code from the PSN servers for the calling user.
			/// </summary>
			/// <param name="request">The information to obtain the authorization code and environment </param>
			/// <param name="response">This response contains the return code and the auth.code information and its environment</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetAuthCode(GetAuthCodeRequest request, AuthCodeResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetAuthCode(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Get Id Token

			/// <summary>
			/// Response object containing the Id token that will be sent to the application server.
			/// </summary>
			public class IdTokenResponse : ResponseBase
			{
				internal string idToken;

				/// <summary>
				/// The token to be sent to the application server to authenticate the user
				/// </summary>
				public string IdToken { get { return idToken; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.IdTokenBegin);

					readBuffer.ReadString(ref idToken);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.IdTokenEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// Gets an Id Token from the PSN servers for the calling user.
			/// </summary>
			/// <param name="request">The information to obtain the id token </param>
			/// <param name="response">This response contains the id token obtained by the PSN server for the calling user</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetIdToken(GetIdTokenRequest request, IdTokenResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetIdToken(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion
		}
	}
}
