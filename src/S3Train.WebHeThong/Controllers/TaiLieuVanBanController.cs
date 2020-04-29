using Microsoft.AspNet.Identity;
using S3Train.Contract;
using S3Train.Core.Constant;
using S3Train.Domain;
using S3Train.WebHeThong.CommomClientSide.DropDownList;
using S3Train.WebHeThong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AlgorithmLibrary.Kmeans;
using X.PagedList;

namespace S3Train.WebHeThong.Controllers
{
    public class TaiLieuVanBanController : Controller
    {
        private readonly ITaiLieuVanBanService _taiLieuVanBanService;
        private readonly INoiBanHanhService _noiBanHanhService;
        private readonly IHoSoService _hoSoService;
        private readonly ILoaiHoSoService _loaiHoSoService;

        public TaiLieuVanBanController()
        {

        }

        public TaiLieuVanBanController(ITaiLieuVanBanService taiLieuVanBanService, INoiBanHanhService noiBanHanhService, 
            IHoSoService hoSoService, ILoaiHoSoService loaiHoSoService)
        {
            _taiLieuVanBanService = taiLieuVanBanService;
            _noiBanHanhService = noiBanHanhService;
            _hoSoService = hoSoService;
            _loaiHoSoService = loaiHoSoService;
        }

        // GET: TaiLieuVanBan
        public ActionResult Index(int? pageIndex, int? pageSize, string dang)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? GlobalConfigs.DEFAULT_PAGESIZE;

            var model = new TaiLieuVanBanIndexViewModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var taiLieuVanBans = _taiLieuVanBanService.GetAllPaged(pageIndex, pageSize.Value, p => p.Dang == dang, p => p.OrderBy(c => c.Ten));

            ViewBag.Dang = dang;
            model.Paged = taiLieuVanBans;
            model.Items = GetTaiLieuVanBans(taiLieuVanBans.ToList());
            return View(model);
        }

        [HttpGet]
        public ActionResult CreateOrUpdate(string id, string dang)
        {
            var model = new TaiLieu_VanBanViewModel();

            ViewBag.LoaiHoSos = SelectListItemFromDomain.SelectListItem_LoaiHoSo(_loaiHoSoService.GetAll(m => m.OrderBy(t => t.Ten)));
            ViewBag.HoSos = SelectListItemFromDomain.SelectListItem_HoSo(_hoSoService.GetAll(m => m.OrderBy(t => t.PhongLuuTru)));
            ViewBag.NoiBanHanhs = SelectListItemFromDomain.SelectListItem_NoiBanHanh(_noiBanHanhService.GetAll(m => m.OrderBy(t => t.Ten)));

            if (string.IsNullOrEmpty(id))
            {
                model.Dang = dang;
                return View(model);
            }
            else
            {
                var taiLieuVanBan = _taiLieuVanBanService.Get(m => m.Id == id);
                model = GetTaiLieuVanBan(taiLieuVanBan);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult CreateOrUpdate(TaiLieu_VanBanViewModel model)
        {
            var taiLieuVanBan = string.IsNullOrEmpty(model.Id) ? new TaiLieuVanBan { NgayCapNhat = DateTime.Now }
                : _taiLieuVanBanService.Get(m => m.Id == model.Id);

            taiLieuVanBan.Dang = model.Dang;
            taiLieuVanBan.DuongDan = model.DuongDan;
            taiLieuVanBan.GhiChu = model.GhiChu;
            taiLieuVanBan.HoSoId = model.HoSoId;
            taiLieuVanBan.Loai = model.Loai;
            taiLieuVanBan.NguoiDuyet = model.NguoiDuyet;
            taiLieuVanBan.NoiDung = model.NoiDung;
            taiLieuVanBan.NguoiGuiHoacNhan = model.NguoiGuiHoacNhan;
            taiLieuVanBan.NguoiKy = model.NguoiKy;
            taiLieuVanBan.NoiBanHanhId = model.NoiBanHanhId;
            taiLieuVanBan.NoiNhan = model.NoiNhan;
            taiLieuVanBan.SoKyHieu = model.SoKyHieu;
            taiLieuVanBan.SoTo = model.SoTo;
            taiLieuVanBan.Ten = model.Ten;
            taiLieuVanBan.TrichYeu = model.TrichYeu;
            taiLieuVanBan.NgayBanHanh = model.NgayBanHanh;
            taiLieuVanBan.UserId = User.Identity.GetUserId();

            if(model.Dang == "Đi")
            {
                taiLieuVanBan.TinhTrang = "Đã Gởi";
                taiLieuVanBan.TrangThai = false;
            }
            else
            {
                taiLieuVanBan.TinhTrang = "Trong Kho";
                taiLieuVanBan.TrangThai = true;
            }

            if (string.IsNullOrEmpty(model.Id))
            {
                taiLieuVanBan.Id = Guid.NewGuid().ToString();
                taiLieuVanBan.NgayTao = DateTime.Now;
                _taiLieuVanBanService.Insert(taiLieuVanBan);
                TempData["AlertMessage"] = "Tạo Mới Thành Công";
            }
            else
            {
                taiLieuVanBan.NgayCapNhat = DateTime.Now;
                _taiLieuVanBanService.Update(taiLieuVanBan);
                TempData["AlertMessage"] = "Cập Nhật Thành Công";
            }
            return RedirectToAction("Index", new { dang = model.Dang});
        }

        public ActionResult Delete(string id)
        {
            var taiLieuVanBan = _taiLieuVanBanService.Get(m => m.Id == id);
            _taiLieuVanBanService.Remove(taiLieuVanBan);
            TempData["AlertMessage"] = "Xóa Thành Công";
            return RedirectToAction("Index", new { dang = taiLieuVanBan.Dang});
        }

        public ActionResult Detail(string id)
        {
            var model = GetTaiLieuVanBan(_taiLieuVanBanService.Get(m => m.Id == id));

            return View(model);
        }

        public ActionResult DemoKmeans()
        {
            var list = _taiLieuVanBanService.GetDocuments();

            var docCollection = new DocumentCollection()
            {
                DocumentList = list
            };
            
            List<DocumentVector> vSpace = VectorSpaceModel.ProcessDocumentCollection(docCollection);
            int totalIteration = 0;
            List<Centroid> resultSet = DocumnetClustering.PrepareDocumentCluster(3, vSpace, ref totalIteration);
            
            return View(resultSet);
        }

        private TaiLieu_VanBanViewModel GetTaiLieuVanBan(TaiLieuVanBan x)
        {
            var model = new TaiLieu_VanBanViewModel
            {
               Id = x.Id,
               Dang = x.Dang,
               DuongDan = x.DuongDan,
               GhiChu = x.GhiChu,
               HoSo = x.HoSo,
               HoSoId = x.HoSoId,
               Loai = x.Loai,
               NgayCapNhat =  x.NgayCapNhat,
               NgayTao = x.NgayTao,
               NguoiDuyet = x.NguoiDuyet,
               NoiDung = x.NoiDung,
               NguoiGuiHoacNhan = x.NguoiGuiHoacNhan,
               NguoiKy = x.NguoiKy,
               NoiBanHanh = x.NoiBanHanh,
               NoiBanHanhId = x.NoiBanHanhId,
               NoiNhan = x.NoiNhan,
               SoKyHieu = x.SoKyHieu,
               SoTo = x.SoTo,
               Ten = x.Ten,
               TinhTrang = x.TinhTrang,
               TrangThai = x.TrangThai,
               TrichYeu = x.TrichYeu,
               User = x.User,
               UserId = x.UserId,
               NgayBanHanh = x.NgayBanHanh
            };
            return model;
        }

        private List<TaiLieu_VanBanViewModel> GetTaiLieuVanBans(IList<TaiLieuVanBan> taiLieuVanBans)
        {
            return taiLieuVanBans.Select(x => new TaiLieu_VanBanViewModel
            {
                Id = x.Id,
                Dang = x.Dang,
                DuongDan = x.DuongDan,
                GhiChu = x.GhiChu,
                HoSo = x.HoSo,
                HoSoId = x.HoSoId,
                Loai = x.Loai,
                NgayCapNhat = x.NgayCapNhat,
                NgayTao = x.NgayTao,
                NguoiDuyet = x.NguoiDuyet,
                NoiDung = x.NoiDung,
                NguoiGuiHoacNhan = x.NguoiGuiHoacNhan,
                NguoiKy = x.NguoiKy,
                NoiBanHanh = x.NoiBanHanh,
                NoiBanHanhId = x.NoiBanHanhId,
                NoiNhan = x.NoiNhan,
                SoKyHieu = x.SoKyHieu,
                SoTo = x.SoTo,
                Ten = x.Ten,
                TinhTrang = x.TinhTrang,
                TrangThai = x.TrangThai,
                TrichYeu = x.TrichYeu,
                User = x.User,
                UserId = x.UserId,
                NgayBanHanh = x.NgayBanHanh
            }).ToList();
        }
    }
}