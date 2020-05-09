using S3Train.Contract;
using S3Train.Domain;

namespace S3Train.Services
{
    public class HopService : GenenicServiceBase<Hop>, IHopService
    {
        public HopService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
