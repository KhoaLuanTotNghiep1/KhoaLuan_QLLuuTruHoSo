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
        public string UserId { get; set; }
        public int SoThuTu { get; set; }
        public int SoHopToiDa { get; set; }
        public int SoHopHienTai { get; set; }
        public DateTime NamBatDau { get; set; }
        public DateTime NamKetThuc { get; set; }
        public string TinhTrang { get; set; }

        [ForeignKey("Kho")]
        public string KhoId { get; set; }

        public virtual Kho Kho { get; set; }
        public virtual ICollection<Hop> Hops { get; set; }
    }
}
