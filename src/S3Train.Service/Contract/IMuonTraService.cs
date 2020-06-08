using S3Train.Domain;
using System.Linq;

namespace S3Train.Contract
{
    public interface IMuonTraService : IGenenicServiceBase<MuonTra>
    {
        IQueryable<MuonTra> GetAllHaveJoinUser();
    }
}
