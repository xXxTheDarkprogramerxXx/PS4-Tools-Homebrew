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
}
