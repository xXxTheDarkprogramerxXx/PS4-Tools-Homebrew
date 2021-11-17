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
