using System.ComponentModel.DataAnnotations.Schema;

namespace S3Train.Domain
{
    public class ChiTietMuonTra : EntityBase
    {

        [ForeignKey("MuonTra")]
        public string MuonTraID { get; set; }
        [ForeignKey("TaiLieuVanBan")]
        public string TaiLieuVanBanId { get; set; }

        public virtual MuonTra MuonTra { get; set; }
        public virtual TaiLieuVanBan TaiLieuVanBan { get; set; }
    }
}
