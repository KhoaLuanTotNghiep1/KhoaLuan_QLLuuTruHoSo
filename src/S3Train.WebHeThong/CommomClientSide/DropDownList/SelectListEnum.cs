using S3Train.Core.Extension;
using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S3Train.WebHeThong.CommomClientSide.DropDownList
{
    public static class SelectListEnum
    {
        public static List<SelectListItem> SelectListItem_DangVanBan()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in Enum.GetValues(typeof(EnumDangVanBan)))
            {
                items.Add(new SelectListItem { Text = item.GetDecription(), Value = ((int)item).ToString() });
            }
            return items;
        }

    }
}