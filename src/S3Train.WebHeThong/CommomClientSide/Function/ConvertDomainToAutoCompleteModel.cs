using S3Train.Domain;
using S3Train.WebHeThong.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;



namespace S3Train.WebHeThong.CommomClientSide.Function
{
    public static class ConvertDomainToAutoCompleteModel
    {
        public static HashSet<AutoCompleteTextModel> LocalTaiLieu(IList<HoSo> hoSos)
        {
            var list = new HashSet<AutoCompleteTextModel>();

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

        public static HashSet<AutoCompleteTextModel> LocalHoSo(IList<HoSo> hoSos, IList<TaiLieuVanBan> taiLieuVanBans)
        {
            var list = new HashSet<AutoCompleteTextModel>();
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

        public static HashSet<AutoCompleteTextModel> LocalUser(IList<ApplicationUser> users)
        {
            var list = new HashSet<AutoCompleteTextModel>();

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

       

        public static HashSet<AutoCompleteTextModel> LocalHop(IList<Ke> kes)
        {
            var list = new HashSet<AutoCompleteTextModel>();

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

        public static HashSet<AutoCompleteTextModel> LocalHoSo(IList<Hop> hops)
        {
            var list = new HashSet<AutoCompleteTextModel>();

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