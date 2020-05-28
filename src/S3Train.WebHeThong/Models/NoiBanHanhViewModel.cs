using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using X.PagedList;

namespace S3Train.WebHeThong.Models
{
    public class NoiBanHanhViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage ="Bạn chưa điền tên nơi ban hành")]
        [Display(Name = "Tên Nơi Ban Hành")]
        public string Ten { get; set; }

        [Display(Name = "Mô Tả")]
        public string MoTa { get; set; }

        [Display(Name = "Ngày Tạo")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime NgayTao { get; set; }

        [Display(Name = " Ngày Cập Nhật")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? NgayCapNhat { get; set; }

        [Display(Name = "Trạng Thái")]
        public bool TrangThai { get; set; }

        public ICollection<TaiLieuVanBan> TaiLieuVanBans { get; set; }
    }

    public class NoiBanHanhIndexViewModel : IndexViewModelBase
    {
        public IPagedList<NoiBanHanh> Paged { get; set; }
        public List<NoiBanHanhViewModel> Items { get; set; }
    }
}