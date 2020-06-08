using Microsoft.AspNet.Identity;
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
    [Authorize(Roles = GlobalConfigs.ROLE_GIAMDOC_CANBOVANTHU)]
    public class LHSAndPBController : Controller
    {
        private readonly ILoaiHoSoService _loaiHoSoService;
        private readonly IHopService _hopService;
        private readonly IHoSoService _hoSoService;
        private readonly IPhongBanService _phongBanService;

        private readonly IFunctionLichSuHoatDongService _functionLichSuHoatDongService;

        public LHSAndPBController()
        {

        }

        public LHSAndPBController(ILoaiHoSoService loaiHoSoService, IPhongBanService phongBanService,
            IFunctionLichSuHoatDongService functionLichSuHoatDongService, IHoSoService hoSoService, IHopService hopService)
        {
            _loaiHoSoService = loaiHoSoService;
            _phongBanService = phongBanService;
            _hopService = hopService;
            _hoSoService = hoSoService;
            _functionLichSuHoatDongService = functionLichSuHoatDongService;
        }

        // GET: LHSAnfPB

        public ActionResult Index(int? pageIndexLHS, int? pageSizeLHS, int? pageIndexPB, int? pageSizePB)
        {
            var model = new LHSAndPBViewModel()
            {
                LHS = PagedLoaiHoSoIndexViewModel(pageIndexLHS, pageSizeLHS),
                PB= PagedPhongBanndexViewModel(pageIndexPB, pageSizePB)

            };
            return View(model);
        }

        [HttpGet]
        public PartialViewResult CreateOrUpdateLHS(string id)
        {
            var model = new LoaiHoSoViewModel();
            if (string.IsNullOrEmpty(id))
            {
                return PartialView("~/Views/LHSAndPB/_PartialCreateOrUpdateLHS.cshtml", model);
            }
            else
            {
                var loaiHoSo = _loaiHoSoService.Get(m => m.Id == id);
                model = GetLoaiHoso(loaiHoSo);
                return PartialView("~/Views/LHSAndPB/_PartialCreateOrUpdateLHS.cshtml", model);
            }
        }

        [HttpPost]
        public ActionResult CreateOrUpdateLHS(LoaiHoSoViewModel model)
        {
            var loaiHoSo = string.IsNullOrEmpty(model.Id) ? new LoaiHoSo { NgayCapNhat = DateTime.Now }
                : _loaiHoSoService.Get(m => m.Id == model.Id);

            loaiHoSo.Ma = model.Ma;
            loaiHoSo.Ten = model.Ten;

            if (string.IsNullOrEmpty(model.Id))
            {
                _loaiHoSoService.Insert(loaiHoSo);
                _functionLichSuHoatDongService.Create(ActionWithObject.Create, User.Identity.GetUserId(), "loại hồ sơ: " + loaiHoSo.Ten);
                TempData["AlertMessage"] = "Tạo Mới Loại Hồ Sơ Thành Công";
            }
            else
            {
                _loaiHoSoService.Update(loaiHoSo);
                _functionLichSuHoatDongService.Create(ActionWithObject.Update, User.Identity.GetUserId(), "loại hồ sơ: " + loaiHoSo.Ten);
                TempData["AlertMessage"] = "Cập Nhật Loại Hồ Sơ Thành Công";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult CreateOrUpdatePB(string id)
        {
            var model = new PhongBanViewModel();
            if (string.IsNullOrEmpty(id))
            {
                return PartialView("~/Views/LHSAndPB/_PartialCreateOrUpdatePB.cshtml", model);
            }
            else
            {
                var phongBan = _phongBanService.Get(m => m.Id == id);
                model = GetPhongBan(phongBan);
                return PartialView("~/Views/LHSAndPB/_PartialCreateOrUpdatePB.cshtml", model);
            }
        }

        [HttpPost]
        public ActionResult CreateOrUpdatePB(PhongBanViewModel model)
        {
            var phongBan = string.IsNullOrEmpty(model.Id) ? new PhongBan { NgayCapNhat = DateTime.Now }
                : _phongBanService.Get(m => m.Id == model.Id);

            phongBan.Ten = model.Ten;

            if (string.IsNullOrEmpty(model.Id))
            {
                _phongBanService.Insert(phongBan);
                _functionLichSuHoatDongService.Create(ActionWithObject.Create, User.Identity.GetUserId(), "phòng ban: " + phongBan.Ten);
                TempData["AlertMessage"] = "Tạo Mới Phong Ban Thành Công";
            }
            else
            {
                _phongBanService.Update(phongBan);
                _functionLichSuHoatDongService.Create(ActionWithObject.Update, User.Identity.GetUserId(), "phòng ban: " + phongBan.Ten);
                TempData["AlertMessage"] = "Cập Nhật Phòng Ban Thành Công";
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteLHS(string id)
        {
            var loaiHoSo = _loaiHoSoService.Get(m => m.Id == id);

            var count = _hoSoService.Gets(p => p.LoaiHoSoId == id).Count();

            if (count > 0)
            {
                TempData["AlertMessage"] = "Không Thể Xóa Vì Có " + count + " Hồ Sơ Phụ Thuộc";
                return RedirectToAction("Index", new { active = false });
            }

            _loaiHoSoService.Remove(loaiHoSo);
            _functionLichSuHoatDongService.Create(ActionWithObject.Delete, User.Identity.GetUserId(), "loại hồ sơ: " + loaiHoSo.Ten);
            TempData["AlertMessage"] = "Xóa Loại Hồ Sơ Thành Công";
            return RedirectToAction("Index");
        }

        public ActionResult DeletePB(string id)
        {
            var phongBan = _phongBanService.Get(m => m.Id == id);

            var count = _hopService.Gets(p => p.PhongBanId == id).Count();

            if (count > 0)
            {
                TempData["AlertMessage"] = "Không Thể Xóa Vì Có " + count + " Hộp Phụ Thuộc";
                return RedirectToAction("Index", new { active = false });
            }

            _phongBanService.Remove(phongBan);
            _functionLichSuHoatDongService.Create(ActionWithObject.Delete, User.Identity.GetUserId(), "phòng ban: " + phongBan.Ten);
            TempData["AlertMessage"] = "Xóa Phòng Ban Thành Công";
            return RedirectToAction("Index");
        }

        public LoaiHoSoVIndexiewModel PagedLoaiHoSoIndexViewModel(int? pageIndex, int? pageSize)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? 5;

            var model = new LoaiHoSoVIndexiewModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var loaiHoSos = _loaiHoSoService.GetAllPaged(pageIndex, pageSize.Value, null, p => p.OrderBy(c => c.Ten));

            model.Paged = loaiHoSos;
            model.Items = GetLoaiHoSos(loaiHoSos.ToList());
            return model;
        }

        public PhongBanVIndexiewModel PagedPhongBanndexViewModel(int? pageIndex, int? pageSize)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? 5;

            var model = new PhongBanVIndexiewModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var phongBans = _phongBanService.GetAllPaged(pageIndex, pageSize.Value, null, p => p.OrderBy(c => c.Ten));

            model.Paged = phongBans;
            model.Items = GetPhongBans(phongBans.ToList());
            return model;
        }

        private List<LoaiHoSoViewModel> GetLoaiHoSos(IList<LoaiHoSo> loaiHoSos)
        {
            return loaiHoSos.Select(x => new LoaiHoSoViewModel
            {
                Id = x.Id,
                Ten = x.Ten,
                Ma = x.Ma,
                HoSos = x.HoSos,
                NgayTao = x.NgayTao,
                NgayCapNhat = x.NgayCapNhat,
                TrangThai = x.TrangThai
            }).ToList();
        }

        private List<PhongBanViewModel> GetPhongBans(IList<PhongBan> phongBans)
        {
            return phongBans.Select(x => new PhongBanViewModel
            {
                Id = x.Id,
                Ten = x.Ten,
                Hops = x.Hops,
                NgayTao = x.NgayTao,
                NgayCapNhat = x.NgayCapNhat,
                TrangThai = x.TrangThai
            }).ToList();
        }

        private LoaiHoSoViewModel GetLoaiHoso(LoaiHoSo x)
        {
            var model = new LoaiHoSoViewModel
            {
                Id = x.Id,
                Ten = x.Ten,
                Ma = x.Ma,
                HoSos = x.HoSos,
                NgayTao = x.NgayTao,
                NgayCapNhat = x.NgayCapNhat,
                TrangThai = x.TrangThai
            };
            return model;
        }

        private PhongBanViewModel GetPhongBan(PhongBan x)
        {
            var model = new PhongBanViewModel
            {
                Id = x.Id,
                Ten = x.Ten,
                Hops = x.Hops,
                NgayTao = x.NgayTao,
                NgayCapNhat = x.NgayCapNhat,
                TrangThai = x.TrangThai
            };
            return model;
        }
    }
}