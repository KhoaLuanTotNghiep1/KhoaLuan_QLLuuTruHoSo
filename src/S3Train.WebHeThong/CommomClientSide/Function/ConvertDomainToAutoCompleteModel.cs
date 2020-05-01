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
        public static List<AutoCompleteTextModel> ConvertHoSo(IList<HoSo> hoSos)
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

    }
}