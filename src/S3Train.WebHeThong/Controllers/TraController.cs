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

        //[HttpPost]
        //public ActionResult Create(List<ChiTietMuonTraViewModel> model)
        //{
        //    var dem = model.Count(m => m.Checkbox == true);
        //    var a = new List<ChiTietMuonTraViewModel>();

        //    var muontra = new MuonTra();
        //    var chitietmuontra = new ChiTietMuonTra();

        //    if (dem == 0)
        //        TempData["AlertMessage"] = "Bạn Chưa Chọn TL/VB Để Trả";

        //    foreach (var muon in model)
        //    {
        //        if (muon.Id != null)
        //        {
        //            a.Add(new ChiTietMuonTraViewModel
        //            {
        //                Id = muon.Id
        //            });
        //        }
        //    }
        //    foreach (var item in model)
        //    {
        //        if (item.Id != null)
        //        {
        //            var vanBan = _taiLieuVanBanService.Get(p => p.Id == item.TaiLieuVanBanId);
        //            var muonTra = _muonTraService.Get(m => m.Id == item.MuonTra.Id);

        //            if (a.Count() == dem)
        //            {
        //                vanBan.TinhTrang = EnumTinhTrang.TrongKho;
        //                _taiLieuVanBanService.Update(vanBan);

        //                muonTra.TinhTrang = EnumTinhTrang.DaTra;
        //                _muonTraService.Update(muonTra);
        //            }
        //            else
        //            if (item.Checkbox == true)
        //            {
        //                int sl = 0;
        //                muonTra = _muonTraService.Get(m => m.Id == item.MuonTra.Id);
        //                sl = a.Count() - dem;
        //                muonTra.SoLuong = sl;
        //                _muonTraService.Update(muonTra);


        //                muontra.UserId = item.MuonTra.UserId;
        //                muontra.VanThu = User.Identity.GetUserName();
        //                muontra.NgayMuon = item.MuonTra.NgayMuon;
        //                muontra.NgayKetThuc = DateTime.Now;
        //                muontra.TinhTrang = EnumTinhTrang.DaTra;
        //                muontra.SoLuong = dem;


        //                var muon = _chiTietMuonTraService.Get(m => m.TaiLieuVanBanId == item.TaiLieuVanBanId);
        //                muon.TrangThai = false;
        //                _chiTietMuonTraService.Update(muon);



        //                chitietmuontra.TaiLieuVanBanId = item.TaiLieuVanBanId;
        //                _chiTietMuonTraService.Insert(chitietmuontra);
        //                vanBan.TinhTrang = EnumTinhTrang.TrongKho;
        //                _taiLieuVanBanService.Update(vanBan);
        //            }
        //        }
        //    }
        //    if(muontra.UserId != null)
        //    {
        //        _muonTraService.Insert(muontra);
        //        chitietmuontra.MuonTraID = muontra.Id;
        //        _chiTietMuonTraService.Update(chitietmuontra);

        //    }
        //    else
        //        TempData["AlertMessage"] = "Trả Văn Bản Thành Công";

        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public ActionResult Create(List<ChiTietMuonTraViewModel> model)
        {
            var a = new List<ChiTietMuonTraViewModel>();
            var dem = model.Count(m => m.Checkbox == true);
            if (dem == 0)
                TempData["AlertMessage"] = "Bạn Chưa Chọn TL/VB Để Trả";
            foreach (var muon in model)
            {
                if (muon.Id != null)
                {
                    a.Add(new ChiTietMuonTraViewModel
                    {
                        Id = muon.Id
                    });
                }
            }
            if (a.Count() == dem)
            {
                foreach (var item in model)
                {
                    if (item.Id == null)
                    {
                        var chitietmuontra = _chiTietMuonTraService.Get(m => m.TrangThai == false);
                        _chiTietMuonTraService.Remove(chitietmuontra);
                    }
                    UpdateVanBan(item.TaiLieuVanBanId);
                    UpdateMuonTra(item.MuonTraId);
                }
            }
            else
            {

                foreach (var tra in model)
                {
                    if (tra.Checkbox == true)
                    {
                        var muonTra = _muonTraService.Get(m => m.Id == tra.MuonTra.Id);
                        muonTra.SoLuong = a.Count() - dem;
                        _muonTraService.Update(muonTra);
                    }
                }
                GiveBackOneOrTwo(model);

            }
            TempData["AlertMessage"] = "Trả Văn Bản Thành Công";
            return RedirectToAction("Index");

        }


        public bool UpdateVanBan(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            var vb = _taiLieuVanBanService.GetById(id);

            if (vb != null)
            {
                vb.TinhTrang = EnumTinhTrang.TrongKho;
                _taiLieuVanBanService.Update(vb);

                return true;
            }
            else
                return false;

        }

        public bool UpdateMuonTra(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            var vb = _muonTraService.GetById(id);

            if (vb != null)
            {
                vb.TinhTrang = EnumTinhTrang.DaTra;
                _muonTraService.Update(vb);

                return true;
            }
            else
                return false;
        }

        public void GiveBackOneOrTwo(List<ChiTietMuonTraViewModel> chiTietMuons)
        {
            if (chiTietMuons.Count() <= 0)
                return;
            string muontraId = "";

            var muontraNew = new MuonTra
            {
                Id = Guid.NewGuid().ToString(),
                VanThu = User.Identity.GetUserName(),
            };

            var chiTietMuonTras = new List<ChiTietMuonTra>();

            foreach (var item in chiTietMuons)
            {
                var ctmt = _chiTietMuonTraService.GetById(item.Id);
                muontraId = item.MuonTraId;
                if (item.Checkbox)
                {
                    ctmt.TrangThai = false;
                    UpdateVanBan(ctmt.TaiLieuVanBanId);
                    var chtmtNew = new ChiTietMuonTra
                    {
                        Id = Guid.NewGuid().ToString(),
                        MuonTraID = muontraNew.Id,
                        TaiLieuVanBanId = item.TaiLieuVanBanId,
                        NgayTao = DateTime.Now,
                        TrangThai = true
                    };
                    chiTietMuonTras.Add(chtmtNew);
                }
            }

            var muonTraOld = _muonTraService.GetById(muontraId);
            _muonTraService.Update(muonTraOld);

            muontraNew.UserId = muonTraOld.UserId;
            muontraNew.NgayMuon = muonTraOld.NgayMuon;
            muontraNew.NgayKetThuc = muonTraOld.NgayKetThuc;
            muontraNew.SoLuong = chiTietMuons.Count(m => m.Checkbox == true);

            _muonTraService.Insert(muontraNew);
            _chiTietMuonTraService.Insert(chiTietMuonTras);

            UpdateMuonTra(muontraNew.Id);
        }

        public ActionResult Delete(string id)
        {
            var muontra = _muonTraService.GetHaveJoinUser(p => p.Id == id);
            var chiTietMuonTra = _chiTietMuonTraService.Gets(p => p.MuonTraID == id);
            foreach (var item in chiTietMuonTra)
            {
                var vanBan = _taiLieuVanBanService.Get(p => p.Id == item.TaiLieuVanBanId);

                vanBan.TinhTrang = EnumTinhTrang.TrongKho;

                _taiLieuVanBanService.Update(vanBan);
                _chiTietMuonTraService.Remove(item);
            }
            _functionLichSuHoatDongService.Create(ActionWithObject.Delete, User.Identity.GetUserId(), "phiếu mượn của người dùng " + muontra.User.UserName);
            _muonTraService.Remove(muontra);
            TempData["AlertMessage"] = "Xóa Thành Công";

            return RedirectToAction("Index");
        }


        public ActionResult Detail(string id)
        {
            var muonTra = _muonTraService.GetHaveJoinUserAndCTMT(m => m.Id == id);

            var model = GetMuonTra(muonTra);

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
                ChiTietMuonTras = muonTra.ChiTietMuonTras
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