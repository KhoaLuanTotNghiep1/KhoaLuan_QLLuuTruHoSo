using S3Train.Contract;
using S3Train.Domain;

namespace S3Train.Services
{
    public class HoSoService : GenenicServiceBase<HoSo>, IHoSoService
    {
        public HoSoService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
