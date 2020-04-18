using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Domain
{
    public class Ke : EntityBase
    {
        public string Ten { get; set; }
        public int SoThuTu { get; set; }
        public int SoHopToiDa { get; set; }
        public int SoHopHienTai { get; set; }
        public DateTime NamBatDau { get; set; }
        public DateTime NamKetThuc { get; set; }
        public string TinhTrang { get; set; }

        public string UserId { get; set; }
        [ForeignKey("Tu")]
        public string Tuid { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public virtual Tu Tu { get; set; }
        public virtual ICollection<Hop> Hops { get; set; }
    }
}
