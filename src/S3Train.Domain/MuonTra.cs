using System;
using System.Collections.Generic;

namespace S3Train.Domain
{
    public class MuonTra : EntityBase
    {
        public DateTime NgayMuon { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public int SoLuong { get; set; }
        public string TinhTrang { get; set; }
        public string UserId { get; set; }

        public virtual ICollection<ChiTietMuonTra> ChiTietMuonTras { get; set; }
    }
}
