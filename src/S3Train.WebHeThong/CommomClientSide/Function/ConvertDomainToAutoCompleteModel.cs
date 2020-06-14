using S3Train.Domain;
using S3Train.WebHeThong.Models;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using S3Train.Contract;
using S3Train.Core.Constant;
using S3Train.Core.Extension;
using S3Train.WebHeThong.CommomClientSide.Function;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;



namespace S3Train.WebHeThong.CommomClientSide.Function
{
    public static class ConvertDomainToAutoCompleteModel
    {
        public static HashSet<AutoCompleteTextModel> LocalTaiLieu(IList<HoSo> hoSos)
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
                string local = item.HoSo.Hop.Ke.Tu.Ten + " kệ " + item.HoSo.Hop.Ke.Ten + " hộp số " + item.HoSo.Hop.SoHop + " hồ sơ " + item.HoSo.PhongLuuTru;
                var auto = new AutoCompleteTextModel()
                {
                    Id = item.Id,
                    Text = item.Ten,
                    TinhTrang = item.TinhTrang,
                    ViTri = local,
                };
                list.Add(auto);


            }
            return list;
        }

        public static HashSet<AutoCompleteTextModel> LocalUser(IList<ApplicationUser> users, IList<MuonTra> muonTras)
        {
            var list = new HashSet<AutoCompleteTextModel>();
            var sl = 0;
            foreach (var item in users)
            {
                foreach(var muon in muonTras)
                {
                    if(item.Id == muon.UserId && muon.TinhTrang == EnumTinhTrang.DangMuon)
                    {
                        sl = sl + muon.SoLuong;
                    }

                }
                var auto = new AutoCompleteTextModel()
                {
                    Id = item.Id,
                    Text = item.FullName,
                    SoLuongMuon = sl,
                    UserName = item.UserName,
                };
                list.Add(auto);
                sl = 0;
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