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
        public string So { get; set; }
        public string KhoGiay { get; set; }
        public int SoTo { get; set; }
        public string TacGia { get; set; }
        public string TinhTrang { get; set; }
        public string DuongDan { get; set; }
        public string GhiChu { get; set; }

        [ForeignKey("HoSo")]
        public string HoSoId { get; set; }

        public virtual HoSo HoSo { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
