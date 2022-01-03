using System;
using System.Runtime.InteropServices;

namespace Sony
{
	namespace NP
	{
		/// <summary>
		/// TSS service related functionality.
		/// </summary>
		public class Tss
		{
			#region DLL Imports

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxTssGetData(GetDataRequest request, out APIResult result);

			#endregion

			#region Requests

			/// <summary>
			/// The parameters required to retrieve data from the Title Small Storage (TSS) server
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
			public class GetDataRequest : RequestBase
			{
				internal UInt64 offset;             /// Optional parameter - The byte offset where to start retieving the data
				internal UInt64 length;             /// Optional parameter - The length of the block of memory to retrieve. 
				internal UInt64 lastModifiedTicks;  /// Optional parameter - Test when data was last modified and not to return it unless it has changed. This is stored in SceRtcTick format

				internal Int32 tssSlotId;

				[MarshalAs(UnmanagedType.I1)]
				internal bool retrieveStatusOnly;

				/// <summary>
				/// Specify the position from which to obtain TSS data. Defaults to 0. This is ignored if <see cref="Length"/> is 0.
				/// </summary>
				public UInt64 Offset
				{
					get { return offset; }
					set { offset = value; }
				}

				/// <summary>
				/// The length of the TSS data chunk to receive. Defaults to 0. If length is set then <see cref="Offset"/> is also used to specify the start of the block.
				/// To retrieve the entire block of TSS data from the slot, set <see cref="Length"/> to 0. 
				/// </summary>
				public UInt64 Length
				{
					get { return length; }
					set { length = value; }
				}

				/// <summary>
				/// Optional parameter to evaluate when the TSS data was last written to the server the TSS data. If the time at which the TSS file was last updated on the server is equal to or older than the time specified the data won't be retrieved.
				/// Use this when caching the TSS data and only retrieve the data if it has changed on the server.
				/// Because evaluation is carried out according to the time on the server, do not use time obtained locally, use the time of the TSS server obtainable in the Response object <see cref="TssDataResponse"/>. 
				/// This can be obtained when retrieving TSS data or when <see cref="RetrieveStatusOnly"/> is set to true. 
				/// </summary>
				public DateTime LastModifiedTicks
				{
					get 
					{
						return Core.RtcTicksToDateTime(lastModifiedTicks);
					}
					set 
					{ 
						lastModifiedTicks = Core.DateTimeToRtcTicks(value);
					}
				}

				/// <summary>
				/// The Slot ID on the TSS server to retrieve data from
				/// </summary>
				public Int32 TssSlotId
				{
					get { return tssSlotId; }
					set { tssSlotId = value; }
				}

				/// <summary>
				/// If true, only the TSS data status is retrieved. The data buffer will be empty, but other infomation about the data such as length and last modified time will be returned.
				/// </summary>
				public bool RetrieveStatusOnly
				{
					get { return retrieveStatusOnly; }
					set { retrieveStatusOnly = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetDataRequest"/> class.
				/// </summary>
				public GetDataRequest()
					: base(ServiceTypes.Tss, FunctionTypes.TssGetData)
				{

				}
			}

			#endregion

			#region Get Data

			/// <summary>
			/// The status of the TSS data buffer
			/// </summary>
			public enum TssStatusCodes
			{
				/// <summary>
				/// The entire buffer has been returned.
				/// </summary>
				Ok, // SCE_NP_TSS_STATUS_TYPE_OK,
				/// <summary>
				/// Not supported
				/// </summary>
				Partial, // SCE_NP_TSS_STATUS_TYPE_PARTIAL,
				/// <summary>
				/// The buffer was not modified since the conditional time passed into the <see cref="GetData"/> method.
				/// </summary>
				NotModified, //SCE_NP_TSS_STATUS_TYPE_NOT_MODIFIED
			}

			/// <summary>
			/// The binary data that was retrieved from the TSS server along with it's status
			/// </summary>
			public class TssDataResponse : ResponseBase
			{
				internal byte[] data;
				internal DateTime lastModified;
				internal TssStatusCodes statusCode;
				internal Int64 contentLength;

				/// <summary>
				/// The binary data
				/// </summary>
				public byte[] Data
				{
					get { return data; }
				}

				/// <summary>
				/// Last update time
				/// </summary>
				public DateTime LastModified
				{
					get { return lastModified; }
				}

				/// <summary>
				/// The status of the data buffer
				/// </summary>
				public TssStatusCodes StatusCode
				{
					get { return statusCode; }
				}

				/// <summary>
				/// The content length of the buffer.
				/// </summary>
				public Int64 ContentLength
				{
					get { return contentLength; }
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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.TssDataBegin);

					readBuffer.ReadData(ref data);
					lastModified = Core.ReadRtcTick(readBuffer);
					statusCode = (TssStatusCodes)readBuffer.ReadInt32();
					contentLength = readBuffer.ReadInt64();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.TssDataEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// Get Title Small Storage (TSS) data from a specified slot
			/// </summary>
			/// <param name="request">Parameters needed to retrieve TSS data</param>
			/// <param name="response">This response contains the return code and the TSS Data</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetData(GetDataRequest request, TssDataResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxTssGetData(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion
		}
	}
}
