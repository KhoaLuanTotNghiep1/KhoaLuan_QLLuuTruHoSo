using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace S3Train.Contract
{
    public interface ITaiLieuVanBanService : IGenenicServiceBase<TaiLieuVanBan>
    {
        List<string> GetDocuments();
        TaiLieuVanBan GetByIdHaveJoin(string id);
        TaiLieuVanBan GetHaveJoinCTMT(Expression<Func<TaiLieuVanBan, bool>> predicate);
        int CountDocumentType(string type);
        IList<TaiLieuVanBan> GetAllHaveJoinAll();
    }
}
