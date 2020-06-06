using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace S3Train.WebHeThong.Models
{
    public class TuViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Điền Tên")]
        [Display(Name = "Tên")]
        public string Ten { get; set; }

        [Required(ErrorMessage = "Điền Vị Trí")]
        [Display(Name = "Vị Trí")]
        public string ViTri { get; set; }

        [Required(ErrorMessage = "Điền Tên Người Quản Lý")]
        [Display(Name = "Người Quản Lý")]
        public string NgươiQuanLy { get; set; }

        [Required(ErrorMessage = "Điền Đơn Vị Tính")]
        [Display(Name = "Đơn Vị Tính")]
        public string DonViTinh { get; set; }

        [Display(Name = "Số Lượng Hiện Tại")]
        public int SoLuongHienTai { get; set; }

        [Required(ErrorMessage = "Điền Sức Chứa")]
        [Display(Name = "Sức Chứa")]
        public int SoLuongMax { get; set; }

        [Display(Name = "Tình Trạng Tủ Lưu Trữ")]
        public string TinhTrang { get; set; }

        [Display(Name = "Ngày Tạo")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime NgayTao { get; set; }

        [Display(Name = " Ngày Cập Nhật")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? NgayCapNhat { get; set; }

        [Display(Name = "Trạng Thái")]
        public bool TrangThai { get; set; }

        public ICollection<Ke> Kes { get; set; }
    }

    public class TuIndexViewModel : IndexViewModelBase
    {
        public IPagedList<Tu> Paged { get; set; }
        public List<TuViewModel> Items { get; set; }
    }

    public class LichSuHoatDongIndexViewModel : IndexViewModelBase
    {
        public IPagedList<LichSuHoatDong> Paged { get; set; }
        public List<LichSuHoatDong> Items { get; set; }
    }
}