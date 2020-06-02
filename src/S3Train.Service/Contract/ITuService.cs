using S3Train.Domain;
using System.Linq;

namespace S3Train.Contract
{
    public interface ITuService : IGenenicServiceBase<Tu>
    {
        IQueryable<Tu> GetAllHaveJoinKes();
    }
}
