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
            var list = EntityDbSet.Include(p => p.Tu);

            return list;
        }
    }
}
