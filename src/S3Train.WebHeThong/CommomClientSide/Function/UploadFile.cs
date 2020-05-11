using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace S3Train.WebHeThong.CommomClientSide.Function
{
    public static class UploadFile
    {
        public static string UpFileAndGetFileName(HttpPostedFileBase a, string local)
        {
            string fileName = "";
            if (a != null && a.ContentLength > 0)
            {
                fileName = Path.GetFileName(a.FileName).ToString();
                string path = Path.Combine(local, fileName);
                a.SaveAs(path);

                return fileName;
            }
            else
            {
                return fileName;
            }
        }
    }
}