using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Domain
{
    public class LoaiHoSo : EntityBase
    {
        public string Ma { get; set; }
        public string Ten { get; set; }

        public virtual ICollection<HoSo> HoSos { get; set; }
    }
}
