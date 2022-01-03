using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Sony
{
    namespace NP
    {
        /// <summary>
        /// Defines the different services provided by the NpToolkit2.
        /// It is set automatically when a request object is created, and identifies the service it belongs to.
        /// </summary>
        public enum ServiceTypes
        {
            /// <summary>Non-valid service. It should never be returned (check memset() is not being used in a non-POD object, as a request)</summary>
            Invalid = 0,
            /// <summary>Auth service</summary>
            Auth,
            /// <summary>Presence service</summary>
            Presence,
            /// <summary>Ranking service</summary>
            Ranking,
            /// <summary>Trophy service</summary>
            Trophy,
            /// <summary>Network Utils service</summary>
            NetworkUtils,
            /// <summary>Np Utils service</summary>
            NpUtils,
            /// <summary>Wordfilter service</summary>
            WordFilter,
            /// <summary>User Profile service</summary>
            UserProfile,
            /// <summary>Events Client service</summary>
            EventsClient,
            /// <summary>Messaging service</summary>
            Messaging,
            /// <summary>Matching service</summary>
            Matching,
            /// <summary>Commerce service</summary>
            Commerce,
            /// <summary>Challenge service</summary>
            Challenge,
            /// <summary>TUS service</summary>
            Tus,
            /// <summary>TSS service</summary>
            Tss,
            /// <summary>Friend service</summary>
            Friends,
            /// <summary>Session service</summary>
            Session,
            /// <summary>Activity Feed service</summary>
            ActivityFeed,
            /// <summary>Social Media service</summary>
            SocialMedia,
            /// <summary>Shared Media service</summary>
            SharedMedia,
            /// <summary>Core service. Used only when a Core request is performed</summary>
            Core,
            /// <summary>Notification service. Used to notify when something external to the application happened. Not bound to a request</summary>
            Notification,
            /// <summary>Size of this enum</summary>
            Size
        };

        /// <summary>
        /// Defines the different APIs provided by the NpToolkit2 library.
        /// It is set automatically when a request object is created, and identifies the function it belongs to.
        /// </summary>
        public enum FunctionTypes
        {
            /// <summary>Non-valid function. It should never be returned (check memset() is not being used in a non-POD object, as a request)</summary>
            Invalid = 0,

            /// <summary>Not implemented</summary>
            ActivityFeedGetSharedVideos,
            /// <summary>Not implemented</summary>
            ActivityFeedGetPlayedWith,
            /// <summary>Not implemented</summary>
            ActivityFeedPostPlayedWith,
            /// <summary>Not implemented</summary>
            ActivityFeedGetWhoLiked,
            /// <summary>Not implemented</summary>
            ActivityFeedSetLiked,
            /// <summary>Not implemented</summary>
            ActivityFeedGetFeed,
            /// <summary>Not implemented</summary>
            ActivityFeedPostInGameStory,

            /// <summary>Not implemented</summary>
            AuthGetAuthCode,
            /// <summary>Not implemented</summary>	
            AuthGetIdToken,

            /// <summary>Not implemented</summary>
            ChallengeConsumeChallenge,
            /// <summary>Not implemented</summary>
            ChallengeSendChallenge,
            /// <summary>Not implemented</summary>
            ChallengeGetReceivedChallenges,
            /// <summary>Not implemented</summary>
            ChallengeGetChallengeData,
            /// <summary>Not implemented</summary>
            ChallengeGetChallengeThumbnail,

            /// <summary>Not implemented</summary>
            CommerceGetCategories,                      //Commerce::getCategories() function
                                                        /// <summary>Not implemented</summary>
            CommerceGetProducts,                        //Commerce::getProducts() function
                                                        /// <summary>Not implemented</summary>
            CommerceGetServiceEntitlements,             //Commerce::getServiceEntitlements() function
                                                        /// <summary>Not implemented</summary>
            CommerceConsumeServiceEntitlement,          //Commerce::getConsumeServiceEntitlement() function
                                                        /// <summary>Not implemented</summary>
            CommerceDisplayCategoryBrowseDialog,        //Commerce::displayCategoryBrowseDialog() function
                                                        /// <summary>Not implemented</summary>
            CommerceDisplayProductBrowseDialog,         //Commerce::displayProductsBrowseDialog() function
                                                        /// <summary>Not implemented</summary>
            CommerceDisplayVoucherCodeInputDialog,      //Commerce::displayVoucherCodeInputDialog() function
                                                        /// <summary>Not implemented</summary>
            CommerceDisplayCheckoutDialog,              //Commerce::displayCheckoutDialog() function
                                                        /// <summary>Not implemented</summary>
            CommerceDisplayJoinPlusDialog,              //Commerce::displayJoinPlusDialog() function
                                                        /// <summary>Not implemented</summary>
            CommerceDisplayDownloadListDialog,          //Commerce::displayDownloadListDialog() function
                                                        /// <summary>Not implemented</summary>
            CommerceSetPsStoreIconDisplayState,         //Commerce::setPsStoreIconDisplayState() function

            /// <summary>Not implemented</summary>
            EventsClientGetEvent,                       //EventsClient::getEvent() function

            /// <summary>Used by <see cref="Friends.GetFriends"/></summary>
            FriendsGetFriends,                          //Friend::getFriends() function
                                                        /// <summary>Used by <see cref="Friends.GetFriendsOfFriends"/></summary>
            FriendsGetFriendsOfFriends,                 //Friend::getFriendsOfFriends() function
                                                        /// <summary>Used by <see cref="Friends.GetBlockedUsers"/></summary>
            FriendsGetBlockedUsers,                     //Friend::getBlockedUsers() function
                                                        /// <summary>Used by <see cref="Friends.DisplayFriendRequestDialog"/></summary>
            FriendsDisplayFriendRequestDialog,          //Friend::displayFriendRequestDialog() function
                                                        /// <summary>Used by <see cref="Friends.DisplayBlockUserDialog"/></summary>
            FriendsDisplayBlockUserDialog,              //Friend::displayBlockUserDialog() function

            /// <summary>Not implemented</summary>
            MatchingSetInitConfiguration,               //Matching::setInitConfiguration() function
                                                        /// <summary>Not implemented</summary>
            MatchingGetWorlds,                          //Matching::getWorlds() function
                                                        /// <summary>Not implemented</summary>
            MatchingCreateRoom,                         //Matching::createRoom() function
                                                        /// <summary>Not implemented</summary>
            MatchingLeaveRoom,                          //Matching::leaveRoom() function
                                                        /// <summary>Not implemented</summary>
            MatchingSearchRooms,                        //Matching::searchRooms() function
                                                        /// <summary>Not implemented</summary>
            MatchingJoinRoom,                           //Matching::joinRoom() function
                                                        /// <summary>Not implemented</summary>
            MatchingGetRoomPingTime,                    //Matching::getRoomPingTime() function
                                                        /// <summary>Not implemented</summary>
            MatchingKickOutRoomMember,                  //Matching::kickOutRoomMember() function
                                                        /// <summary>Not implemented</summary>
            MatchingSendRoomMessage,                    //Matching::sendRoomMessage() function
                                                        /// <summary>Not implemented</summary>
            MatchingGetAttributes,                      //Matching::getAttributes() function
                                                        /// <summary>Not implemented</summary>
            MatchingSetRoomInfo,                        //Matching::setRoomInfo() function
                                                        /// <summary>Not implemented</summary>
            MatchingSendInvitation,                     //Matching::sendInvitation() function
                                                        /// <summary>Not implemented</summary>
            MatchingGetData,
            /// <summary>Added In SDK 4.5</summary>
            MatchingSetMembersAsRecentlyMet,

            /// <summary>Not implemented</summary>
            MessagingSendInGameMessage,                 //Messaging::sendInGameMessage() function

            /// <summary>Not implemented</summary>
            MessagingSendGameDataMessage,               //Messaging::sendGameDataMessage() function
                                                        /// <summary>Not implemented</summary>
            MessagingDisplayReceivedGameDataMessagesDialog, //Messaging::displayReceivedGameDataMessagesDialog() function
                                                            /// <summary>Not implemented</summary>
            MessagingGetReceivedGameDataMessages,       //Messaging::getReceivedGameDataMessages() function
                                                        /// <summary>Not implemented</summary>
            MessagingConsumeGameDataMessage,            //Messaging::consumeGameDataMessage() function
                                                        /// <summary>Not implemented</summary>
            MessagingGetGameDataMessageThumbnail,       //Messaging:getGameDataMessageThumbnail() function
                                                        /// <summary>Not implemented</summary>
            MessagingGetGameDataMessageAttachment,      //Messaging:getGameDataMessageAttachment() function

            /// <summary>Used by <see cref="NetworkUtils.GetBandwidthInfo"/></summary>
            NetworkUtilsGetBandwidthInfo,               //NetworkUtils::getBandwithInfo() function
                                                        /// <summary>Used by <see cref="NetworkUtils.GetBasicNetworkInfoInfo"/></summary>
            NetworkUtilsGetBasicNetworkInfo,            //NetworkUtils::getBasicNetworkInfo() function
                                                        /// <summary>Used by <see cref="NetworkUtils.GetDetailedNetworkInfo"/></summary>
            NetworkUtilsGetDetailedNetworkInfo,         //NetworkUtils::getDetailedNetworkInfo() function

            /// <summary>Used by <see cref="NpUtils.DisplaySigninDialog"/></summary>
            NpUtilsDisplaySigninDialog,                 //NpUtils::displaySigninDialog() function
                                                        /// <summary>Used by <see cref="NpUtils.SetTitleIdForDevelopment"/></summary>
            NpUtilsSetTitleIdForDevelopment,            //NpUtils::setTitleIdForDevelopment() function
                                                        /// <summary>Used by <see cref="NpUtils.CheckAvailablity"/></summary>
            NpUtilsCheckAvailability,                   //NpUtils::checkAvailability() function


            /// <summary>Used by <see cref="Presence.SetPresence"/></summary>
            PresenceSetPresence,                        //Presence::setPresence() function
                                                        /// <summary>Used by <see cref="Presence.GetPresence"/></summary>
            PresenceGetPresence,                        //Presence::getPresence() function
                                                        /// <summary>Used by <see cref="Presence.DeletePresence"/></summary>
            PresenceDeletePresence,                     //Presence::deletePresence() function

            /// <summary>Not implemented</summary>
            RankingSetScore,                            //Ranking::setScore() function
                                                        /// <summary>Not implemented</summary>
            RankingGetRangeOfRanks,                     //Ranking::getRangeOfRanks() function
                                                        /// <summary>Not implemented</summary>
            RankingGetFriendsRanks,                     //Ranking::getFriendsRanks() function
                                                        /// <summary>Not implemented</summary>
            RankingGetUsersRanks,                       //Ranking::getUsersRanks() function
                                                        /// <summary>Not implemented</summary>
            RankingSetGameData,                         //Ranking::setGameData() function
                                                        /// <summary>Not implemented</summary>
            RankingGetGameData,                         //Ranking::getGameData() function

            /// <summary>Not implemented</summary>
            SessionSendInvitation,                      //Session::sendInvitation() function
                                                        /// <summary>Not implemented</summary>
            SessionDisplayReceivedInvitationsDialog,    //Session::displayReceivedInvitationsDialog() function
                                                        /// <summary>Not implemented</summary>
            SessionGetReceivedInvitations,              //Session::getReceivedInvitations() function
                                                        /// <summary>Not implemented</summary>
            SessionGetInvitationData,                   //Session::getInvitationData() function
                                                        /// <summary>Not implemented</summary>
            SessionConsumeInvitation,                   //Session::consumeInvitation() function
                                                        /// <summary>Not implemented</summary>
            SessionGetData,                             //Session::getData() function
                                                        /// <summary>Not implemented</summary>
            SessionLeave,                               //Session::leave() function
                                                        /// <summary>Not implemented</summary>
            SessionUpdate,                              //Session::update() function
                                                        /// <summary>Not implemented</summary>
            SessionGetInfo,                             //Session::getInfo() function
                                                        /// <summary>Not implemented</summary>
            SessionJoin,                                //Session::join() function
                                                        /// <summary>Not implemented</summary>
            SessionSearch,                              //Session::search() function
                                                        /// <summary>Not implemented</summary>
            SessionCreate,                              //Session::create() function

            /// <summary>Not implemented</summary>
            SocialMediaPostMessageToFacebook,           //SocialMedia::postMessageToFacebook() function

            /// <summary>Not implemented</summary>
            SharedMediaGetScreenshots,                  //SharedMedia::getScreenshots() function
                                                        /// <summary>Not implemented</summary>
            SharedMediaGetBroadcasts,                   //SharedMedia::getBroadcasts() function
                                                        /// <summary>Not implemented</summary>
            SharedMediaGetVideos,                       //SharedMedia::getVideos() function

            /// <summary>Used by <see cref="Trophies.RegisterTrophyPack"/></summary>
            TrophyRegisterTrophyPack,                   //Trophy::registerTrophyPack() function
                                                        /// <summary>Used by <see cref="Trophies.UnlockTrophy"/></summary>
            TrophyUnlock,                               //Trophy::unlock() function
                                                        /// <summary>Used by <see cref="Trophies.SetScreenshot"/></summary>
            TrophySetScreenshot,                        //Trophy::setScreenshot() function
                                                        /// <summary>Used by <see cref="Trophies.GetUnlockedTrophies"/></summary>
            TrophyGetUnlockedTrophies,                  //Trophy::getUnlockedTrophies() function
                                                        /// <summary>Used by <see cref="Trophies.DisplayTrophyListDialog"/></summary>
            TrophyDisplayTrophyListDialog,              //Trophy::displayTrophyListDialog() function
                                                        /// <summary>Used by <see cref="Trophies.GetTrophyPackSummary"/></summary>
            TrophyGetTrophyPackSummary,                 //Trophy::getTrophyPackSummary() function
                                                        /// <summary>Used by <see cref="Trophies.GetTrophyPackGroup"/></summary>
            TrophyGetTrophyPackGroup,                   //Trophy::getTrophyPackGroup() function
                                                        /// <summary>Used by <see cref="Trophies.GetTrophyPackTrophy"/></summary>
            TrophyGetTrophyPackTrophy,                  //Trophy::getTrophyPackTrophy() function

            /// <summary>Not implemented</summary>
            TssGetData,                                 //TSS::getData() function

            /// <summary>Not implemented</summary>
            TusSetVariables,                            //TUS::setVariables() function
                                                        /// <summary>Not implemented</summary>
            TusGetVariables,                            //TUS::getVariables() function
                                                        /// <summary>Not implemented</summary>
            TusAddToAndGetVariable,                     //TUS::addToAndGetVariable() function
                                                        /// <summary>Not implemented</summary>
            TusSetData,                                 //TUS::setData() function
                                                        /// <summary>Not implemented</summary>
            TusGetData,                                 //TUS::getData() function
                                                        /// <summary>Not implemented</summary>
            TusDeleteData,                              //TUS::deleteData() function

            /// <summary>Used by <see cref="UserProfiles.GetNpProfiles"/></summary>
            UserProfileGetNpProfiles,                   //UserProfile::getNpProfiles() function
                                                        /// <summary>Used by <see cref="UserProfiles.GetVerifiedAccountsForTitle"/></summary>
            UserProfileGetVerifiedAccountsForTitle,     //UserProfile::getVerifiedAccountsForTitle() function
                                                        /// <summary>Used by <see cref="UserProfiles.DisplayUserProfileDialog"/></summary>
            UserProfileDisplayUserProfileDialog,        // < <c>UserProfile::displayUserProfileDialog() function
                                                        /// <summary>Used by <see cref="UserProfiles.DisplayGriefReportingDialog"/></summary>
            UserProfileDisplayGriefReportingDialog,     // < <c>UserProfile::displayGriefReportingDialog() function

            /// <summary>Not implemented</summary>
            WordfilterFilterComment,                    //Wordfilter::filterComment() function

            /// <summary>Not implemented</summary>
            CoreTerminateService,                       //Core::terminateService() function

            //Matching notifications
            /// <summary>Not implemented</summary>
            NotificationRefreshRoom,                    //The Response in the <c>CallbackEvent is <c>Response\<Matching::Notification::RefreshRoom\>
                                                        /// <summary>Not implemented</summary>
            NotificationNewRoomMessage,                 //The Response in the <c>CallbackEvent is <c>Response\<Matching::Notification::NewRoomMessage\>

            //In game message notifications. // Moved from here in SDK 4.5 to later in the enum.
            //NotificationNewInGameMessage,				//The Response in the <c>CallbackEvent is <c>Response\<Messaging::Notification::NewInGameMessage\>

            //Change on focus notifications
            /// <summary>The Response in the callback is <see cref="Core.EmptyResponse"/></summary>
            NotificationDialogOpened,                   //The Response in the <c>CallbackEvent is <c>Response\<Core::Empty\>
                                                        /// <summary>The Response in the callback is <see cref="Core.EmptyResponse"/></summary>
            NotificationDialogClosed,                   //The Response in the <c>CallbackEvent is <c>Response\<Core::Empty\>
                                                        /// <summary>The Response in the callback is <see cref="NpUtils.UserStateChangeResponse"/></summary>
            NotificationUserStateChange,                //The Response in the <c>CallbackEvent is <c>Response\<Core::Notification::UserStateChange\>
                                                        /// <summary>The Response in the callback is <see cref="NetworkUtils.NetStateChangeResponse"/></summary>
            NotificationNetStateChange,                 //The Response in the <c>CallbackEvent is <c>Response\<Core::Notification::NetStateChange\>

            //Server push notifications
            /// <summary>Not implemented</summary> // Moved here in SDK 4.5
            NotificationNewInGameMessage,               //The Response in the <c>CallbackEvent is <c>Response\<Messaging::Notification::NewInGameMessage\>
                                                        /// <summary>The Response in the callback is <see cref="Friends.FriendListUpdateResponse"/></summary>
            NotificationUpdateFriendsList,              //The Response in the <c>CallbackEvent is <c>Response\<Friend::Notification::UpdateFriendsList\>
                                                        /// <summary>Not implemented</summary>
            NotificationNewInvitation,                  //The Response in the <c>CallbackEvent is <c>Response\<Invitation::Notification::NewInvitation\>
                                                        /// <summary>Not implemented</summary>
            NotificationNewGameDataMessage,             //The Response in the <c>CallbackEvent is <c>Response\<Messaging::Notification::NewGameDataMessage\>
                                                        /// <summary>The Response in the callback is <see cref="Presence.PresenceUpdateResponse"/></summary>
            NotificationUpdateFriendPresence,           //The Response in the <c>CallbackEvent is <c>Response\<Friend::Notification::UpdateFriendPresence\>
                                                        /// <summary>The Response in the callback is <see cref="Friends.BlocklistUpdateResponse"/></summary>
            NotificationUpdateBlockedUsersList,         //The Response in the <c>CallbackEvent is <c>Response\<Friend::Notification::UpdateBlockedUsersList\>

            //Abort notifications
            /// <summary>The Response in the callback is <see cref="Core.EmptyResponse"/></summary>
            NotificationAborted,                        //The Response in the <c>CallbackEvent is <c>Response\<Core::Empty\>. See <c>Core::abortRequest()

            /// <summary>The number of function types</summary>	
            NumFunctionTypes,

            // Custom notification types. These aren't provided by NpToolkit, but are implemented in such a way as to look like normal response objects coming in from NpToolkit.

            /// <summary></summary>	
            NotificationSessionInvitationEvent,

            /// <summary></summary>	
            NotificationPlayTogetherHostEvent,

            /// <summary></summary>	
            NotificationGameCustomDataEvent,

            // Custom requests
            /// <summary></summary>	
            NpUtilsCheckPlus,

            /// <summary></summary>	
            NpUtilsGetParentalControlInfo
        };

        /// <summary>
        /// The base class contain common settings for all request classes
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public class RequestBase
        {
            internal ServiceTypes serviceType;
            internal FunctionTypes functionType;
            internal UInt32 serviceLabel;    // SceNpServiceLabel - Service Label for the service, as configured in DevNet Forms
            internal Core.UserServiceUserId userId;

            [MarshalAs(UnmanagedType.I1)]
            internal bool async = true;

            internal UInt32 padding = 1234;

            /// <summary>
            /// Returns a value representing the service that uses the request.
            /// </summary>
            public ServiceTypes ServiceType { get { return serviceType; } }

            /// <summary>
            /// Returns a value representing the function that uses the request.
            /// </summary>
            public FunctionTypes FunctionType { get { return functionType; } }

            /// <summary>
            /// Service Label for the service, as configured in DevNet Forms
            /// </summary>
            public UInt32 ServiceLabel
            {
                get { return serviceLabel; }
                set { serviceLabel = value; }
            }

            /// <summary>
            /// Calling user Id performing the request
            /// </summary>
            public Core.UserServiceUserId UserId
            {
                get { return userId; }
                set { userId = value; }
            }

            /// <summary>
            /// Way the request will be performed: asynchronous or synchronous
            /// </summary>
            public bool Async
            {
                get { return async; }
                set { async = value; }
            }

            /// <summary>
            /// Initialise the class with its service type and function type.
            /// </summary>
            /// <param name="serviceType">The service type.</param>
            /// <param name="functionType">The function type.</param>
            internal RequestBase(ServiceTypes serviceType, FunctionTypes functionType)
            {
                userId.id = -1;
                //serviceLabel = 0; 
                this.serviceType = serviceType;
                this.functionType = functionType;
            }

            static internal void FinaliseRequest(RequestBase request, ResponseBase response, int npRequestId)
            {
                if (request.async == false)
                {
                    // If the request is blocking, fetch the results back from the plugin immediately.
                    response.PopulateFromNative((UInt32)npRequestId, request.functionType, request);
                }
                else
                {
                    // If async request add the request and response to their pending lists
                    // and update the state of the response object as it will be locked.
                    PendingAsyncRequestList.AddRequest((UInt32)npRequestId, request);
                    PendingAsyncResponseList.AddResponse((UInt32)npRequestId, response);
                    response.UpdateAsyncState((UInt32)npRequestId, request.functionType);
                }
            }
        }

        /// <summary>
        /// Details on a request that is currently in the async queue waiting to be completed.
        /// </summary>
        public class PendingRequest
        {
            internal UInt32 npRequestId;
            internal RequestBase request;
            internal bool abortPending;

            /// <summary>
            /// The unique request id
            /// </summary>
            public UInt32 NpRequestId { get { return npRequestId; } }

            /// <summary>
            /// The request object containing the request settings
            /// </summary>
            public RequestBase Request { get { return request; } }

            /// <summary>
            /// If the request has been aborted but is still in the pending list awaiting removal.
            /// </summary>
            public bool AbortPending { get { return abortPending; } }
        }

        /// <summary>
        /// Used to store the request id returned by some of the NpToolkit methods.
        /// This Id can be used to abort the request and remove it from the internal NpToolkit queue.
        /// </summary>
        internal static class PendingAsyncRequestList
        {
            // Contains a dictionary of requests for quick lookup
            static Dictionary<UInt32, PendingRequest> requestsLookup = new Dictionary<UInt32, PendingRequest>();

            // Contains a list of pending requests that can be access via the C# interface
            static List<PendingRequest> pendingRequests = new List<PendingRequest>();

            private static Object syncObject = new Object();

            /// <summary>
            /// Get a 'copy' of the pending list. This is safe to enumerate
            /// and won't corrupt the internal list
            /// </summary>
            static public List<PendingRequest> PendingRequests
            {
                get
                {
                    lock (syncObject)
                    {
                        List<PendingRequest> copy = new List<PendingRequest>(pendingRequests);
                        return copy;
                    }
                }
            }

            static internal void Shutdown()
            {
                lock (syncObject)
                {
                    pendingRequests.Clear();
                }
            }

            /// <summary>
            /// Check if a request id is in the pending queue waiting to be processed.
            /// </summary>
            /// <param name="npRequestId">The request id to check.</param>
            /// <returns>Retruns true if the request is in the queue.</returns>
            static public bool IsPending(UInt32 npRequestId)
            {
                lock (syncObject)
                {
                    return requestsLookup.ContainsKey(npRequestId);
                }
            }

            static internal void AddRequest(UInt32 npRequestId, RequestBase request)
            {
                lock (syncObject)
                {
                    PendingRequest pending = new PendingRequest();
                    pending.npRequestId = npRequestId;
                    pending.request = request;
                    pending.abortPending = false;

                    requestsLookup.Add(npRequestId, pending);
                    pendingRequests.Add(pending);
                }
            }

            static internal RequestBase RemoveRequest(UInt32 npRequestId)
            {
                lock (syncObject)
                {
                    PendingRequest pending = null;

                    if (requestsLookup.TryGetValue(npRequestId, out pending) == false)
                    {
                        return null;
                    }

                    requestsLookup.Remove(npRequestId);
                    pendingRequests.Remove(pending);

                    return pending.request;
                }
            }

            static internal bool MarkRequestAsAborting(UInt32 npRequestId)
            {
                lock (syncObject)
                {
                    PendingRequest pending = null;

                    if (requestsLookup.TryGetValue(npRequestId, out pending) == false)
                    {
                        return false;
                    }

                    pending.abortPending = true;

                    return true;
                }
            }

            static internal void RequestHasBeenAborted(UInt32 npRequestId)
            {
                lock (syncObject)
                {
                    RemoveRequest(npRequestId);
                }
            }
        }
    }
}
