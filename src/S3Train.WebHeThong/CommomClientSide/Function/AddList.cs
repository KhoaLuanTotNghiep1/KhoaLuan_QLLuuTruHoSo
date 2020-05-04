using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S3Train.WebHeThong.CommomClientSide.Function
{
    public static class AddList
    {
        public static List<string> AddItemByArray(string[] array)
        {
            var list = new List<string>();
            list.AddRange(array);
            return list;
        }
    }
}