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
    [RoutePrefix("Hop")]
    public class HopController : Controller
    {
        private readonly IKeService _keService;
        private readonly IHopService _hopService;
        private readonly IHoSoService _hoSoService;
        private readonly IPhongBanService _phongBanService;
        private readonly IFunctionLichSuHoatDongService _functionLichSuHoatDongService;

        public HopController()
        {

        }

        public HopController(IKeService keService, IHopService hopService, IPhongBanService phongBanService,
            IFunctionLichSuHoatDongService functionLichSuHoatDongService, IHoSoService hoSoService)
        {
            _keService = keService;
            _hopService = hopService;
            _phongBanService = phongBanService;
            _hoSoService = hoSoService;
            _functionLichSuHoatDongService = functionLichSuHoatDongService;
        }

        // GET: Hop
        [Route("Danh-Sach")]
        public ActionResult Index(int? pageIndex, int? pageSize, string searchString, bool active = true)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? GlobalConfigs.DEFAULT_PAGESIZE;

            var listHop = _hopService.GetAllHaveJoinAll();

            var model = new HopViewIndexModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var hops = _hopService.GetAllPaged(listHop,pageIndex, pageSize.Value, p => p.TrangThai == active, 
                p => p.OrderByDescending(c => c.NgayTao));

            if (!string.IsNullOrEmpty(searchString))
            {
                hops = _hopService.GetAllPaged(listHop,pageIndex, pageSize.Value, p => p.ChuyenDe.Contains(searchString) || p.PhongBan.Ten.Contains(searchString)
                    && p.TrangThai == active, p => p.OrderByDescending(c => c.NgayTao));
            }

            model.Paged = hops;
            model.Items = GetHops(hops.ToList());

            ViewBag.Active = active;
            ViewBag.searchString = searchString;
            ViewBag.Controller = "Hop";

            return View(model);
        }

        [HttpGet]
        public ActionResult CreateOrUpdate(string id)
        {
            var model = new HopViewModel();

            DropDownList(); 

            if (string.IsNullOrEmpty(id))
            {
                return View(model);
            }
            else
            {
                var hop = _hopService.Get(m => m.Id == id);
                model = GetHop(hop);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult CreateOrUpdate(HopViewModel model)
        {
            var hop = string.IsNullOrEmpty(model.Id) ? new Hop { NgayCapNhat = DateTime.Now }
                : _hopService.Get(m => m.Id == model.Id);

            var autoList = AutoCompleteTextKes(GetKes());
            string userId = User.Identity.GetUserId();
            string chiTietHoatDong = "hộp: " + hop.ChuyenDe;

            hop.ChuyenDe = model.ChuyenDe;
            hop.KeId = autoList.FirstOrDefault(p => p.Text == model.KeId).Id;
            hop.PhongBanId = model.PhongBanId;
            hop.SoHop = model.SoHop;
            hop.UserId = userId;
            hop.NgayBatDau = model.NgayBatDau;
            hop.NgayKetThuc = model.NgayKetThuc;

            // create new
            if (string.IsNullOrEmpty(model.Id))
            {
                DropDownList();

                var checkName = _hopService.Get(m => m.ChuyenDe == model.ChuyenDe);

                // check name
                if (checkName != null)
                {
                    TempData["AlertMessage"] = "Hộp Chuyên Đề Đã Tồn Tại";
                    return View(model);
                }

                hop.TinhTrang = EnumTinhTrang.TrongKho;
                var result = UpdateTu_SoHopHienTai(hop.KeId, ActionWithObject.Update);

                // check amout ke
                if (!result)
                {
                    TempData["AlertMessage"] = "Số Lượng Hộp Trong Kệ Bạn Chọn Đã Đầy";
                    return View(model);
                }
                _hopService.Insert(hop);

                _functionLichSuHoatDongService.Create(ActionWithObject.Create, userId, chiTietHoatDong);

                TempData["AlertMessage"] = "Tạo Mới Thành Công";
            }
            else // update
            {
                _hopService.Update(hop);

                _functionLichSuHoatDongService.Create(ActionWithObject.Update, userId, chiTietHoatDong);
                TempData["AlertMessage"] = "Cập Nhật Thành Công";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            var hop = _hopService.Get(m => m.Id == id);
            var hosos = _hoSoService.Gets(m => m.HopId == id).Count();

            if (hosos > 0)
            {
                TempData["AlertMessage"] = "Không Thể Xóa Vì Có " + hosos + " Hồ Sơ Phụ Thuộc";
                return RedirectToAction("Index", new { active = false });
            }

            string chiTietHoatDong = "hộp " + hop.ChuyenDe + " trên kệ thứ " + hop.Ke.SoThuTu;

            UpdateTu_SoHopHienTai(hop.KeId, ActionWithObject.Delete);

            _hopService.Remove(hop);

            _functionLichSuHoatDongService.Create(ActionWithObject.Delete, User.Identity.GetUserId(), chiTietHoatDong);

            TempData["AlertMessage"] = "Xóa Thành Công";
            
            return RedirectToAction("Index");
        }

        [Route("Thong-Tin-Chi-Tiet")]
        public ActionResult Detail(string id)
        {
            var hops = _hopService.GetAllHaveJoinAll();

            var model = GetHop(hops.FirstOrDefault(p => p.Id == id));
            
            return View(model);
        }

        public ActionResult ChangeActive(string id, bool active)
        {
            var model = _hopService.Get(m => m.Id == id);
            string chiTietHoatDong = "hộp " + model.ChuyenDe + " trên kệ thứ " + model.Ke.SoThuTu + " thành " + active;

            model.TrangThai = active;

            _hopService.Update(model);

            _functionLichSuHoatDongService.Create(ActionWithObject.ChangeStatus, User.Identity.GetUserId(), chiTietHoatDong);

            var messenge = Messenger.ChangeActiveMessenge(active);

            TempData["AlertMessage"] = messenge;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AutoCompleteText(string text)
        {
            var model = AutoCompleteTextKes(GetKes());

            model = model.Where(p => p.Text.Contains(text)).ToHashSet();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public bool UpdateTu_SoHopHienTai(string id, ActionWithObject action)
        {
            var ke = _keService.GetById(id);

            if (ke == null)
                return false;

            var soluong = Compute.ComputeAmountWithAction(ke.SoHopHienTai, action);

            if (soluong > ke.SoHopToiDa)
            {
                DropDownList();
                return false;
            }
            else
            {
                ke.SoHopHienTai = soluong;
                _keService.Update(ke);
                return true;
            }
        }

        public void DropDownList()
        {
            ViewBag.PhongBans = SelectListItemFromDomain.SelectListItem_PhongBan(
                _phongBanService.GetAll(m => m.OrderBy(t => t.Ten)));
        }

        private HashSet<AutoCompleteTextModel> AutoCompleteTextKes(IEnumerable<Ke> kes)
        {
            var list = ConvertDomainToAutoCompleteModel.LocalHop(kes);

            return list;
        }

        private IEnumerable<Ke> GetKes()
        {
            var listKe = _keService.GetAllHaveJoinTu();

            var model = listKe.Where(p => p.TrangThai == true);

            return model;
        }

        private HopViewModel GetHop(Hop hop)
        {
            var autoList = AutoCompleteTextKes(GetKes());

            var model = new HopViewModel
            {
                Id = hop.Id,
                ChuyenDe = hop.ChuyenDe,
                NgayBatDau = hop.NgayBatDau,
                NgayKetThuc = hop.NgayKetThuc,
                PhongBan = hop.PhongBan,
                SoHop = hop.SoHop,
                PhongBanId = hop.PhongBanId,
                TinhTrang = hop.TinhTrang,
                NgayTao = hop.NgayTao,
                NgayCapNhat = hop.NgayCapNhat,
                TrangThai = hop.TrangThai,
                UserId = hop.UserId,
                HoSos = hop.HoSos,
                Ke = hop.Ke,
                User = hop.User,
                KeId = autoList.FirstOrDefault(p => p.Id == hop.KeId).Text
            };

            return model;
        }

        private List<HopViewModel> GetHops(IList<Hop> hops)
        {
            var autoList = AutoCompleteTextKes(GetKes());

            return hops.Select(hop => new HopViewModel
            {
                Id = hop.Id,
                ChuyenDe = hop.ChuyenDe,
                PhongBan = hop.PhongBan,
                TinhTrang = hop.TinhTrang,
                NgayTao = hop.NgayTao,
                TrangThai = hop.TrangThai,
                KeId = autoList.FirstOrDefault(p => p.Id == hop.KeId).Text
            }).ToList();
        }
    }
}