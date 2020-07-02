using System.Linq;
using S3Train.Contract;
using S3Train.Domain;
using System.Data.Entity;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;

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
        public MuonTra GetHaveJoinUser(Expression<Func<MuonTra, bool>> predicate)
        {
            return EntityDbSet.Include(p => p.User).FirstOrDefault(predicate);
        }

        public IQueryable<MuonTra> GetAllHaveJoinChiTietMuonTra()
        {
            var list = EntityDbSet.Include(p => p.ChiTietMuonTras);

            return list;
        }
        
        public MuonTra GetHaveJoinUserAndCTMT(Expression<Func<MuonTra, bool>> predicate)
        {
            return EntityDbSet.Include(p => p.User).Include(p => p.ChiTietMuonTras).FirstOrDefault(predicate);
        }

      
    }
}
