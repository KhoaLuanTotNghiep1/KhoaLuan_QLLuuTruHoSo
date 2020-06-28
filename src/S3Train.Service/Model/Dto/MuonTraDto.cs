using S3Train.Domain;
using S3Train.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Model.Dto
{
    public class MuonTraDto : EntityBase
    {
        public DateTime NgayMuon { get; set; }
        public DateTime NgayKetThuc { get; set; }

        public EnumTinhTrang TinhTrang { get; set; }
        public string UserId { get; set; }
        public string VanThu { get; set; }
        public int SoLuong { get; set; }

        public UserDto User { get; set; }
    }
}
