using S3Train.Domain;
using System.Collections.Generic;

namespace S3Train.Contract
{
    public interface ITaiLieuVanBanService : IGenenicServiceBase<TaiLieuVanBan>
    {
        List<string> GetDocuments();
    }
}
