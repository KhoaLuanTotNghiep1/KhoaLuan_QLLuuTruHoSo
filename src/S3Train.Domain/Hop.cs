using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Domain
{
    public class Hop : EntityBase
    {
        public string ChuyenDe { get; set; }
        public int SoHop { get; set; }
        public EnumTinhTrang TinhTrang { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }

        [ForeignKey("Ke")]
        public string KeId { get; set; }
        [ForeignKey("PhongBan")]
        public string PhongBanId { get; set; }       
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public virtual Ke Ke { get; set; }
        public virtual PhongBan PhongBan { get; set; }
        public virtual ICollection<HoSo> HoSos { get; set; }
    }
}
