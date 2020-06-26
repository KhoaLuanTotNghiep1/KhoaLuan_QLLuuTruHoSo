using S3Train.Domain;
using S3Train.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Model.Dto
{
    public class TaiLieuVanBanDto : EntityBase
    {
        public string Ten { get; set; }
        public string Loai { get; set; }
        public string Dang { get; set; } // công văn đến/đi/nội bộ
        public string SoKyHieu { get; set; }
        public int SoTo { get; set; }
        public string TinhTrang { get; set; } // còn trong kho hay đã cho mượn
        public string DuongDan { get; set; }
        public string GhiChu { get; set; }
        public string NoiDung { get; set; }
        public string NoiNhan { get; set; }
        public string NguoiGuiHoacNhan { get; set; }
        public string NguoiKy { get; set; }
        public string NguoiDuyet { get; set; }
        public string HinhAnh { get; set; }
        public DateTime NgayBanHanh { get; set; }

        public HoSoDto HoSo { get; set; }
        public NoiBanHanhDto NoiBanHanh { get; set; }
        public UserDto User { get; set; }
    }
}
