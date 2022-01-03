using System;
using System.Runtime.InteropServices;

namespace Sony
{
	namespace NP
	{
		/// <summary>
		/// Ranking service related functionality.
		/// </summary>
		public class Ranking
		{
			#region DLL Imports

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxSetScore(SetScoreRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetRangeOfRanks(GetRangeOfRanksRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetFriendsRanks(GetFriendsRanksRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetUsersRanks(GetUsersRanksRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxSetGameData(SetGameDataRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetGameData(GetGameDataRequest request, out APIResult result);

			#endregion

			/// <summary>
			/// The maximum number of boards that an application can have
			/// </summary>
			public const int MAX_NUM_BOARDS = 1000; 

			/// <summary>
			/// The minimum, and default, player character Id
			/// </summary>
			public const int MIN_PCID = 0;			

			/// <summary>
			/// The maximum player character Id that can be set
			/// </summary>
			public const int MAX_PCID = 9;	
		
			/// <summary>
			/// The maximum range of ranks that can be requested in one call
			/// </summary>
			public const int MAX_RANGE = 100;   // MAX_RANGE = SCE_NP_SCORE_MAX_RANGE_NUM_PER_REQUEST

			/// <summary>
			/// The minimum range of ranks that can be requested in one call
			/// </summary>
			public const int MIN_RANGE = 1;

			/// <summary>
			/// The first rank that can be requested. Please note that rank 0 does not exist   
			/// </summary>
			public const int FIRST_RANK = 1;

			#region Requests

			/// <summary>
			/// Request object to set a new score on a board for the calling user.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class SetScoreRequest : RequestBase
			{
				/// <summary>
				/// The maximum length of the UTF8 comment string
				/// </summary>
				public const int NP_SCORE_COMMENT_MAXLEN = 63;   // SCE_NP_SCORE_COMMENT_MAXLEN

				/// <summary>
				/// The maximum number of bytes in the <see cref="GameInfoData"/> array.
				/// </summary>
				public const int NP_SCORE_GAMEINFO_MAXSIZE = 189;   // SCE_NP_SCORE_GAMEINFO_MAXSIZE

				[MarshalAs(UnmanagedType.I8)]
				internal Int64 score;			

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = NP_SCORE_COMMENT_MAXLEN+1)]
				internal string utf8Comment;                                        

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = NP_SCORE_GAMEINFO_MAXSIZE)]
				internal byte[] gameInfoData;
													
				[MarshalAs(UnmanagedType.U8)]
				internal UInt64 dataLength;

				[MarshalAs(UnmanagedType.U4)]
				internal UInt32 boardId;
										
				[MarshalAs(UnmanagedType.I4)]
				internal Int32 pcId; 

				/// <summary>
				/// The score to register
				/// </summary>
				public Int64 Score
				{
					get { return score; }
					set { score = value; }
				}

				/// <summary>
				/// Optional. A string comment to be registered along with the score
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the string is more than <see cref="NP_SCORE_COMMENT_MAXLEN"/> characters.</exception>
				public string Comment 
				{ 
					get { return utf8Comment; }
					set
					{
						if (value.Length > NP_SCORE_COMMENT_MAXLEN)
						{
							throw new NpToolkitException("The size of the comment string is more than " + NP_SCORE_COMMENT_MAXLEN + " characters.");
						}
						utf8Comment = value;
					}
				}

				/// <summary>
				/// Optional. Game information data that can be saved per score
				/// </summary>
				/// <remarks>
				/// Takes a copy of the data or returns a copy. The binary data must be assign explicitly. Changing the array set or returned by this property won't change the stored data in this instance.
				/// </remarks>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is more than <see cref="NP_SCORE_GAMEINFO_MAXSIZE"/> characters.</exception>
				public byte[] GameInfoData
				{
					get
					{
						if (dataLength == 0) return null;

						byte[] infoData = new byte[dataLength];

						Array.Copy(gameInfoData, infoData, (int)dataLength);

						return infoData;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > NP_SCORE_GAMEINFO_MAXSIZE)
							{
								throw new NpToolkitException("The size of the game data is more than " + NP_SCORE_GAMEINFO_MAXSIZE + " bytes.");
							}

							value.CopyTo(gameInfoData, 0);

							dataLength = (UInt32)value.Length;
						}
						else
						{
							dataLength = 0;
						}
					}
				}

				/// <summary>
				/// The board Id of the board where the score will be registered
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if it is more than <see cref="MAX_NUM_BOARDS"/>.</exception>
				public UInt32 BoardId 
				{ 
					get { return boardId; }
					set
					{
						if (value > MAX_NUM_BOARDS)
						{
							throw new NpToolkitException("The BoardId can't be more than " + MAX_NUM_BOARDS);
						}
						boardId = value;
					} 
				}

				/// <summary>
				/// Optional. The player character Id, in case it is different than 0           
				/// </summary>
				public Int32 PcId
				{
					get { return pcId; }
					set { pcId = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="SetScoreRequest"/> class.
				/// </summary>
				public SetScoreRequest()
					: base(ServiceTypes.Ranking, FunctionTypes.RankingSetScore)
				{
					gameInfoData = new byte[NP_SCORE_GAMEINFO_MAXSIZE];
				}
			}

			/// <summary>
			/// Request object to get a range of ranks from a board (if any) starting from a specified rank.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class  GetRangeOfRanksRequest : RequestBase
			{
				[MarshalAs(UnmanagedType.U4)]
				internal UInt32 boardId;

				[MarshalAs(UnmanagedType.U4)]
				internal UInt32 startRank;

				[MarshalAs(UnmanagedType.U4)]
				internal UInt32 range;

				[MarshalAs(UnmanagedType.I1)]
				internal bool isCrossSaveInformation;

				/// <summary>
				/// The board Id of the board where the range of ranks will be obtained from     
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if it is more than <see cref="MAX_NUM_BOARDS"/>.</exception>
				public UInt32 BoardId
				{
					get { return boardId; }
					set 
					{
						if (value > MAX_NUM_BOARDS)
						{
							throw new NpToolkitException("The BoardId can't be more than " + MAX_NUM_BOARDS);
						}

						boardId = value; 
					}
				}

				/// <summary>
				/// The first rank that will be obtained in the range
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if it is less than <see cref="FIRST_RANK"/>.</exception>
				public UInt32 StartRank
				{
					get { return startRank; }
					set 
					{
						if (value < FIRST_RANK)
						{
							throw new NpToolkitException("The StartRank can't be less than " + FIRST_RANK);
						}
						
						startRank = value; 
					}
				}

				/// <summary>
				/// The number of ranks that will be obtained, starting from the <see cref="StartRank"/> 
				/// </summary>
				/// /// <exception cref="NpToolkitException">Will throw an exception if it is less than <see cref="MIN_RANGE"/> or greater than <see cref="MAX_RANGE"/>.</exception>
				public UInt32 Range
				{
					get { return range; }
					set 
					{
						if (value < MIN_RANGE || value > MAX_RANGE)
						{
							throw new NpToolkitException("The Range must be between " + MIN_RANGE + " and " + MAX_RANGE);
						}

						range = value; 
					}
				}

				/// <summary>
				/// Optional. Set it to true if the board is shared with PS Vita or PS3 applications 
				/// </summary>
				public bool IsCrossSaveInformation
				{
					get { return isCrossSaveInformation; }
					set { isCrossSaveInformation = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetRangeOfRanksRequest"/> class.
				/// </summary>
				public GetRangeOfRanksRequest()
					: base(ServiceTypes.Ranking, FunctionTypes.RankingGetRangeOfRanks)
				{
					IsCrossSaveInformation = false;
				}
			}

			/// <summary>
			/// Request object to get the ranks of the friends for the calling user (if any) starting from a specified rank.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class GetFriendsRanksRequest : RequestBase
			{
				[MarshalAs(UnmanagedType.U4)]
				internal UInt32 boardId;

				[MarshalAs(UnmanagedType.U4)]
				internal UInt32 startRank;

				[MarshalAs(UnmanagedType.U4)]
				internal UInt32 friendsWithPcId;

				[MarshalAs(UnmanagedType.I1)]
				internal bool isCrossSaveInformation;

				[MarshalAs(UnmanagedType.I1)]
				internal bool addCallingUserRank;

				/// <summary>
				/// The board Id of the board where the friends ranks will be obtained from 
				/// </summary>
				public UInt32 BoardId
				{
					get { return boardId; }
					set { boardId = value; }
				}

				/// <summary>
				/// Optional. In case pagination is needed, specify the last rank obtained in the previous call. Otherwise, leave blank
				/// </summary>
				public UInt32 StartRank
				{
					get { return startRank; }
					set { startRank = value; }
				}

				/// <summary>
				/// Optional. In case the pc Id to be retrieved is different than the default one 0, specify the pc Id to retrieve
				/// </summary>
				public UInt32 FriendsWithPcId
				{
					get { return friendsWithPcId; }
					set { friendsWithPcId = value; }
				}

				/// <summary>
				/// Optional. Set it to true if the board is shared with PS Vita or PS3 applications 
				/// </summary>
				public bool IsCrossSaveInformation
				{
					get { return isCrossSaveInformation; }
					set { isCrossSaveInformation = value; }
				}

				/// <summary>
				/// True by default. It also returns the calling user rank, along with the ranks of the user' friends 
				/// </summary>
				public bool AddCallingUserRank
				{
					get { return addCallingUserRank; }
					set { addCallingUserRank = value; }
				}


				/// <summary>
				/// Initializes a new instance of the <see cref="GetRangeOfRanksRequest"/> class.
				/// </summary>
				public GetFriendsRanksRequest()
					: base(ServiceTypes.Ranking, FunctionTypes.RankingGetFriendsRanks)
				{
					IsCrossSaveInformation = false;
					addCallingUserRank = true;
				}
			}

			/// <summary>
			/// Account ID structure with player character ID
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public struct ScoreAccountIdPcId
			{
				/// <summary>
				/// Account ID
				/// </summary>
				public Core.NpAccountId accountId;

				/// <summary>
				/// Player character ID
				/// </summary>
				public Int32 pcId;
			};

			/// <summary>
			/// Request object to get ranks from a board for specific users (if any rank is found).
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class GetUsersRanksRequest : RequestBase
			{
				/// <summary>
				/// The maximum number of users that can be requested in one call
				/// </summary>
				public const int MAX_NUM_USERS = 101;  // SCE_NP_SCORE_MAX_ID_NUM_PER_REQUEST

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_NUM_USERS)]
				internal ScoreAccountIdPcId[] users;

				[MarshalAs(UnmanagedType.U4)]
				internal UInt32 numUsers;

				[MarshalAs(UnmanagedType.U4)]
				internal UInt32 boardId;

				[MarshalAs(UnmanagedType.I1)]
				internal bool isCrossSaveInformation;

				[MarshalAs(UnmanagedType.I1)]
				internal bool ignorePcIds;

				/// <summary>
				/// The array identifying the users whom ranks want to be obtained. If pc ids should be looked into, set <see cref="IgnorePcIds"/> to false. Otherwise, do not set pcIds
				/// The array size must not exceed <see cref="MAX_NUM_USERS"/>.
				/// </summary>
				/// <remarks>
				/// Takes a copy of the array or returns a copy. The ids must be assign explicitly. Changing the array set or returned by this property won't change the stored data in this instance.
				/// </remarks>
				/// <exception cref="NpToolkitException">Will throw an exception if the array is more than <see cref="MAX_NUM_USERS"/>.</exception>
				/// <exception cref="NpToolkitException">Will throw an exception if the PcIds are outside the range of <see cref="MIN_PCID"/> and <see cref="MAX_PCID"/>.</exception>
				public ScoreAccountIdPcId[] Users
				{
					get
					{
						if (numUsers == 0) return null;

						ScoreAccountIdPcId[] ids = new ScoreAccountIdPcId[numUsers];

						Array.Copy(users, ids, numUsers);

						return ids;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_NUM_USERS)
							{
								throw new NpToolkitException("The size of the Users array is more than " + MAX_NUM_USERS);
							}

							if (IgnorePcIds == false)
							{
								for (int i = 0; i < value.Length; i++)
								{
									if (value[i].pcId < MIN_PCID || value[i].pcId > MAX_PCID)
									{
										throw new NpToolkitException("The pcId in Users[" + i + "] is outside the range of MIN_PCID/MAX_PCID");
									}
								}
							}

							value.CopyTo(users, 0);
							numUsers = (UInt32)value.Length;
						}
						else
						{
							numUsers = 0;
						}
					}
				}

				/// <summary>
				/// The board Id of the board where the users ranks will be obtained from
				/// </summary>
				public UInt32 BoardId
				{
					get { return boardId; }
					set { boardId = value; }
				}

				/// <summary>
				/// Optional. Set it to true if the board is shared with PS Vita or PS3 applications     
				/// </summary>
				public bool IsCrossSaveInformation
				{
					get { return isCrossSaveInformation; }
					set { isCrossSaveInformation = value; }
				}

				/// <summary>
				/// Optional. When set to true, the pc Ids specified in the <see cref="Users"/> array should be ignored 
				/// </summary>
				public bool IgnorePcIds
				{
					get { return ignorePcIds; }
					set { ignorePcIds = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetRangeOfRanksRequest"/> class.
				/// </summary>
				public GetUsersRanksRequest()
					: base(ServiceTypes.Ranking, FunctionTypes.RankingGetUsersRanks)
				{
					users = new ScoreAccountIdPcId[MAX_NUM_USERS];
					IsCrossSaveInformation = false;
					ignorePcIds = true;
				}
			}

			/// <summary>
			/// Request object to set game data for an already registered entry on a board for the calling user.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class SetGameDataRequest : RequestBase
			{
				internal UInt32 boardId;
				internal Int32 idOfPrevChunk;
				internal Int64 score;
				internal UInt64 totalSize;			
												
				[MarshalAs(UnmanagedType.LPArray)]
				internal byte[] data;

				internal UInt64 byteOffset; 
				internal UInt64 chunkDataSize;
				internal Int32 pcId;

				/// <summary>
				/// The board Id of the board where the game data will be set
				/// </summary>
				public UInt32 BoardId
				{
					get { return boardId; }
					set { boardId = value; }
				}

				/// <summary>
				/// Set it when sending game data in chunks, after the first chunk has been sent. The value should be the value received in <see cref="SetGameDataResultResponse.ChunkId"/> in the response of the previous request.
				/// </summary>
				public Int32 IdOfPrevChunk
				{
					get { return idOfPrevChunk; }
					set { idOfPrevChunk = value; }
				}

				/// <summary>
				/// The score to save the game data that is being send. This score must belong to the calling user (only the calling user game data can be set)
				/// </summary>
				public Int64 Score
				{
					get { return score; }
					set { score = value; }
				}

				/// <summary>
				/// Get the total size of the entire game data. All chunks included. Set by <see cref="SetDataChunk(byte[], ulong, ulong)"/>.
				/// </summary>
				public UInt64 TotalSize
				{
					get { return totalSize; }
				}

				/// <summary>
				/// Get the game data set by <see cref="SetDataChunk(byte[], ulong, ulong)"/>. 
				/// </summary>
				public byte[] Data
				{
					get { return data; }
				}

				/// <summary>
				/// Get the start index for the <see cref="Data"/> array where the chunk of data will start.
				/// </summary>
				public UInt64 StartIndex
				{
					get { return byteOffset; }
				}

				/// <summary>
				/// Optional. The player character Id, in case it is different than 0    
				/// </summary>
				public Int32 PcId
				{
					get { return pcId; }
					set { pcId = value; }
				}

				/// <summary>
				/// Set the Chunk of data to upload to the the Ranking board.
				/// </summary>
				/// <param name="data">The array contain the data.</param>
				/// <param name="startIndex">An offset into the array to start writing the data.</param>
				/// <param name="chunkSize">The total number of bytes to write.</param>
				public void SetDataChunk(byte[] data, UInt64 startIndex, UInt64 chunkSize)
				{
					SetDataChunk(data, startIndex, chunkSize, 0);
				}

				/// <summary>
				/// Set the Chunk of data to upload to the the Ranking board.
				/// </summary>
				/// <param name="data">The array contain the data.</param>
				/// <param name="startIndex">An offset into the array to start writing the data.</param>
				/// <param name="chunkSize">The total number of bytes to write.</param>
				/// <param name="totalSize">The total size of the entire game data. All chunks included.</param>
				public void SetDataChunk(byte[] data, UInt64 startIndex, UInt64 chunkSize, UInt64 totalSize)
				{
					// Validate if the startIndex + chunkSize, is past the end of the array and
					if (startIndex + chunkSize > (UInt64)data.Length)
					{
						throw new NpToolkitException("The start Index and chunk size go off the end of the data array.");
					}

					this.data = data;
					this.byteOffset = startIndex;

					this.chunkDataSize = chunkSize;

					if (totalSize == 0)
					{
						this.totalSize = (UInt64)data.Length;
					}
					else
					{
						this.totalSize = totalSize;
					}
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="SetGameDataRequest"/> class.
				/// </summary>
				public SetGameDataRequest()
					: base(ServiceTypes.Ranking, FunctionTypes.RankingSetGameData)
				{

				}
			}

			/// <summary>
			/// Request object to get game data for an already registered entry on a board for a specified user.
			/// </summary>
			[StructLayout(LayoutKind.Sequential)]
			public class GetGameDataRequest : RequestBase
			{
				internal UInt32 boardId;
				internal Int32 idOfPrevChunk;
				internal Core.NpAccountId accountId;
				internal byte[] rcvData;
				internal UInt64 byteOffset;
				internal UInt64 chunkToRcvDataSize;
				internal Int32 pcId;

				/// <summary>
				/// The board Id of the board where the game data will be get from
				/// </summary>
				public UInt32 BoardId
				{
					get { return boardId; }
					set { boardId = value; }
				}

				/// <summary>
				/// Set it when getting game data in chunks, after the first chunk has been received. The value should be the value received in <see cref="GetGameDataResultResponse.ChunkId"/> in the response of the previous request
				/// </summary>
				public Int32 IdOfPrevChunk
				{
					get { return idOfPrevChunk; }
					set { idOfPrevChunk = value; }
				}

				/// <summary>
				/// The account Id of the user to get the game data from
				/// </summary>
				public Core.NpAccountId AccountId
				{
					get { return accountId; }
					set { accountId = value; }
				}

				/// <summary>
				/// Gets the array to received the game data requested. Set in <see cref="SetRcvDataChunk"/>.
				/// </summary>
				public byte[] RcvData
				{
					get { return rcvData; }
				}

				/// <summary>
				/// Optional. The player character Id, in case it is different than 0
				/// </summary>
				public Int32 PcId
				{
					get { return pcId; }
					set { pcId = value; }
				}

				/// <summary>
				/// 
				/// </summary>
				/// <param name="data"></param>
				/// <param name="startIndex"></param>
				/// <param name="chunkSize"></param>
				public void SetRcvDataChunk(byte[] data, UInt64 startIndex, UInt64 chunkSize)
				{
					// Validate if the startIndex + chunkSize, is past the end of the array and
					if (startIndex + chunkSize > (UInt64)data.Length)
					{
						throw new NpToolkitException("The start Index and chunk size go off the end of the data array.");
					}

					this.rcvData = data;
					this.byteOffset = startIndex;

					this.chunkToRcvDataSize = chunkSize;
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetGameDataRequest"/> class.
				/// </summary>
				public GetGameDataRequest()
					: base(ServiceTypes.Ranking, FunctionTypes.RankingGetGameData)
				{

				}
			}

			#endregion

			#region Set Score

			/// <summary>
			/// Response data containing an approximated rank of the calling user when a score is registered from a call to <see cref="SetScore"/>
			/// </summary>
			public class TempRankResponse : ResponseBase
			{
				internal UInt32 tempRank;

				/// <summary>
				/// Temporary rank given by the server when a score is just registered. It may be a bit innacurate to the final result, but it is a quick and safe approximation
				/// </summary>
				public UInt32 TempRank
				{
					get { return tempRank; }
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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.TempRankBegin);

					tempRank = readBuffer.ReadUInt32();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.TempRankEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function registers a new score for the calling user on the server (in the board specified) and provides a temporary rank.
			/// </summary>
			/// <param name="request">The parameters needed to register a score on the Ranking server </param>
			/// <param name="response">This response contains the return code  and an array with the temporary rank </param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int SetScore(SetScoreRequest request, TempRankResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxSetScore(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}
			#endregion

			#region Common structures

			/// <summary>
			/// Common data members for score rank data
			/// </summary>
			public class ScoreRankDataBase
			{
				internal string utf8Comment;
				internal byte[] gameInfo;

				internal Int32 pcId;
				internal UInt32 serialRank;
				internal UInt32 rank;
				internal UInt32 highestRank;
				internal bool hasGameData;
				internal Int64 scoreValue;
				internal DateTime recordDate;
				internal Core.NpAccountId accountId;

				/// <summary>
				///  Comments on the board for the user
				/// </summary>
				public string Comment { get { return utf8Comment; } }

				/// <summary>
				/// Game information on the board for the user.
				/// </summary>
				public byte[] GameInfo { get { return gameInfo; } }

				/// <summary>
				/// Player character ID of the player registering the score. 0 is set for a score registered without the player character ID set.
				/// </summary>
				public Int32 PcId { get { return pcId; } }

				/// <summary>
				/// Current rank. For same scores, the first one registered to the server will rank higher.
				/// </summary>
				public UInt32 SerialRank { get { return serialRank; } }

				/// <summary>
				/// Current rank. For same scores, scores will be ranked equally.
				/// </summary>
				public UInt32 Rank { get { return rank; } }

				/// <summary>
				/// Highest rank achieved by the player registering the score. rank value will be used.
				/// </summary>
				public UInt32 HighestRank { get { return highestRank; } }

				/// <summary>
				/// Flag to indicate whether score has game data attached or not. Only players having high ranks can register game data.
				/// </summary>
				public bool HasGameData { get { return hasGameData; } }

				/// <summary>
				/// Score value
				/// </summary>
				public Int64 ScoreValue { get { return scoreValue; } }

				/// <summary>
				/// Time at which this score was registered (processed by the ranking server)
				/// </summary>
				public DateTime RecordDate { get { return recordDate; } }

				/// <summary>
				/// Account ID of the player registering the score
				/// </summary>
				public Core.NpAccountId AccountId { get { return accountId; } }

				// Read data from PRX marshaled buffer
				internal void ReadBase(MemoryBuffer buffer)
				{
					pcId = buffer.ReadInt32();
					serialRank = buffer.ReadUInt32();
					rank = buffer.ReadUInt32();
					highestRank = buffer.ReadUInt32();
					hasGameData = buffer.ReadBool();
					scoreValue = buffer.ReadInt64();

					recordDate = Core.ReadRtcTick(buffer);
					accountId.Read(buffer);
				}

				internal void ReadAdditionalData(MemoryBuffer buffer)
				{
					buffer.ReadString(ref utf8Comment);
					buffer.ReadData(ref gameInfo);
				}
			}

			/// <summary>
			/// Ranking information
			/// </summary>
			public class ScoreRankData : ScoreRankDataBase  // Maps to SceNpScoreRankDataA
			{
				internal Core.OnlineID onlineId;

				/// <summary>
				/// Online ID of the player registering the score
				/// </summary>
				public Core.OnlineID OnlineId { get { return onlineId; } }

				// Read data from PRX marshaled buffer
				internal void ReadData(MemoryBuffer buffer)
				{
					ReadBase(buffer);

					onlineId = new Core.OnlineID();
					onlineId.Read(buffer);
				}
			}

			/// <summary>
			/// Ranking information for Cross-Save
			/// </summary>
			public class ScoreRankDataForCrossSave : ScoreRankDataBase  // Maps to SceNpScoreRankDataForCrossSave
			{
				internal Core.NpId npId;

				/// <summary>
				/// Online ID of the player registering the score
				/// </summary>
				public Core.NpId NpId { get { return npId; } }

				// Read data from PRX marshaled buffer
				internal void ReadData(MemoryBuffer buffer)
				{
					ReadBase(buffer);

					npId.Read(buffer);
				}
			}

			/// <summary>
			/// Ranking information of a specific player
			/// </summary>
			public class ScorePlayerRankData : ScoreRankData  // Maps to SceNpScorePlayerRankDataA
			{
				internal bool hasData;

				/// <summary>
				/// Indicating whether target player has rank registered or not. 
				/// If false all other data in this instance isn't valid.
				/// </summary>
				public bool HasData { get { return hasData; } }

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					hasData = buffer.ReadBool();

					if (hasData == true)
					{
						ReadData(buffer);
					}	
				}
			}

			/// <summary>
			/// Ranking information of a specific player for Cross-Save
			/// </summary>
			public class ScorePlayerRankDataForCrossSave : ScoreRankDataForCrossSave  // Maps to SceNpScorePlayerRankDataForCrossSave
			{
				internal bool hasData;

				/// <summary>
				/// Indicating whether target player has rank registered or not. 
				/// If false all other data in this instance isn't valid.
				/// </summary>
				public bool HasData { get { return hasData; } }

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					hasData = buffer.ReadBool();

					if (hasData == true)
					{
						ReadData(buffer);
					}
				}
			}

			#endregion

			#region Get Range Of Ranks

			/// <summary>
			/// Response data containing the ranks, comments and game information of the range requested (if any) from a call to <see cref="SetScore"/>
			/// </summary>
			public class RangeOfRanksResponse : ResponseBase
			{
				internal ScoreRankData[] scoreRankData;
				internal ScoreRankDataForCrossSave[] scoreRankDataForCrossSave;

				internal bool isCrossSaveInformation;
				internal UInt64 numValidEntries;
				internal DateTime updateTime;
				internal UInt32 totalEntriesOnBoard;
				internal UInt32 boardId;
				internal Int32 startRank;

				/// <summary>
				/// Rank data information for the users on the board in case <see cref="IsCrossSaveInformation"/> was not set in the request   
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if <see cref="IsCrossSaveInformation"/> isn't set to false.</exception>
				public ScoreRankData[] RankData 
				{ 
					get 
					{
						if (isCrossSaveInformation != false)
						{
							throw new NpToolkitException("RankData isn't valid unless 'IsCrossSaveInformation' is set to false.");
						}

						return scoreRankData; 
					} 
				}

				/// <summary>
				/// Rank data information for the users on the board in case <see cref="IsCrossSaveInformation"/> was set to true in the request 
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if <see cref="IsCrossSaveInformation"/> isn't set to true.</exception>
				public ScoreRankDataForCrossSave[] RankDataForCrossSave 
				{ 
					get 
					{
						if (isCrossSaveInformation != true)
						{
							throw new NpToolkitException("RankDataForCrossSave isn't valid unless 'IsCrossSaveInformation' is set to true.");
						}

						return scoreRankDataForCrossSave; 
					} 
				}

				/// <summary>
				/// It is true if the information requested was from a shared board between PS Vita or PS3        
				/// </summary>
				public bool IsCrossSaveInformation { get { return isCrossSaveInformation; } }

				/// <summary>
				/// The time when the server created the ranking information (UTC)  
				/// </summary>
				public DateTime UpdateTime { get { return updateTime; } }

				/// <summary>
				/// The total number of entries in the board
				/// </summary>
				public UInt32 TotalEntriesOnBoard { get { return totalEntriesOnBoard; } }

				/// <summary>
				/// The board of which ranks belong to
				/// </summary>
				public UInt32 BoardId { get { return boardId; } }

				/// <summary>
				/// Number of valid entries in the <see cref="RankData"/> (or <see cref="RankDataForCrossSave"/>) array. Different from size of those arrays if the number of entries in the board is less than the requested
				/// </summary>
				public UInt64 NumValidEntries { get { return numValidEntries; } }

				/// <summary>
				/// The rank from which the range was originally requested
				/// </summary>
				public Int32 StartRank { get { return startRank; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.RangeOfRanksBegin);

					isCrossSaveInformation = readBuffer.ReadBool();
					UInt64 range = readBuffer.ReadUInt64();

					scoreRankData = null;
					scoreRankDataForCrossSave = null;

					if (range > 0)
					{
						if (isCrossSaveInformation == true)
						{
							scoreRankDataForCrossSave = new ScoreRankDataForCrossSave[range];

							for (UInt64 i = 0; i < range; i++)
							{
								scoreRankDataForCrossSave[i] = new ScoreRankDataForCrossSave();
								scoreRankDataForCrossSave[i].ReadData(readBuffer);
								scoreRankDataForCrossSave[i].ReadAdditionalData(readBuffer);
							}
						}
						else
						{
							scoreRankData = new ScoreRankData[range];

							for (UInt64 i = 0; i < range; i++)
							{
								scoreRankData[i] = new ScoreRankData();
								scoreRankData[i].ReadData(readBuffer);
								scoreRankData[i].ReadAdditionalData(readBuffer);
							}
						}
					}

					numValidEntries = readBuffer.ReadUInt64();

					updateTime = Core.ReadRtcTick(readBuffer);
					totalEntriesOnBoard = readBuffer.ReadUInt32();
					boardId = readBuffer.ReadUInt32();
					startRank = readBuffer.ReadInt32();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.RangeOfRanksEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function gets a range of ranks from a board from an specified starting rank.
			/// </summary>
			/// <param name="request">The parameters needed to get a range of ranks from a board starting from a rank specified.</param>
			/// <param name="response">This response contains the return code and an array with the information requested</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetRangeOfRanks(GetRangeOfRanksRequest request, RangeOfRanksResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetRangeOfRanks(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Get Friends Ranks

			/// <summary>
			/// Response data containing the ranks, comments and game information of the range requested (if any) from a call to <see cref="SetScore"/>
			/// </summary>
			public class FriendsRanksResponse : ResponseBase
			{
				internal ScoreRankData[] scoreRankData;
				internal ScoreRankDataForCrossSave[] scoreRankDataForCrossSave;

				internal bool isCrossSaveInformation;
				internal UInt64 numFriends;
				internal DateTime updateTime;
				internal UInt32 boardId;
				internal UInt32 totalEntriesOnBoard;
				internal UInt32 totalFriendsOnBoard;
				internal Int32 friendsWithPcId;

				/// <summary>
				/// Rank data information for the users on the board in case <see cref="IsCrossSaveInformation"/> was not set in the request   
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if <see cref="IsCrossSaveInformation"/> isn't set to false.</exception>
				public ScoreRankData[] RankData
				{
					get
					{
						if (isCrossSaveInformation != false)
						{
							throw new NpToolkitException("RankData isn't valid unless 'IsCrossSaveInformation' is set to false.");
						}

						return scoreRankData;
					}
				}

				/// <summary>
				/// Rank data information for the users on the board in case <see cref="IsCrossSaveInformation"/> was set to true in the request 
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if <see cref="IsCrossSaveInformation"/> isn't set to true.</exception>
				public ScoreRankDataForCrossSave[] RankDataForCrossSave
				{
					get
					{
						if (isCrossSaveInformation != true)
						{
							throw new NpToolkitException("RankDataForCrossSave isn't valid unless 'IsCrossSaveInformation' is set to true.");
						}

						return scoreRankDataForCrossSave;
					}
				}

				/// <summary>
				/// It is true if the information requested was from a shared board between PS Vita or PS3        
				/// </summary>
				public bool IsCrossSaveInformation { get { return isCrossSaveInformation; } }

				/// <summary>
				/// The size of <see cref="RankData"/> (or <see cref="RankDataForCrossSave"/>) array   
				/// </summary>
				public UInt64 NumFriends { get { return numFriends; } }

				/// <summary>
				/// The time when the server created the ranking information (UTC)  
				/// </summary>
				public DateTime UpdateTime { get { return updateTime; } }

				/// <summary>
				/// The board of which ranks belong to
				/// </summary>
				public UInt32 BoardId { get { return boardId; } }

				/// <summary>
				/// The total amount of entries the board has
				/// </summary>
				public UInt32 TotalEntriesOnBoard { get { return totalEntriesOnBoard; } }

				/// <summary>
				/// The total amount of friends of the calling user the board has
				/// </summary>
				public UInt32 TotalFriendsOnBoard { get { return totalFriendsOnBoard; } }

				/// <summary>
				/// The pc Id for all friends retrieved 
				/// </summary>
				public Int32 FriendsWithPcId { get { return friendsWithPcId; } }


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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.FriendsRanksBegin);

					isCrossSaveInformation = readBuffer.ReadBool();
					numFriends = readBuffer.ReadUInt64();

					scoreRankData = null;
					scoreRankDataForCrossSave = null;

					if (isCrossSaveInformation == true)
					{
						scoreRankDataForCrossSave = new ScoreRankDataForCrossSave[numFriends];

						for (UInt64 i = 0; i < numFriends; i++)
						{
							scoreRankDataForCrossSave[i] = new ScoreRankDataForCrossSave();
							scoreRankDataForCrossSave[i].ReadData(readBuffer);
							scoreRankDataForCrossSave[i].ReadAdditionalData(readBuffer);
						}
					}
					else
					{
						scoreRankData = new ScoreRankData[numFriends];

						for (UInt64 i = 0; i < numFriends; i++)
						{
							scoreRankData[i] = new ScoreRankData();
							scoreRankData[i].ReadData(readBuffer);
							scoreRankData[i].ReadAdditionalData(readBuffer);
						}
					}			

					updateTime = Core.ReadRtcTick(readBuffer);
				
					boardId = readBuffer.ReadUInt32();
					totalEntriesOnBoard = readBuffer.ReadUInt32();
					totalFriendsOnBoard = readBuffer.ReadUInt32();
					friendsWithPcId = readBuffer.ReadInt32();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.FriendsRanksEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function gets ranks from a board from users who are friends of the calling user, starting on an specified rank.
			/// </summary>
			/// <param name="request">The parameters needed to get all friends ranks from a board starting from a rank specified.</param>
			/// <param name="response">This response contains the return code and an array with the information requested.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetFriendsRanks(GetFriendsRanksRequest request, FriendsRanksResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetFriendsRanks(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Get Users Ranks

			/// <summary>
			/// Response data containing the ranks, comments and game information of the specific users requested (if any).
			/// </summary>
			public class UsersRanksResponse : ResponseBase
			{
				internal ScorePlayerRankData[] users;
				internal ScorePlayerRankDataForCrossSave[] usersForCrossSave;

				internal bool isCrossSaveInformation;
				internal UInt64 numUsers;
				internal UInt64 numValidUsers;
				internal DateTime updateTime;
				internal UInt32 boardId;
				internal UInt32 totalEntriesOnBoard;

				/// <summary>
				/// Users requested, and their rank data information in case <see cref="IsCrossSaveInformation"/> was not set in the request. The order of users in the array maps the order given in the request, in the <see cref="GetUsersRanksRequest.Users"/> array.
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if <see cref="IsCrossSaveInformation"/> isn't set to false.</exception>
				public ScorePlayerRankData[] Users
				{
					get
					{
						if (isCrossSaveInformation != false)
						{
							throw new NpToolkitException("RankData isn't valid unless 'IsCrossSaveInformation' is set to false.");
						}

						return users;
					}
				}

				/// <summary>
				/// Users requested, and their rank data information in case <see cref="IsCrossSaveInformation"/> was set to true in the request. The order of users in the array maps the order given in the request, in the <see cref="GetUsersRanksRequest.Users"/> array.
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if <see cref="IsCrossSaveInformation"/> isn't set to true.</exception>
				public ScorePlayerRankDataForCrossSave[] UsersForCrossSave
				{
					get
					{
						if (isCrossSaveInformation != true)
						{
							throw new NpToolkitException("RankDataForCrossSave isn't valid unless 'IsCrossSaveInformation' is set to true.");
						}

						return usersForCrossSave;
					}
				}

				/// <summary>
				/// It is true if the information requested was from a shared board between PS Vita or PS3        
				/// </summary>
				public bool IsCrossSaveInformation { get { return isCrossSaveInformation; } }

				/// <summary>
				/// The size of <see cref="Users"/> (or <see cref="UsersForCrossSave"/>) array   
				/// </summary>
				public UInt64 NumUsers { get { return numUsers; } }

				/// <summary>
				/// Number of valid entries in the <see cref="Users"/> (or <see cref="UsersForCrossSave"/>) array. This is, the number of users found to have a rank and returned in this call   
				/// </summary>
				public UInt64 NumValidUsers { get { return numValidUsers; } }

				/// <summary>
				/// The time when the server created the ranking information (UTC)  
				/// </summary>
				public DateTime UpdateTime { get { return updateTime; } }

				/// <summary>
				/// The board of which ranks belong to
				/// </summary>
				public UInt32 BoardId { get { return boardId; } }

				/// <summary>
				/// The total amount of entries the board has
				/// </summary>
				public UInt32 TotalEntriesOnBoard { get { return totalEntriesOnBoard; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.UsersRanksBegin);

					isCrossSaveInformation = readBuffer.ReadBool();
					numUsers = readBuffer.ReadUInt64();
					numValidUsers = readBuffer.ReadUInt64();

					users = null;
					usersForCrossSave = null;

					if (isCrossSaveInformation == true)
					{
						usersForCrossSave = new ScorePlayerRankDataForCrossSave[numUsers];

						for (UInt64 i = 0; i < numUsers; i++)
						{
							usersForCrossSave[i] = new ScorePlayerRankDataForCrossSave();
							usersForCrossSave[i].Read(readBuffer);

							if (usersForCrossSave[i].HasData == true)
							{
								usersForCrossSave[i].ReadAdditionalData(readBuffer);
							}
						}
					}
					else
					{
						users = new ScorePlayerRankData[numUsers];

						for (UInt64 i = 0; i < numUsers; i++)
						{
							users[i] = new ScorePlayerRankData();
							users[i].Read(readBuffer);

							if (users[i].HasData == true)
							{
								users[i].ReadAdditionalData(readBuffer);
							}
						}
					}

					updateTime = Core.ReadRtcTick(readBuffer);

					boardId = readBuffer.ReadUInt32();
					totalEntriesOnBoard = readBuffer.ReadUInt32();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.UsersRanksEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function gets the ranks of all users specified for one specific board.
			/// </summary>
			/// <param name="request">The parameters needed to get information from users on the Ranking server for an specific board.</param>
			/// <param name="response">This response contains the return code and an array with the information requested.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetUsersRanks(GetUsersRanksRequest request, UsersRanksResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetUsersRanks(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Set Game Data

			/// <summary>
			/// Response data containing an indicator if a new call needs to be performed to set a new chunk of game data or if all game data information has been already set.
			/// </summary>
			public class SetGameDataResultResponse : ResponseBase
			{
				internal Int32 chunkId;

				/// <summary>
				/// When different than 0 it means there are more chunks pending. Use this value to set, in the next request in <see cref="SetGameData"/>, the member <see cref="SetGameDataRequest.idOfPrevChunk"/>.
				/// </summary>
				public Int32 ChunkId { get { return chunkId; } }
			
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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.SetGameDataBegin);

					chunkId = readBuffer.ReadInt32();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.SetGameDataEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function sets game data for an specific entry of the calling user in one board on the Server.
			/// </summary>
			/// <param name="request">The parameters parameters needed to set game data for an entry on the Ranking server .</param>
			/// <param name="response">This response contains the chunk Id. If it is 0, it means all game data has been set. Otherwise (chunks are being used and the last one has not been sent yet), this value needs to be used in the next request to set idOfPrevChunk.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int SetGameData(SetGameDataRequest request, SetGameDataResultResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxSetGameData(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Get Game Data

			/// <summary>
			/// Response data containing the game data retrieved and an indicator if a new call needs to be performed to get a new chunk.
			/// </summary>
			public class GetGameDataResultResponse : ResponseBase
			{
				internal UInt64 totalSize;
				internal UInt64 rcvDataSize;
				internal UInt64 rcvDataValidSize;
				internal UInt64 startIndex;
				internal Int32 chunkId;
				internal byte[] rcvData;

				/// <summary>
				/// The total size of the entire game data. All chunks included
				/// </summary>
				public UInt64 TotalSize { get { return totalSize; } }

				/// <summary>
				/// The size of the chunk buffer recieved.
				/// </summary>
				public UInt64 RcvDataSize { get { return rcvDataSize; } }

				/// <summary>
				/// The number of bytes of proper data. It may defer from <see cref="RcvDataSize"/> if the remaining bytes from the game data do not match the chunk size.
				/// </summary>
				public UInt64 RcvDataValidSize { get { return rcvDataValidSize; } }

				/// <summary>
				/// The index into the <see cref="RcvData"/> array where the recieved data was written.
				/// </summary>
				public UInt64 StartIndex { get { return startIndex; } }

				/// <summary>
				/// Same array as the one given in the request. Now it contains the game data chunk.
				/// </summary>
				public byte[] RcvData { get { return rcvData; } }
				
				/// <summary>
				/// When different than 0 it means there are more chunks pending. Use this value to set, in the next request in <see cref="GetGameData"/>, the member <see cref="GetGameDataRequest.IdOfPrevChunk"/>.  
				/// </summary>
				public Int32 ChunkId { get { return chunkId; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.GetGameDataBegin);

					totalSize = readBuffer.ReadUInt64();
					rcvDataSize = readBuffer.ReadUInt64();
					rcvDataValidSize = readBuffer.ReadUInt64();
					chunkId = readBuffer.ReadInt32();

					// Special case for GetGameData
					// The chunkToRcvData in the GetGameDataRequest instance is used to fill in the data in the Response object.
 					// In this request the array instance should work the same way as defined in the NpToolkit2 system.
					GetGameDataRequest gameDataRequest = request as GetGameDataRequest;
					rcvData = gameDataRequest.rcvData;

					startIndex = gameDataRequest.byteOffset;

					readBuffer.ReadData(ref rcvData, (uint)startIndex);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.GetGameDataEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function gets game data for an specific entry of the user specified in one board on the Server.
			/// </summary>
			/// <param name="request">The parameters needed to get game data for an entry on the Ranking server.</param>
			/// <param name="response">This response contains the chunk Id. If it is 0, it means all game data has been retrieved. Otherwise (chunks are being used and the last one has not been retrieved yet), this value needs to be used in the next request to set idOfPrevChunk.</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetGameData(GetGameDataRequest request, GetGameDataResultResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetGameData(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion
		}
	}
}
