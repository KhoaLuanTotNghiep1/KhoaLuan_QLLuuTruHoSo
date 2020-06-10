using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using S3Train.Domain;

namespace S3Train.WebHeThong.Models
{
    public class AutoCompleteTextModel
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public EnumTinhTrang TinhTrang { get; set; }
        public string ViTri { get; set; }
        public string UserName { get; set; }
        public int SoLuongMuon { get; set; }
        public DateTime HanTra { get; set; }
    }
}