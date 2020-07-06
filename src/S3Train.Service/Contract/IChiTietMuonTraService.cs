using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace S3Train.Contract
{
    public interface IChiTietMuonTraService : IGenenicServiceBase<ChiTietMuonTra>
    {
        IQueryable<ChiTietMuonTra> GetAllHaveJoinTLVB();
        IList<ChiTietMuonTra> GetHaveJoinMuonTraAndTLVB();
        ChiTietMuonTra GetHaveJoinMT(Expression<Func<ChiTietMuonTra, bool>> predicate);
    }
}
