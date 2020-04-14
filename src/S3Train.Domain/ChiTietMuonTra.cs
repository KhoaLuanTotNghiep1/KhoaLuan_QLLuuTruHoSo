using System.ComponentModel.DataAnnotations.Schema;

namespace S3Train.Domain
{
    public class ChiTietMuonTra : EntityBase
    {
        public string ThuMuon { get; set; }

        [ForeignKey("MuonTra")]
        public string MuonTraID { get; set; }

        public virtual MuonTra MuonTra { get; set; }
    }
}
