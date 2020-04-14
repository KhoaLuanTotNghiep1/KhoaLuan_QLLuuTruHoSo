using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Domain
{
    public class Kho : EntityBase
    {
        public string Ten { get; set; }
        public string ViTri { get; set; }
        public string DienTich { get; set; }
        public string NgươiQuanLy { get; set; }
        public string DonViTinh { get; set; }
        public int SoLuongHienTai { get; set; }
        public int SoLuongMax { get; set; }
        public string TinhTrang { get; set; }

        public virtual ICollection<Ke> Kes { get; set; }
    }
}
