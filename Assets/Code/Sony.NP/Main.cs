using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Sony
{
	namespace NP
	{
		/// <summary>
		/// Main entry point to the NpToolkit plug-in and initialization
		/// </summary>
		public class Main
		{

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void OnPrxCallbackEvent();

			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class ValidationChecks
			{
				public UInt32 expectedNumFunctionTypes;

				public void Init()
				{
					expectedNumFunctionTypes = (UInt32)FunctionTypes.NumFunctionTypes;
				}
			}

			// A global struct showing if NpToolkit has been initialised and the SDK version number for the native plugin.
			static public InitResult initResult;

			/// <summary>
			/// Initialise the NpToolkit2 system
			/// </summary>
			/// <param name="initParams">The initialisation paramaters.</param>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an public error has occured inside the NpToolkit plug-in.</exception>
			public static InitResult Initialize(InitToolkit initParams)
			{
				APIResult result;

				ValidationChecks checks = new ValidationChecks();
				checks.Init();
			    MainClass.PrxValidateToolkit(checks, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				// Check if the init params are valid and if something isn't this will result in an exception being thrown.
				initParams.CheckValid();

				OnPrxCallbackEvent npToolkitThreadEvent = new OnPrxCallbackEvent(PopulateThread.OnPrxNpToolkitEvent);
				OnPrxCallbackEvent npRequestThreadEvent = new OnPrxCallbackEvent(NpRequestsThread.OnPrxNpRequestEvent);

                NativeInitResult nativeResult = new NativeInitResult();

                MainClass.PrxInitialize(initParams, out nativeResult, npToolkitThreadEvent, npRequestThreadEvent, out result);

                initResult.Initialise(nativeResult);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				PopulateThread.Start();
				NpRequestsThread.Start();



				return initResult;
			}

			/// <summary>
			/// Delegate event handler defining the callback event
			/// </summary>
			/// <param name="npEvent"></param>
			public delegate void EventHandler(NpCallbackEvent npEvent);

			/// <summary>
			/// The event called when an async request has been completed or a notification
			/// </summary>
			public static event EventHandler OnAsyncEvent;

			// Handle special events that require the DLL to managed public structures
			public static void publicEventHandler(NpCallbackEvent npEvent)
			{
				// Handle request that have been aborted
				if (npEvent.service == ServiceTypes.Notification)
				{
					if (npEvent.apiCalled == FunctionTypes.NotificationAborted)
					{
						// A pending request has been aborted, so remove it from the public list
						PendingAsyncRequestList.RequestHasBeenAborted(npEvent.npRequestId);
					}
				}
			}

			// Called from the Populate thread 
			public static void CallOnAsyncEvent(NpCallbackEvent npEvent)
			{
				// do any public management here
				publicEventHandler(npEvent);

				try
				{
					OnAsyncEvent(npEvent);
				}
				catch (Exception e)
				{
					Console.WriteLine("Exception Occured in OnAsyncEvent handler : " + e.Message);
					Console.WriteLine(e.StackTrace);
					throw;  // Throw the expection again as this shouldn't really hide the exception has occured.
				}
				
			}

			/// <summary>
			/// Update function
			/// </summary>
			public static void Update()
			{
                MainClass.PrxUpdate();
				//PumpAsyncEvents();
			}

			// Use this to call the OnAsyncEvent if doing updates on the Unity script Behaviour thread.
			private static void PumpAsyncEvents()
			{
				//UnityEngine.Profiler.BeginSample("SonyNP PumpAsyncEvents");

				if (OnAsyncEvent != null)
				{
					NpCallbackEvent callbackEvent = PendingCallbackQueue.PopEvent();

					while (callbackEvent != null)
					{
						// do any public management here
						publicEventHandler(callbackEvent);

						// Do callback to Unity project
						OnAsyncEvent(callbackEvent);

						// Get next event
						callbackEvent = PendingCallbackQueue.PopEvent();
					}
				}

				//UnityEngine.Profiler.EndSample();
			}

			/// <summary>
			/// Shutdown the NpToolkit2 system
			/// </summary>
			public static void ShutDown()
			{
				PopulateThread.Stop();
				NpRequestsThread.Stop();

				PendingAsyncRequestList.Shutdown();

                MainClass.PrxShutDown();		
			}

			/// <summary>
			/// Get the pending async requests list. This takes a copy of the list so it is safe to enumerate the list.
			/// </summary>
			/// <returns>A list of pending async requests.</returns>
			public static List<Sony.NP.PendingRequest> GetPendingRequests()
			{
				return PendingAsyncRequestList.PendingRequests;
			}

			/// <summary>
			/// Abort a pending request. A pending request at the top of the list may not abort as processing the request may have already started.
			/// </summary>
			/// <param name="npRequestId">The request to abort.</param>
			/// <returns>Returns true is the request is in the pending list, otherwise returns false.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an public error has occured inside the NpToolkit plug-in.</exception>
			public static bool AbortRequest(UInt32 npRequestId)
			{
				if (PendingAsyncRequestList.IsPending(npRequestId) == false)
				{
					return false;
				}

				APIResult result;

                MainClass.PrxAbortRequest(npRequestId, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				PendingAsyncRequestList.MarkRequestAsAborting(npRequestId);

				return true;
			}
		}
	} // NP
} // Sony
