using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S3Train.WebHeThong.DTOs
{
    public class TaiLieuVanBanDTO
    {
        public string Id { get; set; }
        public string TenTLVB { get; set; }
        public HoSoDTO HoSo { get; set; }
    }
}