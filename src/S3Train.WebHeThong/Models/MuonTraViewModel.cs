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

        [Required(ErrorMessage = "Điền Tên")]
        [Display(Name = "Đơn vị mượn/Người mượn")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Điền hồ sơ, VB/TL mượn")]
        [Display(Name = "Hồ sơ VB/TL")]
        public string ThuMuon { get; set; }

        [Required(ErrorMessage = "Điền Ngày Mượn")]
        [Display(Name = "Ngày Mượn")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime NgayMuon { get; set; }

        [Required(ErrorMessage = "Điền Ngày Trả")]
        [Display(Name = "Ngày Trả")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime NgayTra { get; set; }

        [Required(ErrorMessage = "Điền Tên Văn Thư")]
        [Display(Name = "Văn Thư")]
        public string VanThu { get; set; }

        [Display(Name = "Số Lượng Mượn")]
        public int SoLuong { get; set; }

        [Display(Name = "Tình Trạng")]
        public string TinhTrang { get; set; }

        [Display(Name = " Ngày Cập Nhật")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? NgayCapNhat { get; set; }


        [Display(Name = " Ngày Tạo")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? NgayTao { get; set; }

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