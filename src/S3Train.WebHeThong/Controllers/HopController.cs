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
    public class HopController : Controller
    {
        private readonly IKeService _keService;
        private readonly IHopService _hopService;
        private readonly IPhongBanService _phongBanService;

        public HopController()
        {

        }

        public HopController(IKeService keService, IHopService hopService, IPhongBanService phongBanService)
        {
            _keService = keService;
            _hopService = hopService;
            _phongBanService = phongBanService;
        }

        // GET: Hop
        public ActionResult Index(int? pageIndex, int? pageSize)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? GlobalConfigs.DEFAULT_PAGESIZE;

            var model = new HopViewIndexModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var hops = _hopService.GetAllPaged(pageIndex, pageSize.Value, null, p => p.OrderBy(c => c.Ke.SoThuTu));

            model.Paged = hops;
            model.Items = GetHops(hops .ToList());
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

            hop.ChuyenDe = model.ChuyenDe;
            hop.KeId = model.KeId;
            hop.PhongBanId = model.PhongBanId;
            hop.SoHop = model.SoHop;
            hop.UserId = User.Identity.GetUserId();
            hop.NgayBatDau = model.NgayBatDau;
            hop.NgayKetThuc = model.NgayKetThuc;
            hop.TrangThai = true;

            if (string.IsNullOrEmpty(model.Id))
            {
                hop.Id = Guid.NewGuid().ToString();
                hop.TinhTrang = "Trong Kho";
                hop.NgayTao = DateTime.Now;
                var result = UpdateTu_SoHopHienTai(model.KeId, ActionWithObject.Update);
                if (!result)
                {
                    TempData["AlertMessage"] = "Số Lượng Kệ Trong Tủ Bạn Chọn Đã Đầy";
                    return View(model);
                }
                _hopService.Insert(hop);
                TempData["AlertMessage"] = "Tạo Mới Thành Công";
            }
            else
            {
                hop.NgayCapNhat = DateTime.Now;
                _hopService.Update(hop);
                TempData["AlertMessage"] = "Cập Nhật Thành Công";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            var tu = _hopService.Get(m => m.Id == id);
            _hopService.Remove(tu);
            UpdateTu_SoHopHienTai(tu.KeId, ActionWithObject.Delete);
            TempData["AlertMessage"] = "Xóa Thành Công";
            
            return RedirectToAction("Index");
        }

        public ActionResult Detail(string id)
        {
            var model = GetHop(_hopService.Get(m => m.Id == id));
            
            return View(model);
        }

        public bool UpdateTu_SoHopHienTai(string id, ActionWithObject action)
        {
            var ke = _keService.GetById(id);
            var soluong = Compute.ComputeAmountWithAction(ke.SoHopHienTai, action);

            if (soluong > ke.SoHopToiDa)
            {
                DropDownList();
                return false;
            }
            else
            {
                ke.SoHopHienTai = soluong;
                ke.NgayCapNhat = DateTime.Now;
                _keService.Update(ke);
                return true;
            }
        }

        public void DropDownList()
        {
            ViewBag.PhongBans = SelectListItemFromDomain.SelectListItem_PhongBan(
                _phongBanService.GetAll(m => m.OrderBy(t => t.Ten)));

            ViewBag.Kes = SelectListItemFromDomain.SelectListItem_Ke(
                _keService.GetAll(m => m.OrderBy(t => t.Tu.Ten)));
        }

        private HopViewModel GetHop(Hop hop)
        {
            var model = new HopViewModel
            {
                Id = hop.Id,
                ChuyenDe = hop.ChuyenDe,
                NgayBatDau = hop.NgayBatDau,
                NgayKetThuc = hop.NgayKetThuc,
                PhongBan = hop.PhongBan,
                SoHop = hop.SoHop,
                PhongBanId = hop.PhongBanId,
                KeId = hop.KeId,
                TinhTrang = hop.TinhTrang,
                NgayTao = hop.NgayTao,
                NgayCapNhat = hop.NgayCapNhat,
                TrangThai = hop.TrangThai,
                UserId = hop.UserId,
                HoSos = hop.HoSos,
                Ke = hop.Ke,
                User = hop.User
            };
            return model;
        }

        private List<HopViewModel> GetHops(IList<Hop> hops)
        {
            return hops.Select(hop => new HopViewModel
            {
                Id = hop.Id,
                ChuyenDe = hop.ChuyenDe,
                NgayBatDau = hop.NgayBatDau,
                NgayKetThuc = hop.NgayKetThuc,
                PhongBan = hop.PhongBan,
                SoHop = hop.SoHop,
                PhongBanId = hop.PhongBanId,
                KeId = hop.KeId,
                TinhTrang = hop.TinhTrang,
                NgayTao = hop.NgayTao,
                NgayCapNhat = hop.NgayCapNhat,
                TrangThai = hop.TrangThai,
                UserId = hop.UserId,
                HoSos = hop.HoSos,
                Ke = hop.Ke,
                User = hop.User
            }).ToList();
        }
    }
}