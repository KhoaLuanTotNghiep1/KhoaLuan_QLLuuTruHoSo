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
        private readonly IHopService _hopService;
        private readonly IFunctionLichSuHoatDongService _functionLichSuHoatDongService;

        public KeController()
        {

        }

        public KeController(ITuService tuService, IKeService keService, IFunctionLichSuHoatDongService functionLichSuHoatDongService, IHopService hopService)
        {
            _tuService = tuService;
            _keService = keService;
            _hopService = hopService;
            _functionLichSuHoatDongService = functionLichSuHoatDongService;
        }

        // GET: Ke
        [Route("Danh-Sach")]
        public ActionResult Index(int? pageIndex, int? pageSize, string searchString, bool active = true)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? GlobalConfigs.DEFAULT_PAGESIZE;

            var model = new KeViewIndexModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var listKe = _keService.GetAllHaveJoinTu();

            var kes = _keService.GetAllPaged(listKe, pageIndex, pageSize.Value, p => p.TrangThai == active, 
                p => p.OrderByDescending(c => c.NgayTao));

            if (!string.IsNullOrEmpty(searchString))
            {
                kes = _keService.GetAllPaged(listKe, pageIndex, pageSize.Value, p => p.Ten.Contains(searchString) && p.TrangThai == active
                    && p.TrangThai == active, p => p.OrderByDescending(c => c.NgayTao));
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

                ViewBag.Tus = SelectListItemFromDomain.SelectListItem_Tu(_tuService.GetAll(m => m.OrderBy(t => t.Ten)));

                var checkName = _keService.Get(m => m.Ten == model.Ten);

                if (checkName != null)
                {
                    TempData["AlertMessage"] = "Kệ Có Cùng Tên Đã Tồn Tại";
                    return View(model);
                }

                var result = UpdateTu_SoLuongHienTai(model.Tuid, ActionWithObject.Update);
                if (!result)
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

            var hops = _hopService.Gets(p => p.KeId == id).Count();

            if (hops > 0)
            {
                TempData["AlertMessage"] = "Không Thể Xóa Vì Có " + hops + " Hộp Phụ Thuộc";
                return RedirectToAction("Index", new { active = false });
            }

            UpdateTu_SoLuongHienTai(ke.Tuid, ActionWithObject.Delete);
            _keService.Remove(ke);

            _functionLichSuHoatDongService.Create(ActionWithObject.Delete, User.Identity.GetUserId(), "kệ: " + ke.Ten);

            TempData["AlertMessage"] = "Xóa Thành Công";
            return RedirectToAction("Index");
        }

        [Route("Thong-Tin-Chi-Tiet")]
        public ActionResult Detail(string id)
        {
            var kes = _keService.GetAllHaveJoinAll();

            var model = GetKe(kes.FirstOrDefault(p => p.Id == id));

            return View(model);
        }

        public ActionResult ChangeActive(string id, bool active)
        {
            var model = _keService.Get(m => m.Id == id);

            model.TrangThai = active;

            _keService.Update(model);
            _functionLichSuHoatDongService.Create(ActionWithObject.ChangeStatus, User.Identity.GetUserId(), "kệ " + model.Ten + " thành "+ active);

            var messenge = Messenger.ChangeActiveMessenge(active);

            TempData["AlertMessage"] = messenge;

            return RedirectToAction("Index");
        }

        public bool UpdateTu_SoLuongHienTai(string id, ActionWithObject action)
        {
            int soluong = 0;
            var tu = _tuService.GetById(id);

            if (tu == null)
                return false;

            soluong = Compute.ComputeAmountWithAction(tu.SoLuongHienTai, action);

            if (soluong > tu.SoLuongMax)
            {
                ViewBag.Tus = SelectListItemFromDomain.SelectListItem_Tu(_tuService.GetAll(m => m.OrderBy(t => t.Ten)));
                return false;
            }
            else
            {
                tu.SoLuongHienTai = soluong;

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
                Tu = x.Tu
            };
            return model;
        }

        private List<KeViewModel> GetKes(IEnumerable<Ke> kes)
        {
            return kes.Select(x => new KeViewModel
            {
                Id = x.Id,
                Ten = x.Ten,
                TinhTrang = x.TinhTrang,
                NgayTao = x.NgayTao,
                TrangThai = x.TrangThai,
                Tu = x.Tu
            }).ToList();
        }
    }
}