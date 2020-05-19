using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S3Train.Model
{
    public class NoiBanHanhDto: EntityBase
    {
        public string Ten { get; set; }
        public string MoTa { get; set; }
    }
}