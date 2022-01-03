using System;
using System.Collections.Generic;
using Sony.NP;
using System.Runtime.InteropServices;
using System.Threading;

namespace Sony
{
    namespace NP
    {
        /// <summary>
        /// Callback class containing the results of an async request or notification
        /// </summary>
        public class NpCallbackEvent
        {
            internal ServiceTypes service;
            internal FunctionTypes apiCalled;
            internal UInt32 npRequestId;
            internal ResponseBase response;
            internal Core.UserServiceUserId userId;
            internal RequestBase request;

            /// <summary>
            /// Service for which the request belongs to. The Notification service indicates a modification from the system
            /// </summary>
            public ServiceTypes Service { get { return service; } }

            /// <summary>
            /// Function called to perform the request. In case of Notification service, the type of notification
            /// </summary>
            public FunctionTypes ApiCalled { get { return apiCalled; } }

            /// <summary>
            /// The request Id returned when the async request was made
            /// </summary>
            public UInt32 NpRequestId { get { return npRequestId; } }

            /// <summary>
            /// The response passed when the request was made. In case of notifications, it will be created by the plug-in.
            /// </summary>
            public ResponseBase Response { get { return response; } }

            /// <summary>
            /// The request instance that started the async request. Will be null for any Notification responses.
            /// </summary>
            public RequestBase Request { get { return request; } }

            /// <summary>
            /// The user Id of the user who performed the request
            /// </summary>
            public Core.UserServiceUserId UserId { get { return userId; } }

        }

        /// <summary>
        /// Used to store the request id returned by some of the NpToolkit methods.
        /// This Id can be used to abort the request and remove it from the internal NpToolkit queue.
        /// </summary>
        internal static class PendingCallbackQueue
        {
            // Contains a list of pending requests that can be access via the C# interface
            private static Queue<NpCallbackEvent> pendingEvents = new Queue<NpCallbackEvent>();

            private static Object syncObject = new Object();

            static public void AddEvent(NpCallbackEvent callbackEvent)
            {
                Monitor.Enter(syncObject);

                pendingEvents.Enqueue(callbackEvent);

                Monitor.Exit(syncObject);
            }

            static public NpCallbackEvent PopEvent()
            {
                NpCallbackEvent pending = null;

                if (Monitor.TryEnter(syncObject))
                {
                    if (pendingEvents.Count == 0)
                    {
                        Monitor.Exit(syncObject);
                        return null;
                    }

                    pending = pendingEvents.Dequeue();

                    Monitor.Exit(syncObject);
                }

                return pending;
            }
        }
    }
}
