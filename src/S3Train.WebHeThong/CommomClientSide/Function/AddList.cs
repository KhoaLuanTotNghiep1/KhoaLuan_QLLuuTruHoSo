using S3Train.Domain;
using S3Train.WebHeThong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S3Train.WebHeThong.CommomClientSide.Function
{
    public static class AddList
    {
        public static List<string> AddItemByArray(string[] array)
        {
            var list = new List<string>();
            list.AddRange(array);
            return list;
        }

        public static List<DataPoint> ListDataPonit<T>(Dictionary<string, List<T>> keyValuePairs)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            DataPoint dataPoint;

            foreach (var item in keyValuePairs)
            {
                dataPoint = new DataPoint(item.Value.Count(), item.Key);
                dataPoints.Add(dataPoint);
            }

            return dataPoints;
        }
    }
}