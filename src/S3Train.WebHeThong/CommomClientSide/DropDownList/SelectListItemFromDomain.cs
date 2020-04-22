using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S3Train.WebHeThong.CommomClientSide.DropDownList
{
    public class SelectListItemFromDomain
    {

        /// <summary>
        /// format Tu from domain to SelectListItem
        /// </summary>
        /// <param name="tus">list Tu</param>
        /// <returns>Select List</returns>
        public static List<SelectListItem> SelectListItem_Tu(IList<Tu> tus)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in tus)
            {
                items.Add(new SelectListItem { Text = item.Ten, Value = item.Id });
            }
            return items;
        }

        /// <summary>
        /// format PhongBan from domain to SelectListItem
        /// </summary>
        /// <param name="phongBans">list PhongBans</param>
        /// <returns>Select List</returns>
        public static List<SelectListItem> SelectListItem_PhongBan(IList<PhongBan> phongBans)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in phongBans)
            {
                items.Add(new SelectListItem { Text = item.Ten, Value = item.Id });
            }
            return items;
        }

        /// <summary>
        /// format Ke from domain to SelectListItem
        /// </summary>
        /// <param name="kes">list kes</param>
        /// <returns>Select List</returns>
        public static List<SelectListItem> SelectListItem_Ke(IList<Ke> kes)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in kes)
            {
                string viTri = item.Tu.Ten + " Kệ Thứ " + item.SoThuTu.ToString();
                items.Add(new SelectListItem { Text = viTri, Value = item.Id });
            }
            return items;
        }

        public static List<SelectListItem> SelectListItem_TinhTrangLuuTru()
        {
            var items = new List<SelectListItem>
            {
                new SelectListItem{Value = "Trong Kho", Text = "Trong Kho"},
                new SelectListItem{Value = "Đang Mượn", Text = "Đang Mượn"},
            };
            return items;
        }
    }
}