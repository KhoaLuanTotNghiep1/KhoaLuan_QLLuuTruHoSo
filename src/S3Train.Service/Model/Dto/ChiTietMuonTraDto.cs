using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Model.Dto
{
    public class ChiTietMuonTraDto : EntityBase
    {
        public string MuonTraID { get; set; }
        public string TaiLieuVanBanId { get; set; }

        public MuonTraDto MuonTra { get; set; }
        public TaiLieuVanBanDto TaiLieuVanBan { get; set; }
    }
}
