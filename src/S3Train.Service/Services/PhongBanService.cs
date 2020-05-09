using S3Train.Contract;
using S3Train.Domain;

namespace S3Train.Services
{
    public class PhongBanService : GenenicServiceBase<PhongBan>, IPhongBanService
    {
        public PhongBanService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
