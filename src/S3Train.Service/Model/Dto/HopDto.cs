using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Model.Dto
{
    public class HopDto : EntityBase
    {
        public string ChuyenDe { get; set; }
        public int SoHop { get; set; }
        public EnumTinhTrang TinhTrang { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }

        public KeDto Ke { get; set; }
    }
}
