using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Assets.Code
{
    public static class DataRowExtensions
    {
        public static T GetValue<T>(this DataRow row, string fieldName)
        {
            if (row.IsNull(fieldName))
            {
                return default(T);
            }

            var value = row[fieldName];
            if (value == DBNull.Value)
            {
                return default(T);
            }
            

            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(value.ToString(), typeof(T));
            }

            return (T)Convert.ChangeType((T)value, typeof(T));
        }
    }

    public static class StringExtensions
    {

        public static int FindBytes(byte[] src, byte[] find)
        {
            int index = -1;
            int matchIndex = 0;
            // handle the complete source array
            for (int i = 0; i < src.Length; i++)
            {
                if (src[i] == find[matchIndex])
                {
                    if (matchIndex == (find.Length - 1))
                    {
                        index = i - matchIndex;
                        break;
                    }
                    matchIndex++;
                }
                else if (src[i] == find[0])
                {
                    matchIndex = 1;
                }
                else
                {
                    matchIndex = 0;
                }

            }
            return index;
        }
        public static byte[] ReplaceBytes(byte[] src, byte[] search, byte[] repl)
        {
            byte[] dst = null;
            int index = FindBytes(src, search);
            if (index >= 0)
            {
                dst = new byte[src.Length - search.Length + repl.Length];
                // before found array
                Buffer.BlockCopy(src, 0, dst, 0, index);
                // repl copy
                Buffer.BlockCopy(repl, 0, dst, index, repl.Length);
                // rest of src array
                Buffer.BlockCopy(
                    src,
                    index + search.Length,
                    dst,
                    index + repl.Length,
                    src.Length - (index + search.Length));
            }
            return dst;
        }
    }
}
