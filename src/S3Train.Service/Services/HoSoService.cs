﻿using System.Linq;
using S3Train.Contract;
using S3Train.Domain;
using System.Data.Entity;

namespace S3Train.Services
{
    public class HoSoService : GenenicServiceBase<HoSo>, IHoSoService
    {
        public HoSoService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<HoSo> GetAllHaveJoinHoSo()
        {
            var list = EntityDbSet.Include(p => p.Hop.Ke.Tu);

            return list;
        }
    }
}
