using Microsoft.AspNet.Identity;
using S3Train.Domain;
using S3Train.Model.LichSuHoatDong;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Contract
{
    public interface IFunctionLichSuHoatDongService
    {
        void Create(ActionWithObject hoatDong, string userId, string chiTietHoatDong);
        void Remove(string Id);
        void Remove(DateTime dateTime);
    }
}
