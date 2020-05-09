using S3Train.Contract;
using S3Train.Domain;

namespace S3Train.Services
{
    public class LichSuHoatDongService : GenenicServiceBase<LichSuHoatDong>, ILichSuHoatDongService
    {
        public LichSuHoatDongService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
