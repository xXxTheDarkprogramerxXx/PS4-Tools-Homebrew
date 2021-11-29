using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code
{
    public class MessageBox
    {
        public static void Show(string Message)
        {
            Wrapper.Util.ShowMessageDialog(Message);
        }

        public static void Close()
        {
            Wrapper.Util.HideDialog();
        }
    }


    public class YesNoDialog
    {

        public enum YesNoRessult
        {
            Yes,
            No,
            Cancled,
        }

        /// <summary>
        /// returns True when user accepts
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static YesNoRessult Show(string Message)
        {

            int rtn = Wrapper.Util.ShowMessageYesNoDialog(Message);
            if (rtn == 1)
            {
                //user accapted
                return YesNoRessult.Yes;
            }
            return YesNoRessult.No;
        }

        public static void Close()
        {
            Wrapper.Util.HideDialog();
        }
    }

    public class LoadingDialog
    {
        public static void Show(string Message)
        {
            Wrapper.Util.ShowLoadingDialog(Message);
        }

        public static void Close()
        {
            Wrapper.Util.HideDialog();
        }
    }

}
