using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using X.PagedList;

namespace S3Train.WebHeThong.Models
{
    public class HopViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền tên chuyên đề")]
        [Display(Name = "Tên Chuyên Đề")]
        public string ChuyenDe { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền số hộp")]
        [Display(Name = "Số Hộp")]
        public int SoHop { get; set; }

        [Display(Name = "Tình Trạng")]
        public EnumTinhTrang TinhTrang { get; set; } // Mượn hoặc Trả

        [Required(ErrorMessage = "Bạn chưa điền ngày bắt đầu lưu trữ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Ngày Bắt Đầu Lưu Trữ")]
        public DateTime NgayBatDau { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền ngày kết thúc lưu trữ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Ngày Kết Thúc Lưu Trữ")]
        public DateTime NgayKetThuc { get; set; }

        [Required(ErrorMessage = "Bạn chưa chọn kệ")]
        [Display(Name = "Kệ")]
        public string KeId { get; set; }

        [Required(ErrorMessage = "Bạn chưa chọn phòng ban")]
        [Display(Name = "Phòng Ban")]
        public string PhongBanId { get; set; }

        [Display(Name = "Người Tạo")]
        public string UserId { get; set; }

        [Display(Name = "Ngày Tạo")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime NgayTao { get; set; }

        [Display(Name = " Ngày Cập Nhật")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? NgayCapNhat { get; set; }

        [Display(Name = "Trạng Thái")]
        public bool TrangThai { get; set; }

        public ApplicationUser User { get; set; }
        public Ke Ke { get; set; }
        public PhongBan PhongBan { get; set; }
        public ICollection<HoSo> HoSos { get; set; }
    }

    public class HopViewIndexModel : IndexViewModelBase
    {
        public IPagedList<Hop> Paged { get; set; }
        public List<HopViewModel> Items { get; set; }
    }
}