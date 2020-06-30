using S3Train.Domain;
using S3Train.WebHeThong.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using X.PagedList;


namespace S3Train.WebHeThong.Models
{
    public class ChiTietMuonTraViewModel
    {
        public string Id { get; set; }

        public string TaiLieuVanBanId { get; set; }
        public string MuonTraId { get; set; }
        public int SoLuong { get; set; }

        [Required(ErrorMessage = "Điền Ngày Tạo")]
        [Display(Name = "Ngày Tao")]
        public DateTime NgayTao { get; set; }

        [Display(Name = " Ngày Cập Nhật")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? NgayCapNhat { get; set; }

        [Display(Name = "Trạng Thái")]
        public bool TrangThai { get; set; }

        public bool Checkbox { get; set; }
        public string ViTri { get; set; }
        public ApplicationUser User { get; set; }
        public MuonTra MuonTra { get; set; }

        public TaiLieuVanBan TaiLieuVanBan { get; set; }
        
    }
    
}