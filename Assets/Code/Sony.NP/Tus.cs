using System;
using System.Runtime.InteropServices;

namespace Sony
{
	namespace NP
	{
		/// <summary>
		/// TUS service related functionality.
		/// </summary>
		public class Tus
		{
			#region DLL Imports

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxTusSetVariables(SetVariablesRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxTusGetVariables(GetVariablesRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxTusAddToAndGetVariable(AddToAndGetVariableRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxTusSetData(SetDataRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxTusGetData(GetDataRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxTusDeleteData(DeleteDataRequest request, out APIResult result);

			#endregion

			#region General
			/// <summary>
			/// 16 character TUS virtual user Id
			/// </summary>
		    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct VirtualUserID  // Maps to SceNpTusVirtualUserId
			{
				/// <summary>
				/// Maximum length of the virtual id
				/// </summary>
				public const int NP_ONLINEID_MAX_LENGTH = 16;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = NP_ONLINEID_MAX_LENGTH+1)]
				internal string name;

				/// <summary>
				/// Display representation of an online user
				/// </summary>
				public string Name
				{
					get { return name; }
					set
					{
						if (value.Length > NP_ONLINEID_MAX_LENGTH)
						{
							throw new NpToolkitException("VirtualUserID can't be more than " + NP_ONLINEID_MAX_LENGTH + " characters.");
						}

						name = value;
					}
				}

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					buffer.ReadString(ref name);
				}

				/// <summary>
				/// Returns the OnlineId as a string, with a maximum of 16 characters.
				/// </summary>
				/// <returns>The OnLineId name</returns>
				public override string ToString()
				{
					return name;
				}
			};

			/// <summary>
			/// Represents a TUS (title user storage) variable
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct Variable
			{
				internal Int64 varValue;
				internal Int32 slotId;

				/// <summary>
				/// The TUS variable value
				/// </summary>
				public Int64 Value
				{
					get { return varValue; }
					set { varValue = value; }
				}

				/// <summary>
				/// The slot that the variable belongs to
				/// </summary>
				public Int32 SlotId
				{
					get { return slotId; }
					set { slotId = value; }
				}
			}

			/// <summary>
			/// Common data members for TUS variable
			/// </summary>
			public class NpVariableBase
			{
				internal bool hasData;
				internal DateTime lastChangedDate;
				internal Int64 variable;
				internal Int64 oldVariable;
				internal Core.NpAccountId ownerAccountId;
				internal Core.NpAccountId lastChangedAuthorAccountId;

				/// <summary>
				/// Flag indicating whether a value has been set.
				/// </summary>
				public bool HasData { get { return hasData; } }

				/// <summary>
				/// Last update date
				/// </summary>
				public DateTime LastChangedDate { get { return lastChangedDate; } }

				/// <summary>
				/// Currently set value
				/// </summary>
				public Int64 Variable { get { return variable; } }

				/// <summary>
				/// Previously set value
				/// </summary>
				public Int64 OldVariable { get { return oldVariable; } }

				/// <summary>
				/// Account ID of the owner
				/// </summary>
				public Core.NpAccountId OwnerAccountId { get { return ownerAccountId; } }

				/// <summary>
				/// Account ID of the user who made the last update
				/// </summary>
				public Core.NpAccountId LastChangedAuthorAccountId { get { return lastChangedAuthorAccountId; } }

				// Read data from PRX marshaled buffer
				internal void ReadBase(MemoryBuffer buffer)
				{
					hasData = buffer.ReadBool();
					lastChangedDate = Core.ReadRtcTick(buffer);
					variable = buffer.ReadInt64();
					oldVariable = buffer.ReadInt64();
					ownerAccountId.Read(buffer);
					lastChangedAuthorAccountId.Read(buffer);
				}
			}

			/// <summary>
			/// Represents a TUS variable
			/// </summary>
			public class NpVariable : NpVariableBase
			{
				internal Core.OnlineID ownerId;
				internal Core.OnlineID lastChangedAuthorId;

				/// <summary>
				/// Online ID of the owner
				/// </summary>
				public Core.OnlineID OwnerId { get { return ownerId; } }

				/// <summary>
				/// Online ID of the user who made the last update
				/// </summary>
				public Core.OnlineID LastChangedAuthorId { get { return lastChangedAuthorId; } }

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					ReadBase(buffer);

					ownerId = new Core.OnlineID();
					ownerId.Read(buffer);

					lastChangedAuthorId = new Core.OnlineID();
					lastChangedAuthorId.Read(buffer);
				}
			}

			/// <summary>
			/// Represents a TUS variable (for cross-platform use)
			/// </summary>
			public class NpVariableForCrossSave : NpVariableBase
			{
				internal Core.NpId ownerId;
				internal Core.NpId lastChangedAuthorId;

				/// <summary>
				/// Owner
				/// </summary>
				public Core.NpId OwnerId { get { return ownerId; } }

				/// <summary>
				/// Last updated by
				/// </summary>
				public Core.NpId LastChangedAuthorId { get { return lastChangedAuthorId; } }

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					ReadBase(buffer);

					ownerId.Read(buffer);
					lastChangedAuthorId.Read(buffer);
				}
			}

			/// <summary>
			/// Common data members for TUS status
			/// </summary>
			public class NpTusDataStatusBase
			{
				internal bool hasData;
				internal DateTime lastChangedDate;
				internal byte[] data;
				internal byte[] supplementaryInfo;
				internal Core.NpAccountId ownerAccountId;
				internal Core.NpAccountId lastChangedAuthorAccountId;

				/// <summary>
				/// Flag indicating whether a value has been set.
				/// </summary>
				public bool HasData { get { return hasData; } }

				/// <summary>
				/// Last update date
				/// </summary>
				public DateTime LastChangedDate { get { return lastChangedDate; } }

				/// <summary>
				/// The TUS data
				/// </summary>
				public byte[] Data { get { return data; } }

				/// <summary>
				///  The TUS sumplementary info
				/// </summary>
				public byte[] SupplementaryInfo { get { return supplementaryInfo; } }

				/// <summary>
				/// Account ID of the owner
				/// </summary>
				public Core.NpAccountId OwnerAccountId { get { return ownerAccountId; } }

				/// <summary>
				/// Account ID of the user who made the last update
				/// </summary>
				public Core.NpAccountId LastChangedAuthorAccountId { get { return lastChangedAuthorAccountId; } }

				// Read data from PRX marshaled buffer
				internal void ReadBase(MemoryBuffer buffer)
				{
					hasData = buffer.ReadBool();
					lastChangedDate = Core.ReadRtcTick(buffer);

					buffer.ReadData(ref data);
					buffer.ReadData(ref supplementaryInfo);

					ownerAccountId.Read(buffer);
					lastChangedAuthorAccountId.Read(buffer);
				}
			}

			/// <summary>
			/// Represents the status of TUS data
			/// </summary>
			public class NpTusDataStatus : NpTusDataStatusBase
			{
				internal Core.OnlineID ownerId;
				internal Core.OnlineID lastChangedAuthorId;

				/// <summary>
				/// Online ID of the owner
				/// </summary>
				public Core.OnlineID OwnerId { get { return ownerId; } }

				/// <summary>
				/// Online ID of the user who made the last update
				/// </summary>
				public Core.OnlineID LastChangedAuthorId { get { return lastChangedAuthorId; } }

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					ReadBase(buffer);

					ownerId = new Core.OnlineID();
					ownerId.Read(buffer);

					lastChangedAuthorId = new Core.OnlineID();
					lastChangedAuthorId.Read(buffer);
				}
			}

			/// <summary>
			/// Represents  the status of TUS data (for cross-platform use)
			/// </summary>
			public class NpTusDataStatusForCrossSave : NpTusDataStatusBase
			{
				internal Core.NpId ownerId;
				internal Core.NpId lastChangedAuthorId;

				/// <summary>
				/// Owner
				/// </summary>
				public Core.NpId OwnerId { get { return ownerId; } }

				/// <summary>
				/// Last updated by
				/// </summary>
				public Core.NpId LastChangedAuthorId { get { return lastChangedAuthorId; } }

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					ReadBase(buffer);

					ownerId.Read(buffer);
					lastChangedAuthorId.Read(buffer);
				}
			}

			#endregion

			#region Requests
			
			/// <summary>
			/// Parameters required for setting a specified users TUS variables
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
			public class SetVariablesRequest : RequestBase
			{
				/// <summary>
				/// Maximum size of the <see cref="Vars"/> array.
				/// </summary>
				public const int MAX_VARIABLE_SLOTS = 64;

				internal UInt64 numVars;  

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_VARIABLE_SLOTS)]
				Variable[] vars = new Variable[MAX_VARIABLE_SLOTS];

				internal Core.NpAccountId targetUser;

				internal VirtualUserID virtualUserID;

				[MarshalAs(UnmanagedType.I1)]
				internal bool isVirtualUser;

				/// <summary>
				/// The TUS variables to update
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is larger than <see cref="MAX_VARIABLE_SLOTS"/>.</exception>
				public Variable[] Vars
				{
					get 
					{
						if (numVars == 0) return null;

						Variable[] output = new Variable[numVars];

						Array.Copy(vars, output, (int)numVars);

						return output;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_VARIABLE_SLOTS)
							{
								throw new NpToolkitException("The size of the array is larger than " + MAX_VARIABLE_SLOTS);
							}

							value.CopyTo(vars, 0);
							numVars = (UInt64)value.Length;
						}
						else
						{
							numVars = 0;
						}
					}
				}

				/// <summary>
				/// The account of the user to set the variables for
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the <see cref="IsVirtualUser"/> is true.</exception>
				public Core.NpAccountId TargetUser
				{
					get 
					{
						if (isVirtualUser == true)
						{
							throw new NpToolkitException("A virtual user id is currently configured on for this request, meaning the TargetUser is not valid");
						}
						return targetUser; 
					}
					set 
					{ 
						targetUser = value;
						isVirtualUser = false;
					}
				}

				/// <summary>
				/// Identifier of a virtual user
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the <see cref="IsVirtualUser"/> is false.</exception>
				public VirtualUserID VirtualUserID
				{
					get 
					{
						if (isVirtualUser == false)
						{
							throw new NpToolkitException("This request doesn't have a virtual user id current set.");
						}
						return virtualUserID; 
					}
					set 
					{ 
						virtualUserID = value;

						if (value.name.Length > 0) isVirtualUser = true;
						else isVirtualUser = false;
					}
				}

				/// <summary>
				/// A flag that specifies whether this update is for a virtual user. This is atomatically set depending on if <see cref="TargetUser"/> or <see cref="VirtualUserID"/> is set.
				/// </summary>
				public bool IsVirtualUser
				{
					get { return isVirtualUser; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="SetVariablesRequest"/> class.
				/// </summary>
				public SetVariablesRequest()
					: base(ServiceTypes.Tus, FunctionTypes.TusSetVariables)
				{

				}
			}

			/// <summary>
			/// Parameters required for getting a specified users TUS variables
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class GetVariablesRequest : RequestBase
			{
				/// <summary>
				/// Maximum size of the <see cref="SlotIds"/> array.
				/// </summary>
				public const int MAX_VARIABLE_SLOTS = 64;

				internal UInt64 numSlots;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_VARIABLE_SLOTS)]
				internal Int32[] slotIds = new Int32[MAX_VARIABLE_SLOTS];

				internal Core.NpAccountId targetUser;

				internal VirtualUserID virtualUserID;

				[MarshalAs(UnmanagedType.I1)]
				internal bool isVirtualUser;

				[MarshalAs(UnmanagedType.I1)]
				internal bool forCrossSave;

				/// <summary>
				/// The IDs of the slots to retrieve variables from
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is larger than <see cref="MAX_VARIABLE_SLOTS"/>.</exception>
				public Int32[] SlotIds
				{
					get 
					{
						if (numSlots == 0) return null;

						Int32[] output = new Int32[numSlots];

						Array.Copy(slotIds, output, (int)numSlots);

						return output;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_VARIABLE_SLOTS)
							{
								throw new NpToolkitException("The size of the array is larger than " + MAX_VARIABLE_SLOTS);
							}

							value.CopyTo(slotIds, 0);
							numSlots = (UInt64)value.Length;
						}
						else
						{
							numSlots = 0;
						}
					}
				}

				/// <summary>
				/// The NPID of the user to retrieve the variables for
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the <see cref="IsVirtualUser"/> is true.</exception>
				public Core.NpAccountId TargetUser
				{
					get
					{
						if (isVirtualUser == true)
						{
							throw new NpToolkitException("A virtual user id is currently configured on for this request, meaning the TargetUser is not valid");
						}
						return targetUser;
					}
					set
					{
						targetUser = value;
						isVirtualUser = false;
					}
				}

				/// <summary>
				/// Identifier of a virtual user. Only used if <see cref="IsVirtualUser"/> is set to true
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the <see cref="IsVirtualUser"/> is false.</exception>
				public VirtualUserID VirtualUserID
				{
					get
					{
						if (isVirtualUser == false)
						{
							throw new NpToolkitException("This request doesn't have a virtual user id current set.");
						}
						return virtualUserID;
					}
					set
					{
						virtualUserID = value;

						if (value.name.Length > 0) isVirtualUser = true;
						else isVirtualUser = false;
					}
				}

				/// <summary>
				/// A flag that specifies whether this update is for a virtual user. This is atomatically set depending on if <see cref="TargetUser"/> or <see cref="VirtualUserID"/> is set.
				/// </summary>
				public bool IsVirtualUser
				{
					get { return isVirtualUser; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="SetVariablesRequest"/> class.
				/// </summary>
				public GetVariablesRequest()
					: base(ServiceTypes.Tus, FunctionTypes.TusGetVariables)
				{

				}
			}

			/// <summary>
			/// Used to prevent data conflicts on the TUS server
			/// </summary>
			public struct DataContention
			{
				internal UInt64 lastChangedDateTicks;  // is stored in SceRtcTick format
				internal Core.NpAccountId requiredLastChangeUser;

				/// <summary>
				///  The date and time for conflict prevention. Processing is only executed when the time of the TUS data's last update, which is registered on the server, is identical with or older
				///  than the specified time. When no TUS data is registered on the server, no processing is performed. Specify 0 if no comparison is necessary.
				/// </summary>
				public DateTime LastChangedDate
				{
					get
					{
						return Core.RtcTicksToDateTime(lastChangedDateTicks);
					}
					set
					{
						lastChangedDateTicks = Core.DateTimeToRtcTicks(value);
					}
				}

				/// <summary>
				/// The account id of the updates author for conflict prevention. Processing is only executed when the author of the TUS data's last update, which is registered on the server, 
				/// is identical with the specified account id. When no TUS data is registered on the server, processing is not performed. Set this value to 0 if no comparison is necessary.
				/// </summary>
				public Core.NpAccountId RequiredLastChangeUser
				{
					get { return requiredLastChangeUser; }
					set { requiredLastChangeUser = value; }
				}
			}

			/// <summary>
			/// Parameters required for adding to a specified users TUS variable
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class AddToAndGetVariableRequest : RequestBase
			{
				internal Variable var;

				internal DataContention dataContention;

				internal Core.NpAccountId targetUser;

				internal VirtualUserID virtualUserID;

				[MarshalAs(UnmanagedType.I1)]
				internal bool isVirtualUser;

				[MarshalAs(UnmanagedType.I1)]
				internal bool forCrossSave;

				/// <summary>
				/// The TUS variable to update
				/// </summary>
				public Variable Var
				{
					get { return var; }
					set { var = value; }
				}

				/// <summary>
				/// Prevention of data contention
				/// </summary>
				public DataContention DataContention
				{
					get { return dataContention; }
					set { dataContention = value; }
				}

				/// <summary>
				/// The NPID of the user to retrieve the variables for
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the <see cref="IsVirtualUser"/> is true.</exception>
				public Core.NpAccountId TargetUser
				{
					get
					{
						if (isVirtualUser == true)
						{
							throw new NpToolkitException("A virtual user id is currently configured on for this request, meaning the TargetUser is not valid");
						}
						return targetUser;
					}
					set
					{
						targetUser = value;
						isVirtualUser = false;
					}
				}

				/// <summary>
				/// Identifier of a virtual user. Only used if <see cref="IsVirtualUser"/> is set to true
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the <see cref="IsVirtualUser"/> is false.</exception>
				public VirtualUserID VirtualUserID
				{
					get
					{
						if (isVirtualUser == false)
						{
							throw new NpToolkitException("This request doesn't have a virtual user id current set.");
						}
						return virtualUserID;
					}
					set
					{
						virtualUserID = value;

						if (value.name.Length > 0) isVirtualUser = true;
						else isVirtualUser = false;
					}
				}

				/// <summary>
				/// A flag that specifies whether this update is for a virtual user. This is atomatically set depending on if <see cref="TargetUser"/> or <see cref="VirtualUserID"/> is set.
				/// </summary>
				public bool IsVirtualUser
				{
					get { return isVirtualUser; }
				}

				/// <summary>
				/// For compatibility with older platforms, set this when performing cross save actions
				/// </summary>
				public bool ForCrossSave
				{
					get { return forCrossSave; }
					set { forCrossSave = value; }
				}		

				/// <summary>
				/// Initializes a new instance of the <see cref="AddToAndGetVariableRequest"/> class.
				/// </summary>
				public AddToAndGetVariableRequest()
					: base(ServiceTypes.Tus, FunctionTypes.TusAddToAndGetVariable)
				{
					dataContention.lastChangedDateTicks = 0;
					dataContention.requiredLastChangeUser = 0;
				}
			}

			/// <summary>
			/// Parameters required for setting a specified users TUS binary data
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class SetDataRequest : RequestBase
			{
				/// <summary>
				/// Maximum size of the <see cref="SupplementaryInfo"/> array.
				/// </summary>
				public const int NP_TUS_DATA_INFO_MAX_SIZE = 384;

				[MarshalAs(UnmanagedType.LPArray)]
				internal byte[] data;
				internal UInt64 dataSize;

				internal Core.NpAccountId targetUser;

				internal UInt64 supplementaryInfoSize;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = NP_TUS_DATA_INFO_MAX_SIZE)]
				internal byte[] supplementaryInfo = new byte[NP_TUS_DATA_INFO_MAX_SIZE];

				internal DataContention dataContention;

				internal Int32 slotId;

				internal VirtualUserID virtualUserID;

				[MarshalAs(UnmanagedType.I1)]
				internal bool isVirtualUser;

				/// <summary>
				/// The TUS data to update
				/// </summary>
				public byte[] Data
				{
					get { return data; }
					set 
					{
						data = value;
						dataSize = (value != null) ? (UInt64)value.Length : 0;
					}
				}

				/// <summary>
				/// Supplementary Information for the  TUS Data
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is larger than <see cref="NP_TUS_DATA_INFO_MAX_SIZE"/>.</exception>
				public byte[] SupplementaryInfo
				{
					get 
					{
						if (supplementaryInfoSize == 0) return null;

						byte[] output = new byte[supplementaryInfoSize];

						Array.Copy(supplementaryInfo, output, (int)supplementaryInfoSize);

						return output;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > NP_TUS_DATA_INFO_MAX_SIZE)
							{
								throw new NpToolkitException("The size of the array is larger than " + NP_TUS_DATA_INFO_MAX_SIZE);
							}

							value.CopyTo(supplementaryInfo, 0);
							supplementaryInfoSize = (UInt64)value.Length;
						}
						else
						{
							supplementaryInfoSize = 0;
						}
					}
				}

				/// <summary>
				/// The ID of the slot that the data belongs to
				/// </summary>
				public Int32 SlotId
				{
					get { return slotId; }
					set { slotId = value; }
				}

				/// <summary>
				/// Prevention of data contention
				/// </summary>
				public DataContention DataContention
				{
					get { return dataContention; }
					set { dataContention = value; }
				}

				/// <summary>
				/// The account ID of the user that is being updated
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the <see cref="IsVirtualUser"/> is true.</exception>
				public Core.NpAccountId TargetUser
				{
					get
					{
						if (isVirtualUser == true)
						{
							throw new NpToolkitException("A virtual user id is currently configured on for this request, meaning the TargetUser is not valid");
						}
						return targetUser;
					}
					set
					{
						targetUser = value;
						isVirtualUser = false;
					}
				}

				/// <summary>
				/// Identifier of a virtual user
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the <see cref="IsVirtualUser"/> is false.</exception>
				public VirtualUserID VirtualUserID
				{
					get
					{
						if (isVirtualUser == false)
						{
							throw new NpToolkitException("This request doesn't have a virtual user id current set.");
						}
						return virtualUserID;
					}
					set
					{
						virtualUserID = value;

						if (value.name.Length > 0) isVirtualUser = true;
						else isVirtualUser = false;
					}
				}

				/// <summary>
				/// A flag that specifies whether this request is for a virtual user. This is atomatically set depending on if <see cref="TargetUser"/> or <see cref="VirtualUserID"/> is set.
				/// </summary>
				public bool IsVirtualUser
				{
					get { return isVirtualUser; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="SetVariablesRequest"/> class.
				/// </summary>
				public SetDataRequest()
					: base(ServiceTypes.Tus, FunctionTypes.TusSetData)
				{

				}
			}

			/// <summary>
			/// Parameters required for getting a specified users TUS binary data
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class GetDataRequest : RequestBase
			{
				internal Core.NpAccountId targetUser;

				internal Int32 slotId;

				internal VirtualUserID virtualUserID;

				[MarshalAs(UnmanagedType.I1)]
				internal bool isVirtualUser;

				[MarshalAs(UnmanagedType.I1)]
				internal bool forCrossSave;

				[MarshalAs(UnmanagedType.I1)]
				internal bool retrieveStatusOnly;

				/// <summary>
				/// The ID of the slot that the data belongs to
				/// </summary>
				public Int32 SlotId
				{
					get { return slotId; }
					set { slotId = value; }
				}

				/// <summary>
				/// The account ID of the user to retrieve the data for
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the <see cref="IsVirtualUser"/> is true.</exception>
				public Core.NpAccountId TargetUser
				{
					get
					{
						if (isVirtualUser == true)
						{
							throw new NpToolkitException("A virtual user id is currently configured on for this request, meaning the TargetUser is not valid");
						}
						return targetUser;
					}
					set
					{
						targetUser = value;
						isVirtualUser = false;
					}
				}

				/// <summary>
				/// Identifier of a virtual user
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the <see cref="IsVirtualUser"/> is false.</exception>
				public VirtualUserID VirtualUserID
				{
					get
					{
						if (isVirtualUser == false)
						{
							throw new NpToolkitException("This request doesn't have a virtual user id current set.");
						}
						return virtualUserID;
					}
					set
					{
						virtualUserID = value;

						if (value.name.Length > 0) isVirtualUser = true;
						else isVirtualUser = false;
					}
				}

				/// <summary>
				/// A flag that specifies whether this request is for a virtual user. This is atomatically set depending on if <see cref="TargetUser"/> or <see cref="VirtualUserID"/> is set.
				/// </summary>
				public bool IsVirtualUser
				{
					get { return isVirtualUser; }
				}

				/// <summary>
				/// For compatibility with older platforms, set this when performing cross save actions
				/// </summary>
				public bool ForCrossSave
				{
					get { return forCrossSave; }
					set { forCrossSave = value; }
				}		

				/// <summary>
				/// If set to true, only the data size in status is retrieved. The data buffer will be empty
				/// </summary>
				public bool RetrieveStatusOnly
				{
					get { return retrieveStatusOnly; }
					set { retrieveStatusOnly = value; }
				}		

				/// <summary>
				/// Initializes a new instance of the <see cref="SetVariablesRequest"/> class.
				/// </summary>
				public GetDataRequest()
					: base(ServiceTypes.Tus, FunctionTypes.TusGetData)
				{

				}
			}

			/// <summary>
			/// Parameters required for deleting a specified users TUS binary data
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class DeleteDataRequest : RequestBase
			{
				/// <summary>
				/// Maximum size of the <see cref="SlotIds"/> array.
				/// </summary>
				public const int MAX_DATA_SLOTS = 16;

				internal UInt64 numSlots;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_DATA_SLOTS)]
				Int32[] slotIds = new Int32[MAX_DATA_SLOTS];

				internal Core.NpAccountId targetUser;

				internal VirtualUserID virtualUserID;

				[MarshalAs(UnmanagedType.I1)]
				internal bool isVirtualUser;

				/// <summary>
				/// The IDs of the slots that you want to delete
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is larger than <see cref="MAX_DATA_SLOTS"/>.</exception>
				public Int32[] SlotIds
				{
					get
					{
						if (numSlots == 0) return null;

						Int32[] output = new Int32[numSlots];

						Array.Copy(slotIds, output, (int)numSlots);

						return output;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_DATA_SLOTS)
							{
								throw new NpToolkitException("The size of the array is larger than " + MAX_DATA_SLOTS);
							}

							value.CopyTo(slotIds, 0);
							numSlots = (UInt64)value.Length;
						}
						else
						{
							numSlots = 0;
						}
					}
				}

				/// <summary>
				/// The account ID of the user data to delete
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the <see cref="IsVirtualUser"/> is true.</exception>
				public Core.NpAccountId TargetUser
				{
					get
					{
						if (isVirtualUser == true)
						{
							throw new NpToolkitException("A virtual user id is currently configured on for this request, meaning the TargetUser is not valid");
						}
						return targetUser;
					}
					set
					{
						targetUser = value;
						isVirtualUser = false;
					}
				}

				/// <summary>
				/// Identifier of a virtual user
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the <see cref="IsVirtualUser"/> is false.</exception>
				public VirtualUserID VirtualUserID
				{
					get
					{
						if (isVirtualUser == false)
						{
							throw new NpToolkitException("This request doesn't have a virtual user id current set.");
						}
						return virtualUserID;
					}
					set
					{
						virtualUserID = value;

						if (value.name.Length > 0) isVirtualUser = true;
						else isVirtualUser = false;
					}
				}

				/// <summary>
				/// A flag that specifies whether this update is for a virtual user. This is atomatically set depending on if <see cref="TargetUser"/> or <see cref="VirtualUserID"/> is set.
				/// </summary>
				public bool IsVirtualUser
				{
					get { return isVirtualUser; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="DeleteDataRequest"/> class.
				/// </summary>
				public DeleteDataRequest()
					: base(ServiceTypes.Tus, FunctionTypes.TusDeleteData)
				{

				}
			}

			#endregion

			#region Set Variables

			/// <summary>
			/// Sets Title User Storage (TUS) variables for a given user
			/// </summary>
			/// <param name="request">The request parameters required to set a users TUS variables</param>
			/// <param name="response">This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int SetVariables(SetVariablesRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxTusSetVariables(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Get Variables		

			/// <summary>
			/// TUS variables that were returned from the TUS server
			/// </summary>
			public class VariablesResponse : ResponseBase
			{
				internal bool forCrossSave;

				internal NpVariable[] vars;
				internal NpVariableForCrossSave[] varsForCrossSave;

				/// <summary>
				/// Specifies if the variables returned are for cross save
				/// </summary>
				public bool ForCrossSave
				{
					get { return forCrossSave; }
				}

				/// <summary>
				/// TUS variables
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if <see cref="ForCrossSave"/> isn't set to false.</exception>
				public NpVariable[] Vars
				{
					get 
					{ 
						if (forCrossSave == true)
						{
							throw new NpToolkitException("Vars isn't valid unless 'ForCrossSave' is set to false.");
						}

						return vars; 
					}
				}

				/// <summary>
				/// TUS variables for cross save
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if <see cref="ForCrossSave"/> isn't set to true.</exception>
				public NpVariableForCrossSave[] VarsForCrossSave
				{
					get 
					{
						if (forCrossSave == false)
						{
							throw new NpToolkitException("VarsForCrossSave isn't valid unless 'ForCrossSave' is set to true.");
						}
						return varsForCrossSave; 
					}
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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.TusVariablesBegin);

					Int64 numVars = readBuffer.ReadInt64();
					forCrossSave = readBuffer.ReadBool();

					if (forCrossSave == true)
					{
						varsForCrossSave = new NpVariableForCrossSave[numVars];
					}
					else
					{
						vars = new NpVariable[numVars];
					}

					for (int i = 0; i < numVars; i++)
					{
						if (forCrossSave == true)
						{
							varsForCrossSave[i] = new NpVariableForCrossSave();
							varsForCrossSave[i].Read(readBuffer);
						}
						else
						{
							vars[i] = new NpVariable();
							vars[i].Read(readBuffer);
						}
					}

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.TusVariablesEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// Get a specified users TUS variables from specified slots
			/// </summary>
			/// <param name="request">The parameters required to retrieve TUS variables </param>
			/// <param name="response">The requested TUS variables that will be set upon successful completion</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetVariables(GetVariablesRequest request, VariablesResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxTusGetVariables(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Add To And Get Variable

			/// <summary>
			/// Contains the result of the add and get TUS variable operation
			/// </summary>
			public class AtomicAddToAndGetVariableResponse : ResponseBase
			{
				internal bool forCrossSave;

				internal NpVariable var;
				internal NpVariableForCrossSave varForCrossSave;

				/// <summary>
				/// Specifies if the variable returned is for cross save
				/// </summary>
				public bool ForCrossSave
				{
					get { return forCrossSave; }
				}

				/// <summary>
				/// TUS variable
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if <see cref="ForCrossSave"/> isn't set to false.</exception>
				public NpVariable Var
				{
					get
					{
						if (forCrossSave == true)
						{
							throw new NpToolkitException("Vars isn't valid unless 'ForCrossSave' is set to false.");
						}

						return var;
					}
				}

				/// <summary>
				/// TUS variable for cross save
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if <see cref="ForCrossSave"/> isn't set to true.</exception>
				public NpVariableForCrossSave VarForCrossSave
				{
					get
					{
						if (forCrossSave == false)
						{
							throw new NpToolkitException("VarsForCrossSave isn't valid unless 'ForCrossSave' is set to true.");
						}
						return varForCrossSave;
					}
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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.TusAtomicAddToAndGetVariableBegin);

					forCrossSave = readBuffer.ReadBool();

					if (forCrossSave == true)
					{
						varForCrossSave = new NpVariableForCrossSave();
						varForCrossSave.Read(readBuffer);
					}
					else
					{
						var = new NpVariable();
						var.Read(readBuffer);
					}

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.TusAtomicAddToAndGetVariableEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// Adds and gets a users TUS variable in a single operation
			/// </summary>
			/// <param name="request">The parameters specifying the users slot and value to add to </param>
			/// <param name="response"> Upon successful completion, this will contain the new value of the TUS variable</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int AddToAndGetVariable(AddToAndGetVariableRequest request, AtomicAddToAndGetVariableResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxTusAddToAndGetVariable(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Set Data

			/// <summary>
			/// Sets a specified users TUS binary data
			/// </summary>
			/// <param name="request">The parameters required for setting a users TUS binary data</param>
			/// <param name="response">This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int SetData(SetDataRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxTusSetData(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Get Data

			/// <summary>
			/// TUS data that was returned from the TUS server
			/// </summary>
			public class GetDataResponse : ResponseBase
			{
				internal bool forCrossSave;

				internal NpTusDataStatus status;
				internal NpTusDataStatusForCrossSave statusForCrossSave;

				internal byte[] data;

				/// <summary>
				/// The TUS data
				/// </summary>
				public byte[] Data
				{
					get { return data; }
				}

				/// <summary>
				/// Specifies if the data returned is for cross save
				/// </summary>
				public bool ForCrossSave
				{
					get { return forCrossSave; }
				}

				/// <summary>
				/// The status of the data.
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if <see cref="ForCrossSave"/> isn't set to false.</exception>
				public NpTusDataStatus Status
				{
					get
					{
						if (forCrossSave == true)
						{
							throw new NpToolkitException("Vars isn't valid unless 'ForCrossSave' is set to false.");
						}

						return status;
					}
				}

				/// <summary>
				/// The status of the data when cross save us used
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if <see cref="ForCrossSave"/> isn't set to true.</exception>
				public NpTusDataStatusForCrossSave StatusForCrossSave
				{
					get
					{
						if (forCrossSave == false)
						{
							throw new NpToolkitException("VarsForCrossSave isn't valid unless 'ForCrossSave' is set to true.");
						}
						return statusForCrossSave;
					}
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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.TusDataBegin);

					readBuffer.ReadData(ref data);

					forCrossSave = readBuffer.ReadBool();

					if (forCrossSave == true)
					{
						statusForCrossSave = new NpTusDataStatusForCrossSave();
						statusForCrossSave.Read(readBuffer);
					}
					else
					{
						status = new NpTusDataStatus();
						status.Read(readBuffer);
					}

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.TusDataEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}


			/// <summary>
			/// Gets a specified user's TUS binary data
			/// </summary>
			/// <param name="request">The parameters required to retrieve a users TUS binary data</param>
			/// <param name="response">On successful completion, this will contain the requested users binary data</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetData(GetDataRequest request, GetDataResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxTusGetData(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Delete Data

			/// <summary>
			/// Deletes data from specified slots
			/// </summary>
			/// <param name="request">The required parameters for deleting a users TUS data</param>
			/// <param name="response">This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int DeleteData(DeleteDataRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxTusDeleteData(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion
		}
	}
}
