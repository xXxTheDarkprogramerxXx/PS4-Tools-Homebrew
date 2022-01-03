using System;
using System.Runtime.InteropServices;

namespace Sony
{
	namespace NP
	{
		/// <summary>
		/// Messaging service related functionality.
		/// </summary>
		public class Messaging
		{
			#region DLL Imports

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxSendInGameMessage(SendInGameMessageRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxDisplayReceivedGameDataMessagesDialog(DisplayReceivedGameDataMessagesDialogRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxSendGameDataMessage(SendGameDataMessageRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxConsumeGameDataMessage(ConsumeGameDataMessageRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetReceivedGameDataMessages(GetReceivedGameDataMessagesRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetGameDataMessageThumbnail(GetGameDataMessageThumbnailRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetGameDataMessageAttachment(GetGameDataMessageAttachmentRequest request, out APIResult result);

			#endregion

			#region Requests

			/// <summary>
			/// This class represents the request to send an in-game data message.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
			public class SendInGameMessageRequest : RequestBase
			{
				/// <summary>
				/// Maximum size of the <see cref="Message"/> array.
				/// </summary>
				public const int NP_IN_GAME_MESSAGE_DATA_SIZE_MAX = 512;

				internal UInt64 messageSize;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = NP_IN_GAME_MESSAGE_DATA_SIZE_MAX)]
				internal byte[] message = new byte[NP_IN_GAME_MESSAGE_DATA_SIZE_MAX];

				internal Core.NpAccountId recipientId;
				internal Core.PlatformType recipientPlatformType;

				/// <summary>
				/// The binary data message to be sent
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is larger than <see cref="NP_IN_GAME_MESSAGE_DATA_SIZE_MAX"/>.</exception>
				public byte[] Message
				{
					get 
					{
						if (messageSize == 0) return null;

						byte[] output = new byte[messageSize];

						Array.Copy(message, output, (int)messageSize);

						return output;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > NP_IN_GAME_MESSAGE_DATA_SIZE_MAX)
							{
								throw new NpToolkitException("The size of the array is larger than " + NP_IN_GAME_MESSAGE_DATA_SIZE_MAX);
							}

							value.CopyTo(message, 0);
							messageSize = (UInt64)value.Length;
						}
						else
						{
							messageSize = 0;
						}
					}
				}

				/// <summary>
				/// The recipient to receive the message
				/// </summary>
				public Core.NpAccountId RecipientId
				{
					get { return recipientId; }
					set { recipientId = value; }
				}

				/// <summary>
				/// The platform of the recipient receiving the message
				/// </summary>
				public Core.PlatformType RecipientPlatformType
				{
					get { return recipientPlatformType; }
					set { recipientPlatformType = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="SendInGameMessageRequest"/> class.
				/// </summary>
				public SendInGameMessageRequest()
					: base(ServiceTypes.Messaging, FunctionTypes.MessagingSendInGameMessage)
				{

				}
			}

			/// <summary>
			/// This request is sent to the library to open a dialog for the user "Game Alerts" (Game Data Messages).
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class DisplayReceivedGameDataMessagesDialogRequest : RequestBase
			{
				/// <summary>
				/// Initializes a new instance of the <see cref="DisplayReceivedGameDataMessagesDialogRequest"/> class.
				/// </summary>
				public DisplayReceivedGameDataMessagesDialogRequest()
					: base(ServiceTypes.Messaging, FunctionTypes.MessagingDisplayReceivedGameDataMessagesDialog)
				{

				}
			}

			/// <summary>
			/// This enum identifies the type of data a game data message can have.
			/// </summary>
			public enum GameCustomDataTypes
			{
				/// <summary> Incorrect value for a type of Game Data Message </summary>
				Invalid = 0,
				/// <summary> The Game Data Message has a URL (it will open a URL in the browser of the platform) </summary>
				Url,
				/// <summary> The Game Data Message has an attachment (it will open the application and trigger a system event of game data received)  </summary>
				Attachment
			}

			/// <summary>
			/// Class to set the thumbnail image of the message.
			/// </summary>
			public struct GameDataMessageImage
			{
				/// <summary>
				/// The maximum length of the path where the image is located
				/// </summary>
				public const int IMAGE_PATH_MAX_LEN = 255;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = IMAGE_PATH_MAX_LEN + 1)]
				internal string imgPath;

				/// <summary>
				/// The path of the image to upload to the Game Custom Data server. e.g. Application.streamingAssetsPath + "/PS4MessageImage.jpg"
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the path is more than <see cref="IMAGE_PATH_MAX_LEN"/> characters.</exception>
				public string ImgPath
				{
					get { return imgPath; }
					set
					{
						if (value.Length > IMAGE_PATH_MAX_LEN)
						{
							throw new NpToolkitException("The size of the image path string is more than " + IMAGE_PATH_MAX_LEN + " characters.");
						}
						imgPath = value;
					}
				}

				internal bool IsValid()
				{
					if (imgPath == null || imgPath.Length == 0)
					{
						return false;
					}

					return true;
				}
			}

			/// <summary>
			/// Class to set the localized information for the data (name and description).
			/// </summary>
			public struct LocalizedMetadata
			{
				/// <summary>
				/// Maximum size the localized name of the attachment can have
				/// </summary>
				public const int MAX_SIZE_DATA_NAME = 127;

				/// <summary>
				/// Maximum size the localized description of the attachment can have
				/// </summary>
				public const int MAX_SIZE_DATA_DESCRIPTION = 511;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
				internal string languageCode;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_SIZE_DATA_NAME + 1)]
				internal string name;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_SIZE_DATA_DESCRIPTION + 1)]
				internal string description;

				/// <summary>
				/// The localized language for the name and description. Five digits format (countryCode-language)
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
				/// The localized name in the language specified in <see cref="LanguageCode"/>
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the string is more than <see cref="MAX_SIZE_DATA_NAME"/> characters.</exception>
				public string Name
				{
					get { return name; }
					set
					{
						if (value.Length > MAX_SIZE_DATA_NAME)
						{
							throw new NpToolkitException("The size of the string is more than " + MAX_SIZE_DATA_NAME + " characters.");
						}
						name = value;
					}
				}

				/// <summary>
				/// The localized description in the language specified in <see cref="LanguageCode"/>
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the string is more than <see cref="MAX_SIZE_DATA_DESCRIPTION"/> characters.</exception>
				public string Description
				{
					get { return description; }
					set
					{
						if (value.Length > MAX_SIZE_DATA_DESCRIPTION)
						{
							throw new NpToolkitException("The size of the string is more than " + MAX_SIZE_DATA_DESCRIPTION + " characters.");
						}
						description = value;
					}
				}
			}

			/// <summary>
			/// This request class is used to send a game data message. The data can either be a Url or an attachment.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class SendGameDataMessageRequest : RequestBase
			{
				/// <summary>
				/// The maximum size the text message to be send can have
				/// </summary>
				public const int MAX_SIZE_TEXT_MESSAGE = 511;
				
				/// <summary>
				/// The maximum size the name of the data (attachment/url) can have
				/// </summary>
				public const int MAX_SIZE_DATA_NAME = LocalizedMetadata.MAX_SIZE_DATA_NAME;

				/// <summary>
				/// The maximum size the description of the data (attachment/url) can have
				/// </summary>
				public const int MAX_SIZE_DATA_DESCRIPTION = LocalizedMetadata.MAX_SIZE_DATA_DESCRIPTION;

				/// <summary>
				/// The maximum number of recipients that can receive the message
				/// </summary>
				public const int MAX_NUM_RECIPIENTS = 16;   // SCE_GAME_CUSTOM_DATA_DIALOG_ADDRESS_USER_LIST_MAX_SIZE

				/// <summary>
				/// The maximum size the data attachment can have
				/// </summary>
				public const int MAX_SIZE_ATTACHMENT = 1 * 1024 * 1024;

				/// <summary>
				/// The maximum size the data url can have
				/// </summary>
				public const int MAX_URL_SIZE = 1023; 
				
				/// <summary>
				/// The maximum size the localized metadata array
				/// </summary>
				public const int MAX_LOCALIZED_METADATA = 50; 			

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_SIZE_TEXT_MESSAGE + 1)]
				internal string textMessage;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_SIZE_DATA_NAME + 1)]
				internal string dataName;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_SIZE_DATA_DESCRIPTION + 1)]
				internal string dataDescription;

				internal UInt32 numRecipients;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_NUM_RECIPIENTS)]
				internal Core.NpAccountId[] recipients = new Core.NpAccountId[MAX_NUM_RECIPIENTS];

				internal GameCustomDataTypes dataType;

				internal UInt32 expireMinutes;

				[MarshalAs(UnmanagedType.LPArray)]
				internal byte[] attachment;
				internal UInt64 attachmentSize;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_URL_SIZE + 1)]
				internal string url;

				internal UInt64 numDataLocalized;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_LOCALIZED_METADATA)]
				internal LocalizedMetadata[] localizedMetaData = new LocalizedMetadata[MAX_LOCALIZED_METADATA];

				internal GameDataMessageImage thumbnail;

				internal UInt32 maxNumberRecipientsToAdd;

				[MarshalAs(UnmanagedType.I1)]
				bool enableDialog;

				[MarshalAs(UnmanagedType.I1)]
				bool senderCanEditRecipients;

				[MarshalAs(UnmanagedType.I1)]
				bool isPS4Available;

				[MarshalAs(UnmanagedType.I1)]
				bool isPSVitaAvailable;

				[MarshalAs(UnmanagedType.I1)]
				bool addGameDataMsgIdToUrl;	


				/// <summary>
				/// The text message to be sent
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the string is more than <see cref="MAX_SIZE_TEXT_MESSAGE"/> characters.</exception>
				public string TextMessage
				{
					get { return textMessage; }
					set
					{
						if (value.Length > MAX_SIZE_TEXT_MESSAGE)
						{
							throw new NpToolkitException("The size of the string is more than " + MAX_SIZE_TEXT_MESSAGE + " characters.");
						}
						textMessage = value;
					}
				}

				/// <summary>
				/// The name of the data (either the attachment or the url data)
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the string is more than <see cref="MAX_SIZE_DATA_NAME"/> characters.</exception>
				public string DataName
				{
					get { return dataName; }
					set
					{
						if (value.Length > MAX_SIZE_DATA_NAME)
						{
							throw new NpToolkitException("The size of the string is more than " + MAX_SIZE_DATA_NAME + " characters.");
						}
						dataName = value;
					}
				}

				/// <summary>
				/// The description of the data (either the attachment or the url data)
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the string is more than <see cref="MAX_SIZE_DATA_DESCRIPTION"/> characters.</exception>
				public string DataDescription
				{
					get { return dataDescription; }
					set
					{
						if (value.Length > MAX_SIZE_DATA_DESCRIPTION)
						{
							throw new NpToolkitException("The size of the string is more than " + MAX_SIZE_DATA_DESCRIPTION + " characters.");
						}
						dataDescription = value;
					}
				}

				/// <summary>
				/// The recipients to send the game custom data to. If the sender user can edit the recipients, leave blank. Otherwise, mandatory
				/// </summary>
				public Core.NpAccountId[] Recipients
				{
					get
					{
						if (numRecipients == 0) return null;

						Core.NpAccountId[] output = new Core.NpAccountId[numRecipients];

						Array.Copy(recipients, output, (int)numRecipients);

						return output;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_NUM_RECIPIENTS)
							{
								throw new NpToolkitException("The size of the array is larger than " + MAX_NUM_RECIPIENTS);
							}
							value.CopyTo(recipients, 0);
							numRecipients = (UInt32)value.Length;
						}
						else
						{
							numRecipients = 0;
						}
					}
				}

				/// <summary>
				/// If the custom data to be send is an attachment or a url. Will be set to the message type when either <see cref="Attachment"/> or <see cref="Url"/> is set.
				/// </summary>
				public GameCustomDataTypes DataType
				{
					get { return dataType; }
				}

				/// <summary>
				/// The attachment data for an attachment type message. Will set <see cref="DataType"/> to <see cref="GameCustomDataTypes.Attachment"/>
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is larger than <see cref="MAX_SIZE_ATTACHMENT"/>.</exception>
				public byte[] Attachment
				{
					get { return attachment; }
					set
					{
						if (value.Length > MAX_SIZE_ATTACHMENT)
						{
							throw new NpToolkitException("The size of the array is larger than " + MAX_SIZE_ATTACHMENT);
						}

						attachment = value;
						attachmentSize = (value != null) ? (UInt64)value.Length : 0;
						dataType = GameCustomDataTypes.Attachment;
					}
				}

				/// <summary>
				/// The minutes for this message to be valid. Specify 0 if there is no expiration.
				/// </summary>
				public UInt32 ExpireMinutes
				{
					get { return expireMinutes; }
					set { expireMinutes = value; }
				}

				/// <summary>
				/// If the custom data is a URL, specify the URL here. Will set <see cref="DataType"/> to <see cref="GameCustomDataTypes.Url"/>
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the string is more than <see cref="MAX_URL_SIZE"/> characters.</exception>
				public string Url
				{
					get { return url; }
					set
					{
						if (value.Length > MAX_URL_SIZE)
						{
							throw new NpToolkitException("The size of the string is more than " + MAX_URL_SIZE + " characters.");
						}
						url = value;
						dataType = GameCustomDataTypes.Url;
					}
				}

				/// <summary>
				/// Information for localization. Includes the language code, the name and the description of the data. If the system language of the recipient matches a localized language, the localized information will be retrieved by the APIs and shown by the system. Otherwise, the default information (non-localized) will be used
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is more than <see cref="MAX_LOCALIZED_METADATA"/> characters.</exception>
				public LocalizedMetadata[] LocalizedMetaData
				{
					get 
					{
						if (numDataLocalized == 0) return null;

						LocalizedMetadata[] output = new LocalizedMetadata[numDataLocalized];

						Array.Copy(localizedMetaData, output, (int)numDataLocalized);

						return output;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_LOCALIZED_METADATA)
							{
								throw new NpToolkitException("The size of the localized game metadata array is more than " + MAX_LOCALIZED_METADATA);
							}

							localizedMetaData = value;
							numDataLocalized = (UInt64)value.Length;
						}
						else
						{
							numDataLocalized = 0;
						}
					}
				}

				/// <summary>
				/// The thumbnail image that goes in the message. It is mandatory
				/// </summary>
				public GameDataMessageImage Thumbnail
				{
					get { return thumbnail; }
					set { thumbnail = value; }
				}

				/// <summary>
				/// If the dialog is enabled and the sender user can edit the recipients, then the maximum number of recipients the sender user can add must be specified
				/// </summary>
				public UInt32 MaxNumberRecipientsToAdd
				{
					get { return maxNumberRecipientsToAdd; }
					set { maxNumberRecipientsToAdd = value; }
				}

				/// <summary>
				/// If a dialog is displayed to the sender user or the message is directly sent from the application without user interaction
				/// </summary>
				public bool EnableDialog
				{
					get { return enableDialog; }
					set { enableDialog = value; }
				}

				/// <summary>
				/// In case the dialog is enabled <see cref="EnableDialog"/>, this boolean specifies if the sender user can modify the recipients. If true then set <see cref="MaxNumberRecipientsToAdd"/>. If false then set <see cref="Recipients"/>
				/// </summary>
				public bool SenderCanEditRecipients
				{
					get { return senderCanEditRecipients; }
					set { senderCanEditRecipients = value; }
				}

				/// <summary>
				/// If the message is sent to a PS4 platform
				/// </summary>
				public bool IsPS4Available
				{
					get { return isPS4Available; }
					set { isPS4Available = value; }
				}

				/// <summary>
				/// If the message is sent to a PS Vita platform
				/// </summary>
				public bool IsPSVitaAvailable
				{
					get { return isPSVitaAvailable; }
					set { isPSVitaAvailable = value; }
				}

				/// <summary>
				/// When set to true, it will append the id at the end of the URL specified in <see cref="Url"/> so the custom data message Id can be sent on the URL, perhaps to be recognized by the hosting server
				/// </summary>
				public bool AddGameDataMsgIdToUrl
				{
					get { return addGameDataMsgIdToUrl; }
					set { addGameDataMsgIdToUrl = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="SendGameDataMessageRequest"/> class.
				/// </summary>
				public SendGameDataMessageRequest()
					: base(ServiceTypes.Messaging, FunctionTypes.MessagingSendGameDataMessage)
				{

				}
			}

			/// <summary>
			/// Request class to set a game data message as used.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class ConsumeGameDataMessageRequest : RequestBase
			{
				UInt64 gameDataMsgId;

				/// <summary>
				/// The game data message Id of the game data message to retrieve the thumbnail image from
				/// </summary>
				public UInt64 GameDataMsgId
				{
					get { return gameDataMsgId; }
					set { gameDataMsgId = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="ConsumeGameDataMessageRequest"/> class.
				/// </summary>
				public ConsumeGameDataMessageRequest()
					: base(ServiceTypes.Messaging, FunctionTypes.MessagingConsumeGameDataMessage)
				{

				}
			}

			/// <summary>
			/// This enum represents the way the game data messages to be retrieved will be requested.
			/// </summary>
			public enum GameDataMessagesToRetrieve
			{
				/// <summary> When obtaining game data messages, specify this option if the game custom data Ids of those messages are going to be provided in the request </summary>
				FromGameDataMsgIds = 0,
				/// <summary> When obtaining game data messages, specify this option if all game custom data Ids want to be retrieved. Pagination will need to be specified </summary>
				All = 1
			};

			/// <summary>
			/// This request class is used to get a set of game data messages that have been received by the calling user.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class GetReceivedGameDataMessagesRequest : RequestBase
			{
				/// <summary>
				/// The maximum number of game data Id messages that can be specified to be retrieved
				/// </summary>
				public const int MAX_NUM_GAME_DATA_MSG_IDS = 20;

				/// <summary>
				/// The maximum number of a page containing game data messages in case all of them need to be retrieved
				/// </summary>
				public const int MAX_PAGE_SIZE = 100;

				internal UInt64 numGameDataMsgIds;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_NUM_GAME_DATA_MSG_IDS)]
				internal UInt64[] gameDataMsgIds = new UInt64[MAX_NUM_GAME_DATA_MSG_IDS];

				internal UInt32 pageSize;
				internal UInt32 offset;
				internal GameDataMessagesToRetrieve retrieveType;

				/// <summary>
				/// In case the game data message Ids are provided, specify them here. <see cref="RetrieveType"/> will be <see cref="GameDataMessagesToRetrieve.FromGameDataMsgIds"/>.
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is more than <see cref="MAX_NUM_GAME_DATA_MSG_IDS"/> characters.</exception>
				public UInt64[] GameDataMsgIds
				{
					get 
					{
						if (numGameDataMsgIds == 0) return null;

						UInt64[] output = new UInt64[numGameDataMsgIds];

						Array.Copy(gameDataMsgIds, output, (int)numGameDataMsgIds);

						return output;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_NUM_GAME_DATA_MSG_IDS)
							{
								throw new NpToolkitException("The size of the array is more than " + MAX_NUM_GAME_DATA_MSG_IDS);
							}

							gameDataMsgIds = value;
							numGameDataMsgIds = (UInt64)value.Length;
						}
						else
						{
							numGameDataMsgIds = 0;
						}

						if (numGameDataMsgIds > 0) retrieveType = GameDataMessagesToRetrieve.FromGameDataMsgIds;
						else retrieveType = GameDataMessagesToRetrieve.All;
					}
				}

				/// <summary>
				/// In case all game data messages want to be retrieved, specify the page size here. <see cref="RetrieveType"/> will be <see cref="GameDataMessagesToRetrieve.All"/>.
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if value is more than <see cref="MAX_PAGE_SIZE"/>.</exception>
				public UInt32 PageSize
				{
					get { return pageSize; }
					set
					{
						pageSize = value;
						retrieveType = GameDataMessagesToRetrieve.All;
					}
				}

				/// <summary>
				/// In case all game data messages want to be retrieved, specify the first element to be retrieved here. <see cref="RetrieveType"/> will be <see cref="GameDataMessagesToRetrieve.All"/>. The first element is 0
				/// </summary>
				public UInt32 Offset
				{
					get { return offset; }
					set
					{
						offset = value;
						retrieveType = GameDataMessagesToRetrieve.All;
					}
				}

				/// <summary>
				/// Game data messages can be provided in bulk (all of them) or just a selection of them (specifying the game data message Id). This is set when using either <see cref="GameDataMsgIds"/> or <see cref="PageSize"/> and <see cref="Offset"/>.
				/// </summary>
				public GameDataMessagesToRetrieve RetrieveType
				{
					get { return retrieveType; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetReceivedGameDataMessagesRequest"/> class.
				/// </summary>
				public GetReceivedGameDataMessagesRequest()
					: base(ServiceTypes.Messaging, FunctionTypes.MessagingGetReceivedGameDataMessages)
				{
					pageSize = MAX_PAGE_SIZE;
					offset = 0;
					retrieveType = GameDataMessagesToRetrieve.All;
				}
			}

			/// <summary>
			/// This request class is used to identify a game data message whose thumbnail image wants to be retrieved.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class GetGameDataMessageThumbnailRequest : RequestBase
			{
				UInt64 gameDataMsgId;

				/// <summary>
				/// The game data message Id of the game data message to retrieve the thumbnail image from
				/// </summary>
				public UInt64 GameDataMsgId
				{
					get { return gameDataMsgId; }
					set { gameDataMsgId = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetGameDataMessageThumbnailRequest"/> class.
				/// </summary>
				public GetGameDataMessageThumbnailRequest()
					: base(ServiceTypes.Messaging, FunctionTypes.MessagingGetGameDataMessageThumbnail)
				{

				}
			}

			/// <summary>
			/// This request class is used to identify a game data message whose attachment data wants to be retrieved.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class GetGameDataMessageAttachmentRequest : RequestBase
			{
				UInt64 gameDataMsgId;

				/// <summary>
				/// The game data message Id of the game data message whose attachment data wants to be retrieved
				/// </summary>
				public UInt64 GameDataMsgId
				{
					get { return gameDataMsgId; }
					set { gameDataMsgId = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetGameDataMessageAttachmentRequest"/> class.
				/// </summary>
				public GetGameDataMessageAttachmentRequest()
					: base(ServiceTypes.Messaging, FunctionTypes.MessagingGetGameDataMessageAttachment)
				{

				}
			}

			#endregion

			#region Send In Game Message

			/// <summary>
			/// This function sends an in-game message to the target user.
			/// </summary>
			/// <remarks>
			/// Usage of this method also requires the <see cref="ServerPushNotifications.NewInGameMessage"/> to be initialised to true, other it will result in error SCE_NP_IN_GAME_MESSAGE_ERROR_LIB_CONTEXT_NOT_FOUND
			/// </remarks>
			/// <param name="request">Parameters needed to send an in-game data message </param>
			/// <param name="response">This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int SendInGameMessage(SendInGameMessageRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxSendInGameMessage(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Display Received Game Data Messages Dialog

			/// <summary>
			/// This function opens the System Received Game Data Messages Dialog (also known as Game Alerts on the system).
			/// </summary>
			/// <param name="request">Parameters needed to open the Received Game Data Messages Dialog </param>
			/// <param name="response">This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int DisplayReceivedGameDataMessagesDialog(DisplayReceivedGameDataMessagesDialogRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxDisplayReceivedGameDataMessagesDialog(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Send Game Data Message

			/// <summary>
			/// This function sends a message that contains data defined by the application (game data message).
			/// </summary>
			/// <param name="request">Parameters needed to send a Game Data Message</param>
			/// <param name="response">This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int SendGameDataMessage(SendGameDataMessageRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				if (request.thumbnail.IsValid() == false)
				{
					throw new NpToolkitException("Request thumbnail image hasn't be defined. A message can't be created without an image.");
				}

				int ret = PrxSendGameDataMessage(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Consume Game Data Message

			/// <summary>
			/// This function is used to set a game data message as used, to keep a track of the game data messages already used by the application.
			/// </summary>
			/// <param name="request">Game data message Id to be set as used </param>
			/// <param name="response">This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int ConsumeGameDataMessage(ConsumeGameDataMessageRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxConsumeGameDataMessage(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Get Received Game Data Messages	

			/// <summary>
			/// This class represents details of a game data message.
			/// </summary>
			public class GameDataMessageDetails
			{
				internal string dataName;
				internal string dataDescription;
				internal string textMessage;

				/// <summary>
				/// The name of a game custom data message. In case the system language matches a localized name, the localized name is the one set. Otherwise, the default name is the one provided
				/// </summary>
				public string DataName { get { return dataName; } }

				/// <summary>
				/// The description of a game custom data message. In case the system language matches a localized description, the localized description is the one set. Otherwise, the default description is the one provided
				/// </summary>
				public string DataDescription { get { return dataDescription; } }

				/// <summary>
				/// The text message sent in the game custom data message. This parameter can't be localized
				/// </summary>
				public string TextMessage { get { return textMessage; } }

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.GameDataMessageDetailsBegin);
					buffer.ReadString(ref dataName);
					buffer.ReadString(ref dataDescription);
					buffer.ReadString(ref textMessage);
					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.GameDataMessageDetailsEnd);
				}
			}

			/// <summary>
			/// This class represents a game data message provided to the application.
			/// </summary>
			public class GameDataMessage
			{
				internal UInt64 gameDataMsgId;
				internal Core.OnlineUser fromUser = new Core.OnlineUser();

				internal string receivedDate;
				internal string expiredDate;
				internal bool isPS4Available;
				internal bool isPSVitaAvailable;

				internal GameCustomDataTypes dataType;

				internal string url;

				internal GameDataMessageDetails details;
				internal bool hasDetails;

				internal bool isUsed;

				/// <summary>
				/// The Id of the game data message
				/// </summary>
				public UInt64 GameDataMsgId { get { return gameDataMsgId; } }

				/// <summary>
				/// The sender of the game data message
				/// </summary>
				public Core.OnlineUser FromUser { get { return fromUser; } }

				/// <summary>
				/// The date the game data message was received by the recipient. The format is RFC3339 (ISO8601) fixed to UTC
				/// </summary>
				public string ReceivedDate { get { return receivedDate; } }

				/// <summary>
				/// If an expire date in minutes has been set (a value other than 0) the date for the expiration. The format is RFC3339 (ISO8601) fixed to UTC
				/// </summary>
				public string ExpiredDate { get { return expiredDate; } }

				/// <summary>
				/// If the game data message is available on PS4 platform
				/// </summary>
				public bool IsPS4Available { get { return isPS4Available; } }

				/// <summary>
				/// If the game data message is available on PS Vita platform
				/// </summary>
				public bool IsPSVitaAvailable { get { return isPSVitaAvailable; } }

				/// <summary>
				/// The type of data for the game data message. It can be an attachment (if the attachment wants to be received, a separate call needs to be made) or a URL, which is provided in this class
				/// </summary>
				public GameCustomDataTypes DataType { get { return dataType; } }

				/// <summary>
				/// If the game data message is of <see cref="DataType"/> is <see cref="GameCustomDataTypes.Url"/>, the URL set when sending the game data message. It includes the game custom data Id in the URL (as "itemId=") if that option was set to true when sending the game data message
				/// </summary>
				public string Url { get { return url; } }

				/// <summary>
				/// Only set when the game data messages were requested specifying their Ids (blank when <see cref="GameDataMessagesToRetrieve.All"/> was selected in the request class). It provides the text message, data name and data description of the game data message
				/// </summary>
				public GameDataMessageDetails Details { get { return details; } }			

				/// <summary>
				/// If the <see cref="Details"/> class was meant to be set or not. It will only be set when <see cref="GameDataMessagesToRetrieve.FromGameDataMsgIds"/> is specified as <see cref="GetReceivedGameDataMessagesRequest.RetrieveType"/> of request class
				/// </summary>
				public bool HasDetails { get { return hasDetails; } }

				/// <summary>
				/// If the game data message has been already used
				/// </summary>
				public bool IsUsed { get { return isUsed; } }

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.GameDataMessageBegin);

					gameDataMsgId = buffer.ReadUInt64();

					fromUser.Read(buffer);

					buffer.ReadString(ref receivedDate);
					buffer.ReadString(ref expiredDate);

					isPS4Available = buffer.ReadBool();
					isPSVitaAvailable = buffer.ReadBool();

					dataType = (GameCustomDataTypes)buffer.ReadUInt32();

					buffer.ReadString(ref url);

					hasDetails = buffer.ReadBool();
					if (hasDetails == true)
					{
						details = new GameDataMessageDetails();
						details.Read(buffer);
					}

					isUsed = buffer.ReadBool();

					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.GameDataMessageEnd);
				}
			}

			/// <summary>
			/// The response data class that contains game data messages retrieved.
			/// </summary>
			public class GameDataMessagesResponse : ResponseBase
			{
				internal GameDataMessage[] gameDataMessages;

				/// <summary>
				/// A list with the game data messages requested
				/// </summary>
				public GameDataMessage[] GameDataMessages { get { return gameDataMessages; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.GameDataMessagesBegin);

					UInt64 numGameDataMessages = readBuffer.ReadUInt64();

					gameDataMessages = new GameDataMessage[numGameDataMessages];

					for (UInt64 i = 0; i < numGameDataMessages; i++)
					{
						gameDataMessages[i] = new GameDataMessage();
						gameDataMessages[i].Read(readBuffer);
					}

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.GameDataMessagesEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function returns a list of game data messages received by the calling user.
			/// </summary>
			/// <param name="request">Parameters needed to get game data messages </param>
			/// <param name="response">This response contains a list of <c>GameDataMessage</c> objects.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetReceivedGameDataMessages(GetReceivedGameDataMessagesRequest request, GameDataMessagesResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetReceivedGameDataMessages(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Get Game Data Message Thumbnail

			/// <summary>
			/// TUS data that was returned from the TUS server
			/// </summary>
			public class GameDataMessageThumbnailResponse : ResponseBase
			{
				internal UInt64 gameDataMsgId;
				internal byte[] thumbnail = null;

				/// <summary>
				/// The Id of the game data message
				/// </summary>
				public UInt64 GameDataMsgId { get { return gameDataMsgId; } }

				/// <summary>
				/// The thumbnail image data of the game data message
				/// </summary>
				public byte[] Thumbnail { get { return thumbnail; } }

				/// <summary>
				/// The response data class that contains information about the thumbnail image.
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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.GameDataMessageThumbnailBegin);

					gameDataMsgId = readBuffer.ReadUInt64();

					readBuffer.ReadData(ref thumbnail);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.GameDataMessageThumbnailEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function returns the thumbnail of a game data message as an array of bytes.
			/// </summary>
			/// <param name="request">Parameters needed to get the thumbnail of a game data message </param>
			/// <param name="response">This response contains the return code and a pointer to the thumbnail of the game data message</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetGameDataMessageThumbnail(GetGameDataMessageThumbnailRequest request, GameDataMessageThumbnailResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetGameDataMessageThumbnail(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Get Game Data Message Attachment

			/// <summary>
			/// The response data class that contains information about the attachment provided in a game data message.
			/// </summary>
			public class GameDataMessageAttachmentResponse : ResponseBase
			{
				internal UInt64 gameDataMsgId;

				internal byte[] attachment;

				/// <summary>
				/// The Id of the game data message containing the attachment
				/// </summary>
				public UInt64 GameDataMsgId { get { return gameDataMsgId; } }

				/// <summary>
				/// The attachment data of the game data message.
				/// </summary>
				public byte[] Attachment { get { return attachment; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.GameDataMessageAttachmentBegin);

					gameDataMsgId = readBuffer.ReadUInt64();

					readBuffer.ReadData(ref attachment);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.GameDataMessageAttachmentEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function returns the attachment data of a game data message as an array of bytes.
			/// </summary>
			/// <param name="request">Parameters needed to get the attachment data of a game data message </param>
			/// <param name="response">This response contains the return code and a pointer to the attachment data of the game data message</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetGameDataMessageAttachment(GetGameDataMessageAttachmentRequest request, GameDataMessageAttachmentResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetGameDataMessageAttachment(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Notifications

			/// <summary>
			/// Notification sent by the library to the application when a new in-game message has been received.
			/// </summary>
			public class NewInGameMessageResponse : ResponseBase
			{
				internal byte[] message;
				internal Core.OnlineUser sender = new Core.OnlineUser();
				internal Core.OnlineUser recipient = new Core.OnlineUser();
				internal Core.PlatformType senderPlatformType;
				internal Core.PlatformType recipientPlatformType;

				/// <summary>
				/// The binary data message received
				/// </summary>
				public byte[] Message
				{
					get { return message; }
				}

				/// <summary>
				/// The user who sent the message
				/// </summary>
				public Core.OnlineUser Sender { get { return sender; } }

				/// <summary>
				/// The user who receives the message
				/// </summary>
				public Core.OnlineUser Recipient { get { return recipient; } }

				/// <summary>
				/// The platform used by the sender when the message was sent
				/// </summary>
				public Core.PlatformType SenderPlatformType { get { return senderPlatformType; } }

				/// <summary>
				/// The platform where the recipient needs to be to receive the message
				/// </summary>
				public Core.PlatformType RecipientPlatformType { get { return recipientPlatformType; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NewInGameMessageBegin);

					readBuffer.ReadData(ref message);

					sender.Read(readBuffer);
					recipient.Read(readBuffer);

					senderPlatformType = (Core.PlatformType)readBuffer.ReadUInt32();
					recipientPlatformType = (Core.PlatformType)readBuffer.ReadUInt32();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NewInGameMessageEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// Notification sent by the library to the application when a new in-game data message has been received.
			/// </summary>
			public class NewGameDataMessageResponse : ResponseBase
			{
				internal Core.OnlineUser to = new Core.OnlineUser();
				internal Core.OnlineUser from = new Core.OnlineUser();

				/// <summary>
				/// The user that the game data message was sent to
				/// </summary>
				public Core.OnlineUser To { get { return to; } }

				/// <summary>
				/// The sender of the game data message
				/// </summary>
				public Core.OnlineUser From { get { return from; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NewGameDataMessageBegin);

					to.Read(readBuffer);
					from.Read(readBuffer);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NewGameDataMessageEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// Notification that is received when a local player 'Accepts' a 'Game Alert' message.
			/// </summary>
			public class GameCustomDataEventResponse : ResponseBase
			{
				internal UInt64 itemId;
				internal Core.OnlineID onlineId = new Core.OnlineID();
				internal Core.UserServiceUserId userId;

				/// <summary>
				/// Game custom data ID
				/// </summary>
				public UInt64 ItemId { get { return itemId; } }

				/// <summary>
				/// Online ID of user who performed action
				/// </summary>
				public Core.OnlineID OnlineId { get { return onlineId; } }

				/// <summary>
				/// User ID of user who performed action
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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.GameCustomDataEventBegin);

					itemId = readBuffer.ReadUInt64();
					onlineId.Read(readBuffer);
					userId.Read(readBuffer);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.GameCustomDataEventEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			#endregion
		}
	}
}
