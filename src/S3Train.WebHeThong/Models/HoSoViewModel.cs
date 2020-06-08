using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using X.PagedList;

namespace S3Train.WebHeThong.Models
{
    public class HoSoViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Tập Hồ Sơ")]
        public string TapHoSoId { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền phông lưu trữ")]
        [Display(Name = "Phông Lưu Trữ")]
        public string PhongLuuTru { get; set; }

        [Display(Name = "Tình Trạng Lưu Trữ")]
        public EnumTinhTrang TinhTrang { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền thời gian bảo quản")]
        [Display(Name = "Thời Gian Bảo Quản")]
        public int ThoiGianBaoQuan { get; set; }

        [Display(Name = "Ghi Chú")]
        public string GhiChu { get; set; }

        [Display(Name = "Biên Mục Hồ Sơ")]
        public string BienMucHoSo { get; set; }

        [Required(ErrorMessage = "Bạn chưa chọn hộp lưu trữ")]
        [Display(Name = "Hộp Lưu Trữ")]
        public string HopId { get; set; }

        [Required(ErrorMessage = "Bạn chưa chọn loại hồ sơ")]
        [Display(Name = "Loại Hồ Sơ")]
        public string LoaiHoSoId { get; set; }

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
        public Hop Hop { get; set; }
        public HoSo TapHoSo { get; set; }
        public LoaiHoSo LoaiHoSo { get; set; }
        public ICollection<HoSo> HoSoCons { get; set; }
        public ICollection<TaiLieuVanBan> TaiLieuVanBans { get; set; }
    }

    public class HoSoIndexViewModel : IndexViewModelBase
    {
        public IPagedList<HoSo> Paged { get; set; }
        public List<HoSoViewModel> Items { get; set; }
    }
}