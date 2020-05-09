using S3Train.Contract;
using S3Train.Domain;

namespace S3Train.Services
{
    public class MuonTraService : GenenicServiceBase<MuonTra>, IMuonTraService
    {
        public MuonTraService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
