using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S3Train.WebHeThong.DTOs
{
    public class HoSoDTO
    {
        public string Id { get; set; }
        public string IdHop { get; set; }
        public string TenHoSo { get; set; }
        public string a { get; set; }
        public IList<TaiLieuVanBanDTO> TaiLieuVanBan { get; set; }
    }
}