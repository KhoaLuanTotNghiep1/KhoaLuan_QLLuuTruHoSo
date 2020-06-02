﻿using System.Linq;
using S3Train.Contract;
using S3Train.Domain;
using System.Data.Entity;

namespace S3Train.Services
{
    public class HopService : GenenicServiceBase<Hop>, IHopService
    {
        public HopService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<Hop> GetAllHaveJoinKe()
        {
            var list = EntityDbSet.Include(p => p.Ke.Tu);

            return list;
        }
    }
}
