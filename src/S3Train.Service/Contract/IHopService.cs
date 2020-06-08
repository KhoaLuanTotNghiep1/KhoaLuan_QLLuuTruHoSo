using S3Train.Domain;
using System.Linq;

namespace S3Train.Contract
{
    public interface IHopService : IGenenicServiceBase<Hop>
    {
        IQueryable<Hop> GetAllHaveJoinAll();
        IQueryable<Hop> GetAllHaveJoinKe();
    }
}
