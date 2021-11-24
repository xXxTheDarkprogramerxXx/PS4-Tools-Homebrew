using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Assets.Code.Wrapper
{
    public class Util
    {
        /// <summary>
        /// Use this to hide the loading dialog 
        /// </summary>
        /// <returns></returns>
        [DllImport("universal")]
        public static extern int HideDialog();

        /// <summary>
        /// This shows a message dialog with an ok button on the screen
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        [DllImport("universal")]
        public static extern int ShowMessageDialog(string Message);

        /// <summary>
        /// If you call this you need to remember to terminate the dialogbox somehow
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        [DllImport("universal")]
        public static extern int ShowLoadingDialog(string Message);

    }


    public class TrophyUtil
    {
        /// <summary>
        /// Use this to create and register a trophy's context
        /// This can only be done once per game do not do it more than that
        /// </summary>
        /// <returns>true or false</returns>
        [DllImport("universal")]
        public static extern bool CreateAndRegister();

        /// <summary>
        /// This needs to be called each time a trophy has been reistered else the console will panic on app exit
        /// </summary>
        /// <returns></returns>
        [DllImport("universal")]
        public static extern bool DestroyAndTerminate();

        /// <summary>
        /// Unlockes a spesific trophy per context and trophy Id
        /// </summary>
        /// <param name="trophyId">The trophy's ID</param>
        /// <returns></returns>
        [DllImport("universal")]
        public static extern bool UnlockSpesificTrophy(int trophyId);
    }
}
