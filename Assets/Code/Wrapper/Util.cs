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
}
