using S3Train.Domain;
using S3Train.WebHeThong.Models;
using System.Collections.Generic;



namespace S3Train.WebHeThong.CommomClientSide.Function
{
    public static class ConvertDomainToAutoCompleteModel
    {
        public static HashSet<AutoCompleteTextModel> LocalTaiLieu(IEnumerable<HoSo> hoSos)
        {
            var list = new HashSet<AutoCompleteTextModel>();

            foreach(var item in hoSos)
            {
                string local = item.Hop.Ke.Tu.Ten + " kệ " + item.Hop.Ke.Ten + " hộp số " + item.Hop.SoHop + " hồ sơ " + item.PhongLuuTru;

                var auto = new AutoCompleteTextModel()
                {
                    Id = item.Id,
                    Text = local
                };

                list.Add(auto);
            }

            return list;
        }

        public static HashSet<AutoCompleteTextModel> LocalVanBan(IList<TaiLieuVanBan> taiLieuVanBans)
        {
            var list = new HashSet<AutoCompleteTextModel>();

            foreach (var item in taiLieuVanBans)
            {
                if(item.TinhTrang != EnumTinhTrang.DangMuon && item.TinhTrang != EnumTinhTrang.DaGoi)
                {
                    var auto = new AutoCompleteTextModel()
                    {
                        Id = item.Id,
                        Text = item.Ten,
                    };
                    list.Add(auto);
                }
               
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

        public static HashSet<AutoCompleteTextModel> LocalHop(IEnumerable<Ke> kes)
        {
            var list = new HashSet<AutoCompleteTextModel>();

            foreach (var item in kes)
            {
                string local = item.Tu.Ten + " " + item.Ten;

                var auto = new AutoCompleteTextModel()
                {
                    Id = item.Id,
                    Text = local
                };

                list.Add(auto);
            }

            return list;
        }

        public static HashSet<AutoCompleteTextModel> LocalHoSo(IEnumerable<Hop> hops)
        {
            var list = new HashSet<AutoCompleteTextModel>();

            foreach (var item in hops)
            {
                string local = item.Ke.Tu.Ten + " kệ " + item.Ke.Ten + " hộp số " + item.SoHop;

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