using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace S3Train.Domain
{
    public class MuonTra : EntityBase
    {
        public DateTime NgayMuon { get; set; }
        public DateTime NgayKetThuc { get; set; }
        
        public EnumTinhTrang TinhTrang { get; set; }
        public string UserId { get; set; }
        public string VanThu { get; set; }
        public int SoLuong { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
     
        public virtual ICollection<ChiTietMuonTra> ChiTietMuonTras { get; set; }
    }
}
