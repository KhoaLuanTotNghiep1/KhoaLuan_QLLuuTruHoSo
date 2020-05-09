using S3Train.Contract;
using S3Train.Domain;

namespace S3Train.Services
{
    public class KeService : GenenicServiceBase<Ke>, IKeService
    {
        public KeService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
