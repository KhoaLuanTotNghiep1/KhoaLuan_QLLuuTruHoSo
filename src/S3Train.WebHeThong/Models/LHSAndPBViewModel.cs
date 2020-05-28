using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using X.PagedList;

namespace S3Train.WebHeThong.Models
{
    public class LHSAndPBViewModel
    {
        public LoaiHoSoVIndexiewModel LHS { get; set; }
        public PhongBanVIndexiewModel PB { get; set; }
    }

    public class LoaiHoSoViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền mã loại hồ sơ")]
        [Display(Name = "Mã Loại Hồ Sơ")]
        public string Ma { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền Tên loại hồ sơ")]
        [Display(Name = "Tên Loại Hồ Sơ")]
        public string Ten { get; set; }

        [Display(Name = "Ngày Tạo")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime NgayTao { get; set; }

        [Display(Name = " Ngày Cập Nhật")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? NgayCapNhat { get; set; }

        [Display(Name = "Trạng Thái")]
        public bool TrangThai { get; set; }

        public ICollection<HoSo> HoSos { get; set; }
    }

    public class PhongBanViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Bạn chưa điền tên phòng ban")]
        [Display(Name = "Tên Phòng Ban")]
        public string Ten { get; set; }

        [Display(Name = "Ngày Tạo")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime NgayTao { get; set; }

        [Display(Name = " Ngày Cập Nhật")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? NgayCapNhat { get; set; }

        [Display(Name = "Trạng Thái")]
        public bool TrangThai { get; set; }

        public ICollection<Hop> Hops { get; set; }
    }

    public class LoaiHoSoVIndexiewModel : IndexViewModelBase
    {
        public IPagedList<LoaiHoSo> Paged { get; set; }
        public List<LoaiHoSoViewModel> Items { get; set; }
    }

    public class PhongBanVIndexiewModel : IndexViewModelBase
    {
        public IPagedList<PhongBan> Paged { get; set; }
        public List<PhongBanViewModel> Items { get; set; }
    }
}