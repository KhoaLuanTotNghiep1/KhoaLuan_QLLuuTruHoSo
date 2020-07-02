using Microsoft.Ajax.Utilities;
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
        public ActionResult Index(int? pageIndex, int? pageSize, string searchString, bool active = true)
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
            ViewBag.TinhTrang = EnumTinhTrang.DaTra.GetDecription();
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
            if (dem == 0)
                TempData["AlertMessage"] = "Bạn Chưa Chọn TL/VB Để Trả";

            if (model.Count() == dem)
            {
                foreach (var item in model)
                {
                    UpdateVanBan(item.TaiLieuVanBanId);
                    UpdateMuonTra(item.MuonTraId);
                }
            }
            else
            {
                GiveBackOneOrTwo(model);
            }

            return RedirectToAction("Index");
        
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
            string chiTietHoatDong = "phiếu mượn của tài khoản " + model.User.UserName+ " thành " + active;

            model.TrangThai = active;

            _muonTraService.Update(model);

            _functionLichSuHoatDongService.Create(ActionWithObject.ChangeStatus, User.Identity.GetUserId(), chiTietHoatDong);

            var messenge = Messenger.ChangeActiveMessenge(active);

            TempData["AlertMessage"] = messenge;
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
                VanThu = User.Identity.GetUserName()
            };

            var chiTietMuonTras = new List<ChiTietMuonTra>();

            foreach (var item in chiTietMuons)
            {
                var ctmt = _chiTietMuonTraService.GetById(item.Id);
                if (item.Checkbox)
                {
                    ctmt.TrangThai = false;
                    UpdateVanBan(ctmt.TaiLieuVanBanId);
                }
                else
                {
                    var chtmtNew = new ChiTietMuonTra
                    {
                        Id = Guid.NewGuid().ToString(),
                        MuonTraID = muontraNew.Id,
                        TaiLieuVanBanId = item.TaiLieuVanBanId,
                        NgayTao = DateTime.Now,
                        TrangThai = true                    
                    };
                    chiTietMuonTras.Add(ctmt);
                }
                muontraId = item.MuonTraId;
            }

            var muonTraOld = _muonTraService.GetById(muontraId);

            muontraNew.UserId = muonTraOld.UserId;
            muontraNew.NgayMuon = muonTraOld.NgayMuon;
            muontraNew.NgayKetThuc = muonTraOld.NgayKetThuc;
            muontraNew.SoLuong = chiTietMuonTras.Count();

            _muonTraService.Insert(muontraNew);
            _chiTietMuonTraService.Insert(chiTietMuonTras);

            UpdateMuonTra(muontraId);
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