using Microsoft.AspNet.Identity;
using S3Train.Contract;
using S3Train.Core.Constant;
using S3Train.Core.Extension;
using S3Train.Domain;
using S3Train.Service;
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
    public class MuonController : Controller
    {
        private readonly IMuonTraService _muonTraService;
        private readonly IChiTietMuonTraService _chiTietMuonTraService;
        private readonly ITaiLieuVanBanService _taiLieuVanBanService;
        private readonly IHoSoService _hoSoService;
        private readonly IUserService _userService;

        public MuonController()
        {

        }

        public MuonController(IMuonTraService muonTraService, IChiTietMuonTraService chiTietMuonTraService, ITaiLieuVanBanService taiLieuVanBanService, IHoSoService hoSoService, IUserService userService)
        {
            _muonTraService = muonTraService;
            _chiTietMuonTraService = chiTietMuonTraService;
            _taiLieuVanBanService = taiLieuVanBanService;
            _hoSoService = hoSoService;
            _userService = userService;
        }
        // GET: MuonTra
        public ActionResult Index(int? pageIndex, int? pageSize, string searchString, string tinhTrang = GlobalConfigs.TINHTRANG_DANGMUON, bool active = true)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? GlobalConfigs.DEFAULT_PAGESIZE;

            var model = new MuonTraIndexViewModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var muontras = _muonTraService.GetAllPaged(pageIndex, pageSize.Value, p => p.TrangThai == active
            && p.TinhTrang == EnumTinhTrang.DangMuon, p => p.OrderBy(c => c.NgayMuon));
            if (!string.IsNullOrEmpty(searchString))
            {
                muontras = _muonTraService.GetAllPaged(pageIndex, pageSize.Value, p => p.User.FullName.Contains(searchString) || p.VanThu.Contains(searchString)
                    && p.TrangThai == active && p.TinhTrang == EnumTinhTrang.DangMuon, p => p.OrderBy(c => c.NgayMuon));
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
        public  ActionResult CreateOrUpdate(string userId, string[] array, DateTime ngayTra, int SoLuong)
        {
            var ctMuon = _chiTietMuonTraService.GetAll();
            var muontra = new MuonTra();

            var autoList = AutoCompleteTextHoSos(_taiLieuVanBanService.Gets(p => p.TrangThai == true, p => p.OrderBy(x => x.Ten)).ToList());

            muontra.UserId = userId;
            muontra.VanThu = User.Identity.GetUserId();
            muontra.NgayMuon = DateTime.Now;
            muontra.NgayKetThuc = ngayTra;
            muontra.TinhTrang = EnumTinhTrang.DangMuon;
            _muonTraService.Insert(muontra);

            for (int i = 0; i < array.Length; i++)
            {
                var chitietmuontra = new ChiTietMuonTra();
                var b = array[i];
                var a = autoList.FirstOrDefault(p => p.Text == b).Id; 
                chitietmuontra.TaiLieuVanBanId = a;
                chitietmuontra.MuonTraID = muontra.Id;
                _chiTietMuonTraService.Insert(chitietmuontra);
                var vanBan = _taiLieuVanBanService.Get(m => m.Id == a);
                vanBan.TinhTrang = EnumTinhTrang.DangMuon;
                _taiLieuVanBanService.Update(vanBan);

            }
            TempData["AlertMessage"] = "Tạo Mới Thành Công";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CreateOrUpdateTra(string userId, string NgayMuon, int SoLuong, string Id)
        {
            var muontra = new MuonTra
            {
                UserId = userId,
                VanThu = User.Identity.GetUserId(),
                NgayMuon = DateTime.Parse(NgayMuon),
                NgayKetThuc = DateTime.Now,
                TinhTrang = EnumTinhTrang.DaTra
            };
            _muonTraService.Insert(muontra);

            var muon = _chiTietMuonTraService.Get(m => m.TaiLieuVanBanId == Id);
            muon.TrangThai = false;
            _chiTietMuonTraService.Update(muon);

            var chitietmuontra = new ChiTietMuonTra
            {
                TaiLieuVanBanId = Id,
                MuonTraID = muontra.Id
            };
            _chiTietMuonTraService.Insert(chitietmuontra);

            var vanBan = _taiLieuVanBanService.Get(m => m.Id == Id);
            vanBan.TinhTrang = EnumTinhTrang.TrongKho;
            _taiLieuVanBanService.Update(vanBan);
           
            TempData["AlertMessage"] = "Tạo Mới Thành Công";
            return RedirectToAction("Index");
        }


        private TaiLieu_VanBanViewModel GetTaiLieuVanBan(TaiLieuVanBan vanBan)
        {
            var model = new TaiLieu_VanBanViewModel
            {
                Id = vanBan.Id,

            };
            return model;
        }
       
        public ActionResult Delete(string id)
        {
            var muontra = _muonTraService.Get(m => m.Id == id);
            var chiTietMuonTra = _chiTietMuonTraService.GetAll();
            foreach (var item in chiTietMuonTra)
            {
                if (item.MuonTraID == muontra.Id)
                {
                    var vanBan = _taiLieuVanBanService.GetAll();
                    foreach (var vb in vanBan)
                    {
                        if (vb.Id == item.TaiLieuVanBanId)
                        {
                            vb.TinhTrang = EnumTinhTrang.TrongKho;
                            _taiLieuVanBanService.Update(vb);
                        }
                    }
                    _chiTietMuonTraService.Remove(item);
                }
            }
            _muonTraService.Remove(muontra);
            
            TempData["AlertMessage"] = "Xóa Thành Công";
            return RedirectToAction("Index");
        }

        public ActionResult DeleteTra(string id)
        {
            var muontra = _muonTraService.Get(m => m.Id == id);
            var chiTietMuonTra = _chiTietMuonTraService.GetAll();
            foreach (var item in chiTietMuonTra)
            {
                if (item.MuonTraID == muontra.Id)
                {
                    var vanBan = _taiLieuVanBanService.GetAll();
                    _chiTietMuonTraService.Remove(item);
                }
            }
            _muonTraService.Remove(muontra);

            TempData["AlertMessage"] = "Xóa Thành Công";
            return RedirectToAction("Index");
        }

        public ActionResult Detail(string id)
        {
            var model = new List<ChiTietMuonTraViewModel>();
            var chiTietMuonTras = _chiTietMuonTraService.GetAll();
            var chiTietMuonTra = _chiTietMuonTraService.Get(m => m.MuonTraID == id);
            var MuonTra = _muonTraService.Get(m => m.Id == id);
            //var MuonTras = _muonTraService.GetAll();
            //var users = await _userService.GetAllAsync();
            //var TLVB = _taiLieuVanBanService.Get(m => m.Id == chiTietMuonTra.TaiLieuVanBanId);
            foreach (var item in chiTietMuonTras)
            {
                if (item.MuonTraID == id && item.TrangThai == true)
                {
                    model.Add(new ChiTietMuonTraViewModel
                    {
                        Id = item.Id,
                        MuonTraId = item.MuonTraID,
                        TaiLieuVanBan = item.TaiLieuVanBan,
                        TaiLieuVanBanId = item.TaiLieuVanBanId,
                        MuonTra = item.MuonTra,
                        User = item.MuonTra.User,
                    });
                }

            }
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
            var model = AutoCompleteTextHoSos( _taiLieuVanBanService.GetAll());

            model = model.Where(p => p.Text.Contains(text)).ToHashSet();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> AutoCompleteTextUser(string text)
        {
            var users = await _userService.GetAllAsync();
            var model = Users(users);

            model = model.Where(x => x.Text.Contains(text)).ToHashSet();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private HashSet<AutoCompleteTextModel> Users(IList<ApplicationUser> users)
        {
            var list = ConvertDomainToAutoCompleteModel.LocalUser(users);
            return list;
        }

        private HashSet<AutoCompleteTextModel> AutoCompleteTextHoSos( IList<TaiLieuVanBan> taiLieuVanBans)
        {
            var list = ConvertDomainToAutoCompleteModel.LocalVanBan(taiLieuVanBans);

            return list;
        }

        
        private MuonTraViewModel GetMuonTra(MuonTra muonTra)
        {
            var autoList = AutoCompleteTextHoSos(_taiLieuVanBanService.GetAll());

            var model = new MuonTraViewModel
            {
                Id = muonTra.Id,
                NgayMuon = muonTra.NgayMuon,
                NgayCapNhat = muonTra.NgayCapNhat,
                NgayTra = muonTra.NgayKetThuc,
                UserId = muonTra.UserId,
                TinhTrang = AttributeExtension.GetDecription(muonTra.TinhTrang),
                TrangThai = muonTra.TrangThai,
                User = muonTra.User,
                
            };
            
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
                TinhTrang = AttributeExtension.GetDecription(x.TinhTrang),
                TrangThai = x.TrangThai,
                UserId = x.UserId,
                User = x.User,
                ChiTietMuonTras = x.ChiTietMuonTras,

            }).ToList();
            
        }

        private List<ChiTietMuonTraViewModel> GetChiTietMuonTras(IList<ChiTietMuonTra> chiTietMuonTras)
        {
            return chiTietMuonTras.Select(x => new ChiTietMuonTraViewModel
            {
                Id = x.Id,
                MuonTraId = x.MuonTraID,
                TaiLieuVanBan = x.TaiLieuVanBan,
                TaiLieuVanBanId = x.TaiLieuVanBanId,
                MuonTra = x.MuonTra,
                User = x.MuonTra.User,
            }).ToList();
        }
        
    }
}