using System;
using Sony.NP;
using System.Runtime.InteropServices;
using System.Threading;


namespace Sony
{
    namespace NP
    {
        // Thread used to populate the C# Response objects from the data
        // in the C++ Response objects. 
        // This can take some time, e.g. Copying 2000 friends worth of data to the GetFriendsResponse class

        static class PopulateThread
        {
            [DllImport("UnityNpToolkit2")]
            private static extern bool PrxPopFirstResponse(out ServiceTypes service, out FunctionTypes apiCalled, out UInt32 npRequestId, out Int32 userId, out Int32 customReturnCode);

            static Thread populateThread;
            static bool stopThread = false;
            static Semaphore workLoad = new Semaphore(0, 1000);

            public static void Start()
            {
                stopThread = false;
                populateThread = new Thread(new ThreadStart(RunProc));
                populateThread.Name = "Sony Np";
                populateThread.Start();
            }

            private static void RunProc()
            {
                workLoad.WaitOne();

                while (!stopThread)
                {
                    // Process the pending queue here
                    ServiceTypes service;
                    FunctionTypes apiCalled;
                    UInt32 npRequestId;
                    Core.UserServiceUserId userId;
                    Int32 customReturnCode;

                    //while (PrxPopFirstResponse(out service, out apiCalled, out npRequestId, out userId.id) == true)
                    if (PrxPopFirstResponse(out service, out apiCalled, out npRequestId, out userId.id, out customReturnCode) == true)
                    {
                        RequestBase request = null;

                        NpCallbackEvent newEvent = null;

                        try
                        {

                            newEvent = new NpCallbackEvent();

                            // Find the response.
                            if (service == ServiceTypes.Notification)
                            {
                                // For notifications the response object isn't created by the App,
                                // but has been created internally by the plugin. Therefore no response object
                                // was added to PendingAsyncResponseList. Time to create a response object based on the
                                // the type of Notifcation
                                // See C++ method CompletedAsyncEvents::CopyNotification
                                newEvent.response = Notifications.CreateNotificationResponse(apiCalled);
                            }
                            else
                            {
                                // Can remove the pending request as it has now finished.
                                // Can't abort the request once it is complete so it needs to be removed from the list
                                // However this must not be done for Notification requests as these can't be aborted and woundn't be part of the list.
                                request = PendingAsyncRequestList.RemoveRequest(npRequestId);

                                newEvent.response = PendingAsyncResponseList.FindAndRemoveResponse(npRequestId);

                                if (newEvent.response == null)
                                {
                                    Console.WriteLine("Error : PopulateThread.RunProc : Can't find response object for Request " + npRequestId);
                                }
                            }

                            if (newEvent.response != null)
                            {
                                // A response object has been found. This will have been allocated in the Unity project
                                // and passed as a paramater to one of the method calls. e.g. GetFriends
                                newEvent.response.PopulateFromNative(npRequestId, apiCalled, request);

                                // For custom requests (NpRequest system) then there might be a custom return code set if an error has occured.
                                // Set it here.
                                if (customReturnCode != 0)
                                {
                                    newEvent.response.returnCode = customReturnCode;
                                }
                            }
                            else
                            {
                                // An error has occured. Neither a notification Response nor a Aysnc response
                                // has been found or created.
                                // This should never happen, but must handle it here.
                                // Can't throw an exception as this is a seperate thread for reading, so need to impleement sending back an
                                // error to the main thread that can be handled during the main loop
                            }

                            newEvent.service = service;
                            newEvent.apiCalled = apiCalled;
                            newEvent.npRequestId = npRequestId;
                            newEvent.userId = userId;
                            newEvent.request = request;

                            // Only add it to the queue if the Behaviour update is reading it.
                            //PendingCallbackQueue.AddEvent(newEvent);

                            // Do callback to Unity project on this thread.
                            Main.CallOnAsyncEvent(newEvent);
                        }
                        catch (NpToolkitException e)
                        {
                            // Must catch any exceptions in this thread otherwise the system will just stop working and this thread will abort
                            Console.WriteLine("Toolkit Exception - PopulateThread.RunProc : " + e.ExtendedMessage);
                            Console.WriteLine(e.StackTrace);

                            Console.WriteLine("Toolkit Exception : service = " + service + " : apiCalled = " + apiCalled + "(" + (int)apiCalled + ") : npRequestId = " + npRequestId + " : userId = " + userId.id);

                            if (request != null)
                            {
                                Console.WriteLine("Toolkit Exception - Caused by Request : " + request.functionType);
                            }

                            if (newEvent != null && newEvent.response != null)
                            {
                                Console.WriteLine("Toolkit Exception - Response Type = " + newEvent.response.GetType().ToString());
                            }
                        }
                        catch (Exception e)
                        {
                            // Must catch any exceptions in this thread otherwise the system will just stop working and this thread will abort
                            Console.WriteLine("Exception - PopulateThread.RunProc : " + e.Message);
                            Console.WriteLine(e.StackTrace);

                            Console.WriteLine("Toolkit Exception : service = " + service + " : apiCalled = " + apiCalled + "(" + (int)apiCalled + ") : npRequestId = " + npRequestId + " : userId = " + userId.id);

                            if (request != null)
                            {
                                Console.WriteLine("Toolkit Exception - Caused by Request : " + request.functionType);
                            }
                            else
                            {
                                Console.WriteLine("Toolkit Exception - No request data available");
                            }
                        }
                    }

                    workLoad.WaitOne();
                }
            }

            public static void Execute()
            {
                workLoad.Release();
            }

            public static void Stop()
            {
                stopThread = true;
                workLoad.Release();
            }

            [AOT.MonoPInvokeCallback(typeof(Main.OnPrxCallbackEvent))]
            public static void OnPrxNpToolkitEvent()
            {
                // Some work is available in the PRX so wake up the "Sony Np" thread (PopulateThread)
                PopulateThread.Execute();
            }

        }

        // There is a NpRequest pending. This is an async request occurring inside the C++ code, so a regular update needs to be called
        // so the requests results can be polled.
        // This uses a semaphore to record how many pending NpRequests there are. This thread will continue to loop
        // around and test if the first request has been completed. If not it will sleep and then poll again.
        // Once a request has been successfully process it will then wait on the semaphore. This means the thread will be dormant
        // if there is no work to be done. 
        // Note this doesn't actually retrieve any data. The C++ code pushes the results into the event queue so the PopulateThread (above)
        // can retrieve the results in the same way it retrieves all other NPT2 results.
        static class NpRequestsThread
        {
            [DllImport("UnityNpToolkit2")]
            private static extern bool PrxPollFirstRequest();

            static Thread requestsThread;
            static bool stopThread = false;
            static Semaphore workLoad = new Semaphore(0, 1000);

            public static void Start()
            {
                stopThread = false;
                requestsThread = new Thread(new ThreadStart(RunProc));
                requestsThread.Name = "Requests Thread";
                requestsThread.Start();
            }

            private static void RunProc()
            {
                workLoad.WaitOne();

                while (!stopThread)
                {
                    if (PrxPollFirstRequest() == true)
                    {
                        // A request has been completed, so now wait.
                        workLoad.WaitOne();
                    }
                    else
                    {
                        // There is a pending NpRequest but it isn't ready yet, so sleep a bit and poll again.
                        Thread.Sleep(1000);
                    }
                }
            }

            public static void Execute()
            {
                workLoad.Release();
            }

            public static void Stop()
            {
                stopThread = true;
                workLoad.Release();
            }

            [AOT.MonoPInvokeCallback(typeof(Main.OnPrxCallbackEvent))]
            public static void OnPrxNpRequestEvent()
            {
                // Some Np Request is available to poll on the NpRequestsThread thread.
                NpRequestsThread.Execute();
            }
        }
    }
}
