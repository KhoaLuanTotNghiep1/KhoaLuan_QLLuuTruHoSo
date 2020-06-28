using Microsoft.AspNet.Identity;
using S3Train.Contract;
using S3Train.Core.Constant;
using S3Train.Core.Extension;
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
        private readonly ITaiLieuVanBanService _taiLieuVanBanService;
        private readonly IFunctionLichSuHoatDongService _functionLichSuHoatDongService;

        public HoSoController()
        {

        }

        public HoSoController(IHoSoService hoSoService, ILoaiHoSoService loaiHoSoService, ITaiLieuVanBanService taiLieuVanBanService,
            IPhongBanService phongBanService, IHopService hopService, IFunctionLichSuHoatDongService functionLichSuHoatDongService)
        {
            _hoSoService = hoSoService;
            _loaiHoSoService = loaiHoSoService;
            _phongBanService = phongBanService;
            _hopService = hopService;
            _taiLieuVanBanService = taiLieuVanBanService;
            _functionLichSuHoatDongService = functionLichSuHoatDongService;
        }

        // GET: HoSo
        [Route("Danh-Sach")]
        public ActionResult Index(int? pageIndex, int? pageSize, string searchString, bool active = true)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? GlobalConfigs.DEFAULT_PAGESIZE;

            var model = new HoSoIndexViewModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var hoSos = _hoSoService.GetAllPaged(pageIndex, pageSize.Value, p => p.TrangThai == active, p => p.OrderByDescending(c => c.PhongLuuTru));

            if (!string.IsNullOrEmpty(searchString))
            {
                hoSos = _hoSoService.GetAllPaged(pageIndex, pageSize.Value, p => p.PhongLuuTru.Contains(searchString) || p.TapHoSo.PhongLuuTru.Contains(searchString)
                   || p.User.FullName.Contains(searchString) && p.TrangThai == active, p => p.OrderByDescending(c => c.PhongLuuTru));
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

            var autoList = LocalHops(GetHops());
            var userId = User.Identity.GetUserId();
            var chiTietHoatDong = "hồ sơ: " + model.PhongLuuTru;

            hoSo.TapHoSoId = model.TapHoSoId;
            hoSo.PhongLuuTru = model.PhongLuuTru;
            hoSo.TinhTrang = EnumTinhTrang.TrongKho;
            hoSo.ThoiGianBaoQuan = model.ThoiGianBaoQuan;
            hoSo.GhiChu = model.GhiChu;
            hoSo.BienMucHoSo = model.BienMucHoSo;
            hoSo.LoaiHoSoId = model.LoaiHoSoId;
            hoSo.HopId = autoList.FirstOrDefault(p => p.Text == model.HopId).Id;
            hoSo.UserId = userId;

            if (string.IsNullOrEmpty(model.Id))
            {
                var checkName = _hoSoService.Get(m => m.PhongLuuTru == model.PhongLuuTru);

                if (checkName != null)
                {
                    ViewBag.LoaiHoSos = SelectListItemFromDomain.SelectListItem_LoaiHoSo(_loaiHoSoService.GetAll(m => m.OrderBy(t => t.Ten)));
                    ViewBag.TapHoSos = SelectListItemFromDomain.SelectListItem_HoSo(_hoSoService.GetAll(m => m.OrderBy(t => t.PhongLuuTru)));

                    TempData["AlertMessage"] = "Hồ Sơ Có Cùng Phông Lưu Trữ Đã Tồn Tại";
                    return View(model);
                }

                _hoSoService.Insert(hoSo);
                _functionLichSuHoatDongService.Create(ActionWithObject.Create, userId, chiTietHoatDong);
                TempData["AlertMessage"] = "Tạo Mới Thành Công";
            }
            else
            {
                _hoSoService.Update(hoSo);
                _functionLichSuHoatDongService.Create(ActionWithObject.Update, userId, chiTietHoatDong);
                TempData["AlertMessage"] = "Cập Nhật Thành Công";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            var hoSo = _hoSoService.Get(m => m.Id == id);

            var countvb = _taiLieuVanBanService.Gets(p => p.HoSoId == id).Count();

            if (countvb > 0)
            {
                TempData["AlertMessage"] = "Không Thể Xóa Vì Có " + countvb + " Tài Liệu Văn Bản Phụ Thuộc";
                return RedirectToAction("Index", new { active = false });
            }

            _hoSoService.Remove(hoSo);
            TempData["AlertMessage"] = "Xóa Thành Công";
            _functionLichSuHoatDongService.Create(ActionWithObject.Delete, User.Identity.GetUserId(), "hồ sơ: "+ hoSo.PhongLuuTru);
            return RedirectToAction("Index");
        }

        [Route("Thong-Tin-Chi-Tiet")]
        public ActionResult Detail(string id)
        {
            var model = GetHoSo(_hoSoService.GetByIdHaveJoin(id));

            return View(model);
        }

        public ActionResult ChangeActive(string id, bool active)
        {
            var model = _hoSoService.Get(m => m.Id == id);

            model.TrangThai = active;

            _hoSoService.Update(model);
            _functionLichSuHoatDongService.Create(ActionWithObject.ChangeStatus, User.Identity.GetUserId(), "hồ sơ: " + model.PhongLuuTru + " thành " +active);

            var messenge = Messenger.ChangeActiveMessenge(active);

            TempData["AlertMessage"] = messenge;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AutoCompleteText(string text)
        {
            var model = LocalHops(GetHops());

            model = model.Where(p => p.Text.Contains(text)).ToHashSet();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private HashSet<AutoCompleteTextModel> LocalHops(IEnumerable<Hop> hops)
        {
            var list = ConvertDomainToAutoCompleteModel.LocalHoSo(hops);

            return list;
        }

        private IEnumerable<Hop> GetHops()
        {
            var listKe = _hopService.GetAllHaveJoinKe();

            var model = listKe.Where(p => p.TrangThai == true);

            return model;
        }

        private HoSoViewModel GetHoSo(HoSo x)
        {
            var autoList = LocalHops(GetHops());

            var model = new HoSoViewModel
            {
                Id = x.Id,
                BienMucHoSo = x.BienMucHoSo,
                GhiChu = x.GhiChu,
                PhongLuuTru = x.PhongLuuTru,
                LoaiHoSo = x.LoaiHoSo,
                HoSoCons = x.HoSoCons,
                TapHoSo = x.TapHoSo,
                TinhTrang = x.TinhTrang,
                ThoiGianBaoQuan = x.ThoiGianBaoQuan,
                User = x.User,
                TaiLieuVanBans = x.TaiLieuVanBans,
                NgayTao = x.NgayTao,
                NgayCapNhat = x.NgayCapNhat,
                TrangThai = x.TrangThai,
                Hop = x.Hop,
                HopId = autoList.FirstOrDefault(p => p.Id == x.HopId).Text,
                LoaiHoSoId = x.LoaiHoSoId,
                TapHoSoId = x.TapHoSoId,
                UserId = x.UserId
            };

            return model;
        }

        private List<HoSoViewModel> GetHoSos(IList<HoSo> hoSos)
        {
            var autoList = LocalHops(GetHops());

            return hoSos.Select(x => new HoSoViewModel
            {
                Id = x.Id,
                PhongLuuTru = x.PhongLuuTru,
                HopId = autoList.FirstOrDefault(p => p.Id == x.HopId).Text,
                TinhTrang = x.TinhTrang,
                NgayTao = x.NgayTao,
                TrangThai = x.TrangThai,
            }).ToList();
        }
    }
}