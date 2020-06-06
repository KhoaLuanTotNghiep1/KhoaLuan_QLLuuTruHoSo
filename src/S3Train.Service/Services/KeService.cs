using System.Linq;
using S3Train.Contract;
using S3Train.Domain;
using System.Data.Entity;
using System.Collections.Generic;

namespace S3Train.Services
{
    public class KeService : GenenicServiceBase<Ke>, IKeService
    {
        public KeService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<Ke> GetAllHaveJoinTu()
        {
            var list = EntityDbSet.Include(p => p.User).Include(p => p.Tu);

            return list;
        }

        public IQueryable<Ke> GetAllHaveJoinAll()
        {
            var kes = EntityDbSet.Include(p => p.User)
                                  .Include(p => p.Tu)
                                  .Include(p => p.Hops);

            return kes;
        }
    }
}
