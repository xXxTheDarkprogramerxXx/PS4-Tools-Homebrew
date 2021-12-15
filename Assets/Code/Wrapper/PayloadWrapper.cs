using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Assets.Code.Wrapper
{
  

    public static class PayloadWrapper
    {
        [DllImport("universal")]
        private static extern bool LoadExec(string path, string arg);
        //Untill we get a working wrapper i will need to do this
        public static void LaunchPs4Debug()
        {
            LoadExec("/app0/ps4debug.bin", null);
        }

        //LoadExec(const char* path, char* const * argv)
    }
}
