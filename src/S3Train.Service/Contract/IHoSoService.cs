using S3Train.Domain;
using System.Linq;

namespace S3Train.Contract
{
    public interface IHoSoService : IGenenicServiceBase<HoSo>
    {
        IQueryable<HoSo> GetAllHaveJoinHoSo();
        HoSo GetByIdHaveJoin(string id);
    }
}
