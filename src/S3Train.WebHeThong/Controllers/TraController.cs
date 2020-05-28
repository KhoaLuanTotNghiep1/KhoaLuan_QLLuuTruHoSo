using Microsoft.AspNet.Identity;
using S3Train.Contract;
using S3Train.Core.Constant;
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
    public class TraController : Controller
    {
        private readonly IMuonTraService _muonTraService;
        private readonly IChiTietMuonTraService _chiTietMuonTraService;
        private readonly ITaiLieuVanBanService _taiLieuVanBanService;
        private readonly IHoSoService _hoSoService;
        private readonly IUserService _userService;

        public const string MuonSession = "MuonSession";

        public TraController()
        {

        }

        public TraController(IMuonTraService muonTraService, IChiTietMuonTraService chiTietMuonTraService, ITaiLieuVanBanService taiLieuVanBanService, IHoSoService hoSoService, IUserService userService)
        {
            _muonTraService = muonTraService;
            _chiTietMuonTraService = chiTietMuonTraService;
            _taiLieuVanBanService = taiLieuVanBanService;
            _hoSoService = hoSoService;
            _userService = userService;
        }
        // GET: MuonTra
        public ActionResult Index(int? pageIndex, int? pageSize, string searchString, string tinhTrang = GlobalConfigs.TINHTRANG_DATRA, bool active = true)
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
            var muontras = _muonTraService.GetAllPaged(pageIndex, pageSize.Value, p => p.TrangThai == active
            && p.TinhTrang == tinhTrang, p => p.OrderBy(c => c.NgayMuon));
            if (!string.IsNullOrEmpty(searchString))
            {
                muontras = _muonTraService.GetAllPaged(pageIndex, pageSize.Value, p => p.User.FullName.Contains(searchString) || p.VanThu.Contains(searchString)
                    && p.TrangThai == active && p.TinhTrang == tinhTrang, p => p.OrderBy(c => c.NgayMuon), includes);
            }
            model.Paged = muontras;
            model.Items = GetMuonTras(muontras.ToList());


            ViewBag.Active = active;
            ViewBag.searchString = searchString;
            ViewBag.Controller = "MuonTra";
            ViewBag.TinhTrang = tinhTrang;
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

        public ActionResult AddItems(string Id)
        {
                var MuonItem = _chiTietMuonTraService.Get(m => m.TaiLieuVanBanId == Id);
                var currentMuonItems = (List<MuonTraViewModel>)Session[MuonSession] ?? new List<MuonTraViewModel>();
                if (currentMuonItems.Exists(x => x.ChiTietMuonTra.TaiLieuVanBanId == Id))
                {
                    TempData["AlertMessage"] = "Đã chọn trả";
                }
                else
                {
                    currentMuonItems.Add(new MuonTraViewModel
                    {
                        ChiTietMuonTra = new ChiTietMuonTraViewModel
                        {
                            Id = Id,
                            SoLuong = MuonItem.SoLuong,
                            User = MuonItem.MuonTra.User,
                            TaiLieuVanBan = MuonItem.TaiLieuVanBan,
                            TaiLieuVanBanId = MuonItem.TaiLieuVanBanId,
                            MuonTra = MuonItem.MuonTra,
                        },

                    });
                }
                Session[MuonSession] = currentMuonItems;
            return Json(new
            {
                status = "success",
            });
        }

        [HttpPost]
        public ActionResult CreateOrUpdate()
        {
            
            var vanThu = User.Identity.GetUserId();
            var muonSession = (List<MuonTraViewModel>)Session[MuonSession];
            var model = new List<ChiTietMuonTraViewModel>();
            var ctMuon = _chiTietMuonTraService.GetAll();
            foreach(var item in ctMuon)
            {
                if(item.MuonTraID == muonSession.First().ChiTietMuonTra.MuonTra.Id && item.TrangThai == true)
                {
                   model.Add(new ChiTietMuonTraViewModel
                    {
                        Id = item.Id,
                        SoLuong = item.SoLuong,
                        MuonTraId = item.MuonTraID,
                        TaiLieuVanBan = item.TaiLieuVanBan,
                        TaiLieuVanBanId = item.TaiLieuVanBanId,
                        MuonTra = item.MuonTra,
                        User = item.MuonTra.User,
                    });
                }
            }

            if (muonSession.Count() == model.Count())
            {
                foreach(var item in muonSession)
                {
                    var vanBan = _taiLieuVanBanService.Get(m => m.Id == item.ChiTietMuonTra.TaiLieuVanBanId);
                    vanBan.TinhTrang = "Trong Kho";
                    _taiLieuVanBanService.Update(vanBan);
                    var muonTra = _muonTraService.Get(m => m.Id == item.ChiTietMuonTra.MuonTra.Id);
                    muonTra.TinhTrang = "Đã Trả";
                    _muonTraService.Update(muonTra);
                   
                }
               
            }
            else
            {
                var tra = new MuonTra
                {
                    VanThu = vanThu,
                    NgayMuon = muonSession.First().ChiTietMuonTra.MuonTra.NgayMuon,
                    NgayKetThuc = DateTime.Now,
                    UserId = muonSession.First().ChiTietMuonTra.MuonTra.UserId,
                    TinhTrang = "Đã Trả",
                };
                _muonTraService.Insert(tra);

                foreach (var item in muonSession)
                {
                    var muon = _chiTietMuonTraService.Get(m => m.TaiLieuVanBanId == item.ChiTietMuonTra.TaiLieuVanBanId);
                    muon.TrangThai = false;
                  
                    _chiTietMuonTraService.Update(muon);
                    var chiTietTra = new ChiTietMuonTra
                    {
                        MuonTraID = tra.Id,
                        SoLuong = item.ChiTietMuonTra.SoLuong,
                        TaiLieuVanBanId = item.ChiTietMuonTra.TaiLieuVanBanId,
                    };
                    _chiTietMuonTraService.Insert(chiTietTra);

                }
            }
            
            Session[MuonSession] = null;
            
            return Json(new
            {
                status = "success",
            });
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
                            vb.TinhTrang = "Trong Kho";
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
        

        public ActionResult Detail(string id)
        {
            var model = new List<ChiTietMuonTraViewModel>();
            var chiTietMuonTras = _chiTietMuonTraService.GetAll();
            //var chiTietMuonTra = _chiTietMuonTraService.Get(m => m.MuonTraID == id);
            //var MuonTra = _muonTraService.Get(m => m.Id == id);
            //var MuonTras = _muonTraService.GetAll();
            //var users = await _userService.GetAllAsync();
            //var TLVB = _taiLieuVanBanService.Get(m => m.Id == chiTietMuonTra.TaiLieuVanBanId);
            foreach (var item in chiTietMuonTras)
            {
                if (item.MuonTraID == id)
                {
                    model.Add(new ChiTietMuonTraViewModel
                    {
                        Id = item.Id,
                        SoLuong = item.SoLuong,
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
                TinhTrang = x.TinhTrang,
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
                SoLuong = x.SoLuong,
                MuonTraId = x.MuonTraID,
                TaiLieuVanBan = x.TaiLieuVanBan,
                TaiLieuVanBanId = x.TaiLieuVanBanId,
                MuonTra = x.MuonTra,
                User = x.MuonTra.User,
            }).ToList();
        }

    }
}