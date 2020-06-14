using System.Linq;
using S3Train.Contract;
using S3Train.Domain;
using System.Data.Entity;

namespace S3Train.Services
{
    public class MuonTraService : GenenicServiceBase<MuonTra>, IMuonTraService
    {
        public MuonTraService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public IQueryable<MuonTra> GetAllHaveJoinUser()
        {
            var list = EntityDbSet.Include(p => p.User);

            return list;
        }
        public IQueryable<MuonTra> GetAllHaveJoinChiTietMuonTra()
        {
            var list = EntityDbSet.Include(p => p.ChiTietMuonTras);

            return list;
        }

    }
}
