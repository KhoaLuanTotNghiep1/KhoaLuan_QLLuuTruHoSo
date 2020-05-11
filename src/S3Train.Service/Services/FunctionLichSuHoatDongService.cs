using Microsoft.AspNet.Identity;
using S3Train.Contract;
using S3Train.Domain;
using S3Train.Model.LichSuHoatDong;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Services
{
    public class FunctionLichSuHoatDongService : IFunctionLichSuHoatDongService
    {
        private readonly ILichSuHoatDongService _lichSuHoatDongService;

        public FunctionLichSuHoatDongService()
        {

        }

        public FunctionLichSuHoatDongService(LichSuHoatDongService lichSuHoatDongService)
        {
            _lichSuHoatDongService = lichSuHoatDongService;
        }
    }
}
