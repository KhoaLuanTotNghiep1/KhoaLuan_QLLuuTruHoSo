using S3Train.Domain;
using System;

namespace S3Train.Contract
{
    public interface IFunctionLichSuHoatDongService
    {
        void Create(ActionWithObject hoatDong, string userId, string chiTietHoatDong);
        void Remove(string Id);
        void Remove(DateTime dateTime);
    }
}
