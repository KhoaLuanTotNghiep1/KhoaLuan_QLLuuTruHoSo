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
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace S3Train.WebHeThong.Controllers
{
    public class TraController : Controller
    {
        private readonly IMuonTraService _muonTraService;
        private readonly IChiTietMuonTraService _chiTietMuonTraService;
        private readonly ITaiLieuVanBanService _taiLieuVanBanService;
        private readonly IHoSoService _hoSoService;
        private readonly IUserService _userService;
        private readonly IFunctionLichSuHoatDongService _functionLichSuHoatDongService;


        public TraController()
        {

        }

        public TraController(IMuonTraService muonTraService, IChiTietMuonTraService chiTietMuonTraService, ITaiLieuVanBanService taiLieuVanBanService,
                            IHoSoService hoSoService, IUserService userService, IFunctionLichSuHoatDongService functionLichSuHoatDongService)
        {
            _muonTraService = muonTraService;
            _chiTietMuonTraService = chiTietMuonTraService;
            _taiLieuVanBanService = taiLieuVanBanService;
            _hoSoService = hoSoService;
            _userService = userService;
            _functionLichSuHoatDongService = functionLichSuHoatDongService;
        }
        // GET: MuonTra
        public ActionResult Index(int? pageIndex, int? pageSize, string searchString, string tinhTrang = GlobalConfigs.TINHTRANG_DATRA, bool active = true)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? GlobalConfigs.DEFAULT_PAGESIZE;

            var model = new MuonTraIndexViewModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var listMuonTra = _muonTraService.GetAllHaveJoinUser();

            listMuonTra = listMuonTra.Where(p => p.TinhTrang == EnumTinhTrang.DaTra);

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
            ViewBag.Controller = "Tra";
            ViewBag.TinhTrang = tinhTrang;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(string id)
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
        public ActionResult Create(List<ChiTietMuonTraViewModel> model)
        {
            var dem = model.Count(m => m.Checkbox == true);
            foreach (var item in model)
            {
                var vanBan = _taiLieuVanBanService.Get(p => p.Id == item.TaiLieuVanBanId);
                var muonTra = _muonTraService.Get(m => m.Id == item.MuonTra.Id);
                if (model.Count() == dem)
                {
                    vanBan.TinhTrang = EnumTinhTrang.TrongKho;
                    _taiLieuVanBanService.Update(vanBan);

                    muonTra.TinhTrang = EnumTinhTrang.DaTra;
                    _muonTraService.Update(muonTra);
                }
                else
                    if (item.Checkbox == true)
                {

                    var muontra = new MuonTra();

                    muontra.UserId = item.MuonTra.UserId;
                    muontra.VanThu = User.Identity.GetUserName();
                    muontra.NgayMuon = item.MuonTra.NgayMuon;
                    muontra.NgayKetThuc = DateTime.Now;
                    muontra.TinhTrang = EnumTinhTrang.DaTra;
                    muontra.SoLuong = dem;
                    _muonTraService.Insert(muontra);
                    int sl = 0;
                    muonTra = _muonTraService.Get(m => m.Id == item.MuonTra.Id);
                    sl = model.Count() - dem;
                    muonTra.SoLuong = sl;
                    _muonTraService.Update(muonTra);

                    var muon = _chiTietMuonTraService.Get(m => m.TaiLieuVanBanId == item.TaiLieuVanBanId);
                    muon.TrangThai = false;
                    _chiTietMuonTraService.Update(muon);

                    var chitietmuontra = new ChiTietMuonTra();
                    chitietmuontra.TaiLieuVanBanId = item.TaiLieuVanBanId;
                    chitietmuontra.MuonTraID = muontra.Id;
                    _chiTietMuonTraService.Insert(chitietmuontra);
                    vanBan.TinhTrang = EnumTinhTrang.TrongKho;
                    _taiLieuVanBanService.Update(vanBan);

                }
                _functionLichSuHoatDongService.Create(ActionWithObject.Create, User.Identity.GetUserId(), item.Id);
                TempData["AlertMessage"] = "Trả Văn Bản Thành Công";
            }
            if (dem == 0)
                TempData["AlertMessage"] = "Bạn Chưa Chọn TL/VB Để Trả";
            return RedirectToAction("Index");
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

            _functionLichSuHoatDongService.Create(ActionWithObject.Delete, User.Identity.GetUserId(), id);
            return RedirectToAction("Index");
        }


        public ActionResult Detail(string id)
        {
            var model = new List<ChiTietMuonTraViewModel>();
            var chiTietMuonTras = _chiTietMuonTraService.GetAll();
            var muonTra = GetMuonTra(_muonTraService.Get(m => m.Id == id));
            foreach (var item in chiTietMuonTras)
            {
                if (item.MuonTraID == id && item.TrangThai == true)
                {
                    var taiLieuVanBan = _taiLieuVanBanService.Get(m => m.Id == item.TaiLieuVanBanId);
                    model.Add(new ChiTietMuonTraViewModel
                    {
                        Id = item.Id,
                        MuonTra = muonTra,
                        User = muonTra.User,
                        MuonTraId = item.MuonTraID,
                        TaiLieuVanBan = taiLieuVanBan,
                        TaiLieuVanBanId = item.TaiLieuVanBanId,
                        SoLuong = 1,
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

        private MuonTraViewModel GetMuonTra(MuonTra muonTra)
        {
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