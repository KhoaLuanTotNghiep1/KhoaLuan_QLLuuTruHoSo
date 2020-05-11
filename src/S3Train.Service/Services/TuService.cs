using S3Train.Contract;
using S3Train.Domain;

namespace S3Train.Services
{
    public class TuService : GenenicServiceBase<Tu>, ITuService
    {
        public TuService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }   
}
