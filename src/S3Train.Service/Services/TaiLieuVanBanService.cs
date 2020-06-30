using S3Train.Contract;
using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace S3Train.Services
{
    public class TaiLieuVanBanService : GenenicServiceBase<TaiLieuVanBan>, ITaiLieuVanBanService
    {
        public TaiLieuVanBanService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public List<string> GetDocuments()
        {
            var result = new List<string>();
            string document;

            foreach (var item in EntityDbSet)
            {
                document = item.Ten;
                result.Add(document);
            }

            return result;
        }

        public int CountDocumentType(string type)
        {
            List<string> listType = new List<string>
            {
                type
            };

            foreach (var item in EntityDbSet)
            {
                var check = listType.Find(p => p.Equals(item.Loai));
                if (check == null)
                    listType.Add(item.Loai);
            }

            return listType.Count;

        }

        public TaiLieuVanBan GetByIdHaveJoin(string id)
        {
            var taiLieuVanBan = EntityDbSet.Include(p => p.HoSo)
                               .Include(p => p.User)
                               .Include(p => p.NoiBanHanh)
                               .FirstOrDefaultAsync(p => p.Id == id).Result;
            return taiLieuVanBan;
        }

        public TaiLieuVanBan GetHaveJoinCTMT(Expression<Func<TaiLieuVanBan, bool>> predicate)
        {
            var taiLieuVanBan = EntityDbSet
                               .Include(p => p.ChiTietMuonTras).FirstOrDefault(predicate);
            return taiLieuVanBan;
        }

        public IList<TaiLieuVanBan> GetAllHaveJoinAll()
        {
            var tailieuvanbans = EntityDbSet.Include(p => p.User)
                                  .Include(p => p.ChiTietMuonTras)
                                  .Include(p => p.NoiBanHanh)
                                  .Where(p => p.TrangThai == true);

            return tailieuvanbans.ToList();
        }
    }
}
