using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sony
{
	namespace NP
	{
		internal class Notifications
		{
			internal static ResponseBase CreateNotificationResponse(FunctionTypes notificationType)
			{
				ResponseBase response = null;

				switch (notificationType)
				{
					// Empty Response
					case FunctionTypes.NotificationAborted:
					case FunctionTypes.NotificationDialogOpened:
					case FunctionTypes.NotificationDialogClosed:
						response = new Core.EmptyResponse();
						break;
					case FunctionTypes.NotificationRefreshRoom:
						response = new Matching.RefreshRoomResponse();
						break;
					case FunctionTypes.NotificationNewRoomMessage:
						response = new Matching.NewRoomMessageResponse();
						break;
					case FunctionTypes.NotificationNewInGameMessage:
						response = new Messaging.NewInGameMessageResponse();
						break;
					case FunctionTypes.NotificationNewGameDataMessage:
						response = new Messaging.NewGameDataMessageResponse();
						break;
					case FunctionTypes.NotificationUserStateChange:
						response = new NpUtils.UserStateChangeResponse();
						break;
					case FunctionTypes.NotificationNetStateChange:
						response = new NetworkUtils.NetStateChangeResponse();
						break;
					case FunctionTypes.NotificationUpdateFriendsList:
						response = new Friends.FriendListUpdateResponse();
						break;
					case FunctionTypes.NotificationUpdateFriendPresence:
						response = new Presence.PresenceUpdateResponse();
						break;
					case FunctionTypes.NotificationUpdateBlockedUsersList:
						response = new Friends.BlocklistUpdateResponse();
						break;
					case FunctionTypes.NotificationNewInvitation:
						response = new Matching.InvitationReceivedResponse();
						break;
					case FunctionTypes.NotificationSessionInvitationEvent:
						response = new Matching.SessionInvitationEventResponse();
						break;
					case FunctionTypes.NotificationPlayTogetherHostEvent:
						response = new Matching.PlayTogetherHostEventResponse();
						break;
					case FunctionTypes.NotificationGameCustomDataEvent:
						response = new Messaging.GameCustomDataEventResponse();
						break;
										
					default:
						break;
				}

				return response;
			}
		}
	}
}
