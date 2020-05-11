using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace S3Train.WebHeThong.Models
{
    public class DataPoint
    {
        public DataPoint(double y, string label)
        {
            this.label = label;
            this.y = y;
        }

        public string label = "";

        public double? y = null;
    }
}