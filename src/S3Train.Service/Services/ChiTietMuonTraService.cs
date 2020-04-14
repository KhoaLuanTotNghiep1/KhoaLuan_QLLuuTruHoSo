using S3Train.Contract;
using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Services
{
    public class ChiTietMuonTraService : GenenicServiceBase<ChiTietMuonTra>, IChiTietMuonTraService
    {
        public ChiTietMuonTraService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
