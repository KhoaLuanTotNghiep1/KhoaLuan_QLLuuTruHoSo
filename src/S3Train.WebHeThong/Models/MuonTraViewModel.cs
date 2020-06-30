using S3Train.Domain;
using S3Train.WebHeThong.Controllers;
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
        [Display(Name = "Người mượn")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Điền Văn Bản Mượn")]
        [Display(Name = "Văn bản")]
        public string ThuMuon { get; set; }

        public string VanThu { get; set; }
        
        [Display(Name = "Ngày Mượn")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime NgayMuon { get; set; }

        [Required(ErrorMessage = "Điền  Số Lượng Mượn")]
        [Display(Name = "Số Lượng ")]
        public int SoLuong { get; set; }

        [Display(Name = "Hạn Trả")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime NgayTra { get; set; }
        
        [Display(Name = "Tình Trạng")]
        public EnumTinhTrang TinhTrang { get; set; }

        [Display(Name = " Ngày Cập Nhật")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? NgayCapNhat { get; set; }

        [Display(Name = "Trạng Thái")]
        public bool TrangThai { get; set; }
        public bool Checkbox { get; set; }
        public string ViTri { get; set; }

        public ApplicationUser User { get; set; }

        public TaiLieuVanBan TaiLieuVanBan { get; set; }

        public  ICollection<ChiTietMuonTra> ChiTietMuonTras { get; set; }
        public ChiTietMuonTraViewModel ChiTietMuonTra { get; set; }
    }

    public class MuonTraIndexViewModel : IndexViewModelBase
    {
        public IPagedList<MuonTra> Paged { get; set; }
        public List<MuonTraViewModel> Items { get; set; }
    }

}