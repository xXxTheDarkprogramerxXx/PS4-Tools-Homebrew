using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sony
{
    namespace NP
    {
        /// <summary>
        /// Contains server error data
        /// </summary>
        public class ServerErrorManaged
        {
            #region DLL Imports

            [DllImport("UnityNpToolkit2")]
            private static extern void PrxReadServerError(UInt32 responseId, FunctionTypes apiCalled,
                                                            out Int64 webApiNextAvailableTime, out Int32 httpStatusCode,
                                                         [MarshalAs(UnmanagedType.LPArray, SizeConst = 512), In, Out] byte[] jsonData, out APIResult result);

            #endregion

            const int JSON_DATA_MAX_LEN = 512;

            internal byte[] jsonData = new byte[JSON_DATA_MAX_LEN];
            internal Int64 webApiNextAvailableTime = 0;
            internal Int32 httpStatusCode = 0;

            /// <summary>
            /// In the event of an error, the JSON error string may be returned. This can be used to further investigate an issue during development
            /// </summary>
            public string JsonData { get { return System.Text.Encoding.UTF8.GetString(jsonData, 0, jsonData.Length); } }

            /// <summary>
            /// The next available time when a WebAPI request can be made, in seconds
            /// </summary>
            public Int64 WebApiNextAvailableTime { get { return webApiNextAvailableTime; } }

            /// <summary>
            /// The HTTP status code that was returned along with the JSON error
            /// </summary>
            public Int32 HttpStatusCode { get { return httpStatusCode; } }

            internal void ReadResult(UInt32 unqiueId, FunctionTypes apiCalled)
            {
                APIResult result;

                PrxReadServerError(unqiueId, apiCalled, out webApiNextAvailableTime, out httpStatusCode, jsonData, out result);

                if (result.RaiseException == true) throw new NpToolkitException(result);
            }
        }

        /// <summary>
        /// Base class that contains common Response data
        /// </summary>
        public class ResponseBase
        {
            #region DLL Imports

            [DllImport("UnityNpToolkit2")]
            private static extern void PrxReadResponseBase(UInt32 nptRequestId, FunctionTypes apiCalled, out Int32 returnCode, out bool locked, out APIResult result);

            [DllImport("UnityNpToolkit2")]
            private static extern void PrxReadResponseBaseLockedState(UInt32 nptRequestId, FunctionTypes apiCalled, out bool locked, out APIResult result);

            [DllImport("UnityNpToolkit2")]
            private static extern void PrxReadResponseCompleted(UInt32 nptRequestId, FunctionTypes apiCalled, out APIResult result);

            [DllImport("UnityNpToolkit2")]
            [return: MarshalAs(UnmanagedType.I1)]
            private static extern bool PrxReadHasServerError(UInt32 nptRequestId, FunctionTypes apiCalled, out APIResult result);

            [DllImport("UnityNpToolkit2")]
            private static extern void PrxMarshalResponse(UInt32 npRequestId, FunctionTypes apiCalled, out NpMemoryBuffer data, out APIResult result);

            #endregion

            internal Int32 returnCode;
            internal bool locked;

            internal ServerErrorManaged serverError;

            /// <summary>
            /// Gets the return code value of a Response object when it is ready. The return value will be a
            /// successful result or an error result. See specific functions to know which return codes may be returned
            /// </summary>
            /// <remarks>
            /// This will be an interger value, which will be an error code is less than 0.
            /// </remarks>
            public Int32 ReturnCodeValue { get { return returnCode; } }

            /// <summary>
            /// Gets the return code enum of a Response object when it is ready. The return value will be a
            /// successful result or an error result. See specific functions to know which return codes may be returned.
            /// </summary>
            public Core.ReturnCodes ReturnCode { get { return (Core.ReturnCodes)returnCode; } }

            /// <summary>
            /// Indicates if a Response object is being calculated or it is ready to be read.
            /// </summary>
            public bool Locked { get { return locked; } }

            /// <summary>
            /// Represents additional information provided when there has been a server error.
            /// </summary>
            public ServerErrorManaged ServerError { get { return serverError; } }

            internal ResponseBase()
            {
            }

            /// <summary>
            /// Does the return code contain an error code.
            /// </summary>
            public bool IsErrorCode
            {
                get
                {
                    if ((UInt32)returnCode >= 0x82000000 && (UInt32)returnCode <= 0x82FFFFFF)
                    {
                        return true;
                    }

                    return false;
                }
            }

            /// <summary>
            /// Is there a server error object available
            /// </summary>
            public bool HasServerError
            {
                get { return serverError != null; }
            }

            internal void PopulateFromNative(UInt32 nptRequestId, FunctionTypes apiCalled, RequestBase request)
            {
                // Read the results into this Response class. Virtual call 
                // to read the results from the different types of Response object
                ReadResult(nptRequestId, apiCalled, request);

                // Once native response object has been read, it can be released in the native code.
                // Subsequent attempts to read the native Response object will fail.
                APIResult result;

                PrxReadResponseCompleted(nptRequestId, apiCalled, out result);

                if (result.RaiseException == true) throw new NpToolkitException(result);
            }

            /// <summary>
            /// Read the response base data from the native plug-in
            /// </summary>
            /// <param name="id">The request id.</param>
            /// <param name="apiCalled">The Api called for the request.</param>
            /// <param name="request">The Request object that started the request. In the case of Notifications this will be null.</param>
            protected internal virtual void ReadResult(UInt32 id, FunctionTypes apiCalled, RequestBase request)
            {
                APIResult result;
                bool tempLockStatus; // Don't pass in the this.locked bool as this response instance needs to be marked as locked until the copy is complete

                PrxReadResponseBase(id, apiCalled, out returnCode, out tempLockStatus, out result);

                if (result.RaiseException == true) throw new NpToolkitException(result);

                if (PrxReadHasServerError(id, apiCalled, out result) == true)
                {
                    serverError = new ServerErrorManaged();
                    serverError.ReadResult(id, apiCalled);
                }

                if (result.RaiseException == true) throw new NpToolkitException(result);

                locked = tempLockStatus;
            }

            internal void UpdateAsyncState(UInt32 nptRequestId, FunctionTypes apiCalled)
            {
                APIResult result;
                PrxReadResponseBaseLockedState(nptRequestId, apiCalled, out locked, out result);
                if (result.RaiseException == true) throw new NpToolkitException(result);
            }

            internal MemoryBuffer BeginReadResponseBuffer(UInt32 id, FunctionTypes apiCalled, out APIResult result)
            {
                NpMemoryBuffer data = new NpMemoryBuffer();

                PrxMarshalResponse(id, apiCalled, out data, out result);

                if (result.RaiseException == true) throw new NpToolkitException(result);

                MemoryBuffer readBuffer = new MemoryBuffer(data);

                readBuffer.CheckStartMarker();  // Will throw exception if start marker isn't present in the buffer.

                return readBuffer;
            }

            internal void EndReadResponseBuffer(MemoryBuffer readBuffer)
            {
                readBuffer.CheckEndMarker();
            }

            /// <summary>
            /// Generate a string containing the Hex return code and if known the Return code name
            /// taken from the Core.ReturnCodes enum.
            /// As some of the enums share the same code the returned string will depend on the
            /// API function used that generated the return code;
            /// </summary>
            /// <param name="apiCalled">The API that generated the return code. </param>
            /// <returns>The constructed string with the Hex and return code name.</returns>
            /// <remarks>
            /// By default apiCalled will be FunctionType.invalid. This will return the first Core.ReturnCodes enum that matches the return code value so may produce an incorrect string. 
            /// </remarks>
            public string ConvertReturnCodeToString(FunctionTypes apiCalled)
            {
                string output = "(0x" + returnCode.ToString("X8") + ")";

                Core.ReturnCodes rc = (Core.ReturnCodes)returnCode;

                if (apiCalled != FunctionTypes.Invalid)
                {
                    // Deal with duplicate enums here
                    // Current Duplicates:
                    // DIALOG_RESULT_OK	= 0x00000000  // Conflicts with SUCCESS
                    // TROPHY_PLATINUM_UNLOCKED	= 0x00000001,	// Conflicts with DIALOG_RESULT_USER_CANCELED

                    switch (apiCalled)
                    {
                        case FunctionTypes.CommerceDisplayCategoryBrowseDialog:
                        case FunctionTypes.CommerceDisplayProductBrowseDialog:
                        case FunctionTypes.CommerceDisplayVoucherCodeInputDialog:
                        case FunctionTypes.CommerceDisplayCheckoutDialog:
                        case FunctionTypes.CommerceDisplayJoinPlusDialog:
                        case FunctionTypes.CommerceDisplayDownloadListDialog:
                        case FunctionTypes.FriendsDisplayFriendRequestDialog:
                        case FunctionTypes.FriendsDisplayBlockUserDialog:
                        case FunctionTypes.MessagingDisplayReceivedGameDataMessagesDialog:
                        case FunctionTypes.NpUtilsDisplaySigninDialog:
                        case FunctionTypes.SessionDisplayReceivedInvitationsDialog:
                        case FunctionTypes.TrophyDisplayTrophyListDialog:
                        case FunctionTypes.UserProfileDisplayUserProfileDialog:
                        case FunctionTypes.UserProfileDisplayGriefReportingDialog:
                            {
                                if (rc == Core.ReturnCodes.DIALOG_RESULT_OK)
                                {
                                    output += " (DIALOG_RESULT_OK) ";
                                    return output;
                                }
                                else if (rc == Core.ReturnCodes.DIALOG_RESULT_USER_CANCELED)
                                {
                                    output += " (DIALOG_RESULT_USER_CANCELED) ";
                                    return output;
                                }
                            }
                            break;

                        case FunctionTypes.TrophyUnlock:
                            {
                                if (rc == Core.ReturnCodes.TROPHY_PLATINUM_UNLOCKED)
                                {
                                    output += " (TROPHY_PLATINUM_UNLOCKED) ";
                                    return output;
                                }
                            }
                            break;

                        default:
                            if (returnCode == 0)
                            {
                                return output += " (SUCCESS) ";
                            }
                            break;
                    }
                }

                if (Enum.IsDefined(typeof(Core.ReturnCodes), rc) == true)
                {
                    output += " (" + rc.ToString() + ") ";
                }
                else
                {
                    output += " (UNKNOWN) ";
                }

                return output;
            }
        }

        /// <summary>
        /// Contains a list of pending asynchronous requests.
        /// </summary>
        internal static class PendingAsyncResponseList
        {
            // Contains a dictionary of response for quick lookup
            static Dictionary<UInt32, ResponseBase> responseLookup = new Dictionary<UInt32, ResponseBase>();

            //static List<ResponseBase> responses = new List<ResponseBase>();
            private static Object syncObject = new Object();

            static public void AddResponse(UInt32 npRequestId, ResponseBase response)
            {
                lock (syncObject)
                {
                    responseLookup.Add(npRequestId, response);
                }
            }

            static public ResponseBase FindAndRemoveResponse(UInt32 npRequestId)
            {
                lock (syncObject)
                {
                    ResponseBase response;

                    if (responseLookup.TryGetValue(npRequestId, out response))
                    {
                        responseLookup.Remove(npRequestId);
                        return response;
                    }

                    return null;
                }
            }
        }
    }
}
