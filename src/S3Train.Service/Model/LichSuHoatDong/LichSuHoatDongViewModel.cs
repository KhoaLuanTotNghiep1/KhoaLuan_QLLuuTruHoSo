using S3Train.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace S3Train.Model.LichSuHoatDong
{
    public class LichSuHoatDongViewModel
    {
        public string Id { get; set; }
        public string HoatDong { get; set; }
        public string ChiTietHoatDong { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime NgayTao { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
