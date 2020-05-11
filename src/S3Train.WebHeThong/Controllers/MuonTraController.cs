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
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace S3Train.WebHeThong.Controllers
{
    public class MuonTraController : Controller
    {
        private readonly IMuonTraService _muonTraService;
        private readonly IChiTietMuonTraService _chiTietMuonTraService;
        private readonly ITaiLieuVanBanService _taiLieuVanBanService;
        private readonly IHoSoService _hoSoService;
        private readonly IUserService _userService;

        public MuonTraController()
        {

        }

        public MuonTraController(IMuonTraService muonTraService, IChiTietMuonTraService chiTietMuonTraService, ITaiLieuVanBanService taiLieuVanBanService, IHoSoService hoSoService, IUserService userService)
        {
            _muonTraService = muonTraService;
            _chiTietMuonTraService = chiTietMuonTraService;
            _taiLieuVanBanService = taiLieuVanBanService;
            _hoSoService = hoSoService;
        }
        // GET: MuonTra
        public ActionResult Index(int? pageIndex, int? pageSize, string searchString, bool active = true)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? GlobalConfigs.DEFAULT_PAGESIZE;

            string[] includeArray = { "ChiTietMuonTras", "User" };
            var includes = AddList.AddItemByArray(includeArray);

            var model = new MuonTraIndexViewModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var muontras = _muonTraService.GetAllPaged(pageIndex, pageSize.Value, null, p => p.OrderBy(c => c.NgayMuon));
            if (!string.IsNullOrEmpty(searchString))
            {
                muontras = _muonTraService.GetAllPaged(pageIndex, pageSize.Value, p => p.User.FullName.Contains(searchString) || p.VanThu.Contains(searchString)
                    && p.TrangThai == active, p => p.OrderBy(c => c.NgayMuon), includes);
            }
            model.Paged = muontras;
            model.Items = GetMuonTras(muontras.ToList());


            ViewBag.Active = active;
            ViewBag.searchString = searchString;
            ViewBag.Controller = "MuonTra";
            return View(model);
        }

        [HttpGet]
        public ActionResult CreateOrUpdate(string id)
        {
            var model = new MuonTraViewModel();
            
            if (string.IsNullOrEmpty(id))
            {
                return View(model);
            }
            else
            {
                var muontra = _muonTraService.Get(m => m.Id == id);
                model = GetMuonTra(muontra);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult CreateOrUpdate(MuonTraViewModel model)
        {
            var muontra = string.IsNullOrEmpty(model.Id) ? new MuonTra { NgayCapNhat = DateTime.Now }
                : _muonTraService.Get(m => m.Id == model.Id);
            var chitietmuontra = string.IsNullOrEmpty(model.Id) ? new ChiTietMuonTra { NgayCapNhat = DateTime.Now }
                : _chiTietMuonTraService.Get(m => m.Id == model.Id);

            var autoList = AutoCompleteTextHoSos(_hoSoService.GetAll(), _taiLieuVanBanService.GetAll());
            var autoListUser = AutoCompleteTextUsers(_userService.GetAllAsync().Result);

            muontra.VanThu = User.Identity.GetUserName();
            muontra.NgayMuon = model.NgayMuon;
            muontra.NgayKetThuc = model.NgayTra;
            muontra.UserId = autoListUser.FirstOrDefault(p => p.Text == model.User.FullName).Id;
            muontra.TrangThai = true;
            muontra.TinhTrang = model.TinhTrang;
            muontra.NgayTao = DateTime.Now;
            chitietmuontra.ThuMuon = autoList.FirstOrDefault(p => p.Text == model.ThuMuon).Text;
            chitietmuontra.SoLuong = model.SoLuong;
            chitietmuontra.NgayTao = DateTime.Now;

            if (string.IsNullOrEmpty(model.Id))
            {
                muontra.Id = Guid.NewGuid().ToString();
                muontra.NgayMuon = DateTime.Now;
                _muonTraService.Insert(muontra);
                chitietmuontra.Id = Guid.NewGuid().ToString();
                chitietmuontra.MuonTraID = muontra.Id;
                _chiTietMuonTraService.Insert(chitietmuontra);
                TempData["AlertMessage"] = "Tạo Mới Thành Công";
            }
            else
            {
                muontra.NgayCapNhat = DateTime.Now;
                _muonTraService.Insert(muontra);
                TempData["AlertMessage"] = "Cập Nhật Thành Công";
            }
            return RedirectToAction("Index");
        }

            public ActionResult Delete(string id)
            {
                var muontra = _muonTraService.Get(m => m.Id == id);
                _muonTraService.Remove(muontra);
                TempData["AlertMessage"] = "Xóa Thành Công";
                return RedirectToAction("Index");
            }

        public ActionResult Detail(string id)
        {
            var model = GetMuonTra(_muonTraService.Get(m => m.Id == id));

            return View(model);
        }

        public ActionResult ChangeActive(string id, bool active)
        {
            var model = _muonTraService.Get(m => m.Id == id);

            model.TrangThai = active;
            model.NgayCapNhat = DateTime.Now;

            _muonTraService.Update(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AutoCompleteText(string text)
        {
            var model = AutoCompleteTextHoSos(_hoSoService.GetAll(), _taiLieuVanBanService.GetAll());

            model = model.Where(p => p.Text.Contains(text)).ToHashSet();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AutoCompleteText1(string text)
        {
            var model = AutoCompleteTextUsers(_userService.GetAllAsync().Result);

            model = model.Where(p => p.Text.Contains(text)).ToHashSet();

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        

        private HashSet<AutoCompleteTextModel> AutoCompleteTextHoSos(IList<HoSo> hosos, IList<TaiLieuVanBan> taiLieuVanBans)
        {
            var list = ConvertDomainToAutoCompleteModel.LocalHoSo(hosos, taiLieuVanBans);

            return list;
        }

        private HashSet<AutoCompleteTextModel> AutoCompleteTextUsers(IList<ApplicationUser> users)
        {
            var list = ConvertDomainToAutoCompleteModel.LocalUser(users);

            return list;
        }
        
        private MuonTraViewModel GetMuonTra(MuonTra muonTra)
        {
            var autoList = AutoCompleteTextHoSos(_hoSoService.GetAll(), _taiLieuVanBanService.GetAll());

            var model = new MuonTraViewModel
            {
                Id = muonTra.Id,
                NgayMuon = muonTra.NgayMuon,
                NgayCapNhat = muonTra.NgayCapNhat,
                NgayTra = muonTra.NgayKetThuc,
                VanThu = muonTra.VanThu,
                UserId = muonTra.User.FullName,
                TinhTrang = muonTra.TinhTrang,
                TrangThai = muonTra.TrangThai,
                NgayTao = muonTra.NgayTao,
                User = muonTra.User,
                ThuMuon = muonTra.ChiTietMuonTras.First().ThuMuon,
                ChiTietMuonTras = muonTra.ChiTietMuonTras,
            };

            model.ThuMuon = autoList.FirstOrDefault(p => p.Id == muonTra.ChiTietMuonTras.First().ThuMuon).Text;
            return model;
        }

        private List<MuonTraViewModel> GetMuonTras(IList<MuonTra> muontras)
        {
            return muontras.Select(x => new MuonTraViewModel
            {
                Id = x.Id,
                NgayMuon = x.NgayMuon,
                NgayTra = x.NgayKetThuc,
                NgayCapNhat = x.NgayCapNhat,
                TinhTrang = x.TinhTrang,
                TrangThai = x.TrangThai,
                UserId = x.UserId,
                VanThu = x.VanThu,
                NgayTao = x.NgayTao,
            }).ToList();
        }
    }
}