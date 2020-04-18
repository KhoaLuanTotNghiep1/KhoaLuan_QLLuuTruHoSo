using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace S3Train.WebHeThong.Models
{
    public class TuViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Tên")]
        public string Ten { get; set; }

        [Required]
        [Display(Name = "Vị Trí")]
        public string ViTri { get; set; }

        [Required]
        [Display(Name = "Diện Tích")]
        public string DienTich { get; set; }

        [Required]
        [Display(Name = "Người Quản Lý")]
        public string NgươiQuanLy { get; set; }

        [Required]
        [Display(Name = "Đơn Vị Tính")]
        public string DonViTinh { get; set; }

        [Display(Name = "Số Lượng Hiện Tại")]
        public int SoLuongHienTai { get; set; }

        [Required]
        [Display(Name = "Sức Chứa")]
        public int SoLuongMax { get; set; }

        [Required]
        [Display(Name = "Tình Trạng")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public string TinhTrang { get; set; }

        [Display(Name = "Ngày Tạo")]
        public DateTime NgayTao { get; set; }

        [Display(Name = " Ngày Cập Nhật")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? NgayCapNhat { get; set; }

        [Display(Name = "Trạng Thái")]
        public bool TrangThai { get; set; }

        public virtual ICollection<Ke> Kes { get; set; }
    }
}