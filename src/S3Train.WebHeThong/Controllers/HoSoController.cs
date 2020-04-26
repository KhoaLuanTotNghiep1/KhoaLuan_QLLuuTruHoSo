using Microsoft.AspNet.Identity;
using S3Train.Contract;
using S3Train.Core.Constant;
using S3Train.Domain;
using S3Train.WebHeThong.CommomClientSide.DropDownList;
using S3Train.WebHeThong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S3Train.WebHeThong.Controllers
{
    public class HoSoController : Controller
    {
        private readonly IHoSoService _hoSoService;
        private readonly ILoaiHoSoService _loaiHoSoService;
        private readonly IPhongBanService _phongBanService;
        private readonly IHopService _hopService;

        public HoSoController()
        {

        }

        public HoSoController(IHoSoService hoSoService, ILoaiHoSoService loaiHoSoService, 
            IPhongBanService phongBanService, IHopService hopService)
        {
            _hoSoService = hoSoService;
            _loaiHoSoService = loaiHoSoService;
            _phongBanService = phongBanService;
            _hopService = hopService;
        }

        // GET: HoSo
        public ActionResult Index(int? pageIndex, int? pageSize)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? GlobalConfigs.DEFAULT_PAGESIZE;

            var model = new HoSoIndexViewModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var tus = _hoSoService.GetAllPaged(pageIndex, pageSize.Value, null, p => p.OrderBy(c => c.Hop.Ke.Tu.Ten));

            model.Paged = tus;
            model.Items = GetHoSos(tus.ToList());
            return View(model);
        }

        [HttpGet]
        public ActionResult CreateOrUpdate(string id)
        {
            var model = new HoSoViewModel();

            ViewBag.LoaiHoSos = SelectListItemFromDomain.SelectListItem_LoaiHoSo(_loaiHoSoService.GetAll(m => m.OrderBy(t => t.Ten)));
            ViewBag.TapHoSos = SelectListItemFromDomain.SelectListItem_HoSo(_hoSoService.GetAll(m => m.OrderBy(t => t.PhongLuuTru)));
            ViewBag.Hops = SelectListItemFromDomain.SelectListItem_Hop(_hopService.GetAll(m => m.OrderBy(t => t.Ke.Tu.Ten)));

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

            hoSo.TapHoSoId = model.TapHoSoId;
            hoSo.PhongLuuTru = model.PhongLuuTru;
            hoSo.TinhTrang = "Trong Kho";
            hoSo.ThoiGianBaoQuan = model.ThoiGianBaoQuan;
            hoSo.GhiChu = model.GhiChu;
            hoSo.BienMucHoSo = model.BienMucHoSo;
            hoSo.LoaiHoSoId = model.LoaiHoSoId;
            hoSo.HopId = model.HopId;
            hoSo.UserId = User.Identity.GetUserId();
            hoSo.TrangThai = true;

            if (string.IsNullOrEmpty(model.Id))
            {
                hoSo.Id = Guid.NewGuid().ToString();
                hoSo.NgayTao = DateTime.Now;
                _hoSoService.Insert(hoSo);
                TempData["AlertMessage"] = "Tạo Mới Thành Công";
            }
            else
            {
                hoSo.NgayCapNhat = DateTime.Now;
                _hoSoService.Update(hoSo);
                TempData["AlertMessage"] = "Cập Nhật Thành Công";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            var tu = _hoSoService.Get(m => m.Id == id);
            _hoSoService.Remove(tu);
            TempData["AlertMessage"] = "Xóa Thành Công";
            return RedirectToAction("Index");
        }

        public ActionResult Detail(string id)
        {
            var model = GetHoSo(_hoSoService.Get(m => m.Id == id));

            return View(model);
        }

        private HoSoViewModel GetHoSo(HoSo x)
        {
            var model = new HoSoViewModel
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
            };
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
                TrangThai = x.TrangThai
            }).ToList();
        }
    }
}