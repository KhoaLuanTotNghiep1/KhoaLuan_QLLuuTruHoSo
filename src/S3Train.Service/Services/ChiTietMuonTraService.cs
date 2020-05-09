using S3Train.Contract;
using S3Train.Domain;

namespace S3Train.Services
{
    public class ChiTietMuonTraService : GenenicServiceBase<ChiTietMuonTra>, IChiTietMuonTraService
    {
        public ChiTietMuonTraService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
