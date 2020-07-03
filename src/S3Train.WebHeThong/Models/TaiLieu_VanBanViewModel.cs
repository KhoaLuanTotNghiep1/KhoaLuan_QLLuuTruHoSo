using AlgorithmLibrary.Kmeans;
using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using X.PagedList;

namespace S3Train.WebHeThong.Models
{
    public class TaiLieu_VanBanViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền tên")]
        [Display(Name = "Tên")]
        public string Ten { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Chọn Loại")]
        [Display(Name = "Loại")]
        public string Loai { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Chọn Dạng ")]
        [Display(Name = "Dạng")]
        public string Dang { get; set; } // công văn đến/đi/nội bộ

        [Required(ErrorMessage = "Bạn Chưa điền số ký hiệu")]
        [Display(Name = "Số Ký Hiệu")]
        public string SoKyHieu { get; set; }

        [Required(ErrorMessage = "Bạn Chưa điền số tờ")]
        [Display(Name = "Số Trang")]
        public int SoTo { get; set; }

        [Display(Name = "Tình Trạng Lưu Trữ")]
        public EnumTinhTrang TinhTrang { get; set; } // còn trong kho hay đã cho mượn

        [Display(Name = "Đương Dẫn File")]
        public string DuongDan { get; set; }

        [Display(Name = "Ghi Chú")]
        public string GhiChu { get; set; }

        [Display(Name = "Nội Dung")]
        public string NoiDung { get; set; }

        [Required(ErrorMessage = "Bạn Chưa điền nơi nhận")]
        [Display(Name = "Địa Chỉ Nhận Tài Liệu/Văn Bản")]
        public string NoiNhan { get; set; }

        public string NguoiGuiHoacNhan { get; set; }

        [Required(ErrorMessage = "Bạn Chưa điền người ký nhận")]
        [Display(Name = "Người Ký Nhận")]
        public string NguoiKy { get; set; }

        [Required(ErrorMessage = "Bạn Chưa điền người duyệt")]
        [Display(Name = "Người Duyệt")]
        public string NguoiDuyet { get; set; }

        [Display(Name = "Người Tạo")]
        public string UserId { get; set; }

        [Display(Name = "Hình Ảnh")]
        public string HinhAnh { get; set; }

        [Required(ErrorMessage = "Bạn Chưa chọn năm ban hành")]
        [Display(Name = "Năm Ban Hành")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime NgayBanHanh { get; set; }

        [Display(Name = "Ngày Tạo")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime NgayTao { get; set; }

        [Display(Name = " Ngày Cập Nhật")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? NgayCapNhat { get; set; }

        [Display(Name = "Trạng Thái")]
        public bool TrangThai { get; set; }

        [Required(ErrorMessage = "Bạn Chưa chọn hồ sơ lưu trữ")]
        [Display(Name = "Hồ Sơ")]
        public string HoSoId { get; set; }

        [Required(ErrorMessage = "Bạn Chưa chọn nơi ban hành")]
        [Display(Name = "Nơi Ban Hành")]
        public string NoiBanHanhId { get; set; }

        public ApplicationUser User { get; set; }
        public HoSo HoSo { get; set; }
        public NoiBanHanh NoiBanHanh { get; set; }
    }

    public class TaiLieuVanBanIndexViewModel : IndexViewModelBase
    {
        public IPagedList<TaiLieuVanBan> Paged { get; set; }
        public List<TaiLieu_VanBanViewModel> Items { get; set; }
    }

    public class TestAlgorithmModel
    {
        public int Cluster { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string DocumentNear { get; set; }
        public List<Centroid> Centroids { get; set; }
    }
}