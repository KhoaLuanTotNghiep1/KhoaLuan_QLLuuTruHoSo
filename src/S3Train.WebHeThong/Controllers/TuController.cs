using Microsoft.AspNet.Identity;
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
    [Authorize(Roles = GlobalConfigs.ROLE_GIAMDOC)]
    [RoutePrefix("Tu")]
    public class TuController : Controller
    {
        private readonly ITuService _tuService;
        private readonly IKeService _keService;
        private readonly IFunctionLichSuHoatDongService _functionLichSuHoatDongService;

        public TuController()
        {

        }

        public TuController(ITuService tuService, IKeService keService, IFunctionLichSuHoatDongService functionLichSuHoatDongService)
        {
            _tuService = tuService;
            _keService = keService;
            _functionLichSuHoatDongService = functionLichSuHoatDongService;
        }

        // GET: Tu
        [Route("Danh-Sach")]
        public ActionResult Index(int? pageIndex, int? pageSize, string searchString, bool active = true)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? GlobalConfigs.DEFAULT_PAGESIZE;

            var model = new TuIndexViewModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var tus = _tuService.GetAllPaged(pageIndex, pageSize.Value,  p=> p.TrangThai == active, p => p.OrderByDescending(c => c.NgayTao));

            if(!string.IsNullOrEmpty(searchString))
            {
                tus = _tuService.GetAllPaged(pageIndex, pageSize.Value, p => p.Ten.Contains(searchString) || p.NgươiQuanLy.Contains(searchString)
                    && p.TrangThai == active, p => p.OrderByDescending(c => c.NgayTao));
            }

            model.Paged = tus;
            model.Items = GetTus(tus.ToList());

            ViewBag.Active = active;
            ViewBag.searchString = searchString;
            ViewBag.Controller = "Tu";

            return View(model);
        }

        [HttpGet]
        public ActionResult CreateOrUpdate(string id)
        {
            var model = new TuViewModel();
            if(string.IsNullOrEmpty(id))
            {
                return View(model);
            }
            else
            {
                var tu = _tuService.Get(m => m.Id == id);
                model = GetTu(tu);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult CreateOrUpdate(TuViewModel model)
        {
            var tu =  string.IsNullOrEmpty(model.Id) ? new Tu { NgayCapNhat = DateTime.Now}
                : _tuService.Get(m => m.Id == model.Id);

            string userId = User.Identity.GetUserId();
            string chiTietHoatDong = model.Ten;

            tu.Ten = model.Ten;
            tu.ViTri = model.ViTri;
            tu.NgươiQuanLy = model.NgươiQuanLy;
            tu.SoLuongMax = model.SoLuongMax;
            tu.SoLuongHienTai = 0;
            tu.TinhTrang = model.TinhTrang;
            tu.DonViTinh = model.DonViTinh;
            tu.Kes = model.Kes;

            if(string.IsNullOrEmpty(model.Id))
            {
                _tuService.Insert(tu);
                _functionLichSuHoatDongService.Create(ActionWithObject.Create,userId,chiTietHoatDong);
                TempData["AlertMessage"] = "Tạo Mới Thành Công";
            }
            else
            {
                _tuService.Update(tu);
                _functionLichSuHoatDongService.Create(ActionWithObject.Update, userId, chiTietHoatDong);
                TempData["AlertMessage"] = "Cập Nhật Thành Công";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            var tu = _tuService.GetById(id);

            var kes = _keService.Gets(p => p.Tuid == id).Count();

            if (kes > 0 )
            {
                TempData["AlertMessage"] = "Không Thể Xóa Vì Có " + kes + " Kệ Phụ Thuộc";
                return RedirectToAction("Index", new { active = false });
            }

            _tuService.Remove(tu);

            _functionLichSuHoatDongService.Create(ActionWithObject.Delete, User.Identity.GetUserId(), tu.Ten);

            TempData["AlertMessage"] = "Xóa Thành Công";

            return RedirectToAction("Index");
        }

        [Route("Thong-Tin-Chi-Tiet")]
        public ActionResult Detail(string id)
        {
            var listTu = _tuService.GetAllHaveJoinKes();

            var tu = _tuService.Get(listTu, p => p.Id == id);

            var model = GetTu(tu);

            return View(model);
        }

        public ActionResult ChangeActive(string id, bool active)
        {
            var model = _tuService.Get(m => m.Id == id);

            model.TrangThai = active;

            _tuService.Update(model);
            _functionLichSuHoatDongService.Create(ActionWithObject.ChangeStatus, User.Identity.GetUserId(), model.Ten + " thành " + active);

            var messenge = Messenger.ChangeActiveMessenge(active);

            TempData["AlertMessage"] = messenge;
            return RedirectToAction("Index");
        }

        private TuViewModel GetTu(Tu tu)
        {
            var model = new TuViewModel
            {
                Id = tu.Id,
                Ten = tu.Ten,
                DonViTinh = tu.DonViTinh,
                NgươiQuanLy = tu.NgươiQuanLy,
                TinhTrang = tu.TinhTrang,
                SoLuongHienTai = tu.SoLuongHienTai,
                SoLuongMax = tu.SoLuongMax,
                ViTri = tu.ViTri,
                NgayTao = tu.NgayTao,
                NgayCapNhat = tu.NgayCapNhat,
                TrangThai = tu.TrangThai,
                Kes = tu.Kes
            };
            return model;
        }

        private List<TuViewModel> GetTus(IList<Tu> tus)
        {
            return tus.Select(x => new TuViewModel
            {
              Id = x.Id,
              Ten = x.Ten,
              DonViTinh = x.DonViTinh,
              NgươiQuanLy = x.NgươiQuanLy,
              TinhTrang = x.TinhTrang,
              SoLuongHienTai = x.SoLuongHienTai,
              SoLuongMax = x.SoLuongMax,
              ViTri = x.ViTri,
              NgayTao = x.NgayTao,
              NgayCapNhat = x.NgayCapNhat,
              TrangThai = x.TrangThai,
              Kes = x.Kes
            }).ToList();
        }
    }
}