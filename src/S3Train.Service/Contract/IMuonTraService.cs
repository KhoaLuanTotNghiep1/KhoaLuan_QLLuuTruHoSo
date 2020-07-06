using S3Train.Domain;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace S3Train.Contract
{
    public interface IMuonTraService : IGenenicServiceBase<MuonTra>
    {
        IQueryable<MuonTra> GetAllHaveJoinUser();
        IQueryable<MuonTra> GetAllHaveJoinChiTietMuonTra();
        MuonTra GetHaveJoinUser(Expression<Func<MuonTra, bool>> predicate);
        MuonTra GetHaveJoinUserAndCTMT(Expression<Func<MuonTra, bool>> predicate);
    }
}
