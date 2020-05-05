using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace S3Train.WebHeThong.Models
{
    public class MuonTraViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        [Required(ErrorMessage = "Điền Tên")]
        [Display(Name = "Đơn vị mượn/Người mượn")]
        public string Ten { get; set; }

        [Required(ErrorMessage = "Điền Ngày Mượn")]
        [Display(Name = "Ngày Mượn")]
        public DateTime NgayMuon { get; set; }

        [Required(ErrorMessage = "Điền Ngày Trả")]
        [Display(Name = "Ngày Trả")]
        public DateTime NgayTra { get; set; }

        [Required(ErrorMessage = "Điền Tên Văn Thư")]
        [Display(Name = "Văn Thư")]
        public string VanThu { get; set; }


        [Display(Name = "Tình Trạng")]
        public string TinhTrang { get; set; }

        [Display(Name = " Ngày Cập Nhật")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? NgayCapNhat { get; set; }

        [Display(Name = "Trạng Thái")]
        public bool TrangThai { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<ChiTietMuonTra> ChiTietMuonTras { get; set; }
    }

    public class MuonTraIndexViewModel : IndexViewModelBase
    {
        public IPagedList<MuonTra> Paged { get; set; }
        public List<MuonTraViewModel> Items { get; set; }
    }

}