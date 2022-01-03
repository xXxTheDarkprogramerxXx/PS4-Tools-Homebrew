using UnityEngine;

using System.Collections;

using UnityEngine.PS4;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Code.Wrapper
{
    public class User
    {
        public static Sony.NP.Core.UserServiceUserId GetActiveUserId
        {
            get
            {
                Sony.NP.Core.UserServiceUserId id = new Sony.NP.Core.UserServiceUserId();
                int Id = 0;
                int.TryParse(MainClass.Get_UserId(), out Id);
                id.Id = Id;
                return id;
            }
        }
    }

    public class TrophyManager
    {
        public static void RegisterTrophyPack()
        {
            try
            {
                Sony.NP.Trophies.RegisterTrophyPackRequest request = new Sony.NP.Trophies.RegisterTrophyPackRequest();
                Sony.NP.Core.UserServiceUserId id = new Sony.NP.Core.UserServiceUserId();
                int Id = 0;
                int.TryParse(MainClass.Get_UserId(), out Id);
                id.Id = Id;


                request.UserId = id;
                request.Async = false;

                Sony.NP.Core.EmptyResponse response = new Sony.NP.Core.EmptyResponse();

                // Make the async call which will return the Request Id 
                int requestId = Sony.NP.Trophies.RegisterTrophyPack(request, response);
                
                //Assets.Code.MessageBox.Show("RegisterTrophyPack Async : Request Id = " + requestId);

                //OnScreenLog.Add("RegisterTrophyPack Async : Request Id = " + requestId);
            }
            catch (Sony.NP.NpToolkitException e)
            {
                Assets.Code.MessageBox.Show("Error\n\n" + e.ExtendedMessage);
                //OnScreenLog.AddError("Exception : " + e.ExtendedMessage);
            }
        }

        public static void GetUnlockedTrophies()
        {
            try
            {
                Sony.NP.Trophies.GetUnlockedTrophiesRequest request = new Sony.NP.Trophies.GetUnlockedTrophiesRequest();
                request.UserId = User.GetActiveUserId;

                Sony.NP.Trophies.UnlockedTrophiesResponse response = new Sony.NP.Trophies.UnlockedTrophiesResponse();

                // Make the async call which will return the Request Id 
                int requestId = Sony.NP.Trophies.GetUnlockedTrophies(request, response);
                MessageBox.Show("GetUnlockedTrophies Async: Request Id = " + requestId);
                //if (requestId != 0)
                //{
                //    return response.TrophyIds.ToList();
                //}
                //return null;
            }
            catch (Sony.NP.NpToolkitException e)
            {
                MessageBox.Show("Exception : " + e.ExtendedMessage);
            }
           
        }

        public static void UnlockTrophy(int TrophyId)
        {
            try
            {
                Sony.NP.Trophies.UnlockTrophyRequest request = new Sony.NP.Trophies.UnlockTrophyRequest();
                request.TrophyId = TrophyId;
                request.UserId = User.GetActiveUserId;
                request.Async = false;

                Sony.NP.Core.EmptyResponse response = new Sony.NP.Core.EmptyResponse();
                
                // Make the async call which will return the Request Id 
                int requestId = Sony.NP.Trophies.UnlockTrophy(request, response);
                //MessageBox.Show("GetUnlockedTrophies Async : Request Id = " + requestId);
            }
            catch (Sony.NP.NpToolkitException e)
            {
                if (e.ResultType == Sony.NP.APIResultTypes.Error)
                {
                    if (e.Message == "SCE_NP_TROPHY_ERROR_TROPHY_ALREADY_UNLOCKED")
                    {

                    }
                    else if(e.Message == "SCE_NP_TROPHY_ERROR_PLATINUM_CANNOT_UNLOCK")
                    {

                    }
                    else
                    {
                        MessageBox.Show("Exception : " + e.ExtendedMessage);
                    }
                }
            }
        }

        public static void GetInfo(int TrophyId)
        {
            
        }

        public static void DisplayTrophyPackDialog()
        {
            try
            {
                Sony.NP.Trophies.DisplayTrophyListDialogRequest request = new Sony.NP.Trophies.DisplayTrophyListDialogRequest();
                request.UserId = User.GetActiveUserId;

                Sony.NP.Core.EmptyResponse response = new Sony.NP.Core.EmptyResponse();

                // Make the async call which will return the Request Id 
                int requestId = Sony.NP.Trophies.DisplayTrophyListDialog(request, response);
                
                //OnScreenLog.Add("DisplayTrophyPackDialog Async : Request Id = " + requestId);
            }
            catch (Sony.NP.NpToolkitException e)
            {
                MessageBox.Show("Exception : " + e.ExtendedMessage);
            }
        }

        public static Sony.NP.Trophies.TrophyPackSummaryResponse GetTrophyPackSummary()
        {
            try
            {
                Sony.NP.Trophies.GetTrophyPackSummaryRequest request = new Sony.NP.Trophies.GetTrophyPackSummaryRequest();
                request.RetrieveTrophyPackSummaryIcon = true;
                request.UserId = User.GetActiveUserId;
                request.Async = false;

                Sony.NP.Trophies.TrophyPackSummaryResponse response = new Sony.NP.Trophies.TrophyPackSummaryResponse();

                // Make the async call which will return the Request Id 
                int requestId = Sony.NP.Trophies.GetTrophyPackSummary(request, response);
                return response;
               
            }
            catch (Sony.NP.NpToolkitException e)
            {
                MessageBox.Show("Exception : " + e.ExtendedMessage);
            }
            return null;
        }

        public static void GetTrophyPackGroup()
        {
            try
            {
                Sony.NP.Trophies.GetTrophyPackGroupRequest request = new Sony.NP.Trophies.GetTrophyPackGroupRequest();
                request.GroupId = -1;
                request.UserId = User.GetActiveUserId;

                Sony.NP.Trophies.TrophyPackGroupResponse response = new Sony.NP.Trophies.TrophyPackGroupResponse();

                // Make the async call which will return the Request Id 
                int requestId = Sony.NP.Trophies.GetTrophyPackGroup(request, response);
              //  return response;
                //OnScreenLog.Add("GetTrophyPackGroup Async : Request Id = " + requestId);
            }
            catch (Sony.NP.NpToolkitException e)
            {
                MessageBox.Show("Exception : " + e.ExtendedMessage);
            }
            //return null;
        }

        /// <summary>
        /// Returns a response with the user unlocked status
        /// </summary>
        /// <param name="trophyid"></param>
        public static Sony.NP.Trophies.TrophyPackTrophyResponse GetTrophyPackTrophy(int trophyid)
        {
            try
            {
                Sony.NP.Trophies.GetTrophyPackTrophyRequest request = new Sony.NP.Trophies.GetTrophyPackTrophyRequest();
                request.TrophyId = trophyid;
                
                request.RetrieveTrophyPackTrophyIcon = true;
                request.UserId = User.GetActiveUserId;
                request.Async = false;

                Sony.NP.Trophies.TrophyPackTrophyResponse response = new Sony.NP.Trophies.TrophyPackTrophyResponse();
                // Make the async call which will return the Request Id 
                int requestId = Sony.NP.Trophies.GetTrophyPackTrophy(request, response);

                return response;
                //OnScreenLog.Add("GetTrophyPackTrophy Async : Request Id = " + requestId);
            }
            catch (Sony.NP.NpToolkitException e)
            {
                if (e.Message == Sony.NP.Core.ReturnCodes.NP_TROPHY_ERROR_TROPHY_NOT_UNLOCKED.ToString())
                {

                }
                else
                {
                    MessageBox.Show("Exception : " + e.ExtendedMessage);
                }
            }
            return null;
        }


        public static void SetScreenshot()
        {
            try
            {
                Sony.NP.Trophies.SetScreenshotRequest request = new Sony.NP.Trophies.SetScreenshotRequest();
                request.AssignToAllUsers = true;
                Sony.NP.Core.UserServiceUserId id = new Sony.NP.Core.UserServiceUserId();
                int Id = 0;
                int.TryParse(MainClass.Get_UserId(), out Id);
                id.Id = Id;


                request.UserId = id;

                int[] ids = new int[4];

                for (int i = 0; i < ids.Length; i++)
                {
                    ids[i] = i + 1;  // Set throphy ids from 1 to 4.
                }

                request.TrophiesIds = ids;

                Sony.NP.Core.EmptyResponse response = new Sony.NP.Core.EmptyResponse();

                // Make the async call which will return the Request Id 
                int requestId = Sony.NP.Trophies.SetScreenshot(request, response);
                
                MessageBox.Show("SetScreenshot Async : Request Id = " + requestId);
            }
            catch (Sony.NP.NpToolkitException e)
            {
                MessageBox.Show("Exception : " + e.ExtendedMessage);
            }
        }

        //Update the Main NPToolkit

        //void Update()
        //{

        //    Sony.NP.Main.Update();

        //}


    }
}
