using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Domain
{
    public class PhongBan : EntityBase
    {
        public string Ten { get; set; }

        public virtual ICollection<Hop> Hops { get; set; }
    }
}
