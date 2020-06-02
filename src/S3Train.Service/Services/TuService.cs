using System.Linq;
using S3Train.Contract;
using S3Train.Domain;
using System.Data.Entity;

namespace S3Train.Services
{
    public class TuService : GenenicServiceBase<Tu>, ITuService
    {
        public TuService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<Tu> GetAllHaveJoinKes()
        {
            var list = EntityDbSet.Include(p => p.Kes);

            return list;
        }
    }   
}
