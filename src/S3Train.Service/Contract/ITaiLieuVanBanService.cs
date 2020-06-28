using S3Train.Domain;
using System.Collections.Generic;
using System.Linq;

namespace S3Train.Contract
{
    public interface ITaiLieuVanBanService : IGenenicServiceBase<TaiLieuVanBan>
    {
        List<string> GetDocuments();
        TaiLieuVanBan GetByIdHaveJoin(string id);
        int CountDocumentType(string type);
        IList<TaiLieuVanBan> GetAllHaveJoinAll();
    }
}
