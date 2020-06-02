using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Domain
{
    public class TaiLieuVanBan : EntityBase
    {
        public string Ten { get; set; }
        public string Loai { get; set; }
        public string Dang { get; set; } // công văn đến/đi/nội bộ
        public string SoKyHieu { get; set; }
        public int SoTo { get; set; }
        public EnumTinhTrang TinhTrang { get; set; } // còn trong kho hay đã cho mượn
        public string DuongDan { get; set; }
        public string GhiChu { get; set; }
        public string NoiDung { get; set; }
        public string NoiNhan { get; set; }
        public string NguoiGuiHoacNhan { get; set; }
        public string NguoiKy { get; set; }
        public string NguoiDuyet { get; set; }
        public string UserId { get; set; }
        public string HinhAnh { get; set; }
        public DateTime NgayBanHanh { get; set; }

        [ForeignKey("HoSo")]
        public string HoSoId { get; set; }
        [ForeignKey("NoiBanHanh")]
        public string NoiBanHanhId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public virtual HoSo HoSo { get; set; }
        public virtual NoiBanHanh NoiBanHanh { get; set; }
        public virtual ICollection<ChiTietMuonTra> ChiTietMuonTras { get; set; }
    }
}
