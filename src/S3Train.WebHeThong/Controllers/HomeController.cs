using Microsoft.AspNet.Identity;
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
    [RoutePrefix("Home")]
    public class HomeController : Controller
    {
        private readonly ITuService _tuService;
        private readonly IKeService _keService;
        private readonly IHopService _hopService;
        private readonly IHoSoService _hoSoService;
        private readonly ITaiLieuVanBanService _taiLieuVanBanService;
        private readonly ILichSuHoatDongService _lichSuHoatDongService;
        private readonly IFunctionLichSuHoatDongService _functionLichSuHoatDongService;

        public HomeController()
        {

        }

        public HomeController(ITuService tuService, IKeService keService, IHopService hopService, IHoSoService hoSoService,
           ITaiLieuVanBanService taiLieuVanBanService, ILichSuHoatDongService lichSuHoatDongService, 
           IFunctionLichSuHoatDongService functionLichSuHoatDongService)
        {
            _tuService = tuService;
            _keService = keService;
            _hopService = hopService;
            _hoSoService = hoSoService;
            _taiLieuVanBanService = taiLieuVanBanService;
            _lichSuHoatDongService = lichSuHoatDongService;
            _functionLichSuHoatDongService = functionLichSuHoatDongService;
        }

        [Authorize(Roles = GlobalConfigs.ROLE_GIAMDOC_CANBOVANTHU)]
        [Route("Bang-Dieu-Khien")]
        public ActionResult Dashboard()
        {
            ViewBag.Array = CountItem();

            ViewBag.DataPoints = JsonConvert.SerializeObject(AddList.ListDataPonit(GetDoc()));

            var listKe = _lichSuHoatDongService.GetAllHaveJoinUser();

            var lichSuHoatDongs = listKe.Where(p => p.NgayTao <= DateTime.Now).OrderByDescending(p => p.NgayTao).Take(5);
            ViewBag.LichSuHoatDong = lichSuHoatDongs;

            return View();
        }

        [Route("Lich-Su-Hoat-Dong")]
        public ActionResult LichSuHoatDong(int? pageIndex, int? pageSize, string searchString, bool active = true)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? GlobalConfigs.DEFAULT_PAGESIZE;

            var model = new LichSuHoatDongIndexViewModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };

            var listKe = _lichSuHoatDongService.GetAllHaveJoinUser();

            var lichSuHoatDongs = _lichSuHoatDongService.GetAllPaged(listKe, pageIndex, pageSize.Value, null, p => p.OrderByDescending(c => c.NgayTao));

            if (!string.IsNullOrEmpty(searchString))
            {
                lichSuHoatDongs = _lichSuHoatDongService.GetAllPaged(listKe, pageIndex, pageSize.Value, p => p.User.FullName.Contains(searchString) || 
                    p.HoatDong.Contains(searchString), p => p.OrderBy(c => c.NgayTao));
            }

            model.Paged = lichSuHoatDongs;
            model.Items = lichSuHoatDongs.ToList();

            ViewBag.Active = active;
            ViewBag.searchString = searchString;

            return View(model);
        }

        public ActionResult DeleteLichSuHoatDong(string id)
        {
            _functionLichSuHoatDongService.Remove(id);

            TempData["AlertMessage"] = "Xóa Thành Công";
            return RedirectToAction("LichSuHoatDong");
        }

        [HttpPost]
        public ActionResult DeleteAll(DateTime dateTime)
        {
            _functionLichSuHoatDongService.Remove(dateTime);
            _functionLichSuHoatDongService.Create(ActionWithObject.Delete, User.Identity.GetUserId(),
                " tất cả các hoạt động trước ngày " + dateTime.ToString());

            TempData["AlertMessage"] = "Xóa Thành Công Các Hoạt Động Trước Ngày " + dateTime.ToString();
            return RedirectToAction("LichSuHoatDong");
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

            var hoSos = _hoSoService.GetAll();
            var hops = _hopService.GetAll();
            var kes = _keService.GetAll();
            var taiLieuVanBans = _taiLieuVanBanService.GetAll();

            var den = taiLieuVanBans.Where(p => p.Dang == GlobalConfigs.DANG_DEN).Count();
            var di = taiLieuVanBans.Where(p => p.Dang == GlobalConfigs.DANG_DI).Count();
            var noiBo = taiLieuVanBans.Where(p => p.Dang == GlobalConfigs.DANG_NOIBO).Count();
           
            array[0] = hoSos.Count();
            array[1] = hoSos.Where(p => p.TrangThai == true).Count();
            array[2] = hoSos.Where(p => p.TrangThai == false).Count();
            array[3] = taiLieuVanBans.Count();
            array[4] = taiLieuVanBans.Where(p => p.TrangThai == true).Count();
            array[5] = taiLieuVanBans.Where(p => p.TrangThai == false).Count();
            array[6] = hops.Count();
            array[7] = hops.Where(p => p.TrangThai == true).Count();
            array[8] = hops.Where(p => p.TrangThai == false).Count();
            array[9] = kes.Count();
            array[10] = kes.Where(p => p.TrangThai == true).Count();
            array[11] = kes.Where(p => p.TrangThai == false).Count();
            array[12] = ComputePercent(taiLieuVanBans.Count(), noiBo);
            array[13] = ComputePercent(taiLieuVanBans.Count(), den);
            array[14] = ComputePercent(taiLieuVanBans.Count(), di);
            array[15] = noiBo;
            array[16] = den;
            array[17] = di;

            return array;
        }

        private int ComputePercent(double max, double number)
        {
            if (max <= 0)
                return 0;
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