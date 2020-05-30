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
    [RoutePrefix("Ke")]
    public class KeController : Controller
    {
        private readonly ITuService _tuService;
        private readonly IKeService _keService;
        private readonly IFunctionLichSuHoatDongService _functionLichSuHoatDongService;

        public KeController()
        {

        }

        public KeController(ITuService tuService, IKeService keService, IFunctionLichSuHoatDongService functionLichSuHoatDongService)
        {
            _tuService = tuService;
            _keService = keService;
            _functionLichSuHoatDongService = functionLichSuHoatDongService;
        }

        // GET: Ke
        [Route("Danh-Sach")]
        public ActionResult Index(int? pageIndex, int? pageSize, string searchString, bool active = true)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? GlobalConfigs.DEFAULT_PAGESIZE;

            string[] includeArray = { "Hops", "User", "Tu" };
            var includes = AddList.AddItemByArray(includeArray);

            var model = new KeViewIndexModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var kes = _keService.GetAllPaged(pageIndex, pageSize.Value, p => p.TrangThai == active, p => p.OrderByDescending(c => c.NgayTao), includes);

            if (!string.IsNullOrEmpty(searchString))
            {
                kes = _keService.GetAllPaged(pageIndex, pageSize.Value, p => p.Ten.Contains(searchString) && p.TrangThai == active
                    && p.TrangThai == active, p => p.OrderByDescending(c => c.NgayTao), includes);
            }

            model.Paged = kes;
            model.Items = GetKes(kes.ToList());

            ViewBag.Active = active;
            ViewBag.searchString = searchString;
            ViewBag.Controller = "Ke";

            return View(model);
        }

        [HttpGet]
        public ActionResult CreateOrUpdate(string id)
        {
            var model = new KeViewModel();
            ViewBag.Tus = SelectListItemFromDomain.SelectListItem_Tu(_tuService.GetAll(m => m.OrderBy(t => t.Ten)));

            if (string.IsNullOrEmpty(id))
            {
                return View(model);
            }
            else
            {
                var ke = _keService.Get(m => m.Id == id);
                model = GetKe(ke);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult CreateOrUpdate(KeViewModel model)
        {
            var ke = string.IsNullOrEmpty(model.Id) ? new Ke { NgayCapNhat = DateTime.Now }
                : _keService.Get(m => m.Id == model.Id);

            var userId = User.Identity.GetUserId();
            var cthd = "kệ: " + model.Ten;

            ke.Ten = model.Ten;
            ke.SoThuTu = model.SoThuTu;
            ke.SoHopToiDa = model.SoHopToiDa;
            ke.NamBatDau = model.NamBatDau;
            ke.NamKetThuc = model.NamKetThuc;
            ke.TinhTrang = model.TinhTrang;
            ke.UserId = User.Identity.GetUserId();
            ke.Tuid = model.Tuid;

            if (string.IsNullOrEmpty(model.Id))
            {
                ke.SoHopHienTai = 0;
                var result = UpdateTu_SoLuongHienTai(model.Tuid, ActionWithObject.Update);
                if(!result)
                {
                    TempData["AlertMessage"] = "Số Lượng Kệ Trong Tủ Bạn Chọn Đã Đầy";
                    return View(model);
                }
                _keService.Insert(ke);

                _functionLichSuHoatDongService.Create(ActionWithObject.Create, userId, cthd);
                TempData["AlertMessage"] = "Tạo Mới Thành Công";
            }
            else
            {
                _keService.Update(ke);
                _functionLichSuHoatDongService.Create(ActionWithObject.Update, userId, cthd);
                TempData["AlertMessage"] = "Cập Nhật Thành Công";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            var ke = _keService.Get(m => m.Id == id);
            
            UpdateTu_SoLuongHienTai(ke.Tuid, ActionWithObject.Delete);
            _keService.Remove(ke);

            _functionLichSuHoatDongService.Create(ActionWithObject.Delete, User.Identity.GetUserId(), "kệ: " + ke.Ten);

            TempData["AlertMessage"] = "Xóa Thành Công";
            return RedirectToAction("Index");
        }

        [Route("Thong-Tin-Chi-Tiet")]
        public ActionResult Detail(string id)
        {
            var model = GetKe(_keService.Get(m => m.Id == id));

            return View(model);
        }

        public ActionResult ChangeActive(string id, bool active)
        {
            var model = _keService.Get(m => m.Id == id);

            model.TrangThai = active;

            _keService.Update(model);
            _functionLichSuHoatDongService.Create(ActionWithObject.ChangeStatus, User.Identity.GetUserId(), "kệ " + model.Ten + " thành "+ active);

            TempData["AlertMessage"] = "Xóa Thành Công";

            return RedirectToAction("Index");
        }

        public bool UpdateTu_SoLuongHienTai(string id, ActionWithObject action)
        {
            var tu = _tuService.GetById(id);
            var soluong = Compute.ComputeAmountWithAction(tu.SoLuongHienTai, action);
            if (soluong > tu.SoLuongMax)
            {
                ViewBag.Tus = SelectListItemFromDomain.SelectListItem_Tu(_tuService.GetAll(m => m.OrderBy(t => t.Ten)));
                return false;
            }
            else
            {
                tu.SoLuongHienTai = soluong;
                tu.NgayCapNhat = DateTime.Now;
                _tuService.Update(tu);
                return true;
            }
        }

        private KeViewModel GetKe(Ke x)
        {
            var model = new KeViewModel
            {
                Id = x.Id,
                Ten = x.Ten,
                SoHopToiDa = x.SoHopToiDa,
                SoHopHienTai = x.SoHopHienTai,
                NamBatDau = x.NamBatDau,
                NamKetThuc = x.NamKetThuc,
                SoThuTu = x.SoThuTu,
                TinhTrang = x.TinhTrang,
                NgayTao = x.NgayTao,
                NgayCapNhat = x.NgayCapNhat,
                TrangThai = x.TrangThai,
                UserId = x.UserId,
                Tuid = x.Tuid,
                Hops = x.Hops,
                User = x.User,
                ViTri = x.Tu.Ten
            };
            return model;
        }

        private List<KeViewModel> GetKes(IEnumerable<Ke> kes)
        {
            return kes.Select(x => new KeViewModel
            {
                Id = x.Id,
                Ten = x.Ten,
                SoHopToiDa = x.SoHopToiDa,
                SoHopHienTai = x.SoHopHienTai,
                NamBatDau = x.NamBatDau,
                NamKetThuc = x.NamKetThuc,
                SoThuTu = x.SoThuTu,
                TinhTrang = x.TinhTrang,
                NgayTao = x.NgayTao,
                NgayCapNhat = x.NgayCapNhat,
                TrangThai = x.TrangThai,
                UserId = x.UserId,
                Tuid = x.Tuid,
                User = x.User,
                ViTri = x.Tu.Ten
            }).ToList();
        }
    }
}