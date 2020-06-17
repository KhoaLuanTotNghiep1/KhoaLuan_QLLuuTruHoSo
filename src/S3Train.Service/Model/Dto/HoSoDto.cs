using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Model.Dto
{
    public class HoSoDto : EntityBase
    {
        public string TapHoSoId { get; set; }
        public string PhongLuuTru { get; set; }
        public EnumTinhTrang TinhTrang { get; set; }
        public int ThoiGianBaoQuan { get; set; }
        public string GhiChu { get; set; }
        public string BienMucHoSo { get; set; }
    }
}
