﻿using System.Linq;
using S3Train.Contract;
using S3Train.Domain;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace S3Train.Services
{
    public class ChiTietMuonTraService : GenenicServiceBase<ChiTietMuonTra>, IChiTietMuonTraService
    {
        public ChiTietMuonTraService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<ChiTietMuonTra> GetAllHaveJoinTLVB()
        {
            var list = EntityDbSet.Include(p => p.TaiLieuVanBan).Include(p => p.MuonTra);

            return list;
        }

        public IList<ChiTietMuonTra> GetHaveJoinMuonTraAndTLVB()
        {
            var chiTietMuonTra = EntityDbSet.Include(p => p.TaiLieuVanBan)
                               .Include(p => p.MuonTra).ToList();
            return chiTietMuonTra;
        }

        public ChiTietMuonTra GetHaveJoinMT(Expression<Func<ChiTietMuonTra, bool>> predicate)
        {
            return EntityDbSet.Include(p => p.MuonTra).FirstOrDefault(predicate);
        }
    }
}
