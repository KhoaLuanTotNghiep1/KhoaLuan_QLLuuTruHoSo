using S3Train.Domain;
using S3Train.WebHeThong.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using X.PagedList;


namespace S3Train.WebHeThong.Controllers
{
    public class ChiTietMuonTraViewModel
    {
        public string Id { get; set; }

        public string IdThuMuon { get; set; }

        [Required(ErrorMessage = "Điền Ngày Tạo")]
        [Display(Name = "Ngày Tao")]
        public DateTime NgayTao { get; set; }

        [Display(Name = " Ngày Cập Nhật")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? NgayCapNhat { get; set; }

        [Display(Name = "Trạng Thái")]
        public bool TrangThai { get; set; }

        public virtual MuonTra MuonTra { get; set; }
        public virtual TaiLieuVanBan TaiLieuVanBan { get; set; }

    }
}