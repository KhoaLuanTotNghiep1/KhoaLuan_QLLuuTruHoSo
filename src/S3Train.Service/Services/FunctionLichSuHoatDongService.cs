using S3Train.Contract;
using S3Train.Domain;
using System;

namespace S3Train.Services
{
    public class FunctionLichSuHoatDongService : IFunctionLichSuHoatDongService
    {
        private readonly ILichSuHoatDongService _lichSuHoatDongService;

        public FunctionLichSuHoatDongService(ILichSuHoatDongService lichSuHoatDongService)
        {
            _lichSuHoatDongService = lichSuHoatDongService;
        }

        public void Create(ActionWithObject hoatDong, string userId, string chiTietHoatDong)
        {
            var lichSuHoatDong = new LichSuHoatDong()
            {
                HoatDong = ActionString(hoatDong),
                ChiTietHoatDong = ActionString(hoatDong) + chiTietHoatDong,
                UserId = userId
            };

            _lichSuHoatDongService.Insert(lichSuHoatDong);
        }

        public void Remove(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                return;

            var item = _lichSuHoatDongService.GetById(Id);

            if (item == null)
                return;

            _lichSuHoatDongService.Remove(item);
        }

        public void Remove(DateTime dateTime)
        {
            if (dateTime == null)
                dateTime = DateTime.Now;

            _lichSuHoatDongService.Remove(p => p.NgayTao <= dateTime);
        }

        private string ActionString(ActionWithObject actionWithObject)
        {
            string result = "";
            switch(actionWithObject)
            {
                case ActionWithObject.Create:
                    result = "Tạo mới ";
                    break;
                case ActionWithObject.Update:
                    result = "Cập nhật ";
                    break;
                case ActionWithObject.Delete:
                    result = "Xóa ";
                    break;
                case ActionWithObject.ChangeStatus:
                    result = "Thay Đổi ";
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
