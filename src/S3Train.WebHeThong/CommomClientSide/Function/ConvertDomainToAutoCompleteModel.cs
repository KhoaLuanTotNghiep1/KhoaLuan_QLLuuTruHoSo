using S3Train.Domain;
using S3Train.WebHeThong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace S3Train.WebHeThong.CommomClientSide.Function
{
    public static class ConvertDomainToAutoCompleteModel
    {
        public static List<AutoCompleteTextModel> LocalTaiLieu(IList<HoSo> hoSos)
        {
            var list = new List<AutoCompleteTextModel>();

            foreach(var item in hoSos)
            {
                string local = item.Hop.Ke.Tu.Ten + " kệ thứ " + item.Hop.Ke.SoThuTu + " hộp số " + item.Hop.SoHop + " hồ sơ " + item.PhongLuuTru;

                var auto = new AutoCompleteTextModel()
                {
                    Id = item.Id,
                    Text = local
                };

                list.Add(auto);
            }

            return list;
        }

        public static List<AutoCompleteTextModel> LocalHoSo(IList<HoSo> hoSos, IList<TaiLieuVanBan> taiLieuVanBans)
        {
            var list = new List<AutoCompleteTextModel>();
            foreach (var item in hoSos)
            {
                string local = item.PhongLuuTru;
                

                var auto = new AutoCompleteTextModel()
                {
                    Id = item.Id,
                    Text = local,
                };

                list.Add(auto);
            }

            foreach (var item in taiLieuVanBans)
            {
                string local = item.Ten;


                var auto = new AutoCompleteTextModel()
                {
                    Id = item.Id,
                    Text = local,
                };

                list.Add(auto);
            }

            return list;
        }

        public static List<AutoCompleteTextModel> LocalUser(IList<ApplicationUser> users)
        {
            var list = new List<AutoCompleteTextModel>();

            foreach (var item in users)
            {
                string local = item.FullName;

                var auto = new AutoCompleteTextModel()
                {
                    Id = item.Id,
                    Text = local
                };

                list.Add(auto);
            }

            return list;
        }

        public static List<AutoCompleteTextModel> LocalHop(IList<Ke> kes)
        {
            var list = new List<AutoCompleteTextModel>();

            foreach (var item in kes)
            {
                string local = item.Tu.Ten + " kệ thứ " + item.SoThuTu;

                var auto = new AutoCompleteTextModel()
                {
                    Id = item.Id,
                    Text = local
                };

                list.Add(auto);
            }

            return list;
        }

        public static List<AutoCompleteTextModel> LocalHoSo(IList<Hop> hops)
        {
            var list = new List<AutoCompleteTextModel>();

            foreach (var item in hops)
            {
                string local = item.Ke.Tu.Ten + " kệ thứ " + item.Ke.SoThuTu + " hộp số " + item.SoHop;

                var auto = new AutoCompleteTextModel()
                {
                    Id = item.Id,
                    Text = local
                };

                list.Add(auto);
            }

            return list;
        }

    }
}