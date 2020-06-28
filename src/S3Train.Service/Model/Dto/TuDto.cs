using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Model.Dto
{
    public class TuDto : EntityBase
    {
        public string Ten { get; set; }
        public string ViTri { get; set; }
        public string NgươiQuanLy { get; set; }
    }
}
