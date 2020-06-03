using S3Train.Domain;
using System.Linq;

namespace S3Train.Contract
{
    public interface ILichSuHoatDongService : IGenenicServiceBase<LichSuHoatDong>
    {
        IQueryable<LichSuHoatDong> GetAllHaveJoinUser();
    }
}
