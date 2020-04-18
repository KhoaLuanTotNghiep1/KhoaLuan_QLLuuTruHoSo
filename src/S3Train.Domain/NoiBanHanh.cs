using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Domain
{
    public class NoiBanHanh : EntityBase
    {
        public string Ten { get; set; }
        public string MoTa { get; set; }

        public virtual ICollection<TaiLieuVanBan> TaiLieuVanBans { get; set; }
    }
}
