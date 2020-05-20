using Microsoft.AspNet.Identity;
using S3Train.Contract;
using S3Train.Core.Constant;
using S3Train.Domain;
using S3Train.WebHeThong.CommomClientSide.DropDownList;
using S3Train.WebHeThong.CommomClientSide.Function;
using S3Train.WebHeThong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S3Train.WebHeThong.Controllers
{
    [Authorize(Roles = GlobalConfigs.ROLE_GIAMDOC_CANBOVANTHU)]
    [RoutePrefix("Ho-So")]
    public class HoSoController : Controller
    {
        private readonly IHoSoService _hoSoService;
        private readonly ILoaiHoSoService _loaiHoSoService;
        private readonly IPhongBanService _phongBanService;
        private readonly IHopService _hopService;
        private readonly IFunctionLichSuHoatDongService _functionLichSuHoatDongService;

        public HoSoController()
        {

        }

        public HoSoController(IHoSoService hoSoService, ILoaiHoSoService loaiHoSoService, 
            IPhongBanService phongBanService, IHopService hopService, IFunctionLichSuHoatDongService functionLichSuHoatDongService)
        {
            _hoSoService = hoSoService;
            _loaiHoSoService = loaiHoSoService;
            _phongBanService = phongBanService;
            _hopService = hopService;
            _functionLichSuHoatDongService = functionLichSuHoatDongService;
        }

        // GET: HoSo
        [Route("Danh-Sach")]
        public ActionResult Index(int? pageIndex, int? pageSize, string searchString, bool active = true)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? GlobalConfigs.DEFAULT_PAGESIZE;

            string[] includeArray = { "Hop", "TapHoSo", "LoaiHoSo", "User", "HoSoCons", "TaiLieuVanBans" };
            var includes = AddList.AddItemByArray(includeArray);

            var model = new HoSoIndexViewModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var hoSos = _hoSoService.GetAllPaged(pageIndex, pageSize.Value, p => p.TrangThai == active, p => p.OrderBy(c => c.PhongLuuTru), includes);

            if (!string.IsNullOrEmpty(searchString))
            {
                hoSos = _hoSoService.GetAllPaged(pageIndex, pageSize.Value, p => p.PhongLuuTru.Contains(searchString) || p.TapHoSo.PhongLuuTru.Contains(searchString)
                   || p.User.FullName.Contains(searchString) && p.TrangThai == active, p => p.OrderBy(c => c.PhongLuuTru), includes);
            }

            model.Paged = hoSos;
            model.Items = GetHoSos(hoSos.ToList());

            ViewBag.Active = active;
            ViewBag.searchString = searchString;
            ViewBag.Controller = "HoSo";

            return View(model);
        }

        [HttpGet]
        public ActionResult CreateOrUpdate(string id)
        {
            var model = new HoSoViewModel();

            ViewBag.LoaiHoSos = SelectListItemFromDomain.SelectListItem_LoaiHoSo(_loaiHoSoService.GetAll(m => m.OrderBy(t => t.Ten)));
            ViewBag.TapHoSos = SelectListItemFromDomain.SelectListItem_HoSo(_hoSoService.GetAll(m => m.OrderBy(t => t.PhongLuuTru)));

            if (string.IsNullOrEmpty(id))
            {
                return View(model);
            }
            else
            {
                var hoSo = _hoSoService.Get(m => m.Id == id);
                model = GetHoSo(hoSo);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateOrUpdate(HoSoViewModel model)
        {
            var hoSo = string.IsNullOrEmpty(model.Id) ? new HoSo { NgayCapNhat = DateTime.Now }
                : _hoSoService.Get(m => m.Id == model.Id);

            var autoList = LocalHops(_hopService.GetAll());
            var userId = User.Identity.GetUserId();
            var chiTietHoatDong = "hồ sơ: " + model.PhongLuuTru;

            hoSo.TapHoSoId = model.TapHoSoId;
            hoSo.PhongLuuTru = model.PhongLuuTru;
            hoSo.TinhTrang = GlobalConfigs.TINHTRANG_TRONGKHO;
            hoSo.ThoiGianBaoQuan = model.ThoiGianBaoQuan;
            hoSo.GhiChu = model.GhiChu;
            hoSo.BienMucHoSo = model.BienMucHoSo;
            hoSo.LoaiHoSoId = model.LoaiHoSoId;
            hoSo.HopId = autoList.FirstOrDefault(p => p.Text == model.HopId).Id;
            hoSo.UserId = userId;
            hoSo.TrangThai = true;

            if (string.IsNullOrEmpty(model.Id))
            {
                hoSo.Id = Guid.NewGuid().ToString();
                hoSo.NgayTao = DateTime.Now;
                _hoSoService.Insert(hoSo);
                _functionLichSuHoatDongService.Create(ActionWithObject.Create, userId, chiTietHoatDong);
                TempData["AlertMessage"] = "Tạo Mới Thành Công";
            }
            else
            {
                hoSo.NgayCapNhat = DateTime.Now;
                _hoSoService.Update(hoSo);
                _functionLichSuHoatDongService.Create(ActionWithObject.Update, userId, chiTietHoatDong);
                TempData["AlertMessage"] = "Cập Nhật Thành Công";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            var hoSo = _hoSoService.Get(m => m.Id == id);
            _hoSoService.Remove(hoSo);
            TempData["AlertMessage"] = "Xóa Thành Công";
            _functionLichSuHoatDongService.Create(ActionWithObject.Delete, User.Identity.GetUserId(), "hồ sơ: "+ hoSo.PhongLuuTru);
            return RedirectToAction("Index");
        }

        [Route("Thong-Tin-Chi-Tiet")]
        public ActionResult Detail(string id)
        {
            var model = GetHoSo(_hoSoService.Get(m => m.Id == id));

            return View(model);
        }

        public ActionResult ChangeActive(string id, bool active)
        {
            var model = _hoSoService.Get(m => m.Id == id);

            model.TrangThai = active;
            model.NgayCapNhat = DateTime.Now;

            _hoSoService.Update(model);
            _functionLichSuHoatDongService.Create(ActionWithObject.ChangeStatus, User.Identity.GetUserId(), "hồ sơ: " + model.PhongLuuTru + " thành " +active);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AutoCompleteText(string text)
        {
            var model = LocalHops(_hopService.GetAll());

            model = model.Where(p => p.Text.Contains(text)).ToHashSet();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private HashSet<AutoCompleteTextModel> LocalHops(IList<Hop> hops)
        {
            var list = ConvertDomainToAutoCompleteModel.LocalHoSo(hops);

            return list;
        }

        private HoSoViewModel GetHoSo(HoSo x)
        {
            var autoList = LocalHops(_hopService.GetAll());

            var model = new HoSoViewModel
            {
                Id = x.Id,
                BienMucHoSo = x.BienMucHoSo,
                GhiChu = x.GhiChu,
                Hop = x.Hop,
                PhongLuuTru = x.PhongLuuTru,
                LoaiHoSoId = x.LoaiHoSoId,
                LoaiHoSo = x.LoaiHoSo,
                HoSoCons = x.HoSoCons,
                TapHoSo = x.TapHoSo,
                TapHoSoId = x.TapHoSoId,
                TinhTrang = x.TinhTrang,
                ThoiGianBaoQuan = x.ThoiGianBaoQuan,
                User = x.User,
                UserId = x.UserId,
                TaiLieuVanBans = x.TaiLieuVanBans,
                NgayTao = x.NgayTao,
                NgayCapNhat = x.NgayCapNhat,
                TrangThai = x.TrangThai,
                ViTri = x.Hop.Ke.Tu.Ten + " kệ thứ " + x.Hop.Ke.SoThuTu + " hộp số " + x.Hop.SoHop
            };

            model.HopId = autoList.FirstOrDefault(p => p.Id == x.HopId).Text;

            return model;
        }

        private List<HoSoViewModel> GetHoSos(IList<HoSo> hoSos)
        {
            return hoSos.Select(x => new HoSoViewModel
            {
                Id = x.Id,
                BienMucHoSo = x.BienMucHoSo,
                GhiChu = x.GhiChu,
                Hop = x.Hop,
                PhongLuuTru = x.PhongLuuTru,
                HopId = x.HopId,
                LoaiHoSoId = x.LoaiHoSoId,
                LoaiHoSo = x.LoaiHoSo,
                HoSoCons = x.HoSoCons,
                TapHoSo = x.TapHoSo,
                TapHoSoId = x.TapHoSoId,
                TinhTrang = x.TinhTrang,
                ThoiGianBaoQuan = x.ThoiGianBaoQuan,
                User = x.User,
                UserId = x.UserId,
                TaiLieuVanBans = x.TaiLieuVanBans,
                NgayTao = x.NgayTao,
                NgayCapNhat = x.NgayCapNhat,
                TrangThai = x.TrangThai,
                ViTri = x.Hop.Ke.Tu.Ten + " kệ thứ " + x.Hop.Ke.SoThuTu + " hộp số " + x.Hop.SoHop
            }).ToList();
        }
    }
}