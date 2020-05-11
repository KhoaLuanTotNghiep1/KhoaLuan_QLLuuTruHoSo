using S3Train.Contract;
using S3Train.Domain;

namespace S3Train.Services
{
    public class LoaiHoSoService : GenenicServiceBase<LoaiHoSo>, ILoaiHoSoService
    {
        public LoaiHoSoService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
