using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S3Train.WebHeThong.CommomClientSide.Function
{
    public static class Messenger
    {
        public static string ChangeActiveMessenge(bool active)
        {
            if (!active)
                return "Ngừng Hoạt Động Thành Công";
            else
                return "Khôi Phục Hoạt Động Thành Công";
        }

    }
}