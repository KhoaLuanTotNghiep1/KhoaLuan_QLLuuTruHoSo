using S3Train.Contract;
using S3Train.Domain;
using System.Collections.Generic;
using System.Data.Entity;

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

            foreach(var item in EntityDbSet)
            {
                result.Add(item.NoiDung);
            }

            return result;
        }

        public TaiLieuVanBan GetByIdHaveJoin(string id)
        {
            var taiLieuVanBan = EntityDbSet.Include(p => p.HoSo)
                               .Include(p => p.User)
                               .Include(p => p.NoiBanHanh)
                               .FirstOrDefaultAsync(p => p.Id == id).Result;
            return taiLieuVanBan;
        }
    }
}
