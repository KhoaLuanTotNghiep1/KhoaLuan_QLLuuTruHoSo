using System.Linq;
using S3Train.Contract;
using S3Train.Domain;
using System.Data.Entity;

namespace S3Train.Services
{
    public class LichSuHoatDongService : GenenicServiceBase<LichSuHoatDong>, ILichSuHoatDongService
    {
        public LichSuHoatDongService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<LichSuHoatDong> GetAllHaveJoinUser()
        {
            var list = EntityDbSet.Include(p => p.User);

            return list;
        }
    }
}
