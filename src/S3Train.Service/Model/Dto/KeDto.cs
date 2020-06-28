using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Model.Dto
{
    public class KeDto : EntityBase
    {
        public string Ten { get; set; }
        public int SoThuTu { get; set; }
        public DateTime NamBatDau { get; set; }
        public DateTime NamKetThuc { get; set; }
        public string TinhTrang { get; set; }

        public TuDto Tu { get; set; }
    }
}
