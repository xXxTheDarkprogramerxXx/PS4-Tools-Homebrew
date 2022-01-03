using System;
using System.Runtime.InteropServices;

namespace Sony
{
    namespace NP
    {
        /// <summary>
        /// Core NpToolkit classes and structures.
        /// </summary>
        public class Core
        {
            #region DLL Imports

            [DllImport("UnityNpToolkit2")]
            private static extern int PrxTerminateService(TerminateServiceRequest request, out APIResult result);

            #endregion

            #region Requests

            /// <summary>
            /// Represents the parameters passed when terminating a service of the library.
            /// </summary>
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public class TerminateServiceRequest : RequestBase
            {
                internal ServiceTypes service;

                /// <summary>
                /// The service to be terminated.
				/// </summary>
                public ServiceTypes Service
                {
                    get { return service; }
                    set { service = value; }
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="TerminateServiceRequest"/> class.
                /// </summary>
                public TerminateServiceRequest()
                    : base(ServiceTypes.Core, FunctionTypes.CoreTerminateService)
                {

                }
            }

            #endregion

            #region Terminate Service

            /// <summary>
            /// Terminates a ToolkitNp2 library service.
            /// </summary>
            /// <param name="request"> The parameter specifying the service to be terminated.  </param>
            /// <param name="response"> This response does not have data, only return code</param>
            /// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
            /// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
            public static int TerminateService(TerminateServiceRequest request, Core.EmptyResponse response)
            {
                APIResult result;

                if (response.locked == true)
                {
                    throw new NpToolkitException("Response object is already locked");
                }

                int ret = PrxTerminateService(request, out result);

                if (result.RaiseException == true) throw new NpToolkitException(result);

                RequestBase.FinaliseRequest(request, response, ret);

                return ret;
            }

            #endregion

            #region Common Enums

            /// <summary>
			///  Representation of a boolean with an extra state to identify a value has not been provided.
			/// </summary>
			public enum OptionalBoolean
            {
                /// <summary> Not set </summary>
                notSet = 0,
                /// <summary> True </summary>	
                setTrue,
                /// <summary> False </summary>			
                setFalse,
            };

            /// <summary>
            /// The type of hardware platform
            /// </summary>
            public enum PlatformType
            {
                /// <summary> No platform specified. It should not be set</summary>
                none = 0,
                /// <summary> PS3 platform</summary>	
                ps3,
                /// <summary> PS Vita platform</summary>			
                psVita,
                /// <summary> PS4 platform</summary>		
                ps4
            };

            /// <summary>
            /// The online status of a user
            /// </summary>
            public enum OnlineStatus
            {
                /// <summary> The online status was not requested. It should never be returned</summary>
                notRequested = 0,
                /// <summary> The user is online on that platform</summary>
                online,
                /// <summary> The user is on stand by mode on that platform (usually PS Vita)</summary>
                standBy,
                /// <summary> The user is offline on that platform. Also returned when the user has never signed in on that platform</summary>
                offline
            };
            #endregion

            #region ReturnCodes
            // Maps to the NpToolkit2 error codes in \target\include\np_toolkit2\errors.h
            // Needs to be a 'uint' enum other some of the error codes can't be assigned

            // <summary>
            // A set of return codes that map to the Sony SCE error codes.
            // </summary>

            /// <exclude/>
            public enum ReturnCodes : uint
            {
                //Generics
                /// <summary></summary>
                SUCCESS = 0x00000000,   // SCE_TOOLKIT_NP_V2_SUCCESS																	 

                //Dialogs
                /// <summary></summary>
                DIALOG_RESULT_OK = 0x00000000,  //SCE_TOOLKIT_NP_V2_DIALOG_RESULT_OK
                                                /// <summary></summary>
                DIALOG_RESULT_USER_CANCELED = 0x00000001,   //SCE_TOOLKIT_NP_V2_DIALOG_RESULT_USER_CANCELED
                                                            /// <summary></summary>
                DIALOG_RESULT_USER_PURCHASED = 0x00000002,  //SCE_TOOLKIT_NP_V2_DIALOG_RESULT_USER_PURCHASED
                                                            /// <summary></summary>
                DIALOG_RESULT_ALREADY_SIGNED_IN = 0x00000003,   //SCE_TOOLKIT_NP_V2_DIALOG_RESULT_ALREADY_SIGNED_IN
                                                                /// <summary></summary>
                DIALOG_RESULT_NOT_SIGNED_IN = 0x00000004,   //SCE_TOOLKIT_NP_V2_DIALOG_RESULT_NOT_SIGNED_IN
                                                            /// <summary></summary>
                DIALOG_RESULT_ABORTED = 0x0000000a, //SCE_TOOLKIT_NP_V2_DIALOG_RESULT_ABORTED

                //Trophy
                /// <summary></summary>
                TROPHY_PLATINUM_UNLOCKED = 0x00000001,  //SCE_TOOLKIT_NP_V2_TROPHY_PLATINUM_UNLOCKED													

                //Matching						
                /// <summary></summary>
                MATCHING_CREATE_SYSTEM_SESSION_FAILED = 0x000000a0, //SCE_TOOLKIT_NP_V2_MATCHING_CREATE_SYSTEM_SESSION_FAILED					//The room is created successfully, the session is not
                                                                    /// <summary></summary>
                MATCHING_JOIN_SYSTEM_SESSION_FAILED = 0x000000b0,   //SCE_TOOLKIT_NP_V2_MATCHING_JOIN_SYSTEM_SESSION_FAILED						//The room is joined successfully, the session is not
                                                                    /// <summary></summary>
                MATCHING_UPDATE_SYSTEM_SESSION_FAILED = 0x000000c0, //SCE_TOOLKIT_NP_V2_MATCHING_UPDATE_SYSTEM_SESSION_FAILED					//The room is updated successfully, the session is not
                                                                    /// <summary></summary>
                MATCHING_UPDATE_EXTERNAL_NOTIFICATION_FAILED = 0x000000d0,  //SCE_TOOLKIT_NP_V2_MATCHING_UPDATE_EXTERNAL_NOTIFICATION_FAILED			//The room is updated successfully with external and search attributes, but no notifications were sent to the members due to an error

                //Error returns

                //Generic and Core
                /// <summary></summary>
                ERROR_FAILED_TO_ALLOCATE = 0x80552e00u, //SCE_TOOLKIT_NP_V2_ERROR_FAILED_TO_ALLOCATE								 ///An error occurred because library failed to allocate memory.
                                                        /// <summary></summary>
                ERROR_TOO_MANY_REQUESTS = 0x80552e01,   //SCE_TOOLKIT_NP_V2_ERROR_TOO_MANY_REQUESTS									 ///An error occurred because too many request in the que.
                                                        /// <summary></summary>
                ERROR_LOCKED_RESPONSE = 0x80552e02, //SCE_TOOLKIT_NP_V2_ERROR_LOCKED_RESPONSE									 ///An error occurred because the response object is locked.
                                                    /// <summary></summary>
                ERROR_ALREADY_INITIALIZED = 0x80552e03, //SCE_TOOLKIT_NP_V2_ERROR_ALREADY_INITIALIZED								 ///An error occurred because the library is already initialized.
                                                        /// <summary></summary>
                ERROR_NOT_INITIALIZED = 0x80552e04, //SCE_TOOLKIT_NP_V2_ERROR_NOT_INITIALIZED									 ///An error occurred because library is not initialized.
                                                    /// <summary></summary>
                ERROR_INCORRECT_ARGUMENTS = 0x80552e05, //SCE_TOOLKIT_NP_V2_ERROR_INCORRECT_ARGUMENTS								 ///An error occurred because incorrect arguments where passed in the request.
                                                        /// <summary></summary>
                ERROR_MODIFICATION_NOT_ALLOWED = 0x80552e06,    //SCE_TOOLKIT_NP_V2_ERROR_MODIFICATION_NOT_ALLOWED							 ///An error was caused as modification of object is not allowed.
                                                                /// <summary></summary>
                ERROR_MAX_USERS_REACHED = 0x80552e07,   //SCE_TOOLKIT_NP_V2_ERROR_MAX_USERS_REACHED									 ///An error occurred because max number of users are already registered .
                                                        /// <summary></summary>
                ERROR_INVALID_IMAGE = 0x80552e08,   //SCE_TOOLKIT_NP_V2_ERROR_INVALID_IMAGE										 ///An error occurred because image is invalid.
                                                    /// <summary></summary>
                ERROR_MEM_POOLS_INCORRECT = 0x80552e09, //SCE_TOOLKIT_NP_V2_ERROR_MEM_POOLS_INCORRECT								 /// An error occurred because memory pools are not correct set up.
                                                        /// <summary></summary>
                ERROR_EXT_ALLOCATOR_INCORRECT = 0x80552e0a, //SCE_TOOLKIT_NP_V2_ERROR_EXT_ALLOCATOR_INCORRECT							 ///An error occurred because incorrect allocator was provided.
                                                            /// <summary></summary>
                ERROR_MAX_NUM_CALLBACKS_REACHED = 0x80552e0b,   //SCE_TOOLKIT_NP_V2_ERROR_MAX_NUM_CALLBACKS_REACHED							 ///An error occurred because number of callback registered has reached maximum limit. 
                                                                /// <summary></summary>
                ERROR_CALLBACK_NOT_REGISTERED = 0x80552e0c, //SCE_TOOLKIT_NP_V2_ERROR_CALLBACK_NOT_REGISTERED							 ///An error occurred because callback was not registered.

                //Trophy									   
                /// <summary></summary>
                ERROR_TROPHY_HOME_DIRECTORY_NOT_CONFIGURED = 0x80552e70,    //SCE_TOOLKIT_NP_V2_ERROR_TROPHY_HOME_DIRECTORY_NOT_CONFIGURED				 ///An error occurred because home directory was not configured.

                //Matching																									   
                /// <summary></summary>
                ERROR_MATCHING_ROOM_DESTROYED = 0x80552f00, //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_ROOM_DESTROYED									///An error occurred because room has already been destroyed. 
                                                            /// <summary></summary>
                ERROR_MATCHING_INVALID_ATTRIBUTE_SCOPE = 0x80552f01,    //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_INVALID_ATTRIBUTE_SCOPE							 ///An error occurred because the scope specified while registering attribute was invalid.
                                                                        /// <summary></summary>
                ERROR_MATCHING_INVALID_ATTRIBUTE_TYP = 0x80552f02,  //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_INVALID_ATTRIBUTE_TYPE								 ///An error occurred because the type of attribute specified while registering attribute was invalid.
                                                                    /// <summary></summary>
                ERROR_MATCHING_INVALID_ROOM_ATTRIBUTE_VISIBILITY = 0x80552f03,  //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_INVALID_ROOM_ATTRIBUTE_VISIBILITY					 ///An error occurred because the visibility of attribute specified while registering attribute was invalid.
                                                                                /// <summary></summary>
                ERROR_MATCHING_SUM_OF_MEMBER_ATTRIBUTES_SIZES_IS_MORE_THAN_64 = 0x80552f04, //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_SUM_OF_MEMBER_ATTRIBUTES_SIZES_IS_MORE_THAN_64		 ///An error occurred because the max size of member attribute is 64 bytes in total.
                                                                                            /// <summary></summary>
                ERROR_MATCHING_MORE_THAN_1_BINARY_SEARCH_ATTRIBUTE_PROVIDED = 0x80552f05,   //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_MORE_THAN_1_BINARY_SEARCH_ATTRIBUTE_PROVIDED				 ///An error occurred because more than one binary search attribute was specified. 
                                                                                            /// <summary></summary>
                ERROR_MATCHING_SEARCH_BINARY_ATTRIBUTE_SIZE_IS_MORE_THAN_64 = 0x80552f06,   //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_SEARCH_BINARY_ATTRIBUTE_SIZE_IS_MORE_THAN_64				 ///An error occurred because the service could not override the ID required.
                                                                                            /// <summary></summary>
                ERROR_MATCHING_MORE_THAN_8_INTEGER_SEARCH_ATTRIBUTES_PROVIDED = 0x80552f07, //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_MORE_THAN_8_INTEGER_SEARCH_ATTRIBUTES_PROVIDED			 ///An error occurred because the max number of search  attribute is 8.
                                                                                            /// <summary></summary>
                ERROR_MATCHING_SUM_OF_EXTERNAL_ROOM_ATTRIBUTES_SIZES_IS_MORE_THAN_512 = 0x80552f08, //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_SUM_OF_EXTERNAL_ROOM_ATTRIBUTES_SIZES_IS_MORE_THAN_512	 /// An error occurred because total size of external attribute was greater than max size of 512. Note: The first 64 bytes are used for the name of the room
                                                                                                    /// <summary></summary>
                ERROR_MATCHING_SUM_OF_INTERNAL_ROOM_ATTRIBUTES_SIZES_IS_MORE_THAN_512 = 0x80552f09, //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_SUM_OF_INTERNAL_ROOM_ATTRIBUTES_SIZES_IS_MORE_THAN_512	 /// An error occurred because total size of internal attribute was greater than max size of 512.Note: The first 64 bytes are used for internal bound session information
                                                                                                    /// <summary></summary>
                ERROR_MATCHING_NAMES_OF_ATTRIBUTES_MUST_BE_UNIQUE = 0x80552f0a, //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_NAMES_OF_ATTRIBUTES_MUST_BE_UNIQUE						 ///An error occurred because attribute name is not unique. 
                                                                                /// <summary></summary>
                ERROR_MATCHING_INTERNAL_ATTRIBUTES_DONT_FIT_IN_256_ARRAYS = 0x80552f0b, //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_INTERNAL_ATTRIBUTES_DONT_FIT_IN_256_ARRAYS				 ///An error occurred because external attributes cannot be arranged in 256 blocks of memory.
                                                                                        /// <summary></summary>
                ERROR_MATCHING_EXTERNAL_ATTRIBUTES_DONT_FIT_IN_256_ARRAYS = 0x80552f0c, //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_EXTERNAL_ATTRIBUTES_DONT_FIT_IN_256_ARRAYS				 ///An error occurred because internal attributes cannot be arranged in 256 blocks of memory.
                                                                                        /// <summary></summary>
                ERROR_MATCHING_BIN_ATTRIBUTE_CANNOT_BE_SIZE_0 = 0x80552f0d, //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_BIN_ATTRIBUTE_CANNOT_BE_SIZE_0							 ///An error occurred size of binary attribute is set to 0. The size should be greater than 0 
                                                                            /// <summary></summary>
                ERROR_MATCHING_INIT_CONFIGURATION_ALREADY_SET = 0x80552f0e, //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_INIT_CONFIGURATION_ALREADY_SET						  ///An error occurred because configuration was already set.
                                                                            /// <summary></summary>
                ERROR_MATCHING_INIT_CONFIGURATION_NOT_SET = 0x80552f0f, //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_INIT_CONFIGURATION_NOT_SET							  ///An error occurred because configuration was not set.
                                                                        /// <summary></summary>
                ERROR_MATCHING_USER_IS_ALREADY_IN_A_ROOM = 0x80552f10,  //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_USER_IS_ALREADY_IN_A_ROOM							 ///An error occurred because user is already in the room.
                                                                        /// <summary></summary>
                ERROR_MATCHING_USER_IS_NOT_IN_A_ROOM = 0x80552f11,  //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_USER_IS_NOT_IN_A_ROOM								 ///An error occurred because user is not in the room.
                                                                    /// <summary></summary>
                ERROR_MATCHING_NO_SESSION_BOUND_TO_ROOM = 0x80552f12,   //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_NO_SESSION_BOUND_TO_ROOM								 ///<An error occurred because no NP Session is bound to the Matching room. 
                                                                        /// <summary></summary>
                ERROR_MATCHING_INVALID_WORLD_NUMBER = 0x80552f13,   //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_INVALID_WORLD_NUMBER									 ///An error occurred because invalid world number was provided .
                                                                    /// <summary></summary>
                ERROR_MATCHING_ATTRIBUTE_IS_NOT_SEARCHABLE_TYPE = 0x80552f14,   //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_ATTRIBUTE_IS_NOT_SEARCHABLE_TYPE						 ///An error occurred because attributes is not searchable type.
                                                                                /// <summary>An error occurred because the attribute was invalid. see SCE_TOOLKIT_NP_V2_ERROR_MATCHING_INVALID_ATTRIBUTE</summary>
                ERROR_MATCHING_INVALID_ATTRIBUTE = 0x80552f15,  //SCE_TOOLKIT_NP_V2_ERROR_MATCHING_INVALID_ATTRIBUTE									
                                                                /// <summary></summary>
                ERROR_MATCHING_INVALID_MEMBER_ID = 0x80552f16,	//SCE_TOOLKIT_NP_V2_ERROR_MATCHING_INVALID_MEMBER_ID									 ///An error occurred because the member id was invalid.

                /// <summary> Argument is invalid</summary>
                NP_ERROR_INVALID_ARGUMENT = 0x80550003, //SCE_NP_ERROR_INVALID_ARGUMENT 
                /// <summary> Undefined platform</summary>
                NP_ERROR_UNKNOWN_PLATFORM_TYPE = 0x80550004, //SCE_NP_ERROR_UNKNOWN_PLATFORM_TYPE 
                /// <summary> Insufficient memory</summary>
                NP_ERROR_OUT_OF_MEMORY = 0x80550005, //SCE_NP_ERROR_OUT_OF_MEMORY 
                /// <summary> The specified user was signed out</summary>
                NP_ERROR_SIGNED_OUT = 0x80550006, //SCE_NP_ERROR_SIGNED_OUT 
                /// <summary> The specified user was not found</summary>
                NP_ERROR_USER_NOT_FOUND = 0x80550007, //SCE_NP_ERROR_USER_NOT_FOUND 
                /// <summary> Callback function is already registered</summary>
                NP_ERROR_CALLBACK_ALREADY_REGISTERED = 0x80550008, //SCE_NP_ERROR_CALLBACK_ALREADY_REGISTERED 
                /// <summary> Callback function is not registered</summary>
                NP_ERROR_CALLBACK_NOT_REGISTERED = 0x80550009, //SCE_NP_ERROR_CALLBACK_NOT_REGISTERED 
                /// <summary> Called in a non-signed up state</summary>
                NP_ERROR_NOT_SIGNED_UP = 0x8055000a, //SCE_NP_ERROR_NOT_SIGNED_UP 
                /// <summary> Applicable to the viewer age restriction</summary>
                NP_ERROR_AGE_RESTRICTION = 0x8055000b, //SCE_NP_ERROR_AGE_RESTRICTION 
                /// <summary> Called in a logged out state</summary>
                NP_ERROR_LOGOUT = 0x8055000c, //SCE_NP_ERROR_LOGOUT 
                /// <summary> A new version of the system software update file exists</summary>
                NP_ERROR_LATEST_SYSTEM_SOFTWARE_EXIST = 0x8055000d, //SCE_NP_ERROR_LATEST_SYSTEM_SOFTWARE_EXIST 
                /// <summary> A required new version of the system software update file exists for this application</summary>
                NP_ERROR_LATEST_SYSTEM_SOFTWARE_EXIST_FOR_TITLE = 0x8055000e, //SCE_NP_ERROR_LATEST_SYSTEM_SOFTWARE_EXIST_FOR_TITLE 
                /// <summary> A new version of the patch package exists. The user has to update before he can play multiplayer games.</summary>
                NP_ERROR_LATEST_PATCH_PKG_EXIST = 0x8055000f, //SCE_NP_ERROR_LATEST_PATCH_PKG_EXIST 
                /// <summary> A new version of the downloaded patch package exists</summary>
                NP_ERROR_LATEST_PATCH_PKG_DOWNLOADED = 0x80550010, //SCE_NP_ERROR_LATEST_PATCH_PKG_DOWNLOADED 
                /// <summary> Structure size specified for the size member in the structure is invalid</summary>
                NP_ERROR_INVALID_SIZE = 0x80550011, //SCE_NP_ERROR_INVALID_SIZE 
                /// <summary> Processing was aborted</summary>
                NP_ERROR_ABORTED = 0x80550012, //SCE_NP_ERROR_ABORTED 
                /// <summary> Requests exceeding the maximum number were generated at the same time</summary>
                NP_ERROR_REQUEST_MAX = 0x80550013, //SCE_NP_ERROR_REQUEST_MAX 
                /// <summary> Specified request does not exist</summary>
                NP_ERROR_REQUEST_NOT_FOUND = 0x80550014, //SCE_NP_ERROR_REQUEST_NOT_FOUND 
                /// <summary> Specified ID is invalid</summary>
                NP_ERROR_INVALID_ID = 0x80550015, //SCE_NP_ERROR_INVALID_ID 
                /// <summary> Patch was not checked</summary>
                NP_ERROR_PATCH_NOT_CHECKED = 0x80550018, //SCE_NP_ERROR_PATCH_NOT_CHECKED 
                /// <summary> Timed out</summary>
                NP_ERROR_TIMEOUT = 0x8055001a, //SCE_NP_ERROR_TIMEOUT 
                /// <summary> Specified NP ID is invalid</summary>
                NP_UTIL_ERROR_INVALID_NP_ID = 0x80550605, //SCE_NP_UTIL_ERROR_INVALID_NP_ID 
                /// <summary> The two IDs that were compared were different values</summary>
                NP_UTIL_ERROR_NOT_MATCH = 0x80550609, //SCE_NP_UTIL_ERROR_NOT_MATCH

                /// <summary></summary>
                NP_WEBAPI_ERROR_LIB_CONTEXT_NOT_FOUND = 0x80552904, //SCE_NP_WEBAPI_ERROR_LIB_CONTEXT_NOT_FOUND

                /// <summary></summary>
                NP_TROPHY_ERROR_INVALID_ARGUMENT = 0x80551604,   // SCE_NP_TROPHY_ERROR_INVALID_ARGUMENT
                                                                 /// <summary></summary>
                NP_TROPHY_ERROR_ALREADY_REGISTERED = 0x80551610,   // SCE_NP_TROPHY_ERROR_ALREADY_REGISTERED
                                                                   /// <summary></summary>
                NP_TROPHY_ERROR_INVALID_GROUP_ID = 0x8055160B,   // SCE_NP_TROPHY_ERROR_INVALID_GROUP_ID			
                                                                 /// <summary></summary>
                NP_TROPHY_ERROR_TROPHY_ALREADY_UNLOCKED = 0x8055160C,   // SCE_NP_TROPHY_ERROR_TROPHY_ALREADY_UNLOCKED
                                                                        /// <summary></summary>
                NP_TROPHY_ERROR_NOT_REGISTERED = 0x8055160F,   // SCE_NP_TROPHY_ERROR_NOT_REGISTERED

                /// <summary></summary>
                NP_TROPHY_ERROR_TROPHY_NOT_UNLOCKED = 0x8055161A,   // SCE_NP_TROPHY_ERROR_TROPHY_NOT_UNLOCKED

                /// <summary></summary>
                TOOLKIT_NP_V2_ERROR_INCORRECT_ARGUMENTS = 0x80552e05,   // TOOLKIT_NP_V2_ERROR_INCORRECT_ARGUMENTS

                /// <summary></summary>
                NET_ERROR_RESOLVER_ENODNS = 0x804101E1,   // SCE_NET_ERROR_RESOLVER_ENODNS
                                                          /// <summary></summary>
                NET_CTL_ERROR_NOT_CONNECTED = 0x80412108,   // SCE_NET_CTL_ERROR_NOT_CONNECTED
                                                            /// <summary></summary>
                NET_CTL_ERROR_NOT_AVAIL = 0x80412109,   // SCE_NET_CTL_ERROR_NOT_AVAIL

                /// <summary></summary>
                NP_COMMUNITY_SERVER_ERROR_NOT_BEST_SCORE = 0x80550815, // SCE_NP_COMMUNITY_SERVER_ERROR_NOT_BEST_SCORE

                /// <summary></summary>
                NP_COMMUNITY_SERVER_ERROR_INVALID_SCORE = 0x80550823, // SCE_NP_COMMUNITY_SERVER_ERROR_INVALID_SCORE

                /// <summary></summary>
                NP_COMMUNITY_SERVER_ERROR_GAME_DATA_ALREADY_EXISTS = 0x8055082C, // SCE_NP_COMMUNITY_SERVER_ERROR_GAME_DATA_ALREADY_EXISTS		

                /// <summary></summary>
                NP_COMMUNITY_SERVER_ERROR_RANKING_GAME_DATA_MASTER_NOT_FOUND = 0x80550818, // SCE_NP_COMMUNITY_SERVER_ERROR_RANKING_GAME_DATA_MASTER_NOT_FOUND

                /// <summary></summary>
                NP_MATCHING2_ERROR_CONTEXT_NOT_STARTED = 0x80550C08 // SCE_NP_COMMUNITY_SERVER_ERROR_RANKING_GAME_DATA_MASTER_NOT_FOUND
            }

            /// <summary>
            /// Generate a string containing the Hex return code and if known the Return code name
            /// taken from the Core.ReturnCodes enum.
            /// As some of the enums share the same code the returned string might be incorrect.
            /// If conversion of a return code from a response object is required use the <see cref="ResponseBase.ConvertReturnCodeToString"/> instead as this can qualify the return code depending on the API called.
            /// </summary>
            /// <param name="errorCode">The error code to convert.</param>
            /// <returns>The constructed string with the Hex and return code name.</returns>
            /// <remarks>
            /// By default apiCalled will be FunctionType.invalid. This will return the first Core.ReturnCodes enum that matches the return code value so may produce an incorrect string. 
            /// </remarks>
            static public string ConvertSceErrorToString(Int32 errorCode)
            {
                string output = "(0x" + errorCode.ToString("X8") + ")";

                ReturnCodes returnCode = (ReturnCodes)errorCode;

                if (Enum.IsDefined(typeof(Sony.NP.Core.ReturnCodes), returnCode) == true)
                {
                    output += " (" + returnCode.ToString() + ") ";
                }
                else
                {
                    output += " (UNKNOWN) ";
                }

                return output;
            }

            #endregion

            #region Ids
            /// <summary>
            /// UserServiceUserId provides a 32bit id for a local console User.
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public struct UserServiceUserId // Maps to SceUserServiceUserId
            {
                /// <summary>
                /// Invalid user id
                /// </summary>
                public const Int32 UserIdInvalid = -1; // SCE_USER_SERVICE_USER_ID_INVALID

                internal Int32 id;

                /// <summary>
                /// The 32bit local console id
                /// </summary>
                public Int32 Id
                {
                    get { return id; }
                    set { id = value; }
                }

                /// <summary>
                /// Return Hex string of id.
                /// </summary>
                /// <returns>10 character HEX string in format 0xFFFFFFFF</returns>
                public override string ToString()
                {
                    return "0x" + id.ToString("X8");
                }

                internal void Read(MemoryBuffer buffer)
                {
                    id = buffer.ReadInt32();
                }

                /// <summary>
                /// Allow direct assignment of Int32
                /// </summary>
                /// <param name="value">32bit local console id</param>
                static public implicit operator UserServiceUserId(Int32 value)
                {
                    UserServiceUserId newId = new UserServiceUserId();
                    newId.id = value;
                    return newId;
                }
            }

            /// <summary>
            /// NpAccountId provides a 64bit unique id for a PSN User.
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public struct NpAccountId // Maps to SceNpAccountId.
            {
                // 64bit Unsigend int value type can has the same memory footprint as a UInt64 so an
                // array of these will marshal to a C++ array of UInt64
                internal UInt64 id;

                /// <summary>
                /// The 64bit local console id
                /// </summary>
                public UInt64 Id
                {
                    get { return id; }
                    set { id = value; }
                }

                /// <summary>
                /// Return Hex string of id.
                /// </summary>
                /// <returns>20 character HEX string in format 0xFFFFFFFFFFFFFFFF</returns>
                public override string ToString()
                {
                    return "0x" + id.ToString("X16");
                }

                internal void Read(MemoryBuffer buffer)
                {
                    id = buffer.ReadUInt64();
                }

                /// <summary>
                /// Allow direct assignment of UInt64
                /// </summary>
                /// <param name="value">64bit unique id</param>
                static public implicit operator NpAccountId(UInt64 value)
                {
                    NpAccountId newId = new NpAccountId();
                    newId.id = value;
                    return newId;
                }

                /// <summary>
                /// The equality operator (==) returns true if the values of its operands are equal, false otherwise
                /// </summary>
                /// <param name="a">Operand A</param>
                /// <param name="b">Operand B</param>
                /// <returns>True if equal.</returns>
                public static bool operator ==(NpAccountId a, NpAccountId b)
                {
                    // If both are null, or both are same instance, return true.
                    if (System.Object.ReferenceEquals(a, b))
                    {
                        return true;
                    }

                    // Return true if the fields match:
                    return a.id == b.id;
                }

                /// <summary>
                /// The inequality operator (==) returns false if the values of its operands are equal, true otherwise
                /// </summary>
                /// <param name="a">Operand A</param>
                /// <param name="b">Operand B</param>
                /// <returns>True if not equal.</returns>
                public static bool operator !=(NpAccountId a, NpAccountId b)
                {
                    return !(a == b);
                }

                /// <summary>
                /// Determines whether the specified object is equal to the current object.
                /// </summary>
                /// <param name="obj">The object to compare with the current object.</param>
                /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
                public override bool Equals(System.Object obj)
                {
                    return obj is NpAccountId && this == (NpAccountId)obj;
                }

                /// <summary>
                /// Returns the hash code for Account id 
                /// </summary>
                /// <returns>A 32-bit signed integer hash code.</returns>
                public override int GetHashCode()
                {
                    return id.GetHashCode();
                }
            }

            /// <summary>
            /// This structure represents the NP ID to be used by the Np library to identify a user. It internally holds the Online ID.
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public struct NpId // Maps to SceNpId.
            {
                internal OnlineID handle;
                internal byte[] opt; // Option data

                /// <summary>
                /// The 16 character PSN Id
                /// </summary>
                public OnlineID Handle
                {
                    get { return handle; }
                }

                /// <summary>
                /// Option data
                /// </summary>
                public byte[] Opt
                {
                    get { return opt; }
                }

                /// <summary>
                /// Returns the id as a string, with a maximum of 16 characters.
                /// </summary>
                /// <returns>The OnLineId name</returns>
                public override string ToString()
                {
                    return handle.ToString();
                }

                internal void Read(MemoryBuffer buffer)
                {
                    buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.SceNpIdBegin);

                    handle.Read(buffer);
                    buffer.ReadData(ref opt);

                    buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.SceNpIdEnd);
                }
            }
            #endregion

            #region Online User IDs

            /// <summary>
            /// 16 character PSN Id
            /// </summary>
            public class OnlineID  // Maps to SceNpOnlineId
            {
                /// <summary>
                /// Maximum length of the online id
                /// </summary>
                public const int SCE_NP_ONLINEID_MAX_LENGTH = 16;

                internal byte[] data;
                internal string name = "";

                /// <summary>
                /// Display representation of an online user
                /// </summary>
                public string Name
                {
                    get { return name; }
                    //set
                    //{
                    //	if (value.Length > SCE_NP_ONLINEID_MAX_LENGTH)
                    //	{
                    //		throw new NpToolkitException("OnlineID can't be more than " + SCE_NP_ONLINEID_MAX_LENGTH + " characters.");
                    //	}

                    //	name = value;

                    //	byte[] toBytes = System.Text.Encoding.ASCII.GetBytes(name);

                    //	for (int i = 0; i < toBytes.Length; i++)
                    //	{
                    //		data[i] = toBytes[i];
                    //	}
                    //}
                }

                /// <summary>
                /// Create an empty/blank online id
                /// </summary>
                public OnlineID()
                {
                    data = new byte[SCE_NP_ONLINEID_MAX_LENGTH];
                }

                // Read data from PRX marshaled buffer
                internal void Read(MemoryBuffer buffer)
                {
                    buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NpOnlineIdBegin);

                    buffer.ReadData(ref data);

                    //Get the first index of a null char
                    int foundIndex = SCE_NP_ONLINEID_MAX_LENGTH;

                    for (int i = 0; i < data.Length; i++)
                    {
                        if (data[i] == 0)
                        {
                            foundIndex = i;
                            break;
                        }
                    }

                    if (foundIndex > 0)
                    {
                        name = System.Text.Encoding.ASCII.GetString(data, 0, foundIndex);
                    }
                    else
                    {
                        name = "";
                    }

                    buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NpOnlineIdEnd);
                }

                /// <summary>
                /// Returns the OnlineId as a string, with a maximum of 16 characters.
                /// </summary>
                /// <returns>The OnLineId name</returns>
                public override string ToString()
                {
                    return name;
                }

                /// <summary>
                /// The equality operator (==) returns true if the values of its operands are equal, false otherwise
                /// </summary>
                /// <param name="a">Operand A</param>
                /// <param name="b">Operand B</param>
                /// <returns>True if equal.</returns>
                public static bool operator ==(OnlineID a, OnlineID b)
                {
                    // If both are null, or both are same instance, return true.
                    if (System.Object.ReferenceEquals(a, b))
                    {
                        return true;
                    }

                    // If one is null, but not both, return false.
                    if (((object)a == null) || ((object)b == null))
                    {
                        return false;
                    }

                    // Return true if the fields match:
                    return a.name == b.name;
                }

                /// <summary>
                /// The inequality operator (==) returns false if the values of its operands are equal, true otherwise
                /// </summary>
                /// <param name="a">Operand A</param>
                /// <param name="b">Operand B</param>
                /// <returns>True if not equal.</returns>
                public static bool operator !=(OnlineID a, OnlineID b)
                {
                    return !(a == b);
                }

                /// <summary>
                /// Determines whether the specified object is equal to the current object.
                /// </summary>
                /// <param name="obj">The object to compare with the current object.</param>
                /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
                public override bool Equals(System.Object obj)
                {
                    return obj is OnlineID && this == (OnlineID)obj;
                }

                /// <summary>
                /// Returns the hash code for Account id 
                /// </summary>
                /// <returns>A 32-bit signed integer hash code.</returns>
                public override int GetHashCode()
                {
                    return name.GetHashCode();
                }
            };

            /// <summary>
            /// Online user contain both the 64bit account id and online user id
            /// </summary>
            public class OnlineUser
            {
                internal NpAccountId accountId;
                internal OnlineID onlineId;

                /// <summary>
                /// Primary key of an online user
                /// </summary>
                public NpAccountId AccountId
                {
                    get { return accountId; }
                }

                /// <summary>
                /// Display representation of an online user
                /// </summary>
                public OnlineID OnlineID
                {
                    get { return onlineId; }
                }

                /// <summary>
                /// Initialise an empty OnlineUser
                /// </summary>
                public OnlineUser()
                {
                    onlineId = new OnlineID();
                }

                // Read data from PRX marshaled buffer
                internal void Read(MemoryBuffer buffer)
                {
                    buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.OnlineUserBegin);

                    accountId.Read(buffer);
                    onlineId.Read(buffer);

                    buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.OnlineUserEnd);
                }

                /// <summary>
                /// Returns a combination of the AccountId and OnlineId.
                /// </summary>
                /// <returns>A concatenation of the AccountId and OnLineId.</returns>
                public override string ToString()
                {
                    return string.Format("0x{0:X} : {1}\n", accountId, onlineId.ToString());
                }
            };
            #endregion

            #region Language and Country codes

            /// <summary>
            /// 2 character country code.
            /// </summary>
            public class CountryCode
            {
                /// <summary>
                /// Maximum length of a country code
                /// </summary>
                public const int SCE_NP_COUNTRY_CODE_LENGTH = 2;

                internal string code = "";

                /// <summary>
                /// Create an empty country code
                /// </summary>
                public CountryCode()
                {
                    code = "";
                }

                /// <summary>
                /// Initialise a country code.
                /// </summary>
                /// <param name="countryCode">The country code.</param>
                public CountryCode(string countryCode)
                {
                    Code = countryCode;
                }

                /// <summary>
                /// A 2 character country code.
                /// </summary>
                /// <exception cref="NpToolkitException">Will throw an exception if the length of the string is longer than <see cref="SCE_NP_COUNTRY_CODE_LENGTH"/></exception>
                public string Code
                {
                    get { return code; }
                    set
                    {
                        if (value.Length > SCE_NP_COUNTRY_CODE_LENGTH)
                        {
                            throw new NpToolkitException("Country code can only be a maximum of 2 characters .");
                        }

                        code = value;
                    }
                }

                // Read data from PRX marshaled buffer
                internal void Read(MemoryBuffer buffer)
                {
                    buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NpCountryCodeBegin);
                    buffer.ReadString(ref code);
                    buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NpCountryCodeEnd);
                }

                /// <summary>
                /// The 2 character country code.
                /// </summary>
                /// <returns>The country code</returns>
                public override string ToString()
                {
                    return code;
                }

                /// <summary>
                /// Initialise a country code when a string is assigned to it.
                /// </summary>
                /// <param name="countryCode">The country code</param>
                /// <returns>A new country code instance</returns>
                static public implicit operator CountryCode(string countryCode)
                {
                    CountryCode newCode = new CountryCode();
                    newCode.Code = countryCode;
                    return newCode;
                }
            };

            /// <summary>
            /// 5 character language code.
            /// </summary>
            public class LanguageCode
            {
                /// <summary>
                /// Maximum length of a language code
                /// </summary>
                public const int SCE_NP_LANGUAGE_CODE_MAX_LEN = 5;

                internal string code;

                /// <summary>
                /// Initialise an empty language code
                /// </summary>
                public LanguageCode()
                {
                    code = "";
                }

                /// <summary>
                /// A 5 character language code
                /// </summary>
                /// <exception cref="NpToolkitException">Will throw an exception if the length of the string is longer than <see cref="SCE_NP_LANGUAGE_CODE_MAX_LEN"/></exception>
                public string Code
                {
                    get { return code; }
                    set
                    {
                        if (value.Length > SCE_NP_LANGUAGE_CODE_MAX_LEN)
                        {
                            throw new NpToolkitException("Language code can only be a maximum of 5 characters .");
                        }

                        code = value;
                    }
                }

                // Read data from PRX marshaled buffer
                internal void Read(MemoryBuffer buffer)
                {
                    buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NpLanguageCodeBegin);
                    buffer.ReadString(ref code);
                    buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NpLanguageCodeEnd);
                }

                /// <summary>
                /// The 5 character country code.
                /// </summary>
                /// <returns>The language code</returns>
                public override string ToString()
                {
                    return code;
                }

                /// <summary>
                /// Initialise a language code when a string is assigned to it.
                /// </summary>
                /// <param name="languageCode">The language code</param>
                /// <returns>A new language code instance</returns>
                static public implicit operator LanguageCode(string languageCode)
                {
                    LanguageCode newCode = new LanguageCode();
                    newCode.Code = languageCode;
                    return newCode;
                }
            };
            #endregion

            #region Title Info

            /// <summary>
            /// A 12 character Np Title Id 
            /// </summary>
            public class TitleId
            {
                /// <summary>
                /// Maximum length of the title id
                /// </summary>
                public const int SCE_NP_TITLE_ID_LEN = 12;

                internal byte[] data;

                /// <summary>
                /// A 12 character Np title id 
                /// </summary>
                public string Id
                {
                    get { return System.Text.Encoding.ASCII.GetString(data, 0, data.Length); }
                }

                /// <summary>
                /// Initialise a blank title id
                /// </summary>
                public TitleId()
                {
                    data = new byte[SCE_NP_TITLE_ID_LEN];
                }

                // Read data from PRX marshaled buffer
                internal void Read(MemoryBuffer buffer)
                {
                    buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NpTitleIdBegin);

                    buffer.ReadData(ref data);

                    buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NpTitleIdEnd);
                }

                /// <summary>
                /// A 12 character Np Title Id 
                /// </summary>
                /// <returns>the Np Title Id</returns>
                public override string ToString()
                {
                    return System.Text.Encoding.ASCII.GetString(data, 0, data.Length);
                }
            }
            #endregion

            #region Empty Response
            // Provides the callback to read the empty response data.
            // As its name implies there is actually no data associated with this reponse.
            // This is provided so any ServerError can be read in base.ReadResult(id);
            // It doesn't need to call into the native code, but does so to validate the
            // existance of the Response object and will throw an exception if anything has gone wrong.

            /// <summary>
            /// Representation of empty data of a ResponseBase class
            /// This still provides basic data that will indicate errors or return codes
            /// </summary>
            public class EmptyResponse : ResponseBase
            {
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

                    // Nothing to read inside an empty buffer
                    EndReadResponseBuffer(readBuffer);
                }
            }
            #endregion

            static internal DateTime ReadRtcTick(MemoryBuffer buffer)
            {
                UInt64 rtcTick = buffer.ReadUInt64();
                return RtcTicksToDateTime(rtcTick);
            }

            static internal UInt64 DateTimeToRtcTicks(DateTime dateTime)
            {
                UInt64 sceToDotNetTicks = 10;   // sce ticks are microsecond, .net are 100 nanosecond

                UInt64 ticks = (UInt64)dateTime.Ticks;

                ticks /= sceToDotNetTicks;

                return ticks;
            }

            static internal DateTime RtcTicksToDateTime(UInt64 rtcTick)
            {
                UInt64 sceToDotNetTicks = 10;   // sce ticks are microsecond, .net are 100 nanosecond

                rtcTick *= sceToDotNetTicks;

                return new DateTime((Int64)rtcTick);
            }
        }

        /// <summary>
        /// PNG image store in bytes from NpToolkit
        /// </summary>
        public class Icon
        {
            internal byte[] rawBytes;
            internal Int32 width;
            internal Int32 height;

            /// <summary>
            /// Get the RawBytes from the Icon. These can be used to create a new Texture2d for example.
            /// </summary>
            public byte[] RawBytes
            {
                get
                {
                    return rawBytes;
                }
            }

            /// <summary>
            /// Width in pixels of the icon.
            /// </summary>
            public Int32 Width { get { return width; } }

            /// <summary>
            /// Height in pixels of the icon.
            /// </summary>
            public Int32 Height { get { return height; } }

            static internal Icon ReadAndCreate(MemoryBuffer buffer)
            {
                Icon result = null;
                buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.PNGBegin);

                bool hasIcon = buffer.ReadBool();

                if (hasIcon == false)
                {
                    //Console.WriteLine("No icon data");
                }
                else
                {
                    //Console.WriteLine("Creating icon.");

                    result = new Icon();
                    // Read the image
                    Int32 numBytes = buffer.ReadInt32();
                    result.width = buffer.ReadInt32();
                    result.height = buffer.ReadInt32();

                    buffer.ReadData(ref result.rawBytes);
                }

                buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.PNGEnd);

                return result;
            }
        }
    }
}
