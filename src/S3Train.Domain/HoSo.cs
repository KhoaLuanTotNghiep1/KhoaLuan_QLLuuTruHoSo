using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Domain
{
    public class HoSo : EntityBase
    {
        public string TapHoSoId { get; set; }
        public string PhongLuuTru { get; set; }
        public EnumTinhTrang TinhTrang { get; set; }
        public int ThoiGianBaoQuan { get; set; }
        public string GhiChu { get; set; }
        public string BienMucHoSo { get; set; }

        [ForeignKey("Hop")]
        public string HopId { get; set; }
        [ForeignKey("LoaiHoSo")]
        public string LoaiHoSoId { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public virtual Hop Hop { get; set; }
        public virtual HoSo TapHoSo { get; set; }
        public virtual LoaiHoSo LoaiHoSo { get; set; }
        public virtual ICollection<HoSo> HoSoCons { get; set; }
        public virtual ICollection<TaiLieuVanBan> TaiLieuVanBans { get; set; }
    }
}
