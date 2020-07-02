using Microsoft.AspNet.Identity;
using S3Train.Contract;
using S3Train.Core.Constant;
using S3Train.Core.Extension;
using S3Train.Domain;
using S3Train.WebHeThong.CommomClientSide.Function;
using S3Train.WebHeThong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public ActionResult Index(int? pageIndex, int? pageSize, string searchString, bool active = true)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? GlobalConfigs.DEFAULT_PAGESIZE;

            var model = new MuonTraIndexViewModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var a = new List<MuonTra>();
            var listMuonTra = _muonTraService.GetAllHaveJoinUser();

            listMuonTra = listMuonTra.Where(p => p.TinhTrang == EnumTinhTrang.DangMuon);

            var muontras = _muonTraService.GetAllPaged(listMuonTra, pageIndex, pageSize.Value, p => p.TrangThai == active, p => p.OrderBy(c => c.NgayMuon));
           
              if (!string.IsNullOrEmpty(searchString))
                {
                    muontras = _muonTraService.GetAllPaged(listMuonTra, pageIndex, pageSize.Value, p => p.User.FullName.Contains(searchString)
                                || p.ChiTietMuonTras.FirstOrDefault().TaiLieuVanBan.Ten.Contains(searchString) || p.VanThu.Contains(searchString)
                                && p.TrangThai == active, p => p.OrderBy(c => c.NgayMuon));

                }
             
            
            model.Paged = muontras;

            model.Items = GetMuonTras(muontras.ToList());


            ViewBag.Active = active;
            ViewBag.searchString = searchString;
            ViewBag.Controller = "Muon";
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
        public async Task<ActionResult> CreateOrUpdate(string userId, string[] array, DateTime ngayTra)
        {
            var muontra = new MuonTra();
            var autoList = AutoCompleteTextHoSos(_taiLieuVanBanService.Gets(p => p.TrangThai == true, p => p.OrderBy(x => x.Ten)).ToList());
            var users = await _userService.GetAllAsync();
            var muonTras = new List<MuonTra>();
            var model = Users(users, muonTras);

            muontra.UserId = model.FirstOrDefault(m => m.UserName == userId).Id;
            muontra.VanThu = User.Identity.GetUserName();
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
                var chiTietMuonTras = _chiTietMuonTraService.GetAll();
                muontra.SoLuong = chiTietMuonTras.Count(m => m.MuonTraID == muontra.Id);
                _muonTraService.Update(muontra);
                var vanBan = _taiLieuVanBanService.Get(m => m.Id == a);
                vanBan.TinhTrang = EnumTinhTrang.DangMuon;
                _taiLieuVanBanService.Update(vanBan);

            }
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
        
        public ActionResult Detail(string id)
        {
            var chiTietMuonTras = _chiTietMuonTraService.GetHaveJoinMuonTraAndTLVB().Where(n => n.MuonTraID == id);
            var model = GetChiTietMuonTras(chiTietMuonTras);
            return View(model);
        }

      

        [HttpPost]
        public ActionResult AutoCompleteText(string pre)
        {
            List<TaiLieuVanBan> tlvb = new List<TaiLieuVanBan>();
            var model = AutoCompleteTextHoSos( _taiLieuVanBanService.GetAllHaveJoinAll());

            model = model.Where(p => p.Text.Contains(pre)).ToHashSet();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> AutoCompleteTextUser(string user)
        {
            var users = await _userService.GetAllAsync();
            var muonTras = _muonTraService.GetAll();
            var model = Users(users, muonTras);

            model = model.Where(x => x.Text.Contains(user)).ToHashSet();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private HashSet<AutoCompleteTextModel> Users(IList<ApplicationUser> users, IList<MuonTra> muonTras)
        {
            var list = ConvertDomainToAutoCompleteModel.LocalUser(users, muonTras);
            return list;
        }

        private HashSet<AutoCompleteTextModel> AutoCompleteTextHoSos( IList<TaiLieuVanBan> taiLieuVanBans)
        {
            var list = ConvertDomainToAutoCompleteModel.LocalVanBan(taiLieuVanBans);

            return list;
        }

        [HttpGet]
        public async Task<ActionResult> KiemTraHanTra(string user)
        {
            var muonTras = _muonTraService.GetAll();
            var users = await _userService.GetAllAsync();
            var model = Users(users, muonTras);
            var userId = model.FirstOrDefault(m => m.Text == user).Id;
            var list = new List<MuonTraViewModel>();
            foreach (var item in muonTras)
            {
                if (userId == item.User.Id && item.TinhTrang == EnumTinhTrang.DangMuon)
                {

                    list.Add(new MuonTraViewModel
                    {
                        UserId = item.UserId,
                        NgayTra = item.NgayKetThuc,
                    });
                }

            }
            return Json(new { d = list }, JsonRequestBehavior.AllowGet);
        }
        
        private MuonTraViewModel GetMuonTra(MuonTra muonTra)
        {
            var autoList = AutoCompleteTextHoSos(_taiLieuVanBanService.GetAll());
            var chiTietMuonTras = _chiTietMuonTraService.GetAll();
            var model = new MuonTraViewModel
            {
                Id = muonTra.Id,
                NgayMuon = muonTra.NgayMuon,
                NgayCapNhat = muonTra.NgayCapNhat,
                NgayTra = muonTra.NgayKetThuc,
                UserId = muonTra.UserId,
                TinhTrang = muonTra.TinhTrang,
                TrangThai = muonTra.TrangThai,
                User = muonTra.User,
                SoLuong = muonTra.SoLuong,
                VanThu = muonTra.VanThu,
                ChiTietMuonTras = chiTietMuonTras,
                
            };
            
            return model;
        }

        private List<ChiTietMuonTraViewModel> GetChiTietMuonTras(IEnumerable<ChiTietMuonTra> chiTietMuonTra)
        {
            var model = new List<ChiTietMuonTraViewModel>();
            
            var autoList = AutoCompleteTextHoSos(_taiLieuVanBanService.Gets(p => p.TrangThai == true, p => p.OrderBy(x => x.Ten)).ToList());
            foreach (var item in chiTietMuonTra)
            {
                var muonTras = _muonTraService.GetHaveJoinUserAndCTMT(m => m.Id == item.MuonTraID);
                var taiLieuVBs = _taiLieuVanBanService.GetHaveJoinCTMT(m => m.Id == item.TaiLieuVanBanId);
                model.Add(new ChiTietMuonTraViewModel
                {
                    Id = item.Id,
                    MuonTra = muonTras,
                    MuonTraId = item.MuonTraID,
                    TaiLieuVanBan = taiLieuVBs,
                    TaiLieuVanBanId = item.TaiLieuVanBanId,
                    ViTri = autoList.First().ViTri,
                    TrangThai = item.TrangThai,

                }); 
            }
           return model;


        }

        private List<MuonTraViewModel> GetMuonTras(IList<MuonTra> muontras)
        {
            var chiTietMuonTras = _chiTietMuonTraService.GetAll();
            return muontras.Select(x => new MuonTraViewModel
            {
                Id = x.Id,
                NgayMuon = x.NgayMuon,
                NgayTra = x.NgayKetThuc,
                NgayCapNhat = x.NgayCapNhat,
                TinhTrang = x.TinhTrang,
                TrangThai = x.TrangThai,
                VanThu = x.VanThu,
                UserId = x.UserId,
                User = x.User,
                SoLuong = x.SoLuong,
                ChiTietMuonTras = chiTietMuonTras,

            }).ToList();
            
        }
    }
}