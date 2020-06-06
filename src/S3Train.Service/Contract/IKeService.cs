using S3Train.Domain;
using System.Collections.Generic;
using System.Linq;

namespace S3Train.Contract
{
    public interface IKeService : IGenenicServiceBase<Ke>
    {
        IQueryable<Ke> GetAllHaveJoinTu();
        IQueryable<Ke> GetAllHaveJoinAll();
    }
}
