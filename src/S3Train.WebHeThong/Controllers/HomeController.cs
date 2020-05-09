using Newtonsoft.Json;
using S3Train.Contract;
using S3Train.Core.Constant;
using S3Train.Domain;
using S3Train.WebHeThong.CommomClientSide.Function;
using S3Train.WebHeThong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S3Train.WebHeThong.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITuService _tuService;
        private readonly IKeService _keService;
        private readonly IHopService _hopService;
        private readonly IHoSoService _hoSoService;
        private readonly ITaiLieuVanBanService _taiLieuVanBanService;
        private readonly ILichSuHoatDongService _lichSuHoatDongService;

        public HomeController()
        {

        }

        public HomeController(ITuService tuService, IKeService keService, IHopService hopService, IHoSoService hoSoService,
           ITaiLieuVanBanService taiLieuVanBanService, ILichSuHoatDongService lichSuHoatDongService)
        {
            _tuService = tuService;
            _keService = keService;
            _hopService = hopService;
            _hoSoService = hoSoService;
            _taiLieuVanBanService = taiLieuVanBanService;
            _lichSuHoatDongService = lichSuHoatDongService;
        }

        [Authorize(Roles = GlobalConfigs.ROLE_GIAMDOC_CANBOVANTHU)]
        public ActionResult Dashboard()
        {
            ViewBag.Array = CountItem();

            ViewBag.DataPoints = JsonConvert.SerializeObject(AddList.ListDataPonit(GetDoc()));

            var lichSuHoatDongs = _lichSuHoatDongService.Gets(p => p.NgayTao == DateTime.Today).Skip(5);
            if (lichSuHoatDongs.Count() > 0)
                ViewBag.LichSuHoatDong = lichSuHoatDongs;

            return View();
        }

        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            ViewBag.Error = 400;
            ViewBag.Title = "Không Tìm Thấy Trang";
            return View();
        }

        public ActionResult ServerError()
        {
            Response.StatusCode = 500;
            ViewBag.Error = 500;
            ViewBag.Title = "Lỗi Server";
            return View("NotFound");
        }

        private int[] CountItem()
        {
            var array = new int[18];

            int getAllTLVB = _taiLieuVanBanService.GetAll().Count();

            var den = _taiLieuVanBanService.Gets(p => p.Dang == GlobalConfigs.DANG_DEN).Count();
            var di = _taiLieuVanBanService.Gets(p => p.Dang == GlobalConfigs.DANG_DI).Count();
            var noiBo = _taiLieuVanBanService.Gets(p => p.Dang == GlobalConfigs.DANG_NOIBO).Count();

            array[0] = _hoSoService.GetAll().Count();
            array[1] = _hoSoService.Gets(p => p.TrangThai == true).Count();
            array[2] = _hoSoService.Gets(p => p.TrangThai == false).Count();
            array[3] = getAllTLVB;
            array[4] = _taiLieuVanBanService.Gets(p => p.TrangThai == true).Count();
            array[5] = _taiLieuVanBanService.Gets(p => p.TrangThai == false).Count();
            array[6] = _hopService.GetAll().Count();
            array[7] = _hopService.Gets(p => p.TrangThai == true).Count();
            array[8] = _hopService.Gets(p => p.TrangThai == false).Count();
            array[9] = _keService.GetAll().Count();
            array[10] = _keService.Gets(p => p.TrangThai == true).Count();
            array[11] = _keService.Gets(p => p.TrangThai == false).Count();
            array[12] = ComputePercent(getAllTLVB, noiBo);
            array[13] = ComputePercent(getAllTLVB, den);
            array[14] = ComputePercent(getAllTLVB, di);
            array[15] = noiBo;
            array[16] = den;
            array[17] = di;

            return array;
        }

        private int ComputePercent(double max, double number)
        {
            var a = number / max;
            return Convert.ToInt32((number / max)*100);
        }

        private Dictionary<string, List<TaiLieuVanBan>> GetDoc()
        {
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            var first = month.AddMonths(-1);
            var last = month.AddDays(-1);

            var list = new Dictionary<string, List<TaiLieuVanBan>>();

            var taiLieuVanBans = _taiLieuVanBanService.Gets(p => p.NgayTao >= first /*&& p.NgayTao <= last*/);

            var a = taiLieuVanBans.Where(p => p.Loai == "Báo Cáo").ToList();
            var b = taiLieuVanBans.Where(p => p.Loai == "Nghị Quyết").ToList();
            var c = taiLieuVanBans.Where(p => p.Loai == "Biên Bản").ToList();
            var e = taiLieuVanBans.Where(p => p.Loai == "Chỉ Thị").ToList();
            var f = taiLieuVanBans.Where(p => p.Loai == "Công Văn").ToList();
            var g = taiLieuVanBans.Where(p => p.Loai == "Giấy Mời").ToList();
            var h = taiLieuVanBans.Where(p => p.Loai == "Kế hoạch").ToList();
            var j = taiLieuVanBans.Where(p => p.Loai == "Kết Quả").ToList();
            var l = taiLieuVanBans.Where(p => p.Loai == "Kết quả xét nghiệm").ToList();
            var m = taiLieuVanBans.Where(p => p.Loai == "Quy chế").ToList();
            var n = taiLieuVanBans.Where(p => p.Loai == "Quyết địng quy phạm").ToList();
            var o = taiLieuVanBans.Where(p => p.Loai == "Quyết định").ToList();
            var t = taiLieuVanBans.Where(p => p.Loai == "Tài Liệu").ToList();
            var x = taiLieuVanBans.Where(p => p.Loai == "Thông Báo").ToList();
            var r = taiLieuVanBans.Where(p => p.Loai == "Thông Tư").ToList();
            var s = taiLieuVanBans.Where(p => p.Loai == "Tờ trình").ToList();          

            list.Add("Báo Cáo", a);
            list.Add("Nghị Quyết", b);
            list.Add("Biên Bản", c);
            list.Add("Chỉ Thị", e);
            list.Add("Công Văn", f);
            list.Add("Giấy Mời", g);
            list.Add("Kế hoạch", h);
            list.Add("Kết Quả", j);
            list.Add("Kết quả xét nghiệm", l);
            list.Add("Quyết địng quy phạm", m);
            list.Add("Quyết định", n);
            list.Add("Tài Liệu", o);
            list.Add("Thông Báo", t);
            list.Add("Thông Tư", r);
            list.Add("Tờ trình", x);

            return list;
        }
    }
}