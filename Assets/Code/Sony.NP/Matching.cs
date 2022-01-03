using System;
using System.Runtime.InteropServices;

namespace Sony
{
	namespace NP
	{
		/// <summary>
		/// Matching service related functionality.
		/// </summary>
		public class Matching
		{
			#region DLL Imports

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxSetInitConfiguration(SetInitConfigurationRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetWorlds(GetWorldsRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxCreateRoom(CreateRoomRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxLeaveRoom(LeaveRoomRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxSearchRooms(SearchRoomsRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxJoinRoom(JoinRoomRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetRoomPingTime(GetRoomPingTimeRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxKickOutRoomMember(KickOutRoomMemberRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxSendRoomMessage(SendRoomMessageRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetAttributes(GetAttributesRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxSetRoomInfo(SetRoomInfoRequest request, out APIResult result);

#if SUPPORT_RECENTLY_MET
			[DllImport("UnityNpToolkit2")]
			private static extern int PrxSetMembersAsRecentlyMet(SetMembersAsRecentlyMetRequest request, out APIResult result);
#endif

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxSendInvitation(SendInvitationRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetData(GetDataRequest request, out APIResult result);		

			#endregion

			#region Common types

			/// <summary>
			/// The invalid room member id
			/// </summary>
			public const int INVALID_ROOM_MEMBER_ID = 0;      // SCE_NP_MATCHING2_INVALID_ROOM_MEMBER_ID

			/// <summary>
			///  Enum listing the different types the value of an attribute can have.
			/// </summary>
			/// <remarks>
			/// Enum listing the different types the value of an attribute
			/// can have. The value can be, either an integer value or a
			/// binary value (in this case, the size will be determined by
			/// the metadata information).
			/// </remarks>
			public enum AttributeType
			{
				/// <summary> Invalid type. This means the attribute type is not set. It should not be returned by APIs </summary>
				Invalid,
				/// <summary> Integer type. The attribute is an 8 bytes integer </summary>
				Integer,
				/// <summary> Binary type. The attribute is a buffer. The size of the buffer is specified as part of the metadata </summary>
				Binary,	
			}

			/// <summary>
			/// Enum listing the different scopes of an attribute.
			/// </summary>
			/// <remarks>
			/// Enum listing the different scopes of an attribute. Attributes
			/// can be either room attributes, where they will describe
			/// information about a room, or member attributes, where they
			/// will describe information about the member they belong.
			/// </remarks>
			public enum AttributeScope
			{
				/// <summary> Invalid scope. This means the attribute scope is not set. It should not be returned by APIs </summary>
				Invalid,
				/// <summary> The attribute belongs to the room. There will be only one attribute with that name on the room </summary>
				Room,
				/// <summary> The attribute belongs to a member of the room. Each member will have only one attribute with that name </summary>
				Member,	
			}

			/// <summary>
			/// This enum lists the visibility of a room attribute.
			/// </summary>
			public enum RoomAttributeVisibility
			{
				/// <summary> Invalid attribute room visibility. This means the visibility is not set. It should not be returned by APIs </summary>
				Invalid,
				/// <summary> The attribute is only visible to members that are part of the room </summary>
				Internal,
				/// <summary> The attribute is visible to members inside the rooms and users outside. It is returned in searches, but it cannot be used as a clause on a search  </summary>
				External,	
				/// <summary> The attribute is visible to members inside the rooms and users outside. It can be used as input in searches as well  </summary>
				Search
			}
			
			/// <summary>
			/// The different degrees of visibility of a room.
			/// </summary>
			public enum RoomVisibility
			{
				/// <summary> Invalid visibility. This is the default value. In case the visibility does not want to be modified when setting new information for the room, this value will identify the non-modification.</summary>
				Invalid,
				/// <summary> The room is public. It is available on searches and can be joined by any user.</summary>
				PublicRoom,
				/// <summary> The room is private. It is not available on searches.</summary>
				PrivateRoom,
				/// <summary> The room is public but some slots will be private and only accessible for users with password. </summary>
				ReserveSlots
			}

			/// <summary>
			/// The type of room migration
			/// </summary>
			public enum RoomMigrationType
			{
				/// <summary> With onwerBind, the session will be deleted when the creator leaves. </summary>
				OwnerBind,
				/// <summary> With ownerMigration, the session will be deleted when the joined users reaches 0. </summary>
				OwnerMigration,
			}

			/// <summary>
			/// The different types of configurations the network can have in terms of topology.
			/// </summary>
			public enum TopologyType
			{
				/// <summary> Invalid topology. This is the default value. In case the topology does not want to be modified when setting new information for the room, this value will identify the non-modification. </summary>
				Invalid,
				/// <summary> The topology should be moved to none. In this case, a topology will be selected by the system. </summary>
				None,
				/// <summary> Mesh topology. Every member can connect directly to any other member inside a room. </summary>
				Mesh,
				/// <summary> Star topology. All members can connect only to one member (the host). That member will always be the room owner. </summary>
				Star
			}

			/// <summary>
			/// This represents metadata information of an attribute. Use <see cref="CreateIntegerAttribute"/> or <see cref="CreateBinaryAttribute"/> to create and initialise an attribute.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct AttributeMetadata
			{
				/// <summary>
				/// The maximum size of the name an attribute can have
				/// </summary>
				public const int MAX_SIZE_NAME = 31;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_SIZE_NAME+1)]
				internal string name;

				internal AttributeType type;
				internal AttributeScope scope;
				internal RoomAttributeVisibility roomAttributeVisibility;
				internal UInt32 size;

				/// <summary>
				/// The name of the attribute. It is its identifier. Defined by application on <see cref="SetInitConfiguration"/>. It must be set as input on all APIs taking attributes as data members of their requests
				/// </summary>
				public string Name
				{
					get { return name; }
				}

				/// <summary>
				/// The type of the attribute. Defined by application on <see cref="SetInitConfiguration"/>. It can be blank for the rest of inputs. Responses will always set it
				/// </summary>
				public AttributeType Type
				{
					get { return type; }
				}

				/// <summary>
				/// The scope of the attribute. Defined by application on <see cref="SetInitConfiguration"/>. It can be blank for the rest of inputs. Responses will always set it
				/// </summary>
				public AttributeScope Scope
				{
					get { return scope; }
				}

				/// <summary>
				/// If the <see cref="Scope"/> is <see cref="AttributeScope.Room"/>, the visibility of the room attribute. Defined by application on <see cref="SetInitConfiguration"/>. It can be blank for the rest of inputs. Responses will set it when applicable	
				/// </summary>
				public RoomAttributeVisibility RoomVisibility
				{
					get { return roomAttributeVisibility; }
				}

				/// <summary>
				/// The size of the attribute. In case the <see cref="Type"/> is <see cref="AttributeType.Binary"/>, defined by application on <see cref="SetInitConfiguration"/>. If <see cref="AttributeType.Integer"/>, the size is always 8. 
				/// </summary>
				public UInt32 Size
				{
					get { return size; }
				}

				private void InternalSetAttribute(string name, AttributeType type, AttributeScope scope, RoomAttributeVisibility roomAttributeVisibility, UInt32 size)
				{
					if (name.Length > MAX_SIZE_NAME)
					{
						throw new NpToolkitException("Attribute " + name + " : The size of the name string is more than " + MAX_SIZE_NAME + " characters.");
					}

					if (type == AttributeType.Invalid)
					{
						throw new NpToolkitException("Attribute " + name + " : Can't set an Invalid type.");
					}

					if (scope == AttributeScope.Invalid)
					{
						throw new NpToolkitException("Attribute " + name + " : Can't set an Invalid scope.");
					}

					if (scope == AttributeScope.Room && roomAttributeVisibility == Matching.RoomAttributeVisibility.Invalid)
					{
						throw new NpToolkitException("Attribute " + name + " : Can't set an Invalid roomAttributeVisibility when Scope is Room.");
					}

					if (type == AttributeType.Integer && size != 8)
					{
						throw new NpToolkitException("Attribute " + name + " : Integer attribute must be size 8.");
					}

					if (type == AttributeType.Binary && size > Attribute.MAX_SIZE_BIN_VALUE)
					{
						throw new NpToolkitException("Attribute " + name + " : Binary attribute size must not be more than " + Attribute.MAX_SIZE_BIN_VALUE);
					}
					
					if (scope == AttributeScope.Member && roomAttributeVisibility != Matching.RoomAttributeVisibility.Invalid)
					{
						throw new NpToolkitException("Attribute " + name + " : A Member attribute can't set a RoomAttributeVisibility of " + roomAttributeVisibility.ToString() + ". It must always be set to RoomAttributeVisibility.Invalid.");
					}

					if (roomAttributeVisibility == RoomAttributeVisibility.Search && type == AttributeType.Binary && size > 64)
					{
						throw new NpToolkitException("Attribute " + name + " : A Binary Search attribute can't be more than 64 bytes.");
					}
		
					this.name = name;
					this.type = type;
					this.scope = scope;
					this.roomAttributeVisibility = roomAttributeVisibility;
					this.size = size;
				}

				internal void Read(MemoryBuffer buffer)
				{
					buffer.ReadString(ref name);

					type = (AttributeType)buffer.ReadUInt32();
					scope = (AttributeScope)buffer.ReadUInt32();
					roomAttributeVisibility = (RoomAttributeVisibility)buffer.ReadUInt32();
					size = buffer.ReadUInt32();
				}

				/// <summary>
				/// Creates an integer type attribute.
				/// </summary>
				/// <param name="name">The name of the attribute. It is its identifier. It must be set as input on all APIs taking attributes as data members of their requests.</param>
				/// <param name="scope">The scope of the attribute.</param>
				/// <param name="roomAttributeVisibility">If the <see cref="scope"/> is <see cref="AttributeScope.Room"/>, the visibility of the room attribute, otherwise ignored.</param>
				/// <returns>A new attribute initialised as an <see cref="AttributeType.Integer"/> type.</returns>
				/// <exception cref="NpToolkitException">Will throw an exception if the string is more than <see cref="MAX_SIZE_NAME"/> characters.</exception>
				/// <exception cref="NpToolkitException">Will throw an exception if the <see cref="scope"/> is <see cref="AttributeScope.Invalid"/>.</exception>
				/// <exception cref="NpToolkitException">Will throw an exception if the <see cref="scope"/> is <see cref="AttributeScope.Room"/> and <see cref="roomAttributeVisibility"/> is <see cref="RoomAttributeVisibility.Invalid"/>.</exception>
				public static AttributeMetadata CreateIntegerAttribute(string name, AttributeScope scope, RoomAttributeVisibility roomAttributeVisibility)
				{
					AttributeMetadata attribute = new AttributeMetadata();

					// All interger attributes have a size of 8
					attribute.InternalSetAttribute(name,  AttributeType.Integer, scope, roomAttributeVisibility, 8);

					return attribute;
				}

				/// <summary>
				///  Creates a binary type attribute.
				/// </summary>
				/// <param name="name">The name of the attribute. It is its identifier. It must be set as input on all APIs taking attributes as data members of their requests.</param>
				/// <param name="scope">The scope of the attribute.</param>
				/// <param name="roomAttributeVisibility">If the <see cref="scope"/> is <see cref="AttributeScope.Room"/>, the visibility of the room attribute, otherwise ignored.</param>
				/// <param name="size"></param>
				/// <returns>A new attribute initialised as an <see cref="AttributeType.Binary"/> type.</returns>
				/// <exception cref="NpToolkitException">Will throw an exception if the string is more than <see cref="MAX_SIZE_NAME"/> characters.</exception>
				/// <exception cref="NpToolkitException">Will throw an exception if the <see cref="scope"/> is <see cref="AttributeScope.Invalid"/>.</exception>
				/// <exception cref="NpToolkitException">Will throw an exception if the <see cref="scope"/> is <see cref="AttributeScope.Room"/> and <see cref="roomAttributeVisibility"/> is <see cref="RoomAttributeVisibility.Invalid"/>.</exception>
				public static AttributeMetadata CreateBinaryAttribute(string name, AttributeScope scope, RoomAttributeVisibility roomAttributeVisibility, UInt32 size)
				{
					AttributeMetadata attribute = new AttributeMetadata();

					attribute.InternalSetAttribute(name, AttributeType.Binary, scope, roomAttributeVisibility, size);

					return attribute;
				}
			}

			/// <summary>
			/// A class representing an attribute, containing attribute metadata and a value.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct Attribute
			{
				/// <summary>
				/// The maximum size a value of any attribute can have. 
				/// </summary>
				public const int MAX_SIZE_BIN_VALUE = 256;	
															
				internal AttributeMetadata metadata;
				internal Int32 intValue;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SIZE_BIN_VALUE)]
				internal byte[] binValue;

				/// <summary>
				/// Metadata information of each attribute, including size of the attribute
				/// </summary>
				public AttributeMetadata Metadata
				{
					get { return metadata; }
				}

				/// <summary>
				/// If the attribute is of type <see cref="AttributeType.Integer"/>, the value of the attribute
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the value doesn't match the requirements defined in the metadata.</exception>
				public Int32 IntValue
				{
					get 
					{
						if (metadata.type != AttributeType.Integer)
						{
							throw new NpToolkitException("Attribute " + metadata.name + " : This is not an interger attribute type.");
						}
						return intValue; 
					}

					set 
					{
						if (metadata.type != AttributeType.Integer)
						{
							throw new NpToolkitException("Attribute " + metadata.name + " : Expecting an interger attribute type.");
						}

						intValue = value; 
					}
				}

				/// <summary>
				/// If the attribute is of type <see cref="AttributeType.Binary"/>, the value of the attribute
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the value doesn't match the requirements defined in the metadata.</exception>
				public byte[] BinValue
				{
					get 
					{
						if (metadata.type != AttributeType.Binary)
						{
							throw new NpToolkitException("Attribute " + metadata.name + " : This is not a binary attribute type.");
						}

						if (metadata.size == 0) return null;

						byte[] output = new byte[metadata.size];

						Array.Copy(binValue, output, metadata.size);

						return binValue; 
					}
					set 
					{
						if (metadata.type != AttributeType.Binary)
						{
							throw new NpToolkitException("Attribute " + metadata.name + " : Expected a binary attribute type.");
						}

						if (value == null)
						{
							throw new NpToolkitException("Attribute " + metadata.name + " : Expected a non-null byte array.");
						}

						if (value.Length > MAX_SIZE_BIN_VALUE)
						{
							throw new NpToolkitException("Attribute " + metadata.name + " : Binary array is more than " + MAX_SIZE_BIN_VALUE);
						}

						if (value.Length > metadata.size)
						{
							throw new NpToolkitException("Attribute " + metadata.name + " : Array size of " + value.Length + " can't exceed " + metadata.size + " bytes defined in metadata.");
						}

						if (binValue == null) // Done here as structs can't have field initialisers.
						{
							binValue = new byte[MAX_SIZE_BIN_VALUE];
						}

						value.CopyTo(binValue, 0);
					}
				}

				internal void Read(MemoryBuffer buffer)
				{
					metadata.Read(buffer);

					if ( metadata.type == AttributeType.Integer )
					{
						intValue = buffer.ReadInt32();
					}
					else if ( metadata.type == AttributeType.Binary )
					{
						buffer.ReadData(ref binValue);
					}
				}

				/// <summary>
				/// Creates an integer attribute.
				/// </summary>
				/// <param name="metadata">The interger type metadata.</param>
				/// <param name="intValue">The integer value of the attribute</param>
				/// <returns>A new interger attribute.</returns>
				/// <exception cref="NpToolkitException">Will throw an exception if the value doesn't match the requirements defined in the metadata.</exception>
				public static Attribute CreateIntegerAttribute(AttributeMetadata metadata, Int32 intValue)
				{
					Attribute attribute = new Attribute();

					attribute.metadata = metadata;
					attribute.IntValue = intValue; // Use the set property as this does validate checking

					return attribute;
				}

				/// <summary>
				/// Creates a binary attribute, which contains an array of bytes
				/// </summary>
				/// <param name="metadata">The binary type meta data.</param>
				/// <param name="binValue">The byte array value of the attribute.</param>
				/// <returns>A new binary attribute.</returns>
				/// <exception cref="NpToolkitException">Will throw an exception if the value doesn't match the requirements defined in the metadata.</exception>
				public static Attribute CreateBinaryAttribute(AttributeMetadata metadata, byte[] binValue)
				{
					Attribute attribute = new Attribute();

					attribute.metadata = metadata;
					attribute.BinValue = binValue;  // Use the set property as this does validate checking

					return attribute;
				}
			}

			/// <summary>
			/// The image that is shown for a session in the system software
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct SessionImage
			{
				/// <summary>
				/// Maximum length of the image path
				/// </summary>
				public const int IMAGE_PATH_MAX_LEN = 255;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = IMAGE_PATH_MAX_LEN + 1)]
				internal string sessionImgPath;

				/// <summary>
				/// The local path of the image to upload to the Session server. e.g. Application.streamingAssetsPath + "/PS4SessionImage.jpg"
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the path is more than <see cref="IMAGE_PATH_MAX_LEN"/> characters.</exception>
				public string SessionImgPath
				{
					get { return sessionImgPath; }
					set
					{
						if (value.Length > IMAGE_PATH_MAX_LEN)
						{
							throw new NpToolkitException("The size of the image path string is more than " + IMAGE_PATH_MAX_LEN + " characters.");
						}
						sessionImgPath = value;
					}
				}

				internal bool IsValid()
				{
					if (sessionImgPath == null || sessionImgPath.Length == 0)
					{
						return false;
					}

					return true;
				}

                internal bool Exists()
				{
					if (sessionImgPath == null || sessionImgPath.Length == 0)
					{
						return false;
					}

					return true;
				}
			}

			/// <summary>
			/// Localized session information. The localized session name and status will be displayed based on a users system software language setting.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct LocalizedSessionInfo
			{
				/// <summary>
				/// Maximum length of the session name
				/// </summary>
				public const int SESSION_NAME_LEN = 63;

				/// <summary>
				/// Maximum length of the status string
				/// </summary>
				public const int STATUS_LEN = 255;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = SESSION_NAME_LEN + 1)]
				string sessionName;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = STATUS_LEN + 1)]
				string status;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
				internal string languageCode;

				/// <summary>
				/// The session name. For example: "Team Death Match"
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the name is more than <see cref="SESSION_NAME_LEN"/> characters.</exception>
				public string SessionName
				{
					get { return sessionName; }
					set
					{
						if (value.Length > SESSION_NAME_LEN)
						{
							throw new NpToolkitException("The size of the session name is more than " + SESSION_NAME_LEN + " characters.");
						}
						sessionName = value;
					}
				}

				/// <summary>
				/// The status. For example: "Stage 1. Beginner only"
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the status is more than <see cref="SESSION_NAME_LEN"/> characters.</exception>
				public string Status
				{
					get { return status; }
					set
					{
						if (value.Length > STATUS_LEN)
						{
							throw new NpToolkitException("The size of the status string is more than " + STATUS_LEN + " characters.");
						}
						status = value;
					}
				}

				/// <summary>
				/// The localized session info provided is written in this language. Five digits format (countryCode-language)
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
				/// Initializes a new instance of the <see cref="LocalizedSessionInfo"/> class.
				/// </summary>
				public LocalizedSessionInfo(string sessionName, string status, Core.LanguageCode languageCode)
				{
					this.sessionName = "";
					this.status = "";
					this.languageCode = "";

					SessionName = sessionName;
					Status = status;
					LanguageCode = languageCode;
				}
			}

			/// <summary>
			/// Accompanying data when joining or leaving a room or lobby
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct PresenceOptionData
			{
				/// <summary>
				/// Maximum number of bytes in the data
				/// </summary>
				public const int NP_MATCHING2_PRESENCE_OPTION_DATA_SIZE = 16;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = NP_MATCHING2_PRESENCE_OPTION_DATA_SIZE)]
				internal byte[] data;

				[MarshalAs(UnmanagedType.U1)]
				internal byte length;  // Maximum length of SCE_NP_MATCHING2_PRESENCE_OPTION_DATA_SIZE is currently only 16, so a byte is large enough to contain the size

				internal void Init()
				{
					data = new byte[NP_MATCHING2_PRESENCE_OPTION_DATA_SIZE];
				}

				/// <summary>
				/// Optional presence data. Sets the data as a series of bytes.
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is larger than <see cref="NP_MATCHING2_PRESENCE_OPTION_DATA_SIZE"/></exception>
				public byte[] Data
				{
					get 
					{
						if (length == 0) return null;

						byte[] output = new byte[length];

						Array.Copy(data, output, length);

						return output;
					}
					set 
					{
						if (data == null)
						{
							data = new byte[NP_MATCHING2_PRESENCE_OPTION_DATA_SIZE];
						}

						if (value != null)
						{
							if (value.Length > NP_MATCHING2_PRESENCE_OPTION_DATA_SIZE)
							{
								throw new NpToolkitException("The size of the data array is more than " + NP_MATCHING2_PRESENCE_OPTION_DATA_SIZE);
							}
							value.CopyTo(data, 0);
							length = (byte)value.Length;
						}
						else
						{
							length = 0;
						}
					}
				}

				/// <summary>
				///  Optional presence data. Sets the data as a ASCII string. String is converted in a series of bytes.
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the string is more than <see cref="NP_MATCHING2_PRESENCE_OPTION_DATA_SIZE"/> characters.</exception>
				public string DataAsString
				{
					get
					{
						if (length == 0) return "";
						return System.Text.Encoding.ASCII.GetString(data, 0, length);
					}
					set
					{
						if (data == null)
						{
							data = new byte[NP_MATCHING2_PRESENCE_OPTION_DATA_SIZE];
						}

						if (value != null)
						{
							byte[] ecodedBytes = System.Text.Encoding.ASCII.GetBytes(value);

							if (ecodedBytes.Length > NP_MATCHING2_PRESENCE_OPTION_DATA_SIZE)
							{
								throw new NpToolkitException("The size of the ASCII string is more than " + NP_MATCHING2_PRESENCE_OPTION_DATA_SIZE + " characters.");
							}

							ecodedBytes.CopyTo(data, 0);
							length = (byte)ecodedBytes.Length;
						}
						else
						{
							if (data == null)
							{
								data = new byte[NP_MATCHING2_PRESENCE_OPTION_DATA_SIZE];
							}
							length = 0;
						}
					}
				}

				internal void Read(MemoryBuffer buffer)
				{
					buffer.ReadData(ref data);
				}
			}

			/// <summary>
			/// Enum listing the different type of operators that can be specified as a clause on a search.
			/// </summary>
			public enum SearchOperatorTypes
			{
				/// <summary> It should never be used. Default value that specifies an invalid operation </summary>
				Invalid = 0,
				/// <summary> The rooms returned will have the same attribute as the one provided. Valid for integer and binary attributes </summary>
				Equals = 1,
				/// <summary> The rooms returned will have a different attribute than the one provided. Valid for integer and binary attributes </summary>
				NotEquals = 2,
				/// <summary> The rooms returned will have a smaller value on the attribute than the one provided. Valid only for integer attributes </summary>
				LessThan = 3,
				/// <summary> The rooms returned will have a smaller or equal value on the attribute than the one provided. Valid only for integer attributes </summary>
				LessEqualsThan = 4,
				/// <summary> The rooms returned will have a greater value on the attribute than the one provided. Valid only for integer attributes </summary>
				GreaterThan = 5,
				/// <summary> The rooms returned will have a greater or equal value on the attribute than the one provided. Valid only for integer attributes </summary>
				GreaterEqualsThan = 6
			};

			/// <summary>
			/// Class used to specify a filter to find rooms.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct SearchClause
			{
				internal Attribute attributeToCompare;
				internal SearchOperatorTypes operatorType;

				/// <summary>
				/// The attribute to be used for comparison with the attribute on the room to be searched. Only the value (integer or binary) and the name must be provided
				/// </summary>
				public Attribute AttributeToCompare
				{
					get { return attributeToCompare; }
					set { attributeToCompare = value; }
				}

				/// <summary>
				/// The operator used to compare the <see cref="AttributeToCompare"/> the attribute on the server.
				/// </summary>
				public SearchOperatorTypes OperatorType
				{
					get { return operatorType; }
					set { operatorType = value; }
				}
			}

			#endregion

			#region Request Classes

			/// <summary>
			/// Request class to specify the attributes configuration in the Matching service.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class SetInitConfigurationRequest : RequestBase
			{
				/// <summary>
				/// Maximum size of the <see cref="Attributes"/> array.
				/// </summary>
				public const int MAX_ATTRIBUTES = 64;

				UInt64 numAttributes;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_ATTRIBUTES)]
				AttributeMetadata[] attributes = new AttributeMetadata[MAX_ATTRIBUTES];

				/// <summary>
				/// Configuration of the attributes that the Matching service will have
				/// </summary>
				public AttributeMetadata[] Attributes
				{
					get 
					{
						if (numAttributes == 0) return null;

						AttributeMetadata[] output = new AttributeMetadata[numAttributes];

						Array.Copy(attributes, output, (int)numAttributes);

						return output; 
					}
					set 
					{
						if (value != null)
						{
							if (value.Length > MAX_ATTRIBUTES)
							{
								throw new NpToolkitException("The size of the array is more than " + MAX_ATTRIBUTES);
							}

							value.CopyTo(attributes, 0);
							numAttributes = (UInt32)value.Length;
						}
						else
						{
							numAttributes = 0;
						}

						ValidateAttributes();
					}
				}

				// * Member attributes:
				//     * The sum of all member attributes has to be a max of 64.
				//     * Integer attributes are 8 bytes.
				//   * Internal room attributes:
				//     * The sum of all internal room attributes has to be a max
				//      of 512, and no single attribute is more than 256 bytes.
				//     * Integer attributes are 8 bytes.
				//     * 64 bytes are always reserved by the NpToolkit2 library.
				//       Usable space by application will be 448 bytes (512 - 64).
				//   * External room attributes:
				//     * The sum of all external room attributes has to be a max
				//       of 512, and no single attribute is more than 256 bytes.
				//     * Integer attributes are 8 bytes.
				//     * 64 bytes are always reserved by the NpToolkit2 library.
				//      Usable space by application will be 448 bytes (512 - 64).
				//   * As search attributes, 1 binary up to 64 bytes and up to 8
				//     integers can be provided
				private void ValidateAttributes()
				{
					UInt32 totalMemberAttributeSize = 0;
					UInt32 totalInternalAttributeSize = 0;
					UInt32 totalExternalAttributeSize = 0;
					UInt32 totalNumSearchBinaries = 0;
					UInt32 totalNumSearchIntegers = 0;

					// The sizes of indivdual attributes will have already been checked in InternalSetAttribute 
					// This needs to check the total of all attributes doesn't exceed the limits

					for (UInt64 i = 0; i < numAttributes; i++)
					{
						if (attributes[i].scope == AttributeScope.Member)
						{
							totalMemberAttributeSize += attributes[i].size;
						}
						else if (attributes[i].scope == AttributeScope.Room)
						{
							if (attributes[i].roomAttributeVisibility == RoomAttributeVisibility.Internal)
							{
								totalInternalAttributeSize += attributes[i].size;
							}
							else if (attributes[i].roomAttributeVisibility == RoomAttributeVisibility.External)
							{
								totalExternalAttributeSize += attributes[i].size;
							}
							else if (attributes[i].roomAttributeVisibility == RoomAttributeVisibility.Search)
							{
								if (attributes[i].type == AttributeType.Binary)
								{
									totalNumSearchBinaries++;
								}
								else if (attributes[i].type == AttributeType.Integer)
								{
									totalNumSearchIntegers++;
								}
								else
								{
									// This shouldn't happen
									throw new NpToolkitException("Attribute " + attributes[i].name + " : Type is not set to either Binary or Integer.");
								}
							}
							else
							{
								// This shouldn't happen
								throw new NpToolkitException("Attribute " + attributes[i].name + " : RoomAttributeVisibility is not set to either Internal or External.");
							}
						}
						else
						{
							// This shouldn't happen
							throw new NpToolkitException("Attribute " + attributes[i].name + " : " + i + " : Scope is not set to either Member or Room.");
						}
					}

					if (totalMemberAttributeSize > 64)
					{
						throw new NpToolkitException("The sum of all member attributes has to be a max of 64.");
					}

					if (totalInternalAttributeSize > 448)
					{
						throw new NpToolkitException("The sum of all internal room attributes has to be a max of 448 bytes. ");
					}

					if (totalExternalAttributeSize > 448)
					{
						throw new NpToolkitException("The sum of all external room attributes has to be a max of 448 bytes");
					}

					if (totalNumSearchBinaries > 1)
					{
						throw new NpToolkitException("Only 1 binary search variable is permitted.");
					}

					if (totalNumSearchIntegers > 8)
					{
						throw new NpToolkitException("Only 8 interger search variables are permitted.");
					}
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="SetInitConfigurationRequest"/> class.
				/// </summary>
				public SetInitConfigurationRequest()
					: base(ServiceTypes.Matching, FunctionTypes.MatchingSetInitConfiguration)
				{
				}
			}

			/// <summary>
			/// Request class used to get a list of worlds from the server.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class GetWorldsRequest : RequestBase
			{
				/// <summary>
				/// Initializes a new instance of the <see cref="GetWorldsRequest"/> class.
				/// </summary>
				public GetWorldsRequest()
					: base(ServiceTypes.Matching, FunctionTypes.MatchingGetWorlds)
				{
				}
			}

			/// <summary>
			/// Request class used to specify how a room will be created.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class CreateRoomRequest : RequestBase
			{
				/// <summary>
				/// Maximum size of the <see cref="Attributes"/> array.
				/// </summary>
				public const int MAX_ATTRIBUTES = 64;

				/// <summary>
				/// The maximum size the name of a room can have
				/// </summary>
				public const int MAX_SIZE_ROOM_NAME = 63;

				/// <summary>
				/// The maximum size the status of a room can have. The status can only be used by the system
				/// </summary>
				public const int MAX_SIZE_ROOM_STATUS = 255;

				/// <summary>
				/// The maximum possible size of fixed data set on creation
				/// </summary>
				public const int MAX_SIZE_FIXED_DATA = 1023 * 1024;

				/// <summary>
				/// The maximum possible size of data that can be updated at any time
				/// </summary>
				public const int MAX_SIZE_CHANGEABLE_DATA = 1024;

				/// <summary>
				/// The maximum number of localizations for name and status. Localizations will only be used by the system
				/// </summary>
				public const int MAX_SIZE_LOCALIZATIONS = 10;

				internal UInt64 numAttributes;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_ATTRIBUTES)]
				internal Attribute[] attributes = new Attribute[MAX_ATTRIBUTES];

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_SIZE_ROOM_NAME + 1)]
				internal string name;

				internal NpMatching2SessionPassword password;

				internal RoomVisibility visibility;

				internal UInt32 numReservedSlots;

				internal UInt64 fixedDataSize;

				[MarshalAs(UnmanagedType.LPArray)]
				internal byte[] fixedData;

				internal UInt64 changeableDataSize;

				[MarshalAs(UnmanagedType.LPArray)]
				internal byte[] changeableData;	

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_SIZE_ROOM_STATUS + 1)]
				internal string status;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SIZE_LOCALIZATIONS)]
				internal LocalizedSessionInfo[] localizations = new LocalizedSessionInfo[MAX_SIZE_LOCALIZATIONS];

				internal SessionImage image;

				internal RoomMigrationType ownershipMigration;
				internal TopologyType topology;
				internal UInt32 maxNumMembers;
				internal NpMatching2WorldNumber worldNumber;

				[MarshalAs(UnmanagedType.I1)]
				internal bool displayOnSystem;

				[MarshalAs(UnmanagedType.I1)]
				internal bool isSystemJoinable;

				[MarshalAs(UnmanagedType.I1)]
				internal bool joinAllLocalUsers;

				[MarshalAs(UnmanagedType.I1)]
				internal bool isNatRestricted;

				[MarshalAs(UnmanagedType.I1)]
				internal bool isCrossplatform;

				[MarshalAs(UnmanagedType.I1)]
				internal bool allowBlockedUsersOfOwner;

				[MarshalAs(UnmanagedType.I1)]
				internal bool allowBlockedUsersOfMembers;

				/// <summary>
				/// The attributes to set. It can include room attributes and member attributes for the owner.
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is larger than <see cref="MAX_ATTRIBUTES"/>.</exception>
				public Attribute[] Attributes
				{
					get 
					{
						if (numAttributes == 0) return null;

						Attribute[] output = new Attribute[numAttributes];

						Array.Copy(attributes, output, (int)numAttributes);

						return output;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_ATTRIBUTES)
							{
								throw new NpToolkitException("The size of the array is more than " + MAX_ATTRIBUTES);
							}

							value.CopyTo(attributes, 0);
							numAttributes = (UInt32)value.Length;
						}
						else
						{
							numAttributes = 0;
						}
					}
				}

				/// <summary>
				/// The default name of the room (no localized). The default value is the one that will always be returned to the application
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the string is more than <see cref="MAX_SIZE_ROOM_NAME"/> characters.</exception>
				public string Name
				{
					get { return name; }
					set 
					{
						if (value.Length > MAX_SIZE_ROOM_NAME)
						{
							throw new NpToolkitException("The size of the name string is more than " + MAX_SIZE_ROOM_NAME + " characters.");
						}
						name = value; 
					}
				}

				/// <summary>
				/// A password for reserved slots. Only applicable when the <see cref="Visibility"/> is not <see cref="RoomVisibility.PublicRoom"/>
				/// </summary>
				public NpMatching2SessionPassword Password
				{
					get { return password; }
					set { password = value; }
				}

				/// <summary>
				/// The visibility of the room. If private, it won't be returned in searches. If <see cref="NumReservedSlots"/> is not 0, it can't be <see cref="RoomVisibility.PublicRoom"/>
				/// </summary>
				public RoomVisibility Visibility
				{
					get { return visibility; }
					set { visibility = value; }
				}

				/// <summary>
				/// The number of slots that will be reserved for the room. Provide a <see cref="Password"/> for these slots and do not select as room visibility a public room. 0 by default. If <see cref="JoinAllLocalUsers"/> is true, this member must be 0.
				/// </summary>
				public UInt32 NumReservedSlots
				{
					get { return numReservedSlots; }
					set { numReservedSlots = value; }
				}

				/// <summary>
				/// Data that will be stored when the session bound to the room is created. This data can't be modified.
				/// </summary>
				public byte[] FixedData
				{
					get { return fixedData; }
					set
					{
						if (value.Length > MAX_SIZE_FIXED_DATA)
						{
							throw new NpToolkitException("The size of the fixed data array is more than " + MAX_SIZE_FIXED_DATA + " bytes.");
						}
						fixedData = value;
						fixedDataSize = (value != null) ? (UInt64)value.Length : 0;
					}
				}

				/// <summary>
				/// Data that will be stored when the session bound to the room is created. This data can be modified after creation.
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is more than <see cref="MAX_SIZE_CHANGEABLE_DATA"/> bytes.</exception>
				public byte[] ChangeableData
				{
					get { return changeableData; }
					set
					{
						if (value.Length > MAX_SIZE_CHANGEABLE_DATA)
						{
							throw new NpToolkitException("The size of the changeable data array is more than " + MAX_SIZE_CHANGEABLE_DATA + " bytes.");
						}
						changeableData = value;
						changeableDataSize = (value != null) ? (UInt64)value.Length : 0;
					}
				}

				/// <summary>
				/// The default status shown in the system. The default name shown in the system is the room name
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the string is more than <see cref="MAX_SIZE_ROOM_STATUS"/> characters.</exception>
				public string Status
				{
					get { return status; }
					set
					{
						if (value.Length > MAX_SIZE_ROOM_STATUS)
						{
							throw new NpToolkitException("The size of the status string is more than " + MAX_SIZE_ROOM_STATUS + " characters.");
						}
						status = value;
					}
				}

				/// <summary>
				/// The localized session information shown by the system. Pairs of name and status on dfferent languages. The system will decide the language based on the system language set on the platform
				/// </summary>
				public LocalizedSessionInfo[] Localizations
				{
					get
					{
						LocalizedSessionInfo[] output = new LocalizedSessionInfo[MAX_SIZE_LOCALIZATIONS];

						Array.Copy(localizations, output, MAX_SIZE_LOCALIZATIONS);

						return output;
					}
					set 
					{
						if (value != null)
						{
							if (value.Length > MAX_SIZE_LOCALIZATIONS)
							{
								throw new NpToolkitException("The size of the array is more than " + MAX_SIZE_LOCALIZATIONS);
							}

							value.CopyTo(localizations, 0);
						}
					}
				}

				/// <summary>
				/// The visible image on the system. 
				/// </summary>
				public SessionImage Image
				{
					get { return image; }
					set { image = value; }
				}

				/// <summary>
				/// When this member is set to true, the Session will be visible in the system. Otherwise the Session will be private and not visible. True by default
				/// </summary>
				public bool DisplayOnSystem
				{
					get { return displayOnSystem; }
					set { displayOnSystem = value; }
				}

				/// <summary>
				/// Specifies if users can join from the system. If they can't, it will not be possible to join from the system. When invitations are received, a system Event is triggered with the NpSessionId, that can be given to <see cref="JoinRoom"/>. True by default
				/// </summary>
				public bool IsSystemJoinable
				{
					get { return isSystemJoinable; }
					set { isSystemJoinable = value; }
				}
				
				/// <summary>
				/// If enabled, all local users will join the just created room at the same time. False by default. It cannot be true if <see cref="NumReservedSlots"/> is different than <c>0</c>
				/// </summary>
				public bool JoinAllLocalUsers
				{
					get { return joinAllLocalUsers; }
					set { joinAllLocalUsers = value; }
				}

				/// <summary>
				/// When set to true, only users who are able to establish a P2P connection will be able to join the room. False by default
				/// </summary>
				public bool IsNatRestricted
				{
					get { return isNatRestricted; }
					set { isNatRestricted = value; }
				}

				/// <summary>
				/// Select if the room is destroyed when the owner leaves or if the ownership will pass to a different member. Automatic migration is disabled by default
				/// </summary>
				public RoomMigrationType OwnershipMigration
				{
					get { return ownershipMigration; }
					set { ownershipMigration = value; }
				}

				/// <summary>
				/// The type of connection to be establish between members. Defaults to <see cref="TopologyType.None"/>, so the system decides
				/// </summary>
				public TopologyType Topology
				{
					get { return topology; }
					set { topology = value; }
				}

				/// <summary>
				/// The maximum number of members that the room can have
				/// </summary>
				public UInt32 MaxNumMembers
				{
					get { return maxNumMembers; }
					set { maxNumMembers = value; }
				}

				/// <summary>
				/// The world to create the room in. Worlds are created with the SMT tool while configuring the Matching server. It defaults to world 1 
				/// </summary>
				public NpMatching2WorldNumber WorldNumber
				{
					get { return worldNumber; }
					set { worldNumber = value; }
				}

				/// <summary>
				/// Select true If the application is on multiple platforms. False by default
				/// </summary>
				public bool IsCrossplatform
				{
					get { return isCrossplatform; }
					set { isCrossplatform = value; }
				}

				/// <summary>
				///  Select true If the blocked users of the owner can enter the room. False by default
				/// </summary>
				public bool AllowBlockedUsersOfOwner
				{
					get { return allowBlockedUsersOfOwner; }
					set { allowBlockedUsersOfOwner = value; }
				}

				/// <summary>
				/// Select true If the blocked users of the members can enter the room when they join. True by default
				/// </summary>
				public bool AllowBlockedUsersOfMembers
				{
					get { return allowBlockedUsersOfMembers; }
					set { allowBlockedUsersOfMembers = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="CreateRoomRequest"/> class.
				/// </summary>
				public CreateRoomRequest()
					: base(ServiceTypes.Matching, FunctionTypes.MatchingCreateRoom)
				{
					numReservedSlots = 0;
					displayOnSystem = true;
					isSystemJoinable = true;
					joinAllLocalUsers = false;
					isNatRestricted = false;
					ownershipMigration = RoomMigrationType.OwnerBind;
					topology = TopologyType.None;
					worldNumber.num = 1;
					isCrossplatform = false;
					allowBlockedUsersOfOwner = false;
					allowBlockedUsersOfMembers = true;
				}
			}

			/// <summary>
			/// Request class to specify the room that wants to be left.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class LeaveRoomRequest : RequestBase
			{
				internal UInt64 roomId;
				internal PresenceOptionData notificationDataToMembers;

				/// <summary>
				/// The room identifier of the room to leave
				/// </summary>
				public UInt64 RoomId
				{
					get { return roomId; }
					set { roomId = value; }
				}

				/// <summary>
				/// Notification sent to other members. Application-defined data
				/// </summary>
				public PresenceOptionData NotificationDataToMembers
				{
					get { return notificationDataToMembers; }
					set { notificationDataToMembers = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="LeaveRoomRequest"/> class.
				/// </summary>
				public LeaveRoomRequest()
					: base(ServiceTypes.Matching, FunctionTypes.MatchingLeaveRoom)
				{
					notificationDataToMembers.Init();
				}
			}
		
			/// <summary>
			/// Enum listing the types of rooms where the search operation can be carried in.
			/// </summary>
			public enum RoomsSearchScope
			{
				/// <summary> If the search operation is for all possible rooms </summary>
				All = 0,
				/// <summary> If the search operation is only for rooms where friends of the calling user are. The search is performed in the first 25 in-context friends </summary>
				FriendsRooms,
				/// <summary> If the search operation is only for rooms where the recently met players are. To set the recently met players, call <c>ActivityFeed::postPlayedWith()</c> and give the members information. The search is performed in the most recent feed published </summary>
				RecentlyMetRooms,
				/// <summary> If the search operation is only for rooms where specific users are. A list of users has to be specified in this case. A maximum of <c>SCE_NP_MATCHING2_GET_USER_INFO_LIST_USER_NUM_MAX</c> can be specified per page </summary>
				CustomUsersList
			};

			/// <summary>
			/// Request used to search for new rooms on the world.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class SearchRoomsRequest : RequestBase
			{
				/// <summary>
				/// Maximum size of the <see cref="SearchClauses"/> array.
				/// </summary>
				public const int MAX_SEARCH_CLAUSES = 64;

				/// <summary>
				/// The maximum number of rooms matching the search criteria that can be returned per call
				/// </summary>
				public const int MAX_PAGE_SIZE = 20;

				/// <summary>
				/// The default offset when doing pagination to obtain the list of rooms
				/// </summary>
				public const int MIN_OFFSET = 1;

				/// <summary>
				/// The maximum number of users on the custom list that can be searched on a room
				/// </summary>
				public const int MAX_NUM_USERS_TO_SEARCH_IN_ROOMS = 20;

				internal UInt64 numSearchClauses;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SEARCH_CLAUSES)]
				internal SearchClause[] searchClauses = new SearchClause[MAX_SEARCH_CLAUSES];

				internal UInt64 numUsersToSearchInRooms;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_NUM_USERS_TO_SEARCH_IN_ROOMS)]
				internal Core.NpAccountId[] usersToSearchInRooms = new Core.NpAccountId[MAX_NUM_USERS_TO_SEARCH_IN_ROOMS];

				internal Int32 offset;
				internal Int32 pageSize;

				internal RoomsSearchScope searchScope;
				internal NpMatching2WorldNumber worldNumber;

				[MarshalAs(UnmanagedType.I1)]
				internal bool provideRandomRooms;

				[MarshalAs(UnmanagedType.I1)]
				internal bool quickJoin;

				[MarshalAs(UnmanagedType.I1)]
				internal bool applyNatTypeFilter;

				/// <summary>
				/// A list of all search clauses that a room must met. The result will be an AND of all listed clauses
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is larger than <see cref="MAX_SEARCH_CLAUSES"/>.</exception>
				public SearchClause[] SearchClauses
				{
					get
					{
						if (numSearchClauses == 0) return null;

						SearchClause[] output = new SearchClause[numSearchClauses];

						Array.Copy(searchClauses, output, (int)numSearchClauses);

						return output; 
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_SEARCH_CLAUSES)
							{
								throw new NpToolkitException("The size of the array is more than " + MAX_SEARCH_CLAUSES);
							}

							value.CopyTo(searchClauses, 0);
							numSearchClauses = (UInt32)value.Length;
						}
						else
						{
							numSearchClauses = 0;
						}
					}
				}

				/// <summary>
				/// When the <see cref="SearchScope"/> is <see cref="RoomsSearchScope.CustomUsersList"/>, the list of users where the rooms should be searched
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is larger than <see cref="MAX_NUM_USERS_TO_SEARCH_IN_ROOMS"/>.</exception>
				public Core.NpAccountId[] UsersToSearchInRooms
				{
					get 
					{
						if (numUsersToSearchInRooms == 0) return null;

						Core.NpAccountId[] output = new Core.NpAccountId[numUsersToSearchInRooms];

						Array.Copy(usersToSearchInRooms, output, (int)numUsersToSearchInRooms);

						return output;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_NUM_USERS_TO_SEARCH_IN_ROOMS)
							{
								throw new NpToolkitException("The size of the array is larger than " + MAX_NUM_USERS_TO_SEARCH_IN_ROOMS);
							}
							value.CopyTo(usersToSearchInRooms, 0);
							numUsersToSearchInRooms = (UInt64)value.Length;
						}
						else
						{
							numUsersToSearchInRooms = 0;
						}
					}
				}

				/// <summary>
				/// For pagination, the index on the list of returned rooms to start returning them. This member is ignored if <see cref="ProvideRandomRooms"/> is set to true. By default is the <see cref="MIN_OFFSET"/>. Value 0 is not accepted
				/// </summary>
				public Int32 Offset
				{
					get { return offset; }
					set { offset = value; }
				}

				/// <summary>
				/// For pagination, the maximum number of rooms to be obtained in one call. This member is ignored if <see cref="ProvideRandomRooms"/> is set to true. By default is <see cref="MAX_PAGE_SIZE"/>. Value 0 is not accepted
				/// </summary>
				public Int32 PageSize
				{
					get { return pageSize; }
					set { pageSize = value; }
				}

				/// <summary>
				/// The scope to search the rooms in. Either all rooms, friends rooms, players met rooms or a custom list of users
				/// </summary>
				public RoomsSearchScope SearchScope
				{
					get { return searchScope; }
					set { searchScope = value; }
				}

				/// <summary>
				/// The world to search the rooms in. Worlds are created with the SMT tool while configuring the Matching server. It defaults to world 1 
				/// </summary>
				public NpMatching2WorldNumber WorldNumber
				{
					get { return worldNumber; }
					set { worldNumber = value; }
				}

				/// <summary>
				/// False by default. When set to true, pagination provided will be ignored and the rooms will be shuffled every time a call is made
				/// </summary>
				public bool ProvideRandomRooms
				{
					get { return provideRandomRooms; }
					set { provideRandomRooms = value; }
				}

				/// <summary>
				/// False by default. When set to true, the first room returned will be joined
				/// </summary>
				public bool QuickJoin
				{
					get { return quickJoin; }
					set { quickJoin = value; }
				}

				/// <summary>
				/// True by default. When set to true, only rooms where all members can establish P2P connections with the calling user will be returned. Note the difference with <see cref="CreateRoomRequest.IsNatRestricted"/> in <see cref="CreateRoom"/>. Even if a room is not NAT restricted on creation, if everyone in the room have a good P2P connection, the room will be returned as it will pass the filter
				/// </summary>
				public bool ApplyNatTypeFilter
				{
					get { return applyNatTypeFilter; }
					set { applyNatTypeFilter = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="SearchRoomsRequest"/> class.
				/// </summary>
				public SearchRoomsRequest()
					: base(ServiceTypes.Matching, FunctionTypes.MatchingSearchRooms)
				{
					offset = MIN_OFFSET;
					pageSize = MAX_PAGE_SIZE;
					worldNumber.num = 1;
					provideRandomRooms = false;
					quickJoin = false;
					applyNatTypeFilter = true;
				}
			}

			/// <summary>
			/// Enum listing the different ways a room can be joined.
			/// </summary>
			public enum RoomJoiningType
			{
				/// <summary> If the identifier of the room is provided in order to join the room and session </summary>
				Room = 0,
				/// <summary> If the identifier of the session is provided in order to join the room and session (i.e.: when an invitation is received) </summary>
				BoundSessionId,
			};

			/// <summary>
			/// Request class provided to join a room.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class JoinRoomRequest : RequestBase
			{
				/// <summary>
				/// Maximum size of the <see cref="MemberAttributes"/> array.
				/// </summary>
				public const int MAX_ATTRIBUTES = 64;

				internal NpMatching2SessionPassword password;

				internal UInt64 numMemberAttributes;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_ATTRIBUTES)]
				internal Attribute[] memberAttributes = new Attribute[MAX_ATTRIBUTES];

				internal PresenceOptionData notificationDataToMembers;
				internal UInt64 roomId;
				internal NpSessionId boundSessionId;
				internal RoomJoiningType identifyRoomBy;

				[MarshalAs(UnmanagedType.I1)]
				internal bool joinAllLocalUsers;

				[MarshalAs(UnmanagedType.I1)]
				internal bool allowBlockedUsers;

				/// <summary>
				/// The password for a reserved slot, in case one is being joined/>
				/// </summary>
				public NpMatching2SessionPassword Password
				{
					get { return password; }
					set { password = value; }
				}

				/// <summary>
				/// The attributes the joining user will have when he/she becomes a member
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is larger than <see cref="MAX_ATTRIBUTES"/>.</exception>
				public Attribute[] MemberAttributes
				{
					get 
					{
						if (numMemberAttributes == 0) return null;

						Attribute[] output = new Attribute[numMemberAttributes];

						Array.Copy(memberAttributes, output, (int)numMemberAttributes);

						return output;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_ATTRIBUTES)
							{
								throw new NpToolkitException("The size of the array is larger than " + MAX_ATTRIBUTES);
							}
							value.CopyTo(memberAttributes, 0);
							numMemberAttributes = (UInt64)value.Length;
						}
						else
						{
							numMemberAttributes = 0;
						}
					}
				}

				/// <summary>
				/// Notification sent to other members. Application-defined data
				/// </summary>
				public PresenceOptionData NotificationDataToMembers
				{
					get { return notificationDataToMembers; }
					set { notificationDataToMembers = value; }
				}

				/// <summary>
				/// Provide the room Id to be joined. Only applicable if <see cref="IdentifyRoomBy"/> is <see cref="RoomJoiningType.Room"/>
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if <see cref="IdentifyRoomBy"/> isn't <see cref="RoomJoiningType.Room"/></exception>
				public UInt64 RoomId
				{
					get { return roomId; }
					set 
					{
						if (identifyRoomBy != RoomJoiningType.Room)
						{
							throw new NpToolkitException("Can't set RoomId if IdentifyRoomBy isn't RoomJoiningType.Room.");
						}
						roomId = value; 
					}
				}

				/// <summary>
				/// Provide the session Id bound to the room to be joined. Only applicable if <see cref="IdentifyRoomBy"/> is <see cref="RoomJoiningType.BoundSessionId"/>
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if <see cref="IdentifyRoomBy"/> isn't <see cref="RoomJoiningType.BoundSessionId"/></exception>
				public NpSessionId BoundSessionId
				{
					get { return boundSessionId; }
					set
					{
						if (identifyRoomBy != RoomJoiningType.BoundSessionId)
						{
							throw new NpToolkitException("Can't set BoundSessionId if IdentifyRoomBy isn't RoomJoiningType.BoundSessionId.");
						}
						boundSessionId = value;
					}
				}

				/// <summary>
				/// Identifier of the way a room will be joined
				/// </summary>
				public RoomJoiningType IdentifyRoomBy
				{
					get { return identifyRoomBy; }
					set { identifyRoomBy = value; }
				}

				/// <summary>
				/// False by default. All local users will be added to the room. It does not work for rooms with private slots
				/// </summary>
				public bool JoinAllLocalUsers
				{
					get { return joinAllLocalUsers; }
					set { joinAllLocalUsers = value; }
				}

				/// <summary>
				/// True by default. If set to false, the blocked users of the joining member will be added to the blacklist of the room. It will be ignored if the room has <see cref="Room.AllowBlockedUsersOfMembers"/> as false
				/// </summary>
				public bool AllowBlockedUsers
				{
					get { return allowBlockedUsers; }
					set { allowBlockedUsers = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="JoinRoomRequest"/> class.
				/// </summary>
				public JoinRoomRequest()
					: base(ServiceTypes.Matching, FunctionTypes.MatchingJoinRoom)
				{
					joinAllLocalUsers = false;
					allowBlockedUsers = true;
					notificationDataToMembers.Init();
				}
			}

			/// <summary>
			/// Request class used to specify the room where RTT information wants to be obtained.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class GetRoomPingTimeRequest : RequestBase
			{
				internal UInt64 roomId;

				/// <summary>
				/// The room identifier of the room to obtain the Round Trip Time (RTT) information
				/// </summary>
				public UInt64 RoomId
				{
					get { return roomId; }
					set { roomId = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetRoomPingTimeRequest"/> class.
				/// </summary>
				public GetRoomPingTimeRequest()
					: base(ServiceTypes.Matching, FunctionTypes.MatchingGetRoomPingTime)
				{

				}
			}

			/// <summary>
			/// Request class to expulse a member of a room.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class KickOutRoomMemberRequest : RequestBase
			{
				internal UInt64 roomId;
				internal PresenceOptionData notificationDataToMembers;
				internal UInt16 memberId;

				[MarshalAs(UnmanagedType.I1)]
				internal bool allowRejoin;

				/// <summary>
				/// The room identifier of the room the user to be kicked out belongs to
				/// </summary>
				public UInt64 RoomId
				{
					get { return roomId; }
					set { roomId = value; }
				}

				/// <summary>
				/// Notification sent to members. Application-defined data
				/// </summary>
				public PresenceOptionData NotificationDataToMembers
				{
					get { return notificationDataToMembers; }
					set 
					{
						if (value.data == null || value.length != PresenceOptionData.NP_MATCHING2_PRESENCE_OPTION_DATA_SIZE)
						{
							notificationDataToMembers.Init();
						}
						notificationDataToMembers = value; 
					}
				}

				/// <summary>
				/// Identifier of the member to be kicked out
				/// </summary>
				public UInt16 MemberId
				{
					get { return memberId; }
					set { memberId = value; }
				}

				/// <summary>
				/// If the kicked out member can join later. False by default
				/// </summary>
				public bool AllowRejoin
				{
					get { return allowRejoin; }
					set { allowRejoin = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="KickOutRoomMemberRequest"/> class.
				/// </summary>
				public KickOutRoomMemberRequest()
					: base(ServiceTypes.Matching, FunctionTypes.MatchingKickOutRoomMember)
				{
					notificationDataToMembers.Init();
				}
			}

			/// <summary>
			/// Request class to send a message to the members of the room.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class SendRoomMessageRequest : RequestBase
			{
				/// <summary>
				/// The maximum size a message to be sent to other members can have
				/// </summary>
				public const int MESSAGE_MAX_SIZE = 1023;

				/// <summary>
				/// The maximum number of memeber the message can be sent to.
				/// </summary>
				public const int MAX_MEMBERS = 32;

				internal UInt64 roomId;

				internal UInt64 numMembers;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_MEMBERS)]
				internal UInt16[] members = new UInt16[MAX_MEMBERS];

				internal UInt64 dataSize;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MESSAGE_MAX_SIZE+1)]
				internal byte[] data = new byte[MESSAGE_MAX_SIZE + 1];

				[MarshalAs(UnmanagedType.I1)]
				internal bool isChatMsg;

				/// <summary>
				/// The room identifier of the room where the message will be sent
				/// </summary>
				public UInt64 RoomId
				{
					get { return roomId; }
					set { roomId = value; }
				}

				/// <summary>
				/// A list of members to sent the message to. Leave it as 'null' to send it to all members. 'null' is the default value
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is larger than <see cref="MAX_MEMBERS"/>.</exception>
				public UInt16[] Members
				{
					get 
					{
						if (numMembers == 0) return null;

						UInt16[] output = new UInt16[numMembers];

						Array.Copy(members, output, (int)numMembers);

						return output;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_MEMBERS)
							{
								throw new NpToolkitException("The size of the array is larger than " + MAX_MEMBERS);
							}
							value.CopyTo(members, 0);
							numMembers = (UInt64)value.Length;
						}
						else
						{
							numMembers = 0;
						}
					}
				}

				/// <summary>
				/// The binary data to be sent to other members
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is more than <see cref="MESSAGE_MAX_SIZE"/> bytes.</exception>
				public byte[] Data
				{
					get 
					{
						if (dataSize == 0) return null;

						byte[] output = new byte[dataSize];

						Array.Copy(data, output, (int)dataSize);

						return output;
					}
					set
					{
						if (data == null)
						{
							data = new byte[MESSAGE_MAX_SIZE];
						}

						if (value != null)
						{
							if (value.Length > MESSAGE_MAX_SIZE)
							{
								throw new NpToolkitException("The size of the data array is more than " + MESSAGE_MAX_SIZE);
							}
							value.CopyTo(data, 0);
							dataSize = (byte)value.Length;
						}
						else
						{
							dataSize = 0;
						}
					}
				}

				/// <summary>
				///  Optional message data. Sets the data as a UTF8 string. String is converted in a series of bytes.
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the string end up more than <see cref="MESSAGE_MAX_SIZE"/> bytes.</exception>
				public string DataAsString
				{
					get
					{
						if (dataSize == 0) return "";
						return System.Text.Encoding.UTF8.GetString(data, 0, (int)dataSize);
					}
					set
					{
						if (data == null)
						{
							data = new byte[MESSAGE_MAX_SIZE];
						}

						if (value != null)
						{
							byte[] ecodedBytes = System.Text.Encoding.UTF8.GetBytes(value);

							if (ecodedBytes.Length > MESSAGE_MAX_SIZE)
							{
								throw new NpToolkitException("The size of the string is more than " + MESSAGE_MAX_SIZE + " bytes.");
							}

							ecodedBytes.CopyTo(data, 0);
							dataSize = (byte)ecodedBytes.Length;
						}
						else
						{
							if (data == null)
							{
								data = new byte[MESSAGE_MAX_SIZE];
							}
							dataSize = 0;
						}
					}
				}

				/// <summary>
				/// False by default. If true, a UTF-8 string is expected on <see cref="Data"/> and the vulgarity filter will be applied to the data.
				/// </summary>
				public bool IsChatMsg
				{
					get { return isChatMsg; }
					set { isChatMsg = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="SendRoomMessageRequest"/> class.
				/// </summary>
				public SendRoomMessageRequest()
					: base(ServiceTypes.Matching, FunctionTypes.MatchingSendRoomMessage)
				{

				}
			}

			/// <summary>
			/// Request class used to obtain attributes of a room or member from the server.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class GetAttributesRequest : RequestBase
			{
				internal UInt64 roomId;
				internal AttributeScope scope;
				internal RoomAttributeVisibility roomAttributeVisibility;
				internal UInt16 memberId;

				/// <summary>
				/// The room identifier of the room to get the attributes from
				/// </summary>
				public UInt64 RoomId
				{
					get { return roomId; }
					set { roomId = value; }
				}

				/// <summary>
				/// The type of attribute to be returned. Either room attributes or member attributes. Set automatically when using either <see cref="RoomAttributeVisibility"/> or <see cref="MemberId"/>
				/// </summary>
				public AttributeScope Scope
				{
					get { return scope; }
				}

				/// <summary>
				/// If <see cref="Scope"/> is <see cref="AttributeScope.Room"/>, the type of room attributes to be obtained. 
				/// </summary>
				public RoomAttributeVisibility RoomAttributeVisibility
				{
					get { return roomAttributeVisibility; }
					set 
					{ 
						roomAttributeVisibility = value;
						scope = AttributeScope.Room;
					}
				}

				/// <summary>
				/// If <see cref="Scope"/> is <see cref="AttributeScope.Member"/>, , the room member to get the attributes from
				/// </summary>
				public UInt16 MemberId
				{
					get { return memberId; }
					set 
					{ 
						memberId = value;
						scope = AttributeScope.Member;
					}
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetAttributesRequest"/> class.
				/// </summary>
				public GetAttributesRequest()
					: base(ServiceTypes.Matching, FunctionTypes.MatchingGetAttributes)
				{

				}
			}

			/// <summary>
			/// Enum listing the different types of data that can exist associated to a session bound to a room.
			/// </summary>
			public enum DataType
			{
				/// <summary> Up to 1023 * 1024 KiB of data that can be set on creation. Only available if a session has been created and linked to a room </summary>
				Fixed,
				/// <summary> Up to 1 KiB of data that can be modified at any time. Only available if a session has been created and linked to a room </summary>
				Changeable
			}

			/// <summary>
			/// Request class provided to obtain data (modifiable or non-modifiable) of a session bound to the joined room.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class GetDataRequest : RequestBase
			{
				internal UInt64 roomId;
				internal DataType type;

				/// <summary>
				/// The room identifier of the room associated to the session to obtain the data from.
				/// </summary>
				public UInt64 RoomId
				{
					get { return roomId; }
					set { roomId = value; }
				}

				/// <summary>
				/// Type of data of the bound session to get
				/// </summary>
				public DataType Type
				{
					get { return type; }
					set { type = value; }
				}
								
				/// <summary>
				/// Initializes a new instance of the <see cref="GetDataRequest"/> class.
				/// </summary>
				public GetDataRequest()
					: base(ServiceTypes.Matching, FunctionTypes.MatchingGetData)
				{

				}
			}

#if SUPPORT_RECENTLY_MET
			/// <summary>
			/// Represents a request to set some members as "Players Met" in the system.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class SetMembersAsRecentlyMetRequest : RequestBase
			{

				/// <summary>
				/// Initializes a new instance of the <see cref="SetMembersAsRecentlyMetRequest"/> class.
				/// </summary>
				public SetMembersAsRecentlyMetRequest()
					: base(ServiceTypes.Matching, FunctionTypes.MatchingSetMembersAsRecentlyMet)
				{

				}
			}
#endif

			/// <summary>
			/// Request used to invite a user to a session and room.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class SendInvitationRequest : RequestBase
			{
				/// <summary>
				/// The maximum size that the data attached to an invitation can have
				/// </summary>
				public const int MAX_SIZE_ATTACHMENT = 1 * 1024 * 1024;

				/// <summary>
				/// The maximum number of recipients that can receive one invitation
				/// </summary>
				public const int MAX_NUM_RECIPIENTS = 16;   // SCE_INVITATION_DIALOG_ADDRESS_USER_LIST_MAX_SIZE

				/// <summary>
				/// The maximum size the text message of an invitation can have, in UTF-8 characters
				/// </summary>
				public const int MAX_SIZE_USER_MESSAGE = 511;

				internal UInt64 roomId;						

				internal UInt64 numRecipients;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_NUM_RECIPIENTS)]
				internal Core.NpAccountId[]	recipients = new Core.NpAccountId[MAX_NUM_RECIPIENTS];

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_SIZE_USER_MESSAGE+1)]
				internal string userMessage;

				internal UInt64 attachmentSize;

				[MarshalAs(UnmanagedType.LPArray)]
				internal byte[] attachment;

				internal Int32 maxNumberRecipientsToAdd;

				[MarshalAs(UnmanagedType.I1)]
				internal bool recipientsEditableByUser;

				[MarshalAs(UnmanagedType.I1)]
				internal bool enableDialog;

				/// <summary>
				/// The room identifier of the room to be joined
				/// </summary>
				public UInt64 RoomId
				{
					get { return roomId; }
					set { roomId = value; }
				}

				/// <summary>
				/// Users to send this invite to. Not used if <see cref="RecipientsEditableByUser"/> is set to true
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
							numRecipients = (UInt64)value.Length;
						}
						else
						{
							numRecipients = 0;
						}
					}
				}

				/// <summary>
				/// Pre-filled message to display if <see cref="EnableDialog"/> is set to true
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the user message is more than <see cref="MAX_SIZE_USER_MESSAGE"/> characters.</exception>
				public string UserMessage
				{
					get { return userMessage; }
					set
					{
						if (value.Length > MAX_SIZE_USER_MESSAGE)
						{
							throw new NpToolkitException("The size of the user message string is more than " + MAX_SIZE_USER_MESSAGE + " characters.");
						}
						userMessage = value;
					}
				}

				/// <summary>
				/// Binary attachment data that can be added if <see cref="EnableDialog"/> is set to false. If provided, the pointer has to remain valid in memory until a response is obtained
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is larger than <see cref="MAX_SIZE_ATTACHMENT"/>.</exception>
				public byte[] Attachment
				{
					get 
					{
						return attachment; 
					}
					set
					{
						if (value.Length > MAX_SIZE_ATTACHMENT)
						{
							throw new NpToolkitException("The size of the attachment array is more than " + MAX_SIZE_ATTACHMENT);
						}
						attachment = value;
						attachmentSize = (byte)value.Length;
					}
				}

				/// <summary>
				/// Maximum number of users that this invitation can be sent to. Only used if <see cref="RecipientsEditableByUser"/> is set to true
				/// </summary>
				public Int32 MaxNumberRecipientsToAdd
				{
					get { return maxNumberRecipientsToAdd; }
					set { maxNumberRecipientsToAdd = value; }
				}

				/// <summary>
				/// If true, allows the user to edit the recipients. Only if <see cref="EnableDialog"/> is set to true
				/// </summary>
				public bool RecipientsEditableByUser
				{
					get { return recipientsEditableByUser; }
					set { recipientsEditableByUser = value; }
				}

				/// <summary>
				/// Set to true to show the invitation dialog. If false, no dialog will be shown to the user and the invite will be sent silently
				/// </summary>
				public bool EnableDialog
				{
					get { return enableDialog; }
					set { enableDialog = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="SendInvitationRequest"/> class.
				/// </summary>
				public SendInvitationRequest()
					: base(ServiceTypes.Matching, FunctionTypes.MatchingSendInvitation)
				{

				}
			}

			/// <summary>
			/// Enum listing the different ways to update a room (set room information).
			/// </summary>
			public enum SetRoomInfoType 
			{
				/// <summary> Default value. It should never be used </summary>
				Invalid = 0,
				/// <summary> Information of the calling user, as the member of the room, will be modified </summary>
				MemberInfo,
				/// <summary> External information of the room will be modified </summary>
				RoomExternalInfo,
				/// <summary> Internal information of the room (only visible to members) will be modified </summary>
				RoomInternalInfo,
				/// <summary> Information of the session bound to the room will be modified </summary>
				RoomSessionInfo,
				/// <summary> The topology of the room will be modified </summary>
				RoomTopology
			};

			/// <summary>
			/// Request class to modify information of a room (or a member of the room).
			/// </summary>
			/// <remarks>
			/// Only one type of update can be performed per call. See <see cref="SetRoomInfoType"/> enum for the different types.
			/// Leave blank any value of this request that you do not want to modify.
			/// Booleans are represented as enums,<see cref="Core.OptionalBoolean"/>, to identify when no modification is required.
			/// </remarks>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class SetRoomInfoRequest : RequestBase
			{
				/// <summary>
				/// The maximum number of member attributes. The total size of all member attributes (integer or binary) can't exceed 64 bytes.
				/// </summary>
				public const int MAX_MEMBER_ATTRIBUTES = 8;

				/// <summary>
				/// The maximum number of external and search attributes.
				/// </summary>
				public const int MAX_ATTRIBUTES = 64;

				#region Structures

				/// <summary>
				/// Information of the calling user, as the member of the room, will be modified
				/// </summary>
				[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
				public struct MemberInformation
				{
					internal UInt64 numMemberAttributes;

					[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_MEMBER_ATTRIBUTES)]
					internal Attribute[] memberAttributes;

					internal UInt16 memberId;

					internal void Init()
					{
						memberAttributes = new Attribute[MAX_MEMBER_ATTRIBUTES];
					}

					/// <summary>
					/// The attributes to modify on the member. 
					/// </summary>
					/// <exception cref="NpToolkitException">Will throw an exception if the array is larger than <see cref="MAX_MEMBER_ATTRIBUTES"/>.</exception>
					public Attribute[] MemberAttributes
					{
						get 
						{
							if (numMemberAttributes == 0) return null;

							Attribute[] output = new Attribute[numMemberAttributes];

							Array.Copy(memberAttributes, output, (int)numMemberAttributes);

							return output; 
						}
						set
						{
							if (memberAttributes == null)
							{
								memberAttributes = new Attribute[MAX_MEMBER_ATTRIBUTES];
							}

							if (value != null)
							{
								if (value.Length > MAX_MEMBER_ATTRIBUTES)
								{
									throw new NpToolkitException("The size of the attributes array is more than " + MAX_MEMBER_ATTRIBUTES);
								}
								value.CopyTo(memberAttributes, 0);
								numMemberAttributes = (UInt64)value.Length;
							}
							else
							{
								numMemberAttributes = 0;
							}
						}
					}

					/// <summary>
					/// The member whose attributes will be modified. Specify always the calling user member Id
					/// </summary>
					public UInt16 MemberId
					{
						get { return memberId; }
						set { memberId = value; }
					}

				};

				/// <summary>
				/// External information of the room will be modified
				/// </summary>
				[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
				public struct ExternalRoomInformation
				{
					internal UInt64 numExternalAttributes;

					[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_ATTRIBUTES)]
					internal Attribute[] externalAttributes;

					internal UInt64 numSearchAttributes;

					[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_ATTRIBUTES)]
					internal Attribute[] searchAttributes;

					internal void Init()
					{
						externalAttributes = new Attribute[MAX_ATTRIBUTES];
						searchAttributes = new Attribute[MAX_ATTRIBUTES];
					}

					/// <summary>
					/// The external attributes to modify on the room
					/// </summary>
					/// <exception cref="NpToolkitException">Will throw an exception if the array is larger than <see cref="MAX_ATTRIBUTES"/>.</exception>
					public Attribute[] ExternalAttributes
					{
						get 
						{
							if (numExternalAttributes == 0) return null;

							Attribute[] output = new Attribute[numExternalAttributes];

							Array.Copy(externalAttributes, output, (int)numExternalAttributes);

							return output; 
						}
						set
						{
							if (externalAttributes == null)
							{
								externalAttributes = new Attribute[MAX_ATTRIBUTES];
							}

							if (value != null)
							{
								if (value.Length > MAX_ATTRIBUTES)
								{
									throw new NpToolkitException("The size of the attributes array is more than " + MAX_ATTRIBUTES);
								}
								value.CopyTo(externalAttributes, 0);
								numExternalAttributes = (UInt64)value.Length;
							}
							else
							{
								numExternalAttributes = 0;
							}
						}
					}

					/// <summary>
					/// The search attributes to modify on the room
					/// </summary>
					/// <exception cref="NpToolkitException">Will throw an exception if the array is larger than <see cref="MAX_ATTRIBUTES"/>.</exception>
					public Attribute[] SearchAttributes
					{
						get 
						{
							if (numSearchAttributes == 0) return null;

							Attribute[] output = new Attribute[numSearchAttributes];

							Array.Copy(searchAttributes, output, (int)numSearchAttributes);

							return output; 
						}
						set
						{
							if (searchAttributes == null)
							{
								searchAttributes = new Attribute[MAX_ATTRIBUTES];
							}

							if (value != null)
							{
								if (value.Length > MAX_ATTRIBUTES)
								{
									throw new NpToolkitException("The size of the attributes array is more than " + MAX_ATTRIBUTES);
								}
								value.CopyTo(searchAttributes, 0);
								numSearchAttributes = (UInt64)value.Length;
							}
							else
							{
								numSearchAttributes = 0;
							}
						}
					}
				}

				/// <summary>
				/// Internal information of the room (only visible to members) will be modified
				/// </summary>
				[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
				public struct InternalRoomInformation
				{
					internal UInt64 numInternalAttributes;

					[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_ATTRIBUTES)]
					internal Attribute[] internalAttributes;

					internal Core.OptionalBoolean allowBlockedUsersOfMembers;
					internal Core.OptionalBoolean joinAllLocalUsers;
					internal Core.OptionalBoolean isNatRestricted;
					internal UInt32 numReservedSlots;
					internal RoomVisibility visibility;
					internal Core.OptionalBoolean closeRoom;

					internal void Init()
					{
						internalAttributes = new Attribute[MAX_ATTRIBUTES];
					}

					/// <summary>
					/// The internal attributes to modify on the room
					/// </summary>
					/// <exception cref="NpToolkitException">Will throw an exception if the array is larger than <see cref="MAX_ATTRIBUTES"/>.</exception>
					public Attribute[] InternalAttributes
					{
						get 
						{
							if (numInternalAttributes == 0) return null;

							Attribute[] output = new Attribute[numInternalAttributes];

							Array.Copy(internalAttributes, output, (int)numInternalAttributes);

							return output; 
						}
						set
						{
							if (internalAttributes == null)
							{
								internalAttributes = new Attribute[MAX_ATTRIBUTES];
							}

							if (value != null)
							{
								if (value.Length > MAX_ATTRIBUTES)
								{
									throw new NpToolkitException("The size of the attributes array is more than " + MAX_ATTRIBUTES);
								}
								value.CopyTo(internalAttributes, 0);
								numInternalAttributes = (UInt64)value.Length;
							}
							else
							{
								numInternalAttributes = 0;
							}
						}
					}

					/// <summary>
					/// Change it to allow or not adding blocked users of members to the blacklist of the room. The blocked users already added will be kept
					/// </summary>
					public Core.OptionalBoolean AllowBlockedUsersOfMembers
					{
						get { return allowBlockedUsersOfMembers; }
						set { allowBlockedUsersOfMembers = value; }
					}

					/// <summary>
					/// Change it to allow or not adding all local users to rooms when more than one local user is available on one system. It does not work if the number of reserved slots of the room is different than 0
					/// </summary>
					public Core.OptionalBoolean JoinAllLocalUsers
					{
						get { return joinAllLocalUsers; }
						set { joinAllLocalUsers = value; }
					}

					/// <summary>
					/// Change it to restrict or not the members that can join the room to only those who can establish P2P connections
					/// </summary>
					public Core.OptionalBoolean IsNatRestricted
					{
						get { return isNatRestricted; }
						set { isNatRestricted = value; }
					}

					/// <summary>
					/// Change it to modify the number of reserved slots in the room. Only applicable if <see cref="Visibility"/> is <see cref="RoomVisibility.ReserveSlots"/> and JoinAllLocalUsers of the room is false
					/// </summary>
					public UInt32 NumReservedSlots
					{
						get { return numReservedSlots; }
						set { numReservedSlots = value; }
					}

					/// <summary>
					/// Change it to make the room private, public or public with reserved slots
					/// </summary>
					public RoomVisibility Visibility
					{
						get { return visibility; }
						set { visibility = value; }
					}

					/// <summary>
					/// Change it to close the room. It will no longer be provided in searches
					/// </summary>
					public Core.OptionalBoolean CloseRoom
					{
						get { return closeRoom; }
						set { closeRoom = value; }
					}
				}

				/// <summary>
				/// Information of the session bound to the room will be modified
				/// </summary>
				[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
				public struct RoomSessionInformation
				{
					internal Core.OptionalBoolean displayOnSystem;
					internal Core.OptionalBoolean isSystemJoinable;

					internal UInt64 changeableDataSize;

					[MarshalAs(UnmanagedType.LPArray)]
					internal byte[] changeableData;

					[MarshalAs(UnmanagedType.ByValTStr, SizeConst = CreateRoomRequest.MAX_SIZE_ROOM_STATUS + 1)]
					internal string status;

					internal UInt64 numLocalizations;

					[MarshalAs(UnmanagedType.ByValArray, SizeConst = CreateRoomRequest.MAX_SIZE_LOCALIZATIONS)]
					internal LocalizedSessionInfo[] localizations;

					internal SessionImage image;

					internal void Init()
					{
						localizations = new LocalizedSessionInfo[CreateRoomRequest.MAX_SIZE_LOCALIZATIONS];
					}

					/// <summary>
					/// Change it to make the Session visible or not in the system
					/// </summary>
					public Core.OptionalBoolean DisplayOnSystem
					{
						get { return displayOnSystem; }
						set { displayOnSystem = value; }
					}

					/// <summary>
					/// Change it to allow or not users joining from the system (the platform)
					/// </summary>
					public Core.OptionalBoolean IsSystemJoinable
					{
						get { return isSystemJoinable; }
						set { isSystemJoinable = value; }
					}

					/// <summary>
					/// Change it to modify the data associated to a system session. Other members won't be notified of this change.
					/// </summary>
					/// <exception cref="NpToolkitException">Will throw an exception if the array is more than <see cref="CreateRoomRequest.MAX_SIZE_CHANGEABLE_DATA"/> bytes.</exception>
					public byte[] ChangeableData
					{
						get { return changeableData; }
						set
						{
							if (value.Length > CreateRoomRequest.MAX_SIZE_CHANGEABLE_DATA)
							{
								throw new NpToolkitException("The size of the changeable data array is more than " + CreateRoomRequest.MAX_SIZE_CHANGEABLE_DATA + " bytes.");
							}
							changeableData = value;
							changeableDataSize = (UInt64)value.Length;
						}
					}

					/// <summary>
					/// Change it to modify the default status shown in the system. The default name is the room name. Other members won't be notified of this change
					/// </summary>
					/// <exception cref="NpToolkitException">Will throw an exception if the string is more than <see cref="CreateRoomRequest.MAX_SIZE_ROOM_STATUS"/> characters.</exception>
					public string Status
					{
						get { return status; }
						set
						{
							if (value.Length > CreateRoomRequest.MAX_SIZE_ROOM_STATUS)
							{
								throw new NpToolkitException("The size of the status string is more than " + CreateRoomRequest.MAX_SIZE_ROOM_STATUS + " characters.");
							}
							status = value;
						}
					}

					/// <summary>
					/// The localized session information shown by the system. If <see cref="Status"/> is provided, the previously set localization will be discarded
					/// </summary>
					/// <exception cref="NpToolkitException">Will throw an exception if the array is more than <see cref="CreateRoomRequest.MAX_SIZE_LOCALIZATIONS"/>.</exception>
					public LocalizedSessionInfo[] Localizations
					{
						get 
						{
							if (numLocalizations == 0) return null;

							LocalizedSessionInfo[] output = new LocalizedSessionInfo[numLocalizations];

							Array.Copy(localizations, output, (int)numLocalizations);

							return output; 
						}
						set
						{
							if (value != null)
							{
								if (value.Length > CreateRoomRequest.MAX_SIZE_LOCALIZATIONS)
								{
									throw new NpToolkitException("The size of the localization array is more than " + CreateRoomRequest.MAX_SIZE_LOCALIZATIONS);
								}
								value.CopyTo(localizations, 0);
								numLocalizations = (UInt64)value.Length;
							}
							else
							{
								numLocalizations = 0;
							}
						}
					}

					/// <summary>
					/// Change it to set a new image on the system session.
					/// </summary>
					public SessionImage Image
					{
						get { return image; }
						set { image = value; }
					}
				}

				#endregion

				internal UInt64 roomId;
				internal SetRoomInfoType roomInfoType;

				internal MemberInformation memberInfo;
				internal ExternalRoomInformation externalRoomInfo;
				internal InternalRoomInformation internalRoomInfo;
				internal RoomSessionInformation roomSessionInfo;
				internal TopologyType roomTopology;

				/// <summary>
				/// The room identifier of the room to be modified
				/// </summary>
				public UInt64 RoomId
				{
					get { return roomId; }
					set { roomId = value; }
				}

				/// <summary>
				/// The type of update that will be performed on this call
				/// </summary>
				public SetRoomInfoType RoomInfoType
				{
					get { return roomInfoType; }
					set { roomInfoType = value; }
				}

				/// <summary>
				/// Information of the calling user, as the member of the room, will be modified. <see cref="RoomInfoType"/> must be <see cref="SetRoomInfoType.MemberInfo"/>
				/// </summary>
				public MemberInformation MemberInfo
				{
					get { return memberInfo; }
					set { memberInfo = value; }
				}

				/// <summary>
				/// External information of the room will be modified. <see cref="RoomInfoType"/> must be <see cref="SetRoomInfoType.RoomExternalInfo"/>
				/// </summary>
				public ExternalRoomInformation ExternalRoomInfo
				{
					get { return externalRoomInfo; }
					set { externalRoomInfo = value; }
				}

				/// <summary>
				/// Internal information of the room (only visible to members) will be modified
				/// </summary>
				public InternalRoomInformation InternalRoomInfo
				{
					get { return internalRoomInfo; }
					set { internalRoomInfo = value; }
				}

				/// <summary>
				/// Information of the session bound to the room will be modified
				/// </summary>
				public RoomSessionInformation RoomSessionInfo
				{
					get { return roomSessionInfo; }
					set { roomSessionInfo = value; }
				}

				/// <summary>
				/// The topology of the room will be modified
				/// </summary>
				public TopologyType RoomTopology
				{
					get { return roomTopology; }
					set { roomTopology = value; }
				}
			
				/// <summary>
				/// Initializes a new instance of the <see cref="GetDataRequest"/> class.
				/// </summary>
				public SetRoomInfoRequest()
					: base(ServiceTypes.Matching, FunctionTypes.MatchingSetRoomInfo)
				{
					memberInfo.Init();
					externalRoomInfo.Init();
					internalRoomInfo.Init();
					roomSessionInfo.Init();
				}
			}

			#endregion

			#region Set Init Configuration Request

			/// <summary>
			/// This function is used to set the metadata of the attributes the application will use. This is the first function that needs to be called when using the Matching service. 
			/// </summary> 
			/// <remarks>
			/// This only needs to be called once and is not dependent on the user. It does not need to be called seperately for each user. 
			/// </remarks>
			/// <param name="request">The members required to understand the attributes configuration of the Matching service </param>
			/// <param name="response">This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int SetInitConfiguration(SetInitConfigurationRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxSetInitConfiguration(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Classes

			/// <summary>
			/// Session password. It is specified when creating and joining a session.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct NpMatching2SessionPassword  // Maps to SceNpMatching2SessionPassword
			{
				/// <summary>
				/// Maximum size of the password
				/// </summary>
				public const int NP_MATCHING2_SESSION_PASSWORD_SIZE = 8;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = NP_MATCHING2_SESSION_PASSWORD_SIZE)]
				internal string password;

				/// <summary>
				/// World ID
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the password is more than <see cref="NP_MATCHING2_SESSION_PASSWORD_SIZE"/> characters.</exception>
				public string Password
				{
					get { return password; }
					set 
					{
						if (value.Length > NP_MATCHING2_SESSION_PASSWORD_SIZE)
						{
							throw new NpToolkitException("The size of the password string is more than " + NP_MATCHING2_SESSION_PASSWORD_SIZE + " characters.");
						} 
						password = value;
					}
				}

				internal void Read(MemoryBuffer buffer)
				{
					buffer.ReadString(ref password);
				}

				/// <summary>
				/// Return the password as a string
				/// </summary>
				/// <returns>The session password.</returns>
				public override string ToString()
				{
					return password;
				}

				/// <summary>
				/// Allow direct assignment of string
				/// </summary>
				/// <param name="value">16bit world number.</param>
				static public implicit operator NpMatching2SessionPassword(string value)
				{
					NpMatching2SessionPassword newPassword = new NpMatching2SessionPassword();
					newPassword.Password = value;  // Use set property to do validation check
					return newPassword;
				}
			}

			/// <summary>
			/// Session ID
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct NpSessionId // Maps to SceNpSessionId
			{
				/// <summary>
				/// Maximum length of the session id.
				/// </summary>
				public const int NP_SESSION_ID_MAX_SIZE = 45;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = NP_SESSION_ID_MAX_SIZE+1)]
				internal string data;

				/// <summary>
				/// Session ID string (45 bytes or less)
				/// </summary>
				public string Data
				{
					get { return data; }
				}

				internal void Read(MemoryBuffer buffer)
				{
					buffer.ReadString(ref data);
				}

				/// <summary>
				/// Return the session id as a string
				/// </summary>
				/// <returns>The session id as a string</returns>
				public override string ToString()
				{
					return data;
				}
			}

			/// <summary>
			/// This type represents a world ID.
			/// </summary>
			public struct NpMatching2WorldId  // Maps to SceNpMatching2WorldId
			{
				internal UInt32 id;

				/// <summary>
				/// World ID
				/// </summary>
				public UInt32 Id
				{
					get { return id; }
					set { id = value; }
				}

				internal void Read(MemoryBuffer buffer)
				{
					id = buffer.ReadUInt32();
				}
			}

			/// <summary>
			/// This type represents a world number.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct NpMatching2WorldNumber  // Maps to SceNpMatching2WorldNumber
			{
				internal UInt16 num;

				/// <summary>
				/// World number
				/// </summary>
				public UInt16 Num
				{
					get { return num; }
					set { num = value; }
				}

				internal void Read(MemoryBuffer buffer)
				{
					num = buffer.ReadUInt16();
				}

				/// <summary>
				/// Allow direct assignment of UInt16
				/// </summary>
				/// <param name="value">16bit world number.</param>
				static public implicit operator NpMatching2WorldNumber(UInt16 value)
				{
					NpMatching2WorldNumber newNum = new NpMatching2WorldNumber();
					newNum.num = value;
					return newNum;
				}
			}

			/// <summary>
			/// A class representing a world on the server. A world always has the identifiers (world Id and world number) and the
			/// current world information (number of members and number of rooms on the world).
			/// </summary>
			public struct World
			{
				internal NpMatching2WorldId worldId;
				internal UInt32 currentNumberOfRooms;
				internal UInt32 currentNumberOfMembers;
				internal NpMatching2WorldNumber worldNumber;

				/// <summary>
				/// The identifier of the world in the server, as it is read by the NpMatching2 function calls
				/// </summary>
				public NpMatching2WorldId WorldId { get { return worldId; } }

				/// <summary>
				/// The number of rooms that exist in the world at the moment
				/// </summary>
				public UInt32 CurrentNumberOfRooms { get { return currentNumberOfRooms; } }

				/// <summary>
				/// The number of members that exist in the world at the moment
				/// </summary>
				public UInt32 CurrentNumberOfMembers { get { return currentNumberOfMembers; } }

				/// <summary>
				/// The identifier of the world in the server, as it is read by the SMT configuration and the NpToolkit2 function calls
				/// </summary>
				public NpMatching2WorldNumber WorldNumber { get { return worldNumber; } }

				internal void Read(MemoryBuffer buffer)
				{
					worldId.Read(buffer);

					currentNumberOfRooms = buffer.ReadUInt32();
					currentNumberOfMembers = buffer.ReadUInt32();

					worldNumber.Read(buffer);
				}
			}

			/// <summary>
			/// Enum listing the different statuses the connection between the local user and the remote user can have.
			/// </summary>
			public enum SignalingStatus 
			{
				/// <summary> Signaling information can not be obtained. </summary>
				NotApplicable = 0,
				/// <summary> Connection between both peers has been established and information between them has been obtained. </summary>
				Established,
				/// <summary> Connection between both peers has been established but information between them can not be obtained. </summary>
				EstablishedFailToGetInformation,
				/// <summary> Connection between both peers is not established (either lost or never established). </summary>
				Dead
			}

			/// <summary>
			/// 
			/// </summary>
			public enum NatType 
			{
				/// <summary> The NAT type is not provided. Default value. </summary>
				Invalid = 0,
				/// <summary> The NAT type is 1. The network connection is quite good. </summary>
				NatType1,
				/// <summary> The NAT type is 2. The network connection is good. </summary>
				NatType2,
				/// <summary> The NAT type is 3. The network connection is bad and online gaming may not be possible. </summary>
				NatType3		
			};

			/// <summary>
			/// This class contains information of the connection between a member and the calling user.
			/// </summary>
			public class MemberSignalingInformation
			{
				internal NatType natType;
				internal SignalingStatus status;
				internal UInt32 roundTripTime;
				internal NetworkUtils.NetInAddr ipAddress;
				internal UInt16 port;
				internal UInt16 portNetworkOrder;

				/// <summary>
				/// The type of NAT of the Member
				/// </summary>
				public NatType NatType { get { return natType; } }

				/// <summary>
				/// The status of the signal
				/// </summary>
				public SignalingStatus Status { get { return status; } }

				/// <summary>
				/// The ping time with the member. Provided when signaling is established with the member
				/// </summary>
				public UInt32 RoundTripTime { get { return roundTripTime; } }

				/// <summary>
				/// The IP address of the member. Provided when signaling is established with the member
				/// </summary>
				public NetworkUtils.NetInAddr IpAddress { get { return ipAddress; } }

				/// <summary>
				/// The port used by the member. Provided when signaling is established with the member. This in host byte order
				/// </summary>
				public UInt16 Port { get { return port; } }

				/// <summary>
				/// The same port returned by <see cref="Port"/> except it is in network byte order
				/// </summary>
				public UInt16 PortNetworkOrder { get { return portNetworkOrder; } }

				internal void Read(MemoryBuffer buffer)
				{
					natType = (NatType)buffer.ReadUInt32();
					status = (SignalingStatus)buffer.ReadUInt32();
					roundTripTime = buffer.ReadUInt32();

					ipAddress.Read(buffer);

					port = buffer.ReadUInt16();
					portNetworkOrder = buffer.ReadUInt16();
				}
			}

			/// <summary>
			/// This class represents a member (a user that is inside a room).
			/// </summary>
			public class Member
			{
				internal Core.OnlineUser onlineUser = new Core.OnlineUser();
				internal Attribute[] memberAttributes;
				internal DateTime joinedDate;
				internal MemberSignalingInformation signalingInformation = new MemberSignalingInformation();
				internal Core.PlatformType platform;
				internal UInt16 roomMemberId;
				internal bool isOwner;
				internal bool isMe;

				/// <summary>
				/// The Account Id and Online Id of the member
				/// </summary>
				public Core.OnlineUser OnlineUser { get { return onlineUser; } }

				/// <summary>
				/// The attributes the member has on the room
				/// </summary>
				public Attribute[] MemberAttributes { get { return memberAttributes; } }

				/// <summary>
				/// The date the member joined the room
				/// </summary>
				public DateTime JoinedDate { get { return joinedDate; } }

				/// <summary>
				/// Signaling information of the member (IP address, port, RTT, etc.)
				/// </summary>
				public MemberSignalingInformation SignalingInformation { get { return signalingInformation; } }

				/// <summary>
				/// The platform the member is using
				/// </summary>
				public Core.PlatformType Platform { get { return platform; } }

				/// <summary>
				/// The identifier of the member in the room
				/// </summary>
				public UInt16 RoomMemberId { get { return roomMemberId; } }

				/// <summary>
				/// If the member is the owner of the room
				/// </summary>
				public bool IsOwner { get { return isOwner; } }

				/// <summary>
				/// If the member is the calling user
				/// </summary>
				public bool IsMe { get { return isMe; } }

				internal void Read(MemoryBuffer buffer)
				{
					onlineUser.Read(buffer);

					UInt64 numMemberAttributes = buffer.ReadUInt64();

					memberAttributes = new Attribute[numMemberAttributes];

					for (UInt64 i = 0; i < numMemberAttributes; i++)
					{
						memberAttributes[i].Read(buffer);
					}

					joinedDate = Core.ReadRtcTick(buffer);

					signalingInformation.Read(buffer);

					platform = (Core.PlatformType)buffer.ReadUInt32();
					roomMemberId = buffer.ReadUInt16();
					isOwner = buffer.ReadBool();
					isMe = buffer.ReadBool();
				}
			}

			/// <summary>
			/// Response data class containing information about a room on
			/// the server (a set of members playing an activity currently
			/// together).
			/// </summary>
			public class Room
			{
				internal UInt16 matchingContext;  // SceNpMatching2ContextId
				internal UInt16 serverId;  // SceNpMatching2ServerId
				internal UInt32 worldId;  // SceNpMatching2WorldId
				internal UInt64 roomId;  // SceNpMatching2RoomId

				internal Attribute[] attributes;

				internal string name;

				internal Member[] currentMembers;

				internal UInt64 numMaxMembers;
				internal TopologyType topology;
				internal UInt32 numReservedSlots;

				internal bool isNatRestricted;
				internal bool allowBlockedUsersOfOwner;
				internal bool allowBlockedUsersOfMembers;
				internal bool joinAllLocalUsers;

				internal RoomMigrationType ownershipMigration;
				internal RoomVisibility visibility;

				internal NpMatching2SessionPassword password;

				internal NpSessionId boundSessionId;

				internal bool isSystemJoinable;
				internal bool displayOnSystem;
				internal bool hasChangeableData;
				internal bool hasFixedData;
				internal bool isCrossplatform;
				internal bool isClosed;

				/// <summary>
				/// The NpMatching2 context of the calling user in case it needs to be used in NpMatching2 calls
				/// </summary>
				public UInt16 MatchingContext { get { return matchingContext; } }

				/// <summary>
				/// The NpMatching2 server where the room is, in case it needs to be used in NpMatching2 calls
				/// </summary>
				public UInt16 ServerId { get { return serverId; } }

				/// <summary>
				/// The NpMatching2 world where the room is, in case it needs to be used in NpMatching2 calls
				/// </summary>
				public UInt32 WorldId { get { return worldId; } }

				/// <summary>
				/// The room identifier of the current room
				/// </summary>
				public UInt64 RoomId { get { return roomId; } }

				/// <summary>
				/// The list of room attributes (internal, external, search)
				/// </summary>
				public Attribute[] Attributes { get { return attributes; } }

				/// <summary>
				/// The name of the room. It won't be localized
				/// </summary>
				public string Name { get { return name; } }

				/// <summary>
				/// The list of members currently in the room
				/// </summary>
				public Member[] CurrentMembers { get { return currentMembers; } }

				/// <summary>
				/// The maximum number of members the <see cref="CurrentMembers"/> list can have
				/// </summary>
				public UInt64 NumMaxMembers { get { return numMaxMembers; } }

				/// <summary>
				/// The connectivity structure the room is using
				/// </summary>
				public TopologyType Topology { get { return topology; } }

				/// <summary>
				/// The number of private spaces the room has. It cannot be used at the same time as <see cref="JoinAllLocalUsers"/>.
				/// </summary>
				public UInt32 NumReservedSlots { get { return numReservedSlots; } }

				/// <summary>
				/// Indicates if members joining the room must be able to connect with other members in a P2P connection
				/// </summary>
				public bool IsNatRestricted { get { return isNatRestricted; } }

				/// <summary>
				/// Indicates if the owner allows the blocked users of its list to join the room
				/// </summary>
				public bool AllowBlockedUsersOfOwner { get { return allowBlockedUsersOfOwner; } }

				/// <summary>
				/// Indicates if the owner allows other members to decide if blocked users of their lists can join the room
				/// </summary>
				public bool AllowBlockedUsersOfMembers { get { return allowBlockedUsersOfMembers; } }

				/// <summary>
				/// Indicates if all signed in users in the platform should join the room or just the calling user. It cannot be used at the same time as <see cref="NumReservedSlots"/>.
				/// </summary>
				public bool JoinAllLocalUsers { get { return joinAllLocalUsers; } }

				/// <summary>
				/// Indicates what happens when the current owner of the room leaves (the room is destroyed or a new owner is selected)
				/// </summary>
				public RoomMigrationType OwnershipMigration { get { return ownershipMigration; } }

				/// <summary>
				/// Indicates if the room is public (visible in searches), private (hidden in searches) or public with some slots restricted
				/// </summary>
				public RoomVisibility Visibility { get { return visibility; } }

				/// <summary>
				/// The password to set when users want to become members using one of the restricted slots 
				/// </summary>
				public NpMatching2SessionPassword Password { get { return password; } }

				/// <summary>
				/// The Session bound to the room
				/// </summary>
				public NpSessionId BoundSessionId { get { return boundSessionId; } }

				/// <summary>
				/// If the Session can be joined from the system. If it is joined from the system, a System Event with the Session Id will be called, and the <see cref="JoinRoom"/> function with the Session Id will need to be used
				/// </summary>
				public bool IsSystemJoinable { get { return isSystemJoinable; } }

				/// <summary>
				/// If the session information of the session bound to the room is shown on the system
				/// </summary>
				public bool DisplayOnSystem { get { return displayOnSystem; } }

				/// <summary>
				/// If there is modifiable data on the session bound to the room
				/// </summary>
				public bool HasChangeableData { get { return hasChangeableData; } }

				/// <summary>
				/// If there is non-modifiable data on the session bound to the room
				/// </summary>
				public bool HasFixedData { get { return hasFixedData; } }

				/// <summary>
				/// If the online gaming experience is for a multi-platform game
				/// </summary>
				public bool IsCrossplatform { get { return isCrossplatform; } }

				/// <summary>
				/// If the room is closed or full. If it is, it won't be retrieved in searches
				/// </summary>
				public bool IsClosed { get { return isClosed; } }

				/// <summary>
				/// Find a room member id using the members account id.
				/// </summary>
				/// <param name="accountId">The account id to find in the room</param>
				/// <returns>If the memeber is found, retuns the member id; other returns <see cref="INVALID_ROOM_MEMBER_ID"/></returns>
				public UInt16 FindRoomMemberId(Core.NpAccountId accountId)
				{
					if (currentMembers == null) return INVALID_ROOM_MEMBER_ID;

					for (int i = 0; i < currentMembers.Length; i++)
					{
						if (currentMembers[i].OnlineUser.accountId == accountId)
						{
							return currentMembers[i].roomMemberId;
						}
					}

					return INVALID_ROOM_MEMBER_ID;
				}

				/// <summary>
				/// Find a room member id using the members online id.
				/// </summary>
				/// <param name="onlineId">The online id to find in the room.</param>
				/// <returns>If the memeber is found, retuns the member id; other returns <see cref="INVALID_ROOM_MEMBER_ID"/></returns>
				public UInt16 FindRoomMemberId(Core.OnlineID onlineId)
				{
					if (currentMembers == null) return INVALID_ROOM_MEMBER_ID;

					for (int i = 0; i < currentMembers.Length; i++)
					{
						if (currentMembers[i].OnlineUser.onlineId == onlineId)
						{
							return currentMembers[i].roomMemberId;
						}
					}

					return INVALID_ROOM_MEMBER_ID;
				}

				internal void Read(MemoryBuffer buffer)
				{
					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.RoomBegin);
			
					matchingContext = buffer.ReadUInt16();
					serverId = buffer.ReadUInt16();
					worldId = buffer.ReadUInt32();
					roomId = buffer.ReadUInt64();

					UInt64 numAttributes = buffer.ReadUInt64();

					attributes = new Attribute[numAttributes];

					for (UInt64 i = 0; i < numAttributes; i++)
					{
						attributes[i].Read(buffer);
					}

					buffer.ReadString(ref name);

					UInt64 numCurrentMembers = buffer.ReadUInt64();

					currentMembers = new Member[numCurrentMembers];

					for (UInt64 i = 0; i < numCurrentMembers; i++)
					{
						currentMembers[i] = new Member();
						currentMembers[i].Read(buffer);
					}

					numMaxMembers = buffer.ReadUInt64();
					topology = (TopologyType)buffer.ReadUInt32();
					numReservedSlots = buffer.ReadUInt32();

					isNatRestricted = buffer.ReadBool();
					allowBlockedUsersOfOwner = buffer.ReadBool();
					allowBlockedUsersOfMembers = buffer.ReadBool();
					joinAllLocalUsers = buffer.ReadBool();

					ownershipMigration = (RoomMigrationType)buffer.ReadUInt32();
					visibility = (RoomVisibility)buffer.ReadUInt32();

					password.Read(buffer);

					boundSessionId.Read(buffer);

					isSystemJoinable = buffer.ReadBool();
					displayOnSystem = buffer.ReadBool();
					hasChangeableData = buffer.ReadBool();
					hasFixedData = buffer.ReadBool();
					isCrossplatform = buffer.ReadBool();
					isClosed = buffer.ReadBool();

					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.RoomEnd);
				}
			}


			#endregion

			#region Get Worlds
		
			/// <summary>
			/// Response data class containing a list of worlds from the server. <see cref="GetWorlds"/>
			/// </summary>
			public class WorldsResponse : ResponseBase
			{
				internal World[] worlds;

				/// <summary>
				/// A list of retrieved worlds
				/// </summary>
				public World[] Worlds { get { return worlds; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.WorldsBegin);

					UInt64 numWorlds = readBuffer.ReadUInt64();

					worlds = new World[numWorlds];

					for (UInt64 i = 0; i < numWorlds; i++)
					{
						worlds[i].Read(readBuffer);
					}

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.WorldsEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function is used to get overall information of the Matching server where the matching context of the calling user is set.
			/// </summary>
			/// <param name="request">The information required to obtain the worlds information.</param>
			/// <param name="response">This response contains the return code and the existing worlds, with extra information of each one.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetWorlds(GetWorldsRequest request, WorldsResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetWorlds(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Create Room Request

			
			/// <summary>
			/// Response data class containing a list of worlds from the server. <see cref="GetWorlds"/>
			/// </summary>
			public class RoomResponse : ResponseBase
			{
				internal Room room;

				/// <summary>
				/// Thre room created by the <see cref="CreateRoom"/> method.
				/// </summary>
				public Room Room { get { return room; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.CreateRoomBegin);

					room = new Room();

					room.Read(readBuffer);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.CreateRoomEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function is used to create a room and join it as the owner.
			/// </summary> 
			/// <remarks>
			/// This only needs to be called once and is not dependent on the user. It does not need to be called seperately for each user. 
			/// </remarks>
			/// <param name="request"> The information required to create a room.</param>
			/// <param name="response"> This response contains the return code and the room created. The object should be kept persistently at application side.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int CreateRoom(CreateRoomRequest request, RoomResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				if (request.image.IsValid() == false)
				{
					throw new NpToolkitException("Request Image hasn't be defined. A session can't be created without an image.");
				}

                if (request.image.Exists() == false)
				{
					throw new NpToolkitException("Request Image doesn't exists. A session can't be created without an image. " + request.image.sessionImgPath);
				}

                if(request.status == null || request.status.Length == 0 )
                {
                    throw new NpToolkitException("Request Status text doesn't exists. A session can't be created without Status text being set.");
                }

				int ret = PrxCreateRoom(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Leave Room

			/// <summary>
			/// This function is used to abandon the room the user is in.
			/// </summary>
			/// <remarks>
			/// The calling user is in a room
			/// </remarks>
			/// <param name="request">The information to leave the room </param>
			/// <param name="response">This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int LeaveRoom(LeaveRoomRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxLeaveRoom(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Search Room

			/// <summary>
			/// Response data class containing a list of rooms matching a search criteria.
			/// </summary>
			public class RoomsResponse : ResponseBase
			{
				internal Room[] rooms;

				/// <summary>
				/// A list of retrieved rooms
				/// </summary>
				public Room[] Rooms
				{
					get { return rooms; }
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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.RoomsBegin);

					UInt64 numRooms = readBuffer.ReadUInt64();

					rooms = new Room[numRooms];

					for (UInt64 i = 0; i < numRooms; i++)
					{
						rooms[i] = new Room();
						rooms[i].Read(readBuffer);
					}

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.RoomsEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function searches for rooms on a world matching the conditions specified on the request, and is able to post-process the search if specified.
			/// </summary>
			/// <param name="request">The configuration to search the required rooms .</param>
			/// <param name="response">This response contains the return code and the rooms found (or joined/created, in case <c>quickJoin</c>/<c>createIfNotFound</c> are specified).</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int SearchRooms(SearchRoomsRequest request, RoomsResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxSearchRooms(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Join Room

			/// <summary>
			/// This function is used to join a room and become a member.
			/// </summary>
			/// <param name="request">The information required to join a room </param>
			/// <param name="response">This response contains the return code and the room joined. The object should be kept persistently at application side</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int JoinRoom(JoinRoomRequest request, RoomResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxJoinRoom(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Get Room Ping Time

			/// <summary>
			/// Response data class containing signaling information (RTT) with the room.
			/// </summary>
			public class GetRoomPingTimeResponse : ResponseBase
			{
				UInt32 roundTripTime;

				/// <summary>
				/// The time for a signal to be sent plus the time for the ACK to be received. Also known as ping time or round-trip delay time
				/// </summary>
				public UInt32 RoundTripTime
				{
					get { return roundTripTime; }
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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.RoomPingTimeBegin);

					roundTripTime = readBuffer.ReadUInt32();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.RoomPingTimeEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function provides round trip time (or ping time) information from the application to a room.
			/// </summary>
			/// <param name="request">The information required to obtain the ping time  </param>
			/// <param name="response">This response contains the return codeand the RTT to the room asked.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetRoomPingTime(GetRoomPingTimeRequest request, GetRoomPingTimeResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetRoomPingTime(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Kick Out Room Member

			/// <summary>
			/// This function is used to expulse a member from the room.
			/// </summary>
			/// <param name="request">The information to kick out the room member </param>
			/// <param name="response">This response does not have data, only return code.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int KickOutRoomMember(KickOutRoomMemberRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxKickOutRoomMember(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Send Room Message

			/// <summary>
			/// This function sends a message to members of a room.
			/// </summary>
			/// <param name="request">The message to be sent to the room members. </param>
			/// <param name="response">This response does not have data, only return code. </param>
			/// <returns>If the operation is asynchronous, the function provides the request Id. </returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int SendRoomMessage(SendRoomMessageRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxSendRoomMessage(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Get Attributes

			/// <summary>
			/// This function obtains attributes related to a room or member directly from the server.
			/// </summary>
			/// <remarks>
			/// This function is provided in case the notification system is not enough or the application prefers to be designed to request attributes when desired. It is recommended to use notifications whenever possible to keep the room information always up to date.
			/// </remarks>
			/// <param name="request">The information of the attributes to obtain. </param>
			/// <param name="response">This response contains the return code and the information updated to be merged at application side</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetAttributes(GetAttributesRequest request, RefreshRoomResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetAttributes(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Get Data

			/// <summary>
			/// Response data class containing data (modifiable or not) linked to the bound session of the current room.
			/// </summary>
			public class GetDataResponse : ResponseBase
			{
				internal byte[] data;
				internal DataType type;

				/// <summary>
				/// A buffer containing the requested data
				/// </summary>
				public byte[] Data
				{
					get { return data; }
				}

				/// <summary>
				/// If the buffer is from modifiable or non-modifiable data
				/// </summary>
				public DataType Type
				{
					get { return type; }
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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.GetDataBegin);

					type = (DataType)readBuffer.ReadUInt32();

					readBuffer.ReadData(ref data);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.GetDataEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function is used to retrieve the data (modifiable or non-modifiable) of the session bound to a room.
			/// </summary>
			/// <param name="request">The identification of the data to be obtained .</param>
			/// <param name="response">This response contains the return code and the requested data.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetData(GetDataRequest request, GetDataResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetData(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Send Invitiation

			/// <summary>
			/// This function sends a system invitation to another user to join the room and bound Session of the calling user.
			/// </summary>
			/// <param name="request">The invitation to be sent.</param>
			/// <param name="response">This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int SendInvitation(SendInvitationRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxSendInvitation(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Set Room Info

			/// <summary>
			/// This function is used to modify information of the current room or member.
			/// </summary>
			/// <param name="request">The members to be updated in the current room.</param>
			/// <param name="response">This response does not have data, only return code.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int SetRoomInfo(SetRoomInfoRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxSetRoomInfo(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion 


			#region Set Members As Recently Met

#if SUPPORT_RECENTLY_MET
			/// <summary>
			/// Sets a group room members as "Players Met" in the system. This information will appear on the system as users that have been Recently Met.
			/// </summary>
			/// <param name="request">Contains the information to make the request.</param>
			/// <param name="response">This response does not have data, only return code.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int SetMembersAsRecentlyMet(SetMembersAsRecentlyMetRequest request, Core.EmptyResponse response)
			{
				if (Main.initResult.sceSDKVersion < 0x04500000 )
				{
					throw new NpToolkitException("SetMembersAsRecentlyMet is only available in SDK version 4.5 or greater.");
				}

				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxSetMembersAsRecentlyMet(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

#endif
			#endregion 

			// Notifications

			#region Refresh Room Notification

			/// <summary>
			/// Enum listing the possible reasons to refresh a room.
			/// </summary>
			public enum Reasons
			{
				/// <summary> A member joined </summary>
				MemberJoined,
				/// <summary> A member left, see <see cref="RefreshRoomResponse.Cause"/> for more granularity </summary>
				MemberLeft,
				/// <summary> Connection with a member was either established or lost </summary>
				MemberSignalingUpdate,
				/// <summary> Attributes or other basic information of a member has changed </summary>
				MemberInfoUpdated,
				/// <summary> The owner changed, see <see cref="RefreshRoomResponse.Cause"/> for more granularity </summary>
				OwnerChanged,
				/// <summary> The room was destroyed, see <see cref="RefreshRoomResponse.Cause"/> for more granularity </summary>
				RoomDestroyed,
				/// <summary> The user was kicked out from the room </summary>
				RoomKickedOut,
				/// <summary> External information of the room has changed </summary>
				RoomExternalInfoUpdated,
				/// <summary> Internal information of the room has changed </summary>
				RoomInternalInfoUpdated,
				/// <summary> Topology information of the room has changed </summary>
				RoomTopologyUpdated,
				/// <summary> Session-related information to the room has changed </summary>
				RoomSessionInfoUpdated
			};

			/// <summary>
			/// Cause of room response. Maps to SCE_NP_MATCHING2_EVENT_CAUSE_XXX defines
			/// </summary>
			public enum Causes
			{
				/// <summary> Unknown cause. </summary>
				Unknown = 1,
				/// <summary> A member explicitly left the room. (SCE_NP_MATCHING2_EVENT_CAUSE_LEAVE_ACTION). </summary>
				LeaveAction = 1,
				/// <summary> A member explicitly kicked another member out. (SCE_NP_MATCHING2_EVENT_CAUSE_KICKOUT_ACTION). </summary>
				KickoutAction = 2,
				/// <summary> A member explicitly transferred room ownership. (SCE_NP_MATCHING2_EVENT_CAUSE_GRANT_OWNER_ACTION). </summary>
				GrantOwnerAction = 3,
				/// <summary> Event was caused by server operation. (SCE_NP_MATCHING2_EVENT_CAUSE_SERVER_OPERATION). </summary>
				ServerOperation = 4,
				/// <summary> A member disappeared. (SCE_NP_MATCHING2_EVENT_CAUSE_MEMBER_DISAPPEARED). </summary>
				MemberDisappeared = 5,
				/// <summary> Event was caused by an internal server error. (SCE_NP_MATCHING2_EVENT_CAUSE_SERVER_INTERNAL). </summary>
				ServerInternal = 6,
				/// <summary> Connection was severed. (SCE_NP_MATCHING2_EVENT_CAUSE_CONNECTION_ERROR). </summary>
				ConnectionError = 7,
				/// <summary> Signed out. (SCE_NP_MATCHING2_EVENT_CAUSE_NP_SIGNED_OUT). </summary>
				SignedOut = 8,
				/// <summary> System error occurred. (SCE_NP_MATCHING2_EVENT_CAUSE_SYSTEM_ERROR). </summary>
				SystemError = 9,
				/// <summary> Context error occurred. (SCE_NP_MATCHING2_EVENT_CAUSE_CONTEXT_ERROR). </summary>
				ContextError = 10,
				/// <summary> Event was caused by a context operation. (SCE_NP_MATCHING2_EVENT_CAUSE_CONTEXT_ACTION). </summary>
				ContextAction = 11
			}

			/// <summary>
			/// Notification provided when a room has to be refreshed.
			/// </summary>
			public class RefreshRoomResponse : ResponseBase
			{
				/// <summary>
				/// Only set when the <see cref="RefreshRoomResponse.Reason"/> is <see cref="Reasons.OwnerChanged"/> 
				/// </summary>
				public class OwnerInformation
				{
					/// <summary>
					/// The size of the array containing the old and new owners of the room
					/// </summary>
					public const int OWNER_EXCHANGE_SIZE = 2;

					internal NpMatching2SessionPassword password;
					internal UInt16[] oldAndNewOwners;

					/// <summary>
					/// Only set when the user is the new owner. The password for the private slots
					/// </summary>
					public NpMatching2SessionPassword Password { get { return password; } }

					/// <summary>
					/// The member Ids of the old and new owners
					/// </summary>
					public UInt16[] OldAndNewOwners { get { return oldAndNewOwners; } }

					internal void Read(MemoryBuffer buffer)
					{
						password.Read(buffer);

						for (int i = 0; i < OWNER_EXCHANGE_SIZE; i++)
						{
							oldAndNewOwners[i] = buffer.ReadUInt16();
						}
					}

					/// <summary>
					/// Initializes a new instance of the <see cref="JoinRoomRequest"/> class.
					/// </summary>
					internal OwnerInformation()
					{
						oldAndNewOwners = new UInt16[OWNER_EXCHANGE_SIZE];
					}
				}

				/// <summary>
				/// Only set when the <see cref="RefreshRoomResponse.Reason"/> is <see cref="Reasons.RoomExternalInfoUpdated"/> 
				/// </summary>
				public class RoomExternalInformation
				{
					internal Attribute[] attributes;

					/// <summary>
					/// The external attributes modified on the room
					/// </summary>
					public Attribute[] Attributes { get { return attributes; } }

					internal void Read(MemoryBuffer buffer)
					{
						UInt64 numAttributes = buffer.ReadUInt64();

						attributes = new Attribute[numAttributes];

						for (UInt64 i = 0; i < numAttributes; i++)
						{
							attributes[i] = new Attribute();
							attributes[i].Read(buffer);
						}
					}
				}

				/// <summary>
				/// Only set when the <see cref="RefreshRoomResponse.Reason"/> is <see cref="Reasons.RoomInternalInfoUpdated"/> 
				/// </summary>
				public class RoomInternalInformation
				{
					internal Attribute[] attributes;

					internal Core.OptionalBoolean allowBlockedUsersOfMembers;
					internal Core.OptionalBoolean joinAllLocalUsers;
					internal Core.OptionalBoolean isNatRestricted;
					internal UInt32 numReservedSlots;
					internal RoomVisibility visibility;
					internal Core.OptionalBoolean closeRoom;

					/// <summary>
					/// The internal attributes modified on the room
					/// </summary>
					public Attribute[] Attributes { get { return attributes; } }

					/// <summary>
					/// Allows or not adding blocked users of members to the blacklist of the room. The blocked users already added are kept
					/// </summary>
					public Core.OptionalBoolean AllowBlockedUsersOfMembers { get { return allowBlockedUsersOfMembers; } }

					/// <summary>
					/// Allows or not adding all local users to rooms when more than one local user is available on one system.
					/// </summary>
					public Core.OptionalBoolean JoinAllLocalUsers { get { return joinAllLocalUsers; } }

					/// <summary>
					/// Restricts or not the members that can join the room to only those who can establish P2P connections
					/// </summary>
					public Core.OptionalBoolean IsNatRestricted { get { return isNatRestricted; } }

					/// <summary>
					/// Modifies the number of reserved slots in the room. 
					/// </summary>
					public UInt32 NumReservedSlots { get { return numReservedSlots; } }

					/// <summary>
					/// Makes the room private, public or public with reserved slots
					/// </summary>
					public RoomVisibility Visibility { get { return visibility; } }

					/// <summary>
					/// Closes the room. It is no longer provided in searches
					/// </summary>
					public Core.OptionalBoolean CloseRoom { get { return closeRoom; } }

					internal void Read(MemoryBuffer buffer)
					{
						UInt64 numAttributes = buffer.ReadUInt64();

						attributes = new Attribute[numAttributes];

						for (UInt64 i = 0; i < numAttributes; i++)
						{
							attributes[i] = new Attribute();
							attributes[i].Read(buffer);
						}

						allowBlockedUsersOfMembers = (Core.OptionalBoolean)buffer.ReadUInt32();
						joinAllLocalUsers = (Core.OptionalBoolean)buffer.ReadUInt32();
						isNatRestricted = (Core.OptionalBoolean)buffer.ReadUInt32();

						numReservedSlots = buffer.ReadUInt32();
						visibility = (RoomVisibility)buffer.ReadUInt32();
						closeRoom = (Core.OptionalBoolean)buffer.ReadUInt32();
					}
				}

				/// <summary>
				/// Only set when the <see cref="RefreshRoomResponse.Reason"/> is <see cref="Reasons.RoomSessionInfoUpdated"/> 
				/// </summary>
				public class RoomSessionInformation
				{
					internal Core.OptionalBoolean displayOnSystem;
					internal Core.OptionalBoolean isSystemJoinable;
					internal Core.OptionalBoolean hasChangeableData;
					internal NpSessionId boundSessionId;

					/// <summary>
					/// Makes the Session visible or not in the system 
					/// </summary>
					public Core.OptionalBoolean DisplayOnSystem { get { return displayOnSystem; } }

					/// <summary>
					/// Allows or not users joining from the system (the platform)
					/// </summary>
					public Core.OptionalBoolean IsSystemJoinable { get { return isSystemJoinable; } }

					/// <summary>
					/// Specifies if there is changeable data in the Session
					/// </summary>
					public Core.OptionalBoolean HasChangeableData { get { return hasChangeableData; } }

					/// <summary>
					/// This handles edge cases where users join late to sessions 
					/// </summary>
					public NpSessionId BoundSessionId { get { return boundSessionId; } }

					internal void Read(MemoryBuffer buffer)
					{
						displayOnSystem = (Core.OptionalBoolean)buffer.ReadUInt32();
						isSystemJoinable = (Core.OptionalBoolean)buffer.ReadUInt32();
						hasChangeableData = (Core.OptionalBoolean)buffer.ReadUInt32();

						boundSessionId.Read(buffer);
					}
				}


				internal UInt64 roomId;
				internal PresenceOptionData notificationFromMember;
				internal Reasons reason;
				internal Causes cause;

				internal OwnerInformation ownerInfo;   // Only set when the reason is ownerChanged
				internal Member memberInfo;	 	 // Only set when the reason is one of the member. Depending on the type, it will contain relevant information of the member 
				internal Int64 roomLeftError;	//< Only set when the <c>reason</c> is roomDestroyed or roomKickedOut. The error why the room was left

				internal RoomExternalInformation roomExternalInfo; // Only set when the reason is RoomExternalInfoUpdated
				internal RoomInternalInformation roomInternalInfo; // Only set when the reason is RoomInternalInfoUpdated

				internal RoomSessionInformation roomSessionInfo;
				internal TopologyType roomTopology;

				/// <summary>
				/// The room where the notification is received
				/// </summary>
				public UInt64 RoomId { get { return roomId; } }

				/// <summary>
				/// Notification sent to other members. Application-defined data
				/// </summary>
				public PresenceOptionData NotificationFromMember { get { return notificationFromMember; } }

				/// <summary>
				/// The reason why the room information needs to be refreshed
				/// </summary>
				public Reasons Reason { get { return reason; } }

				/// <summary>
				/// For the reasons that require more granularity, it provides extra information of the cause for the update
				/// </summary>
				public Causes Cause { get { return cause; } }

				/// <summary>
				/// Only set when the <see cref="Reason"/> is <see cref="Reasons.OwnerChanged"/>
				/// </summary>
				public OwnerInformation OwnerInfo { get { return ownerInfo; } }

				/// <summary>
				/// Only set when <see cref="Reason"/> is one of the 'Member' enums, see remarks. Depending on the type, it will contain relevant information of the member 
				/// </summary>
				/// <remarks>
				/// Only set when <see cref="Reason"/> is either <see cref="Reasons.MemberJoined"/>, <see cref="Reasons.MemberLeft"/>, <see cref="Reasons.MemberSignalingUpdate"/> or <see cref="Reasons.MemberInfoUpdated"/>.
				/// </remarks>
				public Member MemberInfo { get { return memberInfo; } }

				/// <summary>
				/// Only set when the <see cref="Reason"/> is <see cref="Reasons.RoomDestroyed"/> or <see cref="Reasons.RoomKickedOut"/>
				/// </summary>
				public Int64 RoomLeftError { get { return roomLeftError; } }

				/// <summary>
				/// Only set when the <see cref="Reason"/> is <see cref="Reasons.RoomExternalInfoUpdated"/>
				/// </summary>
				public RoomExternalInformation RoomExternalInfo { get { return roomExternalInfo; } }

				/// <summary>
				/// Only set when the <see cref="Reason"/> is <see cref="Reasons.RoomInternalInfoUpdated"/>
				/// </summary>
				public RoomInternalInformation RoomInternalInfo { get { return roomInternalInfo; } }

				/// <summary>
				/// Only set when the <see cref="Reason"/> is <see cref="Reasons.RoomSessionInfoUpdated"/>
				/// </summary>
				public RoomSessionInformation RoomSessionInfo { get { return roomSessionInfo; } }

				/// <summary>
				/// Only set when the <see cref="Reason"/> is <see cref="Reasons.RoomTopologyUpdated"/>
				/// </summary>
				public TopologyType RoomTopology { get { return roomTopology; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.RefreshRoomBegin);

					roomId = readBuffer.ReadUInt64();

					notificationFromMember.Read(readBuffer);

					reason = (Reasons)readBuffer.ReadUInt32();
					cause = (Causes)readBuffer.ReadUInt32();

					if (reason == Reasons.MemberJoined ||
						 reason == Reasons.MemberLeft ||
						 reason == Reasons.MemberSignalingUpdate ||
						 reason == Reasons.MemberInfoUpdated)
					{
						memberInfo = new Member();
						memberInfo.Read(readBuffer);
					}
					else if (reason == Reasons.OwnerChanged)
					{
						ownerInfo = new OwnerInformation();
						ownerInfo.Read(readBuffer);
					}
					else if (reason == Reasons.RoomDestroyed || reason == Reasons.RoomKickedOut)
					{
						roomLeftError = readBuffer.ReadInt64();
					}
					else if (reason == Reasons.RoomExternalInfoUpdated)
					{
						roomExternalInfo = new RoomExternalInformation();
						roomExternalInfo.Read(readBuffer);
					}
					else if (reason == Reasons.RoomInternalInfoUpdated)
					{
						roomInternalInfo = new RoomInternalInformation();
						roomInternalInfo.Read(readBuffer);
					}
					else if (reason == Reasons.RoomSessionInfoUpdated)
					{
						roomSessionInfo = new RoomSessionInformation();
						roomSessionInfo.Read(readBuffer);
					}
					else if (reason == Reasons.RoomTopologyUpdated)
					{
						roomTopology = (TopologyType)readBuffer.ReadUInt32();
					}

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.RefreshRoomEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			#endregion

			#region New Room Message

			/// <summary>
			/// Notification provided when a room has to be refreshed.
			/// </summary>
			public class NewRoomMessageResponse : ResponseBase
			{
				internal UInt64 roomId;
				internal byte[] data;
				internal UInt16 sender; // RoomMemberId
				internal bool isChatMsg;
				internal bool isFiltered;

				/// <summary>
				/// The room where the message is received
				/// </summary>
				public UInt64 RoomId { get { return roomId; } }

				/// <summary>
				/// The message information. If it is a chat message, the data is in UTF-8
				/// </summary>
				public byte[] Data { get { return data; } }

				/// <summary>
				/// The message information as a string. If it is a chat message, the data is in UTF-8
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if <see cref="IsChatMsg"/> is false.</exception>
				public string DataAsString 
				{ 
					get 
					{
						if (isChatMsg == false)
						{
							throw new NpToolkitException("Room message data is not a UTF-8 string.");
						}
						if (data == null) return "";
						return System.Text.Encoding.UTF8.GetString(data, 0, data.Length);; 
					} 
				}

				/// <summary>
				/// The member sending the message
				/// </summary>
				public UInt16 Sender { get { return sender; } }

				/// <summary>
				/// If the message is a chat message. In that case, the <see cref="Data"/> will be in UTF-8 and it may be filtered by the vulgarity filter
				/// </summary>
				public bool IsChatMsg { get { return isChatMsg; } }

				/// <summary>
				/// Indicates if the chat message has been filtered or not.
				/// </summary>
				public bool IsFiltered { get { return isFiltered; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NewRoomMessageBegin);

					roomId = readBuffer.ReadUInt64();

					readBuffer.ReadData(ref data);

					sender = readBuffer.ReadUInt16();

					isChatMsg = readBuffer.ReadBool();
					isFiltered = readBuffer.ReadBool();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.NewRoomMessageEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			#endregion		

			#region Invitation Received

			/// <summary>
			/// The platform that the session is active on
			/// </summary>
			public enum CurrentPlatform
			{
				/// <summary> The value has not been set. </summary>
				NotSet,
				/// <summary> Current platform is PS Vita. </summary>
				PSVita,
				/// <summary> Current platform is PS4. </summary>
				PS4,
			};

			/// <summary>
			/// Push notification that is sent to the NP Toolkit 2 callback when an invitation has been received
			/// </summary>
			public class InvitationReceivedResponse : ResponseBase
			{
				internal Core.OnlineUser localUpdatedUser = new Core.OnlineUser();
				internal Core.OnlineUser remoteUser = new Core.OnlineUser();
				internal CurrentPlatform platform;

				/// <summary>
				/// IDs of the local recipient
				/// </summary>
				public Core.OnlineUser LocalUpdatedUser { get { return localUpdatedUser; } }

				/// <summary>
				/// IDs of the sender
				/// </summary>
				public Core.OnlineUser RemoteUser { get { return remoteUser; } }

				/// <summary>
				/// The platform that the invitation was sent from
				/// </summary>
				public CurrentPlatform Platform { get { return platform; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.InvitationReceivedBegin);

					localUpdatedUser.Read(readBuffer);
					remoteUser.Read(readBuffer);

					platform = (CurrentPlatform)readBuffer.ReadUInt32();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.InvitationReceivedEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			#endregion

			#region Session Invitation Event

			/// <summary>
			/// Invitation ID
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct NpInvitationId  // Maps to SceNpInvitationId
			{
				/// <summary>
				/// Maximum length of the invitiation id
				/// </summary>
				public const int NP_INVITATION_ID_MAX_SIZE = 60;  // SCE_NP_INVITATION_ID_MAX_SIZE

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = NP_INVITATION_ID_MAX_SIZE)]
				internal string id;

				/// <summary>
				/// Invitation ID string 
				/// </summary>
				public string Id
				{
					get { return id; }
				}

				internal void Read(MemoryBuffer buffer)
				{
					buffer.ReadString(ref id);
				}

				/// <summary>
				/// Return the id as a string
				/// </summary>
				/// <returns>The invitiation id.</returns>
				public override string ToString()
				{
					return id;
				}
			}

			/// <summary>
			/// Notification that is received when a local player accepts an invitiation to join a room.
			/// </summary>
			public class SessionInvitationEventResponse : ResponseBase
			{
				internal NpSessionId sessionId;
				internal NpInvitationId invitationId;
				internal bool acceptedInvite; 
				internal Core.OnlineID onlineId = new Core.OnlineID();
				internal Core.UserServiceUserId userId;
				internal Core.OnlineID referralOnlineId = new Core.OnlineID();
				internal Core.NpAccountId referralAccountId;

				/// <summary>
				/// Session ID
				/// </summary>
				public NpSessionId SessionId { get { return sessionId; } }

				/// <summary>
				/// Invitation ID
				/// </summary>
				public NpInvitationId InvitationId { get { return invitationId; } }

				/// <summary>
				/// If true, the user "accepts invitation and joins the game" and has a valid invitation id. Otherwise it means the user performed an action that joins from session information, rather than an invitation.
				/// </summary>
				public bool AcceptedInvite { get { return acceptedInvite; } }

				/// <summary>
				/// Online ID of user who joined session or accepted invitation
				/// </summary>
				public Core.OnlineID OnlineId { get { return onlineId; } }

				/// <summary>
				/// User ID of user who joined session or accepted invitation
				/// </summary>
				public Core.UserServiceUserId UserId { get { return userId; } }

				/// <summary>
				/// Online ID of the referral source user
				/// </summary>
				public Core.OnlineID ReferralOnlineId { get { return referralOnlineId; } }

				/// <summary>
				/// Account ID of the referral source user
				/// </summary>
				public Core.NpAccountId ReferralAccountId { get { return referralAccountId; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.SessionInvitationEventBegin);

					sessionId.Read(readBuffer);

					invitationId.Read(readBuffer);

					Int32 flag = readBuffer.ReadInt32();

					if (((flag) & 0x1) != 0)
					{
						//If flag is SCE_NP_SESSION_INVITATION_EVENT_FLAG_INVITATION (0x1), it indicates an action where the user "accepts invitation and joins the game". At this time, a valid invitation ID will be stored in invitationId.
						//Otherwise, it indicates that a user has performed an action that joins from session information rather than from an invitation. At this time, invitationId will be invalid.
						acceptedInvite = true;
					}

					onlineId.Read(readBuffer);
					userId.Read(readBuffer);
					referralOnlineId.Read(readBuffer);
					referralAccountId.Read(readBuffer);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.SessionInvitationEventEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			#endregion

			#region Play Together Event

			/// <summary>
			/// This structure contains ID information for an invitee to Play Together.
			/// </summary>
			public struct NpPlayTogetherInvitee  // Maps to SceNpPlayTogetherInvitee
			{
				internal Core.NpAccountId accountId;
				internal Core.OnlineID onlineId;

				/// <summary>
				/// The account ID to invite to Play Together.
				/// </summary>
				public Core.NpAccountId AccountId { get { return accountId; } }

				/// <summary>
				/// The online ID to invite to Play Together.
				/// </summary>
				public Core.OnlineID OnlineId { get { return onlineId; } }

				internal void Read(MemoryBuffer buffer)
				{
					onlineId = new Core.OnlineID();

					accountId.Read(buffer);
					onlineId.Read(buffer);
				}
			}

			/// <summary>
			/// Notification that is received when a local player starts the app via playtogether.
			/// </summary>
			public class PlayTogetherHostEventResponse : ResponseBase
			{
				internal Core.UserServiceUserId userId;
				internal NpPlayTogetherInvitee[] invitees;

				/// <summary>
				/// User ID of the local user that started the game.
				/// </summary>
				public Core.UserServiceUserId UserId { get { return userId; } }

				/// <summary>
				/// The players to invite to Play Together.
				/// </summary>
				public NpPlayTogetherInvitee[] Invitees { get { return invitees; } }


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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.PlayTogetherHostEventBegin);

					userId.Read(readBuffer);

					UInt32 inviteeListLen = readBuffer.ReadUInt32();

					invitees = new NpPlayTogetherInvitee[inviteeListLen];

					for (int i = 0; i < inviteeListLen; i++)
					{
						invitees[i].Read(readBuffer);
					}

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.PlayTogetherHostEventEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			#endregion
		}
	}
}
