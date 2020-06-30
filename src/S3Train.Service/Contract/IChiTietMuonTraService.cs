using S3Train.Domain;
using System.Collections.Generic;
using System.Linq;

namespace S3Train.Contract
{
    public interface IChiTietMuonTraService : IGenenicServiceBase<ChiTietMuonTra>
    {
        IQueryable<ChiTietMuonTra> GetAllHaveJoinTLVB();
        IList<ChiTietMuonTra> GetHaveJoinMuonTraAndTLVB();
    }
}
