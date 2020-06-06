using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using X.PagedList;

namespace S3Train.WebHeThong.Models
{
    public class KeViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Điền Tên Kệ")]
        [Display(Name = "Tên Kệ")]
        public string Ten { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Điền Tên Kệ")]
        [Display(Name = "Số Thứ Tự")]
        public int SoThuTu { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Điền Tên Số Hộp Tối Đa")]
        [Display(Name = "Số Hộp Tối Đa")]
        public int SoHopToiDa { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Điền Số Hộp Hiện Tại")]
        [Display(Name = "Số Hộp Hiện Tại")]
        public int SoHopHienTai { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Điền Năm Bắt Đầu Lưu trữ")]
        [Display(Name = "Năm Bắt Đầu Lưu Trữ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime NamBatDau { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Điền Năm Kết Thúc Lưu Trữ")]
        [DataType(DataType.Date)]
        [Display(Name = "Năm Kết Thúc Lưu Trữ")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime NamKetThuc { get; set; }

        [Display(Name = "Tình Trạng")]
        public string TinhTrang { get; set; }

        public string UserId { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Chọn Tủ")]
        [Display(Name = "Tủ")]
        public string Tuid { get; set; }

        [Display(Name = "Ngày Tạo")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime NgayTao { get; set; }

        [Display(Name = " Ngày Cập Nhật")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? NgayCapNhat { get; set; }

        [Display(Name = "Trạng Thái")]
        public bool TrangThai { get; set; }

        public ApplicationUser User { get; set; }
        public Tu Tu { get; set; }
        public ICollection<Hop> Hops { get; set; }
    }

    public class KeViewIndexModel : IndexViewModelBase
    {
        public IPagedList<Ke> Paged { get; set; }
        public List<KeViewModel> Items { get; set; }
    }
}