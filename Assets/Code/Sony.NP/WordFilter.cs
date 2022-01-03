using System;
using System.Runtime.InteropServices;

namespace Sony
{
	namespace NP
	{
		/// <summary>
		/// Word Filter service related functionality.
		/// </summary>
		public class WordFilter
		{
			#region DLL Imports

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxFilterComment(FilterCommentRequest request, out APIResult result);

			#endregion

			#region Requests

			/// <summary>
			/// Parameters passed to filter a comment that may contain profanity.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
			public class FilterCommentRequest : RequestBase
			{
				/// <summary>
				/// The maximum size of the comment to filter
				/// </summary>
				public const int MAX_SIZE_COMMENT = 1024;   // SCE_NP_WORD_FILTER_SANITIZE_COMMENT_MAXLEN

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_SIZE_COMMENT + 1)]
				internal string comment;

				/// <summary>
				/// The comment to filter.
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the path is more than <see cref="MAX_SIZE_COMMENT"/> characters.</exception>
				public string Comment
				{
					get { return comment; }
					set
					{
						if (value.Length > MAX_SIZE_COMMENT)
						{
							throw new NpToolkitException("The size of the string is more than " + MAX_SIZE_COMMENT + " characters.");
						}
						comment = value;
					}
				}


				/// <summary>
				/// Initializes a new instance of the <see cref="FilterCommentRequest"/> class.
				/// </summary>
				public FilterCommentRequest()
					: base(ServiceTypes.WordFilter, FunctionTypes.WordfilterFilterComment)
				{

				}
			}

			#endregion

			#region Filter Comment

			/// <summary>
			/// Class containing a comment that has been sanitized in case it is needed.
			/// </summary>
			public class SanitizedCommentResponse : ResponseBase
			{
				internal string resultComment;
				internal bool isCommentChanged;

				/// <summary>
				/// The comment sanitized in case it wants to be used
				/// </summary>
				public string ResultComment { get { return resultComment; } }

				/// <summary>
				/// True if the comment had words that have been filtered
				/// </summary>
				public bool IsCommentChanged { get { return isCommentChanged; } }

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

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.WordFilterBegin);

					readBuffer.ReadString(ref resultComment);
					isCommentChanged = readBuffer.ReadBool();

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.WordFilterEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// This function will filter the comment provided in case it is necessary.
			/// </summary>
			/// <param name="request">Parameters needed to filter a comment</param>
			/// <param name="response">This response contains the return code, a flag indicating if the comment was modified, the resulted comment and its size</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int FilterComment(FilterCommentRequest request, SanitizedCommentResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxFilterComment(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion
		}
	}
}
