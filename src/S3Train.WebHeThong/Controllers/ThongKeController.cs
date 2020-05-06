using Newtonsoft.Json;
using S3Train.Contract;
using S3Train.Core.Constant;
using S3Train.Domain;
using S3Train.WebHeThong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S3Train.WebHeThong.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly ITaiLieuVanBanService _taiLieuVanBanService;

        public ThongKeController()
        {

        }

        public ThongKeController(ITaiLieuVanBanService taiLieuVanBanService)
        {
            _taiLieuVanBanService = taiLieuVanBanService;
        }

        // GET: ThongKe
        public ActionResult Index(DateTime? startTime, DateTime? endTime)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            DataPoint dataPoint;

            var list = GetDoc(startTime, endTime);

            foreach(var item in list)
            {
                dataPoint = new DataPoint(item.Value.Count(), item.Key);
                dataPoints.Add(dataPoint);
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View(list);
        }

        private Dictionary<string, List<TaiLieuVanBan>> GetDoc(DateTime? startTime, DateTime? endTime)
        {
            var list = new Dictionary<string, List<TaiLieuVanBan>>();

            var taiLieuVanBans = _taiLieuVanBanService.GetAll();

            if (startTime.HasValue)
                taiLieuVanBans = taiLieuVanBans.Where(p => p.NgayTao >= startTime).ToList();

            if (endTime.HasValue)
                taiLieuVanBans = taiLieuVanBans.Where(p => p.NgayTao <= endTime).ToList();

            var a = taiLieuVanBans.Where(p => p.Dang == GlobalConfigs.DANG_DEN).ToList();
            var b = taiLieuVanBans.Where(p => p.Dang == GlobalConfigs.DANG_DI).ToList();
            var c = taiLieuVanBans.Where(p => p.Dang == GlobalConfigs.DANG_NOIBO).ToList();

            list.Add(GlobalConfigs.DANG_DEN,a);
            list.Add(GlobalConfigs.DANG_DI, b);
            list.Add(GlobalConfigs.DANG_NOIBO, c);

            return list;
        }

    }
}