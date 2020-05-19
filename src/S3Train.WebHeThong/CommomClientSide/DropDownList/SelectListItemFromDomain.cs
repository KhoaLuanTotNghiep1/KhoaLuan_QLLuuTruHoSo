using S3Train.Core.Constant;
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

        /// <summary>
        /// format LoaiHoSo from domain to SelectListItem
        /// </summary>
        /// <param name="loaiHoSos">list LoaiHoSo</param>
        /// <returns>Select List</returns>
        public static List<SelectListItem> SelectListItem_LoaiHoSo(IList<LoaiHoSo> loaiHoSos)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in loaiHoSos)
            {
                items.Add(new SelectListItem { Text = item.Ten, Value = item.Id });
            }
            return items;
        }

        /// <summary>
        /// format HoSo from domain to SelectListItem
        /// </summary>
        /// <param name="hoSos">list HoSo</param>
        /// <returns>Select List</returns>
        public static List<SelectListItem> SelectListItem_HoSo(IList<HoSo> hoSos)
        {
            List<SelectListItem> items = new List<SelectListItem>()
            {
                new SelectListItem { Text = "", Value = "" }
            };
            foreach (var item in hoSos)
            {
                items.Add(new SelectListItem { Text = item.PhongLuuTru, Value = item.Id });
            }
            return items;
        }


        /// <summary>
        /// format Hop from domain to SelectListItem
        /// </summary>
        /// <param name="taiLieuVanBans">list Hop</param>
        /// <returns>Select List</returns>
        public static List<SelectListItem> SelectListItem_VanBanTaiLieu(IList<TaiLieuVanBan> taiLieuVanBans)
        {
            List<SelectListItem> items = new List<SelectListItem>()
            {
                new SelectListItem { Text = "", Value = "" }
            };
            foreach (var item in taiLieuVanBans)
            {
                items.Add(new SelectListItem { Text = item.Ten, Value = item.Id });
            }
            return items;
        }


        /// <summary>
        /// format Hop from domain to SelectListItem
        /// </summary>
        /// <param name="users">list Hop</param>
        /// <returns>Select List</returns>
        public static List<SelectListItem> SelectListItem_User(IList<ApplicationUser> users)
        {
            List<SelectListItem> items = new List<SelectListItem>()
            {
                new SelectListItem { Text = "", Value = "" }
            };
            foreach (var item in users)
            {
                items.Add(new SelectListItem { Text = item.FullName, Value = item.Id });
            }
            return items;
        }

        /// <summary>
        /// format Hop from domain to SelectListItem
        /// </summary>
        /// <param name="hops">list Hop</param>
        /// <returns>Select List</returns>
        public static List<SelectListItem> SelectListItem_Hop(IList<Hop> hops)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in hops)
            {
                string vt = item.Ke.Tu.Ten +" kệ thứ "+item.Ke.SoThuTu + " hộp số " + item.SoHop;
                items.Add(new SelectListItem { Text = vt, Value = item.Id });
            }
            return items;
        }

        /// <summary>
        /// format Hop from domain to SelectListItem
        /// </summary>
        /// <param name="hops">list Hop</param>
        /// <returns>Select List</returns>
        public static List<SelectListItem> SelectListItem_NoiBanHanh(IList<NoiBanHanh> noiBanHanhs)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in noiBanHanhs)
            {
                items.Add(new SelectListItem { Text = item.Ten, Value = item.Id });
            }
            return items;
        }

        public static List<SelectListItem> SelectListItem_Object(List<object> list)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in list)
            {
                items.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }
            return items;
        }
    }
}