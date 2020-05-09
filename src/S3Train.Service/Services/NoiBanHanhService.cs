using S3Train.Contract;
using S3Train.Domain;

namespace S3Train.Services
{
    public class NoiBanHanhService : GenenicServiceBase<NoiBanHanh>, INoiBanHanhService
    {
        public NoiBanHanhService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
